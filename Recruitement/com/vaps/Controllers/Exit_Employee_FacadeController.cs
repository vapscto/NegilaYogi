using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.Exit;
using Recruitment.com.vaps.Interfaces;

namespace Recruitment.com.vaps.Controllers
{
    [Produces("application/json")]
    [Route("api/Exit_Employee_Facade")]
    public class Exit_Employee_FacadeController : Controller
    {
        public Exit_Employee_Interface _eei;

        public VMSContext _vmsconte;

        public Exit_Employee_FacadeController(Exit_Employee_Interface ee)
        {
            _eei = ee;
        }

        //===========================Reason master Start====================================
        [Route("Get_Reason")]
        public ISM_Resignation_Master_Reasons_DTO Get_Reason([FromBody] ISM_Resignation_Master_Reasons_DTO dto)
        {
            return _eei.Get_Reason(dto);
        }

        [Route("Save_Edit_Reason")]
        public ISM_Resignation_Master_Reasons_DTO Save_Edit_Reason([FromBody] ISM_Resignation_Master_Reasons_DTO dto)
        {
            return _eei.Save_Edit_Reason(dto);
        }
        [Route("get_details_reason")]
        public ISM_Resignation_Master_Reasons_DTO get_details_reason([FromBody] ISM_Resignation_Master_Reasons_DTO dto)
        {
            return _eei.get_details_reason(dto);
        }

        [Route("active_deactive_reason")]
        public ISM_Resignation_Master_Reasons_DTO active_deactive_reason([FromBody]ISM_Resignation_Master_Reasons_DTO dto)
        {

            return _eei.active_deactive_reason(dto);
        }
        //==========================================End=============================================
        //===========================check list master Start====================================
        [Route("Get_Checklist")]
        public ISM_Resignation_Master_CheckLists_DTO Get_Checklist([FromBody] ISM_Resignation_Master_CheckLists_DTO dto)
        {
            return _eei.Get_Checklist(dto);
        }

        [Route("Save_Edit_Checklist")]
        public ISM_Resignation_Master_CheckLists_DTO Save_Edit_Checklist([FromBody] ISM_Resignation_Master_CheckLists_DTO dto)
        {
            return _eei.Save_Edit_Checklist(dto);
        }
        [Route("get_details_checklist")]
        public ISM_Resignation_Master_CheckLists_DTO get_details_checklist([FromBody] ISM_Resignation_Master_CheckLists_DTO dto)
        {
            return _eei.get_details_checklist(dto);
        }

        [Route("active_deactive_checklist")]
        public ISM_Resignation_Master_CheckLists_DTO active_deactive_checklist([FromBody]ISM_Resignation_Master_CheckLists_DTO dto)
        {

            return _eei.active_deactive_checklist(dto);
        }
        //==========================================End=============================================
        //==================================Exit Employee Process Start=================================

        [Route("Load_all_data")]
        public ISM_Resignation_DTO Load_all_data([FromBody] ISM_Resignation_DTO dto)
        {
           
            return _eei.Load_all_data(dto);
        }

       
        [Route("GetAllRelData")]
        public ISM_Resignation_DTO GetAllRelData([FromBody] ISM_Resignation_DTO dto)
        {

            return _eei.GetAllRelData(dto);
        }
        [Route("GetAllRelData1")]
        public ISM_Resignation_DTO GetAllRelData1([FromBody] ISM_Resignation_DTO dto)
        {

            return _eei.GetAllRelData1(dto);
        }

        [Route("Exit_empl_SaveEdit")]
        public ISM_Resignation_DTO Exit_empl_SaveEdit([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.Exit_empl_SaveEdit(dto);
        }
        [Route("Exit_empl_AccRej")]
        public ISM_Resignation_DTO Exit_empl_AccRej([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.Exit_empl_AccRej(dto);
        }
        [Route("Edit_Employee")]
        public ISM_Resignation_DTO Edit_Employee([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.Edit_Employee(dto);
        }
        [Route("c_approve_new")]
        public ISM_Resignation_DTO c_approve_new([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.c_approve_new(dto);
        }
        [Route("Savedata_td")]
        public ISM_Resignation_DTO Savedata_td([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.Savedata_td(dto);
        }
        [Route("edit_relieving")]
        public ISM_Resignation_DTO edit_relieving([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.edit_relieving(dto);
        }
        [Route("Savedata_print")]
        public Task<ISM_Resignation_DTO> Savedata_print([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.Savedata_printAsync(dto);
        }
        [Route("print_exit_employee")]
        public ISM_Resignation_DTO print_exit_employee([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.print_exit_employee(dto);
        }
        [Route("download_doc")]
        public ISM_Resignation_DTO download_doc([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.download_doc(dto);
        }
        //===========================Reports========================
        [Route("get_all_data_R")]
        public ISM_Resignation_DTO get_all_data_R([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.get_all_data_R(dto);
        }
        [Route("showdetails_R")]
        public Task<ISM_Resignation_DTO> showdetails_R([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.showdetails_R(dto);
        }
        [Route("get_all_relieving_data_R")]
        public ISM_Resignation_DTO get_all_relieving_data_R([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.get_all_relieving_data_R(dto);
        }
        [Route("showdetails_relieving_R")]
        public Task<ISM_Resignation_DTO> showdetails_relieving_R([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.showdetails_relieving_R(dto);
        }
       
        [Route("relieving_exit_employee_view")]
        public ISM_Resignation_DTO relieving_exit_employee_view([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.relieving_exit_employee_view(dto);
        }

        //GAUTAM
        [Route("loadEmployeeData")]
        public ISM_Resignation_DTO loadEmployeeData([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.loadEmployeeData(dto);
        }
        [Route("sendResignationMail")]
        public ISM_Resignation_DTO sendResignationMail([FromBody] ISM_Resignation_DTO dto)
        {
            return _eei.sendResignationMail(dto);
        }        
        //GAUTAM
    }
}