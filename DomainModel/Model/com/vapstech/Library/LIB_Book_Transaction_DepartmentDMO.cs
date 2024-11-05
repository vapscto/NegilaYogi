using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_Book_Transaction_Department", Schema ="LIB")]
   public class LIB_Book_Transaction_DepartmentDMO : CommonParamDMO
    {
        [Key]
        public long LBTRD_Id { get; set; }
        public long LBTR_Id { get; set; }
        public long HRMD_Id { get; set; }
        public bool LBTRD_ActiveFlg { get; set; }
        
    }
}
