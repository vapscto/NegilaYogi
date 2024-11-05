using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.College.Fees.Reports
{
    public class CollegeAccountsPositionReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

       CommonDelegate<CollegeConcessionDTO, CollegeConcessionDTO> COMMM = new CommonDelegate<CollegeConcessionDTO, CollegeConcessionDTO>();

      

        public CollegeConcessionDTO getdata(CollegeConcessionDTO data)
        {
            return COMMM.POSTDataCollfee(data, "CollegeAccountsPositionReportFacade/getdata/");
          
        }
        public CollegeConcessionDTO getgroupByCG(CollegeConcessionDTO data)
        {

           
            return COMMM.POSTDataCollfee(data, "CollegeAccountsPositionReportFacade/getgroupByCG/");

            
        }
        public CollegeConcessionDTO getReport(CollegeConcessionDTO data)
        {

                       return COMMM.POSTDataCollfee(data, "CollegeAccountsPositionReportFacade/getReport/");

        }
    }
}
