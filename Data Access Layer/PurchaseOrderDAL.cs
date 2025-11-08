using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POModel = JRSApplication.Components.Models.PurchaseOrder;
using JRSApplication.Components.Models;

namespace JRSApplication.Data_Access_Layer
{

    public class PurchaseOrderDAL
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        public int InsertFullPurchaseOrder(POModel order)
        {
            int newOrderId = -1;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                using (MySqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // ✅ STEP 1: Insert purchaseorder
                        string insertOrderQuery = @"
                        INSERT INTO purchaseorder
                        (order_number, order_detail, order_date, order_status, order_duedate, order_remark, emp_id, pro_id)
                        VALUES
                        (@OrderNumber, @OrderDetail, @OrderDate, @OrderStatus, @OrderDueDate, @OrderRemark, @EmpId, @ProId);
                        SELECT LAST_INSERT_ID();";

                        using (MySqlCommand cmd = new MySqlCommand(insertOrderQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@OrderNumber", order.OrderNumber);
                            cmd.Parameters.AddWithValue("@OrderDetail", order.OrderDetail);
                            cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                            cmd.Parameters.AddWithValue("@OrderStatus", order.OrderStatus);
                            cmd.Parameters.AddWithValue("@OrderDueDate", order.OrderDueDate);
                            cmd.Parameters.AddWithValue("@OrderRemark", DBNull.Value);
                            cmd.Parameters.AddWithValue("@EmpId", order.EmpId);
                            cmd.Parameters.AddWithValue("@ProId", order.ProId);

                            object result = cmd.ExecuteScalar();
                            if (result != null && int.TryParse(result.ToString(), out int id))
                            {
                                newOrderId = id;
                            }
                            else
                            {
                                throw new Exception("ไม่สามารถสร้างใบสั่งซื้อได้");
                            }
                        }

                        // ✅ STEP 2: Insert material_detail
                        int lineNo = 1;
                        foreach (var mat in order.MaterialDetails)
                        {
                            string insertMatQuery = @"
                            INSERT INTO material_detail
                            (order_id, mat_line_no, mat_no, mat_detail, mat_amount, mat_price, mat_quantity, mat_unit)
                            VALUES
                            (@OrderId, @MatLineNo, @MatNo, @MatDetail, @MatAmount, @MatPrice, @MatQuantity, @MatUnit);";

                            using (MySqlCommand matCmd = new MySqlCommand(insertMatQuery, conn, transaction))
                            {
                                matCmd.Parameters.AddWithValue("@OrderId", newOrderId);
                                matCmd.Parameters.AddWithValue("@MatLineNo", lineNo++);
                                matCmd.Parameters.AddWithValue("@MatNo", mat.MatNo);
                                matCmd.Parameters.AddWithValue("@MatDetail", mat.MatDetail);
                                matCmd.Parameters.AddWithValue("@MatAmount", mat.MatAmount);
                                matCmd.Parameters.AddWithValue("@MatPrice", mat.MatPrice);
                                matCmd.Parameters.AddWithValue("@MatQuantity", mat.MatQuantity);
                                matCmd.Parameters.AddWithValue("@MatUnit", mat.MatUnit);

                                matCmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit(); // ✅ สำเร็จทั้งหมด
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback(); // ❌ ยกเลิกทั้งหมด
                        throw new Exception("เกิดข้อผิดพลาดในการบันทึก: " + ex.Message);
                    }
                }
            }

            return newOrderId;
        }

        public List<PurchaseOrder> GetAllPurchaseOrders()
        {
            var orders = new List<PurchaseOrder>();
            string query = @"
                SELECT 
                    po.order_id, po.order_number, po.order_detail, po.order_date,
                    po.order_status, po.order_duedate, po.order_remark,
                    po.emp_id, po.approved_by_emp_id, po.approved_date, po.pro_id,
                    e1.emp_name AS created_by_name,
                    e2.emp_name AS approved_by_name
                FROM purchaseorder po
                LEFT JOIN employee e1 ON po.emp_id = e1.emp_id
                LEFT JOIN employee e2 ON po.approved_by_emp_id = e2.emp_id
                ORDER BY po.order_date DESC";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var po = new PurchaseOrder
                        {
                            OrderId = reader.GetInt32("order_id"),
                            OrderNumber = reader.GetString("order_number"),
                            ProId = reader.GetInt32("pro_id"),
                            OrderDetail = reader["order_detail"] == DBNull.Value ? null : reader.GetString("order_detail"),
                            OrderDate = reader.GetDateTime("order_date"),
                            OrderStatus = reader["order_status"] == DBNull.Value ? null : reader.GetString("order_status"),
                            OrderDueDate = reader.GetDateTime("order_duedate"),
                            OrderRemark = reader["order_remark"] == DBNull.Value ? null : reader.GetString("order_remark"),
                            EmpId = reader.GetString("emp_id"),
                            ApprovedByEmpId = reader["approved_by_emp_id"] == DBNull.Value ? null : reader.GetString("approved_by_emp_id"),
                            ApprovedDate = reader["approved_date"] == DBNull.Value ? (DateTime?)null : reader.GetDateTime("approved_date"),

                            // ✅ ชื่อที่ JOIN มา
                            EmpName = reader["created_by_name"] == DBNull.Value ? null : reader.GetString("created_by_name"),
                            ApprovedByName = reader["approved_by_name"] == DBNull.Value ? null : reader.GetString("approved_by_name"),
                        };

                        orders.Add(po);
                    }
                }
            }

            return orders;
        }

        public void UpdateOrderStatus(int orderId, string status, string remark, string empId)
        {
            // ✅ 1. ตรวจสอบค่าที่จะอัปเดตให้ตรง ENUM ที่ฐานข้อมูล
            string[] validStatuses = { "draft", "submitted", "approved", "rejected", "canceled" };

            if (string.IsNullOrWhiteSpace(status))
                throw new ArgumentException("Order status must not be empty.");

            // แปลงให้เป็นตัวพิมพ์เล็กเสมอเพื่อความสอดคล้องกับ ENUM
            status = status.Trim().ToLower();

            if (!validStatuses.Contains(status))
            {
                // ❌ ถ้าค่าไม่ตรง ENUM ให้ throw exception พร้อมข้อความชัดเจน
                string validList = string.Join(", ", validStatuses);
                throw new ArgumentException(
                    $"Invalid order status '{status}'. Allowed values are: {validList}");
            }

            // ✅ 2. สร้างคำสั่ง SQL ปลอดภัย
            string query = @"
        UPDATE purchaseorder
        SET 
            order_status = @status,
            order_remark = @remark,
            approved_by_emp_id = @empId,
            approved_date = NOW()
        WHERE order_id = @orderId;
    ";

            // ✅ 3. ใช้ using เพื่อจัดการ connection อย่างปลอดภัย
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@remark", remark ?? "");
                cmd.Parameters.AddWithValue("@empId", empId);
                cmd.Parameters.AddWithValue("@orderId", orderId);

                try
                {
                    conn.Open();
                    int affected = cmd.ExecuteNonQuery();

                    // ✅ ตรวจสอบว่ามีการอัปเดตจริง
                    if (affected == 0)
                    {
                        throw new Exception($"No purchase order found with ID = {orderId}");
                    }

                    // (ทางเลือก) Log ไว้สำหรับ debug
                    Console.WriteLine($"✅ [LOG] Order #{orderId} updated to status '{status}' by employee '{empId}'");
                }
                catch (MySqlException ex)
                {
                    // ❌ ดักเฉพาะ error จาก MySQL เพื่อ debug ง่ายขึ้น
                    Console.WriteLine($"❌ MySQL Error: {ex.Message}");
                    throw new Exception("เกิดข้อผิดพลาดขณะอัปเดตสถานะใบสั่งซื้อ: " + ex.Message);
                }
                catch (Exception ex)
                {
                    // ❌ ดักข้อผิดพลาดทั่วไป
                    Console.WriteLine($"❌ Error: {ex.Message}");
                    throw;
                }
            }
        }


        public PurchaseOrder GetPurchaseOrderById(int orderId)
        {
            PurchaseOrder order = null;

            string sql = @"
                SELECT 
                    po.order_id, po.order_number, po.order_detail, po.order_date,
                    po.order_status, po.order_duedate, po.order_remark,
                    po.emp_id, po.approved_by_emp_id, po.approved_date, po.pro_id,
                    e1.emp_name AS created_by_name,
                    e2.emp_name AS approved_by_name
                FROM purchaseorder po
                LEFT JOIN employee e1 ON po.emp_id = e1.emp_id
                LEFT JOIN employee e2 ON po.approved_by_emp_id = e2.emp_id
                WHERE po.order_id = @OrderId";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@OrderId", orderId);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        order = new PurchaseOrder
                        {
                            OrderId = reader.GetInt32("order_id"),
                            OrderNumber = reader.GetString("order_number"),
                            OrderDetail = reader["order_detail"] == DBNull.Value ? null : reader.GetString("order_detail"),
                            OrderDate = reader["order_date"] == DBNull.Value ? DateTime.MinValue : reader.GetDateTime("order_date"),
                            OrderStatus = reader["order_status"] == DBNull.Value ? null : reader.GetString("order_status"),
                            OrderDueDate = reader["order_duedate"] == DBNull.Value ? DateTime.MinValue : reader.GetDateTime("order_duedate"),
                            OrderRemark = reader["order_remark"] == DBNull.Value ? null : reader.GetString("order_remark"),
                            EmpId = reader["emp_id"] == DBNull.Value ? null : reader.GetString("emp_id"),
                            ApprovedByEmpId = reader["approved_by_emp_id"] == DBNull.Value ? null : reader.GetString("approved_by_emp_id"),
                            ApprovedDate = reader["approved_date"] == DBNull.Value ? (DateTime?)null : reader.GetDateTime("approved_date"),
                            ProId = reader["pro_id"] == DBNull.Value ? 0 : reader.GetInt32("pro_id"),

                            // ✅ ชื่อที่ JOIN มา
                            EmpName = reader["created_by_name"] == DBNull.Value ? null : reader.GetString("created_by_name"),
                            ApprovedByName = reader["approved_by_name"] == DBNull.Value ? null : reader.GetString("approved_by_name"),
                        };
                    }
                }
            }
            return order;
        }

        // ดึงรายการวัสดุตาม order_id
        public List<MaterialDetail> GetMaterialDetailsByOrderId(int orderId)
        {
            var items = new List<MaterialDetail>();

            string sql = @"
            SELECT 
                d.mat_line_no,
                d.mat_no,
                d.mat_detail,
                d.mat_quantity,
                d.mat_price,
                d.mat_unit,
                d.mat_amount
            FROM material_detail d
            WHERE d.order_id = @OrderId
            ORDER BY d.mat_line_no ASC;";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@OrderId", orderId);
                conn.Open();

                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        decimal qty = rdr["mat_quantity"] == DBNull.Value ? 0m : Convert.ToDecimal(rdr["mat_quantity"]);
                        decimal price = rdr["mat_price"] == DBNull.Value ? 0m : Convert.ToDecimal(rdr["mat_price"]);
                        decimal? amt = rdr["mat_amount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rdr["mat_amount"]);

                        items.Add(new MaterialDetail
                        {
                            MatNo = rdr["mat_no"] == DBNull.Value ? 0 : Convert.ToInt32(rdr["mat_no"]),
                            MatDetail = rdr["mat_detail"] == DBNull.Value ? "" : rdr["mat_detail"].ToString(),
                            MatQuantity = qty,
                            MatPrice = price,
                            MatUnit = rdr["mat_unit"] == DBNull.Value ? "" : rdr["mat_unit"].ToString(),
                            MatAmount = amt ?? (qty * price)
                        });
                    }
                }
            }

            return items;
        }



    }
}
