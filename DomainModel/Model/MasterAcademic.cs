using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Adm_School_M_Academic_Year")]
    public class MasterAcademic : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ASMAY_Id { get; set; }
        [ForeignKey("MI_Id")]
        public long MI_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public DateTime? ASMAY_From_Date { get; set; }
        public DateTime? ASMAY_To_Date { get; set; }
        public DateTime? ASMAY_PreAdm_F_Date { get; set; }
        public DateTime? ASMAY_PreAdm_T_Date { get; set; }
        public int ASMAY_Order { get; set; }
        public int ASMAY_ActiveFlag { get; set; }
        public DateTime? ASMAY_Cut_Of_Date { get; set; }
        public int ASMAY_Pre_ActiveFlag { get; set; }
        public bool Is_Active { get; set; }
        public bool? ASMAY_ReggularFlg { get; set; }
        public bool? ASMAY_NewFlg { get; set; }
        public bool? ASMAY_NewAdmissionFlg { get; set; }
        public DateTime? ASMAY_TransportSDate { get; set; }
        public DateTime? ASMAY_TransportEDate { get; set; }
        public long? ASMAY_CreatedBy { get; set; }
        public long? ASMAY_UpdatedBy { get; set; }
        public DateTime? ASMAY_ReferenceDate { get; set; }
        public string ASMAY_AcademicYearCode { get; set; }
        public DateTime? ASMAY_AdvanceFeeDate { get; set; }
        public DateTime?  ASMAY_RegularFeeFDate { get; set; }
        public DateTime? ASMAY_RegularFeeTDate { get; set; }
        public DateTime? ASMAY_ArrearFeeDate { get; set; }

    }
}
