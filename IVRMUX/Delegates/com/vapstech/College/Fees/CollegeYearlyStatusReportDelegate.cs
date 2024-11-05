
using System;
using PreadmissionDTOs.com.vaps.College.Fee;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using CommonLibrary;

namespace corewebapi18072016.Delegates.com.vapstech.College.Fees.Masters
{
    public class CollegeYearlyStatusReportDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<CollegeYearlyStatusReportDTO, CollegeYearlyStatusReportDTO> COMMM = new CommonDelegate<CollegeYearlyStatusReportDTO, CollegeYearlyStatusReportDTO>();
        public CollegeYearlyStatusReportDTO GetYearList(int id)
        {
            return COMMM.GETClgFee(id, "CollegeYearlyStatusReportFacade/GetYearList/");
        }

        public CollegeYearlyStatusReportDTO savedata(CollegeYearlyStatusReportDTO id)
        {
            return COMMM.PostClgFee(id, "CollegeYearlyStatusReportFacade/savedata/");
        }
        public CollegeYearlyStatusReportDTO get_group(CollegeYearlyStatusReportDTO id)
        {
            return COMMM.PostClgFee(id, "CollegeYearlyStatusReportFacade/get_group/");
        }
        
    }
}
