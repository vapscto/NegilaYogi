using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;
using PreadmissionDTOs.com.vaps.VMS.Training;

namespace IVRMUX.Delegates.com.vapstech.VMS.Training
{
    public class SummaryreportsDelegate
    {
        CommonDelegate<SummaryreportsDTO, SummaryreportsDTO> _com = new CommonDelegate<SummaryreportsDTO, SummaryreportsDTO>();

        public SummaryreportsDTO onloaddata(SummaryreportsDTO data)
        {
            return _com.POSTVMS(data,"SummaryreportsFacade/onloaddata");
        }
        public SummaryreportsDTO getreport(SummaryreportsDTO data)
        {
            return _com.POSTVMS(data, "SummaryreportsFacade/getreport");
        }
        public SummaryreportsDTO inhouseReportreport(SummaryreportsDTO data)
        {
            return _com.POSTVMS(data, "SummaryreportsFacade/inhouseReportreport");
        }
        public SummaryreportsDTO trainingcertificate(SummaryreportsDTO data)
        {
            return _com.POSTVMS(data, "SummaryreportsFacade/trainingcertificate");
        }
    }
}

