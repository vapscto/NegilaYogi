using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_543_AlumniMeetings_File_Comments")]
    public class NAAC_AC_543_AlumniMeetings_File_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long NCAC543ALMMETFC_Id { get; set; }
        public string  NCAC543ALMMETFC_Remarks { get; set; }
        public long  NCAC543ALMMETFC_RemarksBy { get; set; }
        public bool  NCAC543ALMMETFC_ActiveFlag { get; set; }
        public long  NCAC543ALMMETFC_CreatedBy { get; set; }
        public DateTime  NCAC543ALMMETFC_CreatedDate { get; set; }
        public long  NCAC543ALMMETFC_UpdatedBy { get; set; }
        public DateTime NCAC543ALMMETFC_UpdatedDate { get; set; }
        public string NCAC543ALMMETFC_StatusFlg { get; set; }
        public long  NCAC543ALMMETF_Id { get; set; }
    }
}
