using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_632_FinanceSupport")]

    public class NAAC_AC_632_FinanceSupport_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC632FINSUP_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC632FINSUP_Year { get; set; }
        public string NCAC632FINSUP_TeacherName { get; set; }
        public string NCAC632FINSUP_Name { get; set; }
        public string NCAC632FINSUP_NameOfMembership { get; set; }
        public string NCAC632FINSUP_PAN { get; set; }
        public bool NCAC632FINSUP_ConferenceProfBodyFlg { get; set; }
        public Nullable<decimal> NCAC632FINSUP_AmountPaid { get; set; }
        public bool NCAC632FINSUP_ActiveFlg { get; set; }
        public long NCAC632FINSUP_CreatedBy { get; set; }
        public long NCAC632FINSUP_UpdatedBy { get; set; }
        public DateTime NCAC632FINSUP_CreatedDate { get; set; }
        public DateTime NCAC632FINSUP_UpdatedDate { get; set; }
        public string NCAC632FINSUP_StatusFlg { get; set; }
        public bool? NCAC632FINSUP_ApprovedFlg { get; set; }
        public string NCAC632FINSUP_Remarks { get; set; }

        public List<NAAC_AC_632_FinanceSupport_Files_DMO> NAAC_AC_632_FinanceSupport_Files_DMO { get; set; }
    }
}
