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
    public class NAAC_MC_312_TeachersResearchFacade : Controller
    {
        NAAC_MC_312_TeachersResearchInterface _Interface;

        public NAAC_MC_312_TeachersResearchFacade(NAAC_MC_312_TeachersResearchInterface Interfaces)
        {
            _Interface = Interfaces;
        }


        [Route("getdata")]
        public UC_312_TeachersResearchDTO getdata([FromBody] UC_312_TeachersResearchDTO data)
        {
            return _Interface.getdata(data);
        }
        [Route("get_312U_report")]
        public Task<UC_312_TeachersResearchDTO> get_312U_report([FromBody] UC_312_TeachersResearchDTO data)
        {
            return _Interface.get_312U_report(data);
        }
        [Route("get_313U_report")]
        public Task<UC_312_TeachersResearchDTO> get_313U_report([FromBody] UC_312_TeachersResearchDTO data)
        {
            return _Interface.get_313U_report(data);
        }
        [Route("get_314U_report")]
        public Task<UC_312_TeachersResearchDTO> get_314U_report([FromBody] UC_312_TeachersResearchDTO data)
        {
            return _Interface.get_314U_report(data);
        }
        [Route("get_315U_report")]
        public Task<UC_312_TeachersResearchDTO> get_315U_report([FromBody] UC_312_TeachersResearchDTO data)
        {
            return _Interface.get_315U_report(data);
        }
        [Route("get_316U_report")]
        public Task<UC_312_TeachersResearchDTO> get_316U_report([FromBody] UC_312_TeachersResearchDTO data)
        {
            return _Interface.get_316U_report(data);
        }
        [Route("get_342U_report")]
        public Task<UC_312_TeachersResearchDTO> get_342U_report([FromBody] UC_312_TeachersResearchDTO data)
        {
            return _Interface.get_342U_report(data);
        }
        [Route("get_343U_report")]
        public Task<UC_312_TeachersResearchDTO> get_343U_report([FromBody] UC_312_TeachersResearchDTO data)
        {
            return _Interface.get_343U_report(data);
        }
        [Route("get_372U_report")]
        public Task<UC_312_TeachersResearchDTO> get_372U_report([FromBody] UC_312_TeachersResearchDTO data)
        {
            return _Interface.get_372U_report(data);
        }
        [Route("get_362U_report")]
        public Task<UC_312_TeachersResearchDTO> get_362U_report([FromBody] UC_312_TeachersResearchDTO data)
        {
            return _Interface.get_362U_report(data);
        }
        [Route("get_352U_report")]
        public Task<UC_312_TeachersResearchDTO> get_352U_report([FromBody] UC_312_TeachersResearchDTO data)
        {
            return _Interface.get_352U_report(data);
        }
        [Route("get_371U_report")]
        public Task<UC_312_TeachersResearchDTO> get_371U_report([FromBody] UC_312_TeachersResearchDTO data)
        {
            return _Interface.get_371U_report(data);
        }
        [Route("get_341U_report")]
        public Task<UC_312_TeachersResearchDTO> get_341U_report([FromBody] UC_312_TeachersResearchDTO data)
        {
            return _Interface.get_341U_report(data);
        }
        [Route("NAAC_MC_345_TeachersResearchReport")]
        public Task<UC_312_TeachersResearchDTO> NAAC_MC_345_TeachersResearchReport([FromBody] UC_312_TeachersResearchDTO data)
        {
            return _Interface.NAAC_MC_345_TeachersResearchReport(data);
        }
        [Route("NAAC_MC_346_TeachersResearchReport")]
        public Task<UC_312_TeachersResearchDTO> NAAC_MC_346_TeachersResearchReport([FromBody] UC_312_TeachersResearchDTO data)
        {
            return _Interface.NAAC_MC_346_TeachersResearchReport(data);
        }
    }
}
