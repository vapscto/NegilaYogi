using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
  public class ECRDTO
    {
        public long ECR_ID { get; set; }
        public long MI_Id { get; set; }
        public long Emp_code { get; set; }
        public string name { get; set; }
        public decimal? ECR_EPF_Wages { get; set; }
        public decimal? Ecr_Eps_Wages { get; set; }
        public decimal? Ecr_Epf_Contribution { get; set; }
        public decimal? Ecr_Epf_Cont_Remit { get; set; }
        public decimal? ECr_Epf_Eps_Diff { get; set; }
        public decimal? Ecr_Epf_Eps_ReDif { get; set; }
        public decimal? Ecr_Ncp { get; set; }
        public decimal? Ecr_Adva_Ref { get; set; }
        public decimal? Ecr_Arr_Epf { get; set; }
        public decimal? Ecr_Arr_Epf_EE_Share { get; set; }
        public decimal? Ecr_Arr_Epf_ER_Share { get; set; }
        public decimal? Ecr_Arr_EPS { get; set; }
        public string ECR_GuardianName { get; set; }
        public string Ecr_Guardian_Relation { get; set; }

        public string Ecr_DOB { get; set; }
        public string Ecr_Gender { get; set; }

        public string Ecr_Join_DOEPF { get; set; }
        public string ECR_Exit_DOEPF { get; set; }

        public string ECR_Exit_DoEps { get; set; }
        public string Ecr_Leav_Reason { get; set; }
        public long roleId { get; set; }
        public long LogInUserId { get; set; }
        public Array monthdropdown { get; set; }
        public Array leaveyeardropdown { get; set; }
        public Array groupTypedropdown { get; set; }
        public Array departmentdropdown { get; set; }
        public Array designationdropdown { get; set; }
        public HR_ConfigurationDTO configurationDetails { get; set; }
        public Array employeeDetails { get; set; }
        public Array employeeGender { get; set; }
        public Array employeedropdown { get; set; }
        public string HRES_Year { get; set; }
        public string HRES_Month { get; set; }
        public long?[] hrmdeS_IdList { get; set; }
        public long?[] hrmD_IdList { get; set; }
        public long?[] groupTypeIdList { get; set; }
        public string retrunMsg { get; set; }
        public List<MasterEmployeeDTO> employee { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
    }
}
