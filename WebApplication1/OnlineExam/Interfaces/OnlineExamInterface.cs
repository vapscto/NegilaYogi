using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.OnlineExam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
    public interface OnlineExamInterface
    {
        OnlineExamDTO getloaddata(OnlineExamDTO data);
        OnlineExamDTO getSubjects(OnlineExamDTO data);
        OnlineExamDTO getQuestion(OnlineExamDTO data);
        OnlineExamDTO Saveanswer(OnlineExamDTO data);
        OnlineExamDTO savedanswers(OnlineExamDTO data);
        Task<OnlineExamDTO> submitexam(OnlineExamDTO data);
        
    }
}
