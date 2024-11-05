﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.admission
{
    [Table("Adm_Master_MotherEmail_Id")]
    public class Adm_M_Mother_Emailid:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AMSTMEMAIL_Id { get; set; }
        public long MI_Id { get; set; }
        [ForeignKey("AMST_Id")]
        public long AMST_Id { get; set; }
        public string AMST_MotheremailId { get; set; }
        public long? AMSTMEMAIL_CreatedBy { get; set; }
        public long? AMSTMEMAIL_UpdatedBy { get; set; }
    }
}