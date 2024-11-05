using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class AlternateEnergyDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<NAAC_AC_713_AlternateEnergy_DTO, NAAC_AC_713_AlternateEnergy_DTO> COMMM = new CommonDelegate<NAAC_AC_713_AlternateEnergy_DTO, NAAC_AC_713_AlternateEnergy_DTO>();

        public NAAC_AC_713_AlternateEnergy_DTO loaddata(NAAC_AC_713_AlternateEnergy_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "AlternateEnergyFacade/loaddata/");
        }

        public NAAC_AC_713_AlternateEnergy_DTO savedatatab1(NAAC_AC_713_AlternateEnergy_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "AlternateEnergyFacade/savedatatab1");
        }

        public NAAC_AC_713_AlternateEnergy_DTO editTab1(NAAC_AC_713_AlternateEnergy_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "AlternateEnergyFacade/editTab1");
        }

        public NAAC_AC_713_AlternateEnergy_DTO deactivYTab1(NAAC_AC_713_AlternateEnergy_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "AlternateEnergyFacade/deactivYTab1");
        }

        public NAAC_AC_713_AlternateEnergy_DTO deleteuploadfile(NAAC_AC_713_AlternateEnergy_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "AlternateEnergyFacade/deleteuploadfile");
        }

        public NAAC_AC_713_AlternateEnergy_DTO getData(NAAC_AC_713_AlternateEnergy_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "AlternateEnergyFacade/getData");
        }

        //MC
        public NAAC_AC_713_AlternateEnergy_DTO getDataMC(NAAC_AC_713_AlternateEnergy_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "AlternateEnergyFacade/getDataMC");
        }

        public NAAC_AC_713_AlternateEnergy_DTO savedatatabMC(NAAC_AC_713_AlternateEnergy_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "AlternateEnergyFacade/savedatatabMC");
        }

        public NAAC_AC_713_AlternateEnergy_DTO editTabMC(NAAC_AC_713_AlternateEnergy_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "AlternateEnergyFacade/editTabMC");
        }

        public NAAC_AC_713_AlternateEnergy_DTO deactivateMC(NAAC_AC_713_AlternateEnergy_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "AlternateEnergyFacade/deactivateMC");
        }
        //MC
    }
}
