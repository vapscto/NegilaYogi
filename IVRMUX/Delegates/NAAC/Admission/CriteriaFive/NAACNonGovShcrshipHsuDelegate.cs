using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class NAACNonGovShcrshipHsuDelegate
    {
        CommonDelegate<NAACNonGovShcrshipHsuDTO, NAACNonGovShcrshipHsuDTO> comm = new CommonDelegate<NAACNonGovShcrshipHsuDTO, NAACNonGovShcrshipHsuDTO>();
        public NAACNonGovShcrshipHsuDTO loaddata(NAACNonGovShcrshipHsuDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACNonGovShcrshipHsuFacade/loaddata");
        }
        public NAACNonGovShcrshipHsuDTO save(NAACNonGovShcrshipHsuDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACNonGovShcrshipHsuFacade/save");
        }
        public NAACNonGovShcrshipHsuDTO deactiveStudent(NAACNonGovShcrshipHsuDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACNonGovShcrshipHsuFacade/deactiveStudent");
        }

        public NAACNonGovShcrshipHsuDTO EditData(NAACNonGovShcrshipHsuDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACNonGovShcrshipHsuFacade/EditData");
        }
          public NAACNonGovShcrshipHsuDTO viewuploadflies(NAACNonGovShcrshipHsuDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACNonGovShcrshipHsuFacade/viewuploadflies");
        }
          public NAACNonGovShcrshipHsuDTO deleteuploadfile(NAACNonGovShcrshipHsuDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACNonGovShcrshipHsuFacade/deleteuploadfile");
        }


        public NAACNonGovShcrshipHsuDTO savemedicaldatawisecomments(NAACNonGovShcrshipHsuDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACNonGovShcrshipHsuFacade/savemedicaldatawisecomments");
        }
        public NAACNonGovShcrshipHsuDTO getcomment(NAACNonGovShcrshipHsuDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACNonGovShcrshipHsuFacade/getcomment");
        }
        public NAACNonGovShcrshipHsuDTO getfilecomment(NAACNonGovShcrshipHsuDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACNonGovShcrshipHsuFacade/getfilecomment");
        }
        public NAACNonGovShcrshipHsuDTO savefilewisecomments(NAACNonGovShcrshipHsuDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACNonGovShcrshipHsuFacade/savefilewisecomments");
        }
    }
}
