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

    public class MasterActivityFacadeController : Controller
    {
        public MasterActivityInterface _MasterActivity;

        public MasterActivityFacadeController(MasterActivityInterface MasterActivity)
        {
            _MasterActivity = MasterActivity;
        }

    
        [HttpGet]

        [Route("Getdetails/")]
        public MasterActivityDTO Getdetails(MasterActivityDTO MasterActivityDTO)//int IVRMM_Id
        {
           
            return _MasterActivity.GetMasterActivityData(MasterActivityDTO);
           
        }

        [Route("GetSelectedRowDetails/{id:int}")]
        public MasterActivityDTO GetSelectedRowDetails(int ID)
        {
           
            return _MasterActivity.GetSelectedRowDetails(ID);
        }
    
        [HttpPost]
        public MasterActivityDTO Post([FromBody] MasterActivityDTO masterMDT)
        {
          
            return _MasterActivity.MasterActivityData(masterMDT);
        }

        [HttpGet]
        [Route("MasterDeleteModulesDATA/{id:int}")]
        public MasterActivityDTO MasterDeleteModulesDATA(int ID)
        {
           
            return _MasterActivity.MasterDeleteModulesData(ID);
        }

     
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }

    }
}
