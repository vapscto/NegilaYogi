using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.OnlineExam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
    public interface OnlineExamConfigInterface
    {
        MasterQuestionDTO getloaddata(MasterQuestionDTO data);
        MasterQuestionDTO savedetails(MasterQuestionDTO data);
        MasterQuestionDTO editQuestion(MasterQuestionDTO data);
        MasterQuestionDTO Deletedetails(MasterQuestionDTO data);
        Task<MasterQuestionDTO> getreport(MasterQuestionDTO data);
        MasterQuestionDTO getsection(MasterQuestionDTO data);
        MasterQuestionDTO getonlinereport(MasterQuestionDTO data);
        
    }
}
