using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_7113_CoreValues")]
    public class NAAC_AC_7113_CoreValuesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC7113CORVAL_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC7113CORVAL_Year { get; set; }
        public string NCAC7113CORVAL_URL { get; set; }
        public bool NCAC7113CORVAL_ActiveFlg { get; set; }
        public long NCAC7113CORVAL_CreatedBy { get; set; }
        public long NCAC7113CORVAL_UpdatedBy { get; set; }
        public DateTime NCAC7113CORVAL_CreatedDate { get; set; }
        public DateTime NCAC7113CORVAL_UpdatedDate { get; set; }
        public string NCAC7113CORVAL_StatusFlg { get; set; }
    }
}
