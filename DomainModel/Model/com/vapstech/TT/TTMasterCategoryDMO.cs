using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Master_Category")]
    public class TTMasterCategoryDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TTMC_Id { get; set; }
       
        public long MI_Id { get; set; }
        public string TTMC_CategoryName { get; set; }             
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool TTMC_ActiveFlag { get; set; }
    }
}
