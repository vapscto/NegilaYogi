
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
    public class ClgDatewiseHeadCollectionDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ClgDatewiseHeadCollectionDTO, ClgDatewiseHeadCollectionDTO> COMMM = new CommonDelegate<ClgDatewiseHeadCollectionDTO, ClgDatewiseHeadCollectionDTO>();
        public ClgDatewiseHeadCollectionDTO GetYearList(int id)
        {
            return COMMM.GETClgFee(id, "ClgDatewiseHeadCollectionFacade/GetYearList/");
        }
        public ClgDatewiseHeadCollectionDTO get_feegroups(ClgDatewiseHeadCollectionDTO id)
        {
            return COMMM.PostClgFee(id, "ClgDatewiseHeadCollectionFacade/get_feegroups/");
        }
        public ClgDatewiseHeadCollectionDTO get_heads(ClgDatewiseHeadCollectionDTO id)
        {
            return COMMM.PostClgFee(id, "ClgDatewiseHeadCollectionFacade/get_heads/");
        }
        public ClgDatewiseHeadCollectionDTO get_semisters(ClgDatewiseHeadCollectionDTO id)
        {
            return COMMM.PostClgFee(id, "ClgDatewiseHeadCollectionFacade/get_semisters/");
        }
        public ClgDatewiseHeadCollectionDTO get_report(ClgDatewiseHeadCollectionDTO id)
        {
            return COMMM.PostClgFee(id, "ClgDatewiseHeadCollectionFacade/get_report/");
        }
        public ClgDatewiseHeadCollectionDTO savedata(ClgDatewiseHeadCollectionDTO id)
        {
            return COMMM.PostClgFee(id, "ClgDatewiseHeadCollectionFacade/savedata/");
        }
        public ClgDatewiseHeadCollectionDTO editdata(ClgDatewiseHeadCollectionDTO data)
        {
            return COMMM.PostClgFee(data, "ClgDatewiseHeadCollectionFacade/editdata/"); 
        }
        public ClgDatewiseHeadCollectionDTO DeleteRecord(ClgDatewiseHeadCollectionDTO data)
        {
            return COMMM.PostClgFee(data, "ClgDatewiseHeadCollectionFacade/DeleteRecord/"); 
        }
    }
}
