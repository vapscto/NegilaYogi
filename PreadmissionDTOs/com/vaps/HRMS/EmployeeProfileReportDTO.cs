using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class EmployeeProfileReportDTO : Master_Employee_QulaificationDTO
    {      
        public long roleId { get; set; }    

        //institution
        public InstitutionDTO institutionDetails { get; set; }
        //designation name
        public string DesignationName { get; set; }
        //list for employee
        public MasterEmployeeDTO currentemployeeDetails { get; set; }
        //employee name dropdown
        public Array employeedropdown { get; set; }
        //employee Qualification
        public Array employequalification { get; set; }
        public Array employeedocument { get; set; }
        public string HRMC_QulaificationName { get; set; }


        public long?[] hrmgT_IdList { get; set; }
        public long?[] employeeTypeIdList { get; set; }
        public long?[] hrmD_IdList { get; set; }
        public long?[] hrmdeS_IdList { get; set; }

        public Array departmentdropdown { get; set; }
        public Array designationdropdown { get; set; }
        public Array employeeclasssubject { get; set; }

        public Array employeeTypedropdown { get; set; }

        public Array groupTypedropdown { get; set; }

        public long LogInUserId { get; set; }
        public EmployeeProfileReportDTO[] ArrayempsList { get; set; }
        public Array institutionDetails_Array { get; set; }
        public List<EmployeeProfileReportDTO> AllInOne { get; set; }

        public string HRMEDS_DocumentName { get; set; }
        public string HRMEDS_DocumentImageName { get; set; }
        public string HRMEDS_DucumentDescription { get; set; }

        //Type
        public string Type { get; set; }
        public long HRMGT_Id { get; set; }
        public Array employeedetailList { get; set; }
        public Array grouplist { get; set; }
        public Array departmentlist { get; set; }
        public Array designationlist { get; set; }
        public DateTime? HRME_DOC { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public long? HRMD_Id { get; set; }
        public long? HRMDES_Id { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public string HRMGT_EmployeeGroupType { get; set; }
        public Array filltypes { get; set; }
        public string multipletype { get; set; }
        public string multipledep { get; set; }
        public long[] hrmE_multiId { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long ISMS_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string Institutionname { get; set; }
    }
}
