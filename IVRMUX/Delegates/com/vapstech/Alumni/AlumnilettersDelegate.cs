using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Alumni;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Alumni
{
    public class AlumnilettersDelegate
    {
        CommonDelegate<AlumnilettersDTO, AlumnilettersDTO> comm = new CommonDelegate<AlumnilettersDTO, AlumnilettersDTO>();

        public AlumnilettersDTO BindData(AlumnilettersDTO data)
        {
            return comm.POSTDataaAlumni(data, "AlumnilettersFacade/BindData");
        }     
        public AlumnilettersDTO ShowReport(AlumnilettersDTO data)
        {
            return comm.POSTDataaAlumni(data, "AlumnilettersFacade/ShowReport");
        }
         public AlumnilettersDTO letterReport(AlumnilettersDTO data)
        {
            return comm.POSTDataaAlumni(data, "AlumnilettersFacade/letterReport");
        }
    }
}
