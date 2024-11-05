using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.ClubManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.ClubManagement;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.ClubManagement
{
    [Route("api/[controller]")]
    public class CMS_Transaction : Controller
    {
        //CMS_MasterDepartment
        CMS_TransactionDelegate cms = new CMS_TransactionDelegate();

        [HttpGet]
        [Route("loaddata/{id:int}")]
        public CMS_TransactionDTO loaddata(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //CMS_TransactionDTO data = new CMS_TransactionDTO();
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));            
            return cms.loaddata(id);

        }
        [Route("loaddatatwo/{id:int}")]
        public CMS_TransactionDetailsDTO loaddatatwo(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //CMS_TransactionDTO data = new CMS_TransactionDTO();
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));            
            return cms.loaddatatwo(id);

        }
        [HttpPost]
        [Route("savedata")]
        public CMS_TransactionDTO savedata([FromBody]CMS_TransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.savedata(data);
        }
        //deactive
        [Route("deactive")]
        public CMS_TransactionDTO deactive([FromBody]CMS_TransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.deactive(data);
        }
        //edit
        [Route("edit")]
        public CMS_TransactionDTO edit([FromBody]CMS_TransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.edit(data);
        }
        //CMS_TransactionDetailsDTO
        [Route("savedatatwo")]
        public CMS_TransactionDetailsDTO savedatatwo([FromBody]CMS_TransactionDetailsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.savedatatwo(data);
        }
        [Route("deactivetwo")]
        public CMS_TransactionDetailsDTO deactivetwo([FromBody]CMS_TransactionDetailsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.deactivetwo(data);
        }
        [Route("edittwo")]
        public CMS_TransactionDetailsDTO edittwo([FromBody]CMS_TransactionDetailsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.edittwo(data);
        }
    }
}
