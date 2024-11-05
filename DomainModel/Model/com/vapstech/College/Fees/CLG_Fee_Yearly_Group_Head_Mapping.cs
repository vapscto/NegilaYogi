using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_Yearly_Group_Head_Mapping")]
    public class CLG_Fee_Yearly_Group_Head_Mapping
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FYGHM_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long FMG_Id { get; set; }
        public long FMH_Id { get; set; }
        public long FMI_Id { get; set; }
        public string FYGHM_FineApplicableFlag { get; set; }
        public string FYGHM_Common_AmountFlag { get; set; }
        public string FYGHM_ActiveFlag { get; set; }
    }
}
