using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS.Training
{
    public class Training_Master_Delegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonVMSDelegate<HR_Master_Building_DTO, HR_Master_Building_DTO> COMMB = new CommonVMSDelegate<HR_Master_Building_DTO, HR_Master_Building_DTO>();
       
       // CommonVMSDelegate<HR_Master_Floor_DTO, HR_Master_Floor_DTO> COMMF = new CommonVMSDelegate<HR_Master_Floor_DTO, HR_Master_Floor_DTO>();
        CommonVMSDelegate<HR_Master_Room_DTO, HR_Master_Room_DTO> COMMR = new CommonVMSDelegate<HR_Master_Room_DTO, HR_Master_Room_DTO>();
        CommonVMSDelegate<HR_Master_External_Trainer_Creation_DTO, HR_Master_External_Trainer_Creation_DTO> COMMT = new CommonVMSDelegate<HR_Master_External_Trainer_Creation_DTO, HR_Master_External_Trainer_Creation_DTO>();
         CommonVMSDelegate<HR_Master_Feedback_Qns_DTO, HR_Master_Feedback_Qns_DTO> COMMquestion = new CommonVMSDelegate<HR_Master_Feedback_Qns_DTO, HR_Master_Feedback_Qns_DTO>();
         CommonVMSDelegate<HR_Master_Feedback_Option_DTO, HR_Master_Feedback_Option_DTO> COMMMFO = new CommonVMSDelegate<HR_Master_Feedback_Option_DTO, HR_Master_Feedback_Option_DTO>();
         CommonVMSDelegate<HR_Master_Question_Option_DTO, HR_Master_Question_Option_DTO> COMMMQO = new CommonVMSDelegate<HR_Master_Question_Option_DTO, HR_Master_Question_Option_DTO>();
         CommonVMSDelegate<HR_Training_Question_DTO, HR_Training_Question_DTO> COMMTQM = new CommonVMSDelegate<HR_Training_Question_DTO, HR_Training_Question_DTO>();
         CommonVMSDelegate<HR_Master_TrainingTopicDTO, HR_Master_TrainingTopicDTO> COMMTopic = new CommonVMSDelegate<HR_Master_TrainingTopicDTO, HR_Master_TrainingTopicDTO>();

        CommonDelegate<HR_Master_Floor_DTO, HR_Master_Floor_DTO> COMMF = new CommonDelegate<HR_Master_Floor_DTO, HR_Master_Floor_DTO>();


        //----------------------------Building------------------------------------
        public HR_Master_Building_DTO getdetails_B(HR_Master_Building_DTO id)
        {
            return COMMB.POSTData(id, "Training_MasterFacade/getdetails_B/");
        }
        public HR_Master_Building_DTO SaveEdit_B(HR_Master_Building_DTO dto)
        {
            return COMMB.POSTData(dto, "Training_MasterFacade/SaveEdit_B/");
        }
        public HR_Master_Building_DTO details_B(HR_Master_Building_DTO id)
        {
            return COMMB.POSTData(id, "Training_MasterFacade/detail_B/");
        }
        public HR_Master_Building_DTO deactivate_B(HR_Master_Building_DTO dto)
        {
            return COMMB.POSTData(dto, "Training_MasterFacade/deactivate_B/");
        }
        //---------------------------Floor------------------------------
        public HR_Master_Floor_DTO getdetails_F(HR_Master_Floor_DTO id)
        {
            return COMMF.POSTVMS(id, "Training_MasterFacade/getdetails_F/");
        }
        public HR_Master_Floor_DTO SaveEdit_F(HR_Master_Floor_DTO dto)
        {
            return COMMF.POSTVMS(dto, "Training_MasterFacade/SaveEdit_F/");
        }

        public HR_Master_Floor_DTO deactivate_F(HR_Master_Floor_DTO dto)
        {
            return COMMF.POSTVMS(dto, "Training_MasterFacade/deactivate_F/");
        }
        public HR_Master_Floor_DTO details_F(HR_Master_Floor_DTO id)
        {
            return COMMF.POSTVMS(id, "Training_MasterFacade/details_F/");
        }

        public HR_Master_Floor_DTO deactive_Roomdata(HR_Master_Floor_DTO dto)
        {
            return COMMF.POSTVMS(dto, "Training_MasterFacade/deactive_Roomdata/");
        }

        public HR_Master_Floor_DTO get_Mappedfacility(HR_Master_Floor_DTO id)
        {
            return COMMF.POSTVMS(id, "Training_MasterFacade/get_Mappedfacility/");
        }

        //===================================Room===============================================

        public HR_Master_Room_DTO getdetails_R(HR_Master_Room_DTO id)
        {
            return COMMR.POSTData(id, "Training_MasterFacade/getdetails_R/");
        }
        public HR_Master_Room_DTO SaveEdit_R(HR_Master_Room_DTO dto)
        {
            return COMMR.POSTData(dto, "Training_MasterFacade/SaveEdit_R/");
        }

        public HR_Master_Room_DTO deactivate_R(HR_Master_Room_DTO dto)
        {
            return COMMR.POSTData(dto, "Training_MasterFacade/deactivate_R/");
        }

        public HR_Master_Room_DTO viewuploadflies(HR_Master_Room_DTO dto)
        {
            return COMMR.POSTData(dto, "Training_MasterFacade/viewuploadflies/");
        }
        public HR_Master_Room_DTO deleteuploadfile(HR_Master_Room_DTO dto)
        {
            return COMMR.POSTData(dto, "Training_MasterFacade/deleteuploadfile/");
        }
        public HR_Master_Room_DTO deleteamn(HR_Master_Room_DTO dto)
        {
            return COMMR.POSTData(dto, "Training_MasterFacade/deleteamn/");
        }
        public HR_Master_Room_DTO deletecontact(HR_Master_Room_DTO dto)
        {
            return COMMR.POSTData(dto, "Training_MasterFacade/deletecontact/");
        }
        public HR_Master_Room_DTO viewamnity(HR_Master_Room_DTO dto)
        {
            return COMMR.POSTData(dto, "Training_MasterFacade/viewamnity/");
        }
        public HR_Master_Room_DTO viewcontact(HR_Master_Room_DTO dto)
        {
            return COMMR.POSTData(dto, "Training_MasterFacade/viewcontact/");
        }
        public HR_Master_Room_DTO details_R(HR_Master_Room_DTO id)
        {
            return COMMR.POSTData(id, "Training_MasterFacade/details_R/");
        }

        //=======================================Training==============================

        public HR_Master_External_Trainer_Creation_DTO getdata_T(HR_Master_External_Trainer_Creation_DTO dto)
        {
            return COMMT.POSTData(dto, "Training_MasterFacade/getdata_T/");
        }
        public HR_Master_External_Trainer_Creation_DTO SaveEdit_T(HR_Master_External_Trainer_Creation_DTO dto)
        {
            return COMMT.POSTData(dto, "Training_MasterFacade/SaveEdit_T/");
        }

        public HR_Master_External_Trainer_Creation_DTO deactivate_T(HR_Master_External_Trainer_Creation_DTO dto)
        {
            return COMMT.POSTData(dto, "Training_MasterFacade/deactivate_T/");
        }


        public HR_Master_External_Trainer_Creation_DTO details_T(HR_Master_External_Trainer_Creation_DTO id)
        {
            return COMMT.POSTData(id, "Training_MasterFacade/details_T/");
        }

        //=======================================Master Question=============================

        public HR_Master_Feedback_Qns_DTO getdata_question(HR_Master_Feedback_Qns_DTO dto)
        {
            return COMMquestion.POSTData(dto, "Training_MasterFacade/getdata_question/");
        }
        public HR_Master_Feedback_Qns_DTO SaveEdit_question(HR_Master_Feedback_Qns_DTO dto)
        {
            return COMMquestion.POSTData(dto, "Training_MasterFacade/SaveEdit_question/");
        }

        public HR_Master_Feedback_Qns_DTO deactivate_question(HR_Master_Feedback_Qns_DTO dto)
        {
            return COMMquestion.POSTData(dto, "Training_MasterFacade/deactivate_question/");
        }


        public HR_Master_Feedback_Qns_DTO details_question(HR_Master_Feedback_Qns_DTO id)
        {
            return COMMquestion.POSTData(id, "Training_MasterFacade/details_question/");
        }

        //=======================================Master Feedback option=============================

        public HR_Master_Feedback_Option_DTO getdata_MFO(HR_Master_Feedback_Option_DTO dto)
        {
            return COMMMFO.POSTData(dto, "Training_MasterFacade/getdata_MFO/");
        }
        public HR_Master_Feedback_Option_DTO SaveEdit_MFO(HR_Master_Feedback_Option_DTO dto)
        {
            return COMMMFO.POSTData(dto, "Training_MasterFacade/SaveEdit_MFO/");
        }

        public HR_Master_Feedback_Option_DTO deactivate_MFO(HR_Master_Feedback_Option_DTO dto)
        {
            return COMMMFO.POSTData(dto, "Training_MasterFacade/deactivate_MFO/");
        }


        public HR_Master_Feedback_Option_DTO details_MFO(HR_Master_Feedback_Option_DTO id)
        {
            return COMMMFO.POSTData(id, "Training_MasterFacade/details_MFO/");
        }

        //=======================================Master Question option=============================

        public HR_Master_Question_Option_DTO getdata_MQO(HR_Master_Question_Option_DTO dto)
        {
            return COMMMQO.POSTData(dto, "Training_MasterFacade/getdata_MQO/");
        }
        public HR_Master_Question_Option_DTO SaveEdit_MQO(HR_Master_Question_Option_DTO dto)
        {
            return COMMMQO.POSTData(dto, "Training_MasterFacade/SaveEdit_MQO/");
        }

        public HR_Master_Question_Option_DTO deactivate_MQO(HR_Master_Question_Option_DTO dto)
        {
            return COMMMQO.POSTData(dto, "Training_MasterFacade/deactivate_MQO/");
        }
        public HR_Master_Question_Option_DTO details_MQO(HR_Master_Question_Option_DTO id)
        {
            return COMMMQO.POSTData(id, "Training_MasterFacade/details_MQO/");
        }
         public HR_Master_Question_Option_DTO option_view_MQO(HR_Master_Question_Option_DTO id)
        {
            return COMMMQO.POSTData(id, "Training_MasterFacade/option_view_MQO/");
        }


        //=======================================Training Question mapping=============================

        public HR_Training_Question_DTO getdata_TQM(HR_Training_Question_DTO dto)
        {
            return COMMTQM.POSTData(dto, "Training_MasterFacade/getdata_TQM/");
        }
        public HR_Training_Question_DTO SaveEdit_TQM(HR_Training_Question_DTO dto)
        {
            return COMMTQM.POSTData(dto, "Training_MasterFacade/SaveEdit_TQM/");
        }

        public HR_Training_Question_DTO deactivate_TQM(HR_Training_Question_DTO dto)
        {
            return COMMTQM.POSTData(dto, "Training_MasterFacade/deactivate_TQM/");
        }
        public HR_Training_Question_DTO details_TQM(HR_Training_Question_DTO id)
        {
            return COMMTQM.POSTData(id, "Training_MasterFacade/details_TQM/");
        }
        public HR_Training_Question_DTO option_view_TQM(HR_Training_Question_DTO id)
        {
            return COMMTQM.POSTData(id, "Training_MasterFacade/option_view_TQM/");
        }
         public HR_Training_Question_DTO question_evaluation_TQM(HR_Training_Question_DTO id)
        {
            return COMMTQM.POSTData(id, "Training_MasterFacade/question_evaluation_TQM/");
        }
         public HR_Training_Question_DTO Training_Feedback_TQM(HR_Training_Question_DTO id)
        {
            return COMMTQM.POSTData(id, "Training_MasterFacade/Training_Feedback_TQM/");
        }
        public HR_Master_TrainingTopicDTO getdatatopic(HR_Master_TrainingTopicDTO dto)
        {
            return COMMTopic.POSTData(dto, "Training_MasterFacade/getdatatopic/");
        }
        public HR_Master_TrainingTopicDTO SaveEdit_Topic(HR_Master_TrainingTopicDTO dto)
        {
            return COMMTopic.POSTData(dto, "Training_MasterFacade/SaveEdit_Topic/");
        }
        public HR_Master_TrainingTopicDTO deactivate_Topic(HR_Master_TrainingTopicDTO dto)
        {
            return COMMTopic.POSTData(dto, "Training_MasterFacade/deactivate_Topic/");
        }
        public HR_Master_TrainingTopicDTO details_Topic(HR_Master_TrainingTopicDTO dto)
        {
            return COMMTopic.POSTData(dto, "Training_MasterFacade/details_Topic/");
        }
        
    }
}

