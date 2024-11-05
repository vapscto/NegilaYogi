using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Master_Quarter")]
    public class HR_Master_Quarter 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long HRMQ_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMQ_QuarterName { get; set; }
       public DateTime? HRMQ_FromDay { get; set; }
       public string HRMQ_FromMonth  { get; set; }
       public DateTime? HRMQ_ToDay  { get; set; }
       public string HRMQ_ToMonth { get; set; }
        public bool HRMQ_ActiveFlg { get; set; }

       public long HRMQ_CreatedBy { get; set; }
       public long HRMQ_UpdatedBy { get; set; }


    }
}
