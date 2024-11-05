using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Master_Class_Category")]
    public class FeeClassCategoryDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMCC_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMCC_ClassCategoryName { get; set; }
        public string FMCC_ClassCategoryCode { get; set; }
        public bool FMCC_ActiveFlag { get; set; }

        public long? FMCC_CreatedBy { get; set; }
        public long? FMCC_UpdatedBy { get; set; }

    }
}
