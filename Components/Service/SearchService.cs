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
                            CAST(phase_no AS CHAR) AS phase_no,   -- ✅ ตรงนี้!
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
                    query = "SELECT cus_id AS 'ID', cus_name AS 'ชื่อ', cus_lname AS 'นามสกุล', " +
                            "cus_id_card AS 'เลขบัตรประชาชน', cus_tel AS 'เบอร์โทร', cus_email AS 'อีเมล' " +
                            "FROM customer WHERE cus_name LIKE @Keyword OR cus_lname LIKE @Keyword";
                }
                else if (searchType == "Employee")
                {
                    query = "SELECT emp_id AS 'ID', emp_name AS 'ชื่อ', emp_lname AS 'นามสกุล', " +
                            "emp_pos AS 'ตำแหน่ง' FROM employee " +
                            "WHERE emp_name LIKE @Keyword OR emp_lname LIKE @Keyword";
                }
                else if (searchType == "Supplier")
                {
                    query = @"SELECT sup_id AS 'ID', sup_name AS 'ชื่อบริษัท', sup_juristic AS 'เลขทะเบียนนิติบุคคล',
                      sup_tel AS 'เบอร์โทร', sup_address AS 'ที่อยู่', sup_email AS 'อีเมล'
                      FROM supplier
                      WHERE sup_name LIKE @Keyword OR sup_juristic LIKE @Keyword";
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
                                    CONCAT(c.cus_name, ' ', c.cus_lname) AS 'ลูกค้า',
                                    CONCAT(e.emp_name, ' ', e.emp_lname) AS 'พนักงานดูแล'
                                FROM project p
                                LEFT JOIN customer c ON p.cus_id = c.cus_id
                                LEFT JOIN employee e ON p.emp_id = e.emp_id
                                WHERE p.pro_name LIKE @Keyword OR p.pro_id LIKE @Keyword
                            ";
                }
                else if (searchType == "Invoice")
                {
                    query = @"
                            SELECT 
                                inv_no AS 'เลขที่ใบแจ้งหนี้',
                                inv_date AS 'วันที่ออก',
                                inv_status AS 'สถานะ',
                                inv_method AS 'วิธีชำระเงิน',
                                cus_id AS 'รหัสลูกค้า',
                                pro_id AS 'รหัสโครงการ',
                                emp_id AS 'รหัสพนักงาน',
                                inv_duedate AS 'กำหนดชำระ',
                                paid_date AS 'วันที่ชำระ'
                            FROM invoice
                            WHERE pro_id LIKE @Keyword OR inv_no LIKE @Keyword
    ";
                }

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
        public DataTable GetPaidInvoices()
        {
            DataTable dt = new DataTable();
            string query = @"
                            SELECT 
                                i.inv_no AS 'inv_no',
                                CONCAT(c.cus_name, ' ', c.cus_lname) AS 'ข้อมูลลูกค้า',
                                c.cus_id_card AS 'เลขบัตรประชาชน',
                                c.cus_address AS 'ที่อยู่',
                                p.pro_number AS 'เลขที่สัญญา',
                                p.pro_name AS 'ชื่อโครงการ',
                                p.pro_currentphasenumber AS 'เฟสงาน',
                                CONCAT(e.emp_name, ' ', e.emp_lname) AS 'พนักงานผู้รับเงิน',
                                i.paid_date AS 'วันที่ชำระ',
                                i.inv_method AS 'วิธีชำระเงิน'
                            FROM invoice i
                            JOIN customer c ON i.cus_id = c.cus_id
                            JOIN project p ON i.pro_id = p.pro_id
                            JOIN employee e ON i.emp_id = e.emp_id
                            WHERE i.inv_status = 'ชำระแล้ว';
                        ";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            return dt;
        }




    }
}
