
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface BatchwiseStudentMappingInterface
    {
        AdmSchoolAttendanceSubjectBatchDTO GetDropdowndetailsbyYearandInstitute(AdmSchoolAttendanceSubjectBatchDTO dto);
        AdmSchoolAttendanceSubjectBatchDTO saveAdmSchoolAttendanceSubjectBatch(AdmSchoolAttendanceSubjectBatchDTO dto);
        AdmSchoolAttendanceSubjectBatchDTO getbatchwisestdlist(AdmSchoolAttendanceSubjectBatchDTO data);
        AdmSchoolAttendanceSubjectBatchDTO getbatchname(AdmSchoolAttendanceSubjectBatchDTO data);
        
    }
}
