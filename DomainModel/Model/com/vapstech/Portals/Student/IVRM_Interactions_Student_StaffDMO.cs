using DomainModel.Model.com.vapstech.admission;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Portals.Student
{
    [Table("IVRM_Interactions_Student_Staff")]
    public class IVRM_Interactions_Student_StaffDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IINTSS_Id { get; set; }
        public long IINTS_Id { get; set; }
        public string IINTSS_Interaction { get; set; }
        public DateTime IINTSS_Date { get; set; }
        public string IINTSS_ByFlg { get; set; }
        public bool IINTSS_ActiveFlag { get; set; }


    }
}

