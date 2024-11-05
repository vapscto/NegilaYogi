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
    public class NaacLinkageFacade : Controller
    {
        public NaacLinkageInterface inter;
        public NaacLinkageFacade(NaacLinkageInterface h)
        {
            inter = h;
        }

        [Route("loaddata")]
       public NAAC_AC_351_Linkage_DTO loaddata([FromBody] NAAC_AC_351_Linkage_DTO data)
        {
            return inter.loaddata(data);
        }
        [Route("save")]
        public NAAC_AC_351_Linkage_DTO save([FromBody] NAAC_AC_351_Linkage_DTO data)
        {
            return inter.save(data);
        }
        [Route("EditData")]
        public NAAC_AC_351_Linkage_DTO EditData([FromBody] NAAC_AC_351_Linkage_DTO data)
        {
            return inter.EditData(data);
        }
        [Route("getcomment")]
        public NAAC_AC_351_Linkage_DTO getcomment([FromBody] NAAC_AC_351_Linkage_DTO data)
        {
            return inter.getcomment(data);
        }
        [Route("savefilewisecomments")]
        public NAAC_AC_351_Linkage_DTO savefilewisecomments([FromBody] NAAC_AC_351_Linkage_DTO data)
        {
            return inter.savefilewisecomments(data);
        }
        [Route("getfilecomment")]
        public NAAC_AC_351_Linkage_DTO getfilecomment([FromBody] NAAC_AC_351_Linkage_DTO data)
        {
            return inter.getfilecomment(data);
        }
        [Route("savemedicaldatawisecomments")]
        public NAAC_AC_351_Linkage_DTO savemedicaldatawisecomments([FromBody] NAAC_AC_351_Linkage_DTO data)
        {
            return inter.savemedicaldatawisecomments(data);
        }

        [Route("deactive")]
        public NAAC_AC_351_Linkage_DTO deactive([FromBody] NAAC_AC_351_Linkage_DTO data)
        {
            return inter.deactive(data);
        }

        [Route("deleteuploadfile")]
        public NAAC_AC_351_Linkage_DTO deleteuploadfile([FromBody] NAAC_AC_351_Linkage_DTO data)
        {
            return inter.deleteuploadfile(data);
        }

        [Route("viewuploadflies")]
        public NAAC_AC_351_Linkage_DTO viewuploadflies([FromBody] NAAC_AC_351_Linkage_DTO data)
        {
            return inter.viewuploadflies(data);
        }
    }
}
