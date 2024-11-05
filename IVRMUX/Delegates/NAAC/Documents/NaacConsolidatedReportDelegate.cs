using CommonLibrary;
using PreadmissionDTOs.NAAC.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Documents
{
    public class NaacConsolidatedReportDelegate
    {
        CommonDelegate<NaacDocumentUploadReport_DTO, NaacDocumentUploadReport_DTO> _commnbranch = new CommonDelegate<NaacDocumentUploadReport_DTO, NaacDocumentUploadReport_DTO>();

        public NaacDocumentUploadReport_DTO getdata(NaacDocumentUploadReport_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NaacConsolidatedReportFacade/getdata/");
        }

        public NaacDocumentUploadReport_DTO get_report(NaacDocumentUploadReport_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NaacConsolidatedReportFacade/get_report/");
        }
    }
}
