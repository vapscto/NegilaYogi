using PreadmissionDTOs.com.vaps.VMS.Exit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
    public interface Exit_Employee_Interface
    {

        ISM_Resignation_Master_Reasons_DTO Get_Reason(ISM_Resignation_Master_Reasons_DTO dto);
        ISM_Resignation_Master_Reasons_DTO Save_Edit_Reason(ISM_Resignation_Master_Reasons_DTO dto);
        ISM_Resignation_Master_Reasons_DTO get_details_reason(ISM_Resignation_Master_Reasons_DTO dto);
        ISM_Resignation_Master_Reasons_DTO active_deactive_reason(ISM_Resignation_Master_Reasons_DTO dto);

        //====================================
        ISM_Resignation_DTO Load_all_data(ISM_Resignation_DTO dto);
        
        ISM_Resignation_DTO GetAllRelData(ISM_Resignation_DTO dto);
        ISM_Resignation_DTO GetAllRelData1(ISM_Resignation_DTO dto);
        ISM_Resignation_DTO Exit_empl_SaveEdit(ISM_Resignation_DTO dto);
        ISM_Resignation_DTO Exit_empl_AccRej(ISM_Resignation_DTO dto);

        ISM_Resignation_Master_CheckLists_DTO Get_Checklist(ISM_Resignation_Master_CheckLists_DTO dto);

        ISM_Resignation_Master_CheckLists_DTO Save_Edit_Checklist(ISM_Resignation_Master_CheckLists_DTO dto);
        ISM_Resignation_Master_CheckLists_DTO get_details_checklist(ISM_Resignation_Master_CheckLists_DTO dto);
        ISM_Resignation_Master_CheckLists_DTO active_deactive_checklist(ISM_Resignation_Master_CheckLists_DTO dto);
        ISM_Resignation_DTO Edit_Employee(ISM_Resignation_DTO dto);
        ISM_Resignation_DTO c_approve_new(ISM_Resignation_DTO dto);
        ISM_Resignation_DTO Savedata_td(ISM_Resignation_DTO dto);
        ISM_Resignation_DTO edit_relieving(ISM_Resignation_DTO dto);
        Task<ISM_Resignation_DTO> Savedata_printAsync(ISM_Resignation_DTO dto);
        ISM_Resignation_DTO print_exit_employee(ISM_Resignation_DTO dto);
        ISM_Resignation_DTO download_doc(ISM_Resignation_DTO dto);
        //============================reports================

        ISM_Resignation_DTO get_all_data_R(ISM_Resignation_DTO dto);
       Task<ISM_Resignation_DTO> showdetails_R(ISM_Resignation_DTO dto);
       ISM_Resignation_DTO get_all_relieving_data_R(ISM_Resignation_DTO dto);
       Task<ISM_Resignation_DTO> showdetails_relieving_R(ISM_Resignation_DTO dto);      
        ISM_Resignation_DTO relieving_exit_employee_view(ISM_Resignation_DTO dto);

        //GAUTAM
        ISM_Resignation_DTO loadEmployeeData(ISM_Resignation_DTO dto);
        ISM_Resignation_DTO sendResignationMail(ISM_Resignation_DTO dto);
        //GAUTAM
    }
}
