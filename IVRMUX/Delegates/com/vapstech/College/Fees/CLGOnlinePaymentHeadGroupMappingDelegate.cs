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
    public class CLGOnlinePaymentHeadGroupMappingDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<Clg_StudentFeeGroupMapping_DTO, Clg_StudentFeeGroupMapping_DTO> COMMM = new CommonDelegate<Clg_StudentFeeGroupMapping_DTO, Clg_StudentFeeGroupMapping_DTO>();
        public Clg_StudentFeeGroupMapping_DTO onlineMappingDetails(int id)
        {
            return COMMM.GETClgFee(id, "CLGOnlinePaymentHeadGroupMappingFacade/onlineMappingDetails/");
        }
        public Clg_StudentFeeGroupMapping_DTO edit(int id)
        {
            return COMMM.GETClgFee(id, "CLGOnlinePaymentHeadGroupMappingFacade/edit/");
        }
        public Clg_StudentFeeGroupMapping_DTO delete(int id)
        {
            return COMMM.GETClgFee(id, "CLGOnlinePaymentHeadGroupMappingFacade/delete/");
        }
        public Clg_StudentFeeGroupMapping_DTO save(Clg_StudentFeeGroupMapping_DTO id)
        {
            return COMMM.PostClgFee(id, "CLGOnlinePaymentHeadGroupMappingFacade/save/");
        }
        public Clg_StudentFeeGroupMapping_DTO groupsel(Clg_StudentFeeGroupMapping_DTO id)
        {
            return COMMM.PostClgFee(id, "CLGOnlinePaymentHeadGroupMappingFacade/groupsel/");
        }
        public Clg_StudentFeeGroupMapping_DTO savedata(Clg_StudentFeeGroupMapping_DTO id)
        {
            return COMMM.PostClgFee(id, "CLGOnlinePaymentHeadGroupMappingFacade/savedata/");
        }
        public Clg_StudentFeeGroupMapping_DTO headsel(Clg_StudentFeeGroupMapping_DTO data)
        {
            return COMMM.PostClgFee(data, "CLGOnlinePaymentHeadGroupMappingFacade/headsel/"); 
        }
        public Clg_StudentFeeGroupMapping_DTO academicsel(Clg_StudentFeeGroupMapping_DTO data)
        {
            return COMMM.PostClgFee(data, "CLGOnlinePaymentHeadGroupMappingFacade/academicsel/"); 
        }
    }
}
