using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.FrontOffice
{
    [Table("FO_Master_Shifts", Schema = "FO")]
    public class MasterShiftsDMO : CommonParamDMO
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int FOMS_Id { get; set; }
        public long MI_Id { get; set; }
        public string FOMS_ShiftName { get; set; }
        public bool FOMS_ActiveFlg { get; set; }
        public MasterShiftsTimingsDMO Mstimings { get; set; }
    }
}     
     
















       
