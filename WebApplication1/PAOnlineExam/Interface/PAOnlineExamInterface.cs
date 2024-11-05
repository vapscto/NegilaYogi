using PreadmissionDTOs.PAOnlineExam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.PAOnlineExam.Interface
{
    public interface PAOnlineExamInterface
    {
        PAOnlineExamDTO getloaddata(PAOnlineExamDTO data);
        PAOnlineExamDTO getSubjects(PAOnlineExamDTO data);
        PAOnlineExamDTO getQuestion(PAOnlineExamDTO data);
        PAOnlineExamDTO Saveanswer(PAOnlineExamDTO data);
        PAOnlineExamDTO savedanswers(PAOnlineExamDTO data);
        Task<PAOnlineExamDTO> submitexam(PAOnlineExamDTO data);
    }
}
