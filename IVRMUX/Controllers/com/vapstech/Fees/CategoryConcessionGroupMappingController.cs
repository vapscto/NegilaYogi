using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Fees;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Fees
{
    [Route("api/[controller]")]
    public class CategoryConcessionGroupMappingController : Controller
    {
        CategoryConcessionGroupMappingDelegate del = new CategoryConcessionGroupMappingDelegate();


        [Route("loaddata/{id:int}")]
        public CategoryConcessionGroupMappingDTO loaddata(int id)
        {
            CategoryConcessionGroupMappingDTO data = new CategoryConcessionGroupMappingDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return del.loaddata(data);
        }
        [Route("gethead")]
        public CategoryConcessionGroupMappingDTO gethead([FromBody] CategoryConcessionGroupMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.gethead(data);
        }

        [Route("getconcession")]
        public CategoryConcessionGroupMappingDTO getconcession([FromBody] CategoryConcessionGroupMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getconcession(data);
        }
        [Route("save")]
        public CategoryConcessionGroupMappingDTO save([FromBody] CategoryConcessionGroupMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.save(data);
        }
        [Route("deactiveStudent")]
        public CategoryConcessionGroupMappingDTO deactiveStudent([FromBody] CategoryConcessionGroupMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deactiveStudent(data);
        }

        [Route("EditData")]
        public CategoryConcessionGroupMappingDTO EditData([FromBody] CategoryConcessionGroupMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.EditData(data);
        }

        [Route("getgroup")]
        public CategoryConcessionGroupMappingDTO getgroup([FromBody] CategoryConcessionGroupMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getgroup(data);
        }


    }
}
