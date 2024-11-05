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

    public class MasterDocumentFacadeController : Controller
    {
        public MasterDocumentInterface _MasterDocumentInterface;


        public MasterDocumentFacadeController(MasterDocumentInterface MasterDocumentInterface)
        {
            _MasterDocumentInterface = MasterDocumentInterface;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("Getdetails")]
        public MasterDocumentDTO Getdetails([FromBody] MasterDocumentDTO MasterDocumentDTO)//int IVRMM_Id
        {           
            return _MasterDocumentInterface.Getdetails(MasterDocumentDTO);
        }

        [Route("GetSelectedRowDetails/{id:int}")]
        public MasterDocumentDTO GetSelectedRowDetails(int ID)
        {    
            return _MasterDocumentInterface.GetSelectedRowDetails(ID);
        }

        [HttpPost]
        public MasterDocumentDTO Post([FromBody] MasterDocumentDTO masterMDT)
        {
            return _MasterDocumentInterface.SaveData(masterMDT);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }

        [HttpGet]
        [Route("DeleteEntry/{id:int}")]
        public MasterDocumentDTO DeleteEntry(int ID)
        {
            return _MasterDocumentInterface.DeleteEntry(ID);
        }

    }
}
