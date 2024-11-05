using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace corewebapi18072016.Delegates.com.vapstech.TT
{
    public class CLGDeputationDelegate
    {

        CommonDelegate<CLGDeputationDTO, CLGDeputationDTO> comm = new CommonDelegate<CLGDeputationDTO, CLGDeputationDTO>();

        public CLGDeputationDTO savedetails(CLGDeputationDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGDeputationFacade/savedetails");
        }
         public CLGDeputationDTO getdetails(CLGDeputationDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGDeputationFacade/getdetails");
        }
        public CLGDeputationDTO get_period_alloted(CLGDeputationDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGDeputationFacade/get_period_alloted");
        }
        public CLGDeputationDTO get_free_stfdets(CLGDeputationDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGDeputationFacade/get_free_stfdets");
        }
        public CLGDeputationDTO getalldetailsviewrecords2(CLGDeputationDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGDeputationFacade/getalldetailsviewrecords2");
        }
        public CLGDeputationDTO viewdeputation(CLGDeputationDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGDeputationFacade/viewdeputation");
        }
        public CLGDeputationDTO viewabsent(CLGDeputationDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGDeputationFacade/viewabsent");
        }
        public CLGDeputationDTO getabsentstaff(CLGDeputationDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGDeputationFacade/getabsentstaff");
        }

        

    }
}
