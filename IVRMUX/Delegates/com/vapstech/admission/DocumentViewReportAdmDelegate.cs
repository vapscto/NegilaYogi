using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.admission
{
    public class DocumentViewReportAdmDelegate
    {
        CommonDelegate<DocumentViewReportAdmDTO, DocumentViewReportAdmDTO> _comm = new CommonDelegate<DocumentViewReportAdmDTO, DocumentViewReportAdmDTO>();
        public DocumentViewReportAdmDTO getdetails(int id)
        {
            return _comm.GetDataByIdADM(id, "DocumentViewReportAdmFacade/getdetails/");
        }
        public DocumentViewReportAdmDTO getstudent(DocumentViewReportAdmDTO data)
        {
            return _comm.POSTDataADM(data, "DocumentViewReportAdmFacade/getstudent/");
        }

        public DocumentViewReportAdmDTO getreport(DocumentViewReportAdmDTO data)
        {
            return _comm.POSTDataADM(data, "DocumentViewReportAdmFacade/getreport/");
        }
        
    }
}
