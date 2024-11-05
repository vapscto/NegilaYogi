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
    public class NAACGovtShcrShipController : Controller
    {
        NAACGovtShcrShipDelegate del = new NAACGovtShcrShipDelegate();

        [Route("loaddata/{id:int}")]
        public NAACGovtShcrShipDTO loaddata(int id)
        {
            NAACGovtShcrShipDTO data = new NAACGovtShcrShipDTO();
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = id;
            return del.loaddata(data);
        }
        [Route("save")]
        public NAACGovtShcrShipDTO save([FromBody]NAACGovtShcrShipDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
           
            return del.save(data);
        }

        [Route("deactiveStudent")]
        public NAACGovtShcrShipDTO deactiveStudent([FromBody] NAACGovtShcrShipDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
           
            return del.deactiveStudent(data);
        }



        [Route("EditData")]
        public NAACGovtShcrShipDTO EditData([FromBody]NAACGovtShcrShipDTO category)
        {
           
         
            return del.EditData(category);
        }
        [Route("viewuploadflies")]
        public NAACGovtShcrShipDTO viewuploadflies([FromBody]NAACGovtShcrShipDTO category)
        {
        
         
            return del.viewuploadflies(category);
        }
        [Route("deleteuploadfile")]
        public NAACGovtShcrShipDTO deleteuploadfile([FromBody]NAACGovtShcrShipDTO category)
        {
           
         
            return del.deleteuploadfile(category);
        }
        [Route("savemedicaldatawisecomments")]
        public NAACGovtShcrShipDTO savemedicaldatawisecomments([FromBody]NAACGovtShcrShipDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savemedicaldatawisecomments(category);
        }
        [Route("getcomment")]
        public NAACGovtShcrShipDTO getcomment([FromBody]NAACGovtShcrShipDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getcomment(category);
        }
        [Route("getfilecomment")]
        public NAACGovtShcrShipDTO getfilecomment([FromBody]NAACGovtShcrShipDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getfilecomment(category);
        }
        [Route("savefilewisecomments")]
        public NAACGovtShcrShipDTO savefilewisecomments([FromBody]NAACGovtShcrShipDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savefilewisecomments(category);
        }


    }
}
