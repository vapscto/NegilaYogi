using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria7
{
    [Table("NAAC_AC_7112_CodeOfCoduct_File_Comments")]
   public class NAAC_AC_7112_CodeOfCoduct_File_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

      public long NCAC7112CODCONFC_Id { get; set; }
        public string NCAC7112CODCONFC_Remarks { get; set; }
    public long? NCAC7112CODCONFC_RemarksBy { get; set; }
public bool? NCAC7112CODCONFC_ActiveFlag { get; set; }
 public long? NCAC7112CODCONFC_CreatedBy { get; set; }
 public DateTime? NCAC7112CODCONFC_CreatedDate { get; set; }
 public long? NCAC7112CODCONFC_UpdatedBy { get; set; }
 public DateTime? NCAC7112CODCONFC_UpdatedDate { get; set; }
 public string NCAC7112CODCONFC_StatusFlg { get; set; }
 public long  NCAC7112CODCONF_Id { get; set; }
    }
}
