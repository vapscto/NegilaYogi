using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class BankCashReportDTO
    {
        public long MI_Id { get; set; }
        public string retrunMsg { get; set; }
        public long roleId { get; set; }
        public string HRES_Year { get; set; }
        public string HRES_Month { get; set; }

        public string AllOrIndividual { get; set; }

        public Array monthdropdown { get; set; }
        public Array leaveyeardropdown { get; set; }
        public Array departmentdropdown { get; set; }
        public Array designationdropdown { get; set; }

        public Array employeeTypedropdown { get; set; }

        public Array groupTypedropdown { get; set; }



        public long?[] groupTypeselected { get; set; }
        public long?[] employeeTypeselected { get; set; }
        public long?[] departmentselected { get; set; }
        public long?[] designationselected { get; set; }

        public List<BankCashReportDTO> employeeDetails { get; set; }

        //datatable columns

        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string BankAcNumber { get; set; }
        public decimal? NetSalary { get; set; }
        public InstitutionDTO institutionDetails { get; set; }

        public string BankCash { get; set; }

        public Array bankdropdown { get; set; }

        public string companyacc { get; set; }

        public long?[] hrmgT_IdList { get; set; }
        public long?[] employeeTypeIdList { get; set; }
        public long?[] hrmD_IdList { get; set; }
        public long?[] hrmdeS_IdList { get; set; }
        public long?[] groupTypeIdList { get; set; }

        public long LogInUserId { get; set; }
        public int? HRME_EmployeeOrder { get; set; }

        public Array bankCashReportDTO { get; set; }
        public bool HRME_ActiveFlag { get; set; }
        public long HRME_Id { get; set; }
        public long HRES_Id { get; set; }
        public string HRMBD_BranchName { get; set; }
        public string HRMBD_BankName { get; set; }
        public string HRMBD_IFSCCode { get; set; }
    }
}
