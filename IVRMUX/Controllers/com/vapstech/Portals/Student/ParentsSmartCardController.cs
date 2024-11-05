using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using corewebapi18072016.Delegates.com.vapstech.Portals.Student;
using PreadmissionDTOs.com.vaps.Portals.Student;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using Microsoft.Extensions.Options;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class ParentsSmartCardController : Controller
    {
        ParentsSmartCardDelegate fdd = new ParentsSmartCardDelegate();
        private readonly DomainModelMsSqlServerContext _context;

        public ParentsSmartCardController(IOptions<FacadeUrl> settings, DomainModelMsSqlServerContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("getloaddata")]
        public ParentSmartCardDTO getloaddata(ParentSmartCardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.AMST_ID = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.RoleName = HttpContext.Session.GetString("RoleNme");
            if(data.AMST_ID!=0)
            {
                string accountname = "";
                string accesskey = "";
                ReadTemplateFromAzure obj = new ReadTemplateFromAzure();

                var datatstu = _context.IVRM_Storage_path_Details.ToList();
                if (datatstu.Count() > 0)
                {
                    accountname = datatstu.FirstOrDefault().IVRM_SD_Access_Name;
                    accesskey = datatstu.FirstOrDefault().IVRM_SD_Access_Key;
                }
                data.htmldata = "";
                try
                {
                    string html = obj.getHtmlContentFromAzure(accountname, accesskey, "smartcarddata/" + data.MI_Id, "Smartcard.html", 0).ToString();
                    data.htmldata = html;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }              
            }
            
            return fdd.getloaddata(data);
        }

        [Route("getstudata")]
        public ParentSmartCardDTO getstudata([FromBody]ParentSmartCardDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            sddto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            // sddto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));                 
            //sddto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            string accountname = "";
            string accesskey = "";
            ReadTemplateFromAzure obj = new ReadTemplateFromAzure();

            var datatstu = _context.IVRM_Storage_path_Details.ToList();
            if (datatstu.Count() > 0)
            {
                accountname = datatstu.FirstOrDefault().IVRM_SD_Access_Name;
                accesskey = datatstu.FirstOrDefault().IVRM_SD_Access_Key;
            }

            sddto.htmldata = "";
            try
            {
                string html = obj.getHtmlContentFromAzure(accountname, accesskey, "smartcarddata/" + sddto.MI_Id, "Smartcard.html", 0);
                sddto.htmldata = html;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        
            return fdd.getstudata(sddto);
        }

        [HttpPost]
        [Route("savedata")]
        public ParentSmartCardDTO savedata([FromBody]ParentSmartCardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMST_ID = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return fdd.savedata(data);
        }

        [Route("savedataadmin")]
        public ParentSmartCardDTO savedataadmin([FromBody]ParentSmartCardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
          //  data.AMST_ID = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return fdd.savedataadmin(data);
        }

        [Route("searchfilter")]
        public ParentSmartCardDTO searchfilter([FromBody]ParentSmartCardDTO student)
        {
            student.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            student.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return fdd.searchfilter(student);
        }

        [Route("guardianDetails")]
        public ParentSmartCardDTO guardianDetails([FromBody]ParentSmartCardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMST_ID = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return fdd.guardianDetails(data);
        }
        [Route("getreport")]
        public ParentSmartCardDTO getreport([FromBody]ParentSmartCardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return fdd.getreport(data);
        }

        [Route("getdpstate/{id:int}")]
        public ParentSmartCardDTO getstate(int id)
        {
            return fdd.getStateByCountry(id);
        }

    }
}
