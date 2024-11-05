using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class MasterAllowanceDTO
    {
        public long HRMAL_Id
        { get; set; }
        public long MI_Id
        { get; set; }
        public string HRMAL_AllowanceName
        { get; set; }
        public bool HRMAL_AllowanceFlg
        { get; set; }
        public bool HRMAL_MaxLimitAplFlg
        { get; set; }
        public decimal? HRMAL_MaxLimit
        { get; set; }
        public bool HRMAL_ActiveFlg
        { get; set; }
        // public bool HRETDS_ActiveFlg { get; set; }
        public long HRMAL_CreatedBy
        { get; set; }
        public long HRMAL_UpdatedBy { get; set; }
    
    public long roleId { get; set; }
       // public long HRETDS_CreatedBy { get; set; }
      //  public long HRETDS_UpdatedBy { get; set; }
//
        public Array bankdetailList { get; set; }
        public string retrunMsg { get; set; }

        public long LogInUserId { get; set; }

    }
}
