using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VisitorsManagement
{
    public class AddVisitorsDTO : CommonParamDTO
    {
        public long VMMV_Id { get; set; }
        public long VMAP_Id { get; set; }
        public long MI_Id { get; set; }
        public string VMMV_VisitorName { get; set; }
        public long VMMV_VisitorContactNo { get; set; }
        public string VMMV_VisitorEmailid { get; set; }
        public string VMMV_IdentityCardType { get; set; }
        public string VMMV_CardNo { get; set; }
        public string VMMV_CardImage { get; set; }
        public string VMMV_FromPlace { get; set; }
        public string VMMV_FromAddress { get; set; }
        public string VMMV_MeetingDuration { get; set; }
        public string VMMV_MeetingLocation { get; set; }
        public string VMMV_PersonToMeet { get; set; }
        public DateTime VMMV_MeetingDateTime { get; set; }
        public string VMMV_MeetingPurpose { get; set; }
        public string VMMV_PersonsAccompanying { get; set; }
        public string VMMV_AuthorisationBy { get; set; }
        public long VMMV_ToMeet { get; set; }
        public long UserId { get; set; }
        public string VMMV_EntryDateTime { get; set; }
        public string VMMV_ExitDateTime { get; set; }
        public DateTime? VMMV_ExitDate { get; set; }
        public string VMMV_VehicleNo { get; set; }
        public string VMMV_VisitTypeFlg { get; set; }
        public string VMMV_CkeckedInOutStatus { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public long HRME_Id { get; set; }
        public string VMMV_Remarks { get; set; }
        public string VMMV_VisitorPhoto { get; set; }
        public string VMMV_VisitorFingerPrint { get; set; }
        public bool VMMV_ActiveFlag { get; set; }
        public long? VMMV_UpdatedBy { get; set; }
        public long? VMMV_CreatedBy { get; set; }
        public long count_subvisitors { get; set; }
        public long count_documents { get; set; }
        public string returnVal { get; set; }
        public string empname { get; set; }
        public bool value { get; set; }
        public Array editDetails { get; set; }
        public Array editmultivisitor { get; set; }
        public Array edituploaddocument { get; set; }
        public Array editidcardDetails { get; set; }
        public Array editmutlivisitoridcardDetails { get; set; }
        public Array viewdocumentdetails { get; set; }
        public string msg { get; set; }
        public Array vis_list { get; set; }
        public Array institution { get; set; }
        public Array gridoptions { get; set; }
        public Array viewdetails { get; set; }
        public int count { get; set; }
        public long VMVAP_Id { get; set; }
        public Array emplist { get; set; }
        public Array getmultivisitorlist { get; set; }
        public long HRMEMNO_MobileNo { get; set; }
        public string HRMEM_EmailId { get; set; }
        public DateTime? createddate { get; set; }
        public bool? SMS_Required { get; set; }
        public bool? Email_Required { get; set; }
        public bool? SMS_Required_Update { get; set; }
        public bool? Email_Required_Update { get; set; }
        public multivisitor[] multivisitor { get; set; }
        public uploaddocments[] uploaddocments { get; set; }
        public int fhrors { get; set; }
        public int fminutes { get; set; }
        public int return_hh { get; set; }
        public int return_mm { get; set; }
        public bool? VMMV_IDCardReturnedFlg { get; set; }
        public bool? VMMV_BlocekFlg { get; set; }
        public DateTime? VMMV_IDCardReturnedDateTime { get; set; }
        public string VMMV_IDCardNo { get; set; }
        public Array getpreviousvisitorlist { get; set; }
        public Array getpreviousvisitordetails { get; set; }
        public Array getpreviousvisitor_multivisitors { get; set; }
        public Array getpreviousvisitor_documents { get; set; }
        public Array assigned_visitorlist { get; set; }
        public Array visitorlist { get; set; }
        public Array emplistautjorizedby { get; set; }
        public Array institutionlist { get; set; }
        public Array visitorassigndetails { get; set; }
        public Array getappvistiordetails { get; set; }
        public Array getAppointmentdetails { get; set; }
        public Array getAppointment_visitordetails { get; set; }
        public Array getAppointment_filesdetails { get; set; }
        public DateTime VMVTMT_DateTime { get; set; }
        public long VMVTMT_ToMeet_HRME_Id { get; set; }
        public string VMVTMT_Location { get; set; }
        public string VMVTMT_Remarks { get; set; }
        public long VMVTMT_Id { get; set; }
        public bool VMVTMT_MetFlg { get; set; }
        public bool VMMV_ExternalFlg { get; set; }
    }
    public class multivisitor
    {
        public long VMMVVI_Id { get; set; }
        public long VMMV_Id { get; set; }
        public string VMMVVI_VisitorName { get; set; }
        public string VMMVVI_VisitorAddress { get; set; }
        public string VMMVVI_VisitorEmailId { get; set; }
        public string VMMVVI_VisitorContactNo { get; set; }
        public string VMMVVI_Remarks { get; set; }
        public long VMMVVI_CreatedBy { get; set; }
        public DateTime VMMVVI_CreatedDate { get; set; }
        public long VMMVVI_UpdatedBy { get; set; }
        public DateTime VMMVVI_Updateddate { get; set; }
        public string VMMVVI_VisitorCardNo { get; set; }
        public string VMMVVI_DocumentUpload { get; set; }
        public string VMMVVI_VisitorPhoto { get; set; }
        public string VMMVVI_IDCardNo { get; set; }
        public bool? VMMVVI_IDCardReturnedFlg { get; set; }
        public bool? VMMVVI_BlocekFlg { get; set; }
        public DateTime? VMMVVI_IDCardReturnedDateTime { get; set; }
        public int? totimehr { get; set; }
        public int? totimemin { get; set; }
        public int? totimesec { get; set; }
    }
    public class uploaddocments
    {
        public long VMMVFL_Id { get; set; }
        public long VMMV_Id { get; set; }
        public string VMMVFL_FileName { get; set; }
        public string VMMVFL_FilePath { get; set; }
        public string VMMVFL_FileRemarks { get; set; }
        public bool? VMMVFL_ActiveFlg { get; set; }
    }
}