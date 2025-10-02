using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

namespace JRSApplication
{
    public class SearchService
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        public DataTable GetPhasesByProjectId(string projectId)
        {
            DataTable dt = new DataTable();

            string query = @"
                SELECT 
                    phase_id, 
                    CAST(phase_no AS CHAR) AS phase_no,
                    phase_detail
                FROM project_phase
                WHERE pro_id = @ProjectId
                ORDER BY phase_no ASC;
            ";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ProjectId", projectId);
                conn.Open();

                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        public DataTable SearchData(string searchType, string keyword)
        {
            DataTable dt = new DataTable();
            string query = "";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                if (searchType == "Customer")
                {
                    query = @"
                        SELECT cus_id AS 'ID', cus_name AS 'ชื่อ', cus_lname AS 'นามสกุล',
                               cus_id_card AS 'เลขบัตรประชาชน', cus_tel AS 'เบอร์โทร', cus_email AS 'อีเมล'
                        FROM customer
                        WHERE cus_name LIKE @Keyword OR cus_lname LIKE @Keyword
                    ";
                }
                else if (searchType == "Employee")
                {
                    query = @"
                        SELECT emp_id AS 'ID', emp_name AS 'ชื่อ', emp_lname AS 'นามสกุล',
                               emp_pos AS 'ตำแหน่ง', emp_tel AS 'เบอร์โทร'
                        FROM employee
                        WHERE emp_name LIKE @Keyword OR emp_lname LIKE @Keyword
                    ";
                }
                else if (searchType == "Supplier")
                {
                    query = @"
                        SELECT sup_id AS 'ID', sup_name AS 'ชื่อบริษัท', sup_juristic AS 'เลขทะเบียนนิติบุคคล',
                               sup_tel AS 'เบอร์โทร', sup_address AS 'ที่อยู่', sup_email AS 'อีเมล'
                        FROM supplier
                        WHERE sup_name LIKE @Keyword OR sup_juristic LIKE @Keyword
                    ";
                }
                else if (searchType == "Project")
                {
                    query = @"
                        SELECT 
                            p.pro_id AS 'รหัสโครงการ',
                            p.pro_name AS 'ชื่อโครงการ',
                            p.pro_number AS 'เลขที่สัญญา',
                            p.pro_address AS 'สถานที่',
                            p.pro_budget AS 'งบประมาณ',
                            c.cus_id AS 'รหัสลูกค้า',
                            CONCAT(c.cus_name, ' ', c.cus_lname) AS 'ลูกค้า',
                            c.cus_tel AS 'เบอร์โทร',
                            c.cus_email AS 'อีเมล',
                            CONCAT(e.emp_name, ' ', e.emp_lname) AS 'พนักงานดูแล'
                        FROM project p
                        LEFT JOIN customer c ON p.cus_id = c.cus_id
                        LEFT JOIN employee e ON p.emp_id = e.emp_id
                        WHERE p.pro_name LIKE @Keyword OR p.pro_id LIKE @Keyword
                    ";
                }
                else if (searchType == "Invoice")
                {
                    // เปลี่ยนมาใช้ inv_id (ไม่มี inv_no ในตาราง)
                    // ถ้าค้นด้วยคีย์เวิร์ด ให้ CAST inv_id เป็น CHAR ก่อน
                    query = @"
                        SELECT 
                            inv_id        AS 'เลขที่ใบแจ้งหนี้',
                            inv_date      AS 'วันที่ออก',
                            inv_status    AS 'สถานะ',
                            inv_method    AS 'วิธีชำระเงิน',
                            cus_id        AS 'รหัสลูกค้า',
                            pro_id        AS 'รหัสโครงการ',
                            emp_id        AS 'รหัสพนักงาน',
                            inv_duedate   AS 'กำหนดชำระ',
                            paid_date     AS 'วันที่ชำระ'
                        FROM invoice
                        WHERE pro_id LIKE @Keyword OR CAST(inv_id AS CHAR) LIKE @Keyword
                    ";
                }
                else if (searchType == "UnpaidInvoiceByProject")
                {
                    // เฉพาะใบแจ้งหนี้สถานะ 'รอชำระเงิน' ของโครงการที่ระบุ
                    query = @"
                        SELECT 
                            inv_id,
                            DATE_FORMAT(inv_date,'%Y-%m-%d')     AS inv_date,
                            DATE_FORMAT(inv_duedate,'%Y-%m-%d')  AS inv_duedate,
                            pro_id,
                            cus_id,
                            emp_id,
                            inv_method,
                            inv_status
                        FROM invoice
                        WHERE pro_id = @projectId
                          AND inv_status = 'รอชำระเงิน'
                        ORDER BY inv_date DESC, inv_id DESC;
                    ";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@projectId", keyword); // keyword = pro_id
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                    return dt; // ออกเลย
                }

