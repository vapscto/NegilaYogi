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
    public class HSU_323_ResearchProjectsRatioFacade : Controller
    {
        public HSU_323_ResearchProjectsRatioInterface _inter;
        public HSU_323_ResearchProjectsRatioFacade(HSU_323_ResearchProjectsRatioInterface i)
        {
            _inter = i;
        }

        //[HttpPost]
        [Route("loaddata")]
        public HSU_323_ResearchProjectsRatioDTO loaddata([FromBody] HSU_323_ResearchProjectsRatioDTO data)
        {
            return _inter.loaddata(data);
        }
        [Route("save")]
        public HSU_323_ResearchProjectsRatioDTO save([FromBody] HSU_323_ResearchProjectsRatioDTO data)
        {
            return _inter.save(data);
        }
        [Route("deactive")]
        public HSU_323_ResearchProjectsRatioDTO deactive([FromBody] HSU_323_ResearchProjectsRatioDTO data)
        {
            return _inter.deactive(data);
        }
        [Route("EditData")]
        public HSU_323_ResearchProjectsRatioDTO EditData([FromBody] HSU_323_ResearchProjectsRatioDTO data)
        {
            return _inter.EditData(data);
        }

        [Route("deleteuploadfile")]
        public HSU_323_ResearchProjectsRatioDTO deleteuploadfile([FromBody] HSU_323_ResearchProjectsRatioDTO data)
        {
            return _inter.deleteuploadfile(data);
        }

        [Route("viewuploadflies")]
        public HSU_323_ResearchProjectsRatioDTO viewuploadflies([FromBody] HSU_323_ResearchProjectsRatioDTO data)
        {
            return _inter.viewuploadflies(data);
        }
    }
}
