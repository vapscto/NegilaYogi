using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class NAACPlacementDelegate
    {
        CommonDelegate<NAACPlacementDTO, NAACPlacementDTO> comm = new CommonDelegate<NAACPlacementDTO, NAACPlacementDTO>();
        public NAACPlacementDTO loaddata(NAACPlacementDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACPlacementFacade/loaddata");
        }
        public NAACPlacementDTO save(NAACPlacementDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACPlacementFacade/save");
        }
        public NAACPlacementDTO deactiveStudent(NAACPlacementDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACPlacementFacade/deactiveStudent");
        }

        public NAACPlacementDTO EditData(NAACPlacementDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACPlacementFacade/EditData");
        }
        public NAACPlacementDTO viewuploadflies(NAACPlacementDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACPlacementFacade/viewuploadflies");
        }
        public NAACPlacementDTO deleteuploadfile(NAACPlacementDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACPlacementFacade/deleteuploadfile");
        }
        public NAACPlacementDTO get_course(NAACPlacementDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACPlacementFacade/get_course");
        }
         public NAACPlacementDTO get_branch(NAACPlacementDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACPlacementFacade/get_branch");
        }

        public NAACPlacementDTO savemedicaldatawisecomments(NAACPlacementDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACPlacementFacade/savemedicaldatawisecomments");
        }
        public NAACPlacementDTO getcomment(NAACPlacementDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACPlacementFacade/getcomment");
        }
        public NAACPlacementDTO getfilecomment(NAACPlacementDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACPlacementFacade/getfilecomment");
        }
        public NAACPlacementDTO savefilewisecomments(NAACPlacementDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACPlacementFacade/savefilewisecomments");
        }
    }
}
