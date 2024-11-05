
using System;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class ExmSectionPerformDelegates
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ExmSectionPerformDTO, ExmSectionPerformDTO> COMMM = new CommonDelegate<ExmSectionPerformDTO, ExmSectionPerformDTO>();
        public ExmSectionPerformDTO Getdetails(ExmSectionPerformDTO data)
        {
            return COMMM.POSTPORTALData(data, "ExmSectionPerformFacade/Getdetails/");
        }

        public ExmSectionPerformDTO getcategory(ExmSectionPerformDTO data)
        {
            return COMMM.POSTPORTALData(data, "ExmSectionPerformFacade/getcategory/");
        }
        public ExmSectionPerformDTO getclassexam(ExmSectionPerformDTO data)
        {
            return COMMM.POSTPORTALData(data, "ExmSectionPerformFacade/getclassexam/");
        }
        public ExmSectionPerformDTO showreport(ExmSectionPerformDTO data)
        {
            return COMMM.POSTPORTALData(data, "ExmSectionPerformFacade/showreport/");
        }

        public ExmSectionPerformDTO getsubject(ExmSectionPerformDTO data)
        {
            return COMMM.POSTPORTALData(data, "ExmSectionPerformFacade/getsubject/");
        }



        
    }
}
