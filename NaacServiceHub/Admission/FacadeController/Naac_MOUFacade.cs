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
    public class Naac_MOUFacade : Controller
    {
        public Naac_MOUInterface _Interface;
        public Naac_MOUFacade(Naac_MOUInterface q)
        {
            _Interface = q;
        }

        [Route("loaddata")]
        public Naac_MOU_DTO loaddata([FromBody] Naac_MOU_DTO data)
        {
            return _Interface.loaddata(data);
        }
        [Route("save")]
        public Naac_MOU_DTO save([FromBody] Naac_MOU_DTO data)
        {
            return _Interface.save(data);
        }
        [Route("getcomment")]
        public Naac_MOU_DTO getcomment([FromBody] Naac_MOU_DTO data)
        {
            return _Interface.getcomment(data);
        }
        [Route("savemedicaldatawisecomments")]
        public Naac_MOU_DTO savemedicaldatawisecomments([FromBody] Naac_MOU_DTO data)
        {
            return _Interface.savemedicaldatawisecomments(data);
        }
        [Route("savefilewisecomments")]
        public Naac_MOU_DTO savefilewisecomments([FromBody] Naac_MOU_DTO data)
        {
            return _Interface.savefilewisecomments(data);
        }
        [Route("getfilecomment")]
        public Naac_MOU_DTO getfilecomment([FromBody] Naac_MOU_DTO data)
        {
            return _Interface.getfilecomment(data);
        }
        [Route("viewuploadflies")]
        public Naac_MOU_DTO viewuploadflies([FromBody] Naac_MOU_DTO data)
        {
            return _Interface.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public Naac_MOU_DTO deleteuploadfile([FromBody] Naac_MOU_DTO data)
        {
            return _Interface.deleteuploadfile(data);
        }
        [Route("deactiveStudent")]
        public Naac_MOU_DTO deactiveStudent([FromBody] Naac_MOU_DTO data)
        {
            return _Interface.deactiveStudent(data);
        }
        [Route("EditData")]
        public Naac_MOU_DTO EditData([FromBody]Naac_MOU_DTO category)
        {
            return _Interface.EditData(category);
        }
    }
}
