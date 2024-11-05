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
    public class NAAC_422_Clinical_Laboratoryfacade : Controller
    {

        public NAAC_422_Clinical_LaboratoryInterface _Interface;
        public NAAC_422_Clinical_Laboratoryfacade(NAAC_422_Clinical_LaboratoryInterface Para)
        {
            _Interface = Para;
        }


        [Route("loaddata")]
        public NAAC_MC_422_Clinical_Laboratory_DTO loaddata([FromBody] NAAC_MC_422_Clinical_Laboratory_DTO data)
        {
            return _Interface.loaddata(data);
        }
        [Route("savedata")]
        public NAAC_MC_422_Clinical_Laboratory_DTO savedata([FromBody] NAAC_MC_422_Clinical_Laboratory_DTO data)
        {
            return _Interface.savedata(data);
        }
        [Route("editdata")]
        public NAAC_MC_422_Clinical_Laboratory_DTO editdata([FromBody]NAAC_MC_422_Clinical_Laboratory_DTO data)
        {
            return _Interface.editdata(data);
        }
        [Route("deactive_Y")]
        public NAAC_MC_422_Clinical_Laboratory_DTO deactive_Y([FromBody]NAAC_MC_422_Clinical_Laboratory_DTO data)
        {
            return _Interface.deactive_Y(data);
        }
        [Route("viewuploadflies")]
        public NAAC_MC_422_Clinical_Laboratory_DTO viewuploadflies([FromBody]NAAC_MC_422_Clinical_Laboratory_DTO data)
        {
            return _Interface.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public NAAC_MC_422_Clinical_Laboratory_DTO deleteuploadfile([FromBody]NAAC_MC_422_Clinical_Laboratory_DTO data)
        {
            return _Interface.deleteuploadfile(data);
        }
        
    }
}
