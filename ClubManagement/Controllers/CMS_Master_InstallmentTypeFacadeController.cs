using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClubManagement.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.ClubManagement;

namespace ClubManagement.Controllers
{
    [Produces("application/json")]
    [Route("api/CMS_Master_InstallmentTypeFacade")]
    public class CMS_Master_InstallmentTypeFacadeController : Controller
    {
        public CMS_Master_InstallmentTypeINTERFACE _cms;

        public CMS_Master_InstallmentTypeFacadeController(CMS_Master_InstallmentTypeINTERFACE cmsdept)
        {
            _cms = cmsdept;
        }
        [HttpGet]
        [Route("loaddata/{id:int}")]
        public CMS_Master_InstallmentTypeDTO loaddata(int id)
        {
            return _cms.loaddata(id);
            // return _cms.loaddata(id);
        }
        [HttpPost]
        [Route("savedata")]
        public CMS_Master_InstallmentTypeDTO savedata([FromBody]CMS_Master_InstallmentTypeDTO data)
        {
            return _cms.savedata(data);
        }
        //deactive
        [Route("deactive")]
        public CMS_Master_InstallmentTypeDTO deactive([FromBody]CMS_Master_InstallmentTypeDTO data)
        {
            return _cms.deactive(data);
        }
    }
}