using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
   public interface StudentCompliantsInterface
    {
        Task<StudentCompliants_DTO> loaddata(StudentCompliants_DTO data);
        Task<StudentCompliants_DTO> getstudents(StudentCompliants_DTO data);
        StudentCompliants_DTO save(StudentCompliants_DTO data);
        StudentCompliants_DTO searchfilter(StudentCompliants_DTO data);
        StudentCompliants_DTO edittab1(StudentCompliants_DTO data);
        StudentCompliants_DTO getstudentdetails(StudentCompliants_DTO data);
        StudentCompliants_DTO getorganizationdata(StudentCompliants_DTO data);
        StudentCompliants_DTO deactive(StudentCompliants_DTO data);
        StudentCompliants_DTO report(StudentCompliants_DTO data);
        StudentCompliants_DTO deleterecY(StudentCompliants_DTO data);
        StudentCompliants_DTO editdetails(StudentCompliants_DTO data);

    }
}
