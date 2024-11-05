using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.ClubManagement
{
    [Table("CMS_Master_Department", Schema = "CMS")]
    public class CMS_MasterDepartmenDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long CMSMDEPT_Id { get; set; }
        public long MI_Id { get; set; }
        public string CMSMDEPT_DepartmentName { get; set; }
        public string CMSMDEPT_DeptCode { get; set; }
        public bool CMSMDEPT_ActiveFlag { get; set; }
        public DateTime? CMSMDEPT_CreatedDate { get; set; }

        public long CMSMDEPT_CreatedBy { get; set; }
        public DateTime? CMSMDEPT_UpdatedDate { get; set; }

        public long CMSMDEPT_UpdatedBy { get; set; }
    }
}
