using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.College.Preadmission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Preadmission
{
    public class DocumentViewClgDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<CollegePreadmissionstudnetDto, CollegePreadmissionstudnetDto> COMMM = new CommonDelegate<CollegePreadmissionstudnetDto, CollegePreadmissionstudnetDto>();

        public CollegePreadmissionstudnetDto getdetails(CollegePreadmissionstudnetDto CollegePreadmissionstudnetDto)
        {
            return COMMM.CollegePOSTData(CollegePreadmissionstudnetDto, "DocumentViewClgFacade/getdetails/");
        }

        public CollegePreadmissionstudnetDto getclgstudata(CollegePreadmissionstudnetDto CollegePreadmissionstudnetDto)
        {
            return COMMM.CollegePOSTData(CollegePreadmissionstudnetDto, "DocumentViewClgFacade/getclgstudata/");
        }

        public CollegePreadmissionstudnetDto getdocksonly(CollegePreadmissionstudnetDto CollegePreadmissionstudnetDto)
        {
            return COMMM.CollegePOSTData(CollegePreadmissionstudnetDto, "DocumentViewClgFacade/getdocksonly/");
        }
        //Admssion Register Report
        public CollegePreadmissionstudnetDto GetData(CollegePreadmissionstudnetDto CollegePreadmissionstudnetDto)
        {
            return COMMM.CollegePOSTData(CollegePreadmissionstudnetDto, "DocumentViewClgFacade/Getregdata/");
        }

        public CollegePreadmissionstudnetDto getbranch(CollegePreadmissionstudnetDto CollegePreadmissionstudnetDto)
        {
            return COMMM.CollegePOSTData(CollegePreadmissionstudnetDto, "DocumentViewClgFacade/getbranch/");
        }
        public CollegePreadmissionstudnetDto getsemester(CollegePreadmissionstudnetDto CollegePreadmissionstudnetDto)
        {
            return COMMM.CollegePOSTData(CollegePreadmissionstudnetDto, "DocumentViewClgFacade/getsemester/");
        }
    }
}
