using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Fee.Tally
{
    [Table("Fee_FeeGroup_CompanyMapping")]
    public class Fee_FeeGroup_CompanyMappingDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FFGCMA_Id { get; set; }
        public long MI_Id { get; set; }
        public long FMG_Id { get; set; }
        public bool FFGCMA_ActiveId { get; set; }
        public long? FTMCOM_Id { get; set; }

    }
}
