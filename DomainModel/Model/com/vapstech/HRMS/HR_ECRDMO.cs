using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_ECR")]
    public class HR_ECRDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ECR_ID { get; set; }
        public long MI_ID { get; set; }
        public long Emp_code { get; set; }
        public string name { get; set; }
        public decimal? ECR_EPF_Wages { get; set; }
        public decimal? Ecr_Eps_Wages { get; set; }
        public decimal? Ecr_Epf_Contribution { get; set; }
        public decimal? Ecr_Epf_Cont_Remit { get; set; }
        public decimal? ECr_Epf_Eps_Diff { get; set; }
        public decimal? Ecr_Epf_Eps_ReDif { get; set; }
        public decimal? Ecr_Ncp { get; set; }
        public decimal? Ecr_Adva_Ref { get; set; }
        public decimal? Ecr_Arr_Epf { get; set; }
        public decimal? Ecr_Arr_Epf_EE_Share { get; set; }
        public decimal? Ecr_Arr_Epf_ER_Share { get; set; }
        public decimal? Ecr_Arr_EPS { get; set; }
        public string ECR_GuardianName { get; set; }
        public string Ecr_Guardian_Relation { get; set; }

        public string Ecr_DOB { get; set; }
        public string Ecr_Gender { get; set; }

        public string Ecr_Join_DOEPF { get; set; }
        public string ECR_Exit_DOEPF { get; set; }

        public string ECR_Exit_DoEps { get; set; }
        public string Ecr_Leav_Reason { get; set; }

       
    }
}
