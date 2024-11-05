using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.admission
{
    [Table("IVRM_Religion_CasteCategory")]
    public class ReligionCategory_MappingDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IRCC_Id { get; set; }
        public long IVRMMR_Id { get; set; }
        public long IMCC_Id { get; set; }
        public bool IRCC_ActiveFlg { get; set; }
        public long IRCC_CreatedBy { get; set; }
        public long IRCC_UpdatedBy { get; set; }
        public DateTime? IRCC_CreatedDate { get; set; }
        public DateTime? IRCC_UpdatedDate { get; set; }
    }
}
