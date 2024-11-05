using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_Master_NonBook_KeyFactor", Schema = "LIB")]
    public class LIB_Master_NonBook_KeyFactor_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long LMNBKF_Id { get; set; }
        public long LMNBK_Id { get; set; }
        public string LMNBKF_KeyFactor { get; set; }
        public int? LMNBKF_PageNo { get; set; }
        public bool LMNBKF_ActiveFlg { get; set; }
        public long UpdatedBy { get; set; }
        public long CreatedBy { get; set; }
        

    }
}
