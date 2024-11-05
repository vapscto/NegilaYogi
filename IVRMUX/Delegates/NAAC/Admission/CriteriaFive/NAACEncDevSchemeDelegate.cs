using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class NAACEncDevSchemeDelegate
    {
        CommonDelegate<NAACEncDevSchemeDTO, NAACEncDevSchemeDTO> comm = new CommonDelegate<NAACEncDevSchemeDTO, NAACEncDevSchemeDTO>();
        public NAACEncDevSchemeDTO loaddata(NAACEncDevSchemeDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACEncDevSchemeFacade/loaddata");
        }
        public NAACEncDevSchemeDTO save(NAACEncDevSchemeDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACEncDevSchemeFacade/save");
        }
        public NAACEncDevSchemeDTO deactiveStudent(NAACEncDevSchemeDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACEncDevSchemeFacade/deactiveStudent");
        }

        public NAACEncDevSchemeDTO EditData(NAACEncDevSchemeDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACEncDevSchemeFacade/EditData");
        }
        public NAACEncDevSchemeDTO viewuploadflies(NAACEncDevSchemeDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACEncDevSchemeFacade/viewuploadflies");
        }
        public NAACEncDevSchemeDTO deleteuploadfile(NAACEncDevSchemeDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACEncDevSchemeFacade/deleteuploadfile");
        }

        

        public NAACEncDevSchemeDTO savemedicaldatawisecomments(NAACEncDevSchemeDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACEncDevSchemeFacade/savemedicaldatawisecomments");
        }
        public NAACEncDevSchemeDTO getcomment(NAACEncDevSchemeDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACEncDevSchemeFacade/getcomment");
        }
        public NAACEncDevSchemeDTO getfilecomment(NAACEncDevSchemeDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACEncDevSchemeFacade/getfilecomment");
        }
        public NAACEncDevSchemeDTO savefilewisecomments(NAACEncDevSchemeDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACEncDevSchemeFacade/savefilewisecomments");
        }
    }
}
