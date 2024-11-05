using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_IPR_322")]
    public class NAAC_AC_IPR_322_DMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long NCACIPR322_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime NCACIPR322_EstablishmentDate { get; set; }
        public long NCACIPR322_Year { get; set; }
        public string NCACIPR322_WorkshopName { get; set; }
        public DateTime? NCACIPR322_FromDate { get; set; }
        public DateTime? NCACIPR322_ToDate { get; set; }
        public string NCACIPR322_WebisteLink { get; set; }
        public bool NCACIPR322_ActiveFlg { get; set; }
        public long NCACIPR322_CreatedBy { get; set; }
        public long NCACIPR322_UpdatedBy { get; set; }
        public DateTime NCACIPR322_CreatedDate { get; set; }
        public DateTime NCACIPR322_UpdatedDate { get; set; }
        public long NCACIPR322_NoOfParticipants { get; set; }
        public string NCACIPR322_NameOfThePrincipalInvst { get; set; }
        public string NCACIPR322_DeptOfPrincialInvst { get; set; }
        public string NCACIPR322_StatusFlg { get; set; }
        public List<NAAC_AC_IPR_322_Files_DMO> NAAC_AC_IPR_322_Files_DMO { get; set; }
        
    }
}
