using PreadmissionDTOs.NAAC.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Documents.Interface
{
    public interface NaacConsolidatedReportInterface
    {
        NaacDocumentUploadReport_DTO getdata(NaacDocumentUploadReport_DTO data);
        Task<NaacDocumentUploadReport_DTO> get_report(NaacDocumentUploadReport_DTO data);
    }
}
