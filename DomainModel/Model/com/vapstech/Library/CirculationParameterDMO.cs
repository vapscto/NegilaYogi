using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("Lib_M_Circulation_Parameters", Schema ="LIB")]
    public class CirculationParameterDMO : CommonParamDMO
    {
        [Key]
        public long Circ_Id { get; set; }
        public long MI_Id { get; set; }
        public long LMC_Id { get; set; }
        public string CM_Id { get; set; }
        public int Max_Issue_Items { get; set; }
        public int Max_Issue_Days { get; set; }
        public int Max_No_Renewals { get; set; }
        public string Circ_Flag { get; set; }
        public bool Circ_ActiveFlag { get; set; }

    }
}
