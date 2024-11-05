using CommonLibrary;
using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates
{
    public class PreadmissionNoticeRegistrationReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<PreadmissionNoticeRegistrationReportDTO, PreadmissionNoticeRegistrationReportDTO> COMMM = new CommonDelegate<PreadmissionNoticeRegistrationReportDTO, PreadmissionNoticeRegistrationReportDTO>();

        public PreadmissionNoticeRegistrationReportDTO get_intial_data(PreadmissionNoticeRegistrationReportDTO data)
        {
            return COMMM.POSTData(data, "PreadmissionNoticeRegistrationReportFacade/get_intial_data");
        }
        public PreadmissionNoticeRegistrationReportDTO getprospectusno(PreadmissionNoticeRegistrationReportDTO data)
        {
            return COMMM.POSTData(data, "PreadmissionNoticeRegistrationReportFacade/getprospectusno");
        }
        public PreadmissionNoticeRegistrationReportDTO getstudentlist(PreadmissionNoticeRegistrationReportDTO data)
        {
            return COMMM.POSTData(data, "PreadmissionNoticeRegistrationReportFacade/getstudentlist");
        }
        public PreadmissionNoticeRegistrationReportDTO addtolist(PreadmissionNoticeRegistrationReportDTO data)
        {
            return COMMM.POSTData(data, "PreadmissionNoticeRegistrationReportFacade/addtolist");
        }
        public PreadmissionNoticeRegistrationReportDTO Savedata(PreadmissionNoticeRegistrationReportDTO data)
        {
            return COMMM.POSTData(data, "PreadmissionNoticeRegistrationReportFacade/Savedata");
        }
        public PreadmissionNoticeRegistrationReportDTO viewstudent(PreadmissionNoticeRegistrationReportDTO data)
        {
            return COMMM.POSTData(data, "PreadmissionNoticeRegistrationReportFacade/viewstudent");
        }
        public PreadmissionNoticeRegistrationReportDTO Editdata(PreadmissionNoticeRegistrationReportDTO data)
        {
            return COMMM.POSTData(data, "PreadmissionNoticeRegistrationReportFacade/Editdata");
        }
        public PreadmissionNoticeRegistrationReportDTO printData(PreadmissionNoticeRegistrationReportDTO data)
        {
            return COMMM.POSTData(data, "PreadmissionNoticeRegistrationReportFacade/printData");
        }

    }
}
