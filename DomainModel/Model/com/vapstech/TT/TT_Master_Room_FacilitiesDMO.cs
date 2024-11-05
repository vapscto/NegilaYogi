using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Master_Room_Facilities")]
    public class TT_Master_Room_FacilitiesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      
        public long TTMRMFA_Id { get; set; }
        public long MI_Id { get; set; }
        public long TTMRM_Id { get; set; }
        public long TTMFA_Id { get; set; }
        public bool TTMRMFA_ActiveFlg { get; set; }
        public long TTMRMFA_CreatedBy { get; set; }
        public long TTMRMFA_UpdatedBy { get; set; }
        public DateTime TTMRMFA_CreatedDate { get; set; }
        public DateTime TTMRMFA_UpdatedDate { get; set; }


    }
}
