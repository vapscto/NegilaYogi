using DomainModel.Model.com.vapstech.admission;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Portals.Student
{
    [Table("IVRM_Interactions_Student")]
    public class IVRM_Interactions_StudentDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IINTS_Id { get; set; }
        public long MI_Id { get; set; }
        public string IINTS_InteractionId { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMST_Id { get; set; }
        public string IINTS_Subject { get; set; }
        public DateTime IINTS_Date { get; set; }
        public string IINTS_Interaction { get; set; }
        public long HRME_Id { get; set; }
        public string IINTS_Flag { get; set; }
        public bool IINTS_ActiveFlag { get; set; }
        public string IINTS_ComposeFlag { get; set; }
    }
}

