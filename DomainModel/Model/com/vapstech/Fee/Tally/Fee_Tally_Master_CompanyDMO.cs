using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Fee.Tally
{
    [Table("Fee_Tally_Master_Company")]
    public class Fee_Tally_Master_CompanyDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FTMCOM_Id { get; set; }
        public long MI_Id { get; set; }
        public string FTMCOM_CompanyName { get; set; }
        public string FTMCOM_CompanyCode { get; set; }
        public bool FTMCOM_ActiveId { get; set; }
        public DateTime? FTMCOM_CreatedDate { get; set; }
        public DateTime? FTMCOM_UpdatedDate { get; set; }
    }
}
