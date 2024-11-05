using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("HR_Master_Employee_Update_Request_EmailId")]
    public class HR_Master_Employee_Update_Request_EmailIdDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMEREQEM_Id { get; set; }
        public long HRMEREQ_Id { get; set; }
        public string HRMEREQEM_EmailId { get; set; }
        public string HRMEREQEM_Flag { get; set; }
        public bool HRMEREQEM_ActiveFlg { get; set; }
    }
}
