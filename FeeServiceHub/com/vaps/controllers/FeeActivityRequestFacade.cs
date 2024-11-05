using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vapstech.Fee;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeActivityRequestFacade : Controller
    {
        public FeeActivityRequestInterface _org;
        public FeeActivityRequestFacade(FeeActivityRequestInterface orga)
        {
            _org = orga;
        }

        [HttpPost]
        [Route("getalldetails")]
        public Adm_Master_ActivitiesDTO Getdet([FromBody] Adm_Master_ActivitiesDTO data)
        {
            return _org.getdata(data);
        }

        [Route("savedata")]
        public Adm_Master_ActivitiesDTO savedata([FromBody] Adm_Master_ActivitiesDTO data)
        {
            return _org.savedata(data);
        }

        [Route("deletedata")]
        public Adm_Master_ActivitiesDTO deletedata([FromBody] Adm_Master_ActivitiesDTO data)
        {
            return _org.deletedata(data);
        }

        [Route("loadapproval")]
        public Adm_Master_ActivitiesDTO loadapproval([FromBody] Adm_Master_ActivitiesDTO data)
        {
            return _org.loadapproval(data);
        }

        [Route("viewacaclslst")]
        public Adm_Master_ActivitiesDTO viewacaclslst([FromBody] Adm_Master_ActivitiesDTO data)
        {
            return _org.viewacaclslst(data);
        }
        [Route("viewstudentlist")]
        public Adm_Master_ActivitiesDTO viewstudentlist([FromBody] Adm_Master_ActivitiesDTO data)
        {
            return _org.viewstudentlist(data);
        }
        [Route("saveGroupdata")]
        public Adm_Master_ActivitiesDTO saveGroupdata([FromBody] Adm_Master_ActivitiesDTO data)
        {
            return _org.saveGroupdata(data);
        }

        [Route("searching")]
        public Adm_Master_ActivitiesDTO searching([FromBody] Adm_Master_ActivitiesDTO data)
        {
            return _org.searching(data);
        }

        
    }
}
