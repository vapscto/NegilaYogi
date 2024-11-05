using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Sports
{
    [Table("SPCC_Events_Students", Schema = "SPC")]
    public class SPCC_Events_Students_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SPCCEST_Id { get; set; }
        public long MI_Id { get; set; }
        public long SPCCME_Id { get; set; }
        public long SPCCMCL_Id { get; set; }
        public long SPCCMCC_Id { get; set; }
        public long SPCCMSCC_Id { get; set; }
        //public long? SPCCMH_Id { get; set; }
        public long SPCCMUOM_Id { get; set; }
        public string SPCCEST_House_Class_Flag { get; set; }
        public string SPCCEST_OldRecord { get; set; }
        public string SPCCEST_Remarks { get; set; }
        public bool SPCCEST_ActiveFlag { get; set; }
        public long SPCCMSCCG_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        //public long ASMS_Id { get; set; }
        // public bool spccestR_RecordBrokenFlag { get; set; }
        public DateTime? SPCCEST_StardDate { get; set; }
        public DateTime? SPCCEST_EndDate { get; set; }
        public long SPCCMEV_Id { get; set; }

        public List<EventsStudentRecordDMO> EventsStudentRecordDMO { get; set; }

    }
}

