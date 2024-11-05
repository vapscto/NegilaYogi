﻿using System;
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

    public class InstituteSubMenuFacadeController : Controller
    {
        public InstituteSubMenuInterface _MasterModule;


        public InstituteSubMenuFacadeController(InstituteSubMenuInterface MasterModule)
        {
            _MasterModule = MasterModule;
        }

        [HttpGet]

        [Route("GetSelectedRowDetails/{id:int}")]
        public InstituteMainMenuDTO GetSelectedRowDetails(int ID)
        {
            
            return _MasterModule.GetSelectedRowDetails(ID);
        }

        [Route("getMenudetailsByModuleId/")]
        public InstituteMainMenuDTO getMenudetailsByModuleId([FromBody] InstituteMainMenuDTO ID)
        {

            return _MasterModule.getMenudetailsByModuleId(ID);
        }


        [HttpPost]
        [Route("getmoduledetails/")]
        public InstituteMainMenuDTO getmoduledetails([FromBody] InstituteMainMenuDTO data)
        {

            return _MasterModule.getmoduledetails(data);
        }

      
        [Route("getSubMenudetailsByMainMenuId")]
        public InstituteMainMenuDTO getSubMenudetailsByMainMenuId([FromBody] InstituteMainMenuDTO data)
        {

            return _MasterModule.getSubMenudetailsByMainMenuId(data);
        }

      
        public InstituteMainMenuDTO Post([FromBody] InstituteMainMenuDTO masterMDT)
        {
        
            return _MasterModule.MasterMainMenuData(masterMDT);
        }

     

        [Route("Getdetails/")]
        public InstituteMainMenuDTO Getdetails([FromBody]  InstituteMainMenuDTO InstituteMainMenuDTO)//int IVRMM_Id
        {

            return _MasterModule.GetMasterSubMenuData(InstituteMainMenuDTO);

        }

        [HttpDelete]
        [Route("MasterDeleteMainMenuDTO/{id:int}")]
        public InstituteMainMenuDTO MasterDeleteMainMenuDTO(int ID)
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

        // 
        [Route("Onchangedetails")]
        public InstituteMainMenuDTO orderchangedata([FromBody]InstituteMainMenuDTO dto)
        {
            return _MasterModule.changeorderData(dto);
        }

    }
}