using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Resume_Upload")]
    public class HR_Resume_UploadDMO
    {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRRUP_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRRUP_PersonName { get; set; }
        public long HRRUP_PhoneNo { get; set; }
        public string HRRUP_EmailId { get; set; }
        public string HRRUP_Qualification { get; set; }
        public string HRRUP_Experience { get; set; }
        public string HRRUP_Designation { get; set; }
        public string HRRUP_DocName { get; set; }
        public string HRRUP_DocPath { get; set; }
        public bool HRRUP_ActiveFlag { get; set; }
        public DateTime HRRUP_Date { get; set; }
    }
}
