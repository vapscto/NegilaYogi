using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.ClubManagement
{
    [Table("CMS_Transaction_Tax", Schema = "CMS")]

    public class CMS_Transaction_TaxDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CMSTRANSTAX_Id { get; set; }
        public long CMSTRANSDET_Id { get; set; }
        public string CMSTRANSDET_TaxNo { get; set; }
        public decimal CMSTRANSTAX_TaxAmount { get; set; }
        public bool CMSTRANSTAX_ActiveFlg { get; set; }
        public DateTime? CMSTRANSTAX_CreatedDate { get; set; }
        public long CMSTRANSTAX_CreatedBy { get; set; }
        public DateTime? CMSTRANSTAX_UpdatedDate { get; set; }
        public long CMSTRANSTAX_UpdatedBy { get; set; }

    }
}
