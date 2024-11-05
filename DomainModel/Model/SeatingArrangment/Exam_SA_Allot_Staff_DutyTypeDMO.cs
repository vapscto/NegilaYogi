using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.SeatingArrangment
{
    [Table("Exam_SA_Allot_Staff_DutyType")]
    public class Exam_SA_Allot_Staff_DutyTypeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ESAALLSTADTP_Id { get; set; }
        public long MI_Id { get; set; }
        public string ESAALLSTADTP_DTName { get; set; }
        public bool ESAALLSTADTP_ActiveFlag { get; set; }
        public DateTime ESAALLSTADTP_CreatedDate { get; set; }
        public DateTime ESAALLSTADTP_UpdatedDate { get; set; }
        public long ESAALLSTADTP_CreatedBy { get; set; }
        public long ESAALLSTADTP_UpdatedBy { get; set; }
    }
}
