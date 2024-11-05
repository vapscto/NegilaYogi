using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VMS.Training;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.Training;

namespace IVRMUX.Controllers.com.vapstech.VMS.Training
{
    [Produces("application/json")]
    [Route("api/Training_Master")]
    public class Training_MasterController : Controller
    {
        Training_Master_Delegate TMD = new Training_Master_Delegate();


        //---------------------------Building----------------------------------------
        [HttpGet]
        [Route("getdata_B/{id:int}")]
        public HR_Master_Building_DTO getdata_B(int id)
        {
            HR_Master_Building_DTO dto = new HR_Master_Building_DTO();
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return TMD.getdetails_B(dto);
        }
        [Route("SaveEdit_B")]
        public HR_Master_Building_DTO SaveEdit_B([FromBody] HR_Master_Building_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return TMD.SaveEdit_B(dto);
        }
        [Route("details_B/{id:int}")]
        public HR_Master_Building_DTO details_B(int id)
        {
            HR_Master_Building_DTO dto = new HR_Master_Building_DTO();
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRMB_Id = id;
            return TMD.details_B(dto);
        }
        [HttpPost]
        [Route("deactivate_B")]
        public HR_Master_Building_DTO deactvateB([FromBody] HR_Master_Building_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRMB_ActiveFlag = dto.HRMB_ActiveFlag;
            return TMD.deactivate_B(dto);
        }
        //---------------------------Floor----------------------------------------
        [HttpGet]
        [Route("getdata_F/{id:int}")]
        public HR_Master_Floor_DTO getdetails_F(int id)
        {
            HR_Master_Floor_DTO dto = new HR_Master_Floor_DTO();
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return TMD.getdetails_F(dto);
        }


        [Route("SaveEdit_F")]
        public HR_Master_Floor_DTO SaveEdit_F([FromBody] HR_Master_Floor_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return TMD.SaveEdit_F(dto);
        }

        //[HttpPost]
        //[Route("deactivate_F")]
        //public HR_Master_Floor_DTO deactvateF([FromBody] HR_Master_Floor_DTO dto)
        //{
        //    dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
        //    dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    dto.HRMF_ActiveFlag = dto.HRMF_ActiveFlag;
        //    return TMD.deactivate_F(dto);
        //}

        //[Route("details_F/{id:int}")]
        //public HR_Master_Floor_DTO details_F(int id)
        //{
        //    HR_Master_Floor_DTO dto = new HR_Master_Floor_DTO();
        //    dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
        //    dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    dto.HRMF_Id = id;
        //    return TMD.details_F(dto);
        //}

        [HttpPost]
        [Route("deactivate_F")]
        public HR_Master_Floor_DTO deactvateF([FromBody] HR_Master_Floor_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRMF_ActiveFlag = dto.HRMF_ActiveFlag;
            return TMD.deactivate_F(dto);
        }

