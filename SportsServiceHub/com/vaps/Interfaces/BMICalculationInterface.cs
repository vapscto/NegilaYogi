using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Interfaces
{
    public interface BMICalculationInterface
    {
        BMICalculationDTO getDetails(BMICalculationDTO data);
        BMICalculationDTO getStudents(BMICalculationDTO data);
        BMICalculationDTO saveRecord(BMICalculationDTO data);
        BMICalculationDTO get_section(BMICalculationDTO data);
        BMICalculationDTO deactivate(BMICalculationDTO data);
        BMICalculationDTO editdata(BMICalculationDTO data);
        BMICalculationDTO get_classes(BMICalculationDTO data);
        BMICalculationDTO filterStudeDateWise(BMICalculationDTO data);
        
    }
}
