using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_542_AlumniCont_Files")]
    public class NAAC_AC_542_AlumniContFilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long NCAC542ALMCONF_Id { get; set; }
        public long NCAC542ALMCON_Id { get; set; }
     
        public string NCAC542ALMCONF_Filedesc { get; set; }
        public string NCAC542ALMCONF_FileName { get; set; }
        public string NCAC542ALMCONF_FilePath { get; set; }
        public string NCAC542ALMCONF_StatusFlg { get; set; }
        public bool? NCAC542ALMCONF_ActiveFlg { get; set; }

        

    }
}
