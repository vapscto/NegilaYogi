using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_Master_College_Student_PA")]
   public class Adm_Master_College_Student_PA_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long AMCSTPA_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long PACA_Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long AMCSTPA_CreatedBy { get; set; }
        public long AMCSTPA_UpdatedBy { get; set; }
    }
}
