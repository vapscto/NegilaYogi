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

namespace IVRMUX.Delegates.com.vapstech.College.Fees
{
    public class CollegeFeedetailedReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<CollegeOverallFeeStatusDTO, CollegeOverallFeeStatusDTO> COMMM = new CommonDelegate<CollegeOverallFeeStatusDTO, CollegeOverallFeeStatusDTO>();
        public CollegeOverallFeeStatusDTO GetYearList(int id)
        {
            return COMMM.GETClgFee(id, "CollegeFeedetailedReportFacade/GetYearList/");
        }
        public CollegeOverallFeeStatusDTO get_courses(CollegeOverallFeeStatusDTO id)
        {
            return COMMM.PostClgFee(id, "CollegeFeedetailedReportFacade/get_courses/");
        }
        public CollegeOverallFeeStatusDTO get_branches(CollegeOverallFeeStatusDTO id)
        {
            return COMMM.PostClgFee(id, "CollegeFeedetailedReportFacade/get_branches/");
        }
        public CollegeOverallFeeStatusDTO get_semisters(CollegeOverallFeeStatusDTO id)
        {
            return COMMM.PostClgFee(id, "CollegeFeedetailedReportFacade/get_semisters/");
        }
        public CollegeOverallFeeStatusDTO get_report(CollegeOverallFeeStatusDTO id)
        {
            return COMMM.PostClgFee(id, "CollegeFeedetailedReportFacade/get_report/");
        }
        public CollegeOverallFeeStatusDTO savedata(CollegeOverallFeeStatusDTO id)
        {
            return COMMM.PostClgFee(id, "CollegeFeedetailedReportFacade/savedata/");
        }
        public CollegeOverallFeeStatusDTO editdata(CollegeOverallFeeStatusDTO data)
        {
            return COMMM.PostClgFee(data, "CollegeFeedetailedReportFacade/editdata/");
        }
        public CollegeOverallFeeStatusDTO DeleteRecord(CollegeOverallFeeStatusDTO data)
        {
            return COMMM.PostClgFee(data, "CollegeFeedetailedReportFacade/DeleteRecord/");
        }
    }
}
