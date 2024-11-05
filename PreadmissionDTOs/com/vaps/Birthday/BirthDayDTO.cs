using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.BirthDay
{
    public class BirthDayDTO
    {

        //------ public
        long HRMET_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMET_EmployeeType { get; set; }
        public bool HRMET_ActiveFlag { get; set; }
        public Array employeeTypeList { get; set; }
        public string retrunMsg { get; set; }
        public long roleId { get; set; }
        public DateTime? HRME_DOB { get; set; }

        public long HRME_Id { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public bool? HRME_LeftFlag { get; set; }
        public long HRME_MobileNo { get; set; }
        public string HRME_EmailId { get; set; }
        //----
        public long yearid { get; set; }
        public long asmcL_Id { get; set; }
        public long asmC_Id { get; set; }
        public long sectionid { get; set; }
        public string AMST_SOL { get; set; }
        public long AMST_Id { get; set; }
        public string stuFN { get; set; }
        public string stuMN { get; set; }
        public string stuLN { get; set; }
        public string regno { get; set; }
        public string admno { get; set; }
        //-------------------------------
        public string AMST_FirstName { get; set; }
        public string AMST_AdmNo { get; set; }
        public long MI_ID { get; set; }
        public long ASMAY_Id { get; set; }


        public DateTime? amst_dob { get; set; }

        public string ASMCL_ClassName { get; set; }

        public string ASMC_SectionName { get; set; }
        public Array sms_mail_count { get; set; }
        public Array studentDetails { get; set; }
        public Array staffList { get; set; }
        public Array classDrpDwn { get; set; }
        public Array sectionDrpDwn { get; set; }
        public Array studentlist { get; set; }
        public string flags { get; set; }
        public string flag { get; set; }
        public string flagl { get; set; }
        public string day { get; set; }
        public string months { get; set; }

        public string days1 { get; set; }
        public string days2 { get; set; }

        public string all1 { get; set; }
        public Array accyear { get; set; }
        public int count { get; set; }

        public int count1 { get; set; }
        public int count2 { get; set; }

        public long ALMST_Id { get; set; }

        //public DateTime? Start_Date { get; set; }
        public DateTime? date11 { get; set; }
        public long AMST_MobileNo { get; set; }
        public string AMST_emailId { get; set; }
        public string AMST_emailId1 { get; set; }
        public string AMST_emailId2 { get; set; }
        //public long[] SelectedActivityDetails_stu { get; set; }
        public MasterEmployeeDTO[] SelectedActivityDetails1_stf { get; set; }
        public MasterEmployeeDTO[] SelectedActivityDetails_stu { get; set; }
        public string sms_text { get; set; }

        public string rdbbutton { get; set; }
        public string studentName { get; set; }
        public string employeeName { get; set; }
        public BirthDayDTO[] selectedStudent { get; set; }
        public BirthDayDTO[] selectedEmployees { get; set; }
        public string smsflag { get; set; }
        public string emailflag { get; set; }
        public string ISES_WhatsAppTemplateId { get; set; }
        public string ISES_NAME { get; set; }
        public string WhatsappFlag { get; set; }
        public string emailStatus { get; set; }
        public string whatsappstatus { get; set; }
        public string smsStatus { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string smsStatus1 { get; set; }
        public string smsStatus2 { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public Array mail_count_list { get; set; }
        public string SearchColumn { get; set; }
        public string EnteredData { get; set; }
        public string message { get; set; }
        public bool staffChecked { get; set; }
        public bool studChecked { get; set; }
        public string To_FLag { get; set; }
        public string PhotoPath { get; set; }
        public Array fillmonth { get; set; }
        public Array fillyear { get; set; }
        public long year { get; set; }
        public int month { get; set; }
        public DateTime? date12 { get; set; }
        public long ssb { get; set; }
        public long ssb1 { get; set; }
        public long ssb2 { get; set; }
        public long esb { get; set; }
        public long esb1 { get; set; }
        public long esb2 { get; set; }
        public DateTime? Fromdate { get; set; }
        public DateTime? Todate { get; set; }
        public Array staffDetails { get; set; }

        public int smscount { get; set; }
        public int emailcount { get; set; }
        public int workingEmployee { get; set; }
        public int missingPhoto { get; set; }
        public int missingEmailId { get; set; }
        public int missingContactNumber { get; set; }
        public int onlylateinDetailssms { get; set; }
        public int onlylateinDetailsEmail { get; set; }
        public int onlyEarlyOutDetailssms { get; set; }
        public int onlyEarlyOutDetailsEmail { get; set; }
        public int onlyAbsentDetailssms { get; set; }
        public int onlyAbsentDetailsEmail { get; set; }

        public ASMAY_List[] ASMAY_IdList { get; set; }
        public long ayear { get; set; }
        public Array Emailcounts { get; set; }

        public string Logintype { get; set; }
        public long MO_Id { get; set; }






        public string QCAC_Name { get; set; }
        public string QCAC_Email { get; set; }

        public string QCAC_Subject { get; set; }
        public string QCAC_Query { get; set; }
        public string QCAC_MobileNo { get; set; }

    }
    public class ASMAY_List
    {
        public long ASMAY_Id { get; set; }
    }

    public class ApiParm
    {
        public string messaging_product { get; set; }
        public string to { get; set; }
        public string type { get; set; }
        public templete template { get; set; }
    }

    public class templete
    {
        public string name { get; set; }
        public language language { get; set; }
        public List<component> components { get; set; }
    }
    public class component
    {
        public string type { get; set; }
        public List<parameter> parameters { get; set; }
    }
    public class parameter
    {
        public string type { get; set; }
        public string text { get; set; }
    }
    public class language
    {
        public string code { get; set; }
    }







}
