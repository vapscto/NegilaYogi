using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_Master_Country")]
    public class Country : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMMC_Id { get; set; }
        public string IVRMMC_CountryName { get; set; }
        public string IVRMMC_CountryCode { get; set; }
        public int IVRMMC_MobileNoLength { get; set; }
        public int IVRMMC_Default { get; set; }
        public string IVRMMC_Currency { get; set; }
        public string IVRMMC_CountryPhCode { get; set; }
        public string IVRMMC_Nationality { get; set; }
        public bool? IVRMMC_ActiveFlag { get; set; }
        public long? IVRMMC_CreatedBy { get; set; }
        public long? IVRMMC_UpdatedBy { get; set; }

    }
}
