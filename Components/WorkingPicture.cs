using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRSApplication.Components
{
    public class WorkingPicture
    {
        public int PhaseNo { get; set; }
        public int PictureID { get; set; }
        public string PictureDetail { get; set; }
        public byte[] PictureData { get; set; }
    }
}
