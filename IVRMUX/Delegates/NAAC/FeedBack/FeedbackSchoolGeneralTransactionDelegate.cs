using CommonLibrary;
using PreadmissionDTOs.FeedBack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace IVRMUX.Delegates.FeedBack
{
    public class FeedbackSchoolGeneralTransactionDelegate
    {
        CommonDelegate<FeedbackSchoolGeneralTransactionDTO, FeedbackSchoolGeneralTransactionDTO> _com = new CommonDelegate<FeedbackSchoolGeneralTransactionDTO, FeedbackSchoolGeneralTransactionDTO>();

        public FeedbackSchoolGeneralTransactionDTO getdetails(FeedbackSchoolGeneralTransactionDTO data)
        {
            return _com.naacdetailsbypost(data, "FeedbackSchoolGeneralTransactionFacade/getdetails");
        }
        public FeedbackSchoolGeneralTransactionDTO getfeedback(FeedbackSchoolGeneralTransactionDTO data)
        {
            return _com.naacdetailsbypost(data, "FeedbackSchoolGeneralTransactionFacade/getfeedback");
        }
        
        public FeedbackSchoolGeneralTransactionDTO savedata(FeedbackSchoolGeneralTransactionDTO data)
        {
            return _com.naacdetailsbypost(data, "FeedbackSchoolGeneralTransactionFacade/savedata");
        }

        public FeedbackSchoolGeneralTransactionDTO getstudentstaffdetails(FeedbackSchoolGeneralTransactionDTO data)
        {
            return _com.naacdetailsbypost(data, "FeedbackSchoolGeneralTransactionFacade/getstudentstaffdetails");
        }
        public FeedbackSchoolGeneralTransactionDTO getstaffname(FeedbackSchoolGeneralTransactionDTO data)
        {
            return _com.naacdetailsbypost(data, "FeedbackSchoolGeneralTransactionFacade/getstaffname");
        }
        public FeedbackSchoolGeneralTransactionDTO getfeedbacksubject(FeedbackSchoolGeneralTransactionDTO data)
        {
            return _com.naacdetailsbypost(data, "FeedbackSchoolGeneralTransactionFacade/getfeedbacksubject");
        }
        
        public FeedbackSchoolGeneralTransactionDTO studentstaffsavedata(FeedbackSchoolGeneralTransactionDTO data)
        {
            return _com.naacdetailsbypost(data, "FeedbackSchoolGeneralTransactionFacade/studentstaffsavedata");
        }
    }
}
