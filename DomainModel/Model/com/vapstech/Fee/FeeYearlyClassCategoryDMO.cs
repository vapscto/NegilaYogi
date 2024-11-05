using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Yearly_Class_Category")]
    public class FeeYearlyClassCategoryDMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FYCC_Id { get; set; }
        public long MI_Id { get; set; }
        public long FMCC_Id { get; set; }      
        public long ASMAY_Id { get; set; }
        public bool FYCC_ActiveFlag { get; set; }

        public long? FYCC_CreatedBy { get; set; }
        public long? FYCC_UpdatedBy { get; set; }
    }
}
  

