using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdmissionServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.admission;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    [Route("api/[controller]")]

    public class MasterSmsEmailParameterFacadeController : Controller
    {
        public MasterSmsEmailParameterInterface _castecategory;

        public MasterSmsEmailParameterFacadeController(MasterSmsEmailParameterInterface castecategory)
        {
            _castecategory = castecategory;
        }

    
        [HttpGet]

        [Route("Getdetails/")]
        public MasterSmsEmailParameterDTO Getdetails(MasterSmsEmailParameterDTO MasterSmsEmailParameterDTO)//int IVRMM_Id
        {
           
            return _castecategory.GetcastecategoryData(MasterSmsEmailParameterDTO);
           
        }

        [Route("edit")]
        public MasterSmsEmailParameterDTO GetSelectedRowDetails([FromBody] MasterSmsEmailParameterDTO data)
        {
            return _castecategory.edit(data);
        }
    
        [HttpPost]
        [Route("Savedata")]
        public MasterSmsEmailParameterDTO Savedata([FromBody] MasterSmsEmailParameterDTO masterMDT)
        {
          
            return _castecategory.Savedata(masterMDT);
        }

        [HttpDelete]
        [Route("deletedata/{id:int}")]
        public MasterSmsEmailParameterDTO deletedata(int ID)
        {
           
            return _castecategory.deletedata(ID);
        }

        //HTML TEMPLATE
      

        [Route("htmlGetdetails")]
        public MasterSmsEmailParameterDTO htmlGetdetails([FromBody] MasterSmsEmailParameterDTO MasterSmsEmailParameterDTO)//int IVRMM_Id
        {

            return _castecategory.htmlGetcastecategoryData(MasterSmsEmailParameterDTO);

        }

        [Route("htmledit")]
        public MasterSmsEmailParameterDTO htmlGetSelectedRowDetails([FromBody] MasterSmsEmailParameterDTO data)
        {
            return _castecategory.htmledit(data);
        }

        [HttpPost]
        [Route("htmlSavedata")]
        public MasterSmsEmailParameterDTO htmlSavedata([FromBody] MasterSmsEmailParameterDTO masterMDT)
        {

            return _castecategory.htmlSavedata(masterMDT);
        }

        [HttpPost]
        [Route("htmldeletedata")]
        public MasterSmsEmailParameterDTO htmldeletedata([FromBody] MasterSmsEmailParameterDTO masterMDT)
        {

            return _castecategory.htmldeletedata(masterMDT);
        }



    }
}
