using PreadmissionDTOs.FeedBack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace NaacServiceHub.FeedBack.Interface
{
    public interface FeedbackSchoolGeneralTransactionInterface
    {
        FeedbackSchoolGeneralTransactionDTO getdetails(FeedbackSchoolGeneralTransactionDTO data);
        FeedbackSchoolGeneralTransactionDTO getfeedback(FeedbackSchoolGeneralTransactionDTO data);
        FeedbackSchoolGeneralTransactionDTO savedata(FeedbackSchoolGeneralTransactionDTO data);
        FeedbackSchoolGeneralTransactionDTO getstudentstaffdetails(FeedbackSchoolGeneralTransactionDTO data);
        FeedbackSchoolGeneralTransactionDTO getstaffname(FeedbackSchoolGeneralTransactionDTO data);
        FeedbackSchoolGeneralTransactionDTO getfeedbacksubject(FeedbackSchoolGeneralTransactionDTO data);
        FeedbackSchoolGeneralTransactionDTO studentstaffsavedata(FeedbackSchoolGeneralTransactionDTO data);
    }
}
