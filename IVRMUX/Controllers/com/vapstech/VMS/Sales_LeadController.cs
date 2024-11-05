using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VMS;
using IVRMUX.Delegates.com.vapstech.VMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS;
using PreadmissionDTOs.com.vaps.VMS.Sales;

namespace IVRMUX.Controllers.com.vapstech.VMS
{
    [Produces("application/json")]
    [Route("api/Sales_Lead")]
    public class Sales_LeadController : Controller
    {
        Sales_Lead_Delegate LMD = new Sales_Lead_Delegate();
        //=====================Category===========
        [HttpGet]
        [Route("get_load_Cat/{id:int}")]
        public ISM_Sales_Master_Category_DTO get_load_Cat(int id)
        {
            var mi = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            id = Convert.ToInt32(mi);

            return LMD.get_load_Cat(id);
        }
        [Route("SaveEdit_Cat")]
        public ISM_Sales_Master_Category_DTO SaveEdit_Cat([FromBody] ISM_Sales_Master_Category_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return LMD.SaveEdit_Cat(dto);
        }
        [Route("Edit_details_Cat")]
        public ISM_Sales_Master_Category_DTO Edit_details_Cat([FromBody] ISM_Sales_Master_Category_DTO dto)
        {
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return LMD.Edit_details_Cat(dto);
        }
        [HttpPost]
        [Route("deactivate_Cat")]
        public ISM_Sales_Master_Category_DTO deactivate_Cat([FromBody] ISM_Sales_Master_Category_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
           
            return LMD.deactivate_Cat(dto);
        }

        //==============================Product============================

        [HttpGet]
        [Route("get_load_pro/{id:int}")]
        public ISM_Sales_Master_Product_DTO get_load_pro(int id)
        {
            var mi = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            id =Convert.ToInt32(mi);
            return LMD.get_load_pro(id);
        }
        [Route("SaveEdit_pro")]
        public ISM_Sales_Master_Product_DTO SaveEdit_pro([FromBody] ISM_Sales_Master_Product_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return LMD.SaveEdit_pro(dto);
        }
        [Route("Edit_details_pro")]
        public ISM_Sales_Master_Product_DTO Edit_details_pro([FromBody] ISM_Sales_Master_Product_DTO dto)
        {
            dto.MI_Id= Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return LMD.Edit_details_pro(dto);
        }
        [HttpPost]
        [Route("deactivate_pro")]
        public ISM_Sales_Master_Product_DTO deactivate_pro([FromBody] ISM_Sales_Master_Product_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                       return LMD.deactivate_pro(dto);
        }

        //=============================Source=========================
        

        [HttpGet]
        [Route("get_load_src/{id:int}")]
        public ISM_Sales_Master_Source_DTO get_load_src(int id)
        {
            var mi = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            id = Convert.ToInt32(mi);
            return LMD.get_load_src(id);
        }
        [Route("SaveEdit_src")]
        public ISM_Sales_Master_Source_DTO SaveEdit_src([FromBody] ISM_Sales_Master_Source_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return LMD.SaveEdit_src(dto);
        }
        [Route("Edit_details_src")]
        public ISM_Sales_Master_Source_DTO Edit_details_src([FromBody] ISM_Sales_Master_Source_DTO dto)
        {
            dto.MI_Id=Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return LMD.Edit_details_src(dto);
        }
        [HttpPost]
        [Route("deactivate_src")]
        public ISM_Sales_Master_Source_DTO deactivate_src([FromBody] ISM_Sales_Master_Source_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return LMD.deactivate_src(dto);
        }

        //=============================Status=========================

