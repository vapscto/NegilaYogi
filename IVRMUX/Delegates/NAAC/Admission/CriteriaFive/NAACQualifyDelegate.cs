using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class NAACQualifyDelegate
    {
        CommonDelegate<NAACQualifyDTO, NAACQualifyDTO> comm = new CommonDelegate<NAACQualifyDTO, NAACQualifyDTO>();
        public NAACQualifyDTO loaddata(NAACQualifyDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACQualifyFacade/loaddata");
        }

        public NAACQualifyDTO save1(NAACQualifyDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACQualifyFacade/save1");
        }
        public NAACQualifyDTO deactiveStudent1(NAACQualifyDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACQualifyFacade/deactiveStudent1");
        }

        public NAACQualifyDTO EditData1(NAACQualifyDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACQualifyFacade/EditData1");
        }
        public NAACQualifyDTO save(NAACQualifyDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACQualifyFacade/save");
        }
        public NAACQualifyDTO deactiveStudent(NAACQualifyDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACQualifyFacade/deactiveStudent");
        }

        public NAACQualifyDTO EditData(NAACQualifyDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACQualifyFacade/EditData");
        }
          public NAACQualifyDTO viewuploadflies(NAACQualifyDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACQualifyFacade/viewuploadflies");
        }
          public NAACQualifyDTO deleteuploadfile(NAACQualifyDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACQualifyFacade/deleteuploadfile");
        }


        public NAACQualifyDTO savemedicaldatawisecomments(NAACQualifyDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACQualifyFacade/savemedicaldatawisecomments");
        }
        public NAACQualifyDTO getcomment(NAACQualifyDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACQualifyFacade/getcomment");
        }
        public NAACQualifyDTO getfilecomment(NAACQualifyDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACQualifyFacade/getfilecomment");
        }
        public NAACQualifyDTO savefilewisecomments(NAACQualifyDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACQualifyFacade/savefilewisecomments");
        }
    }
}
