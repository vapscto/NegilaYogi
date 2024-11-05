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
    public class NAAC_811MC_NEETFacade : Controller
    {

        public NAAC_811MC_NEETInterface _Iobj;
        public NAAC_811MC_NEETFacade(NAAC_811MC_NEETInterface para)
        {
            _Iobj = para;
        }
        // GET: api/<controller>
        [Route("loaddata")]
        public Task<NAAC_811MC_NEET_DTO> loaddata([FromBody] NAAC_811MC_NEET_DTO data)
        {
            return _Iobj.loaddata(data);
        }
        [Route("savedata")]
        public NAAC_811MC_NEET_DTO savedata([FromBody] NAAC_811MC_NEET_DTO data)
        {
            return _Iobj.savedata(data);
        }
        [Route("editdata")]
        public NAAC_811MC_NEET_DTO editdata([FromBody] NAAC_811MC_NEET_DTO data)
        {
            return _Iobj.editdata(data);
        }
        [Route("deactivY")]
        public NAAC_811MC_NEET_DTO deactivY([FromBody] NAAC_811MC_NEET_DTO data)
        {
            return _Iobj.deactivY(data);
        }
        [Route("viewuploadflies")]
        public NAAC_811MC_NEET_DTO viewuploadflies([FromBody] NAAC_811MC_NEET_DTO data)
        {
            return _Iobj.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public NAAC_811MC_NEET_DTO deleteuploadfile([FromBody] NAAC_811MC_NEET_DTO data)
        {
            return _Iobj.deleteuploadfile(data);
        }
        [Route("getfilecomment")]
        public NAAC_811MC_NEET_DTO getfilecomment([FromBody] NAAC_811MC_NEET_DTO data)
        {
            return _Iobj.getfilecomment(data);
        }
        [Route("savecomments")]
        public NAAC_811MC_NEET_DTO savecomments([FromBody] NAAC_811MC_NEET_DTO data)
        {
            return _Iobj.savecomments(data);
        }
        [Route("savefilewisecomments")]
        public NAAC_811MC_NEET_DTO savefilewisecomments([FromBody] NAAC_811MC_NEET_DTO data)
        {
            return _Iobj.savefilewisecomments(data);
        }
    }


}
