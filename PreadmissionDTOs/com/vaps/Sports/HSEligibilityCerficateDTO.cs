using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Sports
{
    public class HSEligibilityCerficateDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public Array yearlist { get; set; }
        public string ASMAY_Year { get; set; }
        public Array ClassList { get; set; }
        public Array SectionList { get; set; }
        public Array StudentList1 { get; set; }
        public Array EventList { get; set; }
        public long AMST_Id { get; set; }
        public Array age_tilldate { get; set; }
        public int Age_Years { get; set; }
        public int Age_Months { get; set; }
        public int Age_Days { get; set; }
        public Array datareport { get; set; }
        public string studentname { get; set; }
        public string AMST_FatherName { get; set; }
        public string MI_Name { get; set; }
        public string MI_Address { get; set; }
        public long MIMN_MobileNo { get; set; }
        public string MIE_EmailId { get; set; }
        public long AMST_MobileNo { get; set; }
        public DateTime AMST_DOB { get; set; }
        public string AMST_DOB_Words { get; set; }
        public string SPCCME_EventName { get; set; }
        public string Address { get; set; }
        public long? AMST_FatherMobleNo { get; set; }
        public string AMST_AdmNo { get; set; }
        public DateTime AMST_Date { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string prevclsname { get; set; }
        public string ASMC_SectionName { get; set; }
        public long SPCCME_Id { get; set; }
        public string SPCCPM_Name { get; set; }
        public long SPCCPM_Id { get; set; }
        public string AMST_BPLCardNo { get; set; }
        public long? AMST_AadharNo { get; set; }
        public string AMST_Photoname { get; set; }
        public string AMST_MotherName { get; set; }
        public string AMST_GovtAdmno { get; set; }
        public Array pudatareport { get; set; }
        public string AMST_MotherBankAccNo { get; set; }
        public string AMST_MotherBankIFSC_Code { get; set; }
        public int MI_Pincode { get; set; }
        public Array clsslst { get; set; }

    }
}
