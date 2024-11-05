using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.SeatingArrangment
{
    [Table("Exam_SA_ChiefCoordinator")]
    public class Exam_SA_ChiefCoordinatorDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ESACHCRD_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public int EME_Id { get; set; }
        public long ESAUE_Id { get; set; }
        public string ESACHCRD_ChiefCordName { get; set; }
        public string ESACHCRD_Add1 { get; set; }
        public string ESACHCRD_Add2 { get; set; }
        public string ESACHCRD_Add3 { get; set; }
        public string ESACHCRD_Add4 { get; set; }
        public string ESACHCRD_Add5 { get; set; }
        public string ESACHCRD_Add6 { get; set; }
        public bool ESACHCRD_ActiveFlg { get; set; }
        public DateTime? ESACHCRD_CreatedDate { get; set; }
        public DateTime? ESACHCRD_UpdatedDate { get; set; }
        public long ESACHCRD_CreatedBy { get; set; }
        public long ESACHCRD_UpdatedBy { get; set; }
        
    }
}
