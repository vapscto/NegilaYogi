using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("Adm_Master_Activities")]

    public class Adm_Master_Activities

    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long AMA_Id { get; set; }
        public long MI_Id { get; set; }
        public string AMA_ActivityName { get; set; }
        public string AMA_ActivityDesc { get; set; }
        public long FMG_Id { get; set; }
        public long FMH_Id { get; set; }
        public bool AMA_ActiveFlg { get; set; }
        public long AMA_CreatedBy { get; set; }
        public DateTime AMA_CreatedDate { get; set; }
        public long AMA_UpdatedBy { get; set; }
        public DateTime AMA_UpdatedDate { get; set; }
    }
}
