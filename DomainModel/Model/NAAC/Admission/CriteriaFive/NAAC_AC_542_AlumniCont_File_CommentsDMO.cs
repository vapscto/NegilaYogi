using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_542_AlumniCont_File_Comments")]
    public class NAAC_AC_542_AlumniCont_File_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       
      
   public long NCAC542ALMCONFC_Id { get; set; }
    public string NCAC542ALMCONFC_Remarks { get; set; }
    public long NCAC542ALMCONFC_RemarksBy { get; set; }
    public bool NCAC542ALMCONFC_ActiveFlag { get; set; }
    public long NCAC542ALMCONFC_CreatedBy { get; set; }
    public DateTime NCAC542ALMCONFC_CreatedDate { get; set; }
    public long NCAC542ALMCONFC_UpdatedBy { get; set; }
    public DateTime NCAC542ALMCONFC_UpdatedDate { get; set; }
    public string NCAC542ALMCONFC_StatusFlg { get; set; }
    public long NCAC542ALMCONF_Id { get; set; }


}
}
