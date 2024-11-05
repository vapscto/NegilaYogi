using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.SeatingArrangment
{
    [Table("Exam_SA_Building")]
    public class Exam_SA_BuildingDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ESABLD_Id { get; set; }
        public long MI_Id { get; set; }
        public string ESABLD_BuildingName { get; set; }
        public string ESABLD_BuildingDesc { get; set; }
        public bool ESABLD_ActiveFlg { get; set; }
        public DateTime ESABLD_CreatedDate { get; set; }
        public DateTime ESABLD_UpdatedDate { get; set; }
        public long ESABLD_CreatedBy { get; set; }
        public long ESABLD_UpdatedBy { get; set; }
    }
}
