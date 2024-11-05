
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
    public class CollegeOverallFeeStatusDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<CollegeOverallFeeStatusDTO, CollegeOverallFeeStatusDTO> COMMM = new CommonDelegate<CollegeOverallFeeStatusDTO, CollegeOverallFeeStatusDTO>();
        public CollegeOverallFeeStatusDTO GetYearList(int id)
        {
            return COMMM.GETClgFee(id, "CollegeOverallFeeStatusFacade/GetYearList/");
        }
        public CollegeOverallFeeStatusDTO get_courses(CollegeOverallFeeStatusDTO id)
        {
            return COMMM.PostClgFee(id, "CollegeOverallFeeStatusFacade/get_courses/");
        }
        public CollegeOverallFeeStatusDTO get_branches(CollegeOverallFeeStatusDTO id)
        {
            return COMMM.PostClgFee(id, "CollegeOverallFeeStatusFacade/get_branches/");
        }
        public CollegeOverallFeeStatusDTO get_semisters(CollegeOverallFeeStatusDTO id)
        {
            return COMMM.PostClgFee(id, "CollegeOverallFeeStatusFacade/get_semisters/");
        }
        public CollegeOverallFeeStatusDTO get_report(CollegeOverallFeeStatusDTO id)
        {
            return COMMM.PostClgFee(id, "CollegeOverallFeeStatusFacade/get_report/");
        }
        public CollegeOverallFeeStatusDTO savedata(CollegeOverallFeeStatusDTO id)
        {
            return COMMM.PostClgFee(id, "CollegeOverallFeeStatusFacade/savedata/");
        }
        public CollegeOverallFeeStatusDTO editdata(CollegeOverallFeeStatusDTO data)
        {
            return COMMM.PostClgFee(data, "CollegeOverallFeeStatusFacade/editdata/"); 
        }
        public CollegeOverallFeeStatusDTO DeleteRecord(CollegeOverallFeeStatusDTO data)
        {
            return COMMM.PostClgFee(data, "CollegeOverallFeeStatusFacade/DeleteRecord/"); 
        }
    }
}
