
using System;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class ExamHomeDelegates
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ExamHomeDTO, ExamHomeDTO> COMMM = new CommonDelegate<ExamHomeDTO, ExamHomeDTO>();
        public ExamHomeDTO Getdetails(ExamHomeDTO data)
        {
            return COMMM.POSTPORTALData(data, "ExamHomeFacade/Getdetails/");
        }

        public ExamHomeDTO getcategory(ExamHomeDTO data)
        {
            return COMMM.POSTPORTALData(data, "ExamHomeFacade/getcategory/");
        }
        public ExamHomeDTO getclassexam(ExamHomeDTO data)
        {
            return COMMM.POSTPORTALData(data, "ExamHomeFacade/getclassexam/");
        }
        public ExamHomeDTO showreport(ExamHomeDTO data)
        {
            return COMMM.POSTPORTALData(data, "ExamHomeFacade/showreport/");
        }

        public ExamHomeDTO showsectioncount(ExamHomeDTO data)
        {
            return COMMM.POSTPORTALData(data, "ExamHomeFacade/showsectioncount/");
        }



        
    }
}
