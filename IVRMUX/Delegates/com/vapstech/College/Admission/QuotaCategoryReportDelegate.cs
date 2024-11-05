using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.College.Admission
{
    public class QuotaCategoryReportDelegate
    {
        CommonDelegate<QuotaCategoryReportDTO, QuotaCategoryReportDTO> _commbranch = new CommonDelegate<QuotaCategoryReportDTO, QuotaCategoryReportDTO>();

        public QuotaCategoryReportDTO getdetails(QuotaCategoryReportDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "QuotaCategoryReportFacade/getdetails/");
        }
        public QuotaCategoryReportDTO onselectAcdYear(QuotaCategoryReportDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "QuotaCategoryReportFacade/onselectAcdYear/");
        }
        public QuotaCategoryReportDTO onselectCourse(QuotaCategoryReportDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "QuotaCategoryReportFacade/onselectCourse/");
        }
        public QuotaCategoryReportDTO onselectBranch(QuotaCategoryReportDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "QuotaCategoryReportFacade/onselectBranch/");
        }
        

        public QuotaCategoryReportDTO onreport(QuotaCategoryReportDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "QuotaCategoryReportFacade/onreport/");
        }
    }
}
