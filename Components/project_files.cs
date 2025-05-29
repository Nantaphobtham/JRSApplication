using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRSApplication.Components
{
    public class ProjectFile
    {
        public int FileID { get; set; }
        public int ProjectID { get; set; }
        public byte[] ConstructionBlueprint { get; set; }
        public byte[] DemolitionModel { get; set; }
    }

}
