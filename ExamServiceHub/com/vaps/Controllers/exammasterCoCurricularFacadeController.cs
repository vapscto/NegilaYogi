
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExamServiceHub.com.vaps.Interfaces;
//using AdmissionServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Exam;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class exammasterCoCurricularFacadeController : Controller
    {
        public exammasterCoCurricularInterface _exammasterCoCurricular;

        public exammasterCoCurricularFacadeController(exammasterCoCurricularInterface exammasterCoCurricular)
        {
            _exammasterCoCurricular = exammasterCoCurricular;
        }


        [Route("Getdetails")]
        public exammasterCoCurricularDTO Getdetails([FromBody]exammasterCoCurricularDTO data)//int IVRMM_Id
        {           
            return _exammasterCoCurricular.Getdetails(data);           
        }

        [Route("editdetails/{id:int}")]
        public exammasterCoCurricularDTO editdetails(int ID)
        {
            return _exammasterCoCurricular.editdetails(ID);
        }
        
        [Route("validateordernumber")]
        public exammasterCoCurricularDTO validateordernumber([FromBody] exammasterCoCurricularDTO data)
        {
            return _exammasterCoCurricular.validateordernumber(data);
        }

        [Route("savedetails")]
        public exammasterCoCurricularDTO savedetails([FromBody] exammasterCoCurricularDTO data)
        {
            return _exammasterCoCurricular.savedetails(data);
        }
       
        [Route("deactivate")]
        public exammasterCoCurricularDTO deactivate([FromBody] exammasterCoCurricularDTO data)
        {           
            return _exammasterCoCurricular.deactivate(data);
        }

        //Student Mapping Personlity

        [Route("studentdataload")]
        public exammasterCoCurricularDTO studentdataload([FromBody]exammasterCoCurricularDTO data)//int IVRMM_Id
        {
            return _exammasterCoCurricular.studentdataload(data);
        }
        [Route("onchangeyear")]
        public exammasterCoCurricularDTO onchangeyear([FromBody]exammasterCoCurricularDTO data)//int IVRMM_Id
        {
            return _exammasterCoCurricular.onchangeyear(data);
        }
        [Route("onchangeclass")]
        public exammasterCoCurricularDTO onchangeclass([FromBody]exammasterCoCurricularDTO data)//int IVRMM_Id
        {
            return _exammasterCoCurricular.onchangeclass(data);
        }
        [Route("onchangesection")]
        public exammasterCoCurricularDTO onchangesection([FromBody]exammasterCoCurricularDTO data)//int IVRMM_Id
        {
            return _exammasterCoCurricular.onchangesection(data);
        }
        [Route("searchdata")]
        public exammasterCoCurricularDTO searchdata([FromBody]exammasterCoCurricularDTO data)//int IVRMM_Id
        {
            return _exammasterCoCurricular.searchdata(data);
        }
        [Route("savemapping")]
        public exammasterCoCurricularDTO savemapping([FromBody]exammasterCoCurricularDTO data)//int IVRMM_Id
        {
            return _exammasterCoCurricular.savemapping(data);
        }
        [Route("editmappingdetails")]
        public exammasterCoCurricularDTO editmappingdetails([FromBody]exammasterCoCurricularDTO data)//int IVRMM_Id
        {
            return _exammasterCoCurricular.editmappingdetails(data);
        }
    }
}
