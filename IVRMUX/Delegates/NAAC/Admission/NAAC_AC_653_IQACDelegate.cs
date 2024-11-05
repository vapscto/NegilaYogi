using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class NAAC_AC_653_IQACDelegate
    {
        CommonDelegate<NAAC_Criteria_6_DTO, NAAC_Criteria_6_DTO> COMMM = new CommonDelegate<NAAC_Criteria_6_DTO, NAAC_Criteria_6_DTO>();
        public NAAC_Criteria_6_DTO loaddata(NAAC_Criteria_6_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_AC_653_IQACFacade/loaddata");

        }
        public NAAC_Criteria_6_DTO save(NAAC_Criteria_6_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_AC_653_IQACFacade/save");
        }
        public NAAC_Criteria_6_DTO deactiveStudent(NAAC_Criteria_6_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_AC_653_IQACFacade/deactiveStudent");
        }
        public NAAC_Criteria_6_DTO EditData(NAAC_Criteria_6_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_AC_653_IQACFacade/EditData");
        }
        public NAAC_Criteria_6_DTO viewuploadflies(NAAC_Criteria_6_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_AC_653_IQACFacade/viewuploadflies");
        }

        public NAAC_Criteria_6_DTO deleteuploadfile(NAAC_Criteria_6_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_AC_653_IQACFacade/deleteuploadfile");
        }


        public NAAC_Criteria_6_DTO savemedicaldatawisecomments(NAAC_Criteria_6_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_AC_653_IQACFacade/savemedicaldatawisecomments");
        }
        public NAAC_Criteria_6_DTO savefilewisecomments(NAAC_Criteria_6_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_AC_653_IQACFacade/savefilewisecomments");
        }
        public NAAC_Criteria_6_DTO getcomment(NAAC_Criteria_6_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_AC_653_IQACFacade/getcomment");
        }
        public NAAC_Criteria_6_DTO getfilecomment(NAAC_Criteria_6_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_AC_653_IQACFacade/getfilecomment");
        }

    }
}
