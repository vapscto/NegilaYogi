
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Interfaces
{
    public interface Employee_MedicalRecordInterface
    {
        Employee_MedicalRecordDTO savedetail(Employee_MedicalRecordDTO data);
        Employee_MedicalRecordDTO Getdetails(Employee_MedicalRecordDTO data);
        Employee_MedicalRecordDTO deactivate(Employee_MedicalRecordDTO data);
        Employee_MedicalRecordDTO viewData(Employee_MedicalRecordDTO data);
        Employee_MedicalRecordDTO onclick_employee(Employee_MedicalRecordDTO data);
    }
}