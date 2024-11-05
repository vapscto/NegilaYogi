using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using DomainModel.Model.NAAC.University;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.University.Service
{
    public class HSU_362_ExtensionActivitiesImpl:Interface.HSU_362_ExtensionActivitiesInterface
    {
        public GeneralContext _context;
        public HSU_362_ExtensionActivitiesImpl(GeneralContext y)
        {
            _context = y;
        }
        public HSU_362_ExtensionActivitiesDTO loaddata(HSU_362_ExtensionActivitiesDTO data)
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
                data.alldata1 = (from a in _context.Academic
                                 from b in _context.NAAC_AC_343_StudentActivities_DMO
                                 where (a.MI_Id == data.MI_Id && b.MI_Id == a.MI_Id && a.Is_Active == true && b.NCACSA343_Year == a.ASMAY_Id)
                                 select new HSU_362_ExtensionActivitiesDTO
                                 {
                                     NCACSA343_Id = b.NCACSA343_Id,
                                     NCACSA343_Year = b.NCACSA343_Year,
                                     ASMAY_Year = a.ASMAY_Year,
                                     NCACSA343_TypeOfActivity = b.NCACSA343_TypeOfActivity,
                                     NCACSA343_OrgAgency = b.NCACSA343_OrgAgency,
                                     NCACSA343_NoOfStudents = b.NCACSA343_NoOfStudents,
                                     NCACSA343_NoOfTeachers = b.NCACSA343_NoOfTeachers,
                                     NCACSA343_ActiveFlg = b.NCACSA343_ActiveFlg,
                                     MI_Id = data.MI_Id

                                 }).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public HSU_362_ExtensionActivitiesDTO save(HSU_362_ExtensionActivitiesDTO data)
        {
            try
            {
                if (data.NCACSA343_Id == 0)
                {
                    var duplicate = _context.NAAC_AC_343_StudentActivities_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCACSA343_Year == data.asmaY_Id && t.NCACSA343_Id != 0 && t.NCACSA343_TypeOfActivity == data.NCACSA343_TypeOfActivity && t.NCACSA343_OrgAgency == data.NCACSA343_OrgAgency && t.NCACSA343_NoOfStudents == data.NCACSA343_NoOfStudents && t.NCACSA343_NoOfTeachers == data.NCACSA343_NoOfTeachers).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_343_StudentActivities_DMO rrr = new NAAC_AC_343_StudentActivities_DMO();
                        rrr.MI_Id = data.MI_Id;
                        rrr.NCACSA343_TypeOfActivity = data.NCACSA343_TypeOfActivity;
                        rrr.NCACSA343_OrgAgency = data.NCACSA343_OrgAgency;
                        rrr.NCACSA343_NoOfStudents = data.NCACSA343_NoOfStudents;
                        rrr.NCACSA343_NoOfTeachers = data.NCACSA343_NoOfTeachers;
                        rrr.NCACSA343_Year = data.asmaY_Id;
                        rrr.NCACSA343_CreatedDate = DateTime.Now;
                        rrr.NCACSA343_UpdatedDate = DateTime.Now;
                        rrr.NCACSA343_ActiveFlg = true;
                        rrr.NCACSA343_CreatedBy = data.UserId;
                        rrr.NCACSA343_UpdatedBy = data.UserId;
                        _context.Add(rrr);
                        if (data.filelist.Length > 0)
                        {
                            for (int i = 0; i < data.filelist.Length; i++)
                            {
                                if (data.filelist[0].cfilepath != null)
                                {
                                    NAAC_AC_343_StudentActivities_Files_DMO obj2 = new NAAC_AC_343_StudentActivities_Files_DMO();
                                    obj2.NCACSA343_Id = rrr.NCACSA343_Id;
                                    obj2.NCACSA343F_FileName = data.filelist[i].cfilename;
                                    obj2.NCACSA343F_Filedesc = data.filelist[i].cfiledesc;
                                    obj2.NCACSA343F_FilePath = data.filelist[i].cfilepath;
                                    _context.Add(obj2);
                                }
                            }
                        }
                        int y = _context.SaveChanges();
                        if (y > 0)
                        {
                            data.msg = "saved";
                        }
                        else
                        {
                            data.msg = "Failed";
                        }
                    }
                }
                else if (data.NCACSA343_Id > 0)
                {
                    var duplicate = _context.NAAC_AC_343_StudentActivities_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCACSA343_TypeOfActivity == data.NCACSA343_TypeOfActivity && t.NCACSA343_OrgAgency == data.NCACSA343_OrgAgency && t.NCACSA343_Year == data.asmaY_Id && t.NCACSA343_Id != data.NCACSA343_Id && t.NCACSA343_NoOfStudents == data.NCACSA343_NoOfStudents && t.NCACSA343_NoOfTeachers == data.NCACSA343_NoOfTeachers).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var yy = _context.NAAC_AC_343_StudentActivities_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCACSA343_Id == data.NCACSA343_Id).SingleOrDefault();
                        yy.NCACSA343_UpdatedBy = data.UserId;
                        yy.NCACSA343_TypeOfActivity = data.NCACSA343_TypeOfActivity;
                        yy.NCACSA343_OrgAgency = data.NCACSA343_OrgAgency;
                        yy.NCACSA343_NoOfStudents = data.NCACSA343_NoOfStudents;
                        yy.NCACSA343_NoOfTeachers = data.NCACSA343_NoOfTeachers;
                        yy.NCACSA343_Year = data.asmaY_Id;
                        yy.MI_Id = data.MI_Id;
                        yy.NCACSA343_UpdatedDate = DateTime.Now;
                        _context.Update(yy);
                        var CountRemoveFiles = _context.NAAC_AC_343_StudentActivities_Files_DMO.Where(t => t.NCACSA343_Id == data.NCACSA343_Id).ToList();
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
                                        NAAC_AC_343_StudentActivities_Files_DMO obj2 = new NAAC_AC_343_StudentActivities_Files_DMO();
                                        obj2.NCACSA343_Id = yy.NCACSA343_Id;
                                        obj2.NCACSA343F_FileName = data.filelist[i].cfilename;
                                        obj2.NCACSA343F_Filedesc = data.filelist[i].cfiledesc;
                                        obj2.NCACSA343F_FilePath = data.filelist[i].cfilepath;
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
                                        NAAC_AC_343_StudentActivities_Files_DMO obj2 = new NAAC_AC_343_StudentActivities_Files_DMO();
                                        obj2.NCACSA343_Id = yy.NCACSA343_Id;
                                        obj2.NCACSA343F_FileName = data.filelist[i].cfilename;
                                        obj2.NCACSA343F_Filedesc = data.filelist[i].cfiledesc;
                                        obj2.NCACSA343F_FilePath = data.filelist[i].cfilepath;
                                        _context.Add(obj2);
                                    }
                                }
                            }
                        }
                        var r = _context.SaveChanges();
                        if (r > 0)
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
        public HSU_362_ExtensionActivitiesDTO deactive(HSU_362_ExtensionActivitiesDTO data)
        {
            try
            {
                var u = _context.NAAC_AC_343_StudentActivities_DMO.Where(t => t.NCACSA343_Id == data.NCACSA343_Id).SingleOrDefault();
                if (u.NCACSA343_ActiveFlg == true)
                {
                    u.NCACSA343_ActiveFlg = false;
                }
                else if (u.NCACSA343_ActiveFlg == false)
                {
                    u.NCACSA343_ActiveFlg = true;
                }
                u.NCACSA343_UpdatedDate = DateTime.Now;
                u.NCACSA343_UpdatedBy = data.UserId;
                u.MI_Id = data.MI_Id;
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
        public HSU_362_ExtensionActivitiesDTO EditData(HSU_362_ExtensionActivitiesDTO data)
        {
            try
            {
                data.editlist = (from a in _context.Academic
                                 from b in _context.NAAC_AC_343_StudentActivities_DMO
                                 where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCACSA343_Year && b.MI_Id == data.MI_Id && b.NCACSA343_Id == data.NCACSA343_Id)
                                 select new HSU_362_ExtensionActivitiesDTO
                                 {
                                     NCACSA343_Id = b.NCACSA343_Id,
                                     NCACSA343_TypeOfActivity = b.NCACSA343_TypeOfActivity,
                                     NCACSA343_OrgAgency = b.NCACSA343_OrgAgency,
                                     NCACSA343_NoOfStudents = b.NCACSA343_NoOfStudents,
                                     NCACSA343_NoOfTeachers = b.NCACSA343_NoOfTeachers,
                                     NCACSA343_Year = b.NCACSA343_Year,
                                     MI_Id = data.MI_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();
                data.editFileslist = (from a in _context.NAAC_AC_343_StudentActivities_Files_DMO
                                      where (a.NCACSA343_Id == data.NCACSA343_Id)
                                      select new HSU_362_ExtensionActivitiesDTO
                                      {
                                          cfilename = a.NCACSA343F_FileName,
                                          cfilepath = a.NCACSA343F_FilePath,
                                          cfiledesc = a.NCACSA343F_Filedesc,
                                      }).Distinct().ToArray();
            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public HSU_362_ExtensionActivitiesDTO viewuploadflies(HSU_362_ExtensionActivitiesDTO data)
        {
            try
            {
                data.viewuploadflies = (from a in _context.NAAC_AC_343_StudentActivities_Files_DMO
                                        where (a.NCACSA343_Id == data.NCACSA343_Id)
                                        select new HSU_362_ExtensionActivitiesDTO
                                        {
                                            cfilename = a.NCACSA343F_FileName,
                                            cfilepath = a.NCACSA343F_FilePath,
                                            cfiledesc = a.NCACSA343F_Filedesc,
                                            NCACSA343F_Id = a.NCACSA343F_Id,
                                            NCACSA343_Id = a.NCACSA343_Id,
                                        }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public HSU_362_ExtensionActivitiesDTO deleteuploadfile(HSU_362_ExtensionActivitiesDTO data)
        {
            try
            {
                var res = _context.NAAC_AC_343_StudentActivities_Files_DMO.Where(t => t.NCACSA343F_Id == data.NCACSA343F_Id).SingleOrDefault();
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
                data.viewuploadflies = (from a in _context.NAAC_AC_343_StudentActivities_Files_DMO
                                        where (a.NCACSA343_Id == data.NCACSA343_Id)
                                        select new HSU_362_ExtensionActivitiesDTO
                                        {
                                            NCACSA343_Id = a.NCACSA343_Id,
                                            NCACSA343F_Id = a.NCACSA343F_Id,
                                            cfilename = a.NCACSA343F_FileName,
                                            cfilepath = a.NCACSA343F_FilePath,
                                            cfiledesc = a.NCACSA343F_Filedesc,
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
