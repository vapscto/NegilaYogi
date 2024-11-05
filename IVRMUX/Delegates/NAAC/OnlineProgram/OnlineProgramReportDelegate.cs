using CommonLibrary;
using PreadmissionDTOs.NAAC.OnlineProgram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.OnlineProgram
{
    public class OnlineProgramReportDelegate
    {
        CommonDelegate<OnlineProgramReport_DTO, OnlineProgramReport_DTO> comm = new CommonDelegate<OnlineProgramReport_DTO, OnlineProgramReport_DTO>();
        public OnlineProgramReport_DTO getyearlyprogram(OnlineProgramReport_DTO data)
        {
            return comm.naacdetailsbypost( data, "OnlineProgramReportFacade/getyearlyprogram");
        }
        public OnlineProgramReport_DTO getYearlyProgramReport(OnlineProgramReport_DTO data)
        {
            return comm.naacdetailsbypost( data, "OnlineProgramReportFacade/getYearlyProgramReport");
        }
        public OnlineProgramReport_DTO ConferenceDetailsReport(OnlineProgramReport_DTO data)
        {
            return comm.naacdetailsbypost( data, "OnlineProgramReportFacade/ConferenceDetailsReport");
        }
    }
}
