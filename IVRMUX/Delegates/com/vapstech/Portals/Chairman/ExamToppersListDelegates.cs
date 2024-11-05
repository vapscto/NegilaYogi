
using System;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class ExamToppersListDelegates
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ExamToppersListDTO, ExamToppersListDTO> COMMM = new CommonDelegate<ExamToppersListDTO, ExamToppersListDTO>();
        public ExamToppersListDTO Getdetails(ExamToppersListDTO data)
        {
            return COMMM.POSTPORTALData(data, "ExamToppersListFacade/Getdetails/");
        }

        public ExamToppersListDTO getcategory(ExamToppersListDTO data)
        {
            return COMMM.POSTPORTALData(data, "ExamToppersListFacade/getcategory/");
        }
        public ExamToppersListDTO getclassexam(ExamToppersListDTO data)
        {
            return COMMM.POSTPORTALData(data, "ExamToppersListFacade/getclassexam/");
        }
        public ExamToppersListDTO showreport(ExamToppersListDTO data)
        {
            return COMMM.POSTPORTALData(data, "ExamToppersListFacade/showreport/");
        }

        public ExamToppersListDTO getsection(ExamToppersListDTO data)
        {
            return COMMM.POSTPORTALData(data, "ExamToppersListFacade/getsection/");
        }



        
    }
}
