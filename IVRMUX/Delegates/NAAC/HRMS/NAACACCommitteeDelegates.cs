using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.HRMS
{
    public class NAACACCommitteeDelegates
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<NAACACCommitteeDTO, NAACACCommitteeDTO> COMMM = new CommonDelegate<NAACACCommitteeDTO, NAACACCommitteeDTO>();

        public NAACACCommitteeDTO onloadgetdetails(NAACACCommitteeDTO dto)
        {
            return COMMM.naacdetailsbypost(dto, "NAACACCommitteeFacade/onloadgetdetails");
        }
        public NAACACCommitteeDTO savedetails(NAACACCommitteeDTO maspage)
        {
            return COMMM.naacdetailsbypost(maspage, "NAACACCommitteeFacade/");
        }
        public NAACACCommitteeDTO getRecorddetailsById(int id)
        {
            return COMMM.naacdetailsbyid(id, "NAACACCommitteeFacade/getRecordById/");
        }
        public NAACACCommitteeDTO deleterec(NAACACCommitteeDTO maspage)
        {
            return COMMM.naacdetailsbypost(maspage, "NAACACCommitteeFacade/deactivateRecordById/");
        }
    }
}
