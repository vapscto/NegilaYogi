using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_7112_CodeOfCoduct")]
    public class NAAC_AC_7112_CodeOfCoductDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC7112CODCON_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC7112CODCON_Year { get; set; }
        public string NCAC7112CODCON_URL { get; set; }
        public bool NCAC7112CODCON_ActiveFlg { get; set; }
        public long NCAC7112CODCON_CreatedBy { get; set; }
        public long NCAC7112CODCON_UpdatedBy { get; set; }
        public DateTime NCAC7112CODCON_CreatedDate { get; set; }
        public DateTime NCAC7112CODCON_UpdatedDate { get; set; }
        public string NCAC7112CODCON_StatusFlg { get; set; }
    }
}
