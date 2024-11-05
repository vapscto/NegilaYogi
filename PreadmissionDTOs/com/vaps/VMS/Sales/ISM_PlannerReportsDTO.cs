using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class ISM_PlannerReportsDTO : CommonParamDTO
    {
        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public string sound { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }
        public long? HRME_MobileNo { get; set; }
        public long UserId { get; set; }
        public long MI_Id_r { get; set; }
        public long roleId { get; set; }
        public string Auto_id { get; set; }
        public long ISMMAC_Id { get; set; }
        public long MI_Id { get; set; }
        public string TransNo { get; set; }
        public string html { get; set; }
        public DateTime? comp_Date { get; set; }
        public bool roleflag { get; set; }
        public string Role_flag { get; set; }
        public string HRME_Photo { get; set; }
        public string employeename { get; set; }
        public string deviation { get; set; }
        public string departmnt { get; set; }
        public string empdepartment { get; set; }
        public string empname { get; set; }
        public string desgination { get; set; }
        public string gradename { get; set; }
        public string approvedBy { get; set; }
        public string IVRMRT_Role { get; set; }
        public string emailid { get; set; }
        public string approvalstatus { get; set; }
        public string EmployeeEmail { get; set; }
        public bool type { get; set; }
        public string HRMEM_EmailId { get; set; }
        public long HRMEMNO_MobileNo { get; set; }
        public DateTime? ISMDRPT_Date { get; set; }
        public long IVRMRT_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long ISMMPR_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ISMDRF_RCV_HRME_Id { get; set; }
        public string ISMDRF_FeedBack { get; set; }
        public string Todate { get; set; }
        public string Fromdate { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public long IVRMM_Id { get; set; }
        public string IVRMM_ModuleName { get; set; }
        public string IVRMM_ModuleDesc { get; set; }
        public string HRME_AppDownloadedDeviceId { get; set; }
        public long ISMMMD_Id { get; set; }
        public long ISMMCLT_Id { get; set; }
        public string ISMMCLT_ClientName { get; set; }
        public long ISMTCR_Id { get; set; }
        public string ISMTCR_TaskNo { get; set; }
        public string ISMEMN_No { get; set; }
        public string ISMEMN_Date { get; set; }
        public string ISMEMN_CompleByDate { get; set; }
        public string ISMTCR_BugOREnhancementFlg { get; set; }
        public DateTime? ISMTCR_CreationDate { get; set; }
        public string ISMTCR_Title { get; set; }
        public long HRMPR_Id { get; set; }
        public string HRMP_Name { get; set; }
        public string AssignedBy { get; set; }
        public string ISMTCR_Desc { get; set; }
        public string ISMTCR_Status { get; set; }
        public bool ISMTCR_ReOpenFlg { get; set; }
        public DateTime? ISMTCR_ReOpenDate { get; set; }
        public bool ISMTCR_ActiveFlg { get; set; }
        public long getdetails { get; set; }
        public string PlannerFlag { get; set; }
        public string Deviationreporttype { get; set; }
        public string communicationfalg { get; set; }
        public string communicationmsg { get; set; }
        public long ISMTCRASTO_Id { get; set; }
        public DateTime ISMTCRASTO_AssignedDate { get; set; }
        public long ISMTCRASTO_AssignedBy { get; set; }
        public string ISMTCRASTO_Remarks { get; set; }
        public DateTime? ISMTCRASTO_StartDate { get; set; }
        public DateTime? ISMTCRASTO_EndDate { get; set; }
        public long ISMTCRASTO_EffortInHrs { get; set; }
        public bool ISMTCRASTO_ActiveFlg { get; set; }
        public string assignedby { get; set; }
        public string approvedby { get; set; }
        public string ISMTPLTA_EffortInHrsnew { get; set; }
        public string send_flg { get; set; }
        public long ISMTPLTA_Id { get; set; }
        public long ISMTPL_Id { get; set; }
        public string ISMTPLTA_Remarks { get; set; }
        public DateTime? ISMTPLTA_StartDate { get; set; }
        public DateTime? ISMTPLTA_EndDate { get; set; }
        public long ISMTPLTA_EffortInHrs { get; set; }
        public DateTime? ISMTPLTA_FinishedDate { get; set; }
        public string ISMTPLTA_Status { get; set; }
        public string ISMTPLTA_Statusnew { get; set; }
        public string taskStatus { get; set; }
        public bool ISMTPLTA_DeviationFlg { get; set; }
        public bool ISMTPLTA_ExtraFlg { get; set; }
        public long ISMTPLTA_TimeTakenInHrs { get; set; }
        public Array departmnthead { get; set; }
        public Array deviceArray { get; set; }
        public bool ISMTPLTA_ActiveFlg { get; set; }
        public long ISMTPLTA_CreatedBy { get; set; }
        public long ISMTPLTA_UpdatedBy { get; set; }
        public long ISMTPL_PlannedBy { get; set; }
        public string ISMTPL_PlannerName { get; set; }
        public string ISMTPL_Remarks { get; set; }
        public DateTime? ISMTPL_StartDate { get; set; }
        public DateTime? ISMTPL_EndDate { get; set; }
        public decimal ISMTPL_TotalHrs { get; set; }
        public bool ISMTPL_ApprovalFlg { get; set; }
        public long ISMTPL_ApprovedBy { get; set; }
        public bool ISMTPL_ActiveFlg { get; set; }
        public long ISMTPL_CreatedBy { get; set; }
        public long ISMTPL_UpdatedBy { get; set; }
        public DateTime plannerMaxdate { get; set; }
        public long ISMTPLAP_Id { get; set; }
        public string ISMTPLAP_Remarks { get; set; }
        public bool ISMTPLAP_ActiveFlg { get; set; }
        public long ISMTPLAP_CreatedBy { get; set; }
        public long ISMTPLAP_UpdatedBy { get; set; }
        public long ISMTPLAPTA_Id { get; set; }
        public string ISMTPLAPTA_Remarks { get; set; }
        public DateTime? ISMTPLAPTA_StartDate { get; set; }
        public DateTime? ISMTPLAPTA_EndDate { get; set; }
        public long ISMTPLAPTA_EffortInHrs { get; set; }
        public DateTime? ISMTPLAPTA_FinishedDate { get; set; }
        public string ISMTPLAPTA_Status { get; set; }
        public bool ISMTPLAPTA_DeviationFlg { get; set; }
        public bool ISMTPLAPTA_ExtraTaskFlg { get; set; }
        public long ISMTPLAPTA_TimeTakenInHrs { get; set; }
        public bool? ISMTPLAPTA_RejectedFlg { get; set; }
        public bool? ISMTPLAPTA_ApprovalFlg { get; set; }
        public bool ISMTPLAPTA_UnPlannedFlg { get; set; }
        public bool ISMTPLAPTA_ActiveFlg { get; set; }
        public long ISMTPLAPTA_CreatedBy { get; set; }
        public long ISMTPLAPTA_UpdatedBy { get; set; }
        public string plannerStatus { get; set; }
        public bool? flgapproved { get; set; }
        public bool? flgrejected { get; set; }
        public string taskstartdate { get; set; }
        public string taskatenddate  { get; set; }
        public long day_count { get; set; }
        public DateTime? startdate { get; set; }
        public DateTime? enddate { get; set; }
       // public long flag { get; set; }

        public string flag { get; set; }
        public string user { get; set; }
        public Array DepartmentList { get; set; }
        public Array Designation { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public Array get_taskregisterReport { get; set; }
        public Array Deviationpercentage  { get; set; }
        public DepartmentDTO[] departmentlist { get; set; }
        public DesignationDTO[] designationlist { get; set; }
        public EmplistforMNNEDTO[] EmplistforMNNE { get; set; }
        public Array get_Assignedtasklist { get; set; }
        public Array get_client { get; set; }
        public Array get_Status { get; set; }
        public Array holidaylist { get; set; }
        public Array get_employeelist { get; set; }
        public Array get_department { get; set; }
        public Array get_taskdetails { get; set; }
        public Array get_effortdetails { get; set; }
        public Array get_plannerdetails { get; set; }
        public Array get_plannerApprovaldetails { get; set; }
        public Array get_Attdetails { get; set; }
        public Array get_taskproject { get; set; }
        public Array get_taskmodule { get; set; }
        public Array get_plannerlist { get; set; }
        public Array get_extratasklist { get; set; }
        public Array get_deviationreport { get; set; }
        public Array get_emp_rating_report { get; set; }
        public Array getinstitution { get; set; }
        public Array get_plannerreport { get; set; }
        public Array get_userEmplist { get; set; }
        public Array get_taskresponse { get; set; }
        public Array get_taskattachments { get; set; }
        public Array get_responseattachments { get; set; }
        public arrayuserEmployeeDTO[] arrayuserEmp { get; set; }
        public feedbackdt[] feedbackdtl { get; set; }
        //************************************ Approval Report
        public int dateflag { get; set; }
        public bool feedbackflg { get; set; }
        public Array get_Plannerlist { get; set; }
        public Array get_approvalreport { get; set; }
        public Array get_emp_dailyreport { get; set; }
        public temp_PlannerDTO[] temp_Planner { get; set; }
    
        //********************************** Deviation Remarks Report
        public string deviationtype { get; set; }
        public string NoOfDays { get; set; }
        public arraydeviationStatusDTO[] arraydeviationStatus { get; set; }
        public Array get_deviationRemarksReport { get; set; }
        public Array Dept_deviationRemarksReport { get; set; }
        public string MailCc { get; set; }
        public string MailBCc { get; set; }
    }
    public class arrayuserEmployeeDTO
    {
        public long HRME_Id { get; set; }
        public long UserId { get; set; }

    }
    public class DepartmentDTO
    {
        public int HRMDC_ID { get; set; }
    }
    public class DesignationDTO
    {
        public long HRMDES_Id { get; set; }
    }
    public class plannerdetails
    {
        public string Deviation_Percentage { get; set; }
    }
    public class EmplistforMNNEDTO
    {
        public long HRME_Id { get; set; }
        public long MI_Id { get; set; }
        public plannerdetails[] plannerdetails { get; set; }
        public DateTime? comp_Date { get; set; }
        public string ISMEMN_No { get; set; }
        public string ISMEMN_Date { get; set; }
        public string ISMEMN_CompleByDate  { get; set; }
        public DateTime? start_Date { get; set; }
        public DateTime? end_Date  { get; set; }
    }
    public class arraydeviationStatusDTO
    {     
        public string deviationStatus { get; set; }

    }
    public class temp_PlannerDTO
    {
        public long HRME_Id { get; set; }
        public long ISMTPL_Id { get; set; }
    }

    public class feedbackdt
    {
        public long ISMDRF_RCV_HRME_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public long MI_Id_r { get; set; }
        public string ISMDRF_FeedBack { get; set; }
        public DateTime? ISMDRF_Feedback_DR_Date { get; set; }
    }
}