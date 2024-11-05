using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_Sales_Master_Status")]
    public class ISM_Sales_Master_Status_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMSMST_Id { get; set; }
        public long MI_Id { get; set; }
        public string ISMSMST_StatusName { get; set; }
        public string ISMSMST_Remarks { get; set; }
        public bool ISMSMST_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long ISMSMST_CreatedBy { get; set; }
        public long ISMSMST_UpdatedBy { get; set; }
    }
}
