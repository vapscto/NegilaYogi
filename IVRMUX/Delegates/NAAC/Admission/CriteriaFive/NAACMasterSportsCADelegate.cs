using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class NAACMasterSportsCADelegate
    {
        CommonDelegate<NAACMasterSportsCADTO, NAACMasterSportsCADTO> comm = new CommonDelegate<NAACMasterSportsCADTO, NAACMasterSportsCADTO>();
        public NAACMasterSportsCADTO loaddata(NAACMasterSportsCADTO data)
        {
            return comm.naacdetailsbypost(data, "NAACMasterSportsCAFacade/loaddata");
        }
        public NAACMasterSportsCADTO save(NAACMasterSportsCADTO data)
        {
            return comm.naacdetailsbypost(data, "NAACMasterSportsCAFacade/save");
        }
        public NAACMasterSportsCADTO deactiveStudent(NAACMasterSportsCADTO data)
        {
            return comm.naacdetailsbypost(data, "NAACMasterSportsCAFacade/deactiveStudent");
        }

        public NAACMasterSportsCADTO EditData(NAACMasterSportsCADTO data)
        {
            return comm.naacdetailsbypost(data, "NAACMasterSportsCAFacade/EditData");
        }
        public NAACMasterSportsCADTO viewuploadflies(NAACMasterSportsCADTO data)
        {
            return comm.naacdetailsbypost(data, "NAACMasterSportsCAFacade/viewuploadflies");
        }
        public NAACMasterSportsCADTO deleteuploadfile(NAACMasterSportsCADTO data)
        {
            return comm.naacdetailsbypost(data, "NAACMasterSportsCAFacade/deleteuploadfile");
        }
        public NAACMasterSportsCADTO savemedicaldatawisecomments(NAACMasterSportsCADTO data)
        {
            return comm.naacdetailsbypost(data, "NAACMasterSportsCAFacade/savemedicaldatawisecomments");
        }
        public NAACMasterSportsCADTO getcomment(NAACMasterSportsCADTO data)
        {
            return comm.naacdetailsbypost(data, "NAACMasterSportsCAFacade/getcomment");
        }
        public NAACMasterSportsCADTO getfilecomment(NAACMasterSportsCADTO data)
        {
            return comm.naacdetailsbypost(data, "NAACMasterSportsCAFacade/getfilecomment");
        }
        public NAACMasterSportsCADTO savefilewisecomments(NAACMasterSportsCADTO data)
        {
            return comm.naacdetailsbypost(data, "NAACMasterSportsCAFacade/savefilewisecomments");
        }

    }
}
