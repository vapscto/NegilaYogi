using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using WebApplication1.Services;
using PreadmissionDTOs;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]

    public class MasterMainMenuFacadeController : Controller
    {
        public MasterMainMenuInterface _MasterModule;


        public MasterMainMenuFacadeController(MasterMainMenuInterface MasterModule)
        {
            _MasterModule = MasterModule;
        }

        [HttpGet]

        [Route("Getdetails/")]
        public MasterMainMenuDTO Getdetails(MasterMainMenuDTO MasterModulesDTO)//int IVRMM_Id
        {

            return _MasterModule.GetMasterMainMenuData(MasterModulesDTO);

        }

        [Route("GetSelectedRowDetails/{id:int}")]
        public MasterMainMenuDTO GetSelectedRowDetails(int ID)
        {
            
            return _MasterModule.GetSelectedRowDetails(ID);
        }

        [HttpPost]
        public MasterMainMenuDTO Post([FromBody] MasterMainMenuDTO masterMDT)
        {
        
            return _MasterModule.MasterMainMenuData(masterMDT);
        }

        [HttpDelete]
        [Route("MasterDeleteMainMenuDTO/{id:int}")]
        public MasterMainMenuDTO MasterDeleteMainMenuDTO(int ID)
        {

            return _MasterModule.MasterDeleteMainMenuDTO(ID);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }


        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }

    }
}
