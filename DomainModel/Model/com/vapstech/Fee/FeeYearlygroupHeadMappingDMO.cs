using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Yearly_Group_Head_Mapping")]
    public class FeeYearlygroupHeadMappingDMO
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

        public DateTime? FYGHM_CreatedDate { get; set; }
        public DateTime? FYGHM_UpdatedDate { get; set; }
        public long? FYGHM_CreatedBy { get; set; }
        public long? FYGHM_UpdatedBy { get; set; }

    }
}
