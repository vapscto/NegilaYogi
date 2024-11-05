using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class NAACSportsDelegate
    {
        CommonDelegate<NAACSportsDTO, NAACSportsDTO> comm = new CommonDelegate<NAACSportsDTO, NAACSportsDTO>();
        public NAACSportsDTO loaddata(NAACSportsDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACSportsFacade/loaddata");
        }

      
        public NAACSportsDTO save(NAACSportsDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACSportsFacade/save");
        }
        public NAACSportsDTO deactiveStudent(NAACSportsDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACSportsFacade/deactiveStudent");
        }

        public NAACSportsDTO EditData(NAACSportsDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACSportsFacade/EditData");
        }
          public NAACSportsDTO viewuploadflies(NAACSportsDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACSportsFacade/viewuploadflies");
        }
          public NAACSportsDTO deleteuploadfile(NAACSportsDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACSportsFacade/deleteuploadfile");
        }

        

      public NAACSportsDTO get_course(NAACSportsDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACSportsFacade/get_course");
        }
        public NAACSportsDTO get_branch(NAACSportsDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACSportsFacade/get_branch");
        }
        public NAACSportsDTO get_sems(NAACSportsDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACSportsFacade/get_sems");
        }
        public NAACSportsDTO get_section(NAACSportsDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACSportsFacade/get_section");
        }
        public NAACSportsDTO GetStudentDetails(NAACSportsDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACSportsFacade/GetStudentDetails");
        }


        public NAACSportsDTO savemedicaldatawisecomments(NAACSportsDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACSportsFacade/savemedicaldatawisecomments");
        }
        public NAACSportsDTO getcomment(NAACSportsDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACSportsFacade/getcomment");
        }
        public NAACSportsDTO getfilecomment(NAACSportsDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACSportsFacade/getfilecomment");
        }
        public NAACSportsDTO savefilewisecomments(NAACSportsDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACSportsFacade/savefilewisecomments");
        }
    }
}
