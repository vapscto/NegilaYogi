using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Master_Organization")]
    public class Organisation : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MO_Id { get; set; }
        public string IVRMMCT_Name { get; set; }
        public long IVRMMS_Id { get; set; }
        public long IVRMMC_Id { get; set; }
        public string MO_Name { get; set; }
        public string MO_Address1 { get; set; }
        public string MO_Address2 { get; set; }
        public string MO_Address3 { get; set; }
        public string MO_Landmark { get; set; }
        public int MO_Pincode { get; set; }
        public string MO_FaxNo { get; set; }
        public string MO_Website { get; set; }
        public string MO_OrganisationType { get; set; }
        public int MO_ActiveFlag { get; set; }
        public string MT_Currency { get; set; }

        public string MT_Domain_name { get; set; }

        public string MO_AboutInstitute { get; set; }

        public string MO_ContactDetails { get; set; }

        public string MO_Logo { get; set; }


        //public  City IVRM_Master_City { get; set; }
        //public  Country IVRM_Master_Country { get; set; }
        //public State IVRM_Master_State { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Master_Institution> Master_Institution { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Master_Organization_EmailId> Master_Organization_EmailId { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Master_Organization_MobileNo> Master_Organization_MobileNo { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Master_Organization_PhoneNo> Master_Organization_PhoneNo { get; set; }

    }
}
