using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class BranchChangeDTO:CommonParamDTO
    {
        public long ACSCOB_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long ACSCOB_AMB_Id { get; set; }
        public long AMB_Id { get; set; }
        public string ACSCOB_OldRegNo { get; set; }
        public string ACSCOB_NewRegNo { get; set; }
        public string ACSCOB_COBFees { get; set; }
        public string ACSCOB_Remarks { get; set; }
        public DateTime? ACSCOB_COBDate { get; set; }
        public string ACSCOB_CreatedBy { get; set; }
        public string ACSCOB_UpdatedBy { get; set; }
        public bool ACSCOB_ActiveFlag { get; set; }
        public Array yearlist { get; set; }
        public Array courseslist { get; set; }
        public Array branchlist { get; set; }
        public Array semisterslist { get; set; }
        public Array sectionslist { get; set; }
        public Array studlist { get; set; }
        public Array datalist { get; set; }
        public string AMCST_Name { get; set; }
        public string AMCST_RegistrationNo { get; set; }
        public long userid { get; set; }
        public bool returnval { get; set; }
        public string returnduplicatestatus { get; set; }
        public long AMSE_Id_Old { get; set; }
        public long AMSE_Id_New { get; set; }
        public long ACMS_Id_Old { get; set; }
        public long ACMS_Id_New { get; set; }
        public long? ACSCOB_AMSE_Id_Old { get; set; }
        public long? ACSCOB_AMSE_Id_New { get; set; }
    }
}
