using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("IVRM_Table_AuditTrail")]
    public class IVRM_Table_AuditTrailDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ITAT_Id { get; set; }
        public long? IVRMUL_Id { get; set; }
        public long? MI_Id { get; set; }
        public string ITAT_TableName { get; set; }
        public string ITAT_Operation { get; set; }
        public DateTime ITAT_Date { get; set; }
        public TimeSpan ITAT_Time { get; set; }
        public string ITAT_IPV4Address { get; set; }        
        public string ITAT_IPV6Address { get; set; }
        public string ITAT_NetworkIp { get; set; }
        public string ITAT_MAACAddress { get; set; }
        public string ITAT_RecordPKID { get; set; }
    }
}
