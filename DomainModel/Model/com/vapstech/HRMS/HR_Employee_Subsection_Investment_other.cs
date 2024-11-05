using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
  [Table("HR_Employee_InvestmentOthers")]
  public class HR_Employee_Subsection_Investment_other
    {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long HREIDO_Id { get; set; }
    public long MI_Id { get; set; }
    public long HRME_Id   { get; set; }

    public long HRMCVIA_Id { get; set; }
   public long IMFY_Id { get; set; }
    public bool HREIDO_ActiveFlg  { get; set; }
    //public long HREIDSS_CreatedBy  { get; set; }
   // public long HREIDSS_UpdatedBy { get; set; }

        public decimal? HREIDO_RentPaid { get; set; }
        public string HREIDO_RentPaidEvidence { get; set; }

        public string HREIDO_NameOfLandLord { get; set; }
        public string HREIDO_NameEvidence { get; set; }
        public string HREIDO_LandLordAddress { get; set; }

        public string HREIDO_AddressEvidence { get; set; }
        public string HREIDO_LandLordPAN { get; set; }
        public string HREIDO_PANEvidence { get; set; }


        public decimal? HREIDO_TravelConcession { get; set; }
        public string HREIDO_ConcessionEvidence { get; set; }
        public decimal? HREIDO_InterestPaid { get; set; }

        public string HREIDO_InterestEvidence { get; set; }
        public string HREIDO_LenderName { get; set; }
        public string HREIDO_LNameEvidence { get; set; }
        public string HREIDO_LenderAddress { get; set; }

        public string HREIDO_LAddressEvidence { get; set; }
        public string HREIDO_LenderPAN { get; set; }
        public string HREIDO_LPANEvidence { get; set; }
        public long HREIDO_FinanceInst { get; set; }

        public string HREIDO_InstEvidence { get; set; }
        public string HREIDO_Employer { get; set; }
        public string HREIDO_EmpEvidence { get; set; }
        public string HREIDO_Others { get; set; }


        public string HREIDO_OthersEvidence { get; set; }
        public long HREIDO_CreatedBy
        { get; set; }
        public long HREIDO_UpdatedBy { get; set; }
     












        
        



    }
}