        [HttpGet]
        [Route("get_load_sts/{id:int}")]
        public ISM_Sales_Master_Status_DTO get_load_sts(int id)
        {
            var mi = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            id = Convert.ToInt32(mi);
            return LMD.get_load_sts(id);
        }
        [Route("SaveEdit_sts")]
        public ISM_Sales_Master_Status_DTO SaveEdit_sts([FromBody] ISM_Sales_Master_Status_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return LMD.SaveEdit_sts(dto);
        }
        [Route("Edit_details_sts")]
        public ISM_Sales_Master_Status_DTO Edit_details_sts([FromBody] ISM_Sales_Master_Status_DTO dto)
        {
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
           
            return LMD.Edit_details_sts(dto);
        }
        [HttpPost]
        [Route("deactivate_sts")]
        public ISM_Sales_Master_Status_DTO deactivate_sts([FromBody] ISM_Sales_Master_Status_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return LMD.deactivate_sts(dto);
        }
        //=========================lead ================================
        [Route("load_all_lead/{id:int}")]
        public ISM_Sales_Lead_DTO load_all_lead(int id)
        {
            ISM_Sales_Lead_DTO dto = new ISM_Sales_Lead_DTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.load_all_lead(dto);
        }
        [Route("Save_Edit_SaleLead")]
        public ISM_Sales_Lead_DTO Save_Edit_SaleLead([FromBody]ISM_Sales_Lead_DTO dto)
        {
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.Save_Edit_SaleLead(dto);
        }
        [Route("Sales_lead_edit/{id:int}")]
        public ISM_Sales_Lead_DTO Sales_lead_edit(int id)
        {
            ISM_Sales_Lead_DTO dto = new ISM_Sales_Lead_DTO();
            dto.ISMSLE_Id = id;
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.Sales_lead_edit(dto);
        }
        [Route("deactiv_prde")]
        public ISM_Sales_Lead_DTO deactiv_prde([FromBody] ISM_Sales_Lead_DTO dto)
        {
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.deactiv_prde(dto);
        }
        [Route("Sales_lead_view/{id:int}")]
        public ISM_Sales_Lead_DTO Sales_lead_view(int id)
        {
            ISM_Sales_Lead_DTO dto = new ISM_Sales_Lead_DTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.ISMSLE_Id = id;
            return LMD.Sales_lead_view(dto);
        }
        [Route("select_state/{id:int}")]
        public ISM_Sales_Lead_DTO select_state(int id)
        {
            ISM_Sales_Lead_DTO dto = new ISM_Sales_Lead_DTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.IVRMMC_Id = id;
            return LMD.select_state(dto);
        }
        [Route("checkemailtemplet/{id:int}")]
        public ISM_Sales_Lead_DTO checkemailtemplet(int id)
        {
            ISM_Sales_Lead_DTO dto = new ISM_Sales_Lead_DTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.ISMSMSO_Id = id;
            return LMD.checkemailtemplet(dto);
        }
        //==========================Lead Demo=============================

        [Route("get_load_lead_demo/{id:int}")]
        public ISM_Sales_Lead_Demo_DTO get_load_lead_demo(int id)
        {
            ISM_Sales_Lead_Demo_DTO dto = new ISM_Sales_Lead_Demo_DTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.get_load_lead_demo(dto);
        }
        [Route("Edit_details_lead_demo")]
        public ISM_Sales_Lead_Demo_DTO Edit_details_lead_demo([FromBody]ISM_Sales_Lead_Demo_DTO dto)
        {
           
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.Edit_details_lead_demo(dto);
        }
        [Route("Edit_response_lead_demo")]
        public ISM_Sales_Lead_Demo_DTO Edit_response_lead_demo([FromBody]ISM_Sales_Lead_Demo_DTO dto)
        {
           
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.Edit_response_lead_demo(dto);
        }
        [Route("SaveEdit_lead_demo")]
        public ISM_Sales_Lead_Demo_DTO SaveEdit_lead_demo([FromBody] ISM_Sales_Lead_Demo_DTO dto)
        {
           
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.SaveEdit_lead_demo(dto);
        }
        [Route("Save_response_lead_demo")]
        public ISM_Sales_Lead_Demo_DTO Save_response_lead_demo([FromBody]ISM_Sales_Lead_Demo_DTO dto)
        {
           
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.Save_response_lead_demo(dto);
        }
        [Route("view_lead_demo")]
        public ISM_Sales_Lead_Demo_DTO view_lead_demo([FromBody]ISM_Sales_Lead_Demo_DTO dto)
        {
           
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.view_lead_demo(dto);
        }
       

    }
}