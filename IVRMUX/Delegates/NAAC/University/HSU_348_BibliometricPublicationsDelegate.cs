using CommonLibrary;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.University
{
    public class HSU_348_BibliometricPublicationsDelegate
    {
        CommonDelegate<HSU_346_EMPApprovedJournalList_DTO, HSU_346_EMPApprovedJournalList_DTO> comm = new CommonDelegate<HSU_346_EMPApprovedJournalList_DTO, HSU_346_EMPApprovedJournalList_DTO>();

        public HSU_346_EMPApprovedJournalList_DTO loaddata(HSU_346_EMPApprovedJournalList_DTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_348_BibliometricPublicationsFacade/loaddata");
        }
        public HSU_346_EMPApprovedJournalList_DTO save(HSU_346_EMPApprovedJournalList_DTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_348_BibliometricPublicationsFacade/save");
        }
        public HSU_346_EMPApprovedJournalList_DTO deactive(HSU_346_EMPApprovedJournalList_DTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_348_BibliometricPublicationsFacade/deactive");
        }
        public HSU_346_EMPApprovedJournalList_DTO EditData(HSU_346_EMPApprovedJournalList_DTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_348_BibliometricPublicationsFacade/EditData");
        }
        public HSU_346_EMPApprovedJournalList_DTO viewuploadflies(HSU_346_EMPApprovedJournalList_DTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_348_BibliometricPublicationsFacade/viewuploadflies");
        }
        public HSU_346_EMPApprovedJournalList_DTO deleteuploadfile(HSU_346_EMPApprovedJournalList_DTO data)
        {
            return comm.naacdetailsbypost(data, "HSU_348_BibliometricPublicationsFacade/deleteuploadfile");
        }
    }
}
