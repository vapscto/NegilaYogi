using CommonLibrary;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Sports
{
    public class HSEligibilityCerficateDelegate
    {
        CommonDelegate<HSEligibilityCerficateDTO, HSEligibilityCerficateDTO> COMMM = new CommonDelegate<HSEligibilityCerficateDTO, HSEligibilityCerficateDTO>();

        public HSEligibilityCerficateDTO Getdetails(HSEligibilityCerficateDTO data)
        {
            return COMMM.POSTDataSports(data, "HSEligibilityCerficateFacade/Getdetails/");
        }
        public HSEligibilityCerficateDTO get_class(HSEligibilityCerficateDTO data)
        {
            return COMMM.POSTDataSports(data, "HSEligibilityCerficateFacade/get_class/");
        }
        public HSEligibilityCerficateDTO get_section(HSEligibilityCerficateDTO data)
        {
            return COMMM.POSTDataSports(data, "HSEligibilityCerficateFacade/get_section/");
        }
        public HSEligibilityCerficateDTO get_student(HSEligibilityCerficateDTO data)
        {
            return COMMM.POSTDataSports(data, "HSEligibilityCerficateFacade/get_student/");
        }
        public HSEligibilityCerficateDTO get_age(HSEligibilityCerficateDTO data)
        {
            return COMMM.POSTDataSports(data, "HSEligibilityCerficateFacade/get_age/");
        }
        public HSEligibilityCerficateDTO get_certificate(HSEligibilityCerficateDTO data)
        {
            return COMMM.POSTDataSports(data, "HSEligibilityCerficateFacade/get_certificate/");
        }
        public HSEligibilityCerficateDTO get_PUcertificate(HSEligibilityCerficateDTO data)
        {
            return COMMM.POSTDataSports(data, "HSEligibilityCerficateFacade/get_PUcertificate/");
        }
    }
}
