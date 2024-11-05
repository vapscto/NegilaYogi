using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.University.Interface;
using PreadmissionDTOs.NAAC.University;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.University.FacadeController
{
    [Route("api/[controller]")]
    public class MC_314_ResearchAssociatesFacade : Controller
    {
        public MC_314_ResearchAssociatesInterface _inter;
        public MC_314_ResearchAssociatesFacade(MC_314_ResearchAssociatesInterface i)
        {
            _inter = i;
        }

        [Route("loaddata")]
        public MC_314_ResearchAssociatesDTO loaddata([FromBody] MC_314_ResearchAssociatesDTO data)
        {
            return _inter.loaddata(data);
        }
        [Route("save")]
        public MC_314_ResearchAssociatesDTO save([FromBody] MC_314_ResearchAssociatesDTO data)
        {
            return _inter.save(data);
        }
        [Route("deactive")]
        public MC_314_ResearchAssociatesDTO deactive([FromBody] MC_314_ResearchAssociatesDTO data)
        {
            return _inter.deactive(data);
        }
        [Route("EditData")]
        public MC_314_ResearchAssociatesDTO EditData([FromBody] MC_314_ResearchAssociatesDTO data)
        {
            return _inter.EditData(data);
        }



        [Route("deleteuploadfile")]
        public MC_314_ResearchAssociatesDTO deleteuploadfile([FromBody] MC_314_ResearchAssociatesDTO data)
        {
            return _inter.deleteuploadfile(data);
        }

        [Route("viewuploadflies")]
        public MC_314_ResearchAssociatesDTO viewuploadflies([FromBody] MC_314_ResearchAssociatesDTO data)
        {
            return _inter.viewuploadflies(data);
        }
    }
}
