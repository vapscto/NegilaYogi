using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_TParticipation_113_Comments")]
    public class NAAC_AC_TParticipation_113_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCACTP113C_Id { get; set; }
        public string NCACTP113C_Remarks { get; set; }
        public long NCACTP113C_RemarksBy { get; set; }
        public string NCACTP113C_StatusFlg { get; set; }
        public bool NCACTP113C_ActiveFlag { get; set; }
        public long NCACTP113C_CreatedBy { get; set; }
        public DateTime NCACTP113C_CreatedDate { get; set; }
        public long NCACTP113C_UpdatedBy { get; set; }
        public DateTime NCACTP113C_UpdatedDate { get; set; }
        public long NCACTP113_Id { get; set; }
    }
}
