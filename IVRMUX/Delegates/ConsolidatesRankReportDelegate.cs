using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CommonLibrary;
using PreadmissionDTOs;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Delegates
{
   

   
    public class ConsolidatesRankReportDelegate 
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<WrittenTestMarksBindDataDTO, WrittenTestMarksBindDataDTO> COMMM = new CommonDelegate<WrittenTestMarksBindDataDTO, WrittenTestMarksBindDataDTO>();


        public WrittenTestMarksBindDataDTO Getdetails(WrittenTestMarksBindDataDTO WrittenTestMarksBindDataDTO)
        {
            return COMMM.POSTData(WrittenTestMarksBindDataDTO, "ConsolidatesRankReportFacade/Getdetails");
            
        }
        public WrittenTestMarksBindDataDTO getclass(WrittenTestMarksBindDataDTO data)
        {
            return COMMM.POSTData(data, "ConsolidatesRankReportFacade/getclass/");
        }
        public WrittenTestMarksBindDataDTO Getreport(WrittenTestMarksBindDataDTO data)
        {
            return COMMM.POSTData(data, "ConsolidatesRankReportFacade/Getreport/");
        }

    }
}
