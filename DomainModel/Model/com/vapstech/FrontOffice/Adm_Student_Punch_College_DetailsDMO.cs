using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.FrontOffice
{
    [Table("Adm_Student_Punch_College_Details", Schema = "CLG")]
    public  class Adm_Student_Punch_College_DetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ASPUDC_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASPUC_Id { get; set; }
        public string ASPUDC_PunchTime { get; set; }
        public string ASPUDC_InOutFlg { get; set; }
    
        public DateTime? ASPUDC_CreatedDate { get; set; }
        public DateTime? ASPUDC_UpdatedDate { get; set; }
        public long? ASPUDC_CreatedBy { get; set; }
        public long? ASPUDC_UpdatedBy { get; set; }
        public string ASPUDC_Flg { get; set; }
        public long FOBD_Id { get; set; }
    }
}
