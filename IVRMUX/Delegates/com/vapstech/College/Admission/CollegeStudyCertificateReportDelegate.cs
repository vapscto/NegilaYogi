using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Admission
{
    public class CollegeStudyCertificateReportDelegate
    {
        CommonDelegate<CollegeStudyCertificateReportDTO, CollegeStudyCertificateReportDTO> _comm = new CommonDelegate<CollegeStudyCertificateReportDTO, CollegeStudyCertificateReportDTO>();

        public CollegeStudyCertificateReportDTO getdata(CollegeStudyCertificateReportDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeStudyCertificateReportFacade/getdata");
        }
        public CollegeStudyCertificateReportDTO onchangeyear(CollegeStudyCertificateReportDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeStudyCertificateReportFacade/onchangeyear");
        }
        public CollegeStudyCertificateReportDTO onchangecourse(CollegeStudyCertificateReportDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeStudyCertificateReportFacade/onchangecourse");
        }
        public CollegeStudyCertificateReportDTO onchangebranch(CollegeStudyCertificateReportDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeStudyCertificateReportFacade/onchangebranch");
        }
        public CollegeStudyCertificateReportDTO onchangesemester(CollegeStudyCertificateReportDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeStudyCertificateReportFacade/onchangesemester");
        }
        public CollegeStudyCertificateReportDTO searchfilter(CollegeStudyCertificateReportDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeStudyCertificateReportFacade/searchfilter");
        }
        public CollegeStudyCertificateReportDTO onclickreport(CollegeStudyCertificateReportDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeStudyCertificateReportFacade/onclickreport");
        }
    }
}
