
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExamServiceHub.com.vaps.Interfaces;
//using AdmissionServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Exam;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class PromotionSettingFacadeController : Controller
    {
        public PromotionSettingInterface _PromotionSetting;

        public PromotionSettingFacadeController(PromotionSettingInterface PromotionSetting)
        {
            _PromotionSetting = PromotionSetting;
        }


        [Route("Getdetails")]
        public PromotionSettingDTO Getdetails([FromBody]PromotionSettingDTO data)//int IVRMM_Id
        {
           
            return _PromotionSetting.Getdetails(data);
           
        }

        [Route("editdetails/{id:int}")]
        public PromotionSettingDTO editdetails(int id)
        {
            return _PromotionSetting.editdetails(id);
        }

        [Route("savedetails")]
        public PromotionSettingDTO savedetails([FromBody] PromotionSettingDTO data)
        {
            return _PromotionSetting.savedetails(data);
        }

        [Route("get_category")]
        public PromotionSettingDTO get_category([FromBody] PromotionSettingDTO data)
        {
            return _PromotionSetting.get_category(data);
        }

        [Route("get_subjects")]
        public PromotionSettingDTO get_subjects([FromBody] PromotionSettingDTO data)
        {
            return _PromotionSetting.get_subjects(data);
        }

        [Route("deactivate")]
        public PromotionSettingDTO deactivate([FromBody] PromotionSettingDTO data)
        {           
            return _PromotionSetting.deactivate(data);
        }

        [Route("deactivate_sub")]
        public PromotionSettingDTO deactivate_sub([FromBody] PromotionSettingDTO data)
        {
            return _PromotionSetting.deactivate_sub(data);
        }

        [Route("deactive_sub_grp_exm")]
        public PromotionSettingDTO deactive_sub_grp_exm([FromBody] PromotionSettingDTO data)
        {
            return _PromotionSetting.deactive_sub_grp_exm(data);
        }

        [Route("deactive_sub_grp")]
        public PromotionSettingDTO deactive_sub_grp([FromBody] PromotionSettingDTO data)
        {
            return _PromotionSetting.deactive_sub_grp(data);
        }

        [Route("getalldetailsviewrecords/{id:int}")]
        //[Route("getenquirycontroller")]
        public PromotionSettingDTO getalldetailsviewrecords(int id)
        {
            // id = 12;
            return _PromotionSetting.getalldetailsviewrecords(id);
        }

        [Route("getalldetailsviewrecords_sub_grp_exms/{id:int}")]
        //[Route("getenquirycontroller")]
        public PromotionSettingDTO getalldetailsviewrecords_sub_grp_exms(int id)
        {
            // id = 12;
            return _PromotionSetting.getalldetailsviewrecords_sub_grp_exms(id);
        }

        [Route("getalldetailsviewrecords_subgrps/{id:int}")]
        //[Route("getenquirycontroller")]
        public PromotionSettingDTO getalldetailsviewrecords_subgrps(int id)
        {
            // id = 12;
            return _PromotionSetting.getalldetailsviewrecords_subgrps(id);
        }

        [Route("GetSubjectExamMaks")]
        public PromotionSettingDTO GetSubjectExamMaks([FromBody] PromotionSettingDTO data)
        {
            return _PromotionSetting.GetSubjectExamMaks(data);
        }

        [Route("SetSubjectOrder")]
        public PromotionSettingDTO SetSubjectOrder([FromBody] PromotionSettingDTO data)
        {
            return _PromotionSetting.SetSubjectOrder(data);
        }

        [Route("SaveSubjectOrder")]
        public PromotionSettingDTO SaveSubjectOrder([FromBody] PromotionSettingDTO data)
        {
            return _PromotionSetting.SaveSubjectOrder(data);
        }
    }
}
