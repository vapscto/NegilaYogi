using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace corewebapi18072016.Delegates.com.vapstech.TT
{
    public class CLGTTGenerationDelegate
    {
        CommonDelegate<CLGTTGenerationDTO, CLGTTGenerationDTO> comm = new CommonDelegate<CLGTTGenerationDTO, CLGTTGenerationDTO>();
        public CLGTTGenerationDTO getdetails(CLGTTGenerationDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGTTGenerationFacade/getdetails");
        }
        public CLGTTGenerationDTO generate(CLGTTGenerationDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGTTGenerationFacade/generate");
        }

        public CLGTTGenerationDTO get_catg(CLGTTGenerationDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGTTGenerationFacade/get_catg");
        }
        public CLGTTGenerationDTO get_count(CLGTTGenerationDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGTTGenerationFacade/get_count");
        }
        public CLGTTGenerationDTO resetTT(CLGTTGenerationDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGTTGenerationFacade/resetTT");
        }
        public CLGTTGenerationDTO Get_temp_data(CLGTTGenerationDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGTTGenerationFacade/Get_temp_data");
        }
          public CLGTTGenerationDTO getalldetailsviewrecords(CLGTTGenerationDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGTTGenerationFacade/getalldetailsviewrecords");
        }
           public CLGTTGenerationDTO saveTemptomain(CLGTTGenerationDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGTTGenerationFacade/saveTemptomain");
        }
         public CLGTTGenerationDTO getreplacemntdetailsviewrecords(CLGTTGenerationDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGTTGenerationFacade/getreplacemntdetailsviewrecords");
        }
        public CLGTTGenerationDTO deactivate(CLGTTGenerationDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGTTGenerationFacade/deactivate");
        }


    }
}
