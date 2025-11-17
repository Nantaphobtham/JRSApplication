using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace JRSApplication.Components.Models
{
    public static class RoleIconHelper
    {
        public static Image GetProfileIcon(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                return Properties.Resources.Profile; // default

            switch (role)
            {
                case "Admin":
                    return Properties.Resources.Admin;

                case "Projectmanager":     // ชื่อให้ตรงกับ emp_pos ใน DB
                    return Properties.Resources.Projectmanager;

                case "Sitesupervisor":
                    return Properties.Resources.Sitesupervisor;

                case "Accountant":
                    return Properties.Resources.Accountant;

                default:
                    return Properties.Resources.Profile;
            }
        }
    }
}
