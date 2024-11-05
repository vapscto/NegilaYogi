using CommonLibrary;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Sports
{
    public class BMICalculationDelegate
    {
        CommonDelegate<BMICalculationDTO, BMICalculationDTO> COMSPRT = new CommonDelegate<BMICalculationDTO, BMICalculationDTO>();

        public BMICalculationDTO getDetails(BMICalculationDTO data)
        {
            return COMSPRT.POSTDataSports(data, "BMICalculationFacade/getDetails/");
        }
        public BMICalculationDTO get_section(BMICalculationDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "BMICalculationFacade/get_section/");
        }
        public BMICalculationDTO getStudents(BMICalculationDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "BMICalculationFacade/getStudents/");
        }
        public BMICalculationDTO save(BMICalculationDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "BMICalculationFacade/save/");
        }
        public BMICalculationDTO deactivate(BMICalculationDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "BMICalculationFacade/deactivate/");
        }
        public BMICalculationDTO editdata(BMICalculationDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "BMICalculationFacade/editdata/");
        }
        public BMICalculationDTO get_classes(BMICalculationDTO data)
        {
            return COMSPRT.POSTDataSports(data, "BMICalculationFacade/get_classes/");            
        }
        public BMICalculationDTO filterStudeDateWise(BMICalculationDTO data)
        {
            return COMSPRT.POSTDataSports(data, "BMICalculationFacade/filterStudeDateWise/");            
        }
    }
}
