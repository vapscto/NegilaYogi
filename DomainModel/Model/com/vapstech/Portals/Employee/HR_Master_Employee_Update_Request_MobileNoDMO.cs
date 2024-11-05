using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("HR_Master_Employee_Update_Request_MobileNo")]
    public class HR_Master_Employee_Update_Request_MobileNoDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMEREQMN_Id { get; set; }
        public long HRMEREQ_Id { get; set; }
        public long? HRMEREQMN_MobileNo { get; set; }
        public string HRMEREQMN_Flag { get; set; }
        public bool HRMEREQMN_ActiveFlg { get; set; }
    }
}
