using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Route_Sch_Session", Schema = "TRN")]
    public class RouteSessionScheduleDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRRSCS_Id { get; set; }
        public long TRRSC_Id { get; set; }
        public long TRMS_Id { get; set; }
        //public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string TRRSCS_Day { get; set; }
        public string TRRSCS_FromTime { get; set; }
        public string TRRSCS_ToTime { get; set; }
        public long TRMR_Id { get; set; }
        public DateTime TRRSCS_Date { get; set; }



    }
}
