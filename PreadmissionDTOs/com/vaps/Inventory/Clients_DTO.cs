using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Inventory
{
    public class Clients_DTO : CommonParamDTO
    {
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }
        public long ISMMCLT_Id { get; set; }
        public long MI_Id { get; set; }
        public string ISMMCLT_GSTNO { get; set; }
        public string ISMMCLT_ClientName { get; set; }
        public string ISMMCLT_Desc { get; set; }
        public long ISMMCLT_ContactNo { get; set; }
        public string ISMMCLT_EmailId { get; set; }
        public string ISMMCLT_Address { get; set; }
        public string ISMMCLT_NOName { get; set; }
        public string ISMMCLT_NOEmailId { get; set; }
        public long ISMMCLT_NOContactNo { get; set; }
        public long? ISMMCLT_CordinatorId { get; set; }
        public long? ISMMCLT_TeamLeadId { get; set; }
        public bool ISMMCLT_ActiveFlag { get; set; }
        public long ISMMCLT_CreatedBy { get; set; }
        public long ISMMCLT_UpdatedBy { get; set; }
        public long ISMCIM_IEList { get; set; }
        public long UserId { get; set; }
        public long HRME_Id { get; set; }
        public long HRMD_Id { get; set; }
        public string cordinatorname { get; set; }
        public string ieEmpname { get; set; }
        public string leadername { get; set; }
        public string teamleadFirstName { get; set; }
        public string teamleadMiddleName { get; set; }
        public string teamleadLastName { get; set; }
        public string cordinatorFirstName { get; set; }
        public string cordinatorMiddleName { get; set; }
        public string cordinatorLastName { get; set; }
        public string ieEmployName { get; set; }
        public string usersname { get; set; }
        public string employeename { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public string designationName { get; set; }
        public string username { get; set; }
        public long ISMUSEMM_Id { get; set; }
        public int? ISMUSEMM_Order { get; set; }
        public bool ISMUSEMM_ActiveFlag { get; set; }
        public long ISMUSEMM_CreatedBy { get; set; }
        public long ISMUSEMM_UpdatedBy { get; set; }
        public Array cordinartorlist { get; set; }
        public Array teamleadlist { get; set; }
        public Array get_clentlist { get; set; }
        public Array editClient { get; set; }
        public Array clientlist { get; set; }
        public Array ieEmpList { get; set; }
        public Array get_userslist { get; set; }
        public Array get_employeelist { get; set; }
        public Array get_mappedEmplist { get; set; }
        public Array get_departmentlist { get; set; }
        public Array allIeMappingdata { get; set; }
        public Array get_userEmpcount { get; set; }
        public Array get_userEmpDetails { get; set; }
        public bool duplicate { get; set; }
        public long ISMMCLTIE_Id { get; set; }
        public long User_Id { get; set; }
        public bool ISMMCLTIE_ActiveFlag { get; set; }
        public long ISMMCLTIE_CreatedBy { get; set; }
        public long ISMMCLTIE_UpdatedBy { get; set; }
        public long ISMMCLTUS_Id { get; set; }
        public bool ISMMCLTUS_ActiveFlag { get; set; }
        public Array editCltMappinglist { get; set; }
        public Array modaliEMapingList { get; set; }
        public Clients_DTO[] ieEmpListdata { get; set; }
        public Array modalUserMapingList { get; set; }
        public Array institution { get; set; }
        public string useempName { get; set; }
        public employeeListDTO[] employeeList { get; set; }
        public long? ISMMCLT_RemainderDays { get; set; }
        public string ISMMCLT_Code { get; set; }
        public string ISMMCLT_IVRM_URL { get; set; }
        public string ISMMCLT_ClientCode { get; set; }
        public string Flag { get; set; }
        public long? IVRM_MI_Id { get; set; }
        public Array clientdetails { get; set; }
        public multipleclient[] multipleclients { get; set; }

    }
    public class multipleclient
    {
        public long MI_Id { get; set; }
    }

    public class employeeListDTO
    {
        public long HRME_Id { get; set; }
        public long User_Id { get; set; }
        public int ISMUSEMM_Order { get; set; }

        public long ISMUSEMM_Id { get; set; }
        public bool ISMUSEMM_ActiveFlag { get; set; }
        public long ISMUSEMM_CreatedBy { get; set; }
        public long ISMUSEMM_UpdatedBy { get; set; }

    }
}
