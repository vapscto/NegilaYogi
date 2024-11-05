using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.SeatingArrangment
{
    [Table("Exam_SA_Room")]
    public class Exam_SA_RoomDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ESAROOM_Id { get; set; }
        public long MI_Id { get; set; }
        public long ESABLD_Id { get; set; }
        public long ESAFLR_Id { get; set; }
        public string ESAROOM_RoomName { get; set; }
        public string ESAROOM_RoomDesc { get; set; }
        public long? ESAROOM_RoomMaxCapacity { get; set; }
        public string ESAROOM_RoomTypeFlg { get; set; }
        public long ESAROOM_MaxNoOfRows { get; set; }
        public long ESAROOM_MaxNoOfColumns { get; set; }
        public long ESAROOM_BenchCapacity { get; set; }
        public bool ESAROOM_ActiveFlg { get; set; }
        public DateTime ESAROOM_CreatedDate { get; set; }
        public DateTime ESAROOM_UpdatedDate { get; set; }
        public long ESAROOM_CreatedBy { get; set; }
        public long ESAROOM_UpdatedBy { get; set; }
    }
}
