using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.admission
{
    [Table("Adm_Study_Certificate_Report")]
    public class StudycertificateReportDMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ASC_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime ASC_Date { get; set; }
        public int ASC_No { get; set; }
        public string ASC_ReportType { get; set; }
        public long? ASC_Createdby { get; set; }
        public long? ASC_Updatedby { get; set; }
    }
}
