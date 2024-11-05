using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_NonBook_Transaction_Staff", Schema = "LIB")]
    public class LIB_NonBook_Transaction_Staff_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LNBTRST_Id { get; set; }
        public long LNBTR_Id { get; set; }
        public long HRME_Id { get; set; }
        public bool LNBTRST_ActiveFlg { get; set; }

    }
}
