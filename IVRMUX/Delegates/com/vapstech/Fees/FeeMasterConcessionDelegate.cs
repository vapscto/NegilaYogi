using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommonLibrary;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
namespace corewebapi18072016.Delegates.com.vapstech.Fees
{
    public class FeeMasterConcessionDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<FeeMasterConcessionDTO, FeeMasterConcessionDTO> comml = new CommonDelegate<FeeMasterConcessionDTO, FeeMasterConcessionDTO>();

        public FeeMasterConcessionDTO getdata(FeeMasterConcessionDTO id)
        {
            return comml.POSTDatafee(id, "FeeMasterConcessionFacade/getdata/");
        }
        public FeeMasterConcessionDTO savedata(FeeMasterConcessionDTO data)
        {
            return comml.POSTDatafee(data, "FeeMasterConcessionFacade/savedata/");
        }
        public FeeMasterConcessionDTO savedata2(FeeMasterConcessionDTO data)
        {
            return comml.POSTDatafee(data, "FeeMasterConcessionFacade/savedata2/");
        }
        public FeeMasterConcessionDTO activedeactive(FeeMasterConcessionDTO data)
        {
            return comml.POSTDatafee(data, "FeeMasterConcessionFacade/activedeactive/");
        }
        public FeeMasterConcessionDTO deactive2(FeeMasterConcessionDTO data)
        {
            return comml.POSTDatafee(data, "FeeMasterConcessionFacade/deactive2/");
        }
        public FeeMasterConcessionDTO deactive3(FeeMasterConcessionDTO data)
        {
            return comml.POSTDatafee(data, "FeeMasterConcessionFacade/deactive3/");
        }
        public FeeMasterConcessionDTO editdata(FeeMasterConcessionDTO data)
        {
            return comml.POSTDatafee(data, "FeeMasterConcessionFacade/editdata/");
        }
        public FeeMasterConcessionDTO edit2(FeeMasterConcessionDTO data)
        {
            return comml.POSTDatafee(data, "FeeMasterConcessionFacade/edit2/");
        }
        public FeeMasterConcessionDTO gethead(FeeMasterConcessionDTO data)
        {
            return comml.POSTDatafee(data, "FeeMasterConcessionFacade/gethead/");
        }
        public FeeMasterConcessionDTO edit3(FeeMasterConcessionDTO data)
        {
            return comml.POSTDatafee(data, "FeeMasterConcessionFacade/edit3/");
        }
        public FeeMasterConcessionDTO savedata3(FeeMasterConcessionDTO data)
        {
            return comml.POSTDatafee(data, "FeeMasterConcessionFacade/savedata3/");
        }
    }
}
