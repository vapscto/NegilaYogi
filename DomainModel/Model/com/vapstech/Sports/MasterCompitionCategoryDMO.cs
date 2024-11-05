using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Sports
{
    [Table("SPCC_Master_Compition_Category", Schema ="SPC")]
    public class MasterCompitionCategoryDMO:CommonParamDMO
    {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SPCCMCC_Id { get; set; }
        public long MI_Id { get; set; }
        public string SPCCMCC_CompitionCategory { get; set; }
        public string SPCCMCC_CCDesc { get; set; }
        public bool SPCCMCC_CCAgeFlag { get; set; }
        public long SPCCMCC_FromCCAgeYear { get; set; }
        public long SPCCMCC_FromCCAgeMonth { get; set; }
        public long SPCCMCC_FromCCAgeDays { get; set; }
        public bool SPCCMCC_CCWeightFlag { get; set; }
        public decimal SPCCMCC_CCWeight { get; set; }
        public bool SPCCMCC_ActiveFlag { get; set; }
        public long SPCCMCC_ToCCAgeYear { get; set; }
        public long SPCCMCC_ToCCAgeMonth { get; set; }
        public long SPCCMCC_ToCCAgeDays { get; set; }



    }
}
