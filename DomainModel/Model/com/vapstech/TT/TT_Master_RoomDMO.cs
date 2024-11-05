using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Master_Room")]
    public class TT_Master_RoomDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public long TTMRM_Id { get; set; }
        public long  MI_Id { get; set; }
        public string  TTMRM_RoomName { get; set; }
        public string TTMRM_RoomDetails { get; set; }
        public bool  TTMRM_ActiveFlg { get; set; }
        public long  TTMRM_CreatedBy { get; set; }
        public long  TTMRM_UpdatedBy { get; set; }
        public DateTime  TTMRM_CreatedDate { get; set; }
        public DateTime TTMRM_UpdatedDate { get; set; }
        public List<TT_Master_Room_FacilitiesDMO> TT_Master_Room_FacilitiesDMO { get; set; }


    }
}
