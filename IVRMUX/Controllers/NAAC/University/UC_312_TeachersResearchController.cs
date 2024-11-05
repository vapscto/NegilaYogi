using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.University;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.University;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.University
{
    [Route("api/[controller]")]
    public class UC_312_TeachersResearchController : Controller
    {
        UC_312_TeachersResearchDelegate del = new UC_312_TeachersResearchDelegate();

        [Route("loaddata/{id:int}")]
        public UC_312_TeachersResearchDTO loaddata(int id)
        {
            UC_312_TeachersResearchDTO data = new UC_312_TeachersResearchDTO();

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = id;
            return del.loaddata(data);
        }

        [Route("save")]
        public UC_312_TeachersResearchDTO save([FromBody] UC_312_TeachersResearchDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.save(data);
        }

        [Route("deactive")]
        public UC_312_TeachersResearchDTO deactive([FromBody] UC_312_TeachersResearchDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deactive(data);
        }

        [Route("EditData")]
        public UC_312_TeachersResearchDTO EditData([FromBody] UC_312_TeachersResearchDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.EditData(data);
        }

        [Route("viewuploadflies")]
        public UC_312_TeachersResearchDTO viewuploadflies([FromBody] UC_312_TeachersResearchDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.viewuploadflies(data);
        }

        [Route("deleteuploadfile")]
        public UC_312_TeachersResearchDTO deleteuploadfile([FromBody] UC_312_TeachersResearchDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deleteuploadfile(data);
        }

        [Route("get_dept")]
        public UC_312_TeachersResearchDTO get_dept([FromBody] UC_312_TeachersResearchDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId")); ;
            return del.get_dept(data);
        }

        [Route("get_emp")]
        public UC_312_TeachersResearchDTO get_emp([FromBody] UC_312_TeachersResearchDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId")); ;
            return del.get_emp(data);
        }

    }
}
