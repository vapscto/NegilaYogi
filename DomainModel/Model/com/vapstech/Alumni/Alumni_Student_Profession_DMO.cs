using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.Alumni
{
    [Table("Alumni_Student_Profession", Schema = "ALU")]
    public class Alumni_Student_Profession_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ALSPR_Id { get; set; }
        public long MI_Id { get; set; }
        public long ALSREG_Id { get; set; }
        public long ALMST_Id { get; set; }
        public string ALSPR_CompanyName { get; set; }
        public string ALSPR_CompanyAddress { get; set; }
        public string ALSPR_Designation { get; set; }
        public string ALSPR_EmailId { get; set; }
        public long ALSPR_WorkingSince { get; set; }
        public string ALSPR_OtherDetails { get; set; }
        public bool ALSPR_ActiveFlg { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
