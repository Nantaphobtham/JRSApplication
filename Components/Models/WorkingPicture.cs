using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRSApplication.Components
{
    public class WorkingPicture
    {
        public int PictureID { get; set; }         // picture_id
        public int WorkID { get; set; }            // work_id
        public int PicNo { get; set; }          // pic_no
        public string PicName { get; set; }        // pic_name
        public string Description { get; set; }    // description
        public byte[] PictureData { get; set; }    // picture_data
        public DateTime CreatedAt { get; set; }    // created_at

    }
}
