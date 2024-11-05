using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace PreadmissionDTOs.com.vaps.VMS.Exit
{
    public class ISM_Resignation_DTO
    {
        public long ISMRESG_Id { get; set; }
        public long ISMRESG_Id1 { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime ISMRESG_ResignationDate { get; set; }
        public long ISMRESGMRE_Id { get; set; }
        public long ISMRESG_NoticePeriod { get; set; }
        public DateTime ISMRESG_TentativeLeavingDate { get; set; }
        public string ISMRESG_Remarks { get; set; }
        public string ISMRESG_MgmtApprRejFlg { get; set; }
        public DateTime? ISMRESG_AccRejDate { get; set; }
        public string ISMRESG_ManagementRemarks { get; set; }
        public bool ISMRESG_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long ISMRESG_CreatedBy { get; set; }
        public long ISMRESG_UpdatedBy { get; set; }
        public long userId { get; set; }
        public string employeename { get; set; }
        public string employeename1 { get; set; }
        public string ISMRESGMRE_ResignationReasons { get; set; }
        public string returnval { get; set; }
        public DateTime? HRME_DOJ { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string DepartmentCodeName { get; set; }
        public long HRMD_Id { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public long HRMDES_Id { get; set; }
        public bool ISMRESGCL_ActiveFlag { get; set; }
        public string ISMRESGMCL_CheckListName { get; set; }
        public long ISMRESGMCL_Id { get; set; }
        public string return_p { get; set; }
        public string MI_Name { get; set; }
        public long ISMRESG_Status_Flg { get; set; }
        public DateTime ISMRESGRL_RLDate { get; set; }
        public bool ISMRESGRL_ActiveFlag { get; set; }
        public long ISMRESG_Print_Flg { get; set; }
        public DateTime FROMDATE { get; set; }
        public DateTime TODATE { get; set; }
     
        public long ACCEPT { get; set; }
        public long REJECT { get; set; }
        public Array r_ismsesg_id { get; set; }
        public Array delete_date { get; set; }
        public string imagepath { get; set; }
        

        //============================================

        public Array employee_list_dd { get; set; }
        public Array exit_employee_list { get; set; }
        public Array reason_list_dd { get; set; }
        public Array relieving_emp_dd { get; set; }
        public Array relieving_emp_dd1 { get; set; }
        public Array getrelievingchecklist { get; set; }
        public Array exit_employee_details { get; set; }
        public Array getrelievingchecklist1 { get; set; }
        public Array doc_list { get; set; }
        public Array savedata_print_proc { get; set; }
        public Array relieving_check_list { get; set; }
        public Array relieving_check_edit { get; set; }
        public Array relieving_check_edit_dd { get; set; }
        public Array exit_print_list { get; set; }
        public Array Exit_employee_print_report { get; set; }
        public Array department_list_R { get; set; }
        public Array designation_list_R { get; set; }
        public Array exi_employee_print_list { get; set; }
        public Array exit_print_list1 { get; set; }
        public Array exit_print_list2 { get; set; }
        public Array download_photo { get; set; }
      
        public doc_list_new [] doc_list2 { get; set; }
        public ISM_Resignation_DTO[] selectedept_list { get; set; }
        public ISM_Resignation_DTO[] selectedesig_list { get; set; }
        public Array employeedata { get; set; }
        public Array companydetails { get; set; }
        public long HRME_MobileNo { get; set; }
        public string HRME_EmailId { get; set; }
        public string HRME_PerStreet { get; set; }
        public string HRME_PerArea { get; set; }
        public string HRME_PerCity { get; set; }
        public long? HRME_PerStateId { get; set; }
        public long? HRME_PerPincode { get; set; }
        public long? IVRMMG_Id { get; set; }
        public string retrunMsg { get; set; }
        public string ManagerMailid { get; set; }
        public string TeamLeadMailid { get; set; }
        public string Template { get; set; }
        public Array getdeviationreport { get; set; }

        public class doc_list_new
        {
           public long ISMRESGMCL_Id { get; set; }
           public long ISMRESGCL_Id { get; set; }
           public string ISMRESGCL_FileName { get; set; }
            public string ISMRESGMCL_CheckListName { get; set; }
            public string ISMRESGCL_FilePath { get; set; }
           public string document_Path  { get; set; }
           public bool ISMRESGCL_ActiveFlag { get; set; }
           public DateTime? CreatedDate { get; set; }
           public DateTime? UpdatedDate { get; set; }
           public long ISMRESGCL_CreatedBy { get; set; }
           public long ISMRESGCL_UpdatedBy { get; set; }
        }
        public class delete_date1
        {
            public long ISMRESGCL_Id { get; set; }
        }
    }
}
