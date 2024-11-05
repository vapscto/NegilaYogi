using PreadmissionDTOs.FeedBack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.FeedBack.Interface
{
    public interface FeedbackTransactionInterface
    {
        FeedbackTransactionDTO getdetails(FeedbackTransactionDTO data);
        FeedbackTransactionDTO getfeedback(FeedbackTransactionDTO data);        
        FeedbackTransactionDTO savedata(FeedbackTransactionDTO data);
        FeedbackTransactionDTO getstudentstaffdetails(FeedbackTransactionDTO data);
        FeedbackTransactionDTO getstaffname(FeedbackTransactionDTO data);
        FeedbackTransactionDTO getfeedbacksubject(FeedbackTransactionDTO data);        
        FeedbackTransactionDTO studentstaffsavedata(FeedbackTransactionDTO data);

        // Feedbcak Modulewise 17-02-2024
        FeedbackTransactionDTO modulewisefeedback(FeedbackTransactionDTO data);
        FeedbackTransactionDTO loadfeedbackquestion(FeedbackTransactionDTO data);
    }
}
