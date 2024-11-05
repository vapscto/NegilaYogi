using PreadmissionDTOs.NAAC.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Documents.Interface
{
    public interface NaacDocumentUploadInterface
    {
        NaacDocumentUploadDTO onload(NaacDocumentUploadDTO data);
        NaacDocumentUploadDTO save(NaacDocumentUploadDTO data);
        NaacDocumentUploadDTO saveapproved(NaacDocumentUploadDTO data);
        NaacDocumentUploadDTO getuploaddoc(NaacDocumentUploadDTO data);
        NaacDocumentUploadDTO getcomments(NaacDocumentUploadDTO data);
        NaacDocumentUploadDTO savecomments(NaacDocumentUploadDTO data);
        NaacDocumentUploadDTO viewcomments(NaacDocumentUploadDTO data);
        NaacDocumentUploadDTO savegeneralcommetns(NaacDocumentUploadDTO data);
        NaacDocumentUploadDTO savecommentscons(NaacDocumentUploadDTO data);
        NaacDocumentUploadDTO savehyperlinks(NaacDocumentUploadDTO data);
        NaacDocumentUploadDTO viewaddedhyperlink(NaacDocumentUploadDTO data);
        NaacDocumentUploadDTO deletehyperlink(NaacDocumentUploadDTO data);
        NaacDocumentUploadDTO deleteuploadfile(NaacDocumentUploadDTO data);
        NaacDocumentUploadDTO saveCGPA(NaacDocumentUploadDTO data);

    }
}
