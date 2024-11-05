using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_Book_Transaction_Staff", Schema ="LIB")]
   public class LIB_Book_Transaction_StaffDMO : CommonParamDMO
    {
        [Key]
        public long LBTRST_Id { get; set; }
        public long LBTR_Id { get; set; }
        public long HRME_Id { get; set; }
        public bool LBTRST_ActiveFlg { get; set; }
        
    }
}
