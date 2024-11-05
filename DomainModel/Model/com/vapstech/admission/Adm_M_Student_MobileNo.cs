using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.admission
{
    [Table("Adm_Master_Student_SMSNo")]
    public class Adm_M_Student_MobileNo :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AMSTSMS_Id { get; set; }
        [ForeignKey("AMST_Id")]
        public long AMST_Id { get; set; }
        public string AMSTSMS_MobileNo { get; set; }
        public long? AMSTSMS_CreatedBy { get; set; }
        public long? AMSTSMS_UpdatedBy { get; set; }
        public string AMSTSMS_CountryCode { get; set; }
    }
}
