using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.SeatingArrangment;
using SeatingArrangment.Interface;

namespace SeatingArrangment.Controllers
{
    [Route("api/[controller]")]
    public class SAMasterSuperintendentFacade : Controller
    {
        public SAMasterSuperintendent_Interface _sintfc;
        public SAMasterSuperintendentFacade(SAMasterSuperintendent_Interface inter)
        {
            _sintfc = inter;
        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        // ===============superintendent==================
        [Route("load_sup")]
        public SAMasterSuperintendent load_sup([FromBody] SAMasterSuperintendent data)
        {
            return _sintfc.load_sup(data);
        }

        [Route("Save_sup")]
        public SAMasterSuperintendent Save_sup([FromBody] SAMasterSuperintendent data)
        {
            return _sintfc.Save_sup(data);
        }

        [Route("Edit_sup")]
        public SAMasterSuperintendent Edit_sup([FromBody] SAMasterSuperintendent data)
        {
            return _sintfc.Edit_sup(data);
        }

        [Route("ActiveDeactive_sup")]
        public SAMasterSuperintendent ActiveDeactive_sup([FromBody] SAMasterSuperintendent data)
        {
            return _sintfc.ActiveDeactive_sup(data);
        }

        //=========Absent Student======================
        [Route("load_AS")]
        public SAMasterSuperintendent load_AS([FromBody] SAMasterSuperintendent data)
        {
            return _sintfc.load_AS(data);
        }

        [Route("Save_AS")]
        public SAMasterSuperintendent Save_AS([FromBody] SAMasterSuperintendent data)
        {
            return _sintfc.Save_AS(data);
        }

        [Route("Edit_AS")]
        public SAMasterSuperintendent Edit_AS([FromBody] SAMasterSuperintendent data)
        {
            return _sintfc.Edit_AS(data);
        }

        [Route("DeleteAbsentStudent")]
        public SAMasterSuperintendent DeleteAbsentStudent([FromBody] SAMasterSuperintendent data)
        {
            return _sintfc.DeleteAbsentStudent(data);
        }        

        
        //=========Malpractice Student======================

        [Route("load_MPS")]
        public SAMasterSuperintendent load_MPS([FromBody] SAMasterSuperintendent data)
        {
            return _sintfc.load_MPS(data);
        }

        [Route("Save_MPS")]
        public SAMasterSuperintendent Save_MPS([FromBody] SAMasterSuperintendent data)
        {
            return _sintfc.Save_MPS(data);
        }

        [Route("Edit_MPS")]
        public SAMasterSuperintendent Edit_MPS([FromBody] SAMasterSuperintendent data)
        {
            return _sintfc.Edit_MPS(data);
        }

        [Route("DeleteMalPraticeStudent")]
        public SAMasterSuperintendent DeleteMalPraticeStudent([FromBody] SAMasterSuperintendent data)
        {
            return _sintfc.DeleteMalPraticeStudent(data);
        }       


        //=========Chief coordinator======================

        [Route("load_CC")]
        public SAMasterSuperintendent load_CC([FromBody] SAMasterSuperintendent data)
        {
            return _sintfc.load_CC(data);
        }

        [Route("Save_CC")]
        public SAMasterSuperintendent Save_CC([FromBody] SAMasterSuperintendent data)
        {
            return _sintfc.Save_CC(data);
        }

        [Route("Edit_CC")]
        public SAMasterSuperintendent Edit_CC([FromBody] SAMasterSuperintendent data)
        {
            return _sintfc.Edit_CC(data);
        }

        [Route("ActiveDeactive_CC")]
        public SAMasterSuperintendent ActiveDeactive_CC([FromBody] SAMasterSuperintendent data)
        {
            return _sintfc.ActiveDeactive_CC(data);
        }


        // ****************** General Selection ************************ //

        [Route("GetCourse")]
        public SAMasterSuperintendent GetCourse([FromBody] SAMasterSuperintendent dto)
        {
            return _sintfc.GetCourse(dto);
        }

        [Route("GetBranch")]
        public SAMasterSuperintendent GetBranch([FromBody] SAMasterSuperintendent dto)
        {
            return _sintfc.GetBranch(dto);
        }

        [Route("GetSemester")]
        public SAMasterSuperintendent GetSemester([FromBody] SAMasterSuperintendent dto)
        {
            return _sintfc.GetSemester(dto);
        }

        [Route("GetSubject")]
        public SAMasterSuperintendent GetSubject([FromBody] SAMasterSuperintendent dto)
        {
            return _sintfc.GetSubject(dto);
        }

        [Route("GetStudent")]
        public SAMasterSuperintendent GetStudent([FromBody] SAMasterSuperintendent dto)
        {
            return _sintfc.GetStudent(dto);
        }
    }
}