
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VidyaBharathi
{
    [Table("VBSC_Master_Competition_Level")]
    
    public class VBSC_Master_Competition_LevelDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VBSCMCL_Id { get; set; }
        public long MT_Id { get; set; }
        public long VBSCMCL_LevelOrder { get; set; }
    
        public string VBSCMCL_CompetitionLevel { get; set; }
        public string VBSCMCL_CLDesc { get; set; }
  
         public string VBSCMCL_LevelFlg { get; set; }
        public string VBSCMCL_SportsCCFlg { get; set; }
        public bool? VBSCMCL_ActiveFlag { get; set; }
        public DateTime? VBSCMCL_CreatedDate { get; set; }

        public DateTime? VBSCMCL_UpdatedDate { get; set; }

    }

}

