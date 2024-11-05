using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.FrontOffice
{
    [Table("ISM_Security_Roaster")]
    public class SecurityRoasterDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMSR_Id { get; set; }
        public long ISMSD_Id { get; set; }
        public DateTime ISMSR_Date { get; set; }
        public long HRMF_Id { get; set; }
        public string ISMSR_From { get; set; }
        public string ISMSR_To { get; set; }
        public string ISMSR_Status { get; set; }
        public long ISMSR_Userid { get; set; }
        public string ISMSR_Remarks { get; set; }
        public bool ISMSR_ActiveFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long ISMSR_CreatedBy { get; set; }
        public long ISMSR_UpdatedBy { get; set; }
    }
}
