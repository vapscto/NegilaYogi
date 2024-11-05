using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Fee.FinancialAccounting
{
    [Table("FA_Master_Group")]

    public class FAMaster_GroupDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FAMGRP_Id { get; set; }
        public long MI_Id { get; set; }
        public string FAMGRP_GroupName { get; set; }

        public string FAMGRP_GroupCode { get; set; }
        public string FAMGRP_Description { get; set; }
        public string FAMGRP_BSPLFlg { get; set; }
        public string FAMGRP_CRDRFlg { get; set; }
        public string FAMGRP_Position { get; set; }
        public long FAMGRP_ParentId { get; set; }
        public bool FAMGRP_ActiveFlg { get; set; }
        public DateTime? FAMGRP_CreatedDate { get; set; }
        public DateTime? FAMGRP_UpdatedDate { get; set; }
        public long FAMGRP_CreatedBy { get; set; }
        public long FAMGRP_UpdatedBy { get; set; }
    }
}
