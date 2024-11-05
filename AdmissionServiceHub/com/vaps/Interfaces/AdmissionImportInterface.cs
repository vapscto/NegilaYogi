using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface AdmissionImportInterface
    {
        // ImportStudentWrapperDTO getdetails(ImportStudentWrapperDTO stu);
        Task<ImportStudentWrapperDTO> getdetails(ImportStudentWrapperDTO stu);
        Task<ImportStudentWrapperDTO> checkvalidation(ImportStudentWrapperDTO data);
    }
}
