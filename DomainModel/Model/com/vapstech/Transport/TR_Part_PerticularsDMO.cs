using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Part_Perticulars", Schema = "TRN")]
    public class TR_Part_PerticularsDMO: CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRPAP_Id { get; set; }
        public long MI_Id { get; set; }
        public long TRMV_Id { get; set; }
        public long TRMD_Id { get; set; }
        public long TRMSES_Id { get; set; }
        public string TRMSES_Parts { get; set; }
        public DateTime TRMSES_Date { get; set; }
        public bool TRMSESP_ActiveFlag { get; set; }
        public long TRPAPT_Id { get; set; }

    }
}
