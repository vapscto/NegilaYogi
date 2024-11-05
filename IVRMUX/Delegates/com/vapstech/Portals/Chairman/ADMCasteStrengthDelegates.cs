
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs.com.vaps.Portals.Chirman;


using Newtonsoft.Json;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class ADMCasteStrengthDelegates
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ADMCasteStrengthDTO, ADMCasteStrengthDTO> COMMM = new CommonDelegate<ADMCasteStrengthDTO, ADMCasteStrengthDTO>();
        public ADMCasteStrengthDTO Getdetails(ADMCasteStrengthDTO data)
        {
            return COMMM.POSTPORTALData(data, "ADMCasteStrengthFacade/Getdetails/");
        }

        public ADMCasteStrengthDTO getclass(ADMCasteStrengthDTO data)
        {
            return COMMM.POSTPORTALData(data, "ADMCasteStrengthFacade/getclass/");
        }
        public ADMCasteStrengthDTO Getsection(ADMCasteStrengthDTO data)
        {
            return COMMM.POSTPORTALData(data, "ADMCasteStrengthFacade/Getsection/");
        }
        public ADMCasteStrengthDTO Getsectioncount(ADMCasteStrengthDTO data)
        {
            return COMMM.POSTPORTALData(data, "ADMCasteStrengthFacade/Getsectioncount/");
        }
        public ADMCasteStrengthDTO Getreport(ADMCasteStrengthDTO data)
        {
            return COMMM.POSTPORTALData(data, "ADMCasteStrengthFacade/Getreport/");
        }

        public ADMCasteStrengthDTO Getstudentdetails(ADMCasteStrengthDTO data)
        {
            return COMMM.POSTPORTALData(data, "ADMCasteStrengthFacade/Getstudentdetails/");
        }

        

    }
}
