using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class Master_Institution_SubscriptionValidityDTO : CommonParamDTO
    {
        public long MISV_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime? MISV_FromDate { get; set; }
        public DateTime? MISV_ToDate { get; set; }
        public string MISV_SubscriptionNo { get; set; }
        public string MISV_SubscriptionType { get; set; }
        public bool MISV_ActiveFlag { get; set; }


        public string returnval { get; set; }
        public string MI_Name { get; set; }
        public Array subscriptionlist { get; set; }

        public SortingPagingInfoDTO subscriptionPagination { get; set; }
    }
}
