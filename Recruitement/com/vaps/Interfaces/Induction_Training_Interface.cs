using DomainModel.Model.com.vapstech.HRMS;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
    public interface Induction_Training_Interface
    {
        HR_Training_Create_DTO getalldata(HR_Training_Create_DTO id);
        HR_Master_Floor_DTO getFloorDD(HR_Master_Floor_DTO data);
        HR_Master_Room_DTO getRoomDD(HR_Master_Room_DTO data);
        Hr_Master_Employee_DTO getEmpDD(Hr_Master_Employee_DTO data);
        HR_Training_Create_DTO get_trainer(HR_Training_Create_DTO dto);
        HR_Training_Create_DTO SaveEdit_Induction(HR_Training_Create_DTO data);
        HR_Training_Create_DTO Training_Views(HR_Training_Create_DTO id);
        HR_Training_Create_DTO update_status(HR_Training_Create_DTO dto);
        HR_Training_Create_DTO EveGet(HR_Training_Create_DTO id);
        HR_Training_Create_DTO SaveEvalution_trinee_rating(HR_Training_Create_DTO dto);
        HR_Training_Create_DTO edit_induction_create(HR_Training_Create_DTO dto);
        HR_Training_Create_DTO edit_training_details(HR_Training_Create_DTO dto);
        HR_Training_Create_DTO SaveEdit_training_details(HR_Training_Create_DTO dto);

        HR_Training_Create_DTO deactivate_Induction_create(HR_Training_Create_DTO dto);

        HR_Training_Create_DTO GetInductionReport(HR_Training_Create_DTO dto);
        HR_Training_Create_DTO print_trainer_list(HR_Training_Create_DTO dto);


    }
}
