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
    public class HSU_349_HindexController : Controller
    {
        HSU_349_HindexDelegate del = new HSU_349_HindexDelegate();
        [Route("loaddata/{id:int}")]
        public HSU_346_EMPApprovedJournalList_DTO loaddata(int id)
        {
            HSU_346_EMPApprovedJournalList_DTO data = new HSU_346_EMPApprovedJournalList_DTO();

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = id;
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.loaddata(data);
        }
        [Route("save")]
        public HSU_346_EMPApprovedJournalList_DTO save([FromBody] HSU_346_EMPApprovedJournalList_DTO data)
        {
            // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.save(data);
        }
        [Route("deactive")]
        public HSU_346_EMPApprovedJournalList_DTO deactive([FromBody] HSU_346_EMPApprovedJournalList_DTO data)
        {
            // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deactive(data);
        }
        [Route("EditData")]
        public HSU_346_EMPApprovedJournalList_DTO EditData([FromBody] HSU_346_EMPApprovedJournalList_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.EditData(data);
        }

        [Route("viewuploadflies")]
        public HSU_346_EMPApprovedJournalList_DTO viewuploadflies([FromBody] HSU_346_EMPApprovedJournalList_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.viewuploadflies(data);
        }

        [Route("deleteuploadfile")]
        public HSU_346_EMPApprovedJournalList_DTO deleteuploadfile([FromBody] HSU_346_EMPApprovedJournalList_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.deleteuploadfile(data);
        }
    }
}
