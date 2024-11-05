using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_516_GRI_Comments")]
    public class NAAC_AC_516_GRI_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
         
        
        public long  NCAC516GRIC_Id { get; set; }
        public string NCAC516GRIC_Remarks { get; set; }
        public long NCAC516GRIC_RemarksBy { get; set; }
        public string NCAC516GRIC_StatusFlg { get; set; }
        public bool NCAC516GRIC_ActiveFlag { get; set; }
        public long NCAC516GRIC_CreatedBy { get; set; }
        public DateTime NCAC516GRIC_CreatedDate { get; set; }
        public long NCAC516GRIC_UpdatedBy { get; set; }
        public DateTime NCAC516GRIC_UpdatedDate { get; set; }
        public long NCAC516GRI_Id { get; set; }

    }
}
