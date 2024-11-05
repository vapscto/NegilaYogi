using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.OnlineProgram
{
    [Table("Programs_Master_Level")]
    public class ProgramsMasterLevelDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PRMTLE_Id { get; set; }
        public long MI_Id { get; set; }
        public string PRMTLE_ProgramLevel { get; set; }
        public string PRMTLE_ProgramLevelDes { get; set; }
        public bool PRMTLE_ActiveFlg { get; set; }
        public long PRMTLE_CreatedBy { get; set; }
        public DateTime PRMTLE_CreatedDate { get; set; }
        public long PRMTLE_UpdatedBy { get; set; }
        public DateTime PRMTLE_UpdatedDate { get; set; }
      
        
    }
}
