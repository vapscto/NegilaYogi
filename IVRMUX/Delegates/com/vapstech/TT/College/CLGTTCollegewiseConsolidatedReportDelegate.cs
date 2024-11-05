using CommonLibrary;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.TT.College
{
    public class CLGTTCollegewiseConsolidatedReportDelegate
    {

        CommonDelegate<CLGTTCollegewiseConsolidatedReportDTO, CLGTTCollegewiseConsolidatedReportDTO> comm = new CommonDelegate<CLGTTCollegewiseConsolidatedReportDTO, CLGTTCollegewiseConsolidatedReportDTO>();
        public CLGTTCollegewiseConsolidatedReportDTO loaddata(CLGTTCollegewiseConsolidatedReportDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGTTCollegewiseConsolidatedReportFacade/loaddata");
         }
      public CLGTTCollegewiseConsolidatedReportDTO report(CLGTTCollegewiseConsolidatedReportDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGTTCollegewiseConsolidatedReportFacade/report");
        }


    }
    }