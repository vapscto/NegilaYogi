using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Admission;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Impl
{
    public class ClgMasterBranchImpl : Interface.ClgMasterBranchInterface
    {
        public ClgAdmissionContext _ClgAdmissionContext;
        ILogger<ClgMasterBranchImpl> _logbranch;
        public ClgMasterBranchImpl(ClgAdmissionContext ClgAdmissionContext, ILogger<ClgMasterBranchImpl> log)
        {
            _ClgAdmissionContext = ClgAdmissionContext;
            _logbranch = log;
        }
        public ClgMasterBranchDTO getalldetails(ClgMasterBranchDTO data)
        {
            try
            {
                data.getdetails = _ClgAdmissionContext.ClgMasterBranchDMO.Where(a => a.MI_Id == data.MI_Id).OrderBy(a => a.AMB_Order).ToArray();
            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("Master Branch  getalldetails :" + ex.Message);
            }
            return data;
        }
        public ClgMasterBranchDTO savebranch(ClgMasterBranchDTO data)
        {
            try
            {
                if (data.AMB_Id > 0)
                {
                    var checkvalidation = _ClgAdmissionContext.ClgMasterBranchDMO.Where(a => a.MI_Id == data.MI_Id && a.AMB_BranchName.Equals(data.AMB_BranchName) && a.AMB_Id != data.AMB_Id).ToList();


                    var checkvalidation1 = _ClgAdmissionContext.ClgMasterBranchDMO.Where(a => a.MI_Id == data.MI_Id && a.AMB_BranchCode.Equals(data.AMB_BranchCode) && a.AMB_Id != data.AMB_Id).ToList();

                    if (checkvalidation.Count > 0 || checkvalidation1.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        var result = _ClgAdmissionContext.ClgMasterBranchDMO.Single(a => a.MI_Id == data.MI_Id && a.AMB_Id == data.AMB_Id);
                        result.AMB_BranchName = data.AMB_BranchName;
                        result.AMB_BranchCode = data.AMB_BranchCode;
                        result.AMB_BranchInfo = data.AMB_BranchInfo;
                        result.AMB_BranchType = data.AMB_BranchType;
                        result.AMB_Order = data.AMB_Order;
                        result.AMB_StudentCapacity = data.AMB_StudentCapacity;
                        result.AMB_AidedUnAided = data.AMB_AidedUnAided;
                        result.UpdatedDate = DateTime.Now;

                        _ClgAdmissionContext.Update(result);
                        var ii = _ClgAdmissionContext.SaveChanges();
                        if (ii > 0)
                        {
                            data.returnval = true;
                            data.message = "Update";
                        }
                        else
                        {
                            data.returnval = true;
                            data.message = "Update";
                        }
                    }
                }
                else
                {
                    ClgMasterBranchDMO branchdmo = new ClgMasterBranchDMO();

                    var checkvalidation = _ClgAdmissionContext.ClgMasterBranchDMO.Where(a => a.MI_Id == data.MI_Id && a.AMB_BranchName.Equals(data.AMB_BranchName)).ToList();



                    var checkvalidation1 = _ClgAdmissionContext.ClgMasterBranchDMO.Where(a => a.MI_Id == data.MI_Id && a.AMB_BranchCode.Equals(data.AMB_BranchCode)).ToList();

                    if (checkvalidation.Count > 0 || checkvalidation1.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        branchdmo.MI_Id = data.MI_Id;
                        branchdmo.AMB_BranchName = data.AMB_BranchName;
                        branchdmo.AMB_BranchCode = data.AMB_BranchCode;
                        branchdmo.AMB_BranchInfo = data.AMB_BranchInfo;
                        branchdmo.AMB_BranchType = data.AMB_BranchType;
                        branchdmo.AMB_Order = data.AMB_Order;
                        branchdmo.AMB_StudentCapacity = data.AMB_StudentCapacity;
                        branchdmo.CreatedDate = DateTime.Now;
                        branchdmo.UpdatedDate = DateTime.Now;
                        branchdmo.AMB_ActiveFlag = true;
                        branchdmo.AMB_AidedUnAided = data.AMB_AidedUnAided;
                        _ClgAdmissionContext.Add(branchdmo);
                        var ii = _ClgAdmissionContext.SaveChanges();
                        if (ii > 0)
                        {
                            data.returnval = true;
                            data.message = "Add";
                        }
                        else
                        {
                            data.returnval = true;
                            data.message = "Add";
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("Master Branch savebranch :" + ex.Message);
            }
            return data;
        }
        public ClgMasterBranchDTO editbranch(ClgMasterBranchDTO data)
        {
            try
            {
                data.editdetails = _ClgAdmissionContext.ClgMasterBranchDMO.Where(a => a.MI_Id == data.MI_Id && a.AMB_Id == data.AMB_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("Master Branch editbranch :" + ex.Message);
            }
            return data;
        }
        public ClgMasterBranchDTO activedeactivebranch(ClgMasterBranchDTO data)
        {
            try
            {
                var result = _ClgAdmissionContext.ClgMasterBranchDMO.Single(a => a.MI_Id == data.MI_Id && a.AMB_Id == data.AMB_Id);
                if (result.AMB_ActiveFlag == true)
                {
                    result.AMB_ActiveFlag = false;
                }
                else
                {
                    result.AMB_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.Now;

                _ClgAdmissionContext.Update(result);
                var ii = _ClgAdmissionContext.SaveChanges();
                if (ii > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("Master Branch activedeactivebranch :" + ex.Message);
            }
            return data;
        }

        public ClgMasterBranchDTO saveorder(ClgMasterBranchDTO data)
        {
            try
            {
                int id = 0;
                for (int i = 0; i < data.orderlistbranch.Count; i++)
                {
                    _logbranch.LogInformation("enter in class order");
                    var reult = _ClgAdmissionContext.ClgMasterBranchDMO.Single(t => t.MI_Id == data.MI_Id && t.AMB_Id == data.orderlistbranch[i].AMB_Id);
                    id = id + 1;

                    if (i == 0)
                    {
                        reult.AMB_Order = id;
                    }
                    else
                    {
                        reult.AMB_Order = id;
                    }
                    _ClgAdmissionContext.Update(reult);
                    var flag = _ClgAdmissionContext.SaveChanges();
                    if (flag > 0)
                    {
                        _logbranch.LogInformation("data saved successful branch order ");
                        data.returnval = true;
                    }
                    else
                    {
                        _logbranch.LogInformation("data not saved successful branch order");
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("Master Branch  saveorder :" + ex.Message);
            }
            return data;
        }

    }
}
