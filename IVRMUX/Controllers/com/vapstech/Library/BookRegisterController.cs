using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Library;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Library
{
    [Route("api/[controller]")]
    public class BookRegisterController : Controller
    {
        BookRegisterDelegate _delobj = new BookRegisterDelegate();

        [Route("getdetails/{id:int}")]
        public BookRegisterDTO getdetails(int id)
        {
            BookRegisterDTO data = new BookRegisterDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));        
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));

            return _delobj.getdetails(data);
        }
        [Route("Savedata")]
        public BookRegisterDTO Savedata([FromBody] BookRegisterDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.Savedata(data);
        }
        [Route("Tab1Savedata")]
        public BookRegisterDTO Tab1Savedata([FromBody] BookRegisterDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.Tab1Savedata(data);
        }
        [Route("Tab2Savedata")]
        public BookRegisterDTO Tab2Savedata([FromBody] BookRegisterDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.Tab2Savedata(data);
        }
        
        [Route("Ckeck_ISBNNO")]
        public BookRegisterDTO Ckeck_ISBNNO([FromBody] BookRegisterDTO data)
        {
            data.MI_Id=Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.Ckeck_ISBNNO(data);
        }

        [Route("chekAccno")]
        public BookRegisterDTO chekAccno([FromBody] BookRegisterDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.chekAccno(data);
        }
        [Route("Addaccnno")]
        public BookRegisterDTO Addaccnno([FromBody] BookRegisterDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.Addaccnno(data);
        }
        [Route("Ckeck_LMBANO_AccessionNo")]
        public BookRegisterDTO Ckeck_LMBANO_AccessionNo([FromBody] BookRegisterDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.Ckeck_LMBANO_AccessionNo(data);
        }

        [Route("deactiveY")]
        public BookRegisterDTO deactiveY([FromBody] BookRegisterDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.deactiveY(data);
        }

        [Route("Editdata")]
        public BookRegisterDTO Editdata([FromBody] BookRegisterDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.Editdata(data);
        }
        

       [Route("searching")]
        public BookRegisterDTO searching([FromBody] BookRegisterDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.searching(data);
        }
    

       [Route("searchfilter")]
        public BookRegisterDTO searchfilter([FromBody] BookRegisterDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.searchfilter(data);
        }



        [Route("changelibrary")]
        public BookRegisterDTO changelibrary([FromBody] BookRegisterDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));

            return _delobj.changelibrary(data);
        }



    }
}
