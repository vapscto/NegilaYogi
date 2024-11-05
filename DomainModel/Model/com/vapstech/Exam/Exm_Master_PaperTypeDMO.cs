using DomainModel.Model.com.vapstech.Exam;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Exam
{
    [Table("Exm_Master_PaperType", Schema = "Exm")]
    public class Exm_Master_PaperTypeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int EMPATY_Id { get; set; }
        public long MI_Id { get; set; }
        public string EMPATY_PaperTypeName { get; set; }
        public string EMPATY_Color { get; set; }
        public string EMPATY_PaperTypeDescription { get; set; }
        public DateTime? EMPATY_CreatedDate { get; set; }
        public long? EMPATY_CreatedBy { get; set; }
        public DateTime? EMPATY_UpdatedDate { get; set; }
        public long? EMPATY_UpdatedBy { get; set; }
        public bool? EMPATY_ActiveFlag { get; set; }
    }
}
