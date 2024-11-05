using DomainModel.Model.com.vapstech.admission;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Portals.Student
{
    [Table("STUDENT_PORTAL_DATA_UPDATE")]
    public class StudentDetailsupdateDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long STP_ID { get; set; }
        public long AMST_ID { get; set; }
        public string STP_SNAME { get; set; }
        public string STP_SEMAIL { get; set; }
        public long? STP_SMOBILENO { get; set; }
        public string STP_SBLOOD { get; set; }
        public string STP_SPHOTO { get; set; }
        public string STP_FNAME { get; set; }
        public string STP_FEMAIL { get; set; }
        public long? STP_FMOBILENO { get; set; }
        public string STP_FBLOOD { get; set; }
        public string STP_FPHOTO { get; set; }
        public string STP_MNAME { get; set; }
        public string STP_MEMAIL { get; set; }
        public long? STP_MMOBILENO { get; set; }
        public string STP_MBLOOD { get; set; }
        public string STP_MPHOTO { get; set; }
        public string STP_PERSTREET { get; set; }
        public string STP_PERAREA { get; set; }
        public string STP_PERCITY { get; set; }
        public long STP_PERSTATE { get; set; }
        public long STP_PERCOUNTRY { get; set; }
        public long STP_PERPIN { get; set; }
        public string STP_CURSTREET { get; set; }
        public string STP_CURAREA { get; set; }
        public string STP_CURCITY { get; set; }
        public long STP_CURSTATE { get; set; }
        public long STP_CURCOUNTRY { get; set; }
        public long STP_CURPIN { get; set; }
        public string STP_STATUS { get; set; }
        public DateTime? STP_DOB { get; set; }
        public string STP_DOBWORDS { get; set; }



    }
}

