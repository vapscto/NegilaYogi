using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class NAAC_AC_EthicDelegate
    {

        CommonDelegate<NAAC_AC_331_DTO, NAAC_AC_331_DTO> comm = new CommonDelegate<NAAC_AC_331_DTO, NAAC_AC_331_DTO>();
        public NAAC_AC_331_DTO loaddata(NAAC_AC_331_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_AC_EthicFacade/loaddata");
        }
        public NAAC_AC_331_DTO save(NAAC_AC_331_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_AC_EthicFacade/save");
        }
        public NAAC_AC_331_DTO deactive(NAAC_AC_331_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_AC_EthicFacade/deactive");
        }
        public NAAC_AC_331_DTO getcomment(NAAC_AC_331_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_AC_EthicFacade/getcomment");
        }
        public NAAC_AC_331_DTO getfilecomment(NAAC_AC_331_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_AC_EthicFacade/getfilecomment");
        }
        public NAAC_AC_331_DTO savemedicaldatawisecomments(NAAC_AC_331_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_AC_EthicFacade/savemedicaldatawisecomments");
        }
        public NAAC_AC_331_DTO savefilewisecomments(NAAC_AC_331_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_AC_EthicFacade/savefilewisecomments");
        }

        public NAAC_AC_331_DTO EditData(NAAC_AC_331_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_AC_EthicFacade/EditData");
        }
        public NAAC_AC_331_DTO viewuploadflies(NAAC_AC_331_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_AC_EthicFacade/viewuploadflies");
        }
        public NAAC_AC_331_DTO deleteuploadfile(NAAC_AC_331_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_AC_EthicFacade/deleteuploadfile");
        }
    }
}
