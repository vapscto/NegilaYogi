using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("ISM_Master_TaskGroup")]
    public class ISM_Master_TaskGroupDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ISMMTGRP_Id { get; set; }
        public long MI_Id { get; set; }
        public long? ISMMTGRP_CreatedByDeptId { get; set; }
        public string ISMMTGRP_TaskGroupName { get; set; }
        public string ISMMTGRP_TGRemarks { get; set; }
        public bool ISMMTGRP_TGFinishedFlg { get; set; }
        public bool ISMMTGRP_ActiveFlag { get; set; }
        public long? ISMMTGRP_CreatedBy { get; set; }
        public long? ISMMTGRP_UpdatedBy { get; set; }
        public DateTime? ISMMTGRP_CreatedDate { get; set; }
        public DateTime? ISMMTGRP_UpdatedDate { get; set; }

        public List<ISM_Master_TaskGroup_DeptDMO> ISM_Master_TaskGroup_DeptDMO { get; set; }

    }
}
