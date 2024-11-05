using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
   public interface Master_ExamQualified_ClassInterface
    {
        Master_ExamQualified_ClassDTO getalldata(Master_ExamQualified_ClassDTO data);
        Master_ExamQualified_ClassDTO SaveClass(Master_ExamQualified_ClassDTO data);
        Master_ExamQualified_ClassDTO Editdetails(int id);
        Master_ExamQualified_ClassDTO deactiveCat(Master_ExamQualified_ClassDTO data);
    }
}
