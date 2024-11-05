using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Master_Facilities")]
    public class TT_Master_FacilitiesDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TTMFA_Id { get; set; }
        public long MI_Id { get; set; }
        public string TTMFA_FacilityName { get; set; }
        public string TTMFA_FacilityDesc { get; set; }
        public bool TTMFA_ActiveFlg { get; set; }
        public long TTMFA_CreatedBy { get; set; }
        public long TTMFA_UpdatedBy { get; set; }
        public DateTime TTMFA_CreatedDate { get; set; }
        public DateTime TTMFA_UpdatedDate { get; set; }

    }
}
