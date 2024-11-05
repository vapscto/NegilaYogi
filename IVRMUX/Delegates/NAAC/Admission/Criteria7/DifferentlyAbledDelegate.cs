using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class DifferentlyAbledDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<NAAC_AC_719_DifferentlyAbled_DTO, NAAC_AC_719_DifferentlyAbled_DTO> COMMM = new CommonDelegate<NAAC_AC_719_DifferentlyAbled_DTO, NAAC_AC_719_DifferentlyAbled_DTO>();

        public NAAC_AC_719_DifferentlyAbled_DTO loaddata(NAAC_AC_719_DifferentlyAbled_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "DifferentlyAbledFacade/loaddata/");
        }

        public NAAC_AC_719_DifferentlyAbled_DTO savedatatab1(NAAC_AC_719_DifferentlyAbled_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "DifferentlyAbledFacade/savedatatab1");
        }

        public NAAC_AC_719_DifferentlyAbled_DTO editTab1(NAAC_AC_719_DifferentlyAbled_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "DifferentlyAbledFacade/editTab1");
        }

        public NAAC_AC_719_DifferentlyAbled_DTO deactivYTab1(NAAC_AC_719_DifferentlyAbled_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "DifferentlyAbledFacade/deactivYTab1");
        }

        public NAAC_AC_719_DifferentlyAbled_DTO deleteuploadfile(NAAC_AC_719_DifferentlyAbled_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "DifferentlyAbledFacade/deleteuploadfile");
        }

        public NAAC_AC_719_DifferentlyAbled_DTO getData(NAAC_AC_719_DifferentlyAbled_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "DifferentlyAbledFacade/getData");
        }

        public NAAC_AC_719_DifferentlyAbled_DTO getDataMC(NAAC_AC_719_DifferentlyAbled_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "DifferentlyAbledFacade/getDataMC");
        }
        public NAAC_AC_719_DifferentlyAbled_DTO saveMC(NAAC_AC_719_DifferentlyAbled_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "DifferentlyAbledFacade/saveMC");
        }

        public NAAC_AC_719_DifferentlyAbled_DTO EditDataMC(NAAC_AC_719_DifferentlyAbled_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "DifferentlyAbledFacade/EditDataMC");
        }
    }
}
