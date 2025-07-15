using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRSApplication.Components
{
    public class Customer
    {
        public int CustomerID { get; set; } // cus_id (Primary Key)
        public string FirstName { get; set; } // cus_name
        public string LastName { get; set; } // cus_lname
        public string IDCard { get; set; } // cus_id_card
        public string Phone { get; set; } // cus_tel
        public string Address { get; set; } // cus_address
        public string Email { get; set; } // cus_email
    }
}
