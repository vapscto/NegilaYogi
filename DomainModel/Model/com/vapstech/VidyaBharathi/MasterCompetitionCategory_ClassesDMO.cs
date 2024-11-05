using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VidyaBharathi
{
    [Table("VBSC_Master_Competition_Category_Classes")]
    public class MasterCompetitionCategory_ClassesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long VBSCMCCCL_Id { get; set; }
        public long VBSCMCC_Id { get; set; }
        public long ASMCL_ID { get; set; }
        public bool VBSCMCC_ActiveFlag { get; set; }
        public DateTime? VBSCMCC_CreatedDate { get; set; }
        public DateTime? VBSCMCC_UpdatedDate { get; set; }
        public long VBSCMCC_CreatedBy { get; set; }
        public long VBSCMCC_UpdatedBy { get; set; }
    }
}
