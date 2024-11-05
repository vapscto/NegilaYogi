using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.Reports.Interface;
using PreadmissionDTOs.NAAC.University;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.Reports.Controllers
{
    [Route("api/[controller]")]
    public class NAAC_HSU_323_ResearchProjectsRatioFacade : Controller
    {
        NAAC_HSU_323_ResearchProjectsRatioInterface _Interface;

        public NAAC_HSU_323_ResearchProjectsRatioFacade(NAAC_HSU_323_ResearchProjectsRatioInterface Interfaces)
        {
            _Interface = Interfaces;
        }
        [Route("getdata")]
        public HSU_323_ResearchProjectsRatioDTO getdata([FromBody] HSU_323_ResearchProjectsRatioDTO data)
        {
            return _Interface.getdata(data);
        }
        [Route("get_323U_report")]
        public Task<HSU_323_ResearchProjectsRatioDTO> get_323U_report([FromBody] HSU_323_ResearchProjectsRatioDTO data)
        {
            return _Interface.get_323U_report(data);
        }
        [Route("get_334U_report")]
        public Task<HSU_323_ResearchProjectsRatioDTO> get_334U_report([FromBody] HSU_323_ResearchProjectsRatioDTO data)
        {
            return _Interface.get_334U_report(data);
        }
        [Route("get_344U_report")]
        public Task<HSU_323_ResearchProjectsRatioDTO> get_344U_report([FromBody] HSU_323_ResearchProjectsRatioDTO data)
        {
            return _Interface.get_344U_report(data);
        }
        [Route("get_333U_report")]
        public Task<HSU_323_ResearchProjectsRatioDTO> get_333U_report([FromBody] HSU_323_ResearchProjectsRatioDTO data)
        {
            return _Interface.get_333U_report(data);
        }
        [Route("get_349U_report")]
        public Task<HSU_323_ResearchProjectsRatioDTO> get_349U_report([FromBody] HSU_323_ResearchProjectsRatioDTO data)
        {
            return _Interface.get_349U_report(data);
        }
        [Route("get_348U_report")]
        public Task<HSU_323_ResearchProjectsRatioDTO> get_348U_report([FromBody] HSU_323_ResearchProjectsRatioDTO data)
        {
            return _Interface.get_348U_report(data);
        }
    }
}
