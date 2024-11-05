using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Yearly_Class_Category_Classes")]
    public class MasterYearlyClassCategoryClassDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FYCCC_Id { get; set; }
        public long FYCC_Id { get; set; }
        public long ASMCL_Id { get; set; }
      
    }
}
