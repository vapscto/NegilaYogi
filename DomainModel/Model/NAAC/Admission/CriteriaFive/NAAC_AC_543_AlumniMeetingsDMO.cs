using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_543_AlumniMeetings")]
    public class NAAC_AC_543_AlumniMeetingsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long NCAC543ALMMET_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC543ALMMET_MeetingYear { get; set; }
        public long NCAC543ALMMET_NoOfMeetings { get; set; }
        public DateTime NCAC543ALMMET_MeetingDate { get; set; }
        public long NCAC543ALMMET_NoOfMemAttnd { get; set; }
        public long NCAC543ALMMET_TotalAlumniCount { get; set; }
        public bool NCAC543ALMMET_ActiveFlg { get; set; }
        public long NCAC543ALMMET_CreatedBy { get; set; }
        public long NCAC543ALMMET_UpdatedBy { get; set; }
        public string NCAC543ALMMET_StatusFlg { get; set; }
        public DateTime NCAC543ALMMET_CreatedDate { get; set; }
        public DateTime NCAC543ALMMET_UpdatedDate { get; set; }

        public List<NAAC_AC_543_AlumniMeetingsFilesDMO> NAAC_AC_543_AlumniMeetingsFilesDMO { get; set; }
    }
}
