using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Fee
{
    public class Clg_FeeTFineSlabECS_DTO
    {
        public long FTFSE_Id { get; set; }
        public long FMFS_Id { get; set; }
        public long FMA_Id { get; set; }
        public string FTFSE_FineType { get; set; }
        public decimal FTFSE_Amount { get; set; }

        public long FMH_ID { get; set; }

        public long FTI_ID { get; set; }

    }
}
