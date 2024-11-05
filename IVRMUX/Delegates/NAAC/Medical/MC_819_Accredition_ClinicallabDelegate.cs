using CommonLibrary;
using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Medical
{
    public class MC_819_Accredition_ClinicallabDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MC_819_Accredition_ClinicallabDTO, MC_819_Accredition_ClinicallabDTO> COMMM = new CommonDelegate<MC_819_Accredition_ClinicallabDTO, MC_819_Accredition_ClinicallabDTO>();


        public MC_819_Accredition_ClinicallabDTO loaddata(MC_819_Accredition_ClinicallabDTO data)
        {
            return COMMM.naacdetailsbypost(data, "MC_819_Accredition_ClinicallabFacade/loaddata/");
        }
        public MC_819_Accredition_ClinicallabDTO savedata(MC_819_Accredition_ClinicallabDTO data)
        {
            return COMMM.naacdetailsbypost(data, "MC_819_Accredition_ClinicallabFacade/savedata/");
        }
        public MC_819_Accredition_ClinicallabDTO savedata1(MC_819_Accredition_ClinicallabDTO data)
        {
            return COMMM.naacdetailsbypost(data, "MC_819_Accredition_ClinicallabFacade/savedata1/");
        }
        public MC_819_Accredition_ClinicallabDTO savedata2(MC_819_Accredition_ClinicallabDTO data)
        {
            return COMMM.naacdetailsbypost(data, "MC_819_Accredition_ClinicallabFacade/savedata2/");
        }
        public MC_819_Accredition_ClinicallabDTO savedata3(MC_819_Accredition_ClinicallabDTO data)
        {
            return COMMM.naacdetailsbypost(data, "MC_819_Accredition_ClinicallabFacade/savedata3/");
        }
        public MC_819_Accredition_ClinicallabDTO editdata(MC_819_Accredition_ClinicallabDTO data)
        {
            return COMMM.naacdetailsbypost(data, "MC_819_Accredition_ClinicallabFacade/editdata/");
        }
        public MC_819_Accredition_ClinicallabDTO deactivate(MC_819_Accredition_ClinicallabDTO data)
        {
            return COMMM.naacdetailsbypost(data, "MC_819_Accredition_ClinicallabFacade/deactivate/");
        }
        public MC_819_Accredition_ClinicallabDTO getcomment(MC_819_Accredition_ClinicallabDTO data)
        {
            return COMMM.naacdetailsbypost(data, "MC_819_Accredition_ClinicallabFacade/getcomment/");
        }
        public MC_819_Accredition_ClinicallabDTO savecomments(MC_819_Accredition_ClinicallabDTO data)
        {
            return COMMM.naacdetailsbypost(data, "MC_819_Accredition_ClinicallabFacade/savecomments/");
        }
    }
}
