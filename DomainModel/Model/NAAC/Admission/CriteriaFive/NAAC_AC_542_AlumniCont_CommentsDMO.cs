using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_542_AlumniCont_Comments")]
    public class NAAC_AC_542_AlumniCont_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       
        public long NCAC542ALMCONC_Id { get; set; }
        public string NCAC542ALMCONC_Remarks { get; set; }
        public long NCAC542ALMCONC_RemarksBy { get; set; }
        public string NCAC542ALMCONC_StatusFlg { get; set; }
        public bool NCAC542ALMCONC_ActiveFlag { get; set; }
        public long NCAC542ALMCONC_CreatedBy { get; set; }
        public DateTime NCAC542ALMCONC_CreatedDate { get; set; }
        public long NCAC542ALMCONC_UpdatedBy { get; set; }
        public DateTime NCAC542ALMCONC_UpdatedDate { get; set; }
        public long NCAC542ALMCON_Id { get; set; }


    }
}
