using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.Sales;
using Recruitment.com.vaps.Interfaces;

namespace Recruitment.com.vaps.Controllers
{
    [Produces("application/json")]
    [Route("api/Sales_Lead_Facade")]
    public class Sales_Lead_FacadeController : Controller
    {
        public Sales_Lead_Master_Interface _sali;


        public Sales_Lead_FacadeController(Sales_Lead_Master_Interface lm)
        {
            _sali = lm;
        }
        //----------------------------category-----------------------------
        [Route("get_load_Cat/{id:int}")]
        public ISM_Sales_Master_Category_DTO get_load_Cat(int id)
        {
            return _sali.get_load_Cat(id);
        }
        [Route("SaveEdit_Cat")]
        public ISM_Sales_Master_Category_DTO SaveEdit_Cat([FromBody] ISM_Sales_Master_Category_DTO dto)
        {
            return _sali.SaveEdit_Cat(dto);
        }
      
        [Route("Edit_details_Cat")]
        public ISM_Sales_Master_Category_DTO Edit_details_Cat([FromBody] ISM_Sales_Master_Category_DTO dto)
        {
            return _sali.Edit_details_Cat(dto);
        }

        [HttpPost]
        [Route("deactivate_Cat")]
        public ISM_Sales_Master_Category_DTO deactivate_Cat([FromBody] ISM_Sales_Master_Category_DTO dTO)
        {
            return _sali.deactivate_Cat(dTO);
        }
        
         //----------------------------Product-----------------------------
        [Route("get_load_pro/{id:int}")]
        public ISM_Sales_Master_Product_DTO get_load_pro(int id)
        {
            return _sali.get_load_pro(id);
        }
        [Route("SaveEdit_pro")]
        public ISM_Sales_Master_Product_DTO SaveEdit_pro([FromBody] ISM_Sales_Master_Product_DTO dto)
        {
            return _sali.SaveEdit_pro(dto);
        }
       
        [Route("Edit_details_pro")]
        public ISM_Sales_Master_Product_DTO Edit_details_pro([FromBody] ISM_Sales_Master_Product_DTO dto)
        {
            return _sali.Edit_details_pro(dto);
        }

        [HttpPost]
        [Route("deactivate_pro")]
        public ISM_Sales_Master_Product_DTO deactivate_pro([FromBody] ISM_Sales_Master_Product_DTO dTO)
        {
            return _sali.deactivate_pro(dTO);
        }
        //----------------------------sOURCE-----------------------------
        [Route("get_load_src/{id:int}")]
        public ISM_Sales_Master_Source_DTO get_load_src(int id)
        {
            return _sali.get_load_src(id);
        }
        [Route("SaveEdit_src")]
        public ISM_Sales_Master_Source_DTO SaveEdit_src([FromBody] ISM_Sales_Master_Source_DTO dto)
        {
            return _sali.SaveEdit_src(dto);
        }
       
        [Route("Edit_details_src")]
        public ISM_Sales_Master_Source_DTO Edit_details_src([FromBody] ISM_Sales_Master_Source_DTO dto)
        {
            return _sali.Edit_details_src(dto);
        }

        [HttpPost]
        [Route("deactivate_src")]
        public ISM_Sales_Master_Source_DTO deactivate_src([FromBody] ISM_Sales_Master_Source_DTO dTO)
        {
            return _sali.deactivate_src(dTO);
        }
        //=================Status============
        [Route("get_load_sts/{id:int}")]
        public ISM_Sales_Master_Status_DTO get_load_sts(int id)
        {
            return _sali.get_load_sts(id);
        }
        [Route("SaveEdit_sts")]
        public ISM_Sales_Master_Status_DTO SaveEdit_sts([FromBody] ISM_Sales_Master_Status_DTO dto)
        {
            return _sali.SaveEdit_sts(dto);
        }
       
        [Route("Edit_details_sts")]
        public ISM_Sales_Master_Status_DTO Edit_details_sts([FromBody] ISM_Sales_Master_Status_DTO dto)
        {
            return _sali.Edit_details_sts(dto);
        }

        [HttpPost]
        [Route("deactivate_sts")]
        public ISM_Sales_Master_Status_DTO deactivate_sts([FromBody] ISM_Sales_Master_Status_DTO dTO)
        {
            return _sali.deactivate_sts(dTO);
        }
        //====================Lead=====================

        [Route("load_all_lead")]
        public ISM_Sales_Lead_DTO load_all_lead([FromBody] ISM_Sales_Lead_DTO dTO)
        {
            return _sali.load_all_lead(dTO);
        }
        [Route("select_state")]
        public ISM_Sales_Lead_DTO select_state([FromBody] ISM_Sales_Lead_DTO dTO)
        {
            return _sali.select_state(dTO);
        }
        [Route("checkemailtemplet")]
        public ISM_Sales_Lead_DTO checkemailtemplet([FromBody] ISM_Sales_Lead_DTO dTO)
        {
            return _sali.checkemailtemplet(dTO);
        }
        [Route("Save_Edit_SaleLead")]
        public ISM_Sales_Lead_DTO Save_Edit_SaleLead([FromBody] ISM_Sales_Lead_DTO dTO)
        {
            return _sali.Save_Edit_SaleLead(dTO);
        }
        [Route("Sales_lead_edit")]
        public ISM_Sales_Lead_DTO Sales_lead_edit([FromBody] ISM_Sales_Lead_DTO dTO)
        {
            return _sali.Sales_lead_edit(dTO);
        }
        [Route("deactiv_prde")]
        public ISM_Sales_Lead_DTO deactiv_prde([FromBody] ISM_Sales_Lead_DTO dTO)
        {
            return _sali.deactiv_prde(dTO);
        }
        [Route("get_load_lead_demo")]
        public ISM_Sales_Lead_Demo_DTO get_load_lead_demo([FromBody] ISM_Sales_Lead_Demo_DTO dTO)
        {
            return _sali.get_load_lead_demo(dTO);
        }
        [Route("Edit_details_lead_demo")]
        public ISM_Sales_Lead_Demo_DTO Edit_details_lead_demo([FromBody] ISM_Sales_Lead_Demo_DTO dTO)
        {
            return _sali.Edit_details_lead_demo(dTO);
        }
        [Route("Edit_response_lead_demo")]
        public ISM_Sales_Lead_Demo_DTO Edit_response_lead_demo([FromBody] ISM_Sales_Lead_Demo_DTO dTO)
        {
            return _sali.Edit_response_lead_demo(dTO);
        }
        [Route("SaveEdit_lead_demo")]
        public ISM_Sales_Lead_Demo_DTO SaveEdit_lead_demo([FromBody] ISM_Sales_Lead_Demo_DTO dTO)
        {
            return _sali.SaveEdit_lead_demo(dTO);
        }
        [Route("Save_response_lead_demo")]
        public ISM_Sales_Lead_Demo_DTO Save_response_lead_demo([FromBody] ISM_Sales_Lead_Demo_DTO dTO)
        {
            return _sali.Save_response_lead_demo(dTO);
        }
        [Route("view_lead_demo")]
        public ISM_Sales_Lead_Demo_DTO view_lead_demo([FromBody] ISM_Sales_Lead_Demo_DTO dTO)
        {
            return _sali.view_lead_demo(dTO);
        }

    }
}