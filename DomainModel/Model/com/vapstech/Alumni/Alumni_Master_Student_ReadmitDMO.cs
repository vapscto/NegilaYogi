using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Alumni
{
    [Table("Alumni_Master_Student_Readmit", Schema = "ALU")]
    public class Alumni_Master_Student_ReadmitDMO 
    {

        

            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

            public long? ALMSTRADM_Id { get; set; }
            public long ALMST_ID { get; set; }

            public long? ALMSTRADM_YearJoined { get; set; }
            public long? ALMSTRADM_ClassJoined { get; set; }
            public long? ALMSTRADM_YearLeft { get; set; }
            public long? ALMSTRADM_ClassLeft { get; set; }
            public bool ALMSTRADM_ActiveFlg { get; set; }
            public DateTime ALMSTRADM_CreatedDate { get; set; }
            public DateTime ALMSTRADM_UpdatredDate { get; set; }
            public long ALMSTRADM_CreatedBy { get; set; }
            public long ALMSTRADM_UpdatedBy { get; set; }
          
            //public List<Alumni_M_StudentDMO> Alumni_M_StudentDMO { get; set; }



        
    }
}
