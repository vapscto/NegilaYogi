using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class NAACHrEducationDelegate
    {
        CommonDelegate<NAACHrEducationDTO, NAACHrEducationDTO> comm = new CommonDelegate<NAACHrEducationDTO, NAACHrEducationDTO>();
        public NAACHrEducationDTO loaddata(NAACHrEducationDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACHrEducationFacade/loaddata");
        }
        public NAACHrEducationDTO save(NAACHrEducationDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACHrEducationFacade/save");
        }
        public NAACHrEducationDTO deactiveStudent(NAACHrEducationDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACHrEducationFacade/deactiveStudent");
        }

        public NAACHrEducationDTO EditData(NAACHrEducationDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACHrEducationFacade/EditData");
        }
        public NAACHrEducationDTO viewuploadflies(NAACHrEducationDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACHrEducationFacade/viewuploadflies");
        }
        public NAACHrEducationDTO deleteuploadfile(NAACHrEducationDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACHrEducationFacade/deleteuploadfile");
        }
        public NAACHrEducationDTO get_course(NAACHrEducationDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACHrEducationFacade/get_course");
        }
         public NAACHrEducationDTO get_branch(NAACHrEducationDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACHrEducationFacade/get_branch");
        }

        public NAACHrEducationDTO savemedicaldatawisecomments(NAACHrEducationDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACHrEducationFacade/savemedicaldatawisecomments");
        }
        public NAACHrEducationDTO getcomment(NAACHrEducationDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACHrEducationFacade/getcomment");
        }
        public NAACHrEducationDTO getfilecomment(NAACHrEducationDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACHrEducationFacade/getfilecomment");
        }
        public NAACHrEducationDTO savefilewisecomments(NAACHrEducationDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACHrEducationFacade/savefilewisecomments");
        }

    }
}
