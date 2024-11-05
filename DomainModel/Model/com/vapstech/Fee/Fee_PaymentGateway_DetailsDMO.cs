using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("Fee_PaymentGateway_Details")]
    public class Fee_PaymentGateway_DetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FPGD_Id { get; set; }
        public long MI_Id { get; set; }
        public string FPGD_MerchantId { get; set; }

        public string FPGD_SaltKey { get; set; }
        public string FPGD_AuthorisationKey { get; set; }
        public string FPGD_URL { get; set; }
        public string FPGD_SubMerchantId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public string FPGD_PGName { get; set; }
        public string FPGD_Image { get; set; }
        public string FPGD_PGActiveFlag { get; set; }
        public string FPGD_AuthorizationHeader { get; set; }
        public string FPGD_SubMerchantKey { get; set; }

        public long User_id { get; set; }

        public long IMPG_Id { get; set; }

        public bool FPGD_MobileActiveFlag { get; set; }
        public string FPGD_AccNo { get; set; }
    }
}
