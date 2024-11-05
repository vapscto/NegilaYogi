using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.PAOnlineExam;

namespace IVRMUX.Delegates.PAOnlineExam
{
    public class PAMasterQuestionDelegate
    {
        CommonDelegate<PAMasterQuestionDTO, PAMasterQuestionDTO> COMMM = new CommonDelegate<PAMasterQuestionDTO, PAMasterQuestionDTO>();
        public PAMasterQuestionDTO getloaddata(PAMasterQuestionDTO data)
        {
            return COMMM.POSTData(data, "PAMasterQuestionFacade/getloaddata/");
        }

        //-----------------------1st Tab
        public PAMasterQuestionDTO savedetails(PAMasterQuestionDTO data)
        {
            return COMMM.POSTData(data, "PAMasterQuestionFacade/savedetails/");
        }
        public PAMasterQuestionDTO viewdocumetns(PAMasterQuestionDTO data)
        {
            return COMMM.POSTData(data, "PAMasterQuestionFacade/viewdocumetns/");
        }
        public PAMasterQuestionDTO deactiveparticulars(PAMasterQuestionDTO data)
        {
            return COMMM.POSTData(data, "PAMasterQuestionFacade/deactiveparticulars/");
        }

        //-----------------------2st Tab
        public PAMasterQuestionDTO savedataclass(PAMasterQuestionDTO data)
        {
            return COMMM.POSTData(data, "PAMasterQuestionFacade/savedataclass/");
        }



        public PAMasterQuestionDTO editQuestion(PAMasterQuestionDTO data)
        {
            return COMMM.POSTData(data, "PAMasterQuestionFacade/editQuestion/");
        }

        //-----------------------------2nd Tab
        public PAMasterQuestionDTO savedetails1(PAMasterQuestionDTO data)
        {
            return COMMM.POSTData(data, "PAMasterQuestionFacade/savedetails1/");
        }
        public PAMasterQuestionDTO optionChange(PAMasterQuestionDTO data)
        {
            return COMMM.POSTData(data, "PAMasterQuestionFacade/optionChange/");
        }
        public PAMasterQuestionDTO optiondetails(PAMasterQuestionDTO data)
        {
            return COMMM.POSTData(data, "PAMasterQuestionFacade/optiondetails/");
        }
        public PAMasterQuestionDTO Deletedetails(PAMasterQuestionDTO data)
        {
            return COMMM.POSTData(data, "PAMasterQuestionFacade/Deletedetails/");
        }

    }
}
