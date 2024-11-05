﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
  [Table("HR_Master_PAN")]
  public class HRMasterPAN : CommonParamDMO
    {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long HRMPN_Id { get; set; }
    public long MI_Id { get; set; }
    public string HRME_PAN { get; set; }
    public string HRME_TPAN { get; set; }
        public bool HRMPN_ActiveFlag { get; set; }

    }
}