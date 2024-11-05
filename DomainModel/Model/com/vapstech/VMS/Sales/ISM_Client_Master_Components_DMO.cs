using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_Client_Master_Components")]
    public class ISM_Client_Master_Components_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMCLTC_Id { get; set; }
        public long MI_Id { get; set; }
        public string ISMCLTC_Name { get; set; }
        public string ISMCLTC_Description { get; set; }
        public bool ISMCLTC_ActiveFlag { get; set; }
        public long ISMCLTC_CreatedBy { get; set; }
        public long ISMCLTC_UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
