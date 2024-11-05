using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeeServiceHub.com.vaps.interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class StudentFeeEnablePartialPaymentFacadeController : Controller
    {
        public StudentFeeEnablePartialPaymentInterface objInterface;

        public StudentFeeEnablePartialPaymentFacadeController(StudentFeeEnablePartialPaymentInterface bdInterface)
        {
            objInterface = bdInterface;
        }
       

        [HttpGet]
        [Route("GetYearList/{id:int}")]
        public StudentFeeEnablePartialPaymentDTO GetYearList(int id)
        {
            return objInterface.GetYearList(id);
        }
        [Route("getsection")]
        public StudentFeeEnablePartialPaymentDTO getsection([FromBody] StudentFeeEnablePartialPaymentDTO data)
        {
            return objInterface.getsection(data);
        }
        [Route("get_student")]
        public StudentFeeEnablePartialPaymentDTO get_student([FromBody] StudentFeeEnablePartialPaymentDTO data)
        {
            return objInterface.get_student(data);
        }
        [Route("savedata")]
        public StudentFeeEnablePartialPaymentDTO savedata([FromBody] StudentFeeEnablePartialPaymentDTO data)
        {
            return objInterface.savedata(data);
        }
        [Route("deactivate")]
        public StudentFeeEnablePartialPaymentDTO deactivate([FromBody] StudentFeeEnablePartialPaymentDTO data)
        {
            return objInterface.deactivate(data);
        }
    }
}
