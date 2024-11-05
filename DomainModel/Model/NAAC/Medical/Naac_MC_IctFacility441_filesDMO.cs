using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_MC_441_ICTFacilities_files")]
  public  class Naac_MC_IctFacility441_filesDMO
    {
        [Key]
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)]


        public long NCMCCTTF441F_Id { get; set; }
        public string NCMCCTTF441F_FileName { get; set; }
        public string NCMCCTTF441F_Filedesc { get; set; }
        public string NCMCCTTF441F_FilePath { get; set; }
        public long NCMCCTTF441_Id { get; set; }
    }
}
