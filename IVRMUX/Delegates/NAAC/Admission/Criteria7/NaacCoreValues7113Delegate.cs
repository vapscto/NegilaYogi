using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission.Criteria7
{
    public class NaacCoreValues7113Delegate
    {
        CommonDelegate<NAAC_AC_7113_CoreValues_DTO, NAAC_AC_7113_CoreValues_DTO> comm = new CommonDelegate<NAAC_AC_7113_CoreValues_DTO, NAAC_AC_7113_CoreValues_DTO>();


        public NAAC_AC_7113_CoreValues_DTO loaddata(NAAC_AC_7113_CoreValues_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCoreValues7113Facade/loaddata");
        }
        public NAAC_AC_7113_CoreValues_DTO save(NAAC_AC_7113_CoreValues_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCoreValues7113Facade/save");
        }

        public NAAC_AC_7113_CoreValues_DTO deactivate(NAAC_AC_7113_CoreValues_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCoreValues7113Facade/deactivate");
        }
        public NAAC_AC_7113_CoreValues_DTO EditData(NAAC_AC_7113_CoreValues_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCoreValues7113Facade/EditData");
        }

        public NAAC_AC_7113_CoreValues_DTO getcomment(NAAC_AC_7113_CoreValues_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCoreValues7113Facade/getcomment");
        }
        public NAAC_AC_7113_CoreValues_DTO savefilewisecomments(NAAC_AC_7113_CoreValues_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCoreValues7113Facade/savefilewisecomments");
        }
        public NAAC_AC_7113_CoreValues_DTO getfilecomment(NAAC_AC_7113_CoreValues_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCoreValues7113Facade/getfilecomment");
        }
        public NAAC_AC_7113_CoreValues_DTO savemedicaldatawisecomments(NAAC_AC_7113_CoreValues_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCoreValues7113Facade/savemedicaldatawisecomments");
        }
        public NAAC_AC_7113_CoreValues_DTO viewuploadflies(NAAC_AC_7113_CoreValues_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCoreValues7113Facade/viewuploadflies");
        }
        public NAAC_AC_7113_CoreValues_DTO deleteuploadfile(NAAC_AC_7113_CoreValues_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCoreValues7113Facade/deleteuploadfile");
        }

        public NAAC_AC_7113_CoreValues_DTO getData(NAAC_AC_7113_CoreValues_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCoreValues7113Facade/getData");
        }
    }
}
