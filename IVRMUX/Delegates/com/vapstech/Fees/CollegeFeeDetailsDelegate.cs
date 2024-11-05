
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
    public class CollegeFeeDetailsDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<CollegeStudentLedgerDTO, CollegeStudentLedgerDTO> COMMM = new CommonDelegate<CollegeStudentLedgerDTO, CollegeStudentLedgerDTO>();
        public CollegeStudentLedgerDTO GetYearList(int id)
        {
            return COMMM.GETClgFee(id, "CollegeFeeDetailsFacade/GetYearList/");
        }
        public CollegeStudentLedgerDTO get_courses(CollegeStudentLedgerDTO id)
        {
            return COMMM.PostClgFee(id, "CollegeFeeDetailsFacade/get_courses/");
        }
        public CollegeStudentLedgerDTO get_branches(CollegeStudentLedgerDTO id)
        {
            return COMMM.PostClgFee(id, "CollegeFeeDetailsFacade/get_branches/");
        }
        public CollegeStudentLedgerDTO get_semisters(CollegeStudentLedgerDTO id)
        {
            return COMMM.PostClgFee(id, "CollegeFeeDetailsFacade/get_semisters/");
        }
        public CollegeStudentLedgerDTO get_report(CollegeStudentLedgerDTO id)
        {
            return COMMM.PostClgFee(id, "CollegeFeeDetailsFacade/get_report/");
        }

        public CollegeStudentLedgerDTO get_concession_report(CollegeStudentLedgerDTO id)
        {
            return COMMM.PostClgFee(id, "CollegeFeeDetailsFacade/get_concession_report/");
        }


    }
}
