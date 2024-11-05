using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Placement
{
    [Table("PL_CI_Schedule_Company")]
    public class PL_CI_Schedule_CompanyDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PLCISCHCOM_Id { get; set; }
        public long PLCISCH_Id { get; set; }
        public long PLMCOMP_Id { get; set; }
        public DateTime? PLCISCHCOM_DriveFromDate { get; set; }
        public DateTime? PLCISCHCOM_DriveToDate { get; set; }
        public string PLCISCHCOM_JobDetails { get; set; }
        public bool PLCISCHCOM_ActiveFlag { get; set; }

        public DateTime? PLCISCHCOM_CreatedDate { get; set; }
        public long PLCISCHCOM_CreatedBy { get; set; }
        public DateTime? PLCISCHCOM_UpdatedDate { get; set; }
        public long PLCISCHCOM_UpdatedBy { get; set; }

    } 
}
