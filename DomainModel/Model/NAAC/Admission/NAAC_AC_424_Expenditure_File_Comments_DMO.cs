using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_424_Expenditure_File_Comments")]
   public class NAAC_AC_424_Expenditure_File_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

       public long NCAC424EXPFC_Id { get; set; }
        public string NCAC424EXPFC_Remarks { get; set; }
        public long? NCAC424EXPFC_RemarksBy { get; set; }
        public bool? NCAC424EXPFC_ActiveFlag { get; set; }
        public long? NCAC424EXPFC_CreatedBy { get; set; }
        public DateTime? NCAC424EXPFC_CreatedDate { get; set; }
        public long? NCAC424EXPFC_UpdatedBy { get; set; }
        public DateTime? NCAC424EXPFC_UpdatedDate { get; set; }
        public string NCAC424EXPFC_StatusFlg { get; set; }
        public long NCAC424EXPF_Id { get; set; }
    }
}
