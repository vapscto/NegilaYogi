using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria8
{
    [Table("NAAC_DC_8111_Expenditure_File_Comments")]
  public  class DC_8111_Expenditure_File_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCDC8111EFC_Id { get; set; }      
        public long NCDC8111EFC_RemarksBy { get; set; }
        public long NCDC8111EFC_CreatedBy { get; set; }
        public long NCDC8111EFC_UpdatedBy { get; set; }
        public long NCDC8111EF_Id { get; set; }
        public string NCDC8111EFC_Remarks { get; set; }
        public string NCDC8111EFC_StatusFlg { get; set; }
        public bool NCDC8111EFC_ActiveFlag { get; set; }
        public DateTime? NCDC8111EFC_CreatedDate { get; set; }
        public DateTime? NCDC8111EFC_UpdatedDate { get; set; }
    }
}
