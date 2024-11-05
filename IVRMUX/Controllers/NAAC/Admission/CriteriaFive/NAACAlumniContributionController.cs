using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.Admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.Admission
{
    [Route("api/[controller]")]
    public class NAACAlumniContributionController : Controller
    {
        NAACAlumniContributionDelegate del = new NAACAlumniContributionDelegate();

        [Route("loaddatahsu/{id:int}")]
        public NAACAlumniContributionDTO loaddatahsu(int id)
        {
            NAACAlumniContributionDTO data = new NAACAlumniContributionDTO();
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = id;
            return del.loaddatahsu(data);
        }
        [Route("loaddata/{id:int}")]
        public NAACAlumniContributionDTO loaddata(int id)
        {
            NAACAlumniContributionDTO data = new NAACAlumniContributionDTO();
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = id;
            return del.loaddata(data);
        }
        [Route("save")]
        public NAACAlumniContributionDTO save([FromBody]NAACAlumniContributionDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
          
            return del.save(data);
        }

        [Route("savehsu")]
        public NAACAlumniContributionDTO savehsu([FromBody]NAACAlumniContributionDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
          
            return del.savehsu(data);
        }

        [Route("deactiveStudent")]
        public NAACAlumniContributionDTO deactiveStudent([FromBody] NAACAlumniContributionDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
          
            return del.deactiveStudent(data);
        }



        [Route("EditData")]
        public NAACAlumniContributionDTO EditData([FromBody]NAACAlumniContributionDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.EditData(data);
        }
          [Route("viewuploadflies")]
        public NAACAlumniContributionDTO viewuploadflies([FromBody]NAACAlumniContributionDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.viewuploadflies(data);
        }
          [Route("deleteuploadfile")]
        public NAACAlumniContributionDTO deleteuploadfile([FromBody]NAACAlumniContributionDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deleteuploadfile(data);
        }

        [Route("savemedicaldatawisecomments")]
        public NAACAlumniContributionDTO savemedicaldatawisecomments([FromBody]NAACAlumniContributionDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savemedicaldatawisecomments(category);
        }
        [Route("getcomment")]
        public NAACAlumniContributionDTO getcomment([FromBody]NAACAlumniContributionDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getcomment(category);
        }
        [Route("getfilecomment")]
        public NAACAlumniContributionDTO getfilecomment([FromBody]NAACAlumniContributionDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getfilecomment(category);
        }
        [Route("savefilewisecomments")]
        public NAACAlumniContributionDTO savefilewisecomments([FromBody]NAACAlumniContributionDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savefilewisecomments(category);
        }
    }
}
