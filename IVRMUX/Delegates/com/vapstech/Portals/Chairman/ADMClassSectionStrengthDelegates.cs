
using System;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class ADMClassSectionStrengthDelegates
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ADMClassSectionStrengthDTO, ADMClassSectionStrengthDTO> COMMM = new CommonDelegate<ADMClassSectionStrengthDTO, ADMClassSectionStrengthDTO>();
        public ADMClassSectionStrengthDTO Getdetails(ADMClassSectionStrengthDTO data)
        {
            return COMMM.POSTPORTALData(data, "ADMClassSectionStrengthFacade/Getdetails/");
        }

        public ADMClassSectionStrengthDTO getclass(ADMClassSectionStrengthDTO data)
        {
            return COMMM.POSTPORTALData(data, "ADMClassSectionStrengthFacade/getclass/");
        }
        public ADMClassSectionStrengthDTO Getsection(ADMClassSectionStrengthDTO data)
        {
            return COMMM.POSTPORTALData(data, "ADMClassSectionStrengthFacade/Getsection/");
        }
        public ADMClassSectionStrengthDTO Getsectioncount(ADMClassSectionStrengthDTO data)
        {
            return COMMM.POSTPORTALData(data, "ADMClassSectionStrengthFacade/Getsectioncount/");
        }

        


    }
}
