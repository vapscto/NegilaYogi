using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.TT
{
  public  class CLGMasterBuilding_DTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long TTMBCS_Id { get; set; }
      public long TTMB_Id { get; set; }
      public long AMCO_Id { get; set; }
      public long AMB_Id { get; set; }
      public long AMSE_Id { get; set; }
      public long ACMS_Id { get; set; }
      public DateTime CreatedDate { get; set; }
      public DateTime UpdatedDate { get; set; }
        public bool TTMBCS_ActiveFlag { get; set; }
        public string TTMB_BuildingName { get; set; }
        public string ASMAY_Year { get; set; }
        public int ASMAY_Order { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public Array courselist { get; set; }
        public Array bnamedrp { get; set; }
        public Array masterbuilding { get; set; }
        public Array secdrp { get; set; }
        public CLGMasterBuilding_DTO[] sectionarray { get; set; }
        public string AMCO_CourseName { get; set; }
        public string ACMS_SectionName { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMSE_SEMName { get; set; }
        public Array csmap { get; set; }
        public Array academic { get; set; }
        public Array mastersection { get; set; }


    }
}
