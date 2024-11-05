using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.Admission.Interface;
using PreadmissionDTOs.NAAC.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.Admission.FacadeController
{
    [Route("api/[controller]")]
    public class NaacIPRFacade : Controller
    {
        public NaacIPRInterface inter;

        public NaacIPRFacade(NaacIPRInterface q)
        {
            inter = q;
        }

        [Route("loaddata")]
        public NAAC_AC_IPR_322_DTO loaddata([FromBody] NAAC_AC_IPR_322_DTO data)
        {
            return inter.loaddata(data);
        }
        [Route("save")]
        public NAAC_AC_IPR_322_DTO save([FromBody] NAAC_AC_IPR_322_DTO data)
        {
            return inter.save(data);
        }
        [Route("getcomment")]
        public NAAC_AC_IPR_322_DTO getcomment([FromBody] NAAC_AC_IPR_322_DTO data)
        {
            return inter.getcomment(data);
        }
        [Route("savemedicaldatawisecomments")]
        public NAAC_AC_IPR_322_DTO savemedicaldatawisecomments([FromBody] NAAC_AC_IPR_322_DTO data)
        {
            return inter.savemedicaldatawisecomments(data);
        }
        [Route("savefilewisecomments")]
        public NAAC_AC_IPR_322_DTO savefilewisecomments([FromBody] NAAC_AC_IPR_322_DTO data)
        {
            return inter.savefilewisecomments(data);
        }
        [Route("getfilecomment")]
        public NAAC_AC_IPR_322_DTO getfilecomment([FromBody] NAAC_AC_IPR_322_DTO data)
        {
            return inter.getfilecomment(data);
        }

        [Route("deactive")]
        public NAAC_AC_IPR_322_DTO deactive([FromBody] NAAC_AC_IPR_322_DTO data)
        {
            return inter.deactive(data);
        }

        [Route("EditData")]
        public NAAC_AC_IPR_322_DTO EditData([FromBody]NAAC_AC_IPR_322_DTO category)
        {
            return inter.EditData(category);
        }

        [Route("viewuploadflies")]
        public NAAC_AC_IPR_322_DTO viewuploadflies([FromBody] NAAC_AC_IPR_322_DTO data)
        {
            return inter.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public NAAC_AC_IPR_322_DTO deleteuploadfile([FromBody] NAAC_AC_IPR_322_DTO data)
        {
            return inter.deleteuploadfile(data);
        }


    }
}
