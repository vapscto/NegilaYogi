using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.PAOnlineExam;

namespace IVRMUX.Delegates.PAOnlineExam
{
    public class PAOnlineExamConfigDelegate
    {
        CommonDelegate<PAOnlineExamConfigDTO, PAOnlineExamConfigDTO> COMMM = new CommonDelegate<PAOnlineExamConfigDTO, PAOnlineExamConfigDTO>();
        public PAOnlineExamConfigDTO getloaddata(PAOnlineExamConfigDTO data)
        {
            return COMMM.POSTData(data, "PAOnlineExamConfigFacade/getloaddata/");
        }
        public PAOnlineExamConfigDTO savedata(PAOnlineExamConfigDTO data)
        {
            return COMMM.POSTData(data, "PAOnlineExamConfigFacade/savedetails/");
        }
        public PAOnlineExamConfigDTO editQuestion(PAOnlineExamConfigDTO data)
        {
            return COMMM.POSTData(data, "PAOnlineExamConfigFacade/editQuestion/");
        }
        public PAOnlineExamConfigDTO Deletedetails(PAOnlineExamConfigDTO data)
        {
            return COMMM.POSTData(data, "PAOnlineExamConfigFacade/Deletedetails/");
        }
        public PAOnlineExamConfigDTO getreport(PAOnlineExamConfigDTO data)
        {
            return COMMM.POSTData(data, "PAOnlineExamConfigFacade/getreport/");
        }
    }
}
