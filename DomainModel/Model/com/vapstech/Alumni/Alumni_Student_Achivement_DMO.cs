using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.Alumni
{
    [Table("Alumni_Student_Achivement", Schema = "ALU")]
    public class Alumni_Student_Achivement_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ALSAC_Id { get; set; }
        public long MI_Id { get; set; }
        public long ALMST_Id { get; set; }
        public long ALSREG_Id { get; set; }
        public string ALSAC_Achievement { get; set; }
        public string ALSAC_Remarks { get; set; }
        public bool ALSAC_ActiveFlg { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
