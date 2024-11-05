using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Placement
{
    [Table("PL_Master_Company_Contact")]
    public class PL_Master_Company_ContactDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PLMCOMPCON_Id { get; set; }
        public long PLMCOMP_Id { get; set; }
        public string PLMCOMPCON_ContactPersonName { get; set; }
        public string PLMCOMPCON_Designation { get; set; }
        public string PLMCOMPCON_EmailId { get; set; }
        public long PLMCOMPCON_ContactNo { get; set; }
        public bool PLMCOMPCON_ActiveFlag { get; set; }
        public DateTime? PLMCOMPCON_CreatedDate { get; set; }
        public DateTime? PLMCOMPCON_UpdatedDate { get; set; }
        public  long PLMCOMPCON_CreatedBy { get; set; }
        public  long PLMCOMPCON_UpdatedBy { get; set; }
    }
}
