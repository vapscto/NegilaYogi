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

    public class GovernmentBondFacadeController : Controller
    {
        public GovernmentBondInterface _GovernmentBond;

        public GovernmentBondFacadeController(GovernmentBondInterface GovernmentBond)
        {
            _GovernmentBond = GovernmentBond;
        }

    
     

        [Route("Getdetails/")]
        public GovernmentBondDTO Getdetails([FromBody]GovernmentBondDTO GovernmentBondDTO)//int IVRMM_Id
        {
           
            return _GovernmentBond.GetGovernmentBondData(GovernmentBondDTO);
           
        }

        [Route("GetSelectedRowDetails/{id:int}")]
        public GovernmentBondDTO GetSelectedRowDetails(int ID)
        {
           
            return _GovernmentBond.GetSelectedRowDetails(ID);
        }
    
        [HttpPost]
        public GovernmentBondDTO Post([FromBody] GovernmentBondDTO masterMDT)
        {
          
            return _GovernmentBond.GovernmentBondData(masterMDT);
        }

       
        [Route("MasterDeleteModulesDATA")]
        public GovernmentBondDTO MasterDeleteModulesDATA([FromBody]GovernmentBondDTO ID)
        {
           
            return _GovernmentBond.MasterDeleteModulesData(ID);
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
