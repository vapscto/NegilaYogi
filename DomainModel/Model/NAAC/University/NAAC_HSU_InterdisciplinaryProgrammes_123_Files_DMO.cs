using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.University
{
    [Table("NAAC_HSU_InterdisciplinaryProgrammes_123_Files")]
    public class NAAC_HSU_InterdisciplinaryProgrammes_123_Files_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long NCHSUIP123F_Id { get; set; }
        public long NCHSUIP123_Id { get; set; }
        public string NCHSUIP123F_FileName { get; set; }
        public string NCHSUIP123F_FilePath { get; set; }
        public string NCHSUIP123F_FileDesc { get; set; }

        




    }
}
