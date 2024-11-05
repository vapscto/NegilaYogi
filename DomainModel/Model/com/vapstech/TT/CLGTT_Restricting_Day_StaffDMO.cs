﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Restricting_Day_Staff_CourseBranch")]
    public class CLGTT_Restricting_Day_StaffDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TTRPSCB_Id { get; set; }
        public long TTRDS_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long ISMS_Id { get; set; }
        public bool TTRPSCB_ActiveFlag { get; set; }
        public long TTRPSCB_Periods { get; set; }
    }
}
