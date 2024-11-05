using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_Master_Rack_Subjects", Schema ="LIB")]
   public class Lib_Rack_SubjectDMO:CommonParamDMO
    {
      [Key]
      public long LMRAS_Id { get; set; }
      public long LMRA_Id { get; set; }
      public long LMS_Id { get; set; }
      public bool LMRAS_ActiveFlg { get; set; }


    }
}
