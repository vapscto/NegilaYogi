using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Vehicle_Driver_Substitute", Schema = "TRN")]
    public class VehicalDriverSubstituteDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long TRVDST_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime TRVDST_FromDate { get; set; }
        public DateTime TRVDST_ToDate { get; set; }
        public long TRMV_Id { get; set; }
        public long TRVDST_AbsentDriverId { get; set; }

        public long TRVDST_SubstituteDriverId { get; set; }
    }
}
