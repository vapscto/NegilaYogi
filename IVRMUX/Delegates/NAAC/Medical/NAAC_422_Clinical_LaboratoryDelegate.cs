using CommonLibrary;
using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Medical
{
    public class NAAC_422_Clinical_LaboratoryDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<NAAC_MC_422_Clinical_Laboratory_DTO, NAAC_MC_422_Clinical_Laboratory_DTO> COMMM = new CommonDelegate<NAAC_MC_422_Clinical_Laboratory_DTO, NAAC_MC_422_Clinical_Laboratory_DTO>();

        public NAAC_MC_422_Clinical_Laboratory_DTO loaddata(NAAC_MC_422_Clinical_Laboratory_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_422_Clinical_Laboratoryfacade/loaddata/");
        }
        public NAAC_MC_422_Clinical_Laboratory_DTO savedata(NAAC_MC_422_Clinical_Laboratory_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_422_Clinical_Laboratoryfacade/savedata/");
        }
        public NAAC_MC_422_Clinical_Laboratory_DTO editdata(NAAC_MC_422_Clinical_Laboratory_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_422_Clinical_Laboratoryfacade/editdata/");
        }
        public NAAC_MC_422_Clinical_Laboratory_DTO deactive_Y(NAAC_MC_422_Clinical_Laboratory_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_422_Clinical_Laboratoryfacade/deactive_Y/");
        }
        public NAAC_MC_422_Clinical_Laboratory_DTO viewuploadflies(NAAC_MC_422_Clinical_Laboratory_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_422_Clinical_Laboratoryfacade/viewuploadflies/");
        }
        public NAAC_MC_422_Clinical_Laboratory_DTO deleteuploadfile(NAAC_MC_422_Clinical_Laboratory_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NAAC_422_Clinical_Laboratoryfacade/deleteuploadfile/");
        }
    }
}
