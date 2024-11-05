using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("ISM_Master_TaskGroup_Dept")]
    public class ISM_Master_TaskGroup_DeptDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ISMMTGRPD_Id { get; set; }
        public long ISMMTGRP_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long ISMMTGRPD_CreatedBy { get; set; }
        public long? ISMMTGRPD_UpdatedBy { get; set; }
        public bool? ISMMTGRPD_ActiveFlag { get; set; }
        public DateTime? ISMMTGRPD_CreatedDate { get; set; }
        public DateTime? ISMMTGRPD_UpdatedDate { get; set; }
    }
}
