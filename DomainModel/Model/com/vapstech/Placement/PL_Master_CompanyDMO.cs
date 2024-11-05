using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Placement
{
    [Table("PL_Master_Company")]
    public class PL_Master_CompanyDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PLMCOMP_Id { get; set; }
        public long MI_Id { get; set; }
        public string PLMCOMP_CompanyName { get; set; }
        public string PLMCOMP_CompanyAddress { get; set; }
        public string PLMCOMP_Website { get; set; }
        public string PLMCOMP_FacilityFilePath { get; set; }
        public bool PLMCOMP_ActiveFlag { get; set; }
        public DateTime? PLMCOMP_CreatedDate { get; set; }
        public DateTime? PLMCOMP_UpdatedDate { get; set; }
        public  long PLMCOMP_CreatedBy { get; set; }
        public  long PLMCOMP_UpdatedBy { get; set; }
    }
}
