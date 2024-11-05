
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
    public class CollegeStudentLedgerDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<CollegeStudentLedgerDTO, CollegeStudentLedgerDTO> COMMM = new CommonDelegate<CollegeStudentLedgerDTO, CollegeStudentLedgerDTO>();
        public CollegeStudentLedgerDTO GetYearList(int id)
        {
            return COMMM.GETClgFee(id, "CollegeStudentLedgerFacade/GetYearList/");
        }
        public CollegeStudentLedgerDTO get_courses(CollegeStudentLedgerDTO id)
        {
            return COMMM.PostClgFee(id, "CollegeStudentLedgerFacade/get_courses/");
        }
        public CollegeStudentLedgerDTO get_branches(CollegeStudentLedgerDTO id)
        {
            return COMMM.PostClgFee(id, "CollegeStudentLedgerFacade/get_branches/");
        }
        public CollegeStudentLedgerDTO get_semisters(CollegeStudentLedgerDTO id)
        {
            return COMMM.PostClgFee(id, "CollegeStudentLedgerFacade/get_semisters/");
        }
        public CollegeStudentLedgerDTO get_student(CollegeStudentLedgerDTO id)
        {
            return COMMM.PostClgFee(id, "CollegeStudentLedgerFacade/get_student/");
        }
        
        public CollegeStudentLedgerDTO get_report(CollegeStudentLedgerDTO id)
        {
            return COMMM.PostClgFee(id, "CollegeStudentLedgerFacade/get_report/");
        }
        public CollegeStudentLedgerDTO savedata(CollegeStudentLedgerDTO id)
        {
            return COMMM.PostClgFee(id, "CollegeStudentLedgerFacade/savedata/");
        }
        public CollegeStudentLedgerDTO editdata(CollegeStudentLedgerDTO data)
        {
            return COMMM.PostClgFee(data, "CollegeStudentLedgerFacade/editdata/"); 
        }
        public CollegeStudentLedgerDTO DeleteRecord(CollegeStudentLedgerDTO data)
        {
            return COMMM.PostClgFee(data, "CollegeStudentLedgerFacade/DeleteRecord/"); 
        }
    }
}
