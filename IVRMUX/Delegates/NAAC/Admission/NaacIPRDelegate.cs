using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class NaacIPRDelegate
    {
        CommonDelegate<NAAC_AC_IPR_322_DTO, NAAC_AC_IPR_322_DTO> comm = new CommonDelegate<NAAC_AC_IPR_322_DTO, NAAC_AC_IPR_322_DTO>();
        public NAAC_AC_IPR_322_DTO loaddata(NAAC_AC_IPR_322_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacIPRFacade/loaddata");
        }
        public NAAC_AC_IPR_322_DTO save(NAAC_AC_IPR_322_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacIPRFacade/save");
        }
        public NAAC_AC_IPR_322_DTO deactive(NAAC_AC_IPR_322_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacIPRFacade/deactive");
        }
        public NAAC_AC_IPR_322_DTO EditData(NAAC_AC_IPR_322_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacIPRFacade/EditData");
        }
        public NAAC_AC_IPR_322_DTO getfilecomment(NAAC_AC_IPR_322_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacIPRFacade/getfilecomment");
        }
        public NAAC_AC_IPR_322_DTO savefilewisecomments(NAAC_AC_IPR_322_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacIPRFacade/savefilewisecomments");
        }
        
        public NAAC_AC_IPR_322_DTO getcomment(NAAC_AC_IPR_322_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacIPRFacade/getcomment");
        }
        
        public NAAC_AC_IPR_322_DTO savemedicaldatawisecomments(NAAC_AC_IPR_322_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacIPRFacade/savemedicaldatawisecomments");
        }

        public NAAC_AC_IPR_322_DTO viewuploadflies(NAAC_AC_IPR_322_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacIPRFacade/viewuploadflies");
        }
        public NAAC_AC_IPR_322_DTO deleteuploadfile(NAAC_AC_IPR_322_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacIPRFacade/deleteuploadfile");
        }

    }
}
