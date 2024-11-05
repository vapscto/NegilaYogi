using PreadmissionDTOs.PAOnlineExam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.PAOnlineExam.Interface
{
    public interface PAMasterQuestionInterface
    {
        PAMasterQuestionDTO getloaddata(PAMasterQuestionDTO data);
        PAMasterQuestionDTO savedetails(PAMasterQuestionDTO data);
        PAMasterQuestionDTO viewdocumetns(PAMasterQuestionDTO data);
        PAMasterQuestionDTO deactiveparticulars(PAMasterQuestionDTO data);
        PAMasterQuestionDTO savedataclass(PAMasterQuestionDTO data);
        PAMasterQuestionDTO editQuestion(PAMasterQuestionDTO data);
        PAMasterQuestionDTO savedetails1(PAMasterQuestionDTO data);
        PAMasterQuestionDTO optionChange(PAMasterQuestionDTO data);
        PAMasterQuestionDTO optiondetails(PAMasterQuestionDTO data);
        PAMasterQuestionDTO Deletedetails(PAMasterQuestionDTO data);
    }
}
