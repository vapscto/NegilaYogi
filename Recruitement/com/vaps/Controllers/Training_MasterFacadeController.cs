using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.Training;
using Recruitment.com.vaps.Interfaces;

namespace Recruitment.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class Training_MasterFacadeController : Controller
    {
        public Training_Master_Interface _flr;


        public  Training_MasterFacadeController (Training_Master_Interface ct)
        {
            _flr = ct;
        }
        //----------------------------Building-----------------------------
        [Route("getdetails_B")]
        public HR_Master_Building_DTO getData_B([FromBody] HR_Master_Building_DTO id)
        {
            return _flr.getdate_B(id);
        }
        [Route("SaveEdit_B")]
        public HR_Master_Building_DTO SaveEdit_B([FromBody] HR_Master_Building_DTO  dto)
        {
            return _flr.SaveEdit_B(dto);
        }
       
        [Route("detail_B")]
        public HR_Master_Building_DTO details_B([FromBody] HR_Master_Building_DTO id)
        {
            return _flr.details_B(id);
        }

        [HttpPost]
        [Route("deactivate_B")]
        public HR_Master_Building_DTO deactivate_B([FromBody] HR_Master_Building_DTO dTO)
        {
            return _flr.de_activate_B(dTO);
        }
        //----------------------Floor-----------------------------
        [Route("getdetails_F")]
        public HR_Master_Floor_DTO getdetails_F([FromBody] HR_Master_Floor_DTO id)
        {
            return _flr.getdetails_F(id);
        }
        [Route("SaveEdit_F")]
        public HR_Master_Floor_DTO SaveEdit_F([FromBody] HR_Master_Floor_DTO dTO )
        {
            return _flr.SaveEdit_F(dTO);
        }

        [HttpPost]
        [Route("deactivate_F")]
        public HR_Master_Floor_DTO deactivate_F([FromBody] HR_Master_Floor_DTO dTO)
        {
            return _flr.de_activate_F(dTO);
        }

        [Route("details_F")]
        public HR_Master_Floor_DTO details_F ([FromBody] HR_Master_Floor_DTO id)
        {
            return _flr.details_F(id);
        }
        [Route("deactive_Roomdata")]
        public HR_Master_Floor_DTO deactive_Roomdata([FromBody] HR_Master_Floor_DTO data)
        {
            return _flr.deactive_Roomdata(data);
        }

        [Route("get_Mappedfacility")]
        public HR_Master_Floor_DTO get_Mappedfacility([FromBody] HR_Master_Floor_DTO data)
        {
            return _flr.get_Mappedfacility(data);
        }



        //=================================================Room=========================================
        [Route("getdetails_R")]
        public HR_Master_Room_DTO getData_R([FromBody] HR_Master_Room_DTO id)
        {
            return _flr.getdate_R(id);
        }
        [Route("SaveEdit_R")]
        public HR_Master_Room_DTO SaveEdit_R([FromBody] HR_Master_Room_DTO dTO)
        {
            return _flr.SaveEdit_R(dTO);
        }

        [HttpPost]
        [Route("deactivate_R")]
        public HR_Master_Room_DTO deactivate_R([FromBody] HR_Master_Room_DTO dTO)
        {
            return _flr.de_activate_R(dTO);
        }
        [HttpPost]
        [Route("viewuploadflies")]
        public HR_Master_Room_DTO viewuploadflies([FromBody] HR_Master_Room_DTO dTO)
        {
            return _flr.viewuploadflies(dTO);
        }
        [HttpPost]
        [Route("deleteuploadfile")]
        public HR_Master_Room_DTO deleteuploadfile([FromBody] HR_Master_Room_DTO dTO)
        {
            return _flr.deleteuploadfile(dTO);
        }
        [HttpPost]
        [Route("deleteamn")]
        public HR_Master_Room_DTO deleteamn([FromBody] HR_Master_Room_DTO dTO)
        {
            return _flr.deleteamn(dTO);
        }
        [HttpPost]
        [Route("deletecontact")]
        public HR_Master_Room_DTO deletecontact([FromBody] HR_Master_Room_DTO dTO)
        {
            return _flr.deletecontact(dTO);
        }
        [HttpPost]
        [Route("viewamnity")]
        public HR_Master_Room_DTO viewamnity([FromBody] HR_Master_Room_DTO dTO)
        {
            return _flr.viewamnity(dTO);
        }
        [HttpPost]
        [Route("viewcontact")]
        public HR_Master_Room_DTO viewcontact([FromBody] HR_Master_Room_DTO dTO)
        {
            return _flr.viewcontact(dTO);
        }

        [Route("details_R")]
        public HR_Master_Room_DTO details_R([FromBody] HR_Master_Room_DTO id)
        {
            return _flr.details_R(id);
        }
        //===============================Training===================================
        [Route("getdata_T")]
        public HR_Master_External_Trainer_Creation_DTO getdata_T([FromBody] HR_Master_External_Trainer_Creation_DTO dto)
        {
            return _flr.getdata_T(dto);
        }
        [Route("SaveEdit_T")]
        public HR_Master_External_Trainer_Creation_DTO SaveEdit_T([FromBody] HR_Master_External_Trainer_Creation_DTO dTO)
        {
            return _flr.SaveEdit_T(dTO);
        }

        [HttpPost]
        [Route("deactivate_T")]
        public HR_Master_External_Trainer_Creation_DTO deactivate_T([FromBody] HR_Master_External_Trainer_Creation_DTO dTO)
        {
            return _flr.deactivate_T(dTO);
        }

        [Route("details_T")]
        public HR_Master_External_Trainer_Creation_DTO details_T([FromBody]HR_Master_External_Trainer_Creation_DTO id)
        {
            return _flr.details_T(id);
        }

        //=======================================Master Question=============================
        [Route("getdata_question")]
        public HR_Master_Feedback_Qns_DTO getdata_question([FromBody] HR_Master_Feedback_Qns_DTO dto)
        {
            return _flr.getdata_question(dto);
        }
        [Route("SaveEdit_question")]
        public HR_Master_Feedback_Qns_DTO SaveEdit_question([FromBody] HR_Master_Feedback_Qns_DTO dTO)
        {
            return _flr.SaveEdit_question(dTO);
        }

        [HttpPost]
        [Route("deactivate_question")]
        public HR_Master_Feedback_Qns_DTO deactivate_question([FromBody] HR_Master_Feedback_Qns_DTO dTO)
        {
            return _flr.deactivate_question(dTO);
        }

        [Route("details_question")]
        public HR_Master_Feedback_Qns_DTO details_question([FromBody]HR_Master_Feedback_Qns_DTO id)
        {
            return _flr.details_question(id);
        }

        //=======================================Master Feedback option=============================
        [Route("getdata_MFO")]
        public HR_Master_Feedback_Option_DTO getdata_MFO([FromBody] HR_Master_Feedback_Option_DTO dto)
        {
            return _flr.getdata_MFO(dto);
        }
        [Route("SaveEdit_MFO")]
        public HR_Master_Feedback_Option_DTO SaveEdit_MFO([FromBody] HR_Master_Feedback_Option_DTO dTO)
        {
            return _flr.SaveEdit_MFO(dTO);
        }

        [HttpPost]
        [Route("deactivate_MFO")]
        public HR_Master_Feedback_Option_DTO deactivate_MFO([FromBody] HR_Master_Feedback_Option_DTO dTO)
        {
            return _flr.deactivate_MFO(dTO);
        }

        [Route("details_MFO")]
        public HR_Master_Feedback_Option_DTO details_MFO([FromBody]HR_Master_Feedback_Option_DTO id)
        {
            return _flr.details_MFO(id);
        }

        //=======================================Master Question option=============================
        [Route("getdata_MQO")]
        public HR_Master_Question_Option_DTO getdata_MQO([FromBody] HR_Master_Question_Option_DTO dto)
        {
            return _flr.getdata_MQO(dto);
        }
        [Route("SaveEdit_MQO")]
        public HR_Master_Question_Option_DTO SaveEdit_MQO([FromBody] HR_Master_Question_Option_DTO dTO)
        {
            return _flr.SaveEdit_MQO(dTO);
        }

        [HttpPost]
        [Route("deactivate_MQO")]
        public HR_Master_Question_Option_DTO deactivate_MQO([FromBody] HR_Master_Question_Option_DTO dTO)
        {
            return _flr.deactivate_MQO(dTO);
        }

        [Route("details_MQO")]
        public HR_Master_Question_Option_DTO details_MQO([FromBody]HR_Master_Question_Option_DTO id)
        {
            return _flr.details_MQO(id);
        }
         [Route("option_view_MQO")]
        public HR_Master_Question_Option_DTO option_view_MQO([FromBody]HR_Master_Question_Option_DTO id)
        {
            return _flr.option_view_MQO(id);
        }
        //=======================================Training Question mapping=============================
        [Route("getdata_TQM")]
        public HR_Training_Question_DTO getdata_TQM([FromBody] HR_Training_Question_DTO dto)
        {
            return _flr.getdata_TQM(dto);
        }
        [Route("SaveEdit_TQM")]
        public HR_Training_Question_DTO SaveEdit_TQM([FromBody] HR_Training_Question_DTO dTO)
        {
            return _flr.SaveEdit_TQM(dTO);
        }

        [HttpPost]
        [Route("deactivate_TQM")]
        public HR_Training_Question_DTO deactivate_TQM([FromBody] HR_Training_Question_DTO dTO)
        {
            return _flr.deactivate_TQM(dTO);
        }

        [Route("details_TQM")]
        public HR_Training_Question_DTO details_TQM([FromBody]HR_Training_Question_DTO id)
        {
            return _flr.details_TQM(id);
        }
         [Route("option_view_TQM")]
        public HR_Training_Question_DTO option_view_TQM([FromBody]HR_Training_Question_DTO id)
        {
            return _flr.option_view_TQM(id);
        }
         [Route("question_evaluation_TQM")]
        public HR_Training_Question_DTO question_evaluation_TQM([FromBody]HR_Training_Question_DTO id)
        {
            return _flr.question_evaluation_TQM(id);
        }
        [Route("Training_Feedback_TQM")]
        public HR_Training_Question_DTO Training_Feedback_TQM([FromBody]HR_Training_Question_DTO id)
        {
            return _flr.Training_Feedback_TQM(id);
        }

        [HttpPost]
        [Route("getdatatopic")]
        public HR_Master_TrainingTopicDTO getdatatopic([FromBody] HR_Master_TrainingTopicDTO dTO)
        {
            return _flr.getdatatopic(dTO);
        }

        [Route("SaveEdit_Topic")]
        public HR_Master_TrainingTopicDTO SaveEdit_Topic([FromBody] HR_Master_TrainingTopicDTO dTO)
        {
            return _flr.SaveEdit_Topic(dTO);
        }

        [Route("deactivate_Topic")]
        public HR_Master_TrainingTopicDTO deactivate_Topic([FromBody] HR_Master_TrainingTopicDTO dTO)
        {
            return _flr.deactivate_Topic(dTO);
        }

        [Route("details_Topic")]
        public HR_Master_TrainingTopicDTO details_Topic([FromBody] HR_Master_TrainingTopicDTO dTO)
        {
            return _flr.details_Topic(dTO);
        }
        
    }
}