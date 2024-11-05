using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.Reports.Interface;
using PreadmissionDTOs.NAAC.Admission.Criteria8;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.Reports.Controllers
{
    [Route("api/[controller]")]
    public class Naac_M_811StudentsEnrolledInProgrammeFacade : Controller
    {
        Naac_M_811StudentsEnrolledInProgrammeInterface _Interface;

        public Naac_M_811StudentsEnrolledInProgrammeFacade(Naac_M_811StudentsEnrolledInProgrammeInterface Interfaces)
        {
            _Interface = Interfaces;
        }


        [Route("getdata")]
        public NAAC_811MC_NEET_DTO getdata([FromBody] NAAC_811MC_NEET_DTO data)
        {
            return _Interface.getdata(data);
        }

        [Route("get_811M_report")]
        public Task<NAAC_811MC_NEET_DTO> get_811M_report([FromBody] NAAC_811MC_NEET_DTO data)
        {
            return _Interface.get_811M_report(data);
        }
        [Route("get_813M_report")]
        public Task<NAAC_811MC_NEET_DTO> get_813M_report([FromBody] NAAC_811MC_NEET_DTO data)
        {
            return _Interface.get_813M_report(data);
        }
        [Route("get_819M_report")]
        public Task<NAAC_811MC_NEET_DTO> get_819M_report([FromBody] NAAC_811MC_NEET_DTO data)
        {
            return _Interface.get_819M_report(data);
        }
        [Route("get_8110M_report")]
        public Task<NAAC_811MC_NEET_DTO> get_8110M_report([FromBody] NAAC_811MC_NEET_DTO data)
        {
            return _Interface.get_8110M_report(data);
        }
        [Route("get_813D_report")]
        public Task<NAAC_811MC_NEET_DTO> get_813D_report([FromBody] NAAC_811MC_NEET_DTO data)
        {
            return _Interface.get_813D_report(data);
        }
        [Route("get_815D_report")]
        public Task<NAAC_811MC_NEET_DTO> get_815D_report([FromBody] NAAC_811MC_NEET_DTO data)
        {
            return _Interface.get_815D_report(data);
        }
        [Route("get_816D_report")]
        public Task<NAAC_811MC_NEET_DTO> get_816D_report([FromBody] NAAC_811MC_NEET_DTO data)
        {
            return _Interface.get_816D_report(data);
        }
        //[Route("get_817D_report")]
        //public Task<NAAC_811MC_NEET_DTO> get_817D_report([FromBody] NAAC_811MC_NEET_DTO data)
        //{
        //    return _Interface.get_817D_report(data);
        //}
        [Route("get_8111D_report")]
        public Task<NAAC_811MC_NEET_DTO> get_8111D_report([FromBody] NAAC_811MC_NEET_DTO data)
        {
            return _Interface.get_8111D_report(data);
        }
        [Route("get_818N_report")]
        public Task<NAAC_811MC_NEET_DTO> get_818N_report([FromBody] NAAC_811MC_NEET_DTO data)
        {
            return _Interface.get_818N_report(data);
        }
    }
}
