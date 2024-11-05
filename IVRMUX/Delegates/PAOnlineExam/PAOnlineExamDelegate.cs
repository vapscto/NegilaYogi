using CommonLibrary;
using PreadmissionDTOs.PAOnlineExam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.PAOnlineExam
{
    public class PAOnlineExamDelegate
    {
        CommonDelegate<PAOnlineExamDTO, PAOnlineExamDTO> COMMM = new CommonDelegate<PAOnlineExamDTO, PAOnlineExamDTO>();
        public PAOnlineExamDTO getloaddata(PAOnlineExamDTO data)
        {
            return COMMM.POSTData(data, "PAOnlineExamFacade/getloaddata/");
        }

        public PAOnlineExamDTO getSubjects(PAOnlineExamDTO data)
        {
            return COMMM.POSTData(data, "PAOnlineExamFacade/getSubjects/");
        }
        public PAOnlineExamDTO getQuestion(PAOnlineExamDTO data)
        {
            return COMMM.POSTData(data, "PAOnlineExamFacade/getQuestion/");
        }
        public PAOnlineExamDTO Saveanswer(PAOnlineExamDTO data)
        {
            return COMMM.POSTData(data, "PAOnlineExamFacade/Saveanswer/");
        }
        public PAOnlineExamDTO savedanswers(PAOnlineExamDTO data)
        {
            return COMMM.POSTData(data, "PAOnlineExamFacade/savedanswers/");
        }
        public PAOnlineExamDTO submitexam(PAOnlineExamDTO data)
        {
            return COMMM.POSTData(data, "PAOnlineExamFacade/submitexam/");
        }
    }
}
