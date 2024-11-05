﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Final_Generation_Detailed")]
    public class TT_Final_Generation_DetailedDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TTFGD_Id { get; set; }
        public long TTFG_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ISMS_Id { get; set; }
      //  public decimal TTMD_Id { get; set; }
      //  public decimal TTMP_Id { get; set; }
       public long TTMD_Id { get; set; }
        public int TTMP_Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}
