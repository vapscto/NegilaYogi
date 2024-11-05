using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_516_GRI_Files")]
    public class NAAC_AC_516_GRIFilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
         
        public long NCAC516GRIF_Id { get; set; }
        public long NCAC516GRI_Id { get; set; }
       
        public string NCAC516GRIF_FileName { get; set; }
        public string NCAC516GRIF_Filedesc { get; set; }
        public string NCAC516GRIF_FilePath { get; set; }
        public string NCAC516GRIF_StatusFlg { get; set; }
        public bool? NCAC516GRIF_ActiveFlg { get; set; }
        




    }
}
