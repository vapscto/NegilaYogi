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
    public class Naac_ICTfacade : Controller
    {
        // GET: api/<controller>
        public Naac_ICTInterface _Iobj;
        public Naac_ICTfacade(Naac_ICTInterface para)
        {
            _Iobj = para;
        }

        [Route("loaddata")]
        public Naac_ICT_DTO loaddata([FromBody] Naac_ICT_DTO data)
        {
            return _Iobj.loaddata(data);
        }

        [Route("savedata")]
        public Naac_ICT_DTO savedata([FromBody] Naac_ICT_DTO data)
        {
            return _Iobj.savedata(data);
        }

        [Route("savefilewisecomments")]
        public Naac_ICT_DTO savefilewisecomments([FromBody] Naac_ICT_DTO data)
        {
            return _Iobj.savefilewisecomments(data);
        }

        [Route("savemedicaldatawisecomments")]
        public Naac_ICT_DTO savemedicaldatawisecomments([FromBody] Naac_ICT_DTO data)
        {
            return _Iobj.savemedicaldatawisecomments(data);
        }

        [Route("getfilecomment")]
        public Naac_ICT_DTO getfilecomment([FromBody] Naac_ICT_DTO data)
        {
            return _Iobj.getfilecomment(data);
        }

        [Route("getcomment")]
        public Naac_ICT_DTO getcomment([FromBody] Naac_ICT_DTO data)
        {
            return _Iobj.getcomment(data);
        }

        [Route("editdata")]
        public Naac_ICT_DTO editdata([FromBody] Naac_ICT_DTO data)
        {
            return _Iobj.editdata(data);
        }

        [Route("deactivRow")]
        public Naac_ICT_DTO deactivRow([FromBody] Naac_ICT_DTO data)
        {
            return _Iobj.deactivRow(data);
        }
        [Route("viewuploadflies")]
        public Naac_ICT_DTO viewuploadflies([FromBody] Naac_ICT_DTO data)
        {
            return _Iobj.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public Naac_ICT_DTO deleteuploadfile([FromBody] Naac_ICT_DTO data)
        {
            return _Iobj.deleteuploadfile(data);
        }
    }
}
