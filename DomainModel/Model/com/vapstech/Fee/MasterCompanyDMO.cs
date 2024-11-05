using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("MasterCompany")]
    public class MasterCompanyDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Cmp_Code { get; set; }
        public string CMP_Name { get; set; }
        public string CMP_Address { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string E_MailAddress { get; set; }
        public string IncomeTaxNo { get; set; }
        public long MI_Id { get; set; }
    }
}
