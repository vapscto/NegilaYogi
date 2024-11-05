using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Preadmission_Master_Status")]
    public class AdmissionStatus : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long PAMST_Id { get; set; }
        public long MI_Id { get; set; }
        public string PAMST_Status { get; set; }
        public string PAMST_StatusFlag { get; set; }

        public int? active { get; set; }
        //public DateTime? CreatedDate { get; set; }
        //public DateTime? UpdatedDate { get; set; }
        public long? PAMST_CreatedBy { get; set; }
        public long? PAMST_UpdatedBy { get; set; }
    }
}
