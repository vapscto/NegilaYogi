using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.ClubManagement
{
    [Table("CMS_TransactionsType_Tax", Schema = "CMS")]

    public class CMS_TransactionsType_TaxDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CMSTRANSTYTAX_Id { get; set; }
        public long CMSTRANSTY_Id { get; set; }
        public string CMSTRANSTY_TaxNo { get; set; }
        public decimal CMSTRANSTYTAX_TaxPercent { get; set; }
        public bool CMSTRANSTYTAX_ActiveFlag { get; set; }
        public DateTime? CMSTRANSTYTAX_CreatedDate { get; set; }
        public long CMSTRANSTYTAX_CreatedBy { get; set; }
        public DateTime? CMSTRANSTYTAX_UpdatedDate { get; set; }
        public long CMSTRANSTYTAX_UpdatedBy { get; set; }
    }
}
