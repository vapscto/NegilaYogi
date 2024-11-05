using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class GeneralSiblingEmployeeMappingDTO
    {
        public long AMSS_ID { get; set; }
        public long AMST_Id { get; set; }
        public long AMST_ORDER { get; set; }
        public long AMST_CON { get; set; }
        public long EMP_CODE { get; set; }
        public long HRME_Id { get; set; }
        public long TC_FLAG { get; set; }
        public long ASMAY_ID { get; set; }
        public string Contype { get; set; }
        public string Admission_Confrim { get; set; }
        public long Registration_Id { get; set; }
        public long Emp_Flag { get; set; }
        public long MI_Id { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public Array fillmastergroup { get; set; }
        public Array allstudentdata { get; set; }
        public Array allstaffdata { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string radiobtnval { get; set; }
        public Array fillacademic { get; set; }
        public Array fillstaff { get; set; }
        public long pageid { get; set; }
        public GeneralSiblingEmployeeMappingDTO[] savetmpdata { get; set; }
        public string returnval { get; set; }
        public Array alldata { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string HRMDES_DesignationName { get; set; }       
        public Array getstudentlistsaved { get; set; }
        public long AMSTS_Id { get; set; }
        public string AMSTS_SiblingsName { get; set; }
        public string AMSTG_SiblingsRelation { get; set; }
        public long AMCL_Id { get; set; }
        public long AMSTS_SiblingsAMST_Id { get; set; }
        public int AMSTS_SiblingsOrder { get; set; }
        public int AMSTS_Tc_IssuesFlag { get; set; }
        public Array getsudentlist { get; set; }
        public string sibilingname { get; set; }
        public GeneralsavesiblingsDTO[] savesiblingsDTO { get; set; }
        public Array getclassdetails { get; set; }
        public GeneralsavestudentemployeeDTO[] savestudentemployeeDTO { get; set; }
        public long AMSTE_Id { get; set; }
        public Array getdisplaydetails { get; set; }
        public string AMST_AdmNo { get; set; }
        public Array getviewdetails { get; set; }
        public string message { get; set; }
        public int AMSTE_SiblingsOrder { get; set; }
        public long countmapping { get; set; }
    }
    public class GeneralsavesiblingsDTO
    {
        public long AMSTS_Id { get; set; }
        public long AMST_Id { get; set; }
        public string AMSTS_SiblingsName { get; set; }
        public string AMSTG_SiblingsRelation { get; set; }
        public long AMCL_Id { get; set; }
        public long AMSTS_SiblingsAMST_Id { get; set; }
        public int AMSTS_SiblingsOrder { get; set; }
    }
    public class GeneralsavestudentemployeeDTO
    {
        public long AMSTE_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public int AMSTE_SiblingsOrder { get; set; }
    }
}

