using PreadmissionDTOs.com.vaps.College.Preadmission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class CommonDTO : CommonParamDTO
    {
        public string flag { get; set; }
        public long pageid { get; set; }
        public long Rcredit { get; set; }
        public long PaymentNootificationGeneral { get; set; }
        public long smscreditalert { get; set; }
        public int pageId { get; set; }
        public int roleId { get; set; }
        public int IVRM_MO_Id { get; set; }

        public DateTime prestartdate { get; set; }

        public DateTime presenddate { get; set; }
        public int IVRM_MI_Id { get; set; }
        // public int moduleId { get; set; }
        public bool IVRMRP_AddFlag { get; set; }
        public bool smsalrtflag { get; set; }
        public bool IVRMRP_UpdateFlag { get; set; }
        public bool IVRMRP_DeleteFlag { get; set; }
        public bool IVRMRP_ProcessFlag { get; set; }

        public bool IVRMRP_ReportFlag { get; set; }
        public Array AcademicList { get; set; }
        public Array TrustList { get; set; }
        public Array InstituteList { get; set; }
        public Array masterConfigList { get; set; }
        public Array Templates { get; set; }
        public Array InstituteTemplates { get; set; }
        public Array pagePreviledgs { get; set; }
        // Added on 10-11-2016 for organisation & Institute name
        public string orgName { get; set; }
        public string instName { get; set; }
        // Added on 10-11-2016 for organisation & Institute name

        public Array statuslist { get; set; }
        public Array classlist { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long PAMST_Id { get; set; }
        public Array studentlist { get; set; }
        public Array studentlistreport { get; set; }
        public Array studentcountreport { get; set; }
        
        public Array studentlicategorylist { get; set; }
        public StudentApplicationDTO[] pasr_id { get; set; }
        public StudentApplicationDTO[] status { get; set; }
        public StudentApplicationDTO[] selectedclass { get; set; }
        public StudentApplicationDTO[] remark { get; set; }

        public Array countreportstatus { get; set; }
        public Array applicationstatus { get; set; }
        public long PASRAPS_ID { get; set; }

        public string hostname { get; set; }

        public long role { get; set; }

        public long userId { get; set; }
        public string mobileNo { get; set; }
        public int count { get; set; }





        //added on 12 jan 2017

        public long AMC_Id { get; set; }
        public string AMC_Name { get; set; }


        public string name { get; set; }
        public long smsmobileno { get; set; }
        public string sendmailid { get; set; }
        public string mailsubject { get; set; }
        public string msgcontent { get; set; }
        public CommonDTO[] bulkmailsNmobilenos { get; set; }
        public string msgtype { get; set; }
        public string stustaffflag { get; set; }
        public long acayearid { get; set; }
        public long acaclsid { get; set; }
        public Array yerlist { get; set; }
        public Array clslist { get; set; }


        public Array fillstudent { get; set; }
        public Array fillstaff { get; set; }


        public string subDomainName { get; set; }

        public string subDomainNamelogout  { get; set; }

        public string multiplewindowvalue { get; set; }
        public string displaymessage { get; set; }

        public long mi_id { get; set; }

        public string username { get; set; }

        public Array ivrmconfiglist { get; set; }

        public Array fillinstitution  { get; set; }

        public Array EmailarrayList { get; set; }

        public Array PhonearrayList { get; set; }

        public Array institutiondetails { get; set; }

        public string status_type { get; set; }
        public StudentApplicationDTO[] data_array { get; set; }
        public string _type { get; set; }


        public long virtualid { get; set; }

        public string Machine_Ip_Address { get; set; }

        public long amst_id { get; set; }

        public bool payment { get; set; }
        public bool registration  { get; set; }

        public Array allyearlist { get; set; }

        public Array allclasslist { get; set; }
        public string smscontent { get; set; }

        public Array smsemailarry  { get; set; }

      
        public Array studetaarray { get; set; }
        public string header { get; set; }

        public string Subject { get; set; }
        public string Message { get; set; }
        public string Footer { get; set; }

        public bool smscheck { get; set; }
        public bool emailcheck  { get; set; }

        public bool defaultsmsemail  { get; set; }

        public Array studentDetails { get; set; }
        public Array getstudentDetails { get; set; }
        
        public long INSTITUTIONID { get; set; }

        public string INT_NAME { get; set; }

        public long API_ID { get; set; }

        public string API_URL { get; set; }

        public string API_NAME { get; set; }

        public Array APIARRAY { get; set; }

        public string INSTITUTECODE { get; set; }



         public string INSTITUTION_NAME { get; set; }

        public string INSTITUTION_LOGO  { get; set; }

        public bool Noint { get; set; }

        public string file { get; set; }

        public string containername { get; set; }

        public string Base64string { get; set; }

        public string folder { get; set; }

        public Array courselist { get; set; }

        public long AMCO_Id { get; set; }
        public long PACA_Id { get; set; }
        public Array ddoc { get; set; }
        public string status_all { get; set; }

        public string MI_SchoolCollegeFlag { get; set; }

        public Array prospectusPaymentlist { get; set; }
        public CollegePreadmissionstudnetDto[] data_arrayc { get; set; }

      

    }

  
}
