using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_TParticipation_113_File_Comments")]
    public class NAAC_AC_TParticipation_113_File_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCACTP113FC_Id { get; set; }
        public string NCACTP113FC_Remarks { get; set; }
        public long NCACTP113FC_RemarksBy { get; set; }
        public bool NCACTP113FC_ActiveFlag { get; set; }
        public long NCACTP113FC_CreatedBy { get; set; }
        public DateTime NCACTP113FC_CreatedDate { get; set; }
        public long NCACTP113FC_UpdatedBy { get; set; }
        public DateTime NCACTP113FC_UpdatedDate { get; set; }
        public string NCACTP113FC_StatusFlg { get; set; }
        public long NCACTP113F_Id { get; set; }
    }
}
