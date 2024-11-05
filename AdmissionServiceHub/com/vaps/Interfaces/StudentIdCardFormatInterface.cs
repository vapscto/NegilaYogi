using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface StudentIdCardFormatInterface
    {
        StudentIdCardFormatDTO OnLoadStudentIdCardDetails(StudentIdCardFormatDTO data);
        StudentIdCardFormatDTO OnChangeYear(StudentIdCardFormatDTO data);
        StudentIdCardFormatDTO OnChangeClass(StudentIdCardFormatDTO data);
        StudentIdCardFormatDTO OnChangeSection(StudentIdCardFormatDTO data);
        StudentIdCardFormatDTO GetReportDetails(StudentIdCardFormatDTO data);
    }
}
