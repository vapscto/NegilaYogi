using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Interfaces
{
    public interface BMIReportInterface
    {
        Task<BMICalculationDTO> report(BMICalculationDTO data);
        BMICalculationDTO getDetails(BMICalculationDTO data);
        BMICalculationDTO getStudents(BMICalculationDTO data);
        BMICalculationDTO get_section(BMICalculationDTO data);
        BMICalculationDTO get_class(BMICalculationDTO data);


    }
}
