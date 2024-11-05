using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;
namespace IVRMUX.Delegates.com.vapstech.IssueManager.PettyCash
{
    public class PC_ReportDelegate
    {
        CommonDelegate<PC_ReportDTO, PC_ReportDTO> _comm = new CommonDelegate<PC_ReportDTO, PC_ReportDTO>();
        public PC_ReportDTO onloaddata(PC_ReportDTO data)
        {
            return _comm.POSTVMS(data, "PC_ReportFacade/onloaddata");
        }
        public PC_ReportDTO getrequisitionreport(PC_ReportDTO data)
        {
            return _comm.POSTVMS(data, "PC_ReportFacade/getrequisitionreport");
        }       
        public PC_ReportDTO getindentreport(PC_ReportDTO data)
        {
            return _comm.POSTVMS(data, "PC_ReportFacade/getindentreport");
        }
        public PC_ReportDTO getindentapprovedreport(PC_ReportDTO data)
        {
            return _comm.POSTVMS(data, "PC_ReportFacade/getindentapprovedreport");
        }
        public PC_ReportDTO getexpenditurereport(PC_ReportDTO data)
        {
            return _comm.POSTVMS(data, "PC_ReportFacade/getexpenditurereport");
        }
    }
}
