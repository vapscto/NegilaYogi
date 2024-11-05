using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdmissionServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class Master_ExamQualified_ClassFacade : Controller
    {

        Master_ExamQualified_ClassInterface _interface;
        public Master_ExamQualified_ClassFacade(Master_ExamQualified_ClassInterface inter)
        {
            _interface = inter;
        }

        [Route("getalldata")]
        public Master_ExamQualified_ClassDTO getalldata([FromBody]Master_ExamQualified_ClassDTO data)
        {
            return _interface.getalldata(data);
        }

        [Route("SaveClass")]
        public Master_ExamQualified_ClassDTO SaveClass([FromBody]Master_ExamQualified_ClassDTO data)
        {
            return _interface.SaveClass(data);
        }
        [Route("Editdetails/{id:int}")]
        public Master_ExamQualified_ClassDTO Editdetails(int id)
        {
            return _interface.Editdetails(id);
        }

        [Route("deactiveCat")]
        public Master_ExamQualified_ClassDTO deactiveCat([FromBody]Master_ExamQualified_ClassDTO user)
        {
            return _interface.deactiveCat(user);
        }
    }
}
