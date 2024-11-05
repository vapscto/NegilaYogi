using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Route_Schedule", Schema = "TRN")]
    public class TR_Route_ScheduleDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRRSC_Id { get; set; }
        public long MI_Id { get; set; }
       // public DateTime TRRSC_Date { get; set; }
        public string TRRSC_ScheduleName { get; set; }
        public bool TRRSC_ActiveFlag { get; set; }
       // public long TRMR_Id { get; set; }
    }
}
