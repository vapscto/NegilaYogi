using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.University;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.University.Service
{
    public class UC_312_TeachersResearchImpl : Interface.UC_312_TeachersResearchInterface
    {
        public GeneralContext _GeneralContext;
        public UC_312_TeachersResearchImpl(GeneralContext parameter)
        {
            _GeneralContext = parameter;
        }
        public UC_312_TeachersResearchDTO loaddata(UC_312_TeachersResearchDTO data)
        {
            try
            {
                var institutionlist = (from a in _GeneralContext.Institution
                                       from b in _GeneralContext.UserRoleWithInstituteDMO
                                       where (b.Id == data.UserId && b.MI_Id == a.MI_Id && b.Activeflag == 1 && a.MI_ActiveFlag == 1)
                                       select a).Distinct().OrderBy(t => t.MI_Name).ToList();
                data.institutionlist = institutionlist.ToArray();
                if (data.MI_Id == 0)
                {
                    if (institutionlist.Count > 0)
                    {
                        data.MI_Id = institutionlist.FirstOrDefault().MI_Id;
                    }
                }
                data.allacademicyear = _GeneralContext.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).ToArray();

                data.filldepartment = _GeneralContext.HR_Master_Department.Where(a => a.MI_Id == data.MI_Id && a.HRMD_ActiveFlag == true).Distinct().OrderBy(e => e.HRMD_Order).ToArray();

                data.alldata1 = (from a in _GeneralContext.Academic
                                 from b in _GeneralContext.UC_312_TeachersResearchDMO
                                 where (a.MI_Id == data.MI_Id && b.MI_Id == a.MI_Id && a.Is_Active == true && b.NCMCTR312_Year == a.ASMAY_Id)
                                 select new UC_312_TeachersResearchDTO
                                 {
                                     NCMCTR312_Id = b.NCMCTR312_Id,
                                     NCMCTR312_Year = b.NCMCTR312_Year,
                                     ASMAY_Year = a.ASMAY_Year,
                                     NCMCTR312_ProjectName = b.NCMCTR312_ProjectName,
                                     NCMCTR312_Duration = b.NCMCTR312_Duration,
                                     NCMCTR312_ProjReceivingSeedMoney = b.NCMCTR312_ProjReceivingSeedMoney,
                                     NCMCTR312_amountOfSeedMoneyProvided = b.NCMCTR312_amountOfSeedMoneyProvided,
                                     NCMCTR312_ActiveFlag = b.NCMCTR312_ActiveFlag,
                                     MI_Id = b.MI_Id,
                                     ASMAY_Id = a.ASMAY_Id

                                 }).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public UC_312_TeachersResearchDTO save(UC_312_TeachersResearchDTO data)
        {
            try
            {
                if (data.NCMCTR312_Id == 0)
                {
                    var duplicate = _GeneralContext.UC_312_TeachersResearchDMO.Where(t =>t.NCMCTR312_Id != 0 && t.MI_Id == data.MI_Id && t.HRME_Id == data.HRME_Id && t.NCMCTR312_Year == data.ASMAY_Id && t.NCMCTR312_ProjectName == data.NCMCTR312_ProjectName).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        UC_312_TeachersResearchDMO obj = new UC_312_TeachersResearchDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.NCMCTR312_ProjectName = data.NCMCTR312_ProjectName;
                        obj.NCMCTR312_Duration = data.NCMCTR312_Duration;
                        obj.HRME_Id = data.HRME_Id;
                        obj.NCMCTR312_ProjReceivingSeedMoney = data.NCMCTR312_ProjReceivingSeedMoney;
                        obj.NCMCTR312_amountOfSeedMoneyProvided = data.NCMCTR312_amountOfSeedMoneyProvided;
                        obj.NCMCTR312_Year = data.ASMAY_Id;
                        obj.NCMCTR312_CreatedDate = DateTime.Now;
                        obj.NCMCTR312_UpdatedDate = DateTime.Now;
                        obj.NCMCTR312_ActiveFlag = true;
                        obj.NCMCTR312_CreatedBy = data.UserId;
                        obj.NCMCTR312_UpdatedBy = data.UserId;
                        _GeneralContext.Add(obj);
                        if (data.filelist.Length > 0)
                        {
                            for (int i = 0; i < data.filelist.Length; i++)
                            {
                                if (data.filelist[0].cfilepath != null)
                                {
                                    UC_312_TeachersResearchFilesDMO obj2 = new UC_312_TeachersResearchFilesDMO();
                                    obj2.NCMCTR312_Id = obj.NCMCTR312_Id;
                                    obj2.NCMCTR312F_FileName = data.filelist[i].cfilename;
                                    obj2.NCMCTR312F_Filedesc = data.filelist[i].cfiledesc;
                                    obj2.NCMCTR312F_FilePath = data.filelist[i].cfilepath;
                                    _GeneralContext.Add(obj2);
                                }
                            }
                        }
                        int row = _GeneralContext.SaveChanges();
                        if (row > 0)
                        {
                            data.msg = "saved";
                        }
                        else
                        {
                            data.msg = "Failed";
                        }
                    }
                }
                else if (data.NCMCTR312_Id > 0)
                {
                    var duplicate = _GeneralContext.UC_312_TeachersResearchDMO.Where(t => t.NCMCTR312_Id != data.NCMCTR312_Id && t.MI_Id == data.MI_Id && t.HRME_Id == data.HRME_Id && t.NCMCTR312_ProjectName == data.NCMCTR312_ProjectName && t.NCMCTR312_Year == data.ASMAY_Id).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _GeneralContext.UC_312_TeachersResearchDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMCTR312_Id == data.NCMCTR312_Id).SingleOrDefault();
                        update.NCMCTR312_UpdatedBy = data.UserId;
                        update.NCMCTR312_ProjectName = data.NCMCTR312_ProjectName;
                        update.NCMCTR312_Duration = data.NCMCTR312_Duration;
                        update.HRME_Id = data.HRME_Id;
                        update.NCMCTR312_ProjReceivingSeedMoney = data.NCMCTR312_ProjReceivingSeedMoney;
                        update.NCMCTR312_amountOfSeedMoneyProvided = data.NCMCTR312_amountOfSeedMoneyProvided;
                        update.NCMCTR312_Year = data.ASMAY_Id;
                        update.MI_Id = data.MI_Id;
                        update.NCMCTR312_UpdatedDate = DateTime.Now;
                        _GeneralContext.Update(update);
                        var CountRemoveFiles = _GeneralContext.UC_312_TeachersResearchFilesDMO.Where(t => t.NCMCTR312_Id == data.NCMCTR312_Id).ToList();
                        if (CountRemoveFiles.Count > 0)
                        {
                            foreach (var RemoveFiles in CountRemoveFiles)
                            {
                                _GeneralContext.Remove(RemoveFiles);
                            }
                            if (data.filelist.Length > 0)
                            {
                                for (int i = 0; i < data.filelist.Length; i++)
                                {
                                    if (data.filelist[0].cfilepath != null)
                                    {
                                        UC_312_TeachersResearchFilesDMO obj2 = new UC_312_TeachersResearchFilesDMO();
                                        obj2.NCMCTR312_Id = update.NCMCTR312_Id;
                                        obj2.NCMCTR312F_FileName = data.filelist[i].cfilename;
                                        obj2.NCMCTR312F_Filedesc = data.filelist[i].cfiledesc;
                                        obj2.NCMCTR312F_FilePath = data.filelist[i].cfilepath;
                                        _GeneralContext.Add(obj2);
                                    }
                                }
                            }
                        }
                        else if (CountRemoveFiles.Count == 0)
                        {
                            if (data.filelist.Length > 0)
                            {
                                for (int i = 0; i < data.filelist.Length; i++)
                                {
                                    if (data.filelist[0].cfilepath != null)
                                    {
                                        UC_312_TeachersResearchFilesDMO obj2 = new UC_312_TeachersResearchFilesDMO();
                                        obj2.NCMCTR312_Id = update.NCMCTR312_Id;
                                        obj2.NCMCTR312F_FileName = data.filelist[i].cfilename;
                                        obj2.NCMCTR312F_Filedesc = data.filelist[i].cfiledesc;
                                        obj2.NCMCTR312F_FilePath = data.filelist[i].cfilepath;
                                        _GeneralContext.Add(obj2);
                                    }
                                }
                            }
                        }
                        var row = _GeneralContext.SaveChanges();
                        if (row > 0)
                        {
                            data.msg = "updated";
                        }
                        else
                        {
                            data.msg = "failed";
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public UC_312_TeachersResearchDTO deactive(UC_312_TeachersResearchDTO data)
        {
            try
            {
                var u = _GeneralContext.UC_312_TeachersResearchDMO.Where(t => t.NCMCTR312_Id == data.NCMCTR312_Id).SingleOrDefault();
                if (u.NCMCTR312_ActiveFlag == true)
                {
                    u.NCMCTR312_ActiveFlag = false;
                }
                else if (u.NCMCTR312_ActiveFlag == false)
                {
                    u.NCMCTR312_ActiveFlag = true;
                }
                u.NCMCTR312_UpdatedDate = DateTime.Now;
                u.NCMCTR312_UpdatedBy = data.UserId;
                u.MI_Id = data.MI_Id;
                _GeneralContext.Update(u);
                int o = _GeneralContext.SaveChanges();
                if (o > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public UC_312_TeachersResearchDTO EditData(UC_312_TeachersResearchDTO data)
        {
            try
            {
                var edit = (from a in _GeneralContext.Academic
                            from b in _GeneralContext.UC_312_TeachersResearchDMO
                            where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCMCTR312_Year && b.MI_Id == data.MI_Id && b.NCMCTR312_Id == data.NCMCTR312_Id)
                            select new UC_312_TeachersResearchDTO
                            {
                                NCMCTR312_Id = b.NCMCTR312_Id,
                                NCMCTR312_ProjectName = b.NCMCTR312_ProjectName,
                                NCMCTR312_Duration = b.NCMCTR312_Duration,
                                NCMCTR312_ProjReceivingSeedMoney = b.NCMCTR312_ProjReceivingSeedMoney,
                                NCMCTR312_amountOfSeedMoneyProvided = b.NCMCTR312_amountOfSeedMoneyProvided,
                                NCMCTR312_Year = b.NCMCTR312_Year,
                                MI_Id = b.MI_Id,
                                HRME_Id = b.HRME_Id,
                                ASMAY_Year = a.ASMAY_Year,
                            }).Distinct().ToList();

                data.editlist = edit.ToArray();

                data.editFileslist = (from a in _GeneralContext.UC_312_TeachersResearchFilesDMO
                                      where (a.NCMCTR312_Id == data.NCMCTR312_Id)
                                      select new UC_312_TeachersResearchDTO
                                      {
                                          cfilename = a.NCMCTR312F_FileName,
                                          cfilepath = a.NCMCTR312F_FilePath,
                                          cfiledesc = a.NCMCTR312F_Filedesc,
                                      }).Distinct().ToArray();

                if (edit.Count > 0)
                {
                    data.HRME_Id = edit[0].HRME_Id;
                }

                if (data.HRME_Id != 0)
                {
                    data.HRMD_Id = _GeneralContext.HR_Master_Employee_DMO.Where(t => t.HRME_Id == data.HRME_Id && t.MI_Id == data.MI_Id).Select(a => a.HRMD_Id).SingleOrDefault();

                    data.HRMDES_Id = _GeneralContext.HR_Master_Employee_DMO.Where(t => t.HRME_Id == data.HRME_Id && t.MI_Id == data.MI_Id).Select(a => a.HRMDES_Id).SingleOrDefault();


                    data.filldesignation = (from a in _GeneralContext.HR_Master_Employee_DMO
                                            from b in _GeneralContext.HR_Master_Designation
                                            where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag && a.HRMDES_Id == b.HRMDES_Id && a.HRMD_Id == data.HRMD_Id && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true)
                                            select b).Distinct().ToArray();


                    data.emplist = (from a in _GeneralContext.HR_Master_Employee_DMO
                                    where (a.HRMDES_Id == data.HRMDES_Id && a.HRMD_Id == data.HRMD_Id && a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                    select new UC_312_TeachersResearchDTO
                                    {
                                        empname = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                        HRME_Id = a.HRME_Id,
                                        HRME_EmployeeOrder = a.HRME_EmployeeOrder,
                                    }).Distinct().OrderBy(a => a.HRME_EmployeeOrder).ToArray();
                }


            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public UC_312_TeachersResearchDTO viewuploadflies(UC_312_TeachersResearchDTO data)
        {
            try
            {
                data.viewuploadflies = (from a in _GeneralContext.UC_312_TeachersResearchFilesDMO
                                        where (a.NCMCTR312_Id == data.NCMCTR312_Id)
                                        select new UC_312_TeachersResearchDTO
                                        {
                                            cfilename = a.NCMCTR312F_FileName,
                                            cfilepath = a.NCMCTR312F_FilePath,
                                            cfiledesc = a.NCMCTR312F_Filedesc,
                                            NCMCTR312F_Id = a.NCMCTR312F_Id,
                                            NCMCTR312_Id = a.NCMCTR312_Id,
                                        }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public UC_312_TeachersResearchDTO deleteuploadfile(UC_312_TeachersResearchDTO data)
        {
            try
            {
                var res = _GeneralContext.UC_312_TeachersResearchFilesDMO.Where(t => t.NCMCTR312F_Id == data.NCMCTR312F_Id).SingleOrDefault();
                _GeneralContext.Remove(res);
                int s = _GeneralContext.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
                data.viewuploadflies = (from a in _GeneralContext.UC_312_TeachersResearchFilesDMO
                                        where (a.NCMCTR312_Id == data.NCMCTR312_Id)
                                        select new UC_312_TeachersResearchDTO
                                        {
                                            NCMCTR312_Id = a.NCMCTR312_Id,
                                            NCMCTR312F_Id = a.NCMCTR312F_Id,
                                            cfilename = a.NCMCTR312F_FileName,
                                            cfilepath = a.NCMCTR312F_FilePath,
                                            cfiledesc = a.NCMCTR312F_Filedesc,
                                        }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public UC_312_TeachersResearchDTO get_dept(UC_312_TeachersResearchDTO data)
        {
            try
            {


                data.filldesignation = (from a in _GeneralContext.HR_Master_Employee_DMO
                                        from b in _GeneralContext.HR_Master_Designation
                                        where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag && a.HRMDES_Id == b.HRMDES_Id && data.HRMD_Id == a.HRMD_Id && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true)
                                        select b).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }
        public UC_312_TeachersResearchDTO get_emp(UC_312_TeachersResearchDTO data)
        {
            try
            {
                data.emplist = (from a in _GeneralContext.HR_Master_Employee_DMO
                                where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.HRMDES_Id == data.HRMDES_Id && a.HRMD_Id == data.HRMD_Id)
                                select new UC_312_TeachersResearchDTO
                                {
                                    empname = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                    HRME_Id = a.HRME_Id,
                                    HRME_EmployeeOrder = a.HRME_EmployeeOrder,
                                    HRME_EmployeeCode = a.HRME_EmployeeCode
                                }).Distinct().OrderBy(a => a.HRME_EmployeeOrder).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }


    }
}
