using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.Admission.Interface.Criteria8;
using PreadmissionDTOs.NAAC.Admission.Criteria8;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.Admission.FacadeController.Criteria8
{
    [Route("api/[controller]")]
    public class NaacPGDegrees813Facade : Controller
    {
        public NaacPGDegrees813Interface _inter;
        public NaacPGDegrees813Facade(NaacPGDegrees813Interface i)
        {
            _inter = i;
        }

        [Route("loaddata")]
        public NAAC_MC_813_PGDegrees_DTO loaddata([FromBody] NAAC_MC_813_PGDegrees_DTO data)
        {
            return _inter.loaddata(data);
        }
        [Route("save")]
        public NAAC_MC_813_PGDegrees_DTO save([FromBody] NAAC_MC_813_PGDegrees_DTO data)
        {
            return _inter.save(data);
        }
        [Route("deactive")]
        public NAAC_MC_813_PGDegrees_DTO deactive([FromBody] NAAC_MC_813_PGDegrees_DTO data)
        {
            return _inter.deactive(data);
        }
        [Route("EditData")]
        public NAAC_MC_813_PGDegrees_DTO EditData([FromBody] NAAC_MC_813_PGDegrees_DTO data)
        {
            return _inter.EditData(data);
        }



        [Route("deleteuploadfile")]
        public NAAC_MC_813_PGDegrees_DTO deleteuploadfile([FromBody] NAAC_MC_813_PGDegrees_DTO data)
        {
            return _inter.deleteuploadfile(data);
        }

        [Route("viewuploadflies")]
        public NAAC_MC_813_PGDegrees_DTO viewuploadflies([FromBody] NAAC_MC_813_PGDegrees_DTO data)
        {
            return _inter.viewuploadflies(data);
        }
        [Route("getcomment")]
        public NAAC_MC_813_PGDegrees_DTO getcomment([FromBody] NAAC_MC_813_PGDegrees_DTO data)
        {
            return _inter.getcomment(data);
        }
        [Route("getfilecomment")]
        public NAAC_MC_813_PGDegrees_DTO getfilecomment([FromBody] NAAC_MC_813_PGDegrees_DTO data)
        {
            return _inter.getfilecomment(data);
        }
        [Route("savecomments")]
        public NAAC_MC_813_PGDegrees_DTO savecomments([FromBody] NAAC_MC_813_PGDegrees_DTO data)
        {
            return _inter.savecomments(data);
        }
        [Route("savefilewisecomments")]
        public NAAC_MC_813_PGDegrees_DTO savefilewisecomments([FromBody] NAAC_MC_813_PGDegrees_DTO data)
        {
            return _inter.savefilewisecomments(data);
        }
    }
}
