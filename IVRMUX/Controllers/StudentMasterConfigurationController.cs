using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class StudentMasterConfigurationController : Controller
    {
        private static FacadeUrl _config;
        private FacadeUrl fdu = new FacadeUrl();
        StudentMasterConfigurationDelegates _studentMasterDelegate = new StudentMasterConfigurationDelegates();

        public StudentMasterConfigurationController(IOptions<FacadeUrl> settings)
        {
            _config = settings.Value;
            new StudentMasterConfigurationDelegates(_config);
            fdu = _config;
        }

        [HttpGet]
        [Route("masterConfiguration/{id:int}")]
        public CommonDTO getMasterConfigDropdown(int id)
        {
            CommonDTO data = new CommonDTO();

            int roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.roleId = roleidd;

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRM_MI_Id = mid;

            int moid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MO_Id"));
            data.IVRM_MO_Id = moid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userId = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            return _studentMasterDelegate.getMasterConfigDropdown(data);
        }

        [HttpPost]
        public MasterConfigurationDTO saveMasterConfigData([FromBody] MasterConfigurationDTO mstConfig)
        {

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            mstConfig.MI_Id = mid;


            int moid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MO_Id"));
            mstConfig.MO_Id = moid;

            return _studentMasterDelegate.saveMasterConfigData(mstConfig);
        }

        [Route("geteditdata/{id:int}")]
        public MasterConfigurationDTO getMasterConfigEditData(int id)
        {
            return _studentMasterDelegate.getMasterConfigEditData(id);
        }

        [Route("deletedetails/{id:int}")]
        public MasterConfigurationDTO deleteRecord(int id)
        {
            MasterConfigurationDTO data = new MasterConfigurationDTO();

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            data.ISPAC_Id = id;

            return _studentMasterDelegate.deleteRecord(data);
        }
    }
}