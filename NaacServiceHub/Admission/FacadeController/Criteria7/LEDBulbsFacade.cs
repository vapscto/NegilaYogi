using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.Admission.Interface;
using NaacServiceHub.Admission.Interface.Criteria7;
using PreadmissionDTOs.NAAC.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.Admission.FacadeController.Criteria7
{
    [Route("api/[controller]")]
    public class LEDBulbsFacade : Controller
    {
        public LEDBulbsInterface _Iobj;
        public LEDBulbsFacade(LEDBulbsInterface para)
        {
            _Iobj = para;
        }

        [Route("loaddata")]
        public Task<NAAC_AC_714_LEDBulbs_DTO> loaddata([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            return _Iobj.loaddata(data);
        }
        [Route("savedatatab1")]
        public NAAC_AC_714_LEDBulbs_DTO savedatatab1([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            return _Iobj.savedatatab1(data);
        }
        [Route("savemedicaldatawisecomments")]
        public NAAC_AC_714_LEDBulbs_DTO savemedicaldatawisecomments([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            return _Iobj.savemedicaldatawisecomments(data);
        }
        [Route("savefilewisecommentsLEDbulb")]
        public NAAC_AC_714_LEDBulbs_DTO savefilewisecommentsLEDbulb([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            return _Iobj.savefilewisecommentsLEDbulb(data);
        }
        [Route("getfilecommentLEDbulb")]
        public NAAC_AC_714_LEDBulbs_DTO getfilecommentLEDbulb([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            return _Iobj.getfilecommentLEDbulb(data);
        }
        [Route("getcommentLEDbulb")]
        public NAAC_AC_714_LEDBulbs_DTO getcommentLEDbulb([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            return _Iobj.getcommentLEDbulb(data);
        }
        [Route("savemedicaldatawisecommentsLEDbulb")]
        public NAAC_AC_714_LEDBulbs_DTO savemedicaldatawisecommentsLEDbulb([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            return _Iobj.savemedicaldatawisecommentsLEDbulb(data);
        }
        [Route("getcomment")]
        public NAAC_AC_714_LEDBulbs_DTO getcomment([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            return _Iobj.getcomment(data);
        }
        

        [Route("editTab1")]
        public NAAC_AC_714_LEDBulbs_DTO editTab1([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            return _Iobj.editTab1(data);
        }

        [Route("deactivYTab1")]
        public NAAC_AC_714_LEDBulbs_DTO deactivYTab1([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            return _Iobj.deactivYTab1(data);
        }

        [Route("deleteuploadfile")]
        public NAAC_AC_714_LEDBulbs_DTO deleteuploadfile([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            return _Iobj.deleteuploadfile(data);
        }

        [Route("getData")]
        public NAAC_AC_714_LEDBulbs_DTO getData([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            return _Iobj.getData(data);
        }

        //MC
        [Route("getDataMCwater")]
        public NAAC_AC_714_LEDBulbs_DTO getDataMCwater([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            return _Iobj.getDataMCwater(data);
        }
        [Route("saveMCwater")]
        public NAAC_AC_714_LEDBulbs_DTO saveMCwater([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            return _Iobj.saveMCwater(data);
        }

        [Route("EditDataMCwater")]
        public NAAC_AC_714_LEDBulbs_DTO EditDataMCwater([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            return _Iobj.EditDataMCwater(data);
        }

        [Route("deactivateMCwater")]
        public NAAC_AC_714_LEDBulbs_DTO deactivateMCwater([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            return _Iobj.deactivateMCwater(data);
        }

        [Route("getDataMCgreen")]
        public NAAC_AC_714_LEDBulbs_DTO getDataMCgreen([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            return _Iobj.getDataMCgreen(data);
        }
        [Route("saveMCgreen")]
        public NAAC_AC_714_LEDBulbs_DTO saveMCgreen([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            return _Iobj.saveMCgreen(data);
        }

        [Route("EditDataMCgreen")]
        public NAAC_AC_714_LEDBulbs_DTO EditDataMCgreen([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            return _Iobj.EditDataMCgreen(data);
        }

        [Route("deactivateMCgreen")]
        public NAAC_AC_714_LEDBulbs_DTO deactivateMCgreen([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            return _Iobj.deactivateMCgreen(data);
        }

        [Route("getDataMCdisable")]
        public NAAC_AC_714_LEDBulbs_DTO getDataMCdisable([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            return _Iobj.getDataMCdisable(data);
        }
        [Route("saveMCdisable")]
        public NAAC_AC_714_LEDBulbs_DTO saveMCdisable([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            return _Iobj.saveMCdisable(data);
        }

        [Route("EditDataMCdisable")]
        public NAAC_AC_714_LEDBulbs_DTO EditDataMCdisable([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            return _Iobj.EditDataMCdisable(data);
        }

        [Route("deactivateMCdisable")]
        public NAAC_AC_714_LEDBulbs_DTO deactivateMCdisable([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            return _Iobj.deactivateMCdisable(data);
        }
        //MC
    }
}
