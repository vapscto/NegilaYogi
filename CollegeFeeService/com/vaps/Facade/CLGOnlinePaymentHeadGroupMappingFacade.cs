using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CollegeFeeService.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.College.Fee;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeFeeService.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class CLGOnlinePaymentHeadGroupMappingFacade : Controller
    {
        public CLGOnlinePaymentHeadGroupMappingInterface objInterface;

        public CLGOnlinePaymentHeadGroupMappingFacade(CLGOnlinePaymentHeadGroupMappingInterface bdInterface)
        {
            objInterface = bdInterface;
        }
        // GET: api/values

        [HttpGet]
        [Route("onlineMappingDetails/{id:int}")]
        public Clg_StudentFeeGroupMapping_DTO onlineMappingDetails(int id)
        {
            return objInterface.onlineMappingDetails(id);
        }


        [HttpPost]
        [Route("save")]
        public Clg_StudentFeeGroupMapping_DTO save([FromBody]Clg_StudentFeeGroupMapping_DTO data)
        {
            return objInterface.saveDetails(data);
        }

    
        [Route("edit/{id:int}")]
        public Clg_StudentFeeGroupMapping_DTO edit(int id)
        {
            return objInterface.editDetails(id);
        }


        [Route("delete/{id:int}")]
        public Clg_StudentFeeGroupMapping_DTO delete(int id)
        {
            return objInterface.deleteDetails(id);
        }

        
        [Route("groupsel")]
        public Clg_StudentFeeGroupMapping_DTO groupselection([FromBody] Clg_StudentFeeGroupMapping_DTO data)
        {
            return objInterface.groupselection(data);
        }

        [Route("headsel")]
        public Clg_StudentFeeGroupMapping_DTO headsel([FromBody] Clg_StudentFeeGroupMapping_DTO data)
        {
            return objInterface.headsel(data);
        }
        

        [Route("academicsel")]
        public Clg_StudentFeeGroupMapping_DTO academicsel([FromBody]Clg_StudentFeeGroupMapping_DTO data)
        {
            return objInterface.academicsel(data);
        }
        
    }
}
