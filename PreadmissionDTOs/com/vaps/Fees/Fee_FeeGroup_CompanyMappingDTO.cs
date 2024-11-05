using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class Fee_FeeGroup_CompanyMappingDTO
    {
        public long MI_Id { get; set; }
        public long user_id { get; set; }
        public string return_val { get; set; }
         public string return_valtwo { get; set; }
        public bool FFGCMA_ActiveId { get; set; }
        public string FTMCOM_CompanyCode { get; set; }
        public List[] TempararyArrayList { get; set; }
        public long FFGCMA_Id { get; set; }
        public long? FTMCOM_Id { get; set; }
        public string FMG_GroupName { get; set; }
        public string FTMCOM_CompanyName { get; set; }
        public long FMGG_Id { get; set; }
    }
    public class List
    {
        public long FMG_Id { get; set; }
        public string FMGG_GroupName { get; set; }
    }
}
