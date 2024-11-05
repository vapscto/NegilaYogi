﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_NonBook_Transaction_Student", Schema = "LIB")]
    public class LIB_NonBook_Transaction_Student_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LNBTRS_Id { get; set; }
        public long LNBTR_Id { get; set; }
        public long AMST_Id { get; set; }
        public bool LNBTRS_ActiveFlg { get; set; }
    }
}
