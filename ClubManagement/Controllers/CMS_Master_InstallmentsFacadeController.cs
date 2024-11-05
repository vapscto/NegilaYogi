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
    [Route("api/CMS_Master_InstallmentsFacade")]
    public class CMS_Master_InstallmentsFacadeController : Controller
    {
        public CMS_Master_InstallmentInterface _cms;
        public CMS_Master_InstallmentsFacadeController(CMS_Master_InstallmentInterface cmsdept)
        {
            _cms = cmsdept;
        }
        [HttpGet]
        [Route("loaddata/{id:int}")]
        public CMS_Master_InstallmentsDTO loaddata(int id)
        {
            return _cms.loaddata(id);
            // return _cms.loaddata(id);
        }
        [HttpPost]
        [Route("savedata")]
        public CMS_Master_InstallmentsDTO savedata([FromBody]CMS_Master_InstallmentsDTO data)
        {
            return _cms.savedata(data);
        }
        //deactive
        [Route("deactive")]
        public CMS_Master_InstallmentsDTO deactive([FromBody]CMS_Master_InstallmentsDTO data)
        {
            return _cms.deactive(data);
        }
    }
}