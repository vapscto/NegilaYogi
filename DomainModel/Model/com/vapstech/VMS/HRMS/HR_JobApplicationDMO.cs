using DomainModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.VMS.HRMS
{
    [Table("HR_JobApplication")]
    public class HR_JobApplicationDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRJA_Id { get; set; }
        public string HRJA_FirstName { get; set; }
        public string HRJA_LastName { get; set; }
        public string HRJA_EmailId { get; set; }
        public long HRJA_MobileNo { get; set; }
        public string HRJA_Gender { get; set; }
        public DateTime HRJA_DOB { get; set; }
        public string HRJA_CurrentLocation { get; set; }
        public string HRJA_Qualification { get; set; }
        public string HRJA_CurrentCompany { get; set; }
        public decimal HRJA_TotalExpFrom { get; set; }
        public decimal HRJA_TotalExpTo { get; set; }
        public decimal HRJA_CurrentCTCPerYear { get; set; }
        public decimal HRJA_ExpectedCTCPerYear { get; set; }
        public long HRJA_NoticePeriod { get; set; }
        public string HRJA_Resume { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
    }

}