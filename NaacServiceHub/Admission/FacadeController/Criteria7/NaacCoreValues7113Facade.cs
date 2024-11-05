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
    public class NaacCoreValues7113Facade : Controller
    {




        public NaacCoreValues7113Interface _inter;
        public NaacCoreValues7113Facade(NaacCoreValues7113Interface y)
        {
            _inter = y;
        }
        [Route("loaddata")]
        public Task<NAAC_AC_7113_CoreValues_DTO> loaddata([FromBody] NAAC_AC_7113_CoreValues_DTO data)
        {
            return _inter.loaddata(data);
        }
        [Route("save")]
        public NAAC_AC_7113_CoreValues_DTO save([FromBody] NAAC_AC_7113_CoreValues_DTO data)
        {
            return _inter.save(data);
        }
        [Route("deactivate")]
        public NAAC_AC_7113_CoreValues_DTO deactivate([FromBody] NAAC_AC_7113_CoreValues_DTO data)
        {
            return _inter.deactivate(data);
        }
        [Route("EditData")]
        public NAAC_AC_7113_CoreValues_DTO EditData([FromBody] NAAC_AC_7113_CoreValues_DTO data)
        {
            return _inter.EditData(data);
        }
        [Route("getfilecomment")]
        public NAAC_AC_7113_CoreValues_DTO getfilecomment([FromBody] NAAC_AC_7113_CoreValues_DTO data)
        {
            return _inter.getfilecomment(data);
        }
        [Route("savefilewisecomments")]
        public NAAC_AC_7113_CoreValues_DTO savefilewisecomments([FromBody] NAAC_AC_7113_CoreValues_DTO data)
        {
            return _inter.savefilewisecomments(data);
        }
        [Route("getcomment")]
        public NAAC_AC_7113_CoreValues_DTO getcomment([FromBody] NAAC_AC_7113_CoreValues_DTO data)
        {
            return _inter.getcomment(data);
        }
        [Route("savemedicaldatawisecomments")]
        public NAAC_AC_7113_CoreValues_DTO savemedicaldatawisecomments([FromBody] NAAC_AC_7113_CoreValues_DTO data)
        {
            return _inter.savemedicaldatawisecomments(data);
        }

        [Route("viewuploadflies")]
        public NAAC_AC_7113_CoreValues_DTO viewuploadflies([FromBody] NAAC_AC_7113_CoreValues_DTO data)
        {
            return _inter.viewuploadflies(data);
        }

        [Route("deleteuploadfile")]
        public NAAC_AC_7113_CoreValues_DTO deleteuploadfile([FromBody] NAAC_AC_7113_CoreValues_DTO data)
        {
            return _inter.deleteuploadfile(data);
        }

        [Route("getData")]
        public NAAC_AC_7113_CoreValues_DTO getData([FromBody] NAAC_AC_7113_CoreValues_DTO data)
        {
            return _inter.getData(data);
        }


    }
}
