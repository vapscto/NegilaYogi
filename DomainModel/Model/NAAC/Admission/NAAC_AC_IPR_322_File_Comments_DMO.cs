using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_IPR_322_File_Comments")]
   public class NAAC_AC_IPR_322_File_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

       public long NCACIPR322FC_Id { get; set; }
        public string NCACIPR322FC_Remarks { get; set; }
        public long? NCACIPR322FC_RemarksBy { get; set; }
        public bool? NCACIPR322FC_ActiveFlag { get; set; }
        public long? NCACIPR322FC_CreatedBy { get; set; }
        public DateTime? NCACIPR322FC_CreatedDate { get; set; }
        public long? NCACIPR322FC_UpdatedBy { get; set; }
        public DateTime? NCACIPR322FC_UpdatedDate { get; set; }
        public string NCACIPR322FC_StatusFlg { get; set; }
        public long NCACIPR322F_Id { get; set; }
    }
}
