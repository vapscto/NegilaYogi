using CommonLibrary;
using PreadmissionDTOs.NAAC.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Documents
{
    public class NaacDocumentUploadDelegate
    {
        CommonDelegate<NaacDocumentUploadDTO, NaacDocumentUploadDTO> comm = new CommonDelegate<NaacDocumentUploadDTO, NaacDocumentUploadDTO>();

        public NaacDocumentUploadDTO onload(NaacDocumentUploadDTO data)
        {
            return comm.naacdetailsbypost(data, "NaacDocumentUploadFacade/onload");
        }
        public NaacDocumentUploadDTO save(NaacDocumentUploadDTO data)
        {
            return comm.naacdetailsbypost(data, "NaacDocumentUploadFacade/save");
        }
        public NaacDocumentUploadDTO saveapproved(NaacDocumentUploadDTO data)
        {
            return comm.naacdetailsbypost(data, "NaacDocumentUploadFacade/saveapproved");
        }

        // *** Get Upload Documents 
        public NaacDocumentUploadDTO getuploaddoc(NaacDocumentUploadDTO data)
        {
            return comm.naacdetailsbypost(data, "NaacDocumentUploadFacade/getuploaddoc");
        }

        // *** Get Comments  
        public NaacDocumentUploadDTO getcomments(NaacDocumentUploadDTO data)
        {
            return comm.naacdetailsbypost(data, "NaacDocumentUploadFacade/getcomments");
        }
        public NaacDocumentUploadDTO savecomments(NaacDocumentUploadDTO data)
        {
            return comm.naacdetailsbypost(data, "NaacDocumentUploadFacade/savecomments");
        }
        public NaacDocumentUploadDTO viewcomments(NaacDocumentUploadDTO data)
        {
            return comm.naacdetailsbypost(data, "NaacDocumentUploadFacade/viewcomments");
        }
        public NaacDocumentUploadDTO savegeneralcommetns(NaacDocumentUploadDTO data)
        {
            return comm.naacdetailsbypost(data, "NaacDocumentUploadFacade/savegeneralcommetns");
        }
        public NaacDocumentUploadDTO savecommentscons(NaacDocumentUploadDTO data)
        {
            return comm.naacdetailsbypost(data, "NaacDocumentUploadFacade/savecommentscons");
        }
        public NaacDocumentUploadDTO savehyperlinks(NaacDocumentUploadDTO data)
        {
            return comm.naacdetailsbypost(data, "NaacDocumentUploadFacade/savehyperlinks");
        }
        public NaacDocumentUploadDTO viewaddedhyperlink(NaacDocumentUploadDTO data)
        {
            return comm.naacdetailsbypost(data, "NaacDocumentUploadFacade/viewaddedhyperlink");
        }
        public NaacDocumentUploadDTO deletehyperlink(NaacDocumentUploadDTO data)
        {
            return comm.naacdetailsbypost(data, "NaacDocumentUploadFacade/deletehyperlink");
        }
        public NaacDocumentUploadDTO deleteuploadfile(NaacDocumentUploadDTO data)
        {
            return comm.naacdetailsbypost(data, "NaacDocumentUploadFacade/deleteuploadfile");
        }
        public NaacDocumentUploadDTO saveCGPA(NaacDocumentUploadDTO data)
        {
            return comm.naacdetailsbypost(data, "NaacDocumentUploadFacade/saveCGPA");
        }

    }
}
