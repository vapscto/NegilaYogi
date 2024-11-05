using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdmissionServiceHub.com.vaps.Interfaces;
using AdmissionServiceHub.com.vaps.Services;
using PreadmissionDTOs.com.vaps.admission;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vapstech.Controllers
{
    [Route("api/[controller]")]

    public class MasterDocumentMappingFacadeController : Controller
    {
        public MasterDocumentMappingInterface _MasterDocumentMappingInterface;


        public MasterDocumentMappingFacadeController(MasterDocumentMappingInterface MasterDocumentMappingInterface)
        {
            _MasterDocumentMappingInterface = MasterDocumentMappingInterface;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("Getdetails/")]
        public MasterDocumentMappingDTO Getdetails(MasterDocumentMappingDTO MasterDocumentMappingDTO)//int IVRMM_Id
        {           
            return _MasterDocumentMappingInterface.Getdetails(MasterDocumentMappingDTO);
        }

        [Route("GetSelectedRowDetails/{id:int}")]
        public MasterDocumentMappingDTO GetSelectedRowDetails(int ID)
        {    
            return _MasterDocumentMappingInterface.GetSelectedRowDetails(ID);
        }

        [HttpPost]
        public MasterDocumentMappingDTO Post([FromBody] MasterDocumentMappingDTO masterMDT)
        {
            return _MasterDocumentMappingInterface.SaveData(masterMDT);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }

        [HttpDelete]
        [Route("DeleteEntry/{id:int}")]
        public MasterDocumentMappingDTO DeleteEntry(int ID)
        {
            return _MasterDocumentMappingInterface.DeleteEntry(ID);
        }

    }
}
