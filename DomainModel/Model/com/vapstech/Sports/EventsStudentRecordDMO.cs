using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Sports
{
    [Table("SPCC_Events_Students_Record", Schema = "SPC")]
    public class EventsStudentRecordDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SPCCESTR_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public string SPCCESTR_Rank { get; set; }
        public double? SPCCESTR_Points { get; set; }
        public string SPCCESTR_RecordBrokenFlag { get; set; }
        public string SPCCESTR_Remarks { get; set; }
        public bool SPCCESTR_ActiveFlag { get; set; }
        public long SPCCEST_Id { get; set; }
        public string SPCCESTR_Value { get; set; }
        public long? SPCCMUOM_Id { get; set; }
        public bool? SPCCESTR_EventRecordFlg { get; set; }
    }
}
