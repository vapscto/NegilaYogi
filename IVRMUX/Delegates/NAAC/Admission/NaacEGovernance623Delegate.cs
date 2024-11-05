using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class NaacEGovernance623Delegate
    {
        CommonDelegate<NAAC_AC_623_EGovernance_DTO, NAAC_AC_623_EGovernance_DTO> COMMM = new CommonDelegate<NAAC_AC_623_EGovernance_DTO, NAAC_AC_623_EGovernance_DTO>();

        public NAAC_AC_623_EGovernance_DTO loaddata(NAAC_AC_623_EGovernance_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacEGovernance623Facade/loaddata");
        }
        public NAAC_AC_623_EGovernance_DTO save(NAAC_AC_623_EGovernance_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacEGovernance623Facade/save");
        }
        public NAAC_AC_623_EGovernance_DTO deactive(NAAC_AC_623_EGovernance_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacEGovernance623Facade/deactive");
        }
        public NAAC_AC_623_EGovernance_DTO EditData(NAAC_AC_623_EGovernance_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacEGovernance623Facade/EditData");
        }


        public NAAC_AC_623_EGovernance_DTO viewuploadflies(NAAC_AC_623_EGovernance_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacEGovernance623Facade/viewuploadflies");
        }
        public NAAC_AC_623_EGovernance_DTO deleteuploadfile(NAAC_AC_623_EGovernance_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacEGovernance623Facade/deleteuploadfile");
        }



        public NAAC_AC_623_EGovernance_DTO savemedicaldatawisecomments(NAAC_AC_623_EGovernance_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacEGovernance623Facade/savemedicaldatawisecomments");
        }
        public NAAC_AC_623_EGovernance_DTO savefilewisecomments(NAAC_AC_623_EGovernance_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacEGovernance623Facade/savefilewisecomments");
        }
        public NAAC_AC_623_EGovernance_DTO getcomment(NAAC_AC_623_EGovernance_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacEGovernance623Facade/getcomment");
        }
        public NAAC_AC_623_EGovernance_DTO getfilecomment(NAAC_AC_623_EGovernance_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacEGovernance623Facade/getfilecomment");
        }

    }
}
