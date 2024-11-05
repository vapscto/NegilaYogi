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
    public class Naac_MC_IctFacility441Facade : Controller
    {
        public Naac_MC_IctFacility441Interface inter;
        public Naac_MC_IctFacility441Facade(Naac_MC_IctFacility441Interface q)
        {
            inter = q;
        }

        [Route("loaddata")]
        public Naac_MC_IctFacility441_DTO loaddata([FromBody] Naac_MC_IctFacility441_DTO data)
        {
            return inter.loaddata(data);
        }
        [Route("save")]
        public Naac_MC_IctFacility441_DTO save([FromBody] Naac_MC_IctFacility441_DTO data)
        {
            return inter.save(data);
        }
       
        [Route("EditData")]

        public Naac_MC_IctFacility441_DTO EditData([FromBody] Naac_MC_IctFacility441_DTO data)
        {
            return inter.EditData(data);
        }

        [Route("viewuploadflies")]
        public Naac_MC_IctFacility441_DTO viewuploadflies([FromBody] Naac_MC_IctFacility441_DTO data)
        {
            return inter.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public Naac_MC_IctFacility441_DTO deleteuploadfile([FromBody] Naac_MC_IctFacility441_DTO data)
        {
            return inter.deleteuploadfile(data);
        }
    }
}
