using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class StaffParticipationImpl : Interface.StaffParticipationInterface
    {
        public GeneralContext _GeneralContext;
        public StaffParticipationImpl(GeneralContext para)
        {
            _GeneralContext = para;
        }
        public async Task<NAAC_AC_TParticipation_113_DTO> loaddata(NAAC_AC_TParticipation_113_DTO data)
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
                data.yearlist = (from a in _GeneralContext.Academic
                                 where (a.MI_Id == data.MI_Id && a.Is_Active == true)
                                 select new NAAC_AC_TParticipation_113_DTO
                                 {
                                     ASMAY_Id = a.ASMAY_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();
                data.departmentlist = _GeneralContext.HR_Master_Department.Where(t => t.MI_Id == data.MI_Id && t.HRMD_ActiveFlag == true).Distinct().OrderBy(t => t.HRMD_Order).ToArray();
                data.alldata = (from a in _GeneralContext.NAAC_AC_TParticipation_113_DMO
                                from b in _GeneralContext.HR_Master_Department
                                from c in _GeneralContext.HR_Master_Designation
                                from d in _GeneralContext.HR_Master_Employee_DMO
                                from y in _GeneralContext.Academic
                                where (a.MI_Id == b.MI_Id && b.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.HRME_Id == d.HRME_Id && d.HRMD_Id == b.HRMD_Id && d.HRMDES_Id == c.HRMDES_Id && y.ASMAY_Id == a.NCACTP113_ParticipatedYear && d.HRME_ActiveFlag == true && d.HRME_LeftFlag == false && b.HRMD_ActiveFlag == true && c.HRMDES_ActiveFlag == true)
                                select new NAAC_AC_TParticipation_113_DTO
                                {
                                    NCACTP113_Id = a.NCACTP113_Id,
                                    NCACTP113_BodyName = a.NCACTP113_BodyName,
                                    ASMAY_Year = y.ASMAY_Year,
                                    NCACTP113_ActiveFlg = a.NCACTP113_ActiveFlg,
                                    HRMD_DepartmentName = b.HRMD_DepartmentName,
                                    HRMDES_DesignationName = c.HRMDES_DesignationName,
                                    empname = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                    HRME_Id = d.HRME_Id,
                                    MI_Id = a.MI_Id,
                                    NCACTP113_PDate = a.NCACTP113_PDate,
                                    NCACTP113_StatusFlg = a.NCACTP113_StatusFlg,
                                    NCACTP113_ApprovedFlg = a.NCACTP113_ApprovedFlg,
                                }).Distinct().OrderBy(t => t.NCACTP113_Id).ToArray();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
        //aded by sanjeev 
        //  saveadvance
        public NAAC_AC_TParticipation_113_DTO saveadvance(NAAC_AC_TParticipation_113_DTO data)
        {
            try
            {
                if (data.advimppln.Length > 0)
                {
                    var Listarray = new ArrayList();
                    var duplicatevalue = new ArrayList();
                    var rowno = 1;
                    TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                    foreach (var I in data.advimppln)
                    {
                        data.ASMAY_Id = 0; data.HRMD_Id = 0; data.HRMDES_Id = 0; data.HRME_Id = 0;
                        I.Date = I.Date.Trim();
                        I.Month = I.Month.Trim();
                        I.Year = I.Year.Trim();
                        for (int i = 0; i < I.Date.Trim().Length; i++)
                        {
                            if (char.IsLetter(I.Date.Trim()[i]))
                            {

                                return data;
                            }
                        }
                        for (int j = 0; j < I.Month.Trim().Length; j++)
                        {
                            if (char.IsLetter(I.Month.Trim()[j]))
                            {

                                return data;
                            }
                        }
                        for (int K = 0; K < I.Month.Trim().Length; K++)
                        {
                            if (char.IsLetter(I.Month.Trim()[K]))
                            {

                                return data;
                            }
                        }
                        DateTime Stdate = indianTime;
                        string startdat = I.Date + "/" + I.Month + "/" + I.Year;
                        startdat = startdat.Trim();
                        if (startdat != null && startdat != "" && startdat != "NULL")
                        {
                            if (startdat.Contains("-"))
                            {
                                DateTime DT = DateTime.ParseExact(startdat.Trim(), "dd-MM-yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
                                Stdate = DT;
                            }
                            else
                            {
                                DateTime DT = DateTime.ParseExact(startdat.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                Stdate = DT;

                            }
                            data.ASMAY_Id = _GeneralContext.Academic.Where(R => R.ASMAY_Year == I.ParticipationYear && R.MI_Id == data.MI_Id && R.Is_Active == true).Select(P => P.ASMAY_Id).FirstOrDefault();
                            if (data.ASMAY_Id > 0)
                            {
                                data.HRMD_Id = _GeneralContext.HR_Master_Department.Where(t => t.MI_Id == data.MI_Id && t.HRMD_ActiveFlag == true && t.HRMD_DepartmentName == I.DepartmentName).Select(R => R.HRMD_Id).FirstOrDefault();
                                data.HRMDES_Id = (from a in _GeneralContext.HR_Master_Employee_DMO
                                                  from b in _GeneralContext.HR_Master_Designation
                                                  where (a.MI_Id == b.MI_Id && a.HRMDES_Id == b.HRMDES_Id && a.HRMD_Id == data.HRMD_Id && a.MI_Id == data.MI_Id && b.HRMDES_DesignationName == I.DesignationName)
                                                  select b.HRMDES_Id).FirstOrDefault();

                                data.HRME_Id = (from a in _GeneralContext.HR_Master_Employee_DMO
                                                from b in _GeneralContext.HR_Master_Designation
                                                from c in _GeneralContext.HR_Master_Department
                                                where (a.MI_Id == b.MI_Id && a.HRMD_Id == c.HRMD_Id && a.HRMDES_Id == b.HRMDES_Id && a.HRMD_Id == data.HRMD_Id && b.HRMDES_Id == data.HRMDES_Id && a.MI_Id == data.MI_Id && a.HRME_EmployeeCode == I.Employeecode)
                                                select a.HRME_Id).FirstOrDefault();
                                if (data.ASMAY_Id > 0 && data.HRMDES_Id > 0 && data.HRME_Id > 0)
                                {

                                    var duplicate = _GeneralContext.NAAC_AC_TParticipation_113_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCACTP113_ParticipatedYear == data.ASMAY_Id && t.HRME_Id == data.HRME_Id && t.NCACTP113_PDate.Value.Date == Stdate).ToList();
                                    if (duplicate.Count > 0)
                                    {
                                        data.count += 1;
                                        data.msg = "Duplicate";
                                    }
                                    else
                                    {
                                        data.count1 += 1;
                                        NAAC_AC_TParticipation_113_DMO obj = new NAAC_AC_TParticipation_113_DMO();
                                        obj.MI_Id = data.MI_Id;
                                        obj.HRME_Id = data.HRME_Id;
                                        obj.NCACTP113_BodyName = I.Body;
                                        obj.NCACTP113_ParticipatedYear = data.ASMAY_Id;
                                        obj.NCACTP113_PDate = Stdate;
                                        obj.NCACTP113_ActiveFlg = true;
                                        obj.NCACTP113_CreatedBy = data.UserId;
                                        obj.NCACTP113_UpdatedBy = data.UserId;
                                        obj.NCACTP113_CreatedDate = DateTime.Now;
                                        obj.NCACTP113_UpdatedDate = DateTime.Now;
                                        obj.NCACTP113_StatusFlg = "";
                                        obj.NCACTP113_Remarks = "";
                                        obj.NCACTP113_FromExelImportFlag = true;
                                        obj.NCACTP113_FreezeFlag = true;
                                        _GeneralContext.Add(obj);
                                    }
                                }
                                else
                                {
                                    Listarray.Add(I);
                                }

                            }
                            else
                            {
                                Listarray.Add(I);
                            }
                        }
                        else
                        {
                            Listarray.Add(I);
                        }
                    }

                    int s = _GeneralContext.SaveChanges();
                    if (s > 0)
                    {
                        data.msg = "saved";
                    }
                    else
                    {
                        data.msg = "savingFailed";
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }

        //close
        public NAAC_AC_TParticipation_113_DTO savedata(NAAC_AC_TParticipation_113_DTO data)
        {
            try
            {
                if (data.NCACTP113_Id == 0)
                {
                    for (int e = 0; e < data.emplstdata.Length; e++)
                    {
                        var tempdata = data.emplstdata[e].HRME_Id;
                        var duplicate = _GeneralContext.NAAC_AC_TParticipation_113_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCACTP113_ParticipatedYear == data.ASMAY_Id && t.HRME_Id == tempdata && t.NCACTP113_PDate.Value.Date == data.NCACTP113_PDate.Value.Date).ToList();
                        if (duplicate.Count > 0)
                        {
                            data.count += 1;
                            data.msg = "Duplicate";
                        }
                        else
                        {
                            data.count1 += 1;
                            NAAC_AC_TParticipation_113_DMO obj = new NAAC_AC_TParticipation_113_DMO();
                            obj.MI_Id = data.MI_Id;
                            obj.HRME_Id = data.emplstdata[e].HRME_Id;
                            obj.NCACTP113_BodyName = data.NCACTP113_BodyName;
                            obj.NCACTP113_ParticipatedYear = data.ASMAY_Id;
                            obj.NCACTP113_PDate = data.NCACTP113_PDate;
                            obj.NCACTP113_ActiveFlg = true;
                            obj.NCACTP113_CreatedBy = data.UserId;
                            obj.NCACTP113_UpdatedBy = data.UserId;
                            obj.NCACTP113_CreatedDate = DateTime.Now;
                            obj.NCACTP113_UpdatedDate = DateTime.Now;
                            obj.NCACTP113_StatusFlg = "";
                            obj.NCACTP113_Remarks = "";
                            _GeneralContext.Add(obj);
                            if (data.filelist.Length > 0)
                            {
                                for (int j = 0; j < data.filelist.Length; j++)
                                {
                                    if (data.filelist[0].cfilepath != null)
                                    {
                                        NAAC_AC_TParticipation_113_FilesDMO obj2 = new NAAC_AC_TParticipation_113_FilesDMO();
                                        obj2.NCACTP113F_FileName = data.filelist[j].cfilename;
                                        obj2.NCACTP113F_Filedesc = data.filelist[j].cfiledesc;
                                        obj2.NCACTP113F_FilePath = data.filelist[j].cfilepath;
                                        obj2.NCACTP113_Id = obj.NCACTP113_Id;
                                        obj2.NCACTP113F_ActiveFlg = true;
                                        obj2.NCACTP113F_StatusFlg = "";
                                        obj2.NCACTP113F_Remarks = "";
                                        _GeneralContext.Add(obj2);
                                    }

                                }
                            }
                            int s = _GeneralContext.SaveChanges();
                            if (s > 0)
                            {
                                data.msg = "saved";
                            }
                            else
                            {
                                data.msg = "savingFailed";
                            }
                        }
                    }

                }
                else if (data.NCACTP113_Id > 0)
                {
                    for (int t = 0; t < data.emplstdata.Length; t++)
                    {
                        var tempdata = data.emplstdata[t].HRME_Id;
                        var duplicate = _GeneralContext.NAAC_AC_TParticipation_113_DMO.Where(b => b.MI_Id == data.MI_Id && b.NCACTP113_Id != data.NCACTP113_Id
                        && b.HRME_Id == tempdata && b.NCACTP113_ParticipatedYear == data.ASMAY_Id
                        && b.NCACTP113_PDate.Value.Date == data.NCACTP113_PDate.Value.Date).ToList();
                        if (duplicate.Count > 0)
                        {
                            data.count += 1;
                            data.msg = "Duplicate";
                        }
                        else
                        {
                            var update = _GeneralContext.NAAC_AC_TParticipation_113_DMO.Single(a => a.NCACTP113_Id == data.NCACTP113_Id);
                            update.HRME_Id = data.emplstdata[t].HRME_Id;
                            update.NCACTP113_BodyName = data.NCACTP113_BodyName;
                            update.NCACTP113_ParticipatedYear = data.NCACTP113_ParticipatedYear;
                            update.NCACTP113_PDate = data.NCACTP113_PDate;
                            update.NCACTP113_UpdatedBy = data.UserId;
                            update.NCACTP113_ParticipatedYear = data.ASMAY_Id;
                            update.NCACTP113_UpdatedDate = DateTime.Now;
                            _GeneralContext.Update(update);

                            var CountRemoveFiles = _GeneralContext.NAAC_AC_TParticipation_113_FilesDMO.Where(b => b.NCACTP113_Id == data.NCACTP113_Id).ToList();
                            List<long> temparr = new List<long>();
                            foreach (var c in data.filelist)
                            {
                                temparr.Add(c.cfileid);
                            }


                            var Phone_Noresultremove = _GeneralContext.NAAC_AC_TParticipation_113_FilesDMO.Where(c => !temparr.Contains(c.NCACTP113F_Id)
                            && c.NCACTP113_Id == data.NCACTP113_Id).ToList();

                            foreach (var ph1 in Phone_Noresultremove)
                            {
                                var resultremove112 = _GeneralContext.NAAC_AC_TParticipation_113_FilesDMO.Single(a => a.NCACTP113F_Id == ph1.NCACTP113F_Id);
                                resultremove112.NCACTP113F_ActiveFlg = false;
                                _GeneralContext.Update(resultremove112);

                            }

                            if (data.filelist.Length > 0)
                            {
                                for (int k = 0; k < data.filelist.Length; k++)
                                {
                                    var resultupload = _GeneralContext.NAAC_AC_TParticipation_113_FilesDMO.Where(a => a.NCACTP113_Id == data.NCACTP113_Id
                                    && a.NCACTP113F_Id == data.filelist[k].cfileid).ToList();
                                    if (resultupload.Count > 0)
                                    {
                                        var resultupdateupload = _GeneralContext.NAAC_AC_TParticipation_113_FilesDMO.Single(a => a.NCACTP113_Id == data.NCACTP113_Id
                                        && a.NCACTP113F_Id == data.filelist[k].cfileid);
                                        resultupdateupload.NCACTP113F_Filedesc = data.filelist[k].cfiledesc;
                                        resultupdateupload.NCACTP113F_FileName = data.filelist[k].cfilename;
                                        resultupdateupload.NCACTP113F_FilePath = data.filelist[k].cfilepath;
                                        _GeneralContext.Update(resultupdateupload);
                                    }
                                    else
                                    {
                                        if (data.filelist[k].cfilepath != null && data.filelist[k].cfilepath != "")
                                        {
                                            NAAC_AC_TParticipation_113_FilesDMO obj2 = new NAAC_AC_TParticipation_113_FilesDMO();
                                            obj2.NCACTP113F_FileName = data.filelist[k].cfilename;
                                            obj2.NCACTP113F_Filedesc = data.filelist[k].cfiledesc;
                                            obj2.NCACTP113F_FilePath = data.filelist[k].cfilepath;
                                            obj2.NCACTP113_Id = data.NCACTP113_Id;
                                            obj2.NCACTP113F_ActiveFlg = true;
                                            obj2.NCACTP113F_StatusFlg = "";
                                            obj2.NCACTP113F_Remarks = "";
                                            _GeneralContext.Add(obj2);
                                        }
                                    }
                                }
                            }

                            int s = _GeneralContext.SaveChanges();
                            if (s > 0)
                            {
                                data.msg = "updated";
                            }
                            else
                            {
                                data.msg = "updateFailed";
                            }



                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
        public NAAC_AC_TParticipation_113_DTO editdata(NAAC_AC_TParticipation_113_DTO data)
        {
            try
            {
                var edit = (from f in _GeneralContext.Academic
                            from a in _GeneralContext.NAAC_AC_TParticipation_113_DMO
                            from b in _GeneralContext.HR_Master_Employee_DMO
                            from d in _GeneralContext.HR_Master_Department
                            from e in _GeneralContext.HR_Master_Designation
                            where (a.MI_Id == b.MI_Id && b.HRME_Id == a.HRME_Id && b.HRMD_Id == d.HRMD_Id && b.HRMDES_Id == e.HRMDES_Id && a.NCACTP113_Id == data.NCACTP113_Id && a.NCACTP113_ParticipatedYear == f.ASMAY_Id && f.MI_Id == data.MI_Id && a.MI_Id == f.MI_Id)
                            select new NAAC_AC_TParticipation_113_DTO
                            {
                                HRME_Id = a.HRME_Id,
                                NCACTP113_BodyName = a.NCACTP113_BodyName,
                                NCACTP113_Id = a.NCACTP113_Id,
                                NCACTP113_ParticipatedYear = a.NCACTP113_ParticipatedYear,
                                HRMD_Id = d.HRMD_Id,
                                HRMDES_Id = e.HRMDES_Id,
                                NCACTP113_PDate = a.NCACTP113_PDate,
                            }).ToList();
                data.editlist = edit.ToArray();
                var hrmdid = edit.Select(a => a.HRMD_Id);
                data.designationlist = (from a in _GeneralContext.HR_Master_Employee_DMO
                                        from b in _GeneralContext.HR_Master_Designation
                                        where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag && a.HRMDES_Id == b.HRMDES_Id && hrmdid.Contains(a.HRMD_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true)
                                        select b).Distinct().ToArray();
                data.emplist = (from a in _GeneralContext.HR_Master_Employee_DMO
                                where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && hrmdid.Contains(a.HRMD_Id))
                                select new NAAC_AC_TParticipation_113_DTO
                                {
                                    empname = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                    HRME_Id = a.HRME_Id,
                                    HRME_EmployeeOrder = a.HRME_EmployeeOrder,
                                }).Distinct().OrderBy(a => a.HRME_EmployeeOrder).ToArray();
                data.editFileslist = (from a in _GeneralContext.NAAC_AC_TParticipation_113_FilesDMO
                                      where (a.NCACTP113_Id == data.NCACTP113_Id && a.NCACTP113F_ActiveFlg == true)
                                      select new NAAC_AC_TParticipation_113_DTO
                                      {
                                          cfilename = a.NCACTP113F_FileName,
                                          cfilepath = a.NCACTP113F_FilePath,
                                          cfiledesc = a.NCACTP113F_Filedesc,
                                          cfileid = a.NCACTP113F_Id,
                                      }).Distinct().ToArray();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
        public NAAC_AC_TParticipation_113_DTO deactivY(NAAC_AC_TParticipation_113_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_TParticipation_113_DMO.Where(t => t.NCACTP113_Id == data.NCACTP113_Id).SingleOrDefault();
                if (result.NCACTP113_ActiveFlg == true)
                {
                    result.NCACTP113_ActiveFlg = false;
                }
                else if (result.NCACTP113_ActiveFlg == false)
                {
                    result.NCACTP113_ActiveFlg = true;
                }
                result.NCACTP113_UpdatedDate = DateTime.Now;
                result.NCACTP113_UpdatedBy = data.UserId;
                _GeneralContext.Update(result);
                int s = _GeneralContext.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
        public NAAC_AC_TParticipation_113_DTO get_designation(NAAC_AC_TParticipation_113_DTO data)
        {
            try
            {
                data.designationlist = (from a in _GeneralContext.HR_Master_Employee_DMO
                                        from b in _GeneralContext.HR_Master_Designation
                                        where (a.MI_Id == b.MI_Id && a.HRMDES_Id == b.HRMDES_Id && a.HRMD_Id == data.HRMD_Id && a.MI_Id == data.MI_Id)
                                        select b).Distinct().OrderBy(t => t.HRMDES_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_TParticipation_113_DTO get_emp(NAAC_AC_TParticipation_113_DTO data)
        {
            try
            {
                data.emplist = (from a in _GeneralContext.HR_Master_Employee_DMO
                                from b in _GeneralContext.HR_Master_Designation
                                from c in _GeneralContext.HR_Master_Department
                                where (a.MI_Id == b.MI_Id && a.HRMD_Id == c.HRMD_Id && a.HRMDES_Id == b.HRMDES_Id && a.HRMD_Id == data.HRMD_Id && b.HRMDES_Id == data.HRMDES_Id && a.MI_Id == data.MI_Id)
                                select new NAAC_AC_TParticipation_113_DTO
                                {
                                    empname = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                    HRME_Id = a.HRME_Id,
                                    HRME_EmployeeOrder = a.HRME_EmployeeOrder,
                                    HRME_EmployeeCode = a.HRME_EmployeeCode
                                }).Distinct().OrderBy(t => t.HRME_EmployeeOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_TParticipation_113_DTO viewuploadflies(NAAC_AC_TParticipation_113_DTO data)
        {
            try
            {
                data.viewuploadflies = (from t in _GeneralContext.NAAC_AC_TParticipation_113_FilesDMO
                                        from b in _GeneralContext.NAAC_AC_TParticipation_113_DMO
                                        where (t.NCACTP113_Id == data.NCACTP113_Id && t.NCACTP113_Id == b.NCACTP113_Id && b.MI_Id == data.MI_Id && t.NCACTP113F_ActiveFlg == true)
                                        select new NAAC_AC_TParticipation_113_DTO
                                        {
                                            cfilename = t.NCACTP113F_FileName,
                                            cfilepath = t.NCACTP113F_FilePath,
                                            cfiledesc = t.NCACTP113F_Filedesc,
                                            NCACTP113F_Id = t.NCACTP113F_Id,
                                            NCACTP113_Id = b.NCACTP113_Id,
                                            NCACTP113F_StatusFlg = t.NCACTP113F_StatusFlg,
                                            NCACTP113F_ApprovedFlg = t.NCACTP113F_ApprovedFlg,
                                            MI_Id = b.MI_Id,
                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public NAAC_AC_TParticipation_113_DTO deleteuploadfile(NAAC_AC_TParticipation_113_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_TParticipation_113_FilesDMO.Where(t => t.NCACTP113F_Id == data.NCACTP113F_Id).ToList();
                if (result.Count > 0)
                {
                    foreach (var resultid in result)
                    {
                        _GeneralContext.Remove(resultid);
                    }
                }
                int row = _GeneralContext.SaveChanges();
                if (row > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
                data.viewuploadflies = (from t in _GeneralContext.NAAC_AC_TParticipation_113_FilesDMO
                                        from b in _GeneralContext.NAAC_AC_TParticipation_113_DMO
                                        where (t.NCACTP113_Id == data.NCACTP113_Id && t.NCACTP113_Id == b.NCACTP113_Id && b.MI_Id == data.MI_Id && t.NCACTP113F_ActiveFlg == true)
                                        select new NAAC_AC_TParticipation_113_DTO
                                        {
                                            cfilename = t.NCACTP113F_FileName,
                                            cfilepath = t.NCACTP113F_FilePath,
                                            cfiledesc = t.NCACTP113F_Filedesc,
                                            NCACTP113F_Id = t.NCACTP113F_Id,
                                            NCACTP113_Id = b.NCACTP113_Id,
                                            NCACTP113F_StatusFlg = t.NCACTP113F_StatusFlg,
                                            NCACTP113F_ApprovedFlg = t.NCACTP113F_ApprovedFlg,
                                            MI_Id = b.MI_Id,
                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }

        //add row wise comments
        public NAAC_AC_TParticipation_113_DTO savemedicaldatawisecomments(NAAC_AC_TParticipation_113_DTO data)
        {
            try
            {
                NAAC_AC_TParticipation_113_Comments_DMO obj1 = new NAAC_AC_TParticipation_113_Comments_DMO();

                obj1.NCACTP113C_Remarks = data.Remarks;
                obj1.NCACTP113C_RemarksBy = data.UserId;
                obj1.NCACTP113C_StatusFlg = "";
                obj1.NCACTP113_Id = data.filefkid;
                obj1.NCACTP113C_ActiveFlag = true;
                obj1.NCACTP113C_CreatedBy = data.UserId;
                obj1.NCACTP113C_UpdatedBy = data.UserId;
                obj1.NCACTP113C_CreatedDate = DateTime.Now;
                obj1.NCACTP113C_UpdatedDate = DateTime.Now;
                _GeneralContext.Add(obj1);
                int s = _GeneralContext.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        //add file wise comments
        public NAAC_AC_TParticipation_113_DTO savefilewisecomments(NAAC_AC_TParticipation_113_DTO data)
        {
            try
            {
                NAAC_AC_TParticipation_113_File_Comments_DMO obj1 = new NAAC_AC_TParticipation_113_File_Comments_DMO();

                obj1.NCACTP113FC_Remarks = data.Remarks;
                obj1.NCACTP113FC_RemarksBy = data.UserId;
                obj1.NCACTP113FC_StatusFlg = "";
                obj1.NCACTP113F_Id = data.filefkid;
                obj1.NCACTP113FC_ActiveFlag = true;
                obj1.NCACTP113FC_CreatedBy = data.UserId;
                obj1.NCACTP113FC_UpdatedBy = data.UserId;
                obj1.NCACTP113FC_CreatedDate = DateTime.Now;
                obj1.NCACTP113FC_UpdatedDate = DateTime.Now;

                _GeneralContext.Add(obj1);
                int s = _GeneralContext.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }

        // view row wise comments
        public NAAC_AC_TParticipation_113_DTO getcomment(NAAC_AC_TParticipation_113_DTO data)
        {
            try
            {
                data.commentlist = (from a in _GeneralContext.NAAC_AC_TParticipation_113_Comments_DMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCACTP113C_RemarksBy == b.Id && a.NCACTP113_Id == data.NCACTP113_Id)
                                    select new NAAC_AC_TParticipation_113_DTO
                                    {
                                        NCACTP113C_Remarks = a.NCACTP113C_Remarks,
                                        NCACTP113C_Id = a.NCACTP113C_Id,
                                        NCACTP113_Id = a.NCACTP113_Id,
                                        NCACTP113C_RemarksBy = a.NCACTP113C_RemarksBy,
                                        NCACTP113C_StatusFlg = a.NCACTP113C_StatusFlg,
                                        NCACTP113C_ActiveFlag = a.NCACTP113C_ActiveFlag,
                                        NCACTP113C_CreatedBy = a.NCACTP113C_CreatedBy,
                                        NCACTP113C_CreatedDate = a.NCACTP113C_CreatedDate,
                                        NCACTP113C_UpdatedBy = a.NCACTP113C_UpdatedBy,
                                        NCACTP113C_UpdatedDate = a.NCACTP113C_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NCACTP113C_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // view file wise comments
        public NAAC_AC_TParticipation_113_DTO getfilecomment(NAAC_AC_TParticipation_113_DTO data)
        {
            try
            {
                data.commentlist1 = (from a in _GeneralContext.NAAC_AC_TParticipation_113_File_Comments_DMO
                                     from b in _GeneralContext.ApplUser
                                     where (a.NCACTP113FC_RemarksBy == b.Id && a.NCACTP113F_Id == data.NCACTP113F_Id)
                                     select new NAAC_AC_TParticipation_113_DTO
                                     {
                                         NCACTP113F_Id = a.NCACTP113F_Id,
                                         NCACTP113FC_Remarks = a.NCACTP113FC_Remarks,
                                         NCACTP113FC_Id = a.NCACTP113FC_Id,
                                         NCACTP113FC_RemarksBy = a.NCACTP113FC_RemarksBy,
                                         NCACTP113FC_StatusFlg = a.NCACTP113FC_StatusFlg,
                                         NCACTP113FC_ActiveFlag = a.NCACTP113FC_ActiveFlag,
                                         NCACTP113FC_CreatedBy = a.NCACTP113FC_CreatedBy,
                                         NCACTP113FC_CreatedDate = a.NCACTP113FC_CreatedDate,
                                         NCACTP113FC_UpdatedBy = a.NCACTP113FC_UpdatedBy,
                                         NCACTP113FC_UpdatedDate = a.NCACTP113FC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().OrderByDescending(a => a.NCACTP113C_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



    }
}
