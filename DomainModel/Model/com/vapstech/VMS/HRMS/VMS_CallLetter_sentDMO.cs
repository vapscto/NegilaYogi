using DomainModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.VMS.HRMS
{
    [Table("VMS_CallLetter_sent")]
    public class VMS_CallLetter_sentDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VMSCS_Id { get; set; }
        public string VMSCS_EmailId { get; set; }
        public DateTime VMSCS_SentDate { get; set; }
        public long VMSCS_CreatedBy { get; set; }
        public long VMSCS_updatedBy { get; set; }
    }
}