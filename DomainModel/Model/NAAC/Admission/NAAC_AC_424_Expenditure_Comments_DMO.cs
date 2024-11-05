using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_424_Expenditure_Comments")]
   public class NAAC_AC_424_Expenditure_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

      public long NCAC424EXPC_Id { get; set; }
        public string NCAC424EXPC_Remarks { get; set; }
        public long? NCAC424EXPC_RemarksBy { get; set; }
        public string NCAC424EXPC_StatusFlg { get; set; }
        public bool? NCAC424EXPC_ActiveFlag { get; set; }
        public long? NCAC424EXPC_CreatedBy { get; set; }
        public DateTime? NCAC424EXPC_CreatedDate { get; set; }
        public long? NCAC424EXPC_UpdatedBy { get; set; }
        public DateTime? NCAC424EXPC_UpdatedDate { get; set; }
        public long NCAC424EXP_Id { get; set; }
    }
}
