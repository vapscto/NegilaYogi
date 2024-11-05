using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
  [Table("HR_Employee_Investment_SubSection")]
  public class HR_Employee_Subsection_Investment
    {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long HREIDSS_Id


        { get; set; }
    public long HREID_Id
        { get; set; }
    public long HRMCVIAS_Id
        { get; set; }
 
    public decimal? HREIDSS_Amount


        { get; set; }
   // public string HRETDS_ChallanNo { get; set; }
    public bool HREIDSS_ActiveFlg


        { get; set; }
    public long HREIDSS_CreatedBy


        { get; set; }
    public long HREIDSS_UpdatedBy


        { get; set; }

    }
}
