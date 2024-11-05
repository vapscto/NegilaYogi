
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model.com.vaps.Exam;
using PreadmissionDTOs.com.vaps.Exam;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class PromotionSettingController : Controller
    {

        PromotionSettingDelegates PromotionSettingdelStr = new PromotionSettingDelegates();
      
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("Getdetails")]
        public PromotionSettingDTO Getdetails(PromotionSettingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return PromotionSettingdelStr.Getdetails(data);            
        }

        [Route("editdetails/{id:int}")]
        public PromotionSettingDTO editdetails(int id)
        {
            return PromotionSettingdelStr.editdetails(id);
        }

        [Route("savedetails")]
        public PromotionSettingDTO savedetails([FromBody] PromotionSettingDTO data)
        {     
            data.MI_Id= Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return PromotionSettingdelStr.savedetails(data);
        }

        [Route("get_category")]
        public PromotionSettingDTO get_category([FromBody] PromotionSettingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return PromotionSettingdelStr.get_category(data);
        }

        [Route("get_subjects")]
        public PromotionSettingDTO get_subjects([FromBody] PromotionSettingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return PromotionSettingdelStr.get_subjects(data);
        }

        [Route("deactivate")]
        public PromotionSettingDTO deactivate([FromBody] PromotionSettingDTO data)
        {
            return PromotionSettingdelStr.deactivate(data);         
        }

        [Route("deactivate_sub")]
        public PromotionSettingDTO deactivate_sub([FromBody] PromotionSettingDTO data)
        {
            return PromotionSettingdelStr.deactivate_sub(data);
        }

        [Route("deactive_sub_grp_exm")]
        public PromotionSettingDTO deactive_sub_grp_exm([FromBody] PromotionSettingDTO data)
        {
            return PromotionSettingdelStr.deactive_sub_grp_exm(data);
        }

        [Route("deactive_sub_grp")]
        public PromotionSettingDTO deactive_sub_grp([FromBody] PromotionSettingDTO data)
        {
            return PromotionSettingdelStr.deactive_sub_grp(data);
        }

        [Route("getalldetailsviewrecords/{id:int}")]
        public PromotionSettingDTO getalldetailsviewrecords(int id)
        {

            return PromotionSettingdelStr.getalldetailsviewrecords(id);
        }

        [Route("getalldetailsviewrecords_sub_grp_exms/{id:int}")]
        public PromotionSettingDTO getalldetailsviewrecords_sub_grp_exms(int id)
        {

            return PromotionSettingdelStr.getalldetailsviewrecords_sub_grp_exms(id);
        }

        [Route("getalldetailsviewrecords_subgrps/{id:int}")]
        public PromotionSettingDTO getalldetailsviewrecords_subgrps(int id)
        {

            return PromotionSettingdelStr.getalldetailsviewrecords_subgrps(id);
        }

        [Route("GetSubjectExamMaks")]
        public PromotionSettingDTO GetSubjectExamMaks([FromBody] PromotionSettingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return PromotionSettingdelStr.GetSubjectExamMaks(data);
        } 

        [Route("SetSubjectOrder")]
        public PromotionSettingDTO SetSubjectOrder([FromBody] PromotionSettingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return PromotionSettingdelStr.SetSubjectOrder(data);
        } 

        [Route("SaveSubjectOrder")]
        public PromotionSettingDTO SaveSubjectOrder([FromBody] PromotionSettingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return PromotionSettingdelStr.SaveSubjectOrder(data);
        } 
    }
}