                if (string.IsNullOrWhiteSpace(query))
                    return dt;

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            return dt;
        }

        public DataTable GetAllInvoices()
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                string query = @"
                    SELECT 
                        i.inv_id,
                        i.inv_date,
                        i.inv_duedate,
                        i.inv_status,
                        i.paid_date,
                        i.pro_id,
                        i.cus_id,
                        i.phase_id
                    FROM invoice i
                    WHERE i.inv_status = 'รอชำระเงิน' OR i.inv_status IS NULL
                ";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetDraftInvoicesByProject(string projectId)
        {
            // ★ เพิ่ม inv_status เพื่อให้กริดบนเอาไปโชว์และทำสีได้
            string query = @"
                SELECT 
                    i.inv_id,
                    i.inv_date,
                    i.inv_duedate,
                    i.pro_id,
                    i.phase_id,
                    i.cus_id,
                    i.inv_status
                FROM invoice i
                WHERE (i.inv_status IS NULL OR i.inv_status = 'รอชำระเงิน')
                  AND i.pro_id = @projectId
                ORDER BY i.inv_date DESC
            ";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@projectId", projectId);
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetCustomerById(string cusId)
        {
            string query = @"
                SELECT cus_name, cus_lname, cus_id_card, cus_address
                FROM customer
                WHERE cus_id = @cusId
            ";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@cusId", cusId);
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetPaidInvoicesByProject(string projectId)
        {
            string query = @"
                SELECT 
                    i.inv_id,
                    i.inv_date,
                    i.inv_duedate,
                    i.inv_status,
                    i.inv_method,
                    i.paid_date,
                    i.phase_id,
                    c.cus_name,
                    c.cus_id_card,
                    c.cus_address,
                    p.pro_name,
                    p.pro_number,
                    e.emp_name,
                    e.emp_lname,
                    i.cus_id,
                    i.pro_id,
                    i.emp_id
                FROM invoice i
                LEFT JOIN customer c ON i.cus_id = c.cus_id
                LEFT JOIN project p  ON i.pro_id = p.pro_id
                LEFT JOIN employee e ON i.emp_id = e.emp_id
                WHERE i.inv_status = 'ชำระแล้ว'
                  AND i.pro_id = @projectId
                ORDER BY i.paid_date DESC;
            ";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@projectId", projectId);

                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetProjectById(string proId)
        {
            DataTable dt = new DataTable();
            string query = @"
                SELECT pro_name
                FROM project
                WHERE pro_id = @proId
            ";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@proId", proId);
                conn.Open();

                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        public DataTable GetPaidInvoices()
        {
            // เปลี่ยน inv_no -> inv_id
            string query = @"
                SELECT 
                    i.inv_id,
                    i.inv_date,
                    i.inv_duedate,
                    i.inv_status,
                    i.inv_method,
                    i.paid_date,
                    c.cus_name,
                    c.cus_id_card,
                    c.cus_address,
                    p.pro_name,
                    p.pro_number,
                    e.emp_name,
                    e.emp_lname
                FROM invoice i
                LEFT JOIN customer c ON i.cus_id = c.cus_id
                LEFT JOIN project p  ON i.pro_id = p.pro_id
                LEFT JOIN employee e ON i.emp_id = e.emp_id
                WHERE i.inv_status = 'ชำระแล้ว'
                ORDER BY i.paid_date DESC;
            ";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
    }
}
