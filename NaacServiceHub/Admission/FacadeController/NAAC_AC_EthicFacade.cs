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
    public class NAAC_AC_EthicFacade : Controller
    {
        public NAAC_AC_EthicInterface inter;

        public NAAC_AC_EthicFacade(NAAC_AC_EthicInterface q)
        {
            inter = q;
        }

        [Route("loaddata")]
        public NAAC_AC_331_DTO loaddata([FromBody] NAAC_AC_331_DTO data)
        {
            return inter.loaddata(data);
        }
        [Route("save")]
        public NAAC_AC_331_DTO save([FromBody] NAAC_AC_331_DTO data)
        {
            return inter.save(data);
        }
        [Route("getcomment")]
        public NAAC_AC_331_DTO getcomment([FromBody] NAAC_AC_331_DTO data)
        {
            return inter.getcomment(data);
        }
        [Route("getfilecomment")]
        public NAAC_AC_331_DTO getfilecomment([FromBody] NAAC_AC_331_DTO data)
        {
            return inter.getfilecomment(data);
        }
        [Route("savefilewisecomments")]
        public NAAC_AC_331_DTO savefilewisecomments([FromBody] NAAC_AC_331_DTO data)
        {
            return inter.savefilewisecomments(data);
        }
        [Route("savemedicaldatawisecomments")]
        public NAAC_AC_331_DTO savemedicaldatawisecomments([FromBody] NAAC_AC_331_DTO data)
        {
            return inter.savemedicaldatawisecomments(data);
        }
        [Route("deactive")]
        public NAAC_AC_331_DTO deactive([FromBody] NAAC_AC_331_DTO data)
        {
            return inter.deactive(data);
        }

        [Route("EditData")]
        public NAAC_AC_331_DTO EditData([FromBody]NAAC_AC_331_DTO category)
        {
            return inter.EditData(category);
        }


        [Route("deleteuploadfile")]
        public NAAC_AC_331_DTO deleteuploadfile([FromBody] NAAC_AC_331_DTO data)
        {
            return inter.deleteuploadfile(data);
        }

        [Route("viewuploadflies")]
        public NAAC_AC_331_DTO viewuploadflies([FromBody] NAAC_AC_331_DTO data)
        {
            return inter.viewuploadflies(data);
        }

    }
}
