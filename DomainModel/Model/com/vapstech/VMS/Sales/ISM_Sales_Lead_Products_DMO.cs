using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_Sales_Lead_Products")]
    public class ISM_Sales_Lead_Products_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMSLEPR_Id { get; set; }
        public long MI_Id { get; set; }
        public long ISMSLE_Id { get; set; }
        public long ISMSMPR_Id { get; set; }
        public bool ISMSLEPR_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long ISMSLEPR_CreatedBy { get; set; }
        public long ISMSLEPR_UpdatedBy { get; set; }
    }
}
