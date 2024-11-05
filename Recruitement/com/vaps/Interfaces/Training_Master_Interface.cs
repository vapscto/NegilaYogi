using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
    public interface Training_Master_Interface
    {
        //---------------------- building-----------------------
        HR_Master_Building_DTO getdate_B(HR_Master_Building_DTO id);
        HR_Master_Building_DTO de_activate_B(HR_Master_Building_DTO id);
        HR_Master_Building_DTO SaveEdit_B(HR_Master_Building_DTO dto);
        HR_Master_Building_DTO details_B(HR_Master_Building_DTO id);

        //----------------------- Floor--------------------------
        //HR_Master_Floor_DTO getdate_F(HR_Master_Floor_DTO id);
        //HR_Master_Floor_DTO SaveEdit_F(HR_Master_Floor_DTO dto);
        //HR_Master_Floor_DTO de_activate_F(HR_Master_Floor_DTO id);
        

        HR_Master_Floor_DTO getdetails_F(HR_Master_Floor_DTO id);
        HR_Master_Floor_DTO SaveEdit_F(HR_Master_Floor_DTO dto);
        HR_Master_Floor_DTO de_activate_F(HR_Master_Floor_DTO id);
        HR_Master_Floor_DTO details_F(HR_Master_Floor_DTO id);
        HR_Master_Floor_DTO get_Mappedfacility(HR_Master_Floor_DTO data);
        HR_Master_Floor_DTO deactive_Roomdata(HR_Master_Floor_DTO data);


        //==============================Room=======================
        HR_Master_Room_DTO getdate_R(HR_Master_Room_DTO id);
        HR_Master_Room_DTO SaveEdit_R(HR_Master_Room_DTO dto);
        HR_Master_Room_DTO de_activate_R(HR_Master_Room_DTO id);

        HR_Master_Room_DTO viewuploadflies(HR_Master_Room_DTO id);
        HR_Master_Room_DTO deleteuploadfile(HR_Master_Room_DTO id);
        HR_Master_Room_DTO deleteamn(HR_Master_Room_DTO id);
        HR_Master_Room_DTO deletecontact(HR_Master_Room_DTO id);
        HR_Master_Room_DTO viewamnity(HR_Master_Room_DTO id);
        HR_Master_Room_DTO viewcontact(HR_Master_Room_DTO id);
        HR_Master_Room_DTO details_R(HR_Master_Room_DTO id);


        //===============================Training===========================
        HR_Master_External_Trainer_Creation_DTO getdata_T(HR_Master_External_Trainer_Creation_DTO dto);
        HR_Master_External_Trainer_Creation_DTO SaveEdit_T(HR_Master_External_Trainer_Creation_DTO dto);
        HR_Master_External_Trainer_Creation_DTO deactivate_T(HR_Master_External_Trainer_Creation_DTO id);
        HR_Master_External_Trainer_Creation_DTO details_T(HR_Master_External_Trainer_Creation_DTO id);

        //===============================Master Question===========================
        HR_Master_Feedback_Qns_DTO getdata_question(HR_Master_Feedback_Qns_DTO dto);
        HR_Master_Feedback_Qns_DTO SaveEdit_question(HR_Master_Feedback_Qns_DTO dto);
        HR_Master_Feedback_Qns_DTO deactivate_question(HR_Master_Feedback_Qns_DTO id);
        HR_Master_Feedback_Qns_DTO details_question(HR_Master_Feedback_Qns_DTO id);

        //===============================Master feedbackm option===========================
        HR_Master_Feedback_Option_DTO getdata_MFO(HR_Master_Feedback_Option_DTO dto);
        HR_Master_Feedback_Option_DTO SaveEdit_MFO(HR_Master_Feedback_Option_DTO dto);
        HR_Master_Feedback_Option_DTO deactivate_MFO(HR_Master_Feedback_Option_DTO id);
        HR_Master_Feedback_Option_DTO details_MFO(HR_Master_Feedback_Option_DTO id);

        //===============================Master Question option===========================
        HR_Master_Question_Option_DTO getdata_MQO(HR_Master_Question_Option_DTO dto);
        HR_Master_Question_Option_DTO SaveEdit_MQO(HR_Master_Question_Option_DTO dto);
        HR_Master_Question_Option_DTO deactivate_MQO(HR_Master_Question_Option_DTO id);
        HR_Master_Question_Option_DTO details_MQO(HR_Master_Question_Option_DTO id);
        HR_Master_Question_Option_DTO option_view_MQO(HR_Master_Question_Option_DTO id);

        //===============================Training Question mapping===========================
        HR_Training_Question_DTO getdata_TQM(HR_Training_Question_DTO dto);
        HR_Training_Question_DTO SaveEdit_TQM(HR_Training_Question_DTO dto);
        HR_Training_Question_DTO deactivate_TQM(HR_Training_Question_DTO id);
        HR_Training_Question_DTO details_TQM(HR_Training_Question_DTO id);
        HR_Training_Question_DTO option_view_TQM(HR_Training_Question_DTO id);
        HR_Training_Question_DTO question_evaluation_TQM(HR_Training_Question_DTO id);
        HR_Training_Question_DTO Training_Feedback_TQM(HR_Training_Question_DTO id);
        HR_Master_TrainingTopicDTO getdatatopic(HR_Master_TrainingTopicDTO dto);
        HR_Master_TrainingTopicDTO SaveEdit_Topic(HR_Master_TrainingTopicDTO dto);
        HR_Master_TrainingTopicDTO deactivate_Topic(HR_Master_TrainingTopicDTO dto);
        HR_Master_TrainingTopicDTO details_Topic(HR_Master_TrainingTopicDTO dto);
    }
}