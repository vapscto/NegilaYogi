using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeeServiceHub.com.vaps.interfaces.FinancialAccounting;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees.FinancialAccounting;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers.FinancialAccounting
{
    [Route("api/[controller]")]
    public class FAMasterCompanyFacadeController : Controller
    {
        public FAMasterCompanyInterface _feemaster;

        public FAMasterCompanyFacadeController(FAMasterCompanyInterface feemaster)
        {
            _feemaster = feemaster;
        }

        [Route("Getdetails")]
        public FAMasterCompanyDTO Getdetails([FromBody] FAMasterCompanyDTO data)
        {
            return _feemaster.Getdetails(data);
        }

        [Route("GetCompany")]
        public FAUserComapnyMappingDTO GetCompany([FromBody] FAUserComapnyMappingDTO data)
        {
            return _feemaster.GetCompany(data);
        }

        [HttpPost]
        [Route("saveDetails")]
        public FAMasterCompanyDTO saveDetails([FromBody]FAMasterCompanyDTO data)
        {
            return _feemaster.saveDetails(data);
        }



        [HttpPost]
        [Route("saveUserDetails")]
        public FAUserComapnyMappingDTO saveUserDetails([FromBody]FAUserComapnyMappingDTO data)
        {
            return _feemaster.saveUserDetails(data);
        }

        [Route("editDetails/{id:int}")]
        public FAMasterCompanyDTO editDetails(int id)
        {
            return _feemaster.editDetails(id);
        }

        [Route("editUserDetails/{id:int}")]
        public FAUserComapnyMappingDTO editUserDetails(int id)
        {
            return _feemaster.editUserDetails(id);
        }



        [Route("deleteDetails")]
        public FAMasterCompanyDTO deleteDetails([FromBody] FAMasterCompanyDTO data)
        {
            return _feemaster.deleteDetails(data);
        }

        
        [Route("deleteUserDetails")]
        public FAUserComapnyMappingDTO deleteUserDetails([FromBody] FAUserComapnyMappingDTO data)
        {
            return _feemaster.deleteUserDetails(data);
        }

        [HttpPost]
        [Route("saveFYDetails")]
        public FACompanyFYMappingDTO saveFYDetails([FromBody]FACompanyFYMappingDTO data)
        {
            return _feemaster.saveFYDetails(data);
        }

        [Route("deleteFYDetails")]
        public FACompanyFYMappingDTO deleteFYDetails([FromBody] FACompanyFYMappingDTO data)
        {
            return _feemaster.deleteFYDetails(data);
        }

        [Route("editFYDetails/{id:int}")]
        public FACompanyFYMappingDTO editFYDetails(int id)
        {
            return _feemaster.editFYDetails(id);
        }

        [Route("GetInitialData")]
        public FACompanyFYMappingDTO GetInitialData([FromBody] FACompanyFYMappingDTO data)
        {
            return _feemaster.GetInitialData(data);
        }
    }
}
