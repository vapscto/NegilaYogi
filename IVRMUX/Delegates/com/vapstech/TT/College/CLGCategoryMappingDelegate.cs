using Newtonsoft.Json;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CommonLibrary;

namespace corewebapi18072016.Delegates.com.vapstech.TT.College
{
    public class CLGCategoryMappingDelegate
    {
        CommonDelegate<CLGCategoryMappingDTO, CLGCategoryMappingDTO> _comm = new CommonDelegate<CLGCategoryMappingDTO, CLGCategoryMappingDTO>();
        
        public CLGCategoryMappingDTO savedetails(CLGCategoryMappingDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGCategoryMappingFacade/savedetails/");
        }
        public CLGCategoryMappingDTO getBranch(CLGCategoryMappingDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGCategoryMappingFacade/getBranch/");
        }
        public CLGCategoryMappingDTO getalldetails(CLGCategoryMappingDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGCategoryMappingFacade/getalldetails/");
        }
        public CLGCategoryMappingDTO deleterec(CLGCategoryMappingDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGCategoryMappingFacade/deleterec/");
        }

    }
}
