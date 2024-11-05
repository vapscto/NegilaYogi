using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS;
using PreadmissionDTOs.com.vaps.VMS.Exit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS
{
    public class Exit_Employee_Delegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonVMSDelegate<ISM_Resignation_Master_Reasons_DTO, ISM_Resignation_Master_Reasons_DTO> COMMI = new CommonVMSDelegate<ISM_Resignation_Master_Reasons_DTO, ISM_Resignation_Master_Reasons_DTO>();
        CommonVMSDelegate<ISM_Resignation_Master_CheckLists_DTO, ISM_Resignation_Master_CheckLists_DTO> COMMRC = new CommonVMSDelegate<ISM_Resignation_Master_CheckLists_DTO, ISM_Resignation_Master_CheckLists_DTO>();
        CommonVMSDelegate<ISM_Resignation_DTO, ISM_Resignation_DTO> COMMR = new CommonVMSDelegate<ISM_Resignation_DTO, ISM_Resignation_DTO>();
        //===========================Reason master Start====================================
        public ISM_Resignation_Master_Reasons_DTO Get_Reason(ISM_Resignation_Master_Reasons_DTO dto)
        {
            return COMMI.POSTData(dto, "Exit_Employee_Facade/Get_Reason/");
        }
        public ISM_Resignation_Master_Reasons_DTO Save_Edit_Reason(ISM_Resignation_Master_Reasons_DTO dto)
        {
            return COMMI.POSTData(dto, "Exit_Employee_Facade/Save_Edit_Reason/");
        }
        public ISM_Resignation_Master_Reasons_DTO get_details_reason(ISM_Resignation_Master_Reasons_DTO dto)
        {
            return COMMI.POSTData(dto, "Exit_Employee_Facade/get_details_reason/");
        }


        public ISM_Resignation_Master_Reasons_DTO active_deactive_reason(ISM_Resignation_Master_Reasons_DTO dto)
        {

            return COMMI.POSTData(dto, "Exit_Employee_Facade/active_deactive_reason/");
        }
        //==========================================End=============================================
        //===========================Check mlist master Start====================================
        public ISM_Resignation_Master_CheckLists_DTO Get_Checklist(ISM_Resignation_Master_CheckLists_DTO dto)
        {
            return COMMRC.POSTData(dto, "Exit_Employee_Facade/Get_Checklist/");
        }
        public ISM_Resignation_Master_CheckLists_DTO Save_Edit_Checklist(ISM_Resignation_Master_CheckLists_DTO dto)
        {
            return COMMRC.POSTData(dto, "Exit_Employee_Facade/Save_Edit_Checklist/");
        }
        public ISM_Resignation_Master_CheckLists_DTO get_details_checklist(ISM_Resignation_Master_CheckLists_DTO dto)
        {
            return COMMRC.POSTData(dto, "Exit_Employee_Facade/get_details_checklist/");
        }
        public ISM_Resignation_Master_CheckLists_DTO active_deactive_checklist(ISM_Resignation_Master_CheckLists_DTO dto)
        {
            return COMMRC.POSTData(dto, "Exit_Employee_Facade/active_deactive_checklist/");
        }


        //==========================================End=============================================

        //==================================Exit Employee Process Start=================================

        public ISM_Resignation_DTO Load_all_data(ISM_Resignation_DTO dto)
        {

            return COMMR.POSTData(dto, "Exit_Employee_Facade/Load_all_data/");
        }
        
        public ISM_Resignation_DTO GetAllRelData(ISM_Resignation_DTO dto)
        {

            return COMMR.POSTData(dto, "Exit_Employee_Facade/GetAllRelData/");
        }
        public ISM_Resignation_DTO GetAllRelData1(ISM_Resignation_DTO dto)
        {

            return COMMR.POSTData(dto, "Exit_Employee_Facade/GetAllRelData1/");
        }
        public ISM_Resignation_DTO Exit_empl_SaveEdit(ISM_Resignation_DTO dto)
        {

            return COMMR.POSTData(dto, "Exit_Employee_Facade/Exit_empl_SaveEdit/");
        }
        public ISM_Resignation_DTO Exit_empl_AccRej(ISM_Resignation_DTO dto)
        {

            return COMMR.POSTData(dto, "Exit_Employee_Facade/Exit_empl_AccRej/");
        }
        public ISM_Resignation_DTO Edit_Employee(ISM_Resignation_DTO dto)
        {

            return COMMR.POSTData(dto, "Exit_Employee_Facade/Edit_Employee/");
        }
        public ISM_Resignation_DTO c_approve_new(ISM_Resignation_DTO dto)
        {

            return COMMR.POSTData(dto, "Exit_Employee_Facade/c_approve_new/");
        }
        public ISM_Resignation_DTO Savedata_td(ISM_Resignation_DTO dto)
        {

            return COMMR.POSTData(dto, "Exit_Employee_Facade/Savedata_td/");
        }
        public ISM_Resignation_DTO edit_relieving(ISM_Resignation_DTO dto)
        {

            return COMMR.POSTData(dto, "Exit_Employee_Facade/edit_relieving/");
        }
        public ISM_Resignation_DTO Savedata_print(ISM_Resignation_DTO dto)
        {

            return COMMR.POSTData(dto, "Exit_Employee_Facade/Savedata_print/");
        }
        public ISM_Resignation_DTO print_exit_employee(ISM_Resignation_DTO dto)
        {

            return COMMR.POSTData(dto, "Exit_Employee_Facade/print_exit_employee/");
        }
        public ISM_Resignation_DTO download_doc(ISM_Resignation_DTO dto)
        {

            return COMMR.POSTData(dto, "Exit_Employee_Facade/download_doc/");
        }
        //=========================Report===========================
        public ISM_Resignation_DTO get_all_data_R(ISM_Resignation_DTO dto)
        {

            return COMMR.POSTData(dto, "Exit_Employee_Facade/get_all_data_R/");
        }
        public ISM_Resignation_DTO showdetails_R(ISM_Resignation_DTO dto)
        {

            return COMMR.POSTData(dto, "Exit_Employee_Facade/showdetails_R/");
        }
        public ISM_Resignation_DTO get_all_relieving_data_R(ISM_Resignation_DTO dto)
        {

            return COMMR.POSTData(dto, "Exit_Employee_Facade/get_all_relieving_data_R/");
        }
        public ISM_Resignation_DTO showdetails_relieving_R(ISM_Resignation_DTO dto)
        {

            return COMMR.POSTData(dto, "Exit_Employee_Facade/showdetails_relieving_R/");
        }
       
        public ISM_Resignation_DTO relieving_exit_employee_view(ISM_Resignation_DTO dto)
        {

            return COMMR.POSTData(dto, "Exit_Employee_Facade/relieving_exit_employee_view/");
        }

        //GAUTAM
        public ISM_Resignation_DTO loadEmployeeData(ISM_Resignation_DTO dto)
        {
            return COMMR.POSTData(dto, "Exit_Employee_Facade/loadEmployeeData/");
        }
        public ISM_Resignation_DTO sendResignationMail(ISM_Resignation_DTO dto)
        {
            return COMMR.POSTData(dto, "Exit_Employee_Facade/sendResignationMail/");
        }        
        //GAUTAM
    }
}
