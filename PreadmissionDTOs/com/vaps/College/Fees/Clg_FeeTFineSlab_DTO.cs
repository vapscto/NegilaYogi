using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Fee
{
    public class Clg_FeeTFineSlab_DTO
    {
        public long FTFS_Id { get; set; }
        public long FMFS_Id { get; set; }
        public long FMA_Id { get; set; }
        public string FTFS_FineType { get; set; }
        public decimal FTFS_Amount { get; set; }

        public long FMH_ID { get; set; }
        public long FTI_ID { get; set; }
        public long FMG_ID { get; set; }
    }
}
