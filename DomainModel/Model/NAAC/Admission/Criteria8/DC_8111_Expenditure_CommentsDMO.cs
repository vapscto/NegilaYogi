using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria8
{
    [Table("NAAC_DC_8111_Expenditure_Comments")]
   public class DC_8111_Expenditure_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long NCDC8111EC_Id { get; set; }
        public long NCDC8111EC_RemarksBy { get; set; }
        public long NCDC8111EC_CreatedBy { get; set; }
        public long NCDC8111EC_UpdatedBy { get; set; }
        public long NCDC8111E_Id { get; set; }
        public string NCDC8111EC_Remarks { get; set; }
        public string NCDC8111EC_StatusFlg { get; set; }
        public bool NCDC8111EC_ActiveFlag { get; set; }
        public DateTime? NCDC8111EC_CreatedDate { get; set; }
        public DateTime? NCDC8111EC_UpdatedDate { get; set; }
    }
}
