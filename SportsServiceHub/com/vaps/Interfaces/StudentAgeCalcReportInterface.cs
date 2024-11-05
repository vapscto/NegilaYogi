using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Interfaces
{
   public interface StudentAgeCalcReportInterface
    {
        Task<StudentAgeCalcReport_DTO> showdetails(StudentAgeCalcReport_DTO data);
        StudentAgeCalcReport_DTO Getdetails(StudentAgeCalcReport_DTO data);
        StudentAgeCalcReport_DTO get_class(StudentAgeCalcReport_DTO data);
        StudentAgeCalcReport_DTO get_section(StudentAgeCalcReport_DTO data);

    }
}
