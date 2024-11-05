using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using DomainModel.Model.com.vapstech.HRMS;
using IVRMUX.Delegates.com.vapstech.VMS.Training;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.VMS.Training;

namespace IVRMUX.Controllers.com.vapstech.VMS.Training
{
    [Produces("application/json")]
    [Route("api/Induction_Training")]
    public class Induction_TrainingController : Controller
    {
        Induction_Training_Delegate itd = new Induction_Training_Delegate();

        [Route("getalldata/{id:int}")]
        public HR_Training_Create_DTO getalldata(int id)
        {
            HR_Training_Create_DTO data = new HR_Training_Create_DTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return itd.getalldata(data);
        }

        [Route("getEmpDD")]
        public Hr_Master_Employee_DTO getEmpDD([FromBody] Hr_Master_Employee_DTO dto)
        {
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return itd.getEmpDD(dto);
        }

        [Route("getFloorDD")]
        public HR_Master_Floor_DTO getFloorDD([FromBody] HR_Master_Floor_DTO dto)
        {
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return itd.getFloorDD(dto);
        }

        [Route("getRoomDD")]
        public HR_Master_Room_DTO getRoomDD([FromBody] HR_Master_Room_DTO dto)
        {
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return itd.getRoomDD(dto);
        }
        [Route("get_trainer")]
        public HR_Training_Create_DTO get_trainer([FromBody] HR_Training_Create_DTO dto)
        {
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return itd.get_trainer(dto);
        }


        [Route("SaveEdit_Induction")]
        public HR_Training_Create_DTO SaveEdit_Induction([FromBody] HR_Training_Create_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return itd.SaveEdit_Induction(dto);
        }
        [Route("SaveEvalution_trinee_rating")]
        public HR_Training_Create_DTO SaveEvalution_trinee_rating([FromBody] HR_Training_Create_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return itd.SaveEvalution_trinee_rating(dto);
        }
        [Route("Training_Views/{id:int}")]
        public HR_Training_Create_DTO Training_Views(int id)
        {
            HR_Training_Create_DTO dto = new HR_Training_Create_DTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.n_id = id;
            return itd.Training_Views(dto);
        }

        [Route("EveGet/{id:int}")]
        public HR_Training_Create_DTO EveGet(int id)
        {
            HR_Training_Create_DTO dto = new HR_Training_Create_DTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.HRTCR_Id = id;
            return itd.EveGet(dto);
        }
        [Route("update_status")]
        public HR_Training_Create_DTO update_status([FromBody] HR_Training_Create_DTO dto)
        {
            return itd.update_status(dto);
        }
        [Route("edit_induction_create")]
        public HR_Training_Create_DTO edit_induction_create([FromBody] HR_Training_Create_DTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return itd.edit_induction_create(dto);
        }
        [Route("edit_training_details")]
        public HR_Training_Create_DTO edit_training_details([FromBody] HR_Training_Create_DTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return itd.edit_training_details(dto);
        }


        [Route("SaveEdit_training_details")]
        public HR_Training_Create_DTO SaveEdit_training_details([FromBody] HR_Training_Create_DTO dto)
        {
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return itd.SaveEdit_training_details(dto);
        }
        [Route("deactivate_Induction_create")]
        public HR_Training_Create_DTO deactivate_Induction_create([FromBody] HR_Training_Create_DTO dto)
        {
            return itd.deactivate_Induction_create(dto);

        }

        [Route("GetInductionReport")]
        public HR_Training_Create_DTO GetInductionReport([FromBody] HR_Training_Create_DTO dto)
        {
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return itd.GetInductionReport(dto);

        }
        [Route("print_trainer_list/{id:int}")]
        public HR_Training_Create_DTO print_trainer_list(int id )
        {
            HR_Training_Create_DTO dto = new HR_Training_Create_DTO();
            dto.HRTCR_Id = id;
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return itd.print_trainer_list(dto);

        }
    }
}