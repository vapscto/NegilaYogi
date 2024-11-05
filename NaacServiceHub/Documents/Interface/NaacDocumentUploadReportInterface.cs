using PreadmissionDTOs.NAAC.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Documents.Interface
{
   public interface NaacDocumentUploadReportInterface
    {
        Task<NaacDocumentUploadReport_DTO> loaddata(NaacDocumentUploadReport_DTO data);
    }
}
