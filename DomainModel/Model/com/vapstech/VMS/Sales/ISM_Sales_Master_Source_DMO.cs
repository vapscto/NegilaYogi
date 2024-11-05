using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_Sales_Master_Source")]
    public class ISM_Sales_Master_Source_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMSMSO_Id { get; set; }
        public long MI_Id { get; set; }
        public string ISMSMSO_SourceName { get; set; }
        public string ISMSMSO_Remarks { get; set; }
        public bool ISMSMSO_ActiveFlag { get; set; }
        public string ISMSMSO_Templet { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long ISMSMSO_CreatedBy { get; set; }
        public long ISMSMSO_UpdatedBy { get; set; }
     
    }
}
