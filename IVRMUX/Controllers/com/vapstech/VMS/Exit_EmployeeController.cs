using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS;
using PreadmissionDTOs.com.vaps.VMS.Exit;

namespace IVRMUX.Controllers.com.vapstech.VMS
{
    [Produces("application/json")]
    [Route("api/Exit_Employee")]
    public class Exit_EmployeeController : Controller
    {
        Exit_Employee_Delegate EED = new Exit_Employee_Delegate();


        //===========================Reason master Start====================================
        [Route("Get_Reason/{id:int}")]
        public ISM_Resignation_Master_Reasons_DTO Get_Reason(int id)
        {
            ISM_Resignation_Master_Reasons_DTO dto = new ISM_Resignation_Master_Reasons_DTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return EED.Get_Reason(dto);
        }
        [Route("Save_Edit_Reason")]
        public ISM_Resignation_Master_Reasons_DTO Save_Edit_Reason([FromBody] ISM_Resignation_Master_Reasons_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return EED.Save_Edit_Reason(dto);
        }

        [Route("get_details_reason/{id:int}")]
        public ISM_Resignation_Master_Reasons_DTO get_details_reason(int id)
        {
            ISM_Resignation_Master_Reasons_DTO dto = new ISM_Resignation_Master_Reasons_DTO();
            dto.ISMRESGMRE_Id = id;
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.get_details_reason(dto);
        }

        [Route("active_deactive_reason")]
        public ISM_Resignation_Master_Reasons_DTO active_deactive_reason([FromBody]ISM_Resignation_Master_Reasons_DTO dto)
        {
            return EED.active_deactive_reason(dto);
        }

        //==========================================End=============================================

        //===========================Check list master Start====================================
        [Route("Get_Checklist/{id:int}")]
        public ISM_Resignation_Master_CheckLists_DTO Get_Checklist(int id)
        {
            ISM_Resignation_Master_CheckLists_DTO dto = new ISM_Resignation_Master_CheckLists_DTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return EED.Get_Checklist(dto);
        }
        [Route("Save_Edit_Checklist")]
        public ISM_Resignation_Master_CheckLists_DTO Save_Edit_Checklist([FromBody] ISM_Resignation_Master_CheckLists_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return EED.Save_Edit_Checklist(dto);
        }

        [Route("get_details_checklist/{id:int}")]
        public ISM_Resignation_Master_CheckLists_DTO get_details_checklist(int id)
        {
            ISM_Resignation_Master_CheckLists_DTO dto = new ISM_Resignation_Master_CheckLists_DTO();
            dto.ISMRESGMCL_Id = id;
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.get_details_checklist(dto);
        }

        [Route("active_deactive_checklist")]
        public ISM_Resignation_Master_CheckLists_DTO active_deactive_checklist([FromBody]ISM_Resignation_Master_CheckLists_DTO dto)
        {
            return EED.active_deactive_checklist(dto);
        }

        //==========================================End=============================================
        //==================================Exit Employee Process Start=================================

        [Route("Load_all_data/{id:int}")]
        public ISM_Resignation_DTO Load_all_data(int id)
        {
            ISM_Resignation_DTO dto = new ISM_Resignation_DTO();
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.Load_all_data(dto);
        }

        [Route("GetAllRelData/{id:int}")]
        public ISM_Resignation_DTO GetAllRelData(int id)
        {
            ISM_Resignation_DTO dto = new ISM_Resignation_DTO();
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRME_Id = id;
            return EED.GetAllRelData(dto);
        }
        [Route("GetAllRelData1/{id:int}")]
        public ISM_Resignation_DTO GetAllRelData1(int id)
        {
            ISM_Resignation_DTO dto = new ISM_Resignation_DTO();
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRME_Id = id;
            return EED.GetAllRelData1(dto);
        }


        [Route("Exit_empl_SaveEdit")]
        public ISM_Resignation_DTO Exit_empl_SaveEdit([FromBody] ISM_Resignation_DTO dto)
        {

            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.Exit_empl_SaveEdit(dto);
        }
        [Route("Exit_empl_AccRej")]
        public ISM_Resignation_DTO Exit_empl_AccRej([FromBody] ISM_Resignation_DTO dto)
        {

            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.Exit_empl_AccRej(dto);
        }
        [Route("Edit_Employee/{id:int}")]
        public ISM_Resignation_DTO Edit_Employee(int id)
        {
            ISM_Resignation_DTO dto = new ISM_Resignation_DTO();
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ISMRESG_Id = id;
            return EED.Edit_Employee(dto);
        }
        [Route("c_approve_new/{id:int}")]
        public ISM_Resignation_DTO c_approve_new(int id)
        {
            ISM_Resignation_DTO dto = new ISM_Resignation_DTO();
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ISMRESG_Id = id;
            return EED.c_approve_new(dto);
        }
        [Route("Savedata_td")]
        public ISM_Resignation_DTO Savedata_td([FromBody] ISM_Resignation_DTO dto)
        {

            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return EED.Savedata_td(dto);
        }

        [Route("edit_relieving/{id:int}")]
        public ISM_Resignation_DTO edit_relieving(int id)
        {
            ISM_Resignation_DTO dto = new ISM_Resignation_DTO();
            dto.HRME_Id = id;
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return EED.edit_relieving(dto);
        }
        [Route("Savedata_print")]
        public ISM_Resignation_DTO Savedata_print([FromBody] ISM_Resignation_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return EED.Savedata_print(dto);
        }
        [Route("print_exit_employee/{id:int}")]
        public ISM_Resignation_DTO print_exit_employee(int id)
        {
            ISM_Resignation_DTO dto = new ISM_Resignation_DTO();
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRME_Id = id;

            return EED.print_exit_employee(dto);
        }
        [Route("download_doc/{id:int}")]
        public ISM_Resignation_DTO download_doc(int id)
        {
            ISM_Resignation_DTO dto = new ISM_Resignation_DTO();
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ISMRESG_Id = id;

            return EED.download_doc(dto);
        }
        //================================Reports===================================
        [Route("get_all_data_R")]
        public ISM_Resignation_DTO get_all_data_R(ISM_Resignation_DTO dto)
        {

            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return EED.get_all_data_R(dto);
        }
        [Route("get_all_relieving_data_R")]
        public ISM_Resignation_DTO get_all_relieving_data_R(ISM_Resignation_DTO dto)
        {

            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return EED.get_all_relieving_data_R(dto);
        }

        [Route("showdetails_R")]
        public ISM_Resignation_DTO showdetails_R([FromBody]ISM_Resignation_DTO dto)
        {

            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return EED.showdetails_R(dto);
        }

        [Route("showdetails_relieving_R")]
        public ISM_Resignation_DTO showdetails_relieving_R([FromBody] ISM_Resignation_DTO dto)
        {

            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return EED.showdetails_relieving_R(dto);
        }

        [Route("relieving_exit_employee_view/{id:int}")]
        public ISM_Resignation_DTO relieving_exit_employee_view(int id)
        {
            ISM_Resignation_DTO dto = new ISM_Resignation_DTO();
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRME_Id = id;

            return EED.relieving_exit_employee_view(dto);
        }

        //GAUTAM
        [Route("loadEmployeeData/{id:int}")]
        public ISM_Resignation_DTO loadEmployeeData(int id)
        {
            ISM_Resignation_DTO dto = new ISM_Resignation_DTO();
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.loadEmployeeData(dto);
        }

        [Route("sendResignationMail")]
        public ISM_Resignation_DTO sendResignationMail([FromBody] ISM_Resignation_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.sendResignationMail(dto);
        }        
        //GAUTAM

    }
}