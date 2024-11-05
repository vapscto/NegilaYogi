using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("PA_Student_Vaccines")]
    public class PA_Student_Vaccines 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PASVA_Id { get; set; }

        public long PASHD_Id  { get; set; }
        public long PAMVA_Id { get; set; }
        public bool  PASVA_YesNoNAFlg { get; set; }
        public DateTime PASVA_Date { get; set; }
        public bool PASVA_ActiveFlg { get; set; }
        public long PASVA_CreatedBy { get; set; }
        public long PASVA_UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}
