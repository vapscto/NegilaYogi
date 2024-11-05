using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.OnlineProgram
{
    [Table("Programs_Master_Type")]
    public class ProgramsMasterTypeDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PRMTY_Id { get; set; }
        public long MI_Id { get; set; }
        public string PRMTY_ProgramType { get; set; }
        public string PRMTY_ProgramTypeDes { get; set; }
        public bool PRMTY_ActiveFlg { get; set; }
        public long PRMTY_CreatedBy { get; set; }
        public DateTime PRMTY_CreatedDate { get; set; }
        public long PRMTY_UpdatedBy { get; set; }
        public DateTime PRMTY_UpdatedDate { get; set; }
        
    }
}
