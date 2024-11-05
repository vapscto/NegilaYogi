using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class NaacLinkageDelegate
    {
        CommonDelegate<NAAC_AC_351_Linkage_DTO, NAAC_AC_351_Linkage_DTO> comm = new CommonDelegate<NAAC_AC_351_Linkage_DTO, NAAC_AC_351_Linkage_DTO>();

        public NAAC_AC_351_Linkage_DTO loaddata(NAAC_AC_351_Linkage_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacLinkageFacade/loaddata");
        }

      
        public NAAC_AC_351_Linkage_DTO save(NAAC_AC_351_Linkage_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacLinkageFacade/save");
        }
        public NAAC_AC_351_Linkage_DTO EditData(NAAC_AC_351_Linkage_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacLinkageFacade/EditData");
        }
        public NAAC_AC_351_Linkage_DTO getcomment(NAAC_AC_351_Linkage_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacLinkageFacade/getcomment");
        }
        public NAAC_AC_351_Linkage_DTO savemedicaldatawisecomments(NAAC_AC_351_Linkage_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacLinkageFacade/savemedicaldatawisecomments");
        }
        public NAAC_AC_351_Linkage_DTO savefilewisecomments(NAAC_AC_351_Linkage_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacLinkageFacade/savefilewisecomments");
        }
        public NAAC_AC_351_Linkage_DTO getfilecomment(NAAC_AC_351_Linkage_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacLinkageFacade/getfilecomment");
        }
        public NAAC_AC_351_Linkage_DTO deactive(NAAC_AC_351_Linkage_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacLinkageFacade/deactive");
        }
        public NAAC_AC_351_Linkage_DTO viewuploadflies(NAAC_AC_351_Linkage_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacLinkageFacade/viewuploadflies");
        }
        public NAAC_AC_351_Linkage_DTO deleteuploadfile(NAAC_AC_351_Linkage_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacLinkageFacade/deleteuploadfile");
        }
    }
}
