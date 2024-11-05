using LeaveManagementServiceHub.com.vaps.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.LeaveManagement;
using DomainModel.Model.com.vapstech.LeaveManagement;

namespace LeaveManagementServiceHub.com.vaps.Services
{
    public class LeaveConfigImpl : LeaveConfigInterface
    {
        private readonly DomainModelMsSqlServerContext _db;
        //private readonly ILogger<LeaveReportImpl> _log;
        public LMContext _lmContext;
        public LeaveConfigImpl(LMContext data)
        {
            _lmContext = data;
        }
        public HR_Leave_Policy_Config_DTO save(HR_Leave_Policy_Config_DTO data)
        {
            try
            {
                var query = _lmContext.HR_Leave_Policy_Config_DMO.Where(t => t.MI_Id == data.MI_Id).ToList();
                if (query.Count > 0)
                {
                    var resultCount = _lmContext.HR_Leave_Policy_Config_DMO.Single(t => t.HRLPC_Id == query.FirstOrDefault().HRLPC_Id);
                    resultCount.HRLPC_AfterCummulativeTime = data.HRLPC_AfterCummulativeTime;
                    resultCount.HRLPC_CummulativeTime = data.HRLPC_CummulativeTime;
                    resultCount.HRLPC_CummulativeTimeFlag = data.HRLPC_CummulativeTimeFlag;
                    resultCount.HRLPC_EarlyOutFlag = data.HRLPC_EarlyOutFlag;
                    resultCount.HRLPC_EarlyOutTime = data.HRLPC_EarlyOutTime;
                    resultCount.HRLPC_IncludeHolidayFlag = data.HRLPC_IncludeHolidayFlag;
                    resultCount.HRLPC_LateInFlag = data.HRLPC_LateInFlag;
                    resultCount.HRLPC_LateInTime = data.HRLPC_LateInTime;
                    resultCount.HRLPC_LateLeaveFlag = data.HRLPC_LateLeaveFlag;
                    resultCount.HRLPC_LateLOPFlag = data.HRLPC_LateLOPFlag;
                    resultCount.HRLPC_LeavePolicyName = data.HRLPC_LeavePolicyName;
                    resultCount.HRLPC_LeavePrefixSuffixFlag = data.HRLPC_LeavePrefixSuffixFlag;
                    resultCount.HRLPC_NoOfLates = data.HRLPC_NoOfLates;
                    resultCount.HRLPC_NoOfLatesCFFlag = data.HRLPC_NoOfLatesCFFlag;
                    resultCount.HRLPC_NoOfLatesFag = data.HRLPC_NoOfLatesFag;
                    resultCount.HRLPC_ServiceName = data.HRLPC_ServiceName;
                    resultCount.HRLPC_SPName = data.HRLPC_SPName;
                    resultCount.MI_Id = data.MI_Id;
                    resultCount.UpdatedDate = DateTime.Now;
                    resultCount.HRLPC_SpOrGen = data.HRLPC_SpOrGen;
                    resultCount.HRLPC_AbsentLOPFlag = data.HRLPC_AbsentLOPFlag;
                    resultCount.HRLPC_AbsentLeaveFlag = data.HRLPC_AbsentLeaveFlag;
                    resultCount.HRLPC_UpdatedBy = data.LoginId;

                    _lmContext.Update(resultCount);
                    var flag = _lmContext.SaveChanges();
                    if (flag > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }

                }
                else
                {
                    HR_Leave_Policy_Config_DMO obj = new HR_Leave_Policy_Config_DMO();
                    obj.HRLPC_AfterCummulativeTime = data.HRLPC_AfterCummulativeTime;
                    obj.HRLPC_CummulativeTime = data.HRLPC_CummulativeTime;
                    obj.HRLPC_CummulativeTimeFlag = data.HRLPC_CummulativeTimeFlag;
                    obj.HRLPC_EarlyOutFlag = data.HRLPC_EarlyOutFlag;
                    obj.HRLPC_EarlyOutTime = data.HRLPC_EarlyOutTime;
                    obj.HRLPC_IncludeHolidayFlag = data.HRLPC_IncludeHolidayFlag;
                    obj.HRLPC_LateInFlag = data.HRLPC_LateInFlag;
                    obj.HRLPC_LateInTime = data.HRLPC_LateInTime;
                    obj.HRLPC_LateLeaveFlag = data.HRLPC_LateLeaveFlag;
                    obj.HRLPC_LateLOPFlag = data.HRLPC_LateLOPFlag;
                    obj.HRLPC_LeavePolicyName = data.HRLPC_LeavePolicyName;
                    obj.HRLPC_LeavePrefixSuffixFlag = data.HRLPC_LeavePrefixSuffixFlag;
                    obj.HRLPC_NoOfLates = data.HRLPC_NoOfLates;
                    obj.HRLPC_NoOfLatesCFFlag = data.HRLPC_NoOfLatesCFFlag;
                    obj.HRLPC_NoOfLatesFag = data.HRLPC_NoOfLatesFag;
                    obj.HRLPC_ServiceName = data.HRLPC_ServiceName;
                    obj.HRLPC_SPName = data.HRLPC_SPName;
                    obj.MI_Id = data.MI_Id;
                    obj.UpdatedDate = DateTime.Now;
                    obj.CreatedDate = DateTime.Now;
                    obj.HRLPC_SpOrGen = data.HRLPC_SpOrGen;
                    obj.HRLPC_AbsentLOPFlag = data.HRLPC_AbsentLOPFlag;
                    obj.HRLPC_AbsentLeaveFlag = data.HRLPC_AbsentLeaveFlag;
                    obj.HRLPC_UpdatedBy = data.LoginId;
                    obj.HRLPC_CreatedBy = data.LoginId;

                    _lmContext.Add(obj);
                    var flag = _lmContext.SaveChanges();
                    if (flag > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }

                }
               
            }
            catch(Exception ex)
            {
                data.returnval = false;
            }
            return data;
        }

        public HR_Leave_Policy_Config_DTO getSPName(HR_Leave_Policy_Config_DTO data)
        {
            var resultCount = _lmContext.HR_Leave_Policy_Config_DMO.Where(t => t.MI_Id == data.MI_Id).ToList();
            if (resultCount.Count > 0)
            {
                data.config_data = resultCount.ToArray();
            }
          
            return data;
        }
    }
}