        [Route("details_F")]
        public HR_Master_Floor_DTO details_F([FromBody] HR_Master_Floor_DTO dto)
        {            
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));      
            return TMD.details_F(dto);
        }
        [Route("deactive_Roomdata")]
        public HR_Master_Floor_DTO deactive_Roomdata([FromBody] HR_Master_Floor_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.deactive_Roomdata(data);
        }

        [Route("get_Mappedfacility")]
        public HR_Master_Floor_DTO get_Mappedfacility([FromBody] HR_Master_Floor_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return TMD.get_Mappedfacility(data);
        }



        //-----------------------------Room----------------------------------------------

        [HttpGet]
        [Route("getdata_R/{id:int}")]
        public HR_Master_Room_DTO getdata_R(int id)
        {
            HR_Master_Room_DTO dto = new HR_Master_Room_DTO();
            dto.MI_Id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return TMD.getdetails_R(dto);
        }


        [Route("SaveEdit_R")]
        public HR_Master_Room_DTO SaveEdit_R([FromBody] HR_Master_Room_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return TMD.SaveEdit_R(dto);
        }

        [HttpPost]
        [Route("deactivate_R")]
        public HR_Master_Room_DTO deactvateR([FromBody] HR_Master_Room_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            
            return TMD.deactivate_R(dto);
        }

        [HttpPost]
        [Route("viewuploadflies")]
        public HR_Master_Room_DTO viewuploadflies([FromBody] HR_Master_Room_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.viewuploadflies(dto);
        }
        [HttpPost]
        [Route("deleteuploadfile")]
        public HR_Master_Room_DTO deleteuploadfile([FromBody] HR_Master_Room_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.deleteuploadfile(dto);
        }
        [HttpPost]
        [Route("deleteamn")]
        public HR_Master_Room_DTO deleteamn([FromBody] HR_Master_Room_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.deleteamn(dto);
        }

        [HttpPost]
        [Route("deletecontact")]
        public HR_Master_Room_DTO deletecontact([FromBody] HR_Master_Room_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.deletecontact(dto);
        }
        [HttpPost]
        [Route("viewamnity")]
        public HR_Master_Room_DTO viewamnity([FromBody] HR_Master_Room_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.viewamnity(dto);
        }
        [HttpPost]
        [Route("viewcontact")]
        public HR_Master_Room_DTO viewcontact([FromBody] HR_Master_Room_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.viewcontact(dto);
        }

        [Route("details_R/{id:int}")]
        public HR_Master_Room_DTO details_R(int id)
        {
            HR_Master_Room_DTO dto = new HR_Master_Room_DTO();
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRMR_Id = id;
            return TMD.details_R(dto);
        }

        // =======================================Training=============================================

        [HttpGet]
        [Route("getdata_T/{id:int}")]
        public HR_Master_External_Trainer_Creation_DTO getdata_T(int id)
        {
            HR_Master_External_Trainer_Creation_DTO dto = new HR_Master_External_Trainer_Creation_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return TMD.getdata_T(dto);
        }


        [Route("SaveEdit_T")]
        public HR_Master_External_Trainer_Creation_DTO SaveEdit_T([FromBody] HR_Master_External_Trainer_Creation_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.SaveEdit_T(dto);
        }

        [HttpPost]
        [Route("deactivate_T")]
        public HR_Master_External_Trainer_Creation_DTO deactivate_T([FromBody] HR_Master_External_Trainer_Creation_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            dto.HRMETR_ParttimeORFullTimeFlg = dto.HRMETR_ParttimeORFullTimeFlg;
            return TMD.deactivate_T(dto);
        }



        [Route("details_T/{id:int}")]
        public HR_Master_External_Trainer_Creation_DTO details_T(int id)
        {
            HR_Master_External_Trainer_Creation_DTO dto = new HR_Master_External_Trainer_Creation_DTO();
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRMETR_Id = id;
            return TMD.details_T(dto);
        }

        //===================== Master question==========

         [HttpGet]
        [Route("getdata_question/{id:int}")]
        public HR_Master_Feedback_Qns_DTO getdata_question(int id)
        {
            HR_Master_Feedback_Qns_DTO dto = new HR_Master_Feedback_Qns_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return TMD.getdata_question(dto);
        }


        [Route("SaveEdit_question")]
        public HR_Master_Feedback_Qns_DTO SaveEdit_question([FromBody] HR_Master_Feedback_Qns_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.SaveEdit_question(dto);
        }

        [HttpPost]
        [Route("deactivate_question")]
        public HR_Master_Feedback_Qns_DTO deactivate_question([FromBody] HR_Master_Feedback_Qns_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return TMD.deactivate_question(dto);
        }



        [Route("details_question/{id:int}")]
        public HR_Master_Feedback_Qns_DTO details_question(int id)
        {
            HR_Master_Feedback_Qns_DTO dto = new HR_Master_Feedback_Qns_DTO();
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRMFQNS_Id = id;
            return TMD.details_question(dto);
        }

        //===================== Master Feedback option==========

        [HttpGet]
        [Route("getdata_MFO/{id:int}")]
        public HR_Master_Feedback_Option_DTO getdata_MFO(int id)
        {
            HR_Master_Feedback_Option_DTO dto = new HR_Master_Feedback_Option_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return TMD.getdata_MFO(dto);
        }


        [Route("SaveEdit_MFO")]
        public HR_Master_Feedback_Option_DTO SaveEdit_MFO([FromBody] HR_Master_Feedback_Option_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.SaveEdit_MFO(dto);
        }

        [HttpPost]
        [Route("deactivate_MFO")]
        public HR_Master_Feedback_Option_DTO deactivate_MFO([FromBody] HR_Master_Feedback_Option_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return TMD.deactivate_MFO(dto);
        }



        [Route("details_MFO/{id:int}")]
        public HR_Master_Feedback_Option_DTO details_MFO(int id)
        {
            HR_Master_Feedback_Option_DTO dto = new HR_Master_Feedback_Option_DTO();
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRMFOPT_Id = id;
            return TMD.details_MFO(dto);
        }

        //===================== Master Question option==========

        [HttpGet]
        [Route("getdata_MQO/{id:int}")]
        public HR_Master_Question_Option_DTO getdata_MQO(int id)
        {
            HR_Master_Question_Option_DTO dto = new HR_Master_Question_Option_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return TMD.getdata_MQO(dto);
        }


        [Route("SaveEdit_MQO")]
        public HR_Master_Question_Option_DTO SaveEdit_MQO([FromBody] HR_Master_Question_Option_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.SaveEdit_MQO(dto);
        }

        [HttpPost]
        [Route("deactivate_MQO")]
        public HR_Master_Question_Option_DTO deactivate_MQO([FromBody] HR_Master_Question_Option_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return TMD.deactivate_MQO(dto);
        }



        [Route("details_MQO/{id:int}")]
        public HR_Master_Question_Option_DTO details_MQO(int id)
        {
            HR_Master_Question_Option_DTO dto = new HR_Master_Question_Option_DTO();
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRMFQNS_Id = id;
            return TMD.details_MQO(dto);
        }
        [Route("option_view_MQO/{id:int}")]
        public HR_Master_Question_Option_DTO option_view_MQO(int id)
        {
            HR_Master_Question_Option_DTO dto = new HR_Master_Question_Option_DTO();
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRMFQNS_Id = id;
            return TMD.option_view_MQO(dto);
        }

        //===================== Training Question Mapping==========

        [HttpGet]
        [Route("getdata_TQM/{id:int}")]
        public HR_Training_Question_DTO getdata_TQM(int id)
        {
            HR_Training_Question_DTO dto = new HR_Training_Question_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return TMD.getdata_TQM(dto);
        }


        [Route("SaveEdit_TQM")]
        public HR_Training_Question_DTO SaveEdit_TQM([FromBody] HR_Training_Question_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.SaveEdit_TQM(dto);
        }

        [HttpPost]
        [Route("deactivate_TQM")]
        public HR_Training_Question_DTO deactivate_TQM([FromBody] HR_Training_Question_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return TMD.deactivate_TQM(dto);
        }



        [Route("details_TQM/{id:int}")]
        public HR_Training_Question_DTO details_TQM(int id)
        {
            HR_Training_Question_DTO dto = new HR_Training_Question_DTO();
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRTCR_Id = id;
            return TMD.details_TQM(dto);
        }
        [Route("option_view_TQM/{id:int}")]
        public HR_Training_Question_DTO option_view_TQM(int id)
        {
            HR_Training_Question_DTO dto = new HR_Training_Question_DTO();
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRTCR_Id = id;
            return TMD.option_view_TQM(dto);
        }

        [Route("question_evaluation_TQM")]
        public HR_Training_Question_DTO question_evaluation_TQM([FromBody]HR_Training_Question_DTO dto)
        {
           
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
           
            return TMD.question_evaluation_TQM(dto);
        }
        [Route("Training_Feedback_TQM")]
        public HR_Training_Question_DTO Training_Feedback_TQM([FromBody]HR_Training_Question_DTO dto)
        {
           
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
           
            return TMD.Training_Feedback_TQM(dto);
        }

        [HttpGet]
        [Route("getdatatopic/{id:int}")]
        public HR_Master_TrainingTopicDTO getdatatopic(int id)
        {
            HR_Master_TrainingTopicDTO dto = new HR_Master_TrainingTopicDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.getdatatopic(dto);
        }

        [Route("SaveEdit_Topic")]
        public HR_Master_TrainingTopicDTO SaveEdit_Topic([FromBody]HR_Master_TrainingTopicDTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return TMD.SaveEdit_Topic(dto);
        }

        [Route("deactivate_Topic")]
        public HR_Master_TrainingTopicDTO deactivate_Topic([FromBody]HR_Master_TrainingTopicDTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return TMD.deactivate_Topic(dto);
        }

        [HttpGet]
        [Route("details_Topic/{id:int}")]
        public HR_Master_TrainingTopicDTO details_Topic(int id)
        {
            HR_Master_TrainingTopicDTO dto = new HR_Master_TrainingTopicDTO();
            dto.HRMTT_Id = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.details_Topic(dto);
        }
        
    }
}