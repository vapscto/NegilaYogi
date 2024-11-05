using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Fee
{
    public class Fee_College_T_Fine_SlabsDTO
    {
        public long FCTFS_Id { get; set; }
        public long FMFS_Id { get; set; }
        public long FCMAS_Id { get; set; }
        public string FCTFS_FineType { get; set; }
        public decimal FCTFS_Amount { get; set; }

        public long FMH_ID { get; set; }
        public long FTI_ID { get; set; }
        public long FMG_ID { get; set; }
        public decimal FTFS_Amount { get; set; }
        public string FTFS_FineType { get; set; }
        public string FMFS_FineType { get; set; }
        public long FMFS_FromDay { get; set; }
        public long FMFS_ToDay { get; set; }
        public string FCTFS_PercentageFlg { get; set; }

        public long AMSE_Id { get; set; }
    }
}
