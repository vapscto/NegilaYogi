using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.COE
{
    [Table("COE_Events_Employees", Schema = "COE")]
    public class COE_Events_EmployeesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
     
      
        public int COEEE_Id { get; set; }
        [ForeignKey("COEE_Id")]
        public int COEE_Id { get; set; }
        public long HRMGT_Id { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }


    }
}
