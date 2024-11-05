using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeConcessionDTO : CommonParamDTO
    {
        //added
        public long FSS_Id { get; set; }
        public string message{get;set;}

        public long FSC_Id { get; set; }
        public long MI_Id { get; set; }
        public long FMC_Id { get; set; }
        public long AMST_Id { get; set; }
        //public long ASMAY_ID { get; set; }
        public long FMG_Id { get; set; }
        public long FMH_Id { get; set; }
        public string FSC_ConcessionReason { get; set; }
        public string FSC_ConcessionType { get; set; }
        public string FMSG_ActiveFlag { get; set; }

        public long ASMAY_Id { get; set; }
        public Array fillcategory { get; set; }
        public Array fillclass { get; set; }
        public Array fillgroup { get; set; }
        public string AMC_Name { get; set; }
        public long AMC_Id { get; set; }

        public string ASMCL_ClassName { get; set; }
        public long ASMCL_Id { get; set; }
        public long FTI_Id { get; set; }

        public long FMT_ID { get; set; }

        public string FMT_Name { get; set; }
        public string FMG_GroupName { get; set; }
        public string FMH_FeeName { get; set; }

        public string FTI_Name { get; set; }

        public long FMA_Amount { get; set; }
        public long FMA_Id { get; set; }

        public Array studentdata { get; set; }

        public string radiobtnvalue { get; set; }

        public Array fillheaddata { get; set; }

        public Array fillterm { get; set; }
        public FeeConcessionDTO[] savetmpdata { get; set; }
        public FeeConcessionDTO[] savetmpdata1 { get; set; }

        public string returnval { get; set; }

        public long FSCI_ConcessionAmount { get; set; }

        public string studentname { get; set; }

        public string configset { get; set; }

        public Array savedcondatalist { get; set; }

        public string multiplegroups { get; set; }
        public string AMST_AdmNo { get; set; }
        public long userid { get; set; }
        public long FSCI_ID { get; set; } //added by kiran

        //MB
        public long[] FMG_Ids { get; set; }
        //MB
        public long[] ASMCL_Ids { get; set; }

        public long[] FMT_Ids { get; set; }
        public long [] FMI_Ids { get; set; }
        public Array configsetting { get; set; }

        public Array stafflist { get; set; }

        public Array staffdata { get; set; }

        public long FEC_Id { get; set; }
        public long FMCC_Id { get; set; }
        public long HRME_Id { get; set; }
        public string FEC_ConcessionReason { get; set; }
        public string FEC_ConcessionType { get; set; }
        public bool FEC_ActiveFlag { get; set; }
        public long FECI_Id { get; set; }
        public long FECI_FEC_Id { get; set; }
        
        public string HRME_EmployeeFirstName { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public string HRMD_DepartmentName { get; set; }

        public Array fillyear { get; set; }

        public Array specialheaddetails { get; set; }
        public Array specialheadlist { get; set; }
        public Array instalspecial { get; set; }

       public Array filinstallment { get; set; }
        public long[] terms_groups { get; set; }

        //others data
        public Array othersdata { get; set; }
        public Array otherlist { get; set; }

        public string FMOST_StudentName { get; set; }

        public long FMOST_StudentMobileNo { get; set; }
        public string FMOST_StudentEmailId { get; set; }

        public long FOC_Id { get; set; }
        public long FOCI_Id { get; set; }

        public long FMOST_Id { get; set; }

        public Array fillfeecategory { get; set; }

        public Array EditfeeDetails { get; set; }

        public string searchfilter { get; set; }
        public Array fillstudent { get; set; }
    }
}
