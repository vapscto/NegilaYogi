using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class Exm_Login_PrivilegeDTO
    {
        public bool already_cnt { get; set; }
        public int ELP_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public int Login_Id { get; set; }
        public string ELP_Flg { get; set; }
        public bool ELP_ActiveFlg { get; set; }
        public int ELPS_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long ISMS_Id { get; set; }
        public bool ELPs_ActiveFlg { get; set; }
        public bool ELPss_ActiveFlg { get; set; }
        public int ELPSS_Id { get; set; }
        public int EMSS_Id { get; set; }
        public int ASMAY_Order { get; set; }
        public long User_Id { get; set; }
        public string UserName { get; set; }
        public Array exammastername { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public Array editlist {get;set;}
        public Array yearlist { get; set; }
        public Array ctlist { get; set; }
        public Array grouplist { get; set; }
        public Array classlist { get; set; }
        public Array seclist { get; set; }
        public Array subjlist { get; set; }
        public Array subsubject { get; set; }
        public Array studlist { get; set; }
        public Array studmaplist { get; set; }
        public Array gtdetailsview { get; set; }
        public Array edclasslist { get; set; }
        public Array emplist { get; set; }
        public Array userlist { get; set; }
        public Array pllist { get; set; }
        public Array clastechlt { get; set; }
        public Array editlist1 { get; set; }
        public tempPrivilagesDTO[] selectedclass { get; set; }      
        public string action { get; set; }
        public int EMG_Id { get; set; }
        public string EMG_GroupName { get; set; }
        public int EMCA_Id { get; set; }
        public string EMCA_CategoryName { get; set; }
        public int ECAC_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public string AMST_FirstName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string ASMAY_Year { get; set; }
        public string EMSS_SubSubjectName { get; set; }
        public long HRME_Id { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public int IVRMULF_Id { get; set; }
        public long Emp_Code { get; set; }
    }
}
