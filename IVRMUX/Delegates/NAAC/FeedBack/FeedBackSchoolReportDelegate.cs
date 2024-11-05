using CommonLibrary;
using PreadmissionDTOs.NAAC.FeedBack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.FeedBack
{
    public class FeedBackSchoolReportDelegate
    {
        CommonDelegate<FeedBackSchoolReportDTO, FeedBackSchoolReportDTO> comm = new CommonDelegate<FeedBackSchoolReportDTO, FeedBackSchoolReportDTO>();
        public FeedBackSchoolReportDTO getdetails(FeedBackSchoolReportDTO data)
        {
            return comm.naacdetailsbypost(data, "FeedBackSchoolReportFacade/getdetails");
        }
        public FeedBackSchoolReportDTO getreport(FeedBackSchoolReportDTO data)
        {
            return comm.naacdetailsbypost(data, "FeedBackSchoolReportFacade/getreport");
        }
        //FBGivenCount
        public FeedBackSchoolReportDTO count(FeedBackSchoolReportDTO data)
        {
            return comm.naacdetailsbypost(data, "FeedBackSchoolReportFacade/count");
        }
        //onclass
        public FeedBackSchoolReportDTO onclass(FeedBackSchoolReportDTO data)
        {
            return comm.naacdetailsbypost(data, "FeedBackSchoolReportFacade/onclass");
        }
    }
}
