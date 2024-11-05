using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class NAACCompExamsDelegate
    {
        CommonDelegate<NAACCompExamsDTO, NAACCompExamsDTO> comm = new CommonDelegate<NAACCompExamsDTO, NAACCompExamsDTO>();
        public NAACCompExamsDTO loaddata(NAACCompExamsDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACCompExamsFacade/loaddata");
        }
        public NAACCompExamsDTO save(NAACCompExamsDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACCompExamsFacade/save");
        }
        public NAACCompExamsDTO deactiveStudent(NAACCompExamsDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACCompExamsFacade/deactiveStudent");
        }

        public NAACCompExamsDTO EditData(NAACCompExamsDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACCompExamsFacade/EditData");
        }
        public NAACCompExamsDTO viewuploadflies(NAACCompExamsDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACCompExamsFacade/viewuploadflies");
        }
        public NAACCompExamsDTO deleteuploadfile(NAACCompExamsDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACCompExamsFacade/deleteuploadfile");
        }

        public NAACCompExamsDTO savemedicaldatawisecomments(NAACCompExamsDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACCompExamsFacade/savemedicaldatawisecomments");
        }
        public NAACCompExamsDTO getcomment(NAACCompExamsDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACCompExamsFacade/getcomment");
        }
        public NAACCompExamsDTO getfilecomment(NAACCompExamsDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACCompExamsFacade/getfilecomment");
        }
        public NAACCompExamsDTO savefilewisecomments(NAACCompExamsDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACCompExamsFacade/savefilewisecomments");
        }

    }
}
