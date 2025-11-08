using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRSApplication.Components.Models
{
    public class SupplierAssignmentFile
    {
        public int FileID { get; set; }  // file_id
        public string AssignmentId { get; set; }  // supplier_assignment_id
        public string FileName { get; set; }  // file_name
        public string FileType { get; set; }  // file_type
        public byte[] FileData { get; set; }  // file_data
        public DateTime UploadedAt { get; set; }  // uploaded_at
        public string UploadedBy { get; set; }  // uploaded_by

    }
}
