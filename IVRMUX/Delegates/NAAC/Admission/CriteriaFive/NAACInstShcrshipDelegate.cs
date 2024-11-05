using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class NAACInstShcrshipDelegate
    {
        CommonDelegate<NAACInstShcrshipDTO, NAACInstShcrshipDTO> comm = new CommonDelegate<NAACInstShcrshipDTO, NAACInstShcrshipDTO>();
        public NAACInstShcrshipDTO loaddata(NAACInstShcrshipDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACInstShcrshipFacade/loaddata");
        }
        public NAACInstShcrshipDTO save(NAACInstShcrshipDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACInstShcrshipFacade/save");
        }
        public NAACInstShcrshipDTO deactiveStudent(NAACInstShcrshipDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACInstShcrshipFacade/deactiveStudent");
        }

        public NAACInstShcrshipDTO EditData(NAACInstShcrshipDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACInstShcrshipFacade/EditData");
        }
          public NAACInstShcrshipDTO viewuploadflies(NAACInstShcrshipDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACInstShcrshipFacade/viewuploadflies");
        }
          public NAACInstShcrshipDTO deleteuploadfile(NAACInstShcrshipDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACInstShcrshipFacade/deleteuploadfile");
        }

        public NAACInstShcrshipDTO savemedicaldatawisecomments(NAACInstShcrshipDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACInstShcrshipFacade/savemedicaldatawisecomments");
        }
        public NAACInstShcrshipDTO getcomment(NAACInstShcrshipDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACInstShcrshipFacade/getcomment");
        }
        public NAACInstShcrshipDTO getfilecomment(NAACInstShcrshipDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACInstShcrshipFacade/getfilecomment");
        }
        public NAACInstShcrshipDTO savefilewisecomments(NAACInstShcrshipDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACInstShcrshipFacade/savefilewisecomments");
        }

    }
}
