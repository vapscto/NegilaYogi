using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class General_SendSMSDTO : CommonParamDTO
    {
        //public int EME_Id { get; set; }
        public long ISES_Id { get; set; }
        public string ISES_Template_Name { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long HRME_Id { get; set; }
        public long User_Id { get; set; }
        public long HRMDES_Id { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public string esubject { get; set; }
        public string footer { get; set; }
        public string fhead { get; set; }
        public string Header { get; set; }
        public filedtosms[] filelist { get; set; }
        public Array templatelist { get; set; }
        public Array employeedropdown { get; set; }
        public long?[] hrmgT_IdList { get; set; }
        public long?[] employeeTypeIdList { get; set; }
        public long?[] hrmD_IdList { get; set; }
        public long?[] hrmdeS_IdList { get; set; }
        public long ASMCL_Id { get; set; }
        public Array yearlist { get; set; }
        public Array classlist { get; set; }
        public Array sectionlist { get; set; }
        public Array routelist { get; set; }
        public Array studentroutelist { get; set; }
        public string ASMCL_ClassName { get; set; }
        public long ASMS_Id { get; set; }
        public long TRMR_Id { get; set; }
        public string ASMC_SectionName { get; set; }
        public Array studentlist { get; set; }
        public string Mobno { get; set; }
        public string mes { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string AMST_AdmNo { get; set; }
        public long AMST_MobileNo { get; set; }
        public string AMST_emailId { get; set; }
        public long AMST_Id { get; set; }
        public Array StaffName { get; set; }
        public bool atchflag { get; set; }
        public long roleId { get; set; }
        public string selectedRadiobtn { get; set; }
        public Array currentYear { get; set; }
        public Array employe { get; set; }
        public Array stafflist { get; set; }
        public string studentName { get; set; }
        public int studentCount { get; set; }
        public Array departmentdropdown { get; set; }
        public Array designationdropdown { get; set; }
        public string multipledep { get; set; }
        public string multipledes { get; set; }        
        public General_SendSMSDTO[] studentlistdto { get; set; }
        public mobilenolistdto[] mobilenolist { get; set; }
        public emaillistdto[] emaillist { get; set; }
        public string SmsMailText { get; set; }
        public string radiotype { get; set; }
        public string smsStatus { get; set; }
        public long? HRME_MobileNo { get; set; }
        public string emailStatus { get; set; }


        //adda
        public Array exmstdlist { get; set; }
        public int EME_Id { get; set; }
        public string selradioval { get; set; }
        public Array stumarkdetails { get; set; }
        public string MarksDetails { get; set; }
        public string GradeDetails { get; set; }
        public string TotalMarks { get; set; }
       public string TotalGrade { get; set; }
        public string result { get; set; }
        public string ESTMPS_PassFailFlg { get; set; }
        public string attend_radioval { get; set; }
        public DateTime? fr_date { get; set; }
        public DateTime? to_date { get; set; }
        public DateTime? crnt_date { get; set; }
        public string totalpresentday { get; set; }
        public string totalworkingday { get; set; }
        public Array attdetails { get; set; }
        public string atndetails { get; set; }
        public string exm_radioval { get; set; }
        public bool snd_email { get; set; }
        public bool snd_sms { get; set; }
        public bool stfsnd_sms { get; set; }
        public bool stfsnd_email { get; set; }
        public string hrm_email { get; set; }
        public string exmname { get; set; }
        public string attper { get; set; }
        public string neworregular { get; set; }
        public string whatsapp_filetype { get; set; }
        public string fileattachementforwhatsapp { get; set; }
        public bool? whatsapp_flag { get; set; }
        public bool? sms_flag { get; set; }
    }

    public class mobilenolistdto
    {
        public long hrmemnO_MobileNo { get; set; }
    }


    public class emaillistdto
    {
        public string hrmeM_EmailId { get; set; }
    }


    public class filedtosms
    {
        public long gridid { get; set; }
        public long cfileid { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
    }
}


