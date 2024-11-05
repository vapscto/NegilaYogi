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
    public class NAAC_MC_436_EContentFacade : Controller
    {
        public NAAC_MC_436_EContentInterface _InterFace;
        public NAAC_MC_436_EContentFacade(NAAC_MC_436_EContentInterface para)
        {
            _InterFace = para;
        }

        [Route("loaddata")]
        public NAAC_MC_436_EContent_DTO loaddata([FromBody]NAAC_MC_436_EContent_DTO data)
        {
            return _InterFace.loaddata(data);
        }

        [Route("savedata")]
        public NAAC_MC_436_EContent_DTO savedata([FromBody]NAAC_MC_436_EContent_DTO data)
        {
            return _InterFace.savedata(data);
        }

        [Route("editdata")]
        public NAAC_MC_436_EContent_DTO editdata([FromBody] NAAC_MC_436_EContent_DTO data)
        {
            return _InterFace.editdata(data);
        }

        [Route("deactiveStudent")]
        public NAAC_MC_436_EContent_DTO deactiveStudent([FromBody] NAAC_MC_436_EContent_DTO data)
        {
            return _InterFace.deactiveStudent(data);
        }

        [Route("viewuploadflies")]
        public NAAC_MC_436_EContent_DTO viewuploadflies([FromBody] NAAC_MC_436_EContent_DTO data)
        {
            return _InterFace.viewuploadflies(data);
        }

        [Route("deleteuploadfile")]
        public NAAC_MC_436_EContent_DTO deleteuploadfile([FromBody] NAAC_MC_436_EContent_DTO data)
        {
            return _InterFace.deleteuploadfile(data);
        }
    }
}
