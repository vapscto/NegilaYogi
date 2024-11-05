using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Master_Hirer", Schema = "TRN")]
    public class MasterHirerDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRMH_Id { get; set; }
        public long MI_Id { get; set; }
        public long TRHG_Id { get; set; }
        public string TRMH_HirerName { get; set; }
        public string TRMH_ConatctPerName { get; set; }
        public string TRMH_ContactPersonDesg { get; set; }
        public long TRMH_ContactNo { get; set; }
        public long TRMH_MobileNo { get; set; }
        public string TRMH_EmailId { get; set; }
        public string TRMH_Address { get; set; }
        public bool TRMH_ActiveFlg { get; set; }
    }
}
