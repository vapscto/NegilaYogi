using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Portals.Employee
{
    public class Employee_MedicalRecordDelegate
    {
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<Employee_MedicalRecordDTO, Employee_MedicalRecordDTO> COMMM = new CommonDelegate<Employee_MedicalRecordDTO, Employee_MedicalRecordDTO>();

        public Employee_MedicalRecordDTO savedetail(Employee_MedicalRecordDTO data)
        {
            return COMMM.POSTPORTALData(data, "Employee_MedicalRecordFacade/savedetail/");
        }
        public Employee_MedicalRecordDTO Getdetails(Employee_MedicalRecordDTO data)
        {
            return COMMM.POSTPORTALData(data, "Employee_MedicalRecordFacade/Getdetails/");
        }

        public Employee_MedicalRecordDTO deactivate(Employee_MedicalRecordDTO obj)
        {
            return COMMM.POSTPORTALData(obj, "Employee_MedicalRecordFacade/deactivate/");
        }
        public Employee_MedicalRecordDTO viewData(Employee_MedicalRecordDTO data)
        {
            return COMMM.POSTPORTALData(data, "Employee_MedicalRecordFacade/viewData/");
        }
        public Employee_MedicalRecordDTO onclick_employee(Employee_MedicalRecordDTO data)
        {
            return COMMM.POSTPORTALData(data, "Employee_MedicalRecordFacade/onclick_employee/");
        }

    }
}
