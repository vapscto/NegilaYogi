using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_Student_EnablePartialPayment_College", Schema = "CLG")]
    public class Fee_Student_EnablePartialPayment_CollegeDMO
    {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

            public long FSEPPC_Id { get; set; }
            public long MI_Id { get; set; }
            public long AMCST_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public string FSEPPC_Remarks { get; set; }
            public DateTime? FSEPPC_RemarksDate { get; set; }
            public bool FSEPPC_ActiveFlag { get; set; }
            public DateTime? FSEPPC_CreatedDate { get; set; }
            public long? FSEPPC_CreatedBy { get; set; }
            public long? FSEPPC_UpdatedBy { get; set; }

        }
    }
