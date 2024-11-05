using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_NonBook_Transaction_Student_College", Schema = "LIB")]
    public class LIB_NonBook_Transaction_Student_College_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public long LNBTRSC_Id { get; set; }
        public long LNBTR_Id { get; set; }
        public long AMCST_Id { get; set; }
        public bool LNBTRS_ActiveFlg { get; set; }


    }
}
