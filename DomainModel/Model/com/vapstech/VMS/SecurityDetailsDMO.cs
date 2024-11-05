using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.FrontOffice
{
    [Table("ISM_Security_Details")]
   public class SecurityDetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMSD_Id { get; set; }
        public string ISMSD_FirstName { get; set;}
        public string ISMSD_MiddleName { get; set; }
        public string ISMSD_LastName { get; set; }
        public long ISMSD_MobileNo { get; set; }
        public string ISMSD_EmailId { get; set; }
        public string ISMSD_PerStreet { get; set; }
        public string ISMSD_PerArea { get; set; }
        public string ISMSD_PerCity { get; set; }
        public long ISMSD_PerPincode { get; set; }
        public long IVRMMC_Id { get; set; }
        public long IVRMMS_Id { get; set; }
        public string ISMSD_Photo { get; set; }
        public long ISMSD_Aadhar { get; set; }
        public string ISMSD_Designation { get; set; }
        public string ISMSD_PANNo { get; set; }
        public bool ISMSD_LeftFlag { get; set; }
        public bool ISMSD_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long ISMSD_CreatedBy { get; set; }
        public long ISMSD_UpdatedBy { get; set; }
    }
}
