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
    public class NAAC_AC_634_DevProgramsFacade : Controller
    {
        public NAAC_AC_634_DevProgramsInterface _Interface;
        public NAAC_AC_634_DevProgramsFacade(NAAC_AC_634_DevProgramsInterface q)
        {
            _Interface = q;
        }

      [Route("loaddata")] 
      public NAAC_Criteria_6_DTO loaddata([FromBody] NAAC_Criteria_6_DTO data)
        {
            return _Interface.loaddata(data);
        }
        [Route("save")]
        public NAAC_Criteria_6_DTO save([FromBody] NAAC_Criteria_6_DTO data)
        {
            return _Interface.save(data);
        }
        [Route("deactiveStudent")]
        public NAAC_Criteria_6_DTO deactiveStudent([FromBody] NAAC_Criteria_6_DTO data)
        {
            return _Interface.deactiveStudent(data);
        }
        [Route("EditData")]

        public NAAC_Criteria_6_DTO EditData([FromBody] NAAC_Criteria_6_DTO data)
        {
            return _Interface.EditData(data);
        }

        [Route("viewuploadflies")]
        public NAAC_Criteria_6_DTO viewuploadflies([FromBody] NAAC_Criteria_6_DTO data)
        {
            return _Interface.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public NAAC_Criteria_6_DTO deleteuploadfile([FromBody] NAAC_Criteria_6_DTO data)
        {
            return _Interface.deleteuploadfile(data);
        }


        [Route("savemedicaldatawisecomments")]
        public NAAC_Criteria_6_DTO savemedicaldatawisecomments([FromBody] NAAC_Criteria_6_DTO data)
        {
            return _Interface.savemedicaldatawisecomments(data);
        }
        [Route("savefilewisecomments")]
        public NAAC_Criteria_6_DTO savefilewisecomments([FromBody] NAAC_Criteria_6_DTO data)
        {
            return _Interface.savefilewisecomments(data);
        }
        [Route("getcomment")]
        public NAAC_Criteria_6_DTO getcomment([FromBody] NAAC_Criteria_6_DTO data)
        {
            return _Interface.getcomment(data);
        }
        [Route("getfilecomment")]
        public NAAC_Criteria_6_DTO getfilecomment([FromBody] NAAC_Criteria_6_DTO data)
        {
            return _Interface.getfilecomment(data);
        }


    }
}
