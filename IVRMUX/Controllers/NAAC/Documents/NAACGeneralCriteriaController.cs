using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.Admission;
using IVRMUX.Delegates.NAAC.Documents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.Admission;
using PreadmissionDTOs.NAAC.Documents;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.Documents
{
    [Route("api/[controller]")]
    public class NAACGeneralCriteriaController : Controller
    {
        NAACGeneralCriteriaDelegate del = new NAACGeneralCriteriaDelegate();

        [Route("loaddata/{id:int}")]
        public NAACGeneralCriteriaDTO loaddata(int id)
        {
            NAACGeneralCriteriaDTO data = new NAACGeneralCriteriaDTO();
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddata(data);
        }
        [Route("save")]
        public NAACGeneralCriteriaDTO save([FromBody]NAACGeneralCriteriaDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
           
            return del.save(data);
        }

        [Route("deactiveStudent")]
        public NAACGeneralCriteriaDTO deactiveStudent([FromBody] NAACGeneralCriteriaDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            
            return del.deactiveStudent(data);
        }



        [Route("EditData")]
        public NAACGeneralCriteriaDTO EditData([FromBody]NAACGeneralCriteriaDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.EditData(data);
        }
        [Route("viewuploadflies")]
        public NAACGeneralCriteriaDTO viewuploadflies([FromBody]NAACGeneralCriteriaDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public NAACGeneralCriteriaDTO deleteuploadfile([FromBody]NAACGeneralCriteriaDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.deleteuploadfile(data);
        }
        [Route("deletelink")]
        public NAACGeneralCriteriaDTO deletelink([FromBody]NAACGeneralCriteriaDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.deletelink(data);
        }
         [Route("viewlink")]
        public NAACGeneralCriteriaDTO viewlink([FromBody]NAACGeneralCriteriaDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.viewlink(data);
        }


    }
}
