using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class NAACGovtShcrShipDelegate
    {
        CommonDelegate<NAACGovtShcrShipDTO, NAACGovtShcrShipDTO> comm = new CommonDelegate<NAACGovtShcrShipDTO, NAACGovtShcrShipDTO>();
        public NAACGovtShcrShipDTO loaddata(NAACGovtShcrShipDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACGovtShcrShipFacade/loaddata");
        }
        public NAACGovtShcrShipDTO save(NAACGovtShcrShipDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACGovtShcrShipFacade/save");
        }
        public NAACGovtShcrShipDTO deactiveStudent(NAACGovtShcrShipDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACGovtShcrShipFacade/deactiveStudent");
        }

        public NAACGovtShcrShipDTO EditData(NAACGovtShcrShipDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACGovtShcrShipFacade/EditData");
        }
        public NAACGovtShcrShipDTO viewuploadflies(NAACGovtShcrShipDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACGovtShcrShipFacade/viewuploadflies");
        }
          public NAACGovtShcrShipDTO deleteuploadfile(NAACGovtShcrShipDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACGovtShcrShipFacade/deleteuploadfile");
        }
        public NAACGovtShcrShipDTO savemedicaldatawisecomments(NAACGovtShcrShipDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACGovtShcrShipFacade/savemedicaldatawisecomments");
        }
        public NAACGovtShcrShipDTO getcomment(NAACGovtShcrShipDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACGovtShcrShipFacade/getcomment");
        }
        public NAACGovtShcrShipDTO getfilecomment(NAACGovtShcrShipDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACGovtShcrShipFacade/getfilecomment");
        }
        public NAACGovtShcrShipDTO savefilewisecomments(NAACGovtShcrShipDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACGovtShcrShipFacade/savefilewisecomments");
        }


    }
}
