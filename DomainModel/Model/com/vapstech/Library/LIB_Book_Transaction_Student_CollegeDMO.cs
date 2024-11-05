using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_Book_Transaction_Student_College", Schema ="LIB")]
   public class LIB_Book_Transaction_Student_CollegeDMO : CommonParamDMO
    {
        [Key]
       
        public long LBTRS_Id { get; set; }
        public long  LBTR_Id { get; set; }
        public long AMCST_Id { get; set; }
        public bool LBTRS_ActiveFlg { get; set; }

    }
}
