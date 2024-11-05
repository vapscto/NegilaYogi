using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.HRMS
{
    public class NAACACCommitteeMemberDelegates
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<NAACACCommitteeMembersDTO, NAACACCommitteeMembersDTO> COMMM = new CommonDelegate<NAACACCommitteeMembersDTO, NAACACCommitteeMembersDTO>();

        public NAACACCommitteeMembersDTO onloadgetdetails(NAACACCommitteeMembersDTO dto)
        {
            return COMMM.naacdetailsbypost(dto, "NAACACCommitteememberFacade/onloadgetdetails");
        }
        public NAACACCommitteeMembersDTO savedetails(NAACACCommitteeMembersDTO maspage)
        {
            return COMMM.naacdetailsbypost(maspage, "NAACACCommitteememberFacade/");
        }
        public NAACACCommitteeMembersDTO getRecorddetailsById(int id)
        {
            return COMMM.naacdetailsbyid(id, "NAACACCommitteememberFacade/getRecordById/");
        }
        public NAACACCommitteeMembersDTO deleterec(NAACACCommitteeMembersDTO maspage)
        {
            return COMMM.naacdetailsbypost(maspage, "NAACACCommitteememberFacade/deactivateRecordById/");
        }
    }
}
