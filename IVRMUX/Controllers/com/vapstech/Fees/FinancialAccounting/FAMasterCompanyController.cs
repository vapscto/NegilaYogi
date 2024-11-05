using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Fees.FinancialAccounting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees.FinancialAccounting;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Fees.FinancialAccounting
{
    [Route("api/[controller]")]
    public class FAMasterCompanyController : Controller
    {
        FAMasterCompanyDelegate gate = new FAMasterCompanyDelegate();

        [HttpPost]
        [Route("saveDetails")]
        
        public FAMasterCompanyDTO saveDetails([FromBody] FAMasterCompanyDTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("MI_Id"));
            //return gate.savedata(data);

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return gate.savedata(data);
        }


        [HttpPost]
        [Route("saveUserDetails")]

        public FAUserComapnyMappingDTO saveUserDetails([FromBody] FAUserComapnyMappingDTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("MI_Id"));
            //return gate.savedata(data);

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return gate.saveUserDetails(data);
        }

        [Route("editDetails/{id:int}")]
        public FAMasterCompanyDTO editDetails(int id)
        {
            return gate.editdata(id);
        }
        [Route("deleteDetails")]
        public FAMasterCompanyDTO deleteDetails([FromBody] FAMasterCompanyDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return gate.deleteDetails(data);
        }

        [HttpGet]
        [Route("Getdetails/{id:int}")]

        public FAMasterCompanyDTO Getdetails(int id)
        {
            FAMasterCompanyDTO pgmodu = new FAMasterCompanyDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.MI_Id = mid;
            pgmodu.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return gate.Getdetails(pgmodu);
        }

        [HttpGet]
        [Route("GetCompany/{id:int}")]


        public FAUserComapnyMappingDTO GetCompany(int id)
        {
            FAUserComapnyMappingDTO user = new FAUserComapnyMappingDTO();
            int mid= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            user.MI_Id = mid;
            user.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return gate.GetCompany(user);
        }

        [Route("editUserDetails/{id:int}")]
        public FAUserComapnyMappingDTO editUserDetails(int id)
        {
            return gate.editUserdata(id);
        }

        [HttpPost]
        [Route("deleteUserDetails")]
        public FAUserComapnyMappingDTO deleteUserDetails([FromBody] FAUserComapnyMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            
            return gate.deleteUserDetails(data);
        }

        //fafymapping

        [HttpGet]
        [Route("GetInitialData/{id:int}")]
        public FACompanyFYMappingDTO GetInitialData(int id)
        {
            FACompanyFYMappingDTO user = new FACompanyFYMappingDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            user.MI_Id = mid;
            user.MI_Id = mid;
            user.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return gate.GetInitialData(user);
        }


        [HttpPost]
        [Route("saveFYDetails")]

        public FACompanyFYMappingDTO saveFYDetails([FromBody] FACompanyFYMappingDTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("MI_Id"));
            //return gate.savedata(data);

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return gate.saveFYDetails(data);
        }

        [Route("editFYDetails/{id:int}")]
        public FACompanyFYMappingDTO editFYDetails(int id)
        {
            return gate.editFYDetails(id);
        }

        [Route("deleteFYDetails")]
        public FACompanyFYMappingDTO deleteFYDetails([FromBody] FACompanyFYMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return gate.deleteFYDetails(data);
        }
    }
}
