using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.HRMS
{
    public class SMSemailStaffReportDelegate
    {
        CommonDelegate<SMSemailStaffReportDTO, SMSemailStaffReportDTO> COMMM = new CommonDelegate<SMSemailStaffReportDTO, SMSemailStaffReportDTO>();

        public SMSemailStaffReportDTO onloadgetdetails(SMSemailStaffReportDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "SMSemailStaffReportFacade/onloadgetdetails");
        }        
        public SMSemailStaffReportDTO getreport(SMSemailStaffReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "SMSemailStaffReportFacade/getreport/");
        }
        public SMSemailStaffReportDTO smsemail(SMSemailStaffReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "SMSemailStaffReportFacade/smsemail/");
        }
        //Destination
        public SMSemailStaffReportDTO Destination(SMSemailStaffReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "SMSemailStaffReportFacade/Destination/");
        }
    }
}
