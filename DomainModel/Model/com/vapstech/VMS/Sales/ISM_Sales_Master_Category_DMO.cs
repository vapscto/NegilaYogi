using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_Sales_Master_Category")]
    public class ISM_Sales_Master_Category_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ISMSMCA_Id { get; set; }
        public long MI_Id { get; set; }
        public string ISMSMCA_CategoryName { get; set; }
        public string ISMSMCA_Remarks { get; set; }
        public bool ISMSMCA_ActiveFlag { get; set; }
        public DateTime? ISMSMCA_CreatedDate { get; set; }
        public DateTime? ISMSMCA_UpdatedDate { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }
    }
}
