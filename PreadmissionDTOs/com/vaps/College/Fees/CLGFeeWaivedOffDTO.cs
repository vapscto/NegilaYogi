using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Fees
{
    public class CLGFeeWaivedOffDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long AMCST_Id { get; set; }
        public Array yearlist { get; set; }
        public Array courselist { get; set; }
        public Array branchlist { get; set; }
        public Array semisterlist { get; set; }
        public Array sectionlist { get; set; }
        public Array studentlist { get; set; }
        public Array grouplist { get; set; }
        public Array headlist { get; set; }
        public Array alldata { get; set; }
       // public bool returnval { get; set; }
        public string AMCST_FirstName { get; set; }
        public string filterrefund { get; set; }
        public long FMG_Id { get; set; }
        public string FMG_GroupName { get; set; }
        public long FMH_Id { get; set; }
        public string FMH_FeeName { get; set; }
        public long FTI_Id { get; set; }
        public string FTI_Name { get; set; }
        public long FCMAS_Id { get; set; }
        public long FCSS_TotalCharges { get; set; }
        public long FCSS_ToBePaid { get; set; }
        public long FCSS_PaidAmount { get; set; }
        public string multiplegroup { get; set; }
        public long FCSWO_Id { get; set; }
        public DateTime FCSWO_Date { get; set; }
        public long FCSWO_WaivedOffAmount { get; set; }
        public CLGFeeWaivedOffDTO[] checkedlist { get; set; }
        public string returnduplicatestatus { get; set; }
    }
}
