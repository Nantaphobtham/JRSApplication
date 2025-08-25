﻿using JRSApplication.Components;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace JRSApplication.Data_Access_Layer
{
    public class SupplierDAL
    {
        private readonly string connectionString;

        public SupplierDAL()
        {
            connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        }

        // ---------- DTO ช่วยบอกว่าซ้ำฟิลด์ไหน ----------
        private class DuplicateResult
        {
            public bool Email { get; set; }
            public bool Phone { get; set; }
            public bool Juristic { get; set; }
            public bool Name { get; set; }
            public bool HasAny => Email || Phone || Juristic || Name;

            public string ToUserMessage()
            {
                if (Email) return "อีเมลนี้มีอยู่แล้วในระบบ";
                if (Phone) return "เบอร์โทรนี้มีอยู่แล้วในระบบ";
                if (Juristic) return "เลขนิติบุคคล/ภาษีนี้มีอยู่แล้วในระบบ";
                if (Name) return "ชื่อบริษัทนี้มีอยู่แล้วในระบบ";
                return "ข้อมูลซ้ำในระบบ";
            }
        }

        // ---------- ดึงข้อมูลทั้งหมด ----------
        public DataTable GetAllSuppliers()
        {
            DataTable dt = new DataTable();
            using (var conn = new MySqlConnection(connectionString))
            {
                try
                {
                    string sql = @"SELECT 
                                     sup_id      AS 'รหัสซัพพลายเออร์',
                                     sup_name    AS 'ชื่อบริษัท',
                                     sup_juristic AS 'นิติบุคคล',
                                     sup_tel     AS 'เบอร์โทรศัพท์',
                                     sup_email   AS 'อีเมล',
                                     sup_address AS 'ที่อยู่'
                                   FROM supplier
                                   ORDER BY sup_name";
                    using (var adapter = new MySqlDataAdapter(sql, conn))
                    {
                        conn.Open();
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("เกิดข้อผิดพลาดในการดึงข้อมูล: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return dt;
        }

        // ---------- Generate ID ----------
        public string GenerateSupplierID()
        {
            string year = DateTime.Now.Year.ToString().Substring(2);
            string month = DateTime.Now.Month.ToString("D2");
            string newID;
            Random rnd = new Random();

            do
            {
                string randoms = rnd.Next(1000, 9999).ToString();
                newID = year + month + randoms;
            } while (CheckDuplicateSupplierID(newID));

            return newID;
        }

        public bool CheckDuplicateSupplierID(string supplierID)
        {
            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM supplier WHERE sup_id = @id", conn))
            {
                cmd.Parameters.AddWithValue("@id", supplierID);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        // ---------- Exists รายฟิลด์ (ใช้ได้ถ้าต้องการเช็คจุดเดียว) ----------
        public bool ExistsEmail(string email, string excludeId = null)
            => ExistsByColumn("sup_email", email, excludeId);

        public bool ExistsPhone(string phone, string excludeId = null)
            => ExistsByColumn("sup_tel", phone, excludeId);

        public bool ExistsJuristic(string juristic, string excludeId = null)
            => ExistsByColumn("sup_juristic", juristic, excludeId);

        public bool ExistsSupplierName(string name, string excludeId = null)
            => ExistsByColumn("sup_name", name, excludeId);

        private bool ExistsByColumn(string col, string value, string excludeId)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            string sql = $@"SELECT 1 FROM supplier 
                            WHERE {col} = @val 
                              AND (@id IS NULL OR sup_id <> @id)
                            LIMIT 1;";
            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@val", value);
                cmd.Parameters.AddWithValue("@id", string.IsNullOrEmpty(excludeId) ? (object)DBNull.Value : excludeId);
                conn.Open();
                using (var r = cmd.ExecuteReader()) return r.Read();
            }
        }

        // ---------- เช็คซ้ำแบบสรุปรวม (ของเดิม) ----------
        public bool CheckDuplicateSupplier(string email, string phone, string juristic, string name, string supplierID)
        {
            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;

                string query = @"SELECT COUNT(*) FROM supplier 
                                 WHERE (sup_email = @Email OR sup_tel = @Phone OR sup_juristic = @Juristic OR sup_name = @Name)";
                if (!string.IsNullOrEmpty(supplierID))
                    query += " AND sup_id <> @SupplierID";

                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Phone", phone);
                cmd.Parameters.AddWithValue("@Juristic", juristic);
                cmd.Parameters.AddWithValue("@Name", name);
                if (!string.IsNullOrEmpty(supplierID))
                    cmd.Parameters.AddWithValue("@SupplierID", supplierID);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        // ---------- เช็คซ้ำแบบระบุช่อง ----------
        private DuplicateResult CheckDuplicateSupplierDetailed(string email, string phone, string juristic, string name, string excludeId)
        {
            var res = new DuplicateResult
            {
                Email = ExistsEmail(email, excludeId),
                Phone = ExistsPhone(phone, excludeId),
                Juristic = ExistsJuristic(juristic, excludeId),
                Name = ExistsSupplierName(name, excludeId)
            };
            return res;
        }

        // ---------- Insert ----------
        public bool InsertSupplier(Supplier sup)
        {
            // เช็คซ้ำแบบระบุช่องก่อน
            var dup = CheckDuplicateSupplierDetailed(sup.Email, sup.Phone, sup.Juristic, sup.Name, null);
            if (dup.HasAny)
            {
                MessageBox.Show(dup.ToUserMessage(), "ข้อมูลซ้ำ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand())
            {
                string supplierID = GenerateSupplierID();
                string sql = @"INSERT INTO supplier (sup_id, sup_name, sup_juristic, sup_tel, sup_address, sup_email)
                               VALUES (@SupplierID, @Name, @Juristic, @Phone, @Address, @Email)";
                cmd.Connection = conn;
                cmd.CommandText = sql;

                cmd.Parameters.AddWithValue("@SupplierID", supplierID);
                cmd.Parameters.AddWithValue("@Name", sup.Name);
                cmd.Parameters.AddWithValue("@Juristic", sup.Juristic);
                cmd.Parameters.AddWithValue("@Phone", sup.Phone);
                cmd.Parameters.AddWithValue("@Address", sup.Address);
                cmd.Parameters.AddWithValue("@Email", sup.Email);

                try
                {
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
                catch (MySqlException ex) when (ex.Number == 1062) // Duplicate entry
                {
                    string msg = MapDuplicateKeyMessage(ex);
                    MessageBox.Show(msg, "ข้อมูลซ้ำ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("เกิดข้อผิดพลาดในการบันทึกข้อมูล: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        // ---------- Update ----------
        public bool UpdateSupplier(string supplierID, string name, string juristic, string phone, string email, string address)
        {
            // เช็คซ้ำแบบระบุช่องก่อน (ยกเว้นตัวเอง)
            var dup = CheckDuplicateSupplierDetailed(email, phone, juristic, name, supplierID);
            if (dup.HasAny)
            {
                MessageBox.Show(dup.ToUserMessage(), "ข้อมูลซ้ำ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand())
            {
                string sql = @"UPDATE supplier 
                               SET sup_name = @Name, 
                                   sup_juristic = @Juristic,
                                   sup_tel = @Phone, 
                                   sup_email = @Email, 
                                   sup_address = @Address
                               WHERE sup_id = @SupplierID";
                cmd.Connection = conn;
                cmd.CommandText = sql;

                cmd.Parameters.AddWithValue("@SupplierID", supplierID);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Juristic", juristic);
                cmd.Parameters.AddWithValue("@Phone", phone);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Address", address);

                try
                {
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
                catch (MySqlException ex) when (ex.Number == 1062) // Duplicate entry
                {
                    string msg = MapDuplicateKeyMessage(ex);
                    MessageBox.Show(msg, "ข้อมูลซ้ำ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("เกิดข้อผิดพลาดในการอัปเดตข้อมูล: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        // ---------- Delete ----------
        public bool DeleteSupplier(string supplierID)
        {
            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand("DELETE FROM supplier WHERE sup_id = @SupplierID", conn))
            {
                cmd.Parameters.AddWithValue("@SupplierID", supplierID);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // ---------- Search (แก้ให้ใช้ตาราง/คอลัมน์เดียวกับที่อื่น) ----------
        public DataTable SearchSuppliers(string searchBy, string keyword)
        {
            DataTable dt = new DataTable();
            string sql;

            switch (searchBy)
            {
                case "ชื่อบริษัท":
                    sql = @"SELECT sup_id AS 'รหัสซัพพลายเออร์', sup_name AS 'ชื่อบริษัท',
                                   sup_juristic AS 'นิติบุคคล', sup_tel AS 'เบอร์โทรศัพท์',
                                   sup_email AS 'อีเมล', sup_address AS 'ที่อยู่'
                            FROM supplier
                            WHERE sup_name LIKE @kw";
                    break;
                case "เลขทะเบียนนิติบุคคล":
                    sql = @"SELECT sup_id AS 'รหัสซัพพลายเออร์', sup_name AS 'ชื่อบริษัท',
                                   sup_juristic AS 'นิติบุคคล', sup_tel AS 'เบอร์โทรศัพท์',
                                   sup_email AS 'อีเมล', sup_address AS 'ที่อยู่'
                            FROM supplier
                            WHERE sup_juristic LIKE @kw";
                    break;
                case "อีเมล":
                    sql = @"SELECT sup_id AS 'รหัสซัพพลายเออร์', sup_name AS 'ชื่อบริษัท',
                                   sup_juristic AS 'นิติบุคคล', sup_tel AS 'เบอร์โทรศัพท์',
                                   sup_email AS 'อีเมล', sup_address AS 'ที่อยู่'
                            FROM supplier
                            WHERE sup_email LIKE @kw";
                    break;
                default:
                    sql = @"SELECT sup_id AS 'รหัสซัพพลายเออร์', sup_name AS 'ชื่อบริษัท',
                                   sup_juristic AS 'นิติบุคคล', sup_tel AS 'เบอร์โทรศัพท์',
                                   sup_email AS 'อีเมล', sup_address AS 'ที่อยู่'
                            FROM supplier";
                    break;
            }

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                if (sql.Contains("@kw"))
                    cmd.Parameters.AddWithValue("@kw", "%" + (keyword ?? "") + "%");

                using (var ad = new MySqlDataAdapter(cmd))
                {
                    conn.Open();
                    ad.Fill(dt);
                }
            }
            return dt;
        }

        // ---------- Map MySQL duplicate key -> ข้อความไทย ----------
        private string MapDuplicateKeyMessage(MySqlException ex)
        {
            // ตัวอย่างข้อความ MySQL:
            // "Duplicate entry 'xxx' for key 'uq_supplier_email'"
            string m = ex.Message ?? "";
            if (m.Contains("uq_supplier_email") || m.Contains("sup_email")) return "อีเมลนี้มีอยู่แล้วในระบบ";
            if (m.Contains("uq_supplier_phone") || m.Contains("sup_tel")) return "เบอร์โทรนี้มีอยู่แล้วในระบบ";
            if (m.Contains("uq_supplier_juristic") || m.Contains("sup_juristic")) return "เลขนิติบุคคล/ภาษีนี้มีอยู่แล้วในระบบ";
            if (m.Contains("uq_supplier_name") || m.Contains("sup_name")) return "ชื่อบริษัทนี้มีอยู่แล้วในระบบ";
            return "ข้อมูลซ้ำในระบบ";
        }
    }
}
