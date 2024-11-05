using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.Portals.IVRM;
using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.MobileApp
{
    public class AdmissionDTO
    {
        public class getAcademicyear
        {
            public long ASMAY_Id { get; set; }
            public long MI_Id { get; set; }
            public string ASMAY_Year { get; set; }
            public DateTime? ASMAY_From_Date { get; set; }
            public DateTime? ASMAY_To_Date { get; set; }
            public long ASMAY_Order { get; set; }
            public bool status { get; set; }
            public Array getAcaYrLst { get; set; }
        }

        public class ExamLibararyDTO
        {
            //public  
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public long AMCST_Id { get; set; } 
            public Array librarydetails { get; set; }
            public Array MonthDetails { get; set; }
          

            public string Flag { get; set; }
        }

        public class UserProfileDetailsDTO
        {
            public long MI_Id { get; set; }
            public long usercode { get; set; }
            public string type { get; set; }
            public Array getuserdetails { get; set; }

        }
        public class CLGUserProfileDetailsDTO
        {
            public long MI_Id { get; set; }
            public long usercode { get; set; }
            public long ASMAY_Id { get; set; }
            public long AMCST_Id { get; set; }
            public string type { get; set; }
            public string Roleflag { get; set; }
            public Array getuserdetails { get; set; }
            public Array Subtechaer { get; set; }

        }
        public class staffProfileDTO
        {
            public long MI_Id { get; set; }
            public long HRME_Id { get; set; }
            public Array staffdetails { get; set; }

        }
        public class getClass
        {
            public long ASMCL_Id { get; set; }
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public string ASMCL_ClassName { get; set; }
            public int ASMCL_Order { get; set; }
            public string ASMCL_ClassCode { get; set; }
            public int ASMCL_MaxCapacity { get; set; }
            public bool status { get; set; }
            public Array getclass { get; set; }

        }
        public class stdDashboardExam
        {
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public long AMST_Id { get; set; }
            public long ASMS_Id { get; set; }
            public long ASMCL_Id { get; set; }
            public long EME_Id { get; set; }
            public bool status { get; set; }


            public Array examgraphList { get; set; }

        }
        public class getSection
        {
            public long ASMS_Id { get; set; }
            public long MI_Id { get; set; }
            public string ASMC_SectionName { get; set; }
            public string ASMC_SectionCode { get; set; }
            public long ASMC_Order { get; set; }
            public int ASMC_MaxCapacity { get; set; }
            public bool status { get; set; }
            public Array getsection { get; set; }
            public long ASMAY_Id { get; set; }
            public long ASMCL_Id { get; set; }
        }

        public class getstudent
        {
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public long ASMCL_Id { get; set; }
            public long ASMS_Id { get; set; }
            public long AMST_Id { get; set; }
            public Array getstudentdetails { get; set; }
            public string AMST_FirstName { get; set; }
            public string AMST_MiddleName { get; set; }
            public string AMST_LastName { get; set; }
            public string AMST_AdmNo { get; set; }
            public string AMST_RegistrationNo { get; set; }
            public DateTime AMST_DOB { get; set; }
            public string AMST_BloodGroup { get; set; }
            public long? AMST_AadharNo { get; set; }
            public long AMST_MobileNo { get; set; }
            public string AMST_emailId { get; set; }
            public string AMST_Photoname { get; set; }
            public string AMST_Tpin { get; set; }
            public string ASMCL_ClassName { get; set; }
            public string ASMC_SectionName { get; set; }
            public string ASMAY_Year { get; set; }
            public string percentage { get; set; }
            public Array studentDue { get; set; }
        }



        public class CLGgetstudent
        {
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public long AMCST_Id { get; set; }
            public Array getstudentdetails { get; set; }
            public Array studentDue { get; set; }
        }

        //Praveen
        //Certificate Apply 
        public class getCertificateApply : CommonParamDTO
        {

            public long ASCA_Id { get; set; }
            public long MI_Id { get; set; }
            public long AMST_Id { get; set; }

            public long ASMAY_Id { get; set; }
            public string ASCA_Reason { get; set; }
            public DateTime ASCA_ApplyDate { get; set; }
            public string ASCA_Status { get; set; }
            public bool ASCA_ActiveFlg { get; set; }
            public string ASCAP_ApproveReason { get; set; }
            public DateTime ASCAP_ApproveDate { get; set; }


            public long ASMCL_Id { get; set; }
            public string AMST_FirstName { get; set; }
            public string ASMCL_ClassName { get; set; }
            public string ASMC_SectionName { get; set; }
            public long ASMS_Id { get; set; }
            public string AMST_RegistrationNo { get; set; }
            public string AMST_emailId { get; set; }
            public long AMST_MobileNo { get; set; }
            public Array certificate_dropdown { get; set; }
            public Array certificatelist { get; set; }

            public Array studentdetails { get; set; }
            public Array aply_aprvlist { get; set; }
            //public Array editdata { get; set; }
            public string AMST_AdmNo { get; set; }
            public string ASMAY_Year { get; set; }
            public string flag { get; set; }
            public long UserId { get; set; }
            public long IVRMRT_Id { get; set; }
            public string flag_Type { get; set; }
            public long ACERTAPP_Id { get; set; }
            public string ACERTAPP_CertificateName { get; set; }
            public string ACERTAPP_CertificateCode { get; set; }
            public Array get_certificate { get; set; }

            public Array totalcount { get; set; }

            public bool returnval { get; set; }
            //public string returnvalues { get; set; }
            public string roletype { get; set; }
            public long HRME_Id { get; set; }
            public Array studlist { get; set; }
            public Array applylist { get; set; }

        }
        public class daywiseTimetable
        {
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public long AMST_Id { get; set; }
            public long AMCST_Id { get; set; }
            public long ASMS_Id { get; set; }
            public long ASMCL_Id { get; set; }
            public bool status { get; set; }
            public string dayname { get; set; }
            public Array daywiseTimeTableList { get; set; }
            public Array ClgdaywiseTimeTableList { get; set; }

        }
        public class saveCertificateApply : CommonParamDTO
        {

            public long ASCA_Id { get; set; }
            public long MI_Id { get; set; }
            public long AMST_Id { get; set; }

            public long ASMAY_Id { get; set; }

            public string ASCA_CertificateType { get; set; }
            public string ASCA_Reason { get; set; }
            public string employeename { get; set; }

            public string message { get; set; }
            public DateTime ASCA_ApplyDate { get; set; }
            public string ASCA_Status { get; set; }
            public bool ASCA_ActiveFlg { get; set; }

            public string ASCAP_ApproveReason { get; set; }
            public DateTime ASCAP_ApproveDate { get; set; }


            public long ASMCL_Id { get; set; }
            public string ASMCL_ClassName { get; set; }
            public long ACERTAPP_Id { get; set; }
            public Array get_certificate { get; set; }
            public bool returnval { get; set; }
            //public string returnvalues { get; set; }
            public string roletype { get; set; }
            public long HRME_Id { get; set; }
            public Array studlist { get; set; }
            public Array applylist { get; set; }
            public bool duplicate { get; set; }
        }
        public class getCertificateDetails : CommonParamDTO
        {


            public long MI_Id { get; set; }
            public long AMST_Id { get; set; }

            public long ASMAY_Id { get; set; }

            public Array studlist { get; set; }

            public string roletype { get; set; }
            public string flag_Type { get; set; }


        }

        //Feedback
        public class getloadFeedbackdata
        {
            public long MI_Id { get; set; }
            public long AMST_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public Array instname { get; set; }
            public Array get_feedback { get; set; }

        }

        public class saveFeedbackFormDTO
        {
            public bool returnval { get; set; }
            public string message { get; set; }

            public long MI_Id { get; set; }
            public long ASGFE_Id { get; set; }
            public long AMST_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public long ASMCL_Id { get; set; }
            public long ASMS_Id { get; set; }
            public string ASGFE_FeedBack { get; set; }
            public string ASMCL_ClassName { get; set; }
            public DateTime? ASGFE_FeedbackDate { get; set; }
            public bool ASGFE_ActiveFlag { get; set; }
            public long ASGFE_CreatedBy { get; set; }
            public long ASGFE_UpdatedBy { get; set; }
            public Array instname { get; set; }
            public Array get_feedback { get; set; }



        }


        //Interaction 
        public class OnloadInteractionsDTO
        {
            public long UserId { get; set; }
            public string rolename { get; set; }
            public long AMST_Id { get; set; }
            public long HRME_Id { get; set; }
            public long userhrmE_Id { get; set; }
            public string AMST_AdmNo { get; set; }
            public long IVRMRT_Id { get; set; }
            public string Role_flag { get; set; }
            public Array getinboxmsg { get; set; }
            public Array getinboxmsg_readflg { get; set; }
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }

        }

        public class replyInteractionsDTO
        {
            public string composeedto { get; set; }
            public long UserId { get; set; }
            public string rolename { get; set; }
            public long asmclid { get; set; }
            public long asmsid { get; set; }
            public long AMST_Id { get; set; }
            public long HRME_Id1 { get; set; }
            public long HRME_Id { get; set; }
            public long userhrmE_Id { get; set; }
            public string AMST_AdmNo { get; set; }
            public long ASMCL_Id { get; set; }
            public string ASMCL_ClassName { get; set; }
            public long ASMS_Id { get; set; }
            public string ASMC_SectionName { get; set; }
            public long IVRMRT_Id { get; set; }
            public Array subteacherlist { get; set; }
            public Array typelistrole { get; set; }
            public Array typelist { get; set; }
            public Array classteacherlist { get; set; }
            public string Role_flag { get; set; }
            public Array roletype { get; set; }
            public Array configflag { get; set; }
            public Array viewmessage { get; set; }
            public Array getinboxmsg { get; set; }
            public Array getinboxmsg_readflg { get; set; }
            public Array get_msgcreator { get; set; }
            public string[] images_paths { get; set; }
            public long ISMINT_Id { get; set; }
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public string ISMINT_ComposedByFlg { get; set; }
            public string notificationflag { get; set; }
        }

        public class replysaveInteractionsDTO
        {
            public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }
            public List<replysaveInteractionsDTO> devicelist12 { get; set; }
            public bool returnval { get; set; }
            public string message { get; set; }
            public string composeedto { get; set; }
            public long UserId { get; set; }
            public string rolename { get; set; }
            public bool IVRMGC_EnableSTIntFlg { get; set; }
            public bool IVRMGC_EnableCTIntFlg { get; set; }
            public bool IVRMGC_EnableHODIntFlg { get; set; }
            public bool IVRMGC_EnablePrincipalIntFlg { get; set; }
            public bool IVRMGC_EnableASIntFlg { get; set; }
            public long asmclid { get; set; }
            public long asmsid { get; set; }
            public long AMST_Id { get; set; }
            public long HRME_Id1 { get; set; }
            public long HRME_Id { get; set; }
            public long userhrmE_Id { get; set; }
            public long? HRME_MobileNo { get; set; }
            public long? AMST_MobileNo { get; set; }
            public string AMST_AdmNo { get; set; }
            public long ASMCL_Id { get; set; }
            public string ASMCL_ClassName { get; set; }
            public long ASMS_Id { get; set; }
            public string ASMC_SectionName { get; set; }
            public long IVRMRT_Id { get; set; }
            public Array subteacherlist { get; set; }
            public Array typelistrole { get; set; }
            public Array typelist { get; set; }
            public Array classteacherlist { get; set; }
            public string Role_flag { get; set; }
            public Array roletype { get; set; }
            public Array configflag { get; set; }
            public Array viewmessage { get; set; }
            public Array getinboxmsg { get; set; }
            public Array getinboxmsg_readflg { get; set; }
            public Array get_msgcreator { get; set; }
            public string[] images_paths { get; set; }
            public string employeeName { get; set; }
            public string studentName { get; set; }
            public long ISMINT_Id { get; set; }
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public string ISMINT_ComposedByFlg { get; set; }
            public string ISMINT_GroupOrIndFlg { get; set; }
            public long ISTINT_Id { get; set; }
            public long ISTINT_ToId { get; set; }
            public string ISTINT_ToFlg { get; set; }
            public long ISTINT_ComposedById { get; set; }
            public string ISTINT_Interaction { get; set; }
            public DateTime ISTINT_DateTime { get; set; }
            public string ISTINT_ComposedByFlg { get; set; }
            public int ISTINT_InteractionOrder { get; set; }
            public string HRME_AppDownloadedDeviceId { get; set; }
            public string AMST_AppDownloadedDeviceId { get; set; }
            public string AppDownloadedDeviceId { get; set; }
            public string notificationflag { get; set; }
            public Array deviceids { get; set; }
            public Array deviceidGrp { get; set; }
            public string ISMINT_ISPIPAddress { get; set; }
            public string ISMINT_MACAddress { get; set; }


        }

        public class ComposeOnloadInteractionsDTO
        {
            public bool returnval { get; set; }
            public string message { get; set; }
            public string userflag { get; set; }

            public long UserId { get; set; }
            public long asmclid { get; set; }
            public long asmsid { get; set; }
            public long AMST_Id { get; set; }
            public long HRME_Id { get; set; }
            public long? HRME_MobileNo { get; set; }
            public long? AMST_MobileNo { get; set; }
            public string AMST_AdmNo { get; set; }
            public long ASMCL_Id { get; set; }
            public string ASMCL_ClassName { get; set; }
            public long ASMS_Id { get; set; }
            public string ASMC_SectionName { get; set; }
            public long IVRMRT_Id { get; set; }

            public Array configflag { get; set; }
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public string roleflg { get; set; }
            public Array getdetails { get; set; }


        }

        public class composeOnselectOFTeacher
        {
            public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }
            public List<replysaveInteractionsDTO> devicelist12 { get; set; }
            public bool returnval { get; set; }
            public string message { get; set; }
            public string composeedto { get; set; }

            public long UserId { get; set; }
            public string userflag { get; set; }
            public string rolename { get; set; }
            public bool IVRMGC_EnableSTIntFlg { get; set; }
            public bool IVRMGC_EnableCTIntFlg { get; set; }
            public bool IVRMGC_EnableHODIntFlg { get; set; }
            public bool IVRMGC_EnablePrincipalIntFlg { get; set; }
            public bool IVRMGC_EnableASIntFlg { get; set; }
            public long asmclid { get; set; }
            public long asmsid { get; set; }
            public long AMST_Id { get; set; }
            public long HRME_Id1 { get; set; }
            public long HRME_Id { get; set; }
            public long userhrmE_Id { get; set; }
            public long? HRME_MobileNo { get; set; }
            public long? AMST_MobileNo { get; set; }
            public string AMST_AdmNo { get; set; }
            public long ASMCL_Id { get; set; }
            public string ASMCL_ClassName { get; set; }
            public long ASMS_Id { get; set; }
            public string ASMC_SectionName { get; set; }
            public long IVRMRT_Id { get; set; }
            public Array subteacherlist { get; set; }
            public Array typelistrole { get; set; }
            public Array typelist { get; set; }
            public Array classteacherlist { get; set; }
            public string Role_flag { get; set; }
            public Array roletype { get; set; }
            public Array configflag { get; set; }
            public Array viewmessage { get; set; }
            public Array getinboxmsg { get; set; }
            public Array getinboxmsg_readflg { get; set; }
            public Array get_msgcreator { get; set; }
            public string[] images_paths { get; set; }
            public string employeeName { get; set; }
            public string studentName { get; set; }
            public long ISMINT_Id { get; set; }
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public string ISMINT_ComposedByFlg { get; set; }
            public string ISMINT_GroupOrIndFlg { get; set; }
            public long ISTINT_Id { get; set; }
            public long ISTINT_ToId { get; set; }
            public string ISTINT_ToFlg { get; set; }
            public long ISTINT_ComposedById { get; set; }
            public string ISTINT_Interaction { get; set; }
            public DateTime ISTINT_DateTime { get; set; }
            public string ISTINT_ComposedByFlg { get; set; }
            public int ISTINT_InteractionOrder { get; set; }
            public string HRME_AppDownloadedDeviceId { get; set; }
            public string AMST_AppDownloadedDeviceId { get; set; }
            public string AppDownloadedDeviceId { get; set; }
            public string roleflg { get; set; }
            public string notificationflag { get; set; }

            public Array getdetails { get; set; }
            public Array deviceids { get; set; }
            public Array deviceidGrp { get; set; }
            public string ISMINT_ISPIPAddress { get; set; }
            public string ISMINT_MACAddress { get; set; }

        }
        public class composeOnselectOFStudent
        {
            public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }
            public List<replysaveInteractionsDTO> devicelist12 { get; set; }
            public bool returnval { get; set; }
            public string message { get; set; }
            public string composeedto { get; set; }

            public long UserId { get; set; }
            public string userflag { get; set; }
            public string rolename { get; set; }
            public bool IVRMGC_EnableSTIntFlg { get; set; }
            public bool IVRMGC_EnableCTIntFlg { get; set; }
            public bool IVRMGC_EnableHODIntFlg { get; set; }
            public bool IVRMGC_EnablePrincipalIntFlg { get; set; }
            public bool IVRMGC_EnableASIntFlg { get; set; }

            public long asmclid { get; set; }
            public long asmsid { get; set; }
            public long AMST_Id { get; set; }
            public long HRME_Id1 { get; set; }
            public long HRME_Id { get; set; }
            public long userhrmE_Id { get; set; }
            public long? HRME_MobileNo { get; set; }
            public long? AMST_MobileNo { get; set; }
            public string AMST_AdmNo { get; set; }
            public long ASMCL_Id { get; set; }
            public string ASMCL_ClassName { get; set; }
            public long ASMS_Id { get; set; }
            public string ASMC_SectionName { get; set; }
            public long IVRMRT_Id { get; set; }
            public Array subteacherlist { get; set; }
            public Array typelistrole { get; set; }
            public Array typelist { get; set; }
            public Array classteacherlist { get; set; }


            public string Role_flag { get; set; }
            public Array roletype { get; set; }
            public Array configflag { get; set; }
            public Array viewmessage { get; set; }

            public Array getinboxmsg { get; set; }
            public Array getinboxmsg_readflg { get; set; }
            public Array get_msgcreator { get; set; }
            public string[] images_paths { get; set; }


            public string employeeName { get; set; }
            public string studentName { get; set; }

            public long ISMINT_Id { get; set; }
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public string ISMINT_ComposedByFlg { get; set; }
            public string ISMINT_GroupOrIndFlg { get; set; }
            public long ISTINT_Id { get; set; }
            public long ISTINT_ToId { get; set; }
            public string ISTINT_ToFlg { get; set; }
            public long ISTINT_ComposedById { get; set; }
            public string ISTINT_Interaction { get; set; }
            public DateTime ISTINT_DateTime { get; set; }
            public string ISTINT_ComposedByFlg { get; set; }
            public int ISTINT_InteractionOrder { get; set; }
            public string HRME_AppDownloadedDeviceId { get; set; }
            public string AMST_AppDownloadedDeviceId { get; set; }
            public string AppDownloadedDeviceId { get; set; }
            public string roleflg { get; set; }
            public string notificationflag { get; set; }

            public Array getdetails { get; set; }
            public Array deviceids { get; set; }
            public Array deviceidGrp { get; set; }
            public string ISMINT_ISPIPAddress { get; set; }
            public string ISMINT_MACAddress { get; set; }


        }


        public class composeOnsubmitOFStudent
        {
            public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }
            public List<composeOnsubmitOFStudent> devicelist12 { get; set; }
            public string trans_id { get; set; }
            //public bool? IVRMGC_GMRDTOALLFlg { get; set; }
            //public string interactionid { get; set; }
            //public string returnduplicatestatus { get; set; }
            public bool returnval { get; set; }
            //public bool already_cnt { get; set; }
            public string message { get; set; }
            public string composeedto { get; set; }

            public long UserId { get; set; }
            public string userflag { get; set; }
            public string rolename { get; set; }
            public bool IVRMGC_EnableSTIntFlg { get; set; }
            public bool IVRMGC_EnableCTIntFlg { get; set; }
            public bool IVRMGC_EnableHODIntFlg { get; set; }
            public bool IVRMGC_EnablePrincipalIntFlg { get; set; }
            public bool IVRMGC_EnableASIntFlg { get; set; }

            public long asmclid { get; set; }
            public long asmsid { get; set; }
            public long AMST_Id { get; set; }
            public long HRME_Id1 { get; set; }
            public long student_Id { get; set; }
            public long HRME_Id { get; set; }
            public long userhrmE_Id { get; set; }
            public long? HRME_MobileNo { get; set; }
            public long? AMST_MobileNo { get; set; }
            public long employee_Id { get; set; }
            public string AMST_AdmNo { get; set; }
            public long ASMCL_Id { get; set; }
            public string ASMCL_ClassName { get; set; }
            public long ASMS_Id { get; set; }
            public string ASMC_SectionName { get; set; }
            public long IVRMRT_Id { get; set; }
            //public string AMST_FirstName { get; set; }
            //public bool IVRMGC_EnableECIntFlg { get; set; }
            //public Array studentlist { get; set; }
            public Array subteacherlist { get; set; }
            public Array typelistrole { get; set; }
            public Array typelist { get; set; }
            public Array classteacherlist { get; set; }


            public string Role_flag { get; set; }
            public Array roletype { get; set; }
            //public Array roletype2 { get; set; }
            public Array configflag { get; set; }

            //public Array get_replies { get; set; }
            public Array viewmessage { get; set; }

            public Array getinboxmsg { get; set; }
            public Array getinboxmsg_readflg { get; set; }
            public Array get_msgcreator { get; set; }
            //public long inboxcount { get; set; }

            //public Array staffwiseclasslist { get; set; }
            //public Array get_student { get; set; }
            public string[] images_paths { get; set; }


            public string employeeName { get; set; }
            public string studentName { get; set; }

            public long ISMINT_Id { get; set; }
            public long MI_Id { get; set; }
            //public long ISMINT_InteractionId { get; set; }
            public long ASMAY_Id { get; set; }
            public string ISMINT_ComposedByFlg { get; set; }
            public string ISMINT_GroupOrIndFlg { get; set; }
            //public long ISMINT_ComposedById { get; set; }
            public string ISMINT_Subject { get; set; }
            //public DateTime ISMINT_DateTime { get; set; }
            public string ISMINT_Interaction { get; set; }
            //public bool ISMINT_ActiveFlag { get; set; }
            //public long ISMINT_CreatedBy { get; set; }
            //public long ISMINT_UpdatedBy { get; set; }

            public long ISTINT_Id { get; set; }
            public long ISTINT_ToId { get; set; }
            public string ISTINT_ToFlg { get; set; }
            public long ISTINT_ComposedById { get; set; }
            public string ISTINT_Interaction { get; set; }
            public DateTime ISTINT_DateTime { get; set; }
            public string ISTINT_ComposedByFlg { get; set; }
            public int ISTINT_InteractionOrder { get; set; }
            //public bool ISTINT_ActiveFlag { get; set; }
            //public long ISTINT_CreatedBy { get; set; }
            //public long ISTINT_UpdatedBy { get; set; }
            public string HRME_AppDownloadedDeviceId { get; set; }
            public string AMST_AppDownloadedDeviceId { get; set; }
            public string AppDownloadedDeviceId { get; set; }
            public string roleflg { get; set; }
            public string notificationflag { get; set; }

            public Array getdetails { get; set; }
            public Array deviceids { get; set; }
            public Array deviceidGrp { get; set; }
            //public Array deviceidInd { get; set; }
            //public arraySTDTO[] arrayST { get; set; }
            public arrayStudentDTO[] arrayStudent { get; set; }
            //public arrayStudentDTO1[] arrayStudent1 { get; set; }
            public arrayTeachersDTO[] arrayTeachers { get; set; }
            //public bool? ISTINT_ReadFlg { get; set; }


            // clg

            //public long AMCST_Id { get; set; }
            //public long amcoid { get; set; }
            //public long ambid { get; set; }
            //public long amseid { get; set; }
            //public long acmsid { get; set; }
            //public long AMCO_Id { get; set; }
            //public long AMB_Id { get; set; }
            //public long AMSE_Id { get; set; }
            //public string AMB_BranchName { get; set; }
            //public string AMCO_CourseName { get; set; }
            //public string AMSE_SEMName { get; set; }
            //public Array branchList { get; set; }
            //public string AMB_BranchCode { get; set; }
            //public int AMB_Order { get; set; }
            //public Array semesterList { get; set; }
            //public string AMSE_SEMCode { get; set; }
            //public int AMSE_SEMOrder { get; set; }
            //public long? AMCST_MobileNo { get; set; }
            //public string AMCST_AppDownloadedDeviceId { get; set; }
            //public Array sectionList { get; set; }
            //public long ACMS_Id { get; set; }
            //public string ACMS_SectionName { get; set; }
            //public int ACMS_Order { get; set; }

            public string ISMINT_ISPIPAddress { get; set; }
            public string ISMINT_MACAddress { get; set; }
        }

        //add by kavita
        public class ttLoadData
        {
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public long AMST_Id { get; set; }

            public bool status { get; set; }
            public Array getyear { get; set; }
            public long ASMCL_Id { get; set; }
            public string ASMCL_ClassName { get; set; }
            public long ASMS_Id { get; set; }
            public string ASMC_SectionName { get; set; }
            public string ASMAY_Year { get; set; }
        }
        public class ttGetStudent
        {
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public long AMST_Id { get; set; }
            public long ASMCL_Id { get; set; }
            public long ASMS_Id { get; set; }
            public string ASMCL_ClassName { get; set; }
            public string ASMC_SectionName { get; set; }
            public bool status { get; set; }
            public Array alldata { get; set; }
            public Array TT { get; set; }
            public Array TT_Break_list { get; set; }
            public Array TT_Break_list_all { get; set; }
            public Array Break_list { get; set; }
            public Array Break_list_all { get; set; }
            public Array getStudentTT { get; set; }
            public Array periodslst { get; set; }
            public Array gridweeks { get; set; }
            public decimal TTMB_AfterPeriod { get; set; }
            public string type { get; set; }
            public string staffName { get; set; }
            public string ISMS_SubjectName { get; set; }
            public string TTMC_CategoryName { get; set; }
            public string TTMP_PeriodName { get; set; }
            public string TTMD_DayName { get; set; }
            public long TTMC_Id { get; set; }
            public long TTMP_Id { get; set; }
            public long TTMD_Id { get; set; }
            public long ISMS_Id { get; set; }
            public long HRME_Id { get; set; }
            public long TTFG_Id { get; set; }
            public long TTFGD_Id { get; set; }
        }
        //class wise timetable

        //Attendance
        public class attGetLoadData
        {
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public long AMST_Id { get; set; }
            public bool status { get; set; }
            public Array currentyear { get; set; }
            public Array attyearlist { get; set; }
            public string ASMAY_Year { get; set; }
            public int ASMAY_Order { get; set; }
        }

        public class attGetdetails
        {
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public long AMST_Id { get; set; }
            public bool status { get; set; }

            public Array attList { get; set; }
        }

        //added by sanjeev
        public class onclick_LIB
        {
            public bool status { get; set; }
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public long AMST_Id { get; set; }
            public long UserId { get; set; }
            public long RoleId { get; set; }
            public string flag { get; set; }
            public Array librarydetails { get; set; }
            public Array librarygraphs { get; set; }
        }
        public class getCoedata
        {
            public long AMST_Id { get; set; }
            public long mI_ID { get; set; }
            public long ASMAY_Id { get; set; }
            public bool status { get; set; }
            public string ASMAY_Year { get; set; }
            public int ASMAY_Order { get; set; }
            public Array currentyear { get; set; }
            public Array coeyearlist { get; set; }
            public string ASMC_SectionName { get; set; }
            public string ASMCL_ClassName { get; set; }
            public long ASMS_Id { get; set; }
            public long ASMCL_Id { get; set; }
            public long month { get; set; }
            public string COEME_EventName { get; set; }
            public string COEME_EventDesc { get; set; }
            public DateTime? COEE_EStartDate { get; set; }
            public DateTime? COEE_EEndDate { get; set; }
            public string COEEI_Images { get; set; }
            public string COEEV_Videos { get; set; }
            public string COEE_EEndTime { get; set; }
            public Array coereportlist { get; set; }

            public string COEE_EStartTime { get; set; }

        }

        //public class arraySTDTO
        //{
        //    public long HRME_Id { get; set; }
        //}
        //public class arrayStudentDTO
        //{
        //    public long AMST_Id { get; set; }
        //}
        //public class arrayStudentDTO1
        //{
        //    public long AMCST_Id { get; set; }
        //}


        //public class arrayTeachersDTO
        //{
        //    public long HRME_Id { get; set; }
        //}
        //public class numberingdto
        //{
        //    public long IMN_Id { get; set; }
        //    public long MI_Id { get; set; }
        //    public string IMN_AutoManualFlag { get; set; }
        //    public string IMN_DuplicatesFlag { get; set; }
        //    public string IMN_StartingNo { get; set; }
        //    public string IMN_WidthNumeric { get; set; }
        //    public string IMN_ZeroPrefixFlag { get; set; }
        //    public bool IMN_PrefixAcadYearCode { get; set; }
        //    public string IMN_PrefixParticular { get; set; }
        //    public bool IMN_SuffixAcadYearCode { get; set; }
        //    public string IMN_SuffixParticular { get; set; }
        //    public string IMN_RestartNumFlag { get; set; }
        //    public string IMN_Flag { get; set; }
        //    public bool? IMN_PrefixFinYearCode { get; set; }
        //    public bool? IMN_PrefixCalYearCode { get; set; }
        //    public bool? IMN_SuffixFinYearCode { get; set; }
        //    public bool? IMN_SuffixCalYearCode { get; set; }

        //}



        //added by roopa



        public class getAcaclass
        {
            public long ASMAY_Id { get; set; }
            public long MI_Id { get; set; }
            public DateTime asmaY_From_Date { get; set; }
            public DateTime asmaY_To_Date { get; set; }
            public long asmaY_Order { get; set; }
            public bool status { get; set; }
        }

        public class getAcaClaSec
        {
            public long ASMAY_Id { get; set; }
            public long MI_Id { get; set; }
            public DateTime asmaY_From_Date { get; set; }
            public DateTime asmaY_To_Date { get; set; }
            public long asmaY_Order { get; set; }
            public bool status { get; set; }
        }

        public class getStudent
        {
            public long ASMAY_Id { get; set; }
            public long MI_Id { get; set; }
            public DateTime asmaY_From_Date { get; set; }
            public DateTime asmaY_To_Date { get; set; }
            public long asmaY_Order { get; set; }
            public bool status { get; set; }
        }

        //classwork

        public class onclickClasswork
        {
            public long ASMAY_Id { get; set; }
            public long MI_Id { get; set; }
            public bool status { get; set; }
            public string messag { get; set; }

            public long AMST_Id { get; set; }
            public Array yearlist { get; set; }
            public Array studetailslist { get; set; }
            public Array assignmentlist { get; set; }
        }

        //homework


        public class onclick_Homework_load
        {
            public long ASMAY_Id { get; set; }
            public long MI_Id { get; set; }

            public bool status { get; set; }
            public string messag { get; set; }

            public long AMST_Id { get; set; }
            public Array yearlist { get; set; }
            public Array homeworklist { get; set; }
        }


        //notice

        public class onclick_notice
        {
            public long ASMAY_Id { get; set; }
            public long MI_Id { get; set; }
            public bool status { get; set; }
            public string messag { get; set; }
            public string flag { get; set; }
            public long AMST_Id { get; set; }
            public Array yearlist { get; set; }
            public Array homeworklist { get; set; }
            public Array noticelist { get; set; }


        }

        // upload classwork
        public class HomeworkkUpload
        {
            public long ASMAY_Id { get; set; }
            public long MI_Id { get; set; }
            public bool status { get; set; }
            public string messag { get; set; }
            public string flag { get; set; }
            public long AMST_Id { get; set; }
            public Array yearlist { get; set; }
            public Array homeworklist { get; set; }
            public Array noticelist { get; set; }
            public ICollection<IFormFile> File { get; set; }


        }






        public class filedetails
        {
            public string name { get; set; }
            public string path { get; set; }
        }


        public class getClassworksave
        {
            public long ICW_Id { get; set; }
            public long AMST_Id { get; set; }
            public long MI_Id { get; set; }

            public uploaddoc_array1[] uploaddoc_array { get; set; }
            public bool returnval { get; set; }
        }

        public class uploaddoc_array1
        {
            public string DCO_Attachment { get; set; }
            public string Doc_FileName { get; set; }
        }


        public class gethomeworksave
        {
            public long IHW_Id { get; set; }
            public long AMST_Id { get; set; }
            public long MI_Id { get; set; }

            public uploaddoc_array1[] uploaddoc_array { get; set; }
            public bool returnval { get; set; }
        }
        public class stdDashboardLoad
        {
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public long AMST_Id { get; set; }
            public long ASMS_Id { get; set; }
            public long ASMCL_Id { get; set; }
            public bool status { get; set; }

            public Array birthdayList { get; set; }
            public Array calList { get; set; }
            public Array attendanceList { get; set; }
            public Array feesList { get; set; }
            public Array timeTableList { get; set; }
            public Array examList { get; set; }

        }

        public class userDashboardLoad
        {
            //staff
            public long MI_Id { get; set; }
            public long UserId { get; set; }
            public long ASMAY_Id { get; set; }
            public long leavecount { get; set; }
            public long ASMCL_Id { get; set; }
            public long ASMS_Id { get; set; }
            public long AMST_Id { get; set; }
            public string ASMAY_Year { get; set; }
            public string RoleType { get; set; }
            public Array leaveDetails { get; set; }
            public Array timeTableDetails { get; set; }
            public Array lopDetails { get; set; }
            public Array punchDetails { get; set; }
            public Array calList { get; set; }
            public Array ManagerDashboardLeaveDetails { get; set; }
            public Array ManagerDashboardFeesDetails { get; set; }
            public Array ManagerDashboardPreadmissionDetails { get; set; }
            public Array ManagerdashFeetotal { get; set; }
            public Array birthdayList { get; set; }
            public Array attendanceList { get; set; }
            public Array feesList { get; set; }
            public Array timeTableList { get; set; }
            public Array examList { get; set; }


        }

        public class staffDashboardLoad
        {
            public long MI_Id { get; set; }
            public long UserId { get; set; }
            public Array leaveDetails { get; set; }
            public Array timeTableDetails { get; set; }
            public Array lopDetails { get; set; }
            public Array punchDetails { get; set; }
            public Array calList { get; set; }
        }
        public class ManagerDashboard
        {
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public long UserId { get; set; }
            public long leavecount { get; set; }
            public Array ManagerDashboardCOE { get; set; }
            public Array ManagerDashboardLeaveDetails { get; set; }
            public Array ManagerDashboardFeesDetails { get; set; }
            public Array ManagerDashboardPreadmissionDetails { get; set; }
            public Array get_leavestatus { get; set; }
            public bool status { get; set; }
            public string ASMAY_Year { get; set; }

            public Array ManagerdashFeetotal { get; set; }
        }
        public class CollegeUserDashboardDetails
        {
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public long roleid { get; set; }
            public long roleId { get; set; }
            public long AMCST_Id { get; set; }
            public long AMCO_Id { get; set; }
            public long AMB_Id { get; set; }
            public long AMSE_Id { get; set; }
            public long UserId { get; set; }
            public long leavecount { get; set; }
            public string RoleType { get; set; }
            public string ASMAY_Year { get; set; }
            //public string mobileprivileges { get; set; }
            public Array yearlist { get; set; }
            public Array studentdetails { get; set; }
            public Array attendanceList { get; set; }
            public Array feesList { get; set; }
            public Array birthdayList { get; set; }
            public Array noticeboard { get; set; }
            public Array calList { get; set; }
           // public Array calenderlist { get; set; }
            public Array libraryList { get; set; }
            public Array examList { get; set; }
            public Array timeTableList { get; set; }
            public Array leaveDetails { get; set; }
            public Array lopDetails { get; set; }
            public Array punchDetails { get; set; }
            public Array ManagerDashboardCOE { get; set; }
            public Array ManagerDashboardLeaveDetails { get; set; }
            public Array ManagerDashboardFeesDetails { get; set; }
            public Array ManagerDashboardPreadmissionDetails { get; set; }

            // public Array Staffmobileappprivileges { get; set; }
        }
        public class PushNotification
        {
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public long Id { get; set; }
            public Array GetDetails { get; set; }
        }
        public  class NoticeBoadCollegeDto
        {
            public string Typeflg { get; set; }
            public long AMCST_Id { get; set; }
            public long MI_Id { get; set; }
            public long HRME_id { get; set; }
            public Array NoticeBoardlist { get; set; }
            public long ASMAY_Id { get; set; }
            public long INTB_Id { get; set; }

            public DateTime? fromdate { get; set; }
            public DateTime? todate { get; set; }

            public Array NoticeBoardlistdatewsie { get; set; }
            public Array staffNoticeBoardlist { get; set; }
            public Array StaffNoticeBoardlistdatewsie { get; set; }
          //  public Array StaffNoticeBoardlist { get; set; }
        }
        public class PushNotificationonload
        {
            public long userid { get; set; }
            public string flag { get; set; }
            public string retrunMsg { get; set; }
            public long MI_Id { get; set; }
            public long PNSD_Id { get; set; }
            public long PNSDDE_Id { get; set; }
            public long ReadFlg { get; set; }
            public Array getpushnotifications { get; set; }
        }

        public class ShortageOFAttandence
        {
            public long userid { get; set; }
            public string retrunMsg { get; set; }
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public long AMST_Id { get; set; }
            public long AMCST_Id { get; set; }

            public Array studentAttendanceList { get; set; }
            public Array clgstudentAttendanceList { get; set; }
        }




        public class AcademicFeesData
        {
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public long ASMCL_Id { get; set; }
            public bool status { get; set; }
            public Array AcademicFeesdetails { get; set; }
            public Array AcademicclasswiseFeesdetails { get; set; }
            public Array Versiondeatils { get; set; }
            
        }

        public class versiondetails
        {
            public long MI_Id { get; set; }
            public string version { get; set; }
          
            public bool status { get; set; }
            public Array versiondetailslist { get; set; }
        }


        public class ClgTimeTable
        {
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public long UserId { get; set; }
            public long TTMD_Id { get; set; }
            public string AMCO_CourseName { get; set; }
            public string AMB_BranchName { get; set; }
            public string DayName { get; set; }
            public int PeriodCount { get; set; }
            public Array TT_final_generation { get; set; }
            public Array class_sectons { get; set; }
            public Array allperiods { get; set; }
            public Array periods { get; set; }
        }

        public class clgfeedbackDetailsDTO
        {

            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public long AMCST_Id { get; set; }
            public Array feedbackdetails { get; set; }

        }
    }
}
