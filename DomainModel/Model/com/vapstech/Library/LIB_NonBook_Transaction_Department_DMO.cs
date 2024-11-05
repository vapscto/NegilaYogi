using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_NonBook_Transaction_Department", Schema = "LIB")]
    public class LIB_NonBook_Transaction_Department_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LNBTRD_Id { get; set; }
        public long LNBTR_Id { get; set; }
        public long HRMD_Id { get; set; }
        public bool LNBTRST_ActiveFlg { get; set; }

    }
}
