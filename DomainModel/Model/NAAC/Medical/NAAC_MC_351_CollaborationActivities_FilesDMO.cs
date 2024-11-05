using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_MC_351_CollaborationActivities_Files")]
    public class NAAC_MC_351_CollaborationActivities_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMC351CAF_Id { get; set; }
        public string NCMC351CAF_FileName { get; set; }
        public string NCMC351CAF_Filedesc { get; set; }
        public string NCMC351CAF_FilePath { get; set; }
        public long NCMC351CA_Id { get; set; }
    }
}
