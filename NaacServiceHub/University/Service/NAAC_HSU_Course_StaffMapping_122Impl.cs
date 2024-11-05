using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.University;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.University.Service
{
    public class NAAC_HSU_Course_StaffMapping_122Impl : Interface.NAAC_HSU_Course_StaffMapping_122Interface
    {
        public GeneralContext _context;
        public NAAC_HSU_Course_StaffMapping_122Impl(GeneralContext y)
        {
            _context = y;
        }
        public NAAC_HSU_Course_StaffMapping_122DTO loaddata(NAAC_HSU_Course_StaffMapping_122DTO data)
        {
            try
            {
                var institutionlist = (from a in _context.Institution
                                       from b in _context.UserRoleWithInstituteDMO
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
                data.allacademicyear = _context.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).ToArray();

                data.departmentlist = _context.HR_Master_Department.Where(t => t.MI_Id == data.MI_Id && t.HRMD_ActiveFlag == true).Distinct().OrderBy(t => t.HRMD_Order).ToArray();

                data.alldata1 = (from a in _context.Academic
                                 from b in _context.NAAC_HSU_Course_StaffMapping_122DMO
                                 from c in _context.HR_Master_Department
                                 from d in _context.HR_Master_Designation
                                 from e in _context.HR_Master_Employee_DMO
                                 from f in _context.MasterCourseDMO
                                 from g in _context.CLG_Adm_College_AY_CourseDMO
                                 where (a.MI_Id == data.MI_Id && c.MI_Id == d.MI_Id && d.MI_Id == e.MI_Id && a.MI_Id == f.MI_Id && a.MI_Id == g.MI_Id && a.Is_Active == true && g.ASMAY_Id == a.ASMAY_Id && c.HRMD_Id == e.HRMD_Id && d.HRMDES_Id == e.HRMDES_Id && f.AMCO_Id == g.AMCO_Id && g.ACAYC_Id == b.ACAYC_Id && e.HRME_Id == b.HRME_Id)
                                 select new NAAC_HSU_Course_StaffMapping_122DTO
                                 {
                                     NCHSUSM122_Id = b.NCHSUSM122_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                     HRMD_Id = c.HRMD_Id,
                                     HRMD_DepartmentName = c.HRMD_DepartmentName,
                                     ACAYC_Id = g.ACAYC_Id,
                                     AMCO_Id = f.AMCO_Id,
                                     AMCO_CourseName = f.AMCO_CourseName,
                                     HRME_Id = e.HRME_Id,
                                     empname = ((e.HRME_EmployeeFirstName == null ? " " : e.HRME_EmployeeFirstName) + " " + (e.HRME_EmployeeMiddleName == null ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null ? " " : e.HRME_EmployeeLastName)).Trim(),
                                     HRMDES_Id = d.HRMDES_Id,
                                     HRMDES_DesignationName = d.HRMDES_DesignationName,
                                     NCHSUSM122_ActiveFlag = b.NCHSUSM122_ActiveFlag,
                                     MI_Id = data.MI_Id

                                 }).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_HSU_Course_StaffMapping_122DTO save(NAAC_HSU_Course_StaffMapping_122DTO data)
        {
            try
            {
                if (data.NCHSUSM122_Id == 0)
                {
                    var duplicate = _context.NAAC_HSU_Course_StaffMapping_122DMO.Where(t => t.NCHSUSM122_Id != 0 && t.HRME_Id == data.HRME_Id && t.ACAYC_Id == data.ACAYC_Id).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_HSU_Course_StaffMapping_122DMO obj = new NAAC_HSU_Course_StaffMapping_122DMO();

                        obj.HRME_Id = data.HRME_Id;
                        obj.ACAYC_Id = data.ACAYC_Id;
                        obj.NCHSUSM122_CreatedDate = DateTime.Now;
                        obj.NCHSUSM122_UpdatedDate = DateTime.Now;
                        obj.NCHSUSM122_CreatedBy = data.UserId;
                        obj.NCHSUSM122_UpdatedBy = data.UserId;
                        obj.NCHSUSM122_ActiveFlag = true;

                        _context.Add(obj);
                        if (data.filelist.Length > 0)
                        {
                            for (int i = 0; i < data.filelist.Length; i++)
                            {
                                if (data.filelist[0].cfilepath != null)
                                {
                                    NAAC_HSU_Course_StaffMapping_122_FilesDMO obj2 = new NAAC_HSU_Course_StaffMapping_122_FilesDMO();
                                    obj2.NCHSUSM122_Id = obj.NCHSUSM122_Id;
                                    obj2.NCHSUSM122F_FileName = data.filelist[i].cfilename;
                                    obj2.NCHSUSM122F_FileDesc = data.filelist[i].cfiledesc;
                                    obj2.NCHSUSM122F_FilePath = data.filelist[i].cfilepath;
                                    _context.Add(obj2);
                                }
                            }
                        }
                        int row = _context.SaveChanges();
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
                else if (data.NCHSUSM122_Id > 0)
                {
                    var duplicate = _context.NAAC_HSU_Course_StaffMapping_122DMO.Where(t => t.NCHSUSM122_Id != data.NCHSUSM122_Id && t.HRME_Id == data.HRME_Id && t.ACAYC_Id == data.ACAYC_Id).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _context.NAAC_HSU_Course_StaffMapping_122DMO.Where(t => t.NCHSUSM122_Id == data.NCHSUSM122_Id).SingleOrDefault();

                        update.NCHSUSM122_UpdatedBy = data.UserId;
                        update.HRME_Id = data.HRME_Id;
                        update.ACAYC_Id = data.ACAYC_Id;
                        update.NCHSUSM122_UpdatedDate = DateTime.Now;
                        _context.Update(update);

                        var CountRemoveFiles = _context.NAAC_HSU_Course_StaffMapping_122_FilesDMO.Where(t => t.NCHSUSM122_Id == data.NCHSUSM122_Id).ToList();

                        if (CountRemoveFiles.Count > 0)
                        {
                            foreach (var RemoveFiles in CountRemoveFiles)
                            {
                                _context.Remove(RemoveFiles);
                            }
                            if (data.filelist.Length > 0)
                            {
                                for (int i = 0; i < data.filelist.Length; i++)
                                {
                                    if (data.filelist[0].cfilepath != null)
                                    {
                                        NAAC_HSU_Course_StaffMapping_122_FilesDMO obj2 = new NAAC_HSU_Course_StaffMapping_122_FilesDMO();
                                        obj2.NCHSUSM122_Id = update.NCHSUSM122_Id;
                                        obj2.NCHSUSM122F_FileName = data.filelist[i].cfilename;
                                        obj2.NCHSUSM122F_FileDesc = data.filelist[i].cfiledesc;
                                        obj2.NCHSUSM122F_FilePath = data.filelist[i].cfilepath;
                                        _context.Add(obj2);
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
                                        NAAC_HSU_Course_StaffMapping_122_FilesDMO obj2 = new NAAC_HSU_Course_StaffMapping_122_FilesDMO();
                                        obj2.NCHSUSM122_Id = data.NCHSUSM122_Id;
                                        obj2.NCHSUSM122F_FileName = data.filelist[i].cfilename;
                                        obj2.NCHSUSM122F_FileDesc = data.filelist[i].cfiledesc;
                                        obj2.NCHSUSM122F_FilePath = data.filelist[i].cfilepath;
                                        _context.Add(obj2);
                                    }
                                }
                            }
                        }
                        var row = _context.SaveChanges();
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
        public NAAC_HSU_Course_StaffMapping_122DTO deactive(NAAC_HSU_Course_StaffMapping_122DTO data)
        {
            try
            {
                var u = _context.NAAC_HSU_Course_StaffMapping_122DMO.Where(t => t.NCHSUSM122_Id == data.NCHSUSM122_Id).SingleOrDefault();
                if (u.NCHSUSM122_ActiveFlag == true)
                {
                    u.NCHSUSM122_ActiveFlag = false;
                }
                else if (u.NCHSUSM122_ActiveFlag == false)
                {
                    u.NCHSUSM122_ActiveFlag = true;
                }
                u.NCHSUSM122_UpdatedDate = DateTime.Now;
                u.NCHSUSM122_UpdatedBy = data.UserId;
                // u.MI_Id = data.MI_Id;
                _context.Update(u);
                int o = _context.SaveChanges();
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
        public NAAC_HSU_Course_StaffMapping_122DTO EditData(NAAC_HSU_Course_StaffMapping_122DTO data)
        {
            try
            {
                var edit = (from a in _context.NAAC_HSU_Course_StaffMapping_122DMO
                            where (a.NCHSUSM122_Id == data.NCHSUSM122_Id)
                            select new NAAC_HSU_Course_StaffMapping_122DTO
                            {
                                NCHSUSM122_Id = a.NCHSUSM122_Id,
                                ACAYC_Id = a.ACAYC_Id,
                                HRME_Id = a.HRME_Id,
                            }).Distinct().ToList();

                if (edit.Count > 0)
                {
                    data.ACAYC_Id = edit[0].ACAYC_Id;
                    data.HRME_Id = edit[0].HRME_Id;
                    data.NCHSUSM122_Id = edit[0].NCHSUSM122_Id;

                    var editYCourse = (from a in _context.CLG_Adm_College_AY_CourseDMO
                                       where (a.ACAYC_Id == data.ACAYC_Id)
                                       select new NAAC_HSU_Course_StaffMapping_122DTO
                                       {
                                           ACAYC_Id = a.ACAYC_Id,
                                           MI_Id = a.MI_Id,
                                           asmaY_Id = a.ASMAY_Id,
                                           AMCO_Id = a.AMCO_Id,
                                       }).Distinct().ToList();

                    data.editlist = editYCourse.ToArray();

                    data.MI_Id = editYCourse[0].MI_Id;
                    data.asmaY_Id = editYCourse[0].asmaY_Id;
                    data.AMCO_Id = editYCourse[0].AMCO_Id;

                    data.courselist = (from a in _context.MasterCourseDMO
                                       from b in _context.CLG_Adm_College_AY_CourseDMO
                                       where (b.ASMAY_Id == data.asmaY_Id && b.MI_Id == data.MI_Id && a.AMCO_Id == b.AMCO_Id && b.ACAYC_ActiveFlag == true && a.AMCO_ActiveFlag == true)
                                       select new NAAC_HSU_Course_StaffMapping_122DTO
                                       {
                                           ACAYC_Id = b.ACAYC_Id,
                                           AMCO_Id = b.AMCO_Id,
                                           AMCO_CourseName = a.AMCO_CourseName,
                                           AMCO_Order = a.AMCO_Order,
                                       }).Distinct().OrderBy(t => t.AMCO_Order).ToArray();



                    var empdata = (from a in _context.HR_Master_Employee_DMO
                                   where (a.HRME_Id == data.HRME_Id && a.MI_Id == data.MI_Id)
                                   select new NAAC_HSU_Course_StaffMapping_122DTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       empname = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                       HRMD_Id = a.HRMD_Id,
                                       HRMDES_Id = a.HRMDES_Id,
                                   }).Distinct().ToList();

                    data.HRMD_Id = empdata[0].HRMD_Id;
                    data.HRMDES_Id = empdata[0].HRMDES_Id;

                    data.designationlist = (from b in _context.HR_Master_Designation
                                            where (b.HRMDES_Id == data.HRMDES_Id && b.MI_Id == data.MI_Id)
                                            select new NAAC_HSU_Course_StaffMapping_122DTO
                                            {
                                                HRMDES_Id = b.HRMDES_Id,
                                                HRMDES_DesignationName = b.HRMDES_DesignationName,
                                            }).Distinct().ToArray();

                    data.employeelist = empdata.ToArray();

                    //data.editYear = (from a in _context.Academic
                    //                 where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.asmaY_Id && a.Is_Active == true)
                    //                 select new NAAC_HSU_Course_StaffMapping_122DTO
                    //                 {
                    //                     asmaY_Id = a.ASMAY_Id,
                    //                     ASMAY_Year = a.ASMAY_Year
                    //                 }).Distinct().ToArray();

                }



                data.editFileslist = (from a in _context.NAAC_HSU_Course_StaffMapping_122_FilesDMO
                                      where (a.NCHSUSM122_Id == data.NCHSUSM122_Id)
                                      select new NAAC_HSU_Course_StaffMapping_122DTO
                                      {
                                          cfilename = a.NCHSUSM122F_FileName,
                                          cfilepath = a.NCHSUSM122F_FilePath,
                                          cfiledesc = a.NCHSUSM122F_FileDesc,
                                      }).Distinct().ToArray();
            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAAC_HSU_Course_StaffMapping_122DTO viewuploadflies(NAAC_HSU_Course_StaffMapping_122DTO data)
        {
            try
            {
                data.viewuploadflies = (from a in _context.NAAC_HSU_Course_StaffMapping_122_FilesDMO
                                        where (a.NCHSUSM122_Id == data.NCHSUSM122_Id)
                                        select new NAAC_HSU_Course_StaffMapping_122DTO
                                        {
                                            cfilename = a.NCHSUSM122F_FileName,
                                            cfilepath = a.NCHSUSM122F_FilePath,
                                            cfiledesc = a.NCHSUSM122F_FileDesc,
                                            NCHSUSM122F_Id = a.NCHSUSM122F_Id,
                                            NCHSUSM122_Id = a.NCHSUSM122_Id,
                                        }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_HSU_Course_StaffMapping_122DTO deleteuploadfile(NAAC_HSU_Course_StaffMapping_122DTO data)
        {
            try
            {
                var res = _context.NAAC_HSU_Course_StaffMapping_122_FilesDMO.Where(t => t.NCHSUSM122F_Id == data.NCHSUSM122F_Id).SingleOrDefault();
                _context.Remove(res);
                int s = _context.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
                data.viewuploadflies = (from a in _context.NAAC_HSU_Course_StaffMapping_122_FilesDMO
                                        where (a.NCHSUSM122_Id == data.NCHSUSM122_Id)
                                        select new NAAC_HSU_Course_StaffMapping_122DTO
                                        {
                                            NCHSUSM122_Id = a.NCHSUSM122_Id,
                                            NCHSUSM122F_Id = a.NCHSUSM122F_Id,
                                            cfilename = a.NCHSUSM122F_FileName,
                                            cfilepath = a.NCHSUSM122F_FilePath,
                                            cfiledesc = a.NCHSUSM122F_FileDesc,
                                        }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_HSU_Course_StaffMapping_122DTO get_course(NAAC_HSU_Course_StaffMapping_122DTO data)
        {
            try
            {
                data.courselist = (from a in _context.MasterCourseDMO
                                   from b in _context.CLG_Adm_College_AY_CourseDMO
                                   where (b.ASMAY_Id == data.asmaY_Id && b.MI_Id == data.MI_Id && a.AMCO_Id == b.AMCO_Id && b.ACAYC_ActiveFlag == true && a.AMCO_ActiveFlag == true)
                                   select new NAAC_HSU_Course_StaffMapping_122DTO
                                   {
                                       ACAYC_Id = b.ACAYC_Id,
                                       AMCO_Id = b.AMCO_Id,
                                       AMCO_CourseName = a.AMCO_CourseName,
                                       AMCO_Order = a.AMCO_Order,
                                   }).Distinct().OrderBy(t => t.AMCO_Order).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_HSU_Course_StaffMapping_122DTO get_designation(NAAC_HSU_Course_StaffMapping_122DTO data)
        {
            try
            {
                data.designationlist = (from a in _context.HR_Master_Employee_DMO
                                        from b in _context.HR_Master_Designation
                                        from d in _context.HR_Master_Department
                                        where (a.HRMD_Id == data.HRMD_Id && a.MI_Id == data.MI_Id
                                     && a.HRMD_Id == d.HRMD_Id && a.HRMDES_Id == b.HRMDES_Id && a.HRME_ActiveFlag == true
                                     && a.HRME_LeftFlag == false)
                                        select new NAAC_HSU_Course_StaffMapping_122DTO
                                        {
                                            HRMDES_Id = a.HRMDES_Id,
                                            HRMDES_DesignationName = b.HRMDES_DesignationName,
                                        }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_HSU_Course_StaffMapping_122DTO get_employee(NAAC_HSU_Course_StaffMapping_122DTO data)
        {
            try
            {
                data.employeelist = (from a in _context.HR_Master_Employee_DMO
                                     from b in _context.HR_Master_Designation
                                     from d in _context.HR_Master_Department
                                     where (a.HRMDES_Id == data.HRMDES_Id && a.HRMD_Id == data.HRMD_Id && a.MI_Id == data.MI_Id
                                     && a.HRMD_Id == d.HRMD_Id && a.HRMDES_Id == b.HRMDES_Id && a.HRME_ActiveFlag == true
                                     && a.HRME_LeftFlag == false)
                                     select new NAAC_HSU_Course_StaffMapping_122DTO
                                     {
                                         HRME_Id = a.HRME_Id,
                                         empname = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim()
                                     }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

    }
}
