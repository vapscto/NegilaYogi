using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.OnlineExam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
    public interface MasterQuestionCollegeInterface
    {
        MasterQuestionDTO getloaddata(MasterQuestionDTO data);
        MasterQuestionDTO savedetails(MasterQuestionDTO data); 

        MasterQuestionDTO savedataclass(MasterQuestionDTO data);
        MasterQuestionDTO editQuestion(MasterQuestionDTO data);

        MasterQuestionDTO savedetails1(MasterQuestionDTO data);
        MasterQuestionDTO optionChange(MasterQuestionDTO data);
        MasterQuestionDTO optiondetails(MasterQuestionDTO data);
        MasterQuestionDTO Deletedetails(MasterQuestionDTO data);
        MasterQuestionDTO selectcourse(MasterQuestionDTO data);
        MasterQuestionDTO selectbran(MasterQuestionDTO data);
        MasterQuestionDTO editbranchquestion(MasterQuestionDTO data);
        
    }
}
