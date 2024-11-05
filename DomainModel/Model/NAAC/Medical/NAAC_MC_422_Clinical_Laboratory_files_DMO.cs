using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_MC_422_Clinical_Laboratory_files")]
    public class NAAC_MC_422_Clinical_Laboratory_files_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMC422CLF_Id { get; set; }
        public string NCMC422CLF_FileName { get; set; }
        public string NCMC422CLF_Filedesc { get; set; }
        public string NCMC422CLF_FilePath { get; set; }
        public long NCMC422CL_Id { get; set; }

    }
}
