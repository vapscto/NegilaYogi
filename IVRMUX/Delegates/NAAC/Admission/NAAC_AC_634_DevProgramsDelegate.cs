using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class NAAC_AC_634_DevProgramsDelegate
    {
        CommonDelegate<NAAC_Criteria_6_DTO, NAAC_Criteria_6_DTO> COMMM = new CommonDelegate<NAAC_Criteria_6_DTO, NAAC_Criteria_6_DTO>();
        public NAAC_Criteria_6_DTO loaddata(NAAC_Criteria_6_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_AC_634_DevProgramsFacade/loaddata");
           
        }
        public NAAC_Criteria_6_DTO save(NAAC_Criteria_6_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_AC_634_DevProgramsFacade/save");
        }
        public NAAC_Criteria_6_DTO deactiveStudent(NAAC_Criteria_6_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_AC_634_DevProgramsFacade/deactiveStudent");
        }
        public NAAC_Criteria_6_DTO EditData(NAAC_Criteria_6_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_AC_634_DevProgramsFacade/EditData");
        }

        public NAAC_Criteria_6_DTO viewuploadflies(NAAC_Criteria_6_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_AC_634_DevProgramsFacade/viewuploadflies");
        }

        public NAAC_Criteria_6_DTO deleteuploadfile(NAAC_Criteria_6_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_AC_634_DevProgramsFacade/deleteuploadfile");
        }

        public NAAC_Criteria_6_DTO savemedicaldatawisecomments(NAAC_Criteria_6_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_AC_634_DevProgramsFacade/savemedicaldatawisecomments");
        }
        public NAAC_Criteria_6_DTO savefilewisecomments(NAAC_Criteria_6_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_AC_634_DevProgramsFacade/savefilewisecomments");
        }
        public NAAC_Criteria_6_DTO getcomment(NAAC_Criteria_6_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_AC_634_DevProgramsFacade/getcomment");
        }
        public NAAC_Criteria_6_DTO getfilecomment(NAAC_Criteria_6_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_AC_634_DevProgramsFacade/getfilecomment");
        }
    }
}
