using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_RTO_Details", Schema = "TRN")]
    public class RTODetailsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRRTO_Id { get; set; }
        public long TRMV_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime? TRRTO_Insurance_FromDate { get; set; }
        public DateTime? TRRTO_Insurance_Todate { get; set; }
        public string TRRTO_Company_Name { get; set; }
        public DateTime? TRRTO_Tax_FromDate { get; set; }
        public DateTime? TRRTO_Tax_ToDate { get; set; }

        public DateTime? TRRTO_FC_FromDate { get; set; }
        public DateTime? TRRTO_FC_ToDate { get; set; }
        public DateTime? TRRTO_Permit_FromDate { get; set; }
        public DateTime? TRRTO_Permit_ToDate { get; set; }

      
        public DateTime? TRRTO_Emission_FromDate { get; set; }
        public DateTime? TRRTO_Emission_ToDate { get; set; }
        public DateTime? TRRTO_Ceasefire_FromDate { get; set; }
        public DateTime? TRRTO_Ceasefire_ToDate { get; set; }
        public DateTime? TRRTO_GPS_GPRS_Fitted_date { get; set; }

      

    }
}
