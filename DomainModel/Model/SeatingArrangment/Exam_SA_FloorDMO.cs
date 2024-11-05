using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.SeatingArrangment
{
    [Table("Exam_SA_Floor")]
    public class Exam_SA_FloorDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ESAFLR_Id { get; set; }
        public long MI_Id { get; set; }
        public long ESABLD_Id { get; set; }
        public string ESAFLR_FloorName { get; set; }
        public string ESAFLR_FloorDesc { get; set; }
        public bool ESAFLR_ActiveFlg { get; set; }
        public DateTime ESAFLR_CreatedDate { get; set; }
        public DateTime ESAFLR_UpdatedDate { get; set; }
        public long ESAFLR_CreatedBy { get; set; }
        public long ESAFLR_UpdatedBy { get; set; }
    }
}