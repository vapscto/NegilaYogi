using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Fee.FinancialAccounting
{
    [Table("FA_User_Group")]
    public class FAUser_GroupDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FAUGRP_Id { get; set; }
        public long MI_Id { get; set; }
        public long FAMGRP_Id { get; set; }
        public long FAUGRP_ParentId { get; set; }
        public string FAUGRP_UserGroupName { get; set; }
        public string FAUGRP_AliasName { get; set; }
        public string FAUGRP_Description { get; set; }
        public long FAMCOMP_Id { get; set; }
        public long IMFY_Id { get; set; }
        public long FAUGRP_Position { get; set; }
        public bool FAUGRP_ActiveFlg { get; set; }
        public DateTime? FAUGRP_CreatedDate { get; set; }
        public DateTime? FAUGRP_UpdatedDate { get; set; }
        public long FAUGRP_CreatedBy { get; set; }
        public long FAUGRP_UpdatedBy { get; set; }
    }
}
