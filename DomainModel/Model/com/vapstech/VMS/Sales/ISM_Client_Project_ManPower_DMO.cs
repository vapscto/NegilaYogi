using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_Client_Project_ManPower")]
    public class ISM_Client_Project_ManPower_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMCLTPRMP_Id { get; set; }
        public long ISMMCLTPR_Id { get; set; }
        public string ISMCLTPRMP_ResourceName { get; set; }
        public decimal ISMCLTPRMP_Qty { get; set; }
        public string ISMCLTPRMP_Remarks { get; set; }
        public bool ISMCLTPRMP_ActiveFlag { get; set; }
        public long ISMCLTPRMP_CreatedBy { get; set; }
        public long ISMCLTPRMP_UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
