using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class NAAC_EContent_434_Delegate
    {




        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<NAAC_AC_434_EContent_DTO, NAAC_AC_434_EContent_DTO> COMMM = new CommonDelegate<NAAC_AC_434_EContent_DTO, NAAC_AC_434_EContent_DTO>();
        public NAAC_AC_434_EContent_DTO loaddata(NAAC_AC_434_EContent_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacEContent434Facade/loaddata/");
        }
        public NAAC_AC_434_EContent_DTO savedata(NAAC_AC_434_EContent_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacEContent434Facade/savedata/");
        }
        public NAAC_AC_434_EContent_DTO editdata(NAAC_AC_434_EContent_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacEContent434Facade/editdata/");
        }
        public NAAC_AC_434_EContent_DTO deactiveStudent(NAAC_AC_434_EContent_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacEContent434Facade/deactiveStudent/");
        }
        public NAAC_AC_434_EContent_DTO getcomment(NAAC_AC_434_EContent_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacEContent434Facade/getcomment/");
        }
        public NAAC_AC_434_EContent_DTO savemedicaldatawisecomments(NAAC_AC_434_EContent_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacEContent434Facade/savemedicaldatawisecomments/");
        }
        public NAAC_AC_434_EContent_DTO savefilewisecomments(NAAC_AC_434_EContent_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacEContent434Facade/savefilewisecomments/");
        }
        public NAAC_AC_434_EContent_DTO getfilecomment(NAAC_AC_434_EContent_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacEContent434Facade/getfilecomment/");
        }
        public NAAC_AC_434_EContent_DTO viewuploadflies(NAAC_AC_434_EContent_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacEContent434Facade/viewuploadflies");
        }
        public NAAC_AC_434_EContent_DTO deleteuploadfile(NAAC_AC_434_EContent_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacEContent434Facade/deleteuploadfile");
        }
    }
}
