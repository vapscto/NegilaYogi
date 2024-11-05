using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class TT_Master_BuildingDTO
    {
        public string returnduplicatestatus;
        public bool returnval;
        public TT_Master_BuildingDTO[] duplicateList;

        public long TTMB_Id { get; set; }
        public long MI_Id { get; set; }
        public string TTMB_BuildingName { get; set; }
        public bool TTMB_ActiveFlag { get; set; }

        public bool TTMBCS_ActiveFlag { get; set; }

        public long TTMBCS_Id { get; set; }
      //  public int TTMB_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public Array bnamedrp {get;set;}
        public Array secdrp {get; set;}
        public Array clsdrp { get; set; }
        public Array csmap { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }

        public Array masterbuild { get; set; }
        public Array mastersection { get; set; }
        public Array masterbuilding { get; set;}
        public Array mastersectionbuilding { get; set;}
        public TT_Master_BuildingDTO[] classarray { get; set;}
        public TT_Master_BuildingDTO[] sectionarray { get; set; }
    }
}
