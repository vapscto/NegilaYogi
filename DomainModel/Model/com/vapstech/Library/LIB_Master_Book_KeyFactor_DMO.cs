using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_Master_Book_KeyFactor", Schema = "LIB")]
    public class LIB_Master_Book_KeyFactor_DMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LMBKF_Id { get; set; }
        public long LMB_Id { get; set; }
        public string LMBKF_KeyFactor { get; set; }
        public int? LMBKF_PageNo { get; set; }
        public bool LMBKF_ActiveFlg { get; set; }
        public long CreatedBy{get;set;}
       public long UpdatedBy { get; set; }

    }
}
