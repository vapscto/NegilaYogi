using PreadmissionDTOs.PAOnlineExam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.PAOnlineExam.Interface
{
    public interface PAOnlineExamConfigInterface
    {
        PAOnlineExamConfigDTO getloaddata(PAOnlineExamConfigDTO data);
        PAOnlineExamConfigDTO savedetails(PAOnlineExamConfigDTO data);
        PAOnlineExamConfigDTO editQuestion(PAOnlineExamConfigDTO data);
        PAOnlineExamConfigDTO Deletedetails(PAOnlineExamConfigDTO data);
        Task<PAOnlineExamConfigDTO> getreport(PAOnlineExamConfigDTO data);
    }
}
