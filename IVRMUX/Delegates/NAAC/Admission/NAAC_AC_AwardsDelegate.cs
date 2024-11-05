using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class NAAC_AC_AwardsDelegate
    {
        CommonDelegate<NAAC_AC_Awards_342_DTO, NAAC_AC_Awards_342_DTO> comm = new CommonDelegate<NAAC_AC_Awards_342_DTO, NAAC_AC_Awards_342_DTO>();
        public NAAC_AC_Awards_342_DTO loaddata(NAAC_AC_Awards_342_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_AC_AwardsFacade/loaddata");
           
        }
        public NAAC_AC_Awards_342_DTO save(NAAC_AC_Awards_342_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_AC_AwardsFacade/save");
        }
        public NAAC_AC_Awards_342_DTO deactive(NAAC_AC_Awards_342_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_AC_AwardsFacade/deactive");
        }
        public NAAC_AC_Awards_342_DTO EditData(NAAC_AC_Awards_342_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_AC_AwardsFacade/EditData");
        }
        public NAAC_AC_Awards_342_DTO savefilewisecomments(NAAC_AC_Awards_342_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_AC_AwardsFacade/savefilewisecomments");
        }
        public NAAC_AC_Awards_342_DTO getcomment(NAAC_AC_Awards_342_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_AC_AwardsFacade/getcomment");
        }
        public NAAC_AC_Awards_342_DTO savemedicaldatawisecomments(NAAC_AC_Awards_342_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_AC_AwardsFacade/savemedicaldatawisecomments");
        }
        public NAAC_AC_Awards_342_DTO getfilecomment(NAAC_AC_Awards_342_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_AC_AwardsFacade/getfilecomment");
        }
        public NAAC_AC_Awards_342_DTO viewuploadflies(NAAC_AC_Awards_342_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_AC_AwardsFacade/viewuploadflies");
        }
        public NAAC_AC_Awards_342_DTO deleteuploadfile(NAAC_AC_Awards_342_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_AC_AwardsFacade/deleteuploadfile");
        }
    }
}
