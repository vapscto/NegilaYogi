using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Preadmission_School_HealthDetails")]
    public class StudentHelthcertificateDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PASHD_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime? PASHD_EntryDate { get; set; }
        public DateTime? PASHD_UpdateDate { get; set; }
        public long ASMAY_Id { get; set; }
        public long PASR_Id { get; set; }
        public DateTime? PASHD_VaccinationDate { get; set; }
        public int  PASHD_FitsFlag { get; set; }
        public DateTime? PASHD_FitsDate { get; set; }
        public int PASHD_Illness { get; set; }
        public DateTime? PASHD_HepatitisB { get; set; }
        public DateTime? PASHD_TyphoidFever { get; set; }
        public string PASHD_Ailment { get; set; }
        public string PASHD_Allergy { get; set; }
        public string PASHD_HealthProblem { get; set; }
        public string PASHD_BloodGroup { get; set; }

        public int PASHD_ChickenpoxFlag { get; set; }

        public DateTime? PASHD_ChickenpoxDate { get; set; }

        public int PASHD_DiptheriaFlag { get; set; }

        public DateTime? PASHD_DiptheriaDate { get; set; }

        public int PASHD_EpidemicFlag { get; set; }

        public DateTime? PASHD_EpidemicDate { get; set; }

        public int PASHD_MeaslesFlag { get; set; }

        public DateTime? PASHD_MeaslesDate { get; set; }

        public int PASHD_MumpsFlag { get; set; }

        public DateTime? PASHD_MumpsDate { get; set; }

        public int PASHD_RingwormFlag { get; set; }

        public DateTime? PASHD_RingwormDate { get; set; }

        public int PASHD_ScarletFlag { get; set; }

        public DateTime? PASHD_ScarletDate { get; set; }

        public int PASHD_SmallPoxFlag { get; set; }

        public DateTime? PASHD_SmallPoxDate { get; set; }

        public int PASHD_WhoopingFlag { get; set; }

        public DateTime? PASHD_WhoopingDate { get; set; }

        public bool PASHD_AllergyFlg  { get; set; }

        public string PASHD_CronicDesease { get; set; }

        public string PASHD_MEDetails { get; set; }

        public long PASHD_MEContactNo  { get; set; }

    }
}
