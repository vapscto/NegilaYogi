
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
    public class Clg_StudentFeeGroupMappingDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<Clg_StudentFeeGroupMapping_DTO, Clg_StudentFeeGroupMapping_DTO> COMMM = new CommonDelegate<Clg_StudentFeeGroupMapping_DTO, Clg_StudentFeeGroupMapping_DTO>();
        public Clg_StudentFeeGroupMapping_DTO GetYearList(int id)
        {
            return COMMM.GETClgFee(id, "Clg_StudentFeeGroupMappingFacade/GetYearList/");
        }
        public Clg_StudentFeeGroupMapping_DTO get_courses(Clg_StudentFeeGroupMapping_DTO id)
        {
            return COMMM.PostClgFee(id, "Clg_StudentFeeGroupMappingFacade/get_courses/");
        }
        public Clg_StudentFeeGroupMapping_DTO get_branches(Clg_StudentFeeGroupMapping_DTO id)
        {
            return COMMM.PostClgFee(id, "Clg_StudentFeeGroupMappingFacade/get_branches/");
        }
        public Clg_StudentFeeGroupMapping_DTO get_semisters(Clg_StudentFeeGroupMapping_DTO id)
        {
            return COMMM.PostClgFee(id, "Clg_StudentFeeGroupMappingFacade/get_semisters/");
        }
        public Clg_StudentFeeGroupMapping_DTO get_report(Clg_StudentFeeGroupMapping_DTO id)
        {
            return COMMM.PostClgFee(id, "Clg_StudentFeeGroupMappingFacade/get_report/");
        }
        public Clg_StudentFeeGroupMapping_DTO savedata(Clg_StudentFeeGroupMapping_DTO id)
        {
            return COMMM.PostClgFee(id, "Clg_StudentFeeGroupMappingFacade/savedata/");
        }
        public Clg_StudentFeeGroupMapping_DTO editdata(Clg_StudentFeeGroupMapping_DTO data)
        {
            return COMMM.PostClgFee(data, "Clg_StudentFeeGroupMappingFacade/editdata/"); 
        }
        public Clg_StudentFeeGroupMapping_DTO DeleteRecord(Clg_StudentFeeGroupMapping_DTO data)
        {
            return COMMM.PostClgFee(data, "Clg_StudentFeeGroupMappingFacade/DeleteRecord/"); 
        }
        //saveeditdata
        public Clg_StudentFeeGroupMapping_DTO saveeditdata(Clg_StudentFeeGroupMapping_DTO id)
        {
            return COMMM.POSTDataCollfee(id, "Clg_StudentFeeGroupMappingFacade/saveeditdata/");
        }
    }
}
