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
    public class NC_818_EmpCommitteesFacade : Controller
    {
        public NC_818_EmpCommitteesInterface _Iobj;
        public NC_818_EmpCommitteesFacade(NC_818_EmpCommitteesInterface para)
        {
            _Iobj = para;
        }
        // GET: api/<controller>
        [Route("loaddata")]
        public Task<NC_818_EmpCommitteesDTO> loaddata([FromBody] NC_818_EmpCommitteesDTO data)
        {
            return _Iobj.loaddata(data);
        }
        [Route("savedata")]
        public NC_818_EmpCommitteesDTO savedata([FromBody] NC_818_EmpCommitteesDTO data)
        {
            return _Iobj.savedata(data);
        }
        [Route("editdata")]
        public NC_818_EmpCommitteesDTO editdata([FromBody] NC_818_EmpCommitteesDTO data)
        {
            return _Iobj.editdata(data);
        }
        [Route("deactivY")]
        public NC_818_EmpCommitteesDTO deactivY([FromBody] NC_818_EmpCommitteesDTO data)
        {
            return _Iobj.deactivY(data);
        }
        [Route("viewuploadflies")]
        public NC_818_EmpCommitteesDTO viewuploadflies([FromBody] NC_818_EmpCommitteesDTO data)
        {
            return _Iobj.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public NC_818_EmpCommitteesDTO deleteuploadfile([FromBody] NC_818_EmpCommitteesDTO data)
        {
            return _Iobj.deleteuploadfile(data);
        }
        [Route("getcomment")]
        public NC_818_EmpCommitteesDTO getcomment([FromBody] NC_818_EmpCommitteesDTO data)
        {
            return _Iobj.getcomment(data);
        }
        [Route("getfilecomment")]
        public NC_818_EmpCommitteesDTO getfilecomment([FromBody] NC_818_EmpCommitteesDTO data)
        {
            return _Iobj.getfilecomment(data);
        }
        [Route("savecomments")]
        public NC_818_EmpCommitteesDTO savecomments([FromBody] NC_818_EmpCommitteesDTO data)
        {
            return _Iobj.savecomments(data);
        }
        [Route("savefilewisecomments")]
        public NC_818_EmpCommitteesDTO savefilewisecomments([FromBody] NC_818_EmpCommitteesDTO data)
        {
            return _Iobj.savefilewisecomments(data);
        }
    }
}
