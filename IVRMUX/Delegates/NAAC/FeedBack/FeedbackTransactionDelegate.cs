using CommonLibrary;
using PreadmissionDTOs.FeedBack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.FeedBack
{
    public class FeedbackTransactionDelegate
    {
        CommonDelegate<FeedbackTransactionDTO, FeedbackTransactionDTO> _com = new CommonDelegate<FeedbackTransactionDTO, FeedbackTransactionDTO>();

        public FeedbackTransactionDTO getdetails(FeedbackTransactionDTO data)
        {
            return _com.naacdetailsbypost(data, "FeedbackTransactionFacade/getdetails");
        }
        public FeedbackTransactionDTO getfeedback(FeedbackTransactionDTO data)
        {
            return _com.naacdetailsbypost(data, "FeedbackTransactionFacade/getfeedback");
        }
        
        public FeedbackTransactionDTO savedata(FeedbackTransactionDTO data)
        {
            return _com.naacdetailsbypost(data, "FeedbackTransactionFacade/savedata");
        }

        public FeedbackTransactionDTO getstudentstaffdetails(FeedbackTransactionDTO data)
        {
            return _com.naacdetailsbypost(data, "FeedbackTransactionFacade/getstudentstaffdetails");
        }
        public FeedbackTransactionDTO getstaffname(FeedbackTransactionDTO data)
        {
            return _com.naacdetailsbypost(data, "FeedbackTransactionFacade/getstaffname");
        }
        public FeedbackTransactionDTO getfeedbacksubject(FeedbackTransactionDTO data)
        {
            return _com.naacdetailsbypost(data, "FeedbackTransactionFacade/getfeedbacksubject");
        }
        public FeedbackTransactionDTO studentstaffsavedata(FeedbackTransactionDTO data)
        {
            return _com.naacdetailsbypost(data, "FeedbackTransactionFacade/studentstaffsavedata");
        }

        // Feedbcak Modulewise 17-02-2024
        public FeedbackTransactionDTO modulewisefeedback(FeedbackTransactionDTO data)
        {
            return _com.naacdetailsbypost(data, "FeedbackTransactionFacade/modulewisefeedback");
        }

        public FeedbackTransactionDTO loadfeedbackquestion(FeedbackTransactionDTO data)
        {
            return _com.naacdetailsbypost(data, "FeedbackTransactionFacade/loadfeedbackquestion");
        }

    }
}
