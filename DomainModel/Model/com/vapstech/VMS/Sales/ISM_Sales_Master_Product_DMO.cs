using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_Sales_Master_Product")]
    public class ISM_Sales_Master_Product_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMSMPR_Id { get; set; }
        public long MI_Id { get; set; }
        public string ISMSMPR_ProductName { get; set; }
        public string ISMSMPR_Remarks { get; set; }
        public bool ISMSMPR_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long ISMSMPR_CreatedBy { get; set; }
        public long ISMSMPR_UpdatedBy { get; set; }
    }
}
