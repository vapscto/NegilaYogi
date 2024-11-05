using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Employee_Increment_EDHeads")]
    public class HR_Employee_Increment_EDHeads
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HREICED_Id { get; set; }
        public long MI_Id { get; set; }
        public long HREIC_Id { get; set; }
        public decimal? HREICED_Amount { get; set; }
        public decimal? HREICED_PreviousAmount { get; set; }
        public string HREICED_Percentage { get; set; }
        public bool? HREICED_ActiveFlag { get; set; }
        public long HRMED_Id { get; set; }
        public string HREICED_CreatedDate { get; set; }
        public string HREICED_UpdatedDate { get; set; }
        public long HREICED_CreatedBy { get; set; }
        public long HREICED_UpdatedBy { get; set; }
    }
}
