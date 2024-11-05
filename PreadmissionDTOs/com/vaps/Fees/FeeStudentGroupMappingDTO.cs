using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeStudentGroupMappingDTO
    {
        public long FMSG_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long FMG_Id { get; set; }
        public string FMSG_ActiveFlag { get; set; }
        public long FMH_Id { get; set; }
        public long FSS_Id { get; set; }
        public long FTI_Id { get; set; }
        public long IMCC_Id { get; set; }
        public string FMH_FeeName { get; set; }
        public string FTI_Name { get; set; }
        public long FSS_PaidAmount { get; set; }
        public Array fillmasterarea { get; set; }
        public Array fillmasterhead { get; set; }
        public Array fillmasterclass { get; set; }
        public Array fillmastersection { get; set; }        
        public Array fillmastergroup { get; set; }
        public Array fillinstallment { get; set; }
        public Array alldata { get; set; }
        public Array alldatathird { get; set; }
        public Array fillfeeclasscategory { get; set; }
        public Array filladmissionclasscategory { get; set; }
        public Array fillbusroutedet { get; set; }
        public Array fillarearoute { get; set; }
        public Array castecategorydet { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public string AMST_AdmNo { get; set; }
        public string AMST_RegistrationNo { get; set; }
        public long AMAY_RollNo { get; set; }
        public FeeStudentGroupMappingDTO[] studentdata { get; set; }
        public FeeStudentGroupMappingDTO[] savegrplst { get; set; }
        public FeeStudentGroupMappingDTO[] saveheadlst { get; set; }
        public FeeStudentGroupMappingDTO[] saveftilst { get; set; }
        public string returnval { get; set; }
        public bool studchecked { get; set; }
        public bool checkedgrplst { get; set; }
        public bool checkedheadlst { get; set; }
        public bool checkedinstallmentlst { get; set; }
        public bool disableins { get; set; }
        public bool checkedgrplstedit { get; set; }
        public bool checkedheadlstedit { get; set; }
        public bool checkedinstallmentlstedit { get; set; }        
        public string FMG_GroupName { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public long AMST_Mobile { get; set; }
        public string radioval { get; set; }
        public long AMC_Id { get; set; }
        public long FMCC_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public Array fillmappedgroupforstudents { get; set; }
        public long user_id { get; set; }
        public string searchType { get; set; }
        public string searchtext { get; set; }
        public Array searcharray { get; set; }        
        public long AMSC_Id { get; set; }
        public string asmc_sectionname { get; set; }
        public Array alldatathirdall { get; set; }       
        public long[] ASMS_Ids { get; set; }
        public long[] ASMCL_Ids { get; set; }
        public long flag { get; set; }        
        public bool classwisecheckboxvalue { get; set; }
        public bool ingeneralcheckboxvalue { get; set; }
        public long fyp_id { get; set; }
        public long grpidss { get; set; }
        public string  fyp_receipt_no { get; set; }


        //employee related
        public Array employeelist { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public long? HRMD_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public long? HRME_MobileNo { get; set; }
        public bool staffchecked { get; set; }       
        public Array configsetting { get; set; }
        public Array stafflist { get; set; }
        public Array saved_stafflist { get; set; }
        public Array grid_stafflist { get; set; }
        public Array oth_studentlist { get; set; }
        public Array saved_oth_studentlist { get; set; }
        public Array grid_oth_studentlist { get; set; }
        public long FMOST_Id { get; set; }
        public string FMOST_StudentName { get; set; }
        public long FMOST_StudentMobileNo { get; set; }
        public string FMOST_StudentEmailId { get; set; }
        public long HRMDES_Id { get; set; }
        public Temp_Staff_DTO[] staff_list { get; set; }
        public Fee_Master_OtherStudentsDTO[] student_list { get; set; }
        public long FMSTGH_Id { get; set; }
        public long FMOSTGH_Id { get; set; }
        public Array academicdrp { get; set; }
        public long TRMR_Id { get; set; }
        public long TRMA_Id { get; set; }
        public long ASMC_Order { get; set; }
        public Array leftstudent { get; set; }

    }
    public class Temp_Staff_DTO
    {
        public long HRME_Id { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public long? HRMD_Id { get; set; }
        public long HRMDES_Id { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string HRMDES_DesignationName { get; set; }
    }

    public class Fee_Master_OtherStudentsDTO : CommonParamDTO
    {
        public long FMOST_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMOST_StudentName { get; set; }
        public long FMOST_StudentMobileNo { get; set; }
        public string FMOST_StudentEmailId { get; set; }
        public bool FMOST_ActiveFlag { get; set; }
    }
    //MB

}
