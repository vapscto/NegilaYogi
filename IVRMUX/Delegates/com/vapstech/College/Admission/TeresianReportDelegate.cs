using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Admission
{
    public class TeresianReportDelegate
    {
        CommonDelegate<TeresianReportDTO, TeresianReportDTO> _commbranch = new CommonDelegate<TeresianReportDTO, TeresianReportDTO>();
        public TeresianReportDTO getdetails(TeresianReportDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "TeresianReportFacade/getdetails/");
        }
        public TeresianReportDTO onselectAcdYear(TeresianReportDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "TeresianReportFacade/onselectAcdYear/");
        }
        public TeresianReportDTO onselectCourse(TeresianReportDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "TeresianReportFacade/onselectCourse/");
        }
        public TeresianReportDTO onselectBranch(TeresianReportDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "TeresianReportFacade/onselectBranch/");
        }
        public TeresianReportDTO onreport(TeresianReportDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "TeresianReportFacade/onreport/");
        }
        public TeresianReportDTO onselectcategory(TeresianReportDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "TeresianReportFacade/onselectcategory/");
        }

    }
}
