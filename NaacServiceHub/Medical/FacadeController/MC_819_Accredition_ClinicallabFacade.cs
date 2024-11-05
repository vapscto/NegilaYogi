using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.Medical.Interface;
using PreadmissionDTOs.NAAC.Medical;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.Medical.FacadeController
{
    [Route("api/[controller]")]
    public class MC_819_Accredition_ClinicallabFacade : Controller
    {
        public MC_819_Accredition_ClinicallabInterface _Interface;
        public MC_819_Accredition_ClinicallabFacade(MC_819_Accredition_ClinicallabInterface Para)
        {
            _Interface = Para;
        }


        [Route("loaddata")]
        public MC_819_Accredition_ClinicallabDTO loaddata([FromBody] MC_819_Accredition_ClinicallabDTO data)
        {
            return _Interface.loaddata(data);
        }
        [Route("savedata")]
        public MC_819_Accredition_ClinicallabDTO savedata([FromBody] MC_819_Accredition_ClinicallabDTO data)
        {
            return _Interface.savedata(data);
        }
        [Route("savedata1")]
        public MC_819_Accredition_ClinicallabDTO savedata1([FromBody] MC_819_Accredition_ClinicallabDTO data)
        {
            return _Interface.savedata1(data);
        }
        [Route("savedata2")]
        public MC_819_Accredition_ClinicallabDTO savedata2([FromBody] MC_819_Accredition_ClinicallabDTO data)
        {
            return _Interface.savedata2(data);
        }
        [Route("savedata3")]
        public MC_819_Accredition_ClinicallabDTO savedata3([FromBody] MC_819_Accredition_ClinicallabDTO data)
        {
            return _Interface.savedata3(data);
        }
        [Route("editdata")]
        public MC_819_Accredition_ClinicallabDTO editdata([FromBody]MC_819_Accredition_ClinicallabDTO data)
        {
            return _Interface.editdata(data);
        }
        [Route("deactivate")]
        public MC_819_Accredition_ClinicallabDTO deactivate([FromBody]MC_819_Accredition_ClinicallabDTO data)
        {
            return _Interface.deactivate(data);
        }
        [Route("getcomment")]
        public MC_819_Accredition_ClinicallabDTO getcomment([FromBody]MC_819_Accredition_ClinicallabDTO data)
        {
            return _Interface.getcomment(data);
        }
        [Route("savecomments")]
        public MC_819_Accredition_ClinicallabDTO savecomments([FromBody]MC_819_Accredition_ClinicallabDTO data)
        {
            return _Interface.savecomments(data);
        }
    }
}
