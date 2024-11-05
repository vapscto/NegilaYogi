using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria7
{

    [Table("NAAC_AC_7112_CodeOfCoduct_Comments")]
   public class NAAC_AC_7112_CodeOfCoduct_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long NCAC7112CODCONC_Id { get; set; }
        public string NCAC7112CODCONC_Remarks { get; set; }
        public long? NCAC7112CODCONC_RemarksBy { get; set; }
        public string NCAC7112CODCONC_StatusFlg { get; set; }
        public bool? NCAC7112CODCONC_ActiveFlag { get; set; }
        public long? NCAC7112CODCONC_CreatedBy { get; set; }
        public DateTime? NCAC7112CODCONC_CreatedDate { get; set; }
        public long? NCAC7112CODCONC_UpdatedBy { get; set; }
        public DateTime? NCAC7112CODCONC_UpdatedDate { get; set; }
        public long NCAC7112CODCON_Id { get; set; }
    }
}
