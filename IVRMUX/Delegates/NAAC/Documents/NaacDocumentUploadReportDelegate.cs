using CommonLibrary;
using PreadmissionDTOs.NAAC.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Documents
{
    public class NaacDocumentUploadReportDelegate
    {
        CommonDelegate<NaacDocumentUploadReport_DTO, NaacDocumentUploadReport_DTO> comm = new CommonDelegate<NaacDocumentUploadReport_DTO, NaacDocumentUploadReport_DTO>();

        public NaacDocumentUploadReport_DTO loaddata(NaacDocumentUploadReport_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacDocumentUploadReportFacade/onload");
        }

    }
}
