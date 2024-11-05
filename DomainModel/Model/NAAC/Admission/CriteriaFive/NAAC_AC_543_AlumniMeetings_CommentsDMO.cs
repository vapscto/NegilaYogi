using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_543_AlumniMeetings_Comments")]
    public class NAAC_AC_543_AlumniMeetings_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
   
        public long  NCAC543ALMMETC_Id { get; set; }
        public string NCAC543ALMMETC_Remarks { get; set; }
        public long NCAC543ALMMETC_RemarksBy { get; set; }
        public string NCAC543ALMMETC_StatusFlg { get; set; }
        public bool NCAC543ALMMETC_ActiveFlag { get; set; }
        public long NCAC543ALMMETC_CreatedBy { get; set; }
        public DateTime NCAC543ALMMETC_CreatedDate { get; set; }
        public long NCAC543ALMMETC_UpdatedBy { get; set; }
        public DateTime NCAC543ALMMETC_UpdatedDate { get; set; }
        public long NCAC543ALMMET_Id { get; set; }
    }
}
