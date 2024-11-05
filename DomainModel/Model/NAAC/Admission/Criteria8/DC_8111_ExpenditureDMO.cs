using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria8
{
    [Table("NAAC_DC_8111_Expenditure")]
    public  class DC_8111_ExpenditureDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCDC8111E_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCDC8111E_Year { get; set; }
        public decimal NCDC8111E_Expenditure { get; set; }
        public string NCDC8111E_DentalMaterialsName { get; set; }
        public string NCDC8111E_StatusFlg { get; set; }
        public DateTime? NCDC8111E_CreatedDate { get; set; }
        public DateTime? NCDC8111E_UpdatedDate { get; set; }
        public long NCDC8111E_CreatedBy { get; set; }
        public long NCDC8111E_UpdatedBy { get; set; }
        public bool NCDC8111E_ActiveFlag { get; set; }
        public List<DC_8111_ExpenditureFilesDMO> DC_8111_ExpenditureFilesDMO { get; set; }
    }
}
