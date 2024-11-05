using CommonLibrary;
using PreadmissionDTOs.FeedBack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.FeedBack
{
    public class AcademicCalenderReportDelegate
    {
        CommonDelegate<AcademicCalenderReportDTO, AcademicCalenderReportDTO> _comm = new CommonDelegate<AcademicCalenderReportDTO, AcademicCalenderReportDTO>();

        public AcademicCalenderReportDTO getdetails(AcademicCalenderReportDTO data)
        {
            return _comm.naacdetailsbypost(data, "AcademicCalenderReportFacade/getdetails");
        }
        public AcademicCalenderReportDTO getreport(AcademicCalenderReportDTO data)
        {
            return _comm.naacdetailsbypost(data, "AcademicCalenderReportFacade/getreport");
        }
    }
}
