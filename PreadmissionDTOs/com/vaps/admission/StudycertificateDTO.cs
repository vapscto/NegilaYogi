using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class StudycertificateDTO
    {
        public long MI_Id { get; set; }
        public long userid { get; set; }
        public string AMST_FirstName { get; set; }
        public long AMST_Id { get; set; }
        public string AMST_SOL { get; set; }
        public Array fillstudlist { get; set; }
        public string AMST_RegistrationNo { get; set; }
        public string AMST_AdmNo { get; set; }
        public Array AllAcademicYear { get; set; }
        public Array adm_m_student { get; set; }
        public string IMC_CasteName { get; set; }
        public long ASMAY_Id { get; set; }
        public long? ASMCL_Id { get; set; }
        public long ASMC_Id { get; set; }
        public Array studentlist { get; set; }
        public Array academicYearOnLoad { get; set; }
        public Array StudentList { get; set; }
        public Array academicyearforreadmit { get; set; }
        public Array MasterCompany { get; set; }
        public string companyname { get; set; }
        public Array academicList1 { get; set; }
        public Array allsectionlist { get; set; }
        public Array allclasslist { get; set; }
        public string searchfilter { get; set; }
        public int count { get; set; }
        public string photopath { get; set; }
        public string allorindid { get; set; }
        public Array principalsign { get; set; }
        public DateTime dob { get; set; }
        public string dobwords { get; set; }
        public string fathername { get; set; }
        public DateTime joinedyear { get; set; }
        public DateTime leftyear { get; set; }
        public string joinedclass { get; set; }
        public string leftclass { get; set; }
        public string save_flag { get; set; }
        public string message { get; set; }


        //mail parameters
        public long prospectus_no { get; set; }
        public string First_installment { get; set; }
        public DateTime paidOnOrBefore { get; set; }
        public string broughtAdmissionOn { get; set; }

        public string fromtime { get; set; }
        public string totime { get; set; }
        public long demandDraft { get; set; }
        public bool mailflag { get; set; }

        public string AMST_emailId { get; set; }
        public string Template { get; set; }

        public mailParameters1[] mailParameters { get; set; }

        public bool categoryflag { get; set; }

        public Array category_list { get; set; }
        public long AMC_Id { get; set; }
        public Array AMC_logo { get; set; }



    }
    public class mailParameters1
    {
        public string caution_amt { get; set; }
        public DateTime brought_admission { get; set; }
        public string first_installemnt { get; set; }
        public string demand_amt { get; set; }
        public string before_time { get; set; }
        public string from_btw_time { get; set; }
        public string to_btw_time { get; set; }
        public DateTime selected_date { get; set; }
        public DateTime paid_onor_before { get; set; }
    }
}
