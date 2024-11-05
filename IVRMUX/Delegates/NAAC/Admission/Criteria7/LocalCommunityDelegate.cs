using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission.Criteria7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission.Criteria7
{
    public class LocalCommunityDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<LocalCommunityDTO, LocalCommunityDTO> COMMM = new CommonDelegate<LocalCommunityDTO, LocalCommunityDTO>();

        public LocalCommunityDTO loaddata(LocalCommunityDTO data)
        {
            return COMMM.naacdetailsbypost(data, "LocalCommunityFacade/loaddata/");
        }
        public LocalCommunityDTO getdata(LocalCommunityDTO data)
        {
            return COMMM.naacdetailsbypost(data, "LocalCommunityFacade/getdata/");
        }
        public LocalCommunityDTO savedatatab1(LocalCommunityDTO data)
        {
            return COMMM.naacdetailsbypost(data, "LocalCommunityFacade/savedatatab1/");
        }
        public LocalCommunityDTO edittab1(LocalCommunityDTO data)
        {
            return COMMM.naacdetailsbypost(data, "LocalCommunityFacade/edittab1/");
        }
        public LocalCommunityDTO deactivYTab1(LocalCommunityDTO data)
        {
            return COMMM.naacdetailsbypost(data, "LocalCommunityFacade/deactivYTab1/");
        }

        public LocalCommunityDTO deleteuploadfile(LocalCommunityDTO data)
        {
            return COMMM.naacdetailsbypost(data, "LocalCommunityFacade/deleteuploadfile");
        }
        public LocalCommunityDTO viewuploadflies(LocalCommunityDTO data)
        {
            return COMMM.naacdetailsbypost(data, "LocalCommunityFacade/viewuploadflies");
        }
        public LocalCommunityDTO getcomment(LocalCommunityDTO data)
        {
            return COMMM.naacdetailsbypost(data, "LocalCommunityFacade/getcomment");
        }
        public LocalCommunityDTO getfilecomment(LocalCommunityDTO data)
        {
            return COMMM.naacdetailsbypost(data, "LocalCommunityFacade/getfilecomment");
        }
        public LocalCommunityDTO savecomments(LocalCommunityDTO data)
        {
            return COMMM.naacdetailsbypost(data, "LocalCommunityFacade/savecomments");
        }
        public LocalCommunityDTO savefilewisecomments(LocalCommunityDTO data)
        {
            return COMMM.naacdetailsbypost(data, "LocalCommunityFacade/savefilewisecomments");
        }
    }
}
