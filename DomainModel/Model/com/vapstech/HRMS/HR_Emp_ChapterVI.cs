using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Employee_ChapterVIA")]
    public class HR_Emp_ChapterVI
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRECVIA_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long IMFY_Id { get; set; }
        public long HRMCVIA_Id { get; set; }
       // public string HRECVIA_SectionName  { get; set; }
        public decimal? HRECVIA_Amount{ get; set; }
        public bool HRECVIA_ActiveFlg  { get; set; }
        public long HRECVIA_CreatedBy { get; set; }
        public long HRECVIA_UpdatedBy { get; set; }
    }
}
