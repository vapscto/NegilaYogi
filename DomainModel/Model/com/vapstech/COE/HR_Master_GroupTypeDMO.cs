using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.COE
{
    [Table("HR_Master_GroupType")]
    public class HR_Master_GroupTypeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
     
      
        public long HRMGT_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMGT_EmployeeGroupType { get; set; }
       
        public string HRMGT_Code { get; set; }
        public int HRMGT_Order { get; set; }
        public bool HRMGT_ActiveFlag { get;set;}
      
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }


    }
}
