using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.ClubManagement
{
    [Table("CMS_TransactionsType_Installments", Schema = "CMS")]

    public class CMS_TransactionsType_InstallmentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CMSTRANSTYINT_Id { get; set; }
        public long CMSTRANSTY_Id { get; set; }
        public long CMSMINST_Id { get; set; }
        public decimal CMSTRANSTYINT_Amount { get; set; }
        public int CMSTRANSTYINT_InstlOrder { get; set; }
        public bool CMSTRANSTYINT_ActiveFlag { get; set; }
        public DateTime? CMSTRANSTYINT_CreatedDate { get; set; }
        public long CMSTRANSTYINT_CreatedBy { get; set; }
        public DateTime? CMSTRANSTYINT_UpdatedDate { get; set; }
        public long CMSTRANSTYINT_UpdatedBy { get; set; }
    }
}
