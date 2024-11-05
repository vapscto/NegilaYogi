using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("PA_Master_Vaccines")]
    public class PA_Master_Vaccines
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long PAMVA_Id { get; set; }
        public long MI_Id { get; set; }
        public string PAMVA_Vaccines { get; set; }
        public bool PAMVA_YesNoFlg { get; set; }
        public bool PAMVA_DateAplFlg { get; set; }
        public bool PAMVA_ActiveFlg { get; set; }
        public long PAMVA_CreatedBy { get; set; }
        public long PAMVA_UpdatedBy { get; set; }

    }
}
