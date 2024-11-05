using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.admission
{
    [Table("Adm_StudentWise_Vaccine_Details")]
    public class Adm_StudentWise_Vaccine_DetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ASWVD_Id { get; set; }
        public long ASVACD_Id { get; set; }
        public long AMST_Id { get; set; }
        public DateTime? ASWVD_DateGiven { get; set; }
        public string ASWVD_AdministeredBy { get; set; }
        public DateTime? ASWVD_NextDoseDate { get; set; }
        public bool ASWVD_ActiveFlag { get; set; }
        public DateTime? ASWVD_CreatedDate { get; set; }
        public DateTime? ASWVD_UpdatedDate { get; set; }
    }
}
