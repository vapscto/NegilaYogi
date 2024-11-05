using CommonLibrary;
using DomainModel.Model.com.vapstech.HRMS;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS.Training
{
    public class Induction_Training_Delegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonVMSDelegate<HR_Training_Create_DTO, HR_Training_Create_DTO> COMMI = new CommonVMSDelegate<HR_Training_Create_DTO, HR_Training_Create_DTO>();
        CommonVMSDelegate<HR_Master_Floor_DTO, HR_Master_Floor_DTO> COMMF = new CommonVMSDelegate<HR_Master_Floor_DTO, HR_Master_Floor_DTO>();
        CommonVMSDelegate<HR_Master_Room_DTO, HR_Master_Room_DTO> COMMR = new CommonVMSDelegate<HR_Master_Room_DTO, HR_Master_Room_DTO>();
        CommonVMSDelegate<HR_Master_DepartmentDTO, HR_Master_DepartmentDTO> COMMM = new CommonVMSDelegate<HR_Master_DepartmentDTO, HR_Master_DepartmentDTO>();
            CommonVMSDelegate<Hr_Master_Employee_DTO, Hr_Master_Employee_DTO> COMME = new CommonVMSDelegate<Hr_Master_Employee_DTO, Hr_Master_Employee_DTO>();
        CommonVMSDelegate<HR_Master_External_Trainer_Creation_DTO, HR_Master_External_Trainer_Creation_DTO> COMMET = new CommonVMSDelegate<HR_Master_External_Trainer_Creation_DTO, HR_Master_External_Trainer_Creation_DTO>();

        public HR_Training_Create_DTO getalldata(HR_Training_Create_DTO id)
        {
            return COMMI.POSTData(id, "Induction_Training_Facade/getalldata/");
        }

        public Hr_Master_Employee_DTO getEmpDD(Hr_Master_Employee_DTO dto)
        {
            return COMME.POSTData(dto, "Induction_Training_Facade/getEmpDD/");
        }

        public HR_Master_Floor_DTO getFloorDD( HR_Master_Floor_DTO dto)
        {
            return COMMF.POSTData(dto, "Induction_Training_Facade/getFloorDD/");
        }

       
        public HR_Master_Room_DTO getRoomDD(HR_Master_Room_DTO dto)
        {
            return COMMR.POSTData(dto, "Induction_Training_Facade/getRoomDD/"); 
        }
        public HR_Training_Create_DTO get_trainer( HR_Training_Create_DTO dto)
        {
            return COMMI.POSTData(dto, "Induction_Training_Facade/get_trainer/");
        }
        public HR_Training_Create_DTO SaveEdit_Induction(HR_Training_Create_DTO dto)
        {
            return COMMI.POSTData(dto, "Induction_Training_Facade/SaveEdit_Induction/");
        }
        public HR_Training_Create_DTO Training_Views(HR_Training_Create_DTO id)
        {
            return COMMI.POSTData(id, "Induction_Training_Facade/Training_Views/");
        }
        public HR_Training_Create_DTO EveGet(HR_Training_Create_DTO id)
        {
            return COMMI.POSTData(id, "Induction_Training_Facade/EveGet/");
        }
        public HR_Training_Create_DTO SaveEvalution_trinee_rating(HR_Training_Create_DTO dto)
        {
            return COMMI.POSTData(dto, "Induction_Training_Facade/SaveEvalution_trinee_rating/");
        }
        public HR_Training_Create_DTO update_status( HR_Training_Create_DTO dto)
        {
            return COMMI.POSTData(dto, "Induction_Training_Facade/update_status/");
        }
       public   HR_Training_Create_DTO edit_induction_create(HR_Training_Create_DTO dto)
        {
            return COMMI.POSTData(dto, "Induction_Training_Facade/edit_induction_create/");
        }
        public HR_Training_Create_DTO edit_training_details( HR_Training_Create_DTO dto)
        {
            return COMMI.POSTData(dto, "Induction_Training_Facade/edit_training_details/");
        }
        public HR_Training_Create_DTO SaveEdit_training_details(HR_Training_Create_DTO dto)
        {
            return COMMI.POSTData(dto, "Induction_Training_Facade/SaveEdit_training_details/");
        }
        public HR_Training_Create_DTO deactivate_Induction_create(HR_Training_Create_DTO dto)
        {
            return COMMI.POSTData(dto, "Induction_Training_Facade/deactivate_Induction_create/");
        }
        public HR_Training_Create_DTO GetInductionReport( HR_Training_Create_DTO dto)
        {
            return COMMI.POSTData(dto, "Induction_Training_Facade/GetInductionReport/");
        }
        public HR_Training_Create_DTO print_trainer_list(HR_Training_Create_DTO dto)
        {
            return COMMI.POSTData(dto, "Induction_Training_Facade/print_trainer_list/");
        }
    }
}
