using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class NAACAlumniMeetingDelegate
    {
        CommonDelegate<NAACAlumniMeetingDTO, NAACAlumniMeetingDTO> comm = new CommonDelegate<NAACAlumniMeetingDTO, NAACAlumniMeetingDTO>();
        public NAACAlumniMeetingDTO loaddata(NAACAlumniMeetingDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACAlumniMeetingFacade/loaddata");
        }
        public NAACAlumniMeetingDTO save(NAACAlumniMeetingDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACAlumniMeetingFacade/save");
        }
        public NAACAlumniMeetingDTO deactiveStudent(NAACAlumniMeetingDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACAlumniMeetingFacade/deactiveStudent");
        }

        public NAACAlumniMeetingDTO EditData(NAACAlumniMeetingDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACAlumniMeetingFacade/EditData");
        }
        public NAACAlumniMeetingDTO viewuploadflies(NAACAlumniMeetingDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACAlumniMeetingFacade/viewuploadflies");
        }
        public NAACAlumniMeetingDTO deleteuploadfile(NAACAlumniMeetingDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACAlumniMeetingFacade/deleteuploadfile");
        }
        public NAACAlumniMeetingDTO savemedicaldatawisecomments(NAACAlumniMeetingDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACAlumniMeetingFacade/savemedicaldatawisecomments");
        }
        public NAACAlumniMeetingDTO getcomment(NAACAlumniMeetingDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACAlumniMeetingFacade/getcomment");
        }
        public NAACAlumniMeetingDTO getfilecomment(NAACAlumniMeetingDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACAlumniMeetingFacade/getfilecomment");
        }
        public NAACAlumniMeetingDTO savefilewisecomments(NAACAlumniMeetingDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACAlumniMeetingFacade/savefilewisecomments");
        }

    }
}
