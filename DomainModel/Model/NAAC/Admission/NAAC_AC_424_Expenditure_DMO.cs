using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_424_Expenditure")]
    public class NAAC_AC_424_Expenditure_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC424EXP_Id { get; set; }
        public long MI_Id { get; set; }
        public Nullable<decimal> NCAC424EXP_BooksExp { get; set; }
        public Nullable<decimal> NCAC424EXP_JournalExp { get; set; }
        public long NCAC424EXP_ExpYear { get; set; }
        public Nullable<decimal> NCAC424EXP_EJournalExp { get; set; }       
        public Nullable<bool> NCAC424EXP_ActiveFlg { get; set; }
        public Nullable<long> NCAC424EXP_CreatedBy { get; set; }
        public Nullable<long> NCAC424EXP_UpdatedBy { get; set; }
        public Nullable<System.DateTime> NCAC424EXP_CreatedDate { get; set; }
        public Nullable<System.DateTime> NCAC424EXP_UpdatedDate { get; set; }
        public string NCAC424EXP_StatusFlg { get; set; }
        public List<NAAC_AC_424_Expenditure_Files_DMO> NAAC_AC_424_Expenditure_Files_DMO { get; set; }
    }
}
