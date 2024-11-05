using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using Newtonsoft.Json;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class TrnStudentLocationDetailsDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<TrnStudentLocationDetailsDTO, TrnStudentLocationDetailsDTO> comml = new CommonDelegate<TrnStudentLocationDetailsDTO, TrnStudentLocationDetailsDTO>();

        public TrnStudentLocationDetailsDTO Getreportdetails(TrnStudentLocationDetailsDTO data)
        {
            return comml.POSTDataTransport(data, "TrnStudentLocationDetailsFacade/Getreportdetails/");
        }
        public TrnStudentLocationDetailsDTO sendmsg(TrnStudentLocationDetailsDTO data)
        {
            return comml.POSTDataTransport(data, "TrnStudentLocationDetailsFacade/sendmsg/");
        }
        public TrnStudentLocationDetailsDTO getdata(int id)
        {
            return comml.GetDataByIdTransport(id, "TrnStudentLocationDetailsFacade/getdata/");
        }
    }
}
