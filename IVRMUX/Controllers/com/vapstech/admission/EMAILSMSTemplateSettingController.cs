using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model.com.vaps.admission;
using PreadmissionDTOs.com.vaps.admission;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class EMAILSMSTemplateSettingController : Controller
    {

        EMAILSMSTemplateSettingDelegates ed = new EMAILSMSTemplateSettingDelegates();

        private readonly DomainModelMsSqlServerContext _context;

        public EMAILSMSTemplateSettingController(IOptions<FacadeUrl> settings, DomainModelMsSqlServerContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails/")]
        public EMAILSMSTemplateSettingDTO Getdetails(EMAILSMSTemplateSettingDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           


            return ed.Getdetails(data);            
        }
      
        [Route("GetSelectedRowdetails/{id:int}")]
        public EMAILSMSTemplateSettingDTO GetSelectedRowDetails(int ID)
        {
            HttpContext.Session.SetString("MasterActivityID", ID.ToString());
            return ed.GetSelectedRowDetails(ID);
        }


        [HttpPost]      
        public EMAILSMSTemplateSettingDTO EMAILSMSTemplateSettingDTO([FromBody] EMAILSMSTemplateSettingDTO MMD)
        {
            Int32 MasterActivityID = 0;
            if (HttpContext.Session.GetString("MasterActivityID") != null)
            {
                MasterActivityID = Convert.ToInt32(HttpContext.Session.GetString("MasterActivityID"));
            }
            MMD.AMA_Id = MasterActivityID;
            HttpContext.Session.Remove("MasterActivityID");

            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ed.MasterActivityData(MMD);         
           // return MMD;           
        }

        [HttpDelete]
        [Route("MasterDeleteModulesDTO/{id:int}")]
        public EMAILSMSTemplateSettingDTO MasterDeleteModulesDTO(int ID)
        {
            return ed.MasterDeleteModulesData(ID);         
        }
    }

}
