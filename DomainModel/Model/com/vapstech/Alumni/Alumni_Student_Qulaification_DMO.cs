using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.Alumni
{
    [Table("Alumni_Student_Qulaification", Schema = "ALU")]
    public class Alumni_Student_Qulaification_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ALSQU_Id { get; set; }
        public long MI_Id { get; set; }
        public long IVRMMS_Id { get; set; }
        public long ALMST_Id { get; set; }
        public long ALSREG_Id { get; set; }
        public string ALSQU_Qulification { get; set; }
        public long? ALSQU_YearOfPassing { get; set; }
        public string ALSQU_University { get; set; }
        public decimal? ALSQU_Percentage { get; set; }
        public string ALSQU_Place { get; set; }
        public string ALSQU_OtherDetails { get; set; }
        public bool ALSQU_ActiveFlg { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
