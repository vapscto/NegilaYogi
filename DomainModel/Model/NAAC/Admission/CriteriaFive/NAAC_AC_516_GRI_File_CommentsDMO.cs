using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_516_GRI_File_Comments")]
    public class NAAC_AC_516_GRI_File_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
         
        public long NCAC516GRIFC_Id { get; set; }
        public string NCAC516GRIFC_Remarks { get; set; }
        public long NCAC516GRIFC_RemarksBy { get; set; }
        public bool NCAC516GRIFC_ActiveFlag { get; set; }
        public long NCAC516GRIFC_CreatedBy { get; set; }
        public DateTime NCAC516GRIFC_CreatedDate { get; set; }
        public long NCAC516GRIFC_UpdatedBy { get; set; }
        public DateTime NCAC516GRIFC_UpdatedDate { get; set; }
        public string NCAC516GRIFC_StatusFlg { get; set; }
        public long NCAC516GRIF_Id { get; set; }

    }
}
