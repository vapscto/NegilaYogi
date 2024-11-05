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
    public class DC_8111_ExpenditureFacade : Controller
    {
        public DC_8111_ExpenditureInterface _Iobj;
        public DC_8111_ExpenditureFacade(DC_8111_ExpenditureInterface para)
        {
            _Iobj = para;
        }
        // GET: api/<controller>
        [Route("loaddata")]
        public Task<DC_8111_ExpenditureDTO> loaddata([FromBody] DC_8111_ExpenditureDTO data)
        {
            return _Iobj.loaddata(data);
        }
        [Route("savedata")]
        public DC_8111_ExpenditureDTO savedata([FromBody] DC_8111_ExpenditureDTO data)
        {
            return _Iobj.savedata(data);
        }
        [Route("editdata")]
        public DC_8111_ExpenditureDTO editdata([FromBody] DC_8111_ExpenditureDTO data)
        {
            return _Iobj.editdata(data);
        }
        [Route("deactivY")]
        public DC_8111_ExpenditureDTO deactivY([FromBody] DC_8111_ExpenditureDTO data)
        {
            return _Iobj.deactivY(data);
        }
        [Route("viewuploadflies")]
        public DC_8111_ExpenditureDTO viewuploadflies([FromBody] DC_8111_ExpenditureDTO data)
        {
            return _Iobj.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public DC_8111_ExpenditureDTO deleteuploadfile([FromBody] DC_8111_ExpenditureDTO data)
        {
            return _Iobj.deleteuploadfile(data);
        }
        [Route("getcomment")]
        public DC_8111_ExpenditureDTO getcomment([FromBody] DC_8111_ExpenditureDTO data)
        {
            return _Iobj.getcomment(data);
        }
        [Route("getfilecomment")]
        public DC_8111_ExpenditureDTO getfilecomment([FromBody] DC_8111_ExpenditureDTO data)
        {
            return _Iobj.getfilecomment(data);
        }
        [Route("savecomments")]
        public DC_8111_ExpenditureDTO savecomments([FromBody] DC_8111_ExpenditureDTO data)
        {
            return _Iobj.savecomments(data);
        }
        [Route("savefilewisecomments")]
        public DC_8111_ExpenditureDTO savefilewisecomments([FromBody] DC_8111_ExpenditureDTO data)
        {
            return _Iobj.savefilewisecomments(data);
        }
    }
}
