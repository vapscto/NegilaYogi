using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.HRMS;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Services
{
    public class EmployeeAwardImpl : Interfaces.EmployeeAwardInterface
    {
        public HRMSContext _HRMSContext;
        public EmployeeAwardImpl(HRMSContext HRMSContext)
        {
            _HRMSContext = HRMSContext;

        }
        public HR_Employee_Awards_DTO getalldetails(HR_Employee_Awards_DTO data)
        {
            try
            {
                data.yearlist = _HRMSContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().ToArray();

                data.filldepartment = _HRMSContext.HR_Master_Department.Where(a => a.MI_Id == data.MI_Id && a.HRMD_ActiveFlag == true).Distinct().OrderBy(e => e.HRMD_Order).ToArray();


                data.alldata = (from a in _HRMSContext.HR_Employee_Awards_DMO
                                from e in _HRMSContext.MasterEmployee
                                from dp in _HRMSContext.HR_Master_Department
                                from des in _HRMSContext.HR_Master_Designation
                             
                                where (a.MI_Id == data.MI_Id && a.MI_Id == e.MI_Id && a.HRME_Id == e.HRME_Id && e.HRMD_Id == dp.HRMD_Id && e.HRMDES_Id == des.HRMDES_Id)
                                select new HR_Employee_Awards_DTO
                                {

                                    HRME_Id = a.HRME_Id,
                                    empname = ((e.HRME_EmployeeFirstName == null ? " " : e.HRME_EmployeeFirstName) + " " + (e.HRME_EmployeeMiddleName == null ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null ? " " : e.HRME_EmployeeLastName)).Trim(),
                                    empcode = e.HRME_EmployeeCode,
                                    mobileNo = (e.HRME_MobileNo).ToString(),
                                    HREAW_AwardName = a.HREAW_AwardName,
                                    HREAW_AwardYear = a.HREAW_AwardYear,
                                    HREAW_FileName = a.HREAW_FileName,
                                    HREAW_FilePath = a.HREAW_FilePath,
                                    HREAW_Id = a.HREAW_Id,
                                    HREAW_IncentiveAmount = a.HREAW_IncentiveAmount,
                                    HREAW_ActiveFlg = a.HREAW_ActiveFlg,
                                    HRMD_DepartmentName = dp.HRMD_DepartmentName,
                                    HRMDES_DesignationName = des.HRMDES_DesignationName,
                                    

                                }).Distinct().OrderByDescending(t => t.HREAW_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HR_Employee_Awards_DTO get_depchange(HR_Employee_Awards_DTO data)
        {
            try
            {


                data.filldesignation = (from a in _HRMSContext.MasterEmployee
                                        from b in _HRMSContext.HR_Master_Designation
                                        where (a.HRMDES_Id == b.HRMDES_Id && a.MI_Id == data.MI_Id && a.HRME_ActiveFlag && a.HRMD_Id == data.HRMD_Id && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true)
                                        select b).Distinct().ToArray();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;

        }
        public HR_Employee_Awards_DTO get_employee(HR_Employee_Awards_DTO data)
        {
            try
            {



                data.emplist = (from a in _HRMSContext.MasterEmployee
                                where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.HRMDES_Id == data.HRMDES_Id && a.HRMD_Id == data.HRMD_Id)
                                select new HR_Employee_Awards_DTO
                                {
                                    empname = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                    HRME_Id = a.HRME_Id,
                                    HRME_EmployeeOrder = a.HRME_EmployeeOrder,
                                }).Distinct().OrderBy(a => a.HRME_EmployeeOrder).ToArray();


            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;

        }
        public HR_Employee_Awards_DTO saverecord(HR_Employee_Awards_DTO data)
        {
            try
            {
                if (data.HREAW_Id == 0)
                {
                    var duplicate = _HRMSContext.HR_Employee_Awards_DMO.Where(t => t.MI_Id == data.MI_Id && t.HREAW_AwardName == data.HREAW_AwardName && t.HRME_Id == data.HRME_Id && t.HREAW_AwardYear == data.ASMAY_Id).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        HR_Employee_Awards_DMO obj = new HR_Employee_Awards_DMO();

                        obj.HREAW_Id = data.HREAW_Id;
                        obj.MI_Id = data.MI_Id;
                        obj.HRME_Id = data.HRME_Id;
                        obj.HREAW_AwardName = data.HREAW_AwardName;
                        obj.HREAW_AwardYear = data.ASMAY_Id;
                        obj.HREAW_IncentiveAmount = data.HREAW_IncentiveAmount;
                        obj.HREAW_FileName = data.HREAW_FileName;
                        obj.HREAW_FilePath = data.HREAW_FilePath;
                        obj.HREAW_ActiveFlg = true;
                        obj.HREAW_CreatedBy = data.UserId;
                        obj.HREAW_UpdatedBy = data.UserId;
                        obj.HREAW_LevelAwards = data.HREAW_LevelAwards;
                        obj.HREAW_AgencyName = data.HREAW_AgencyName;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;

                        _HRMSContext.Add(obj);
                        int s = _HRMSContext.SaveChanges();
                        if (s > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }

                }
                else if (data.HREAW_Id > 0)
                {
                    var duplicate = _HRMSContext.HR_Employee_Awards_DMO.Where(t => t.MI_Id == data.MI_Id && t.HREAW_Id != data.HREAW_Id && t.HRME_Id == data.HRME_Id && t.HREAW_AwardYear == data.ASMAY_Id && t.HREAW_AwardName == data.HREAW_AwardName).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _HRMSContext.HR_Employee_Awards_DMO.Where(t => t.MI_Id == data.MI_Id && t.HREAW_Id == data.HREAW_Id).Single();

                        update.HRME_Id = data.HRME_Id;
                        update.HREAW_AwardName = data.HREAW_AwardName;
                        update.HREAW_AwardYear = data.ASMAY_Id;
                        update.HREAW_IncentiveAmount = data.HREAW_IncentiveAmount;
                        update.HREAW_FileName = data.HREAW_FileName;
                        update.HREAW_FilePath = data.HREAW_FilePath;
                        update.HREAW_LevelAwards = data.HREAW_LevelAwards;
                        update.HREAW_AgencyName = data.HREAW_AgencyName;
                        update.HREAW_UpdatedBy = data.UserId;
                        update.UpdatedDate = DateTime.Now;

                        _HRMSContext.Update(update);
                        int s = _HRMSContext.SaveChanges();
                        if (s > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HR_Employee_Awards_DTO editrecord(HR_Employee_Awards_DTO data)
        {
            try
            {
                var edit = _HRMSContext.HR_Employee_Awards_DMO.Where(t => t.MI_Id == data.MI_Id && t.HREAW_Id == data.HREAW_Id).ToList();

                data.editlist = edit.ToArray();

                //var year_id = _HRMSContext.HR_Employee_Awards_DMO.Where(t => t.MI_Id == data.MI_Id && t.HREAW_Id == data.HREAW_Id).Select(t => t.HREAW_AwardYear).SingleOrDefault();

                //data.yearlist_edit = _HRMSContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == year_id).Distinct().ToArray();

                var empid = _HRMSContext.HR_Employee_Awards_DMO.Where(t => t.MI_Id == data.MI_Id && t.HREAW_Id == data.HREAW_Id).Select(t => t.HRME_Id).SingleOrDefault();

                var deptid = _HRMSContext.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == empid).Select(t => t.HRMD_Id).SingleOrDefault();

                data.HRMD_Id = deptid;

                data.filldesignation = (from a in _HRMSContext.MasterEmployee
                                        from b in _HRMSContext.HR_Master_Designation
                                        where (a.HRMDES_Id == b.HRMDES_Id && a.MI_Id == data.MI_Id && a.HRME_ActiveFlag && a.HRMD_Id == deptid && b.MI_Id == data.MI_Id && b.HRMDES_ActiveFlag == true)
                                        select b).Distinct().ToArray();
                var des_id = _HRMSContext.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == empid && t.HRMD_Id == data.HRMD_Id).Select(t => t.HRMDES_Id).SingleOrDefault();

                data.HRMDES_Id = des_id;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HR_Employee_Awards_DTO deactive(HR_Employee_Awards_DTO data)
        {
            try
            {
                var result = _HRMSContext.HR_Employee_Awards_DMO.Single(t => t.HREAW_Id == data.HREAW_Id && t.MI_Id == data.MI_Id);

                if (result.HREAW_ActiveFlg == true)
                {
                    result.HREAW_ActiveFlg = false;
                }
                else if (result.HREAW_ActiveFlg == false)
                {
                    result.HREAW_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                result.HREAW_UpdatedBy = data.UserId;
                _HRMSContext.Update(result);
                int rowAffected = _HRMSContext.SaveChanges();
                if (rowAffected > 0)
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
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}
