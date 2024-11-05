using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;



namespace DomainModel.Model.com.vapstech.FrontOffice
{
    [Table("Adm_Student_Punch_Details")]
    public class Adm_Student_Punch_DetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ASPUD_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASPU_Id { get; set; }
        public string ASPUD_PunchTime { get; set; }
        public string ASPUD_InOutFlg { get; set; }
        public long FOBD_Id { get; set; }
        public DateTime? ASPUD_CreatedDate { get; set; }
        public DateTime? ASPUD_UpdatedDate { get; set; }
        public long? ASPUD_CreatedBy { get; set; }
        public long? ASPUD_UpdatedBy { get; set; }
        public string ASPUD_Flg { get; set; }
       
    }
}     
     
















       
