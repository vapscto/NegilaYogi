using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs
{
    public class TaskCreationFromClintDTO : CommonParamDTO
    {
        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool roleflag { get; set; }
        public bool completed { get; set; }
        public bool pending { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }
        public long UserId { get; set; }
        public string flag { get; set; }
        public string Auto_id { get; set; }
        public string Role_flag { get; set; }
        public string roletype { get; set; }
        public long IVRMRT_Id { get; set; }
        public string sound { get; set; }
        public string ISMMCLT_ClientCode { get; set; }
        public Array plannerextension { get; set; }
        public bool plannerextapproval { get; set; }
        public DateTime plannerMaxdate { get; set; }

        public string HRMDC_Name { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string userEmpName { get; set; }
        public string empname { get; set; }
        public string empdepartment { get; set; }
        public Array DepartmentList { get; set; }
        public Array get_userEmplist { get; set; }
        public Array get_makercheckerstslist { get; set; }
        public Array get_makercheckerviewlist { get; set; }
        public Array Designation { get; set; }
        public Array get_Status { get; set; }
        public Array prioritylist { get; set; }
        public DepartmentDTO1[] departmentlist { get; set; }
        public DesignationDTO1[] designationlist { get; set; }
        public priorityarray1[] priorityarray { get; set; }

        public long ISMMAC_Id { get; set; }
        public long MI_Id { get; set; }
        public long MIId { get; set; }
        public long HRME_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string ISMMAC_ActivityName { get; set; }
        public long HRMD_Id { get; set; }
        public long? HRMDC_ID { get; set; }
        public long ISMMPR_Id { get; set; }
        public string ISMMPR_ProjectName { get; set; }


        public long IVRMM_Id { get; set; }
        public string IVRMM_ModuleName { get; set; }
        public string IVRMM_ModuleDesc { get; set; }
        public long ISMMMD_Id { get; set; }
        public long ISMMMD_ModuleHeadId { get; set; }

        public long ISMTCR_Id { get; set; }
        public string ISMTCR_TaskNo { get; set; }
        public string ISMTCR_BugOREnhancementFlg { get; set; }

        public string assignto { get; set; }
        public DateTime? ISMTCR_CreationDate { get; set; }
        public string ISMTCR_Title { get; set; }
        public long HRMPR_Id { get; set; }
        public long ISMMTCAT_Id { get; set; }

        public string HRMP_Name { get; set; }
        public string ISMTCR_Desc { get; set; }
        public string ISMTCR_Status { get; set; }
        public bool ISMTCR_ReOpenFlg { get; set; }
        public DateTime? ISMTCR_ReOpenDate { get; set; }
        public bool ISMTCR_ActiveFlg { get; set; }
        public long ISMTCR_CreatedBy { get; set; }
        public long ISMTCR_UpdatedBy { get; set; }

        public long ISMTCRCL_Id { get; set; }
        public long ISMMCLT_Id { get; set; }
        public bool ISMTCRCL_ActiveFlg { get; set; }
        public long ISMTCRCL_CreatedBy { get; set; }
        public long ISMTCRCL_UpdatedBy { get; set; }

        public long ISMTCRAT_Id { get; set; }
        public string ISMTCRAT_Attatchment { get; set; }
        public bool ISMTCRAT_ActiveFlg { get; set; }
        public long ISMTCRAT_CreatedBy { get; set; }
        public long ISMTCRAT_UpdatedBy { get; set; }
        public long prioritycheck { get; set; }
        public Array Client_Master { get; set; }
        public Array get_department { get; set; }
        public Array get_project { get; set; }
        public Array get_module { get; set; }
        public Array get_priority { get; set; }
        public Array get_category { get; set; }
        public Array get_days { get; set; }
        public Array get_client { get; set; }
        public Array get_taskdetails { get; set; }
        public Array get_Attdetails { get; set; }
        public Array get_taskproject { get; set; }
        public Array get_taskmodule { get; set; }
        public Array get_taskclient { get; set; }
        public attachmentArray1[] attachmentArray { get; set; }
        public string[] file_names { get; set; }

        //********************** Task register Report
        public string TypeFlg { get; set; }
        public string SelectionFlag { get; set; }
        public DateTime? startdate { get; set; }
        public DateTime? enddate { get; set; }

        public string startdate_new { get; set; }
        public string enddate_new { get; set; }
        public Array get_companyDeptist { get; set; }
        public Array get_employeelist { get; set; }
        public Array get_taskregisterReport { get; set; }
        public taskStatusDTO[] taskStatus { get; set; }
        public taskemployeeDTO[] taskemployee { get; set; }
        //************************************************************ Client Side 
        public long ISMCIM_IEList { get; set; }
        public Array get_IEuser { get; set; }
        public TRarrayStatusDTO[] arrayStatus { get; set; }
        public TRarrayClientDTO[] arrayClient { get; set; }
        public DateTime? Yearlydate { get; set; }
        public string periodicity { get; set; }
        public string remarks { get; set; }
        public string TaskDay { get; set; }
        public string TimeRequiredFlg { get; set; }
        public decimal effortinhrs { get; set; }
        public taskemployeeDTO[] taskEmpArray { get; set; }

    }

    public class taskStatusDTO
    {
        public string id { get; set; }

    }

    public class priorityarray1
    {
        public long HRMPR_Id { get; set; }

    }
    public class taskemployeeDTO
    {
        public long HRME_Id { get; set; }
    }
    public class TRarrayClientDTO
    {
        public long ISMMCLT_Id { get; set; }
        public long UserId { get; set; }

    }
    public class TRarrayStatusDTO
    {
        public string id { get; set; }
    }
    public class DepartmentDTO1
    {
        public int HRMDC_ID { get; set; }
    }


    public class DesignationDTO1
    {
        public long HRMDES_Id { get; set; }
    }
    public class attachmentArray1
    {
        public string ISMTCRAT_Attatchment { get; set; }
        public string ISMTCRAT_File { get; set; }
    }
}
