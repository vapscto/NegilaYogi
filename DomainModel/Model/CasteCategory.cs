using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_Master_Caste_Category")]
    public class CasteCategory : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IMCC_Id { get; set; }
        public string IMCC_CategoryName { get; set; }
        public string IMCC_CategoryCode { get; set; }
        public string IMCC_CategoryDesc { get; set; }
    }
}
