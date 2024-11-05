using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_543_AlumniMeetings_Files")]
    public class NAAC_AC_543_AlumniMeetingsFilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC543ALMMETF_Id { get; set; }
        public long NCAC543ALMMET_Id { get; set; }
    
        public string NCAC543ALMMETF_Filedesc { get; set; }
        public string NCAC543ALMMETF_FileName { get; set; }
        public string NCAC543ALMMETF_FilePath { get; set; }
        public string NCAC543ALMMETF_StatusFlg { get; set; }
        public bool? NCAC543ALMMETF_ActiveFlg { get; set; }
        

    }
}
