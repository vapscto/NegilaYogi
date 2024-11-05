using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Preadmission_App_Points_Income")]
    public class Preadmission_App_Points_IncomeDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PAAPI_Id  { get; set; }
        public long MI_Id  { get; set; }
        public decimal PAAPI_FromIncome  { get; set; }
        public decimal  PAAPI_ToIncome  { get; set; }

        public int PAAPI_Points { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
