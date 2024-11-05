using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_KM_LogBook", Schema = "TRN")]
    public class TR_KM_LogBookDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRKMLB_Id { get; set; }
        public long MI_Id { get; set; }
        public long TRMV_Id { get; set; }
        public DateTime TRKMLB_EntryDate { get; set; }
        public DateTime TRKMLB_FromDate { get; set; }
        public string TRKMLB_FromTime { get; set; }
        public DateTime TRKMLB_ToDate { get; set; }
        public string TRKMLB_ToTime { get; set; }
        public string TRKMLB_OpeningReading { get; set; }
        public string TRKMLB_ClosingReading { get; set; }
        public long TRKMLB_NoOfKM { get; set; }
        public string TRKMLB_Remarks { get; set; }
        public bool TRKMLB_ActiveFlag { get; set; }

    }
}
