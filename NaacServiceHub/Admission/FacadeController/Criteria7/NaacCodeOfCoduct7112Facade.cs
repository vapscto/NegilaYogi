using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.Admission.Interface.Criteria7;
using PreadmissionDTOs.NAAC.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.Admission.FacadeController.Criteria7
{
    [Route("api/[controller]")]
    public class NaacCodeOfCoduct7112Facade : Controller
    {
        public NaacCodeOfCoduct7112Interface _inter;
        public NaacCodeOfCoduct7112Facade(NaacCodeOfCoduct7112Interface y)
        {
            _inter = y;
        }
        [Route("loaddata")]
        public Task<NAAC_AC_7112_CodeOfCoduct_DTO> loaddata([FromBody] NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            return _inter.loaddata(data);
        }
                [Route("save")]
        public NAAC_AC_7112_CodeOfCoduct_DTO save([FromBody] NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            return _inter.save(data);
        }
        [Route("deactivate")]
        public NAAC_AC_7112_CodeOfCoduct_DTO deactivate([FromBody] NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            return _inter.deactivate(data);
        }
        [Route("EditData")]
        public NAAC_AC_7112_CodeOfCoduct_DTO EditData([FromBody] NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            return _inter.EditData(data);
        }
        [Route("viewuploadflies")]
        public NAAC_AC_7112_CodeOfCoduct_DTO viewuploadflies([FromBody] NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            return _inter.viewuploadflies(data);
        }        
        [Route("deleteuploadfile")]
        public NAAC_AC_7112_CodeOfCoduct_DTO deleteuploadfile([FromBody] NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            return _inter.deleteuploadfile(data);
        }
        [Route("getfilecomment")]
        public NAAC_AC_7112_CodeOfCoduct_DTO getfilecomment([FromBody] NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            return _inter.getfilecomment(data);
        }
        [Route("savefilewisecomments")]
        public NAAC_AC_7112_CodeOfCoduct_DTO savefilewisecomments([FromBody] NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            return _inter.savefilewisecomments(data);
        }
        [Route("getcomment")]
        public NAAC_AC_7112_CodeOfCoduct_DTO getcomment([FromBody] NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            return _inter.getcomment(data);
        }[Route("savemedicaldatawisecomments")]
        public NAAC_AC_7112_CodeOfCoduct_DTO savemedicaldatawisecomments([FromBody] NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            return _inter.savemedicaldatawisecomments(data);
        }
        [Route("getData")]
        public NAAC_AC_7112_CodeOfCoduct_DTO getData([FromBody] NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            return _inter.getData(data);
        }
    }
}
