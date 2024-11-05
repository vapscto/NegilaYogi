using System;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class Ch_EmployeeSalaryDetailsDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<Emp_salaryDTO, Emp_salaryDTO> COMMM = new CommonDelegate<Emp_salaryDTO, Emp_salaryDTO>();

        public Emp_salaryDTO getalldetails(Emp_salaryDTO data)
        {
            return COMMM.POSTPORTALData(data, "Ch_EmployeeSalaryDetailsFacade/getdata/");
        }
       

       
        public Emp_salaryDTO onmonth(Emp_salaryDTO data)
        {
            return COMMM.POSTPORTALData(data, "Ch_EmployeeSalaryDetailsFacade/onmonth/");
        }

        
    }
}
