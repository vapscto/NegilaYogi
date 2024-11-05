using CommonLibrary;
using PreadmissionDTOs.FeedBack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.FeedBack
{
    public class FeedBackReportDelegate
    {
        CommonDelegate<FeedBackReportDTO, FeedBackReportDTO> comm = new CommonDelegate<FeedBackReportDTO, FeedBackReportDTO>();
        public FeedBackReportDTO getdetails(FeedBackReportDTO data)
        {
            return comm.naacdetailsbypost(data, "FeedBackReportFacade/getdetails");
        }
        public FeedBackReportDTO onchangeradio(FeedBackReportDTO data)
        {
            return comm.naacdetailsbypost(data, "FeedBackReportFacade/onchangeradio");
        }        
        public FeedBackReportDTO getreport(FeedBackReportDTO data)
        {
            return comm.naacdetailsbypost(data, "FeedBackReportFacade/getreport");
        }
        public FeedBackReportDTO onchangeyear(FeedBackReportDTO data)
        {
            return comm.naacdetailsbypost(data, "FeedBackReportFacade/onchangeyear");
        }
        public FeedBackReportDTO getreportnew(FeedBackReportDTO data)
        {
            return comm.naacdetailsbypost(data, "FeedBackReportFacade/getreportnew");
        }
        public FeedBackReportDTO onchangefeedback(FeedBackReportDTO data)
        {
            return comm.naacdetailsbypost(data, "FeedBackReportFacade/onchangefeedback");
        }
        public FeedBackReportDTO getstudentlist(FeedBackReportDTO data)
        {
            return comm.naacdetailsbypost(data, "FeedBackReportFacade/getstudentlist");
        }
        
    }
}
