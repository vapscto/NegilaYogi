using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.HRMS;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Services
{
    public class StaffCompliantsImpl : Interfaces.StaffCompliantsInterface
    {
        public HRMSContext _hRMSContext;
        public StaffCompliantsImpl(HRMSContext _hRMS)
        {
            _hRMSContext = _hRMS;
        }
        public StaffCompliantsDTO loaddata(StaffCompliantsDTO data)
        {
            try
            {
                data.getemployeelist = (from a in _hRMSContext.MasterEmployee
                                        where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                        select new StaffCompliantsDTO
                                        {
                                            HRME_Id = a.HRME_Id,
                                            HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null
                                           || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName) + " " +
                                             (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName) + " " +
                                             (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName) +
                                             (a.HRME_EmployeeCode == null || a.HRME_EmployeeCode == "" ? "" : " : " + a.HRME_EmployeeCode)).Trim(),
                                        }).Distinct().OrderBy(a => a.HRME_EmployeeFirstName).ToArray();


                data.getsavedetails = (from a in _hRMSContext.MasterEmployee
                                       from b in _hRMSContext.HR_Employee_RemarksDMO
                                       where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id)
                                       select new StaffCompliantsDTO
                                       {
                                           HRME_Id = a.HRME_Id,
                                           HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null
                                           || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName) + " " +
                                             (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName) + " " +
                                             (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                           HREREM_Id = b.HREREM_Id,
                                           HRME_EmployeeCode = (a.HRME_EmployeeCode == null || a.HRME_EmployeeCode == "" ? "" : a.HRME_EmployeeCode),
                                           HREREM_Subject = b.HREREM_Subject,
                                           HREREM_Remarks = b.HREREM_Remarks,
                                           HREREM_Date = b.HREREM_Date,
                                           HREREM_FileName = b.HREREM_FileName,
                                           HREREM_ActiveFlg = b.HREREM_ActiveFlg,
                                           HREREM_CreatedDate = b.HREREM_CreatedDate,
                                           HREREM_FilePath = b.HREREM_FilePath
                                       }).Distinct().OrderByDescending(a => a.HREREM_CreatedDate).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StaffCompliantsDTO OnChangeEmployee(StaffCompliantsDTO data)
        {
            try
            {
                data.getemployeedetails = (from a in _hRMSContext.MasterEmployee
                                           from b in _hRMSContext.HR_Master_Department
                                           from c in _hRMSContext.HR_Master_Designation
                                           where (a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true
                                           && a.HRME_LeftFlag == false && a.HRME_Id == data.HRME_Id)
                                           select new StaffCompliantsDTO
                                           {
                                               HRME_Id = a.HRME_Id,
                                               HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " +
                                                 (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " +
                                                 (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                               HRME_EmployeeCode = a.HRME_EmployeeCode,
                                               HRMD_DepartmentName = b.HRMD_DepartmentName,
                                               HRMDES_DesignationName = c.HRMDES_DesignationName

                                           }).Distinct().OrderBy(a => a.HRME_EmployeeFirstName).ToArray();

                data.getemployeesaveddetails = _hRMSContext.HR_Employee_RemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.HRME_Id == data.HRME_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StaffCompliantsDTO SaveDetails(StaffCompliantsDTO data)
        {
            try
            {
                if (data.HREREM_Id > 0)
                {
                    data.message = "Update";
                    var checkresult = _hRMSContext.HR_Employee_RemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.HREREM_Id == data.HREREM_Id).ToList();
                    if (checkresult.Count > 0)
                    {
                        var result = _hRMSContext.HR_Employee_RemarksDMO.Single(a => a.MI_Id == data.MI_Id && a.HREREM_Id == data.HREREM_Id);
                        result.HREREM_Subject = data.HREREM_Subject;
                        result.HREREM_Remarks = data.HREREM_Remarks;
                        result.HREREM_RemarksBy = data.UserId;
                        result.HREREM_Date = data.HREREM_Date;
                        result.HREREM_FileName = data.HREREM_FileName;
                        result.HREREM_FilePath = data.HREREM_FilePath;
                        result.HREREM_UpdatedBy = data.UserId;
                        result.HREREM_UpdatedDate = DateTime.Now;
                        _hRMSContext.Update(result);
                        var i = _hRMSContext.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else
                {
                    data.message = "Add";
                    HR_Employee_RemarksDMO hR_Employee_RemarksDMO = new HR_Employee_RemarksDMO();
                    hR_Employee_RemarksDMO.MI_Id = data.MI_Id;
                    hR_Employee_RemarksDMO.HRME_Id = data.HRME_Id;
                    hR_Employee_RemarksDMO.HREREM_Subject = data.HREREM_Subject;
                    hR_Employee_RemarksDMO.HREREM_Remarks = data.HREREM_Remarks;
                    hR_Employee_RemarksDMO.HREREM_RemarksBy = data.UserId;
                    hR_Employee_RemarksDMO.HREREM_Date = data.HREREM_Date;
                    hR_Employee_RemarksDMO.HREREM_FileName = data.HREREM_FileName;
                    hR_Employee_RemarksDMO.HREREM_FilePath = data.HREREM_FilePath;
                    hR_Employee_RemarksDMO.HREREM_CreatedBy = data.UserId;
                    hR_Employee_RemarksDMO.HREREM_UpdatedBy = data.UserId;
                    hR_Employee_RemarksDMO.HREREM_ActiveFlg = true;
                    hR_Employee_RemarksDMO.HREREM_CreatedDate = DateTime.Now;
                    hR_Employee_RemarksDMO.HREREM_UpdatedDate = DateTime.Now;
                    _hRMSContext.Add(hR_Employee_RemarksDMO);
                    var i = _hRMSContext.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StaffCompliantsDTO EditDetails(StaffCompliantsDTO data)
        {
            try
            {
                data.editlist = _hRMSContext.HR_Employee_RemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.HREREM_Id == data.HREREM_Id).ToArray();

                data.geteditemployeedetails = (from a in _hRMSContext.MasterEmployee
                                               from b in _hRMSContext.HR_Master_Department
                                               from c in _hRMSContext.HR_Master_Designation
                                               where (a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true
                                               && a.HRME_LeftFlag == false && a.HRME_Id == data.HRME_Id)
                                               select new StaffCompliantsDTO
                                               {
                                                   HRME_Id = a.HRME_Id,
                                                   HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " +
                                                     (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " +
                                                     (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                                   HRME_EmployeeCode = a.HRME_EmployeeCode,
                                                   HRMD_DepartmentName = b.HRMD_DepartmentName,
                                                   HRMDES_DesignationName = c.HRMDES_DesignationName

                                               }).Distinct().OrderBy(a => a.HRME_EmployeeFirstName).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StaffCompliantsDTO ActiveDeativeEmployeeCompliantsDetails(StaffCompliantsDTO data)
        {
            try
            {
                var checkresult = _hRMSContext.HR_Employee_RemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.HREREM_Id == data.HREREM_Id).ToList();
                if (checkresult.Count > 0)
                {
                    var result = _hRMSContext.HR_Employee_RemarksDMO.Single(a => a.MI_Id == data.MI_Id && a.HREREM_Id == data.HREREM_Id);
                    result.HREREM_ActiveFlg = result.HREREM_ActiveFlg == true ? false : true;
                    result.HREREM_UpdatedBy = data.UserId;
                    result.HREREM_UpdatedDate = DateTime.Now;
                    _hRMSContext.Update(result);
                    var i = _hRMSContext.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StaffCompliantsDTO GetViewStaffLoaddata(StaffCompliantsDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var startDate = new DateTime(indiantime0.Year, indiantime0.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                data.FromDate = startDate;
                data.ToDate = indiantime0;

                if (data.message == "Admin")
                {
                    data.getreportdetails = (from a in _hRMSContext.MasterEmployee
                                             from b in _hRMSContext.HR_Employee_RemarksDMO
                                             from c in _hRMSContext.HR_Master_Department
                                             from d in _hRMSContext.HR_Master_Designation
                                             where (a.HRME_Id == b.HRME_Id && a.HRMD_Id == c.HRMD_Id && a.HRMDES_Id == d.HRMDES_Id && a.MI_Id == data.MI_Id
                                             && b.MI_Id == data.MI_Id && (b.HREREM_Date.Value.Date >= data.FromDate.Value.Date
                                             && b.HREREM_Date.Value.Date <= data.ToDate.Value.Date))
                                             select new StaffCompliantsDTO
                                             {
                                                 HRME_Id = a.HRME_Id,
                                                 HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null
                                                 || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName) + " " +
                                                   (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName) + " " +
                                                   (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                                 HREREM_Id = b.HREREM_Id,
                                                 HRME_EmployeeCode = (a.HRME_EmployeeCode == null || a.HRME_EmployeeCode == "" ? "" : a.HRME_EmployeeCode),
                                                 HREREM_Subject = b.HREREM_Subject,
                                                 HREREM_Remarks = b.HREREM_Remarks,
                                                 HREREM_Date = b.HREREM_Date,
                                                 HREREM_CreatedDate = b.HREREM_CreatedDate,
                                                 HREREM_FileName = b.HREREM_FileName,
                                                 HREREM_ActiveFlg = b.HREREM_ActiveFlg,
                                                 HREREM_FilePath = b.HREREM_FilePath,
                                                 HRMD_DepartmentName = c.HRMD_DepartmentName,
                                                 HRMDES_DesignationName = d.HRMDES_DesignationName
                                             }).Distinct().OrderByDescending(a => a.HREREM_CreatedDate).ToArray();

                }
                else
                {
                    var getuserid = _hRMSContext.Staff_User_Login.Where(a => a.Id == data.UserId).ToList();
                    if (getuserid.Count > 0)
                    {
                        data.getreportdetails = (from a in _hRMSContext.MasterEmployee
                                                 from b in _hRMSContext.HR_Employee_RemarksDMO
                                                 from c in _hRMSContext.HR_Master_Department
                                                 from d in _hRMSContext.HR_Master_Designation
                                                 where (a.HRME_Id == b.HRME_Id && a.HRMD_Id == c.HRMD_Id && a.HRMDES_Id == d.HRMDES_Id && a.MI_Id == data.MI_Id
                                                 && b.MI_Id == data.MI_Id && (b.HREREM_Date.Value.Date >= data.FromDate.Value.Date
                                                 && b.HREREM_Date.Value.Date <= data.ToDate.Value.Date) && b.HRME_Id == getuserid.FirstOrDefault().Emp_Code)
                                                 select new StaffCompliantsDTO
                                                 {
                                                     HRME_Id = a.HRME_Id,
                                                     HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null
                                                     || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName) + " " +
                                                     (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName) 
                                                     + " " + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : 
                                                     " " + a.HRME_EmployeeLastName)).Trim(),
                                                     HREREM_Id = b.HREREM_Id,
                                                     HRME_EmployeeCode = (a.HRME_EmployeeCode == null || a.HRME_EmployeeCode == "" ? "" : a.HRME_EmployeeCode),
                                                     HREREM_Subject = b.HREREM_Subject,
                                                     HREREM_Remarks = b.HREREM_Remarks,
                                                     HREREM_Date = b.HREREM_Date,
                                                     HREREM_CreatedDate = b.HREREM_CreatedDate,
                                                     HREREM_FileName = b.HREREM_FileName,
                                                     HREREM_ActiveFlg = b.HREREM_ActiveFlg,
                                                     HREREM_FilePath = b.HREREM_FilePath,
                                                     HRMD_DepartmentName = c.HRMD_DepartmentName,
                                                     HRMDES_DesignationName = d.HRMDES_DesignationName
                                                 }).Distinct().OrderByDescending(a => a.HREREM_CreatedDate).ToArray();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StaffCompliantsDTO GetReport(StaffCompliantsDTO data)
        {
            try
            {
                if (data.message == "Admin")
                {
                    data.getreportdetails = (from a in _hRMSContext.MasterEmployee
                                             from b in _hRMSContext.HR_Employee_RemarksDMO
                                             from c in _hRMSContext.HR_Master_Department
                                             from d in _hRMSContext.HR_Master_Designation
                                             where (a.HRME_Id == b.HRME_Id && a.HRMD_Id == c.HRMD_Id && a.HRMDES_Id == d.HRMDES_Id && a.MI_Id == data.MI_Id
                                             && b.MI_Id == data.MI_Id && (b.HREREM_Date.Value.Date >= data.FromDate.Value.Date
                                             && b.HREREM_Date.Value.Date <= data.ToDate.Value.Date))
                                             select new StaffCompliantsDTO
                                             {
                                                 HRME_Id = a.HRME_Id,
                                                 HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null
                                                 || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName) + " " +
                                                   (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName) + " " +
                                                   (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                                 HREREM_Id = b.HREREM_Id,
                                                 HRME_EmployeeCode = (a.HRME_EmployeeCode == null || a.HRME_EmployeeCode == "" ? "" : a.HRME_EmployeeCode),
                                                 HREREM_Subject = b.HREREM_Subject,
                                                 HREREM_Remarks = b.HREREM_Remarks,
                                                 HREREM_Date = b.HREREM_Date,
                                                 HREREM_CreatedDate = b.HREREM_CreatedDate,
                                                 HREREM_FileName = b.HREREM_FileName,
                                                 HREREM_ActiveFlg = b.HREREM_ActiveFlg,
                                                 HREREM_FilePath = b.HREREM_FilePath,
                                                 HRMD_DepartmentName = c.HRMD_DepartmentName,
                                                 HRMDES_DesignationName = d.HRMDES_DesignationName
                                             }).Distinct().OrderByDescending(a => a.HREREM_CreatedDate).ToArray();
                }
                else
                {
                    var getuserid = _hRMSContext.Staff_User_Login.Where(a => a.Id == data.HREREM_Id).ToList();

                    if (getuserid.Count > 0)
                    {
                        data.getreportdetails = (from a in _hRMSContext.MasterEmployee
                                                 from b in _hRMSContext.HR_Employee_RemarksDMO
                                                 from c in _hRMSContext.HR_Master_Department
                                                 from d in _hRMSContext.HR_Master_Designation
                                                 where (a.HRME_Id == b.HRME_Id && a.HRMD_Id == c.HRMD_Id && a.HRMDES_Id == d.HRMDES_Id && a.MI_Id == data.MI_Id
                                                 && b.MI_Id == data.MI_Id && (b.HREREM_Date.Value.Date >= data.FromDate.Value.Date
                                                 && b.HREREM_Date.Value.Date <= data.ToDate.Value.Date) && b.HRME_Id == getuserid.FirstOrDefault().Emp_Code)
                                                 select new StaffCompliantsDTO
                                                 {
                                                     HRME_Id = a.HRME_Id,
                                                     HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null
                                                     || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName) + " " +
                                                       (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName) + " " +
                                                       (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                                     HREREM_Id = b.HREREM_Id,
                                                     HRME_EmployeeCode = (a.HRME_EmployeeCode == null || a.HRME_EmployeeCode == "" ? "" : a.HRME_EmployeeCode),
                                                     HREREM_Subject = b.HREREM_Subject,
                                                     HREREM_Remarks = b.HREREM_Remarks,
                                                     HREREM_Date = b.HREREM_Date,
                                                     HREREM_CreatedDate = b.HREREM_CreatedDate,
                                                     HREREM_FileName = b.HREREM_FileName,
                                                     HREREM_ActiveFlg = b.HREREM_ActiveFlg,
                                                     HREREM_FilePath = b.HREREM_FilePath,
                                                     HRMD_DepartmentName = c.HRMD_DepartmentName,
                                                     HRMDES_DesignationName = d.HRMDES_DesignationName
                                                 }).Distinct().OrderByDescending(a => a.HREREM_CreatedDate).ToArray();
                    }
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