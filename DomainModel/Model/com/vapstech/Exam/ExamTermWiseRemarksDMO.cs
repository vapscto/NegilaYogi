﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Exam
{
    [Table("EXM_CCE_TERMS_Remarks", Schema = "Exm")]
    public class ExamTermWiseRemarksDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ECTERE_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public int ECT_Id { get; set; }
        public long AMST_Id { get; set; }
        public string ECTERE_Remarks { get; set; }
        public bool ECTERE_ActiveFlag { get; set; }
        public string ECTERE_Indi_OverAllFlag { get; set; }
        public long? ECTERE_CreatedBy { get; set; }
        public long? ECTERE_UpdatedBy { get; set; }
        public string ECTERE_Conduct { get; set; }
    }
}