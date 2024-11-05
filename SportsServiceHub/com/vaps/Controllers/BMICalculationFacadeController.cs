using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Sports;
using SportsServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SportsServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class BMICalculationFacadeController : Controller
    {
        BMICalculationInterface _interface;
        public BMICalculationFacadeController(BMICalculationInterface interfaces)
        {
            _interface = interfaces;
        }
        [Route("getDetails")]
        public BMICalculationDTO getDetails([FromBody]BMICalculationDTO data)
        {
            return _interface.getDetails(data);
        }
        [Route("get_section")]
        public BMICalculationDTO get_section([FromBody]BMICalculationDTO data)
        {
            return _interface.get_section(data);
        }
        [Route("getStudents")]
        public BMICalculationDTO getStudents([FromBody]BMICalculationDTO data)
        {
            return _interface.getStudents(data);
        }
        [Route("save")]
        public BMICalculationDTO save([FromBody]BMICalculationDTO data)
        {
            return _interface.saveRecord(data);
        }
        [Route("deactivate")]
        public BMICalculationDTO deactivate([FromBody]BMICalculationDTO data)
        {
            return _interface.deactivate(data);
        }
        [Route("editdata")]
        public BMICalculationDTO editdata([FromBody]BMICalculationDTO data)
        {
            return _interface.editdata(data);
        }
        [Route("get_classes")]
        public BMICalculationDTO get_classes([FromBody]BMICalculationDTO data)
        {
            return _interface.get_classes(data);
        }
        [Route("filterStudeDateWise")]
        public BMICalculationDTO filterStudeDateWise([FromBody]BMICalculationDTO data)
        {
            return _interface.filterStudeDateWise(data);
        }
      
    }
}
