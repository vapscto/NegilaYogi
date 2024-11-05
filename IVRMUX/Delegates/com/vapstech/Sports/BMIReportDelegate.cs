using CommonLibrary;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Sports
{
    public class BMIReportDelegate
    {
        CommonDelegate<BMICalculationDTO, BMICalculationDTO> COMSPRT = new CommonDelegate<BMICalculationDTO, BMICalculationDTO>();

        public BMICalculationDTO report(BMICalculationDTO data)
        {
            return COMSPRT.POSTDataSports(data, "BMIReportFacade/report/");
        }

        public BMICalculationDTO getDetails(BMICalculationDTO data)
        {
            return COMSPRT.POSTDataSports(data, "BMIReportFacade/getDetails/");
        }

        public BMICalculationDTO get_section(BMICalculationDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "BMIReportFacade/get_section/");
        }
        public BMICalculationDTO get_class(BMICalculationDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "BMIReportFacade/get_class/");
        }
        public BMICalculationDTO getStudents(BMICalculationDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "BMIReportFacade/getStudents/");
        }

    }
}
