using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_M_IC_Student_College", Schema = "INV")]
    public class NV_M_ItemConsumptionCLGDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long INVMICSC_Id { get; set; }
        public long INVMIC_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long AMSE_Id { get; set; }
        public bool INVMICS_ActiveFlg { get; set; }
        public DateTime INVMICS_CreatedDate { get; set; }
        public DateTime INVMICS_UpdatedDate { get; set; }
        public long INVMICSC_UpdatedBy { get; set; }
        public long INVMICSC_CreatedBy { get; set; }

    }
}
