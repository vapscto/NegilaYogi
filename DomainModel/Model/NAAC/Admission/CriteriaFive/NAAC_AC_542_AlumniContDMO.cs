using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_542_AlumniCont")]
    public class NAAC_AC_542_AlumniContDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC542ALMCON_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC542ALMCON_ContributionYear { get; set; }
        public string NCAC542ALMCON_AlumnsName { get; set; }
        public long NCAC542ALMCON_GraduationYear { get; set; }
        public string NCAC542ALMCON_AadharPAN { get; set; }
        public string NCAC542ALMCON_StatusFlg { get; set; }
        public decimal NCAC542ALMCON_ContriAmount { get; set; }
        public bool NCAC542ALMCON_ActiveFlg { get; set; }
        public long NCAC542ALMCON_CreatedBy { get; set; }
        public long NCAC542ALMCON_UpdatedBy { get; set; }
        public DateTime NCAC542ALMCON_CreatedDate { get; set; }
        public DateTime NCAC542ALMCON_UpdatedDate { get; set; }
        public string NCAC542ALMCON_AreaOfContribution { get; set; }
        public bool NCAC531SPCAS_FinancialORKindFlag { get; set; }
        public bool NCAC531SPCAS_DonationOfBooksFlag { get; set; }
        public bool NCAC531SPCAS_StudentsplacementFlag { get; set; }
        public bool NCAC531SPCAS_StudentexchangesFlag { get; set; }
        public bool NCAC531SPCAS_InstendowmentsFlag { get; set; }

        public List<NAAC_AC_542_AlumniContFilesDMO> NAAC_AC_542_AlumniContFilesDMO { get; set; }


    }
}
