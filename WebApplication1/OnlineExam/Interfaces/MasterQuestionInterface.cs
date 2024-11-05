using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.OnlineExam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
    public interface MasterQuestionInterface
    {
        MasterQuestionDTO getloaddata(MasterQuestionDTO data);
        MasterQuestionDTO savedetails(MasterQuestionDTO data);
        MasterQuestionDTO viewdocumetns(MasterQuestionDTO data);
        MasterQuestionDTO deactiveparticulars(MasterQuestionDTO data);

        MasterQuestionDTO savedataclass(MasterQuestionDTO data);
        MasterQuestionDTO editQuestion(MasterQuestionDTO data);
        MasterQuestionDTO savedetails1(MasterQuestionDTO data);
        MasterQuestionDTO optionChange(MasterQuestionDTO data);
        MasterQuestionDTO optiondetails(MasterQuestionDTO data);
        MasterQuestionDTO Deletedetails(MasterQuestionDTO data);        
    }
}