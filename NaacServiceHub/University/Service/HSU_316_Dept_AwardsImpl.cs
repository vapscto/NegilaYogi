using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.University;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.University.Service
{
    public class HSU_316_Dept_AwardsImpl:Interface.HSU_316_Dept_AwardsInterface
    {
        public GeneralContext _context;
        public HSU_316_Dept_AwardsImpl(GeneralContext y)
        {
            _context = y;
        }
        public HSU_316_Dept_AwardsDTO loaddata(HSU_316_Dept_AwardsDTO data)
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

                data.departmentlist = _context.HR_Master_Department.Where(t => t.MI_Id == data.MI_Id && t.HRMD_ActiveFlag == true).Distinct().ToArray();

                data.alldata1 = (from a in _context.Academic
                                 from b in _context.NAAC_HSU_316_Dept_AwardsDMO
                                 from c in _context.HR_Master_Department
                                 where (a.MI_Id == data.MI_Id && b.MI_Id == a.MI_Id && a.Is_Active == true && b.NMC316DA_Year == a.ASMAY_Id && c.HRMD_Id==b.HRMD_Id && c.HRMD_DepartmentName==data.HRMD_DepartmentName)
                                 select new HSU_316_Dept_AwardsDTO
                                 {
                                     NMC316DA_Id = b.NMC316DA_Id,
                                     NMC316DA_Year = b.NMC316DA_Year,
                                     ASMAY_Year = a.ASMAY_Year,
                                     HRMD_Id = b.HRMD_Id,
                                     HRMD_DepartmentName=c.HRMD_DepartmentName,
                                     NMC316DA_Scheme = b.NMC316DA_Scheme,
                                     NMC316DA_Agency = b.NMC316DA_Agency,
                                     NMC316DA_FoundProvided = b.NMC316DA_FoundProvided,
                                     NMC316DA_Duration = b.NMC316DA_Duration,
                                     NMC316DA_ActiveFlag = b.NMC316DA_ActiveFlag,
                                     MI_Id = data.MI_Id

                                 }).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public HSU_316_Dept_AwardsDTO save(HSU_316_Dept_AwardsDTO data)
        {
            try
            {
                if (data.NMC316DA_Id == 0)
                {
                    var duplicate = _context.NAAC_HSU_316_Dept_AwardsDMO.Where(t => t.MI_Id == data.MI_Id && t.NMC316DA_Year == data.asmaY_Id && t.NMC316DA_Id != 0 && t.HRMD_Id == data.HRMD_Id && t.NMC316DA_Scheme == data.NMC316DA_Scheme && t.NMC316DA_Agency == data.NMC316DA_Agency && t.NMC316DA_FoundProvided==data.NMC316DA_FoundProvided && t.NMC316DA_Duration==data.NMC316DA_Duration).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_HSU_316_Dept_AwardsDMO rrr = new NAAC_HSU_316_Dept_AwardsDMO();
                        rrr.MI_Id = data.MI_Id;
                        rrr.HRMD_Id = data.HRMD_Id;
                        rrr.NMC316DA_Scheme = data.NMC316DA_Scheme;
                        rrr.NMC316DA_Agency = data.NMC316DA_Agency;
                        rrr.NMC316DA_FoundProvided = data.NMC316DA_FoundProvided;
                        rrr.NMC316DA_Duration = data.NMC316DA_Duration;
                        rrr.NMC316DA_Year = data.asmaY_Id;
                        rrr.NMC316DA_CreatedDate = DateTime.Now;
                        rrr.NMC316DA_UpdatedDate = DateTime.Now;
                        rrr.NMC316DA_ActiveFlag = true;
                       
                        _context.Add(rrr);
                        if (data.filelist.Length > 0)
                        {
                            for (int i = 0; i < data.filelist.Length; i++)
                            {
                                if (data.filelist[0].cfilepath != null)
                                {
                                    NAAC_HSU_316_Dept_Awards_FilesDMO obj2 = new NAAC_HSU_316_Dept_Awards_FilesDMO();
                                    obj2.NMC316DA_Id = rrr.NMC316DA_Id;
                                    obj2.NMC316DAF_FileName = data.filelist[i].cfilename;
                                    obj2.NMC316DAF_FileDescription = data.filelist[i].cfiledesc;
                                    obj2.NMC316DAF_FilePath = data.filelist[i].cfilepath;
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
                else if (data.NMC316DA_Id > 0)
                {
                    var duplicate = _context.NAAC_HSU_316_Dept_AwardsDMO.Where(t => t.MI_Id == data.MI_Id && t.HRMD_Id == data.HRMD_Id && t.NMC316DA_Scheme == data.NMC316DA_Scheme && t.NMC316DA_Agency == data.NMC316DA_Agency && t.NMC316DA_FoundProvided == data.NMC316DA_FoundProvided && t.NMC316DA_Duration == data.NMC316DA_Duration && t.NMC316DA_Id != data.NMC316DA_Id && t.NMC316DA_Year == data.asmaY_Id).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var yy = _context.NAAC_HSU_316_Dept_AwardsDMO.Where(t => t.MI_Id == data.MI_Id && t.NMC316DA_Id == data.NMC316DA_Id).SingleOrDefault();
                       // yy.NCMCTT343_UpdatedBy = data.UserId;
                        yy.HRMD_Id = data.HRMD_Id;
                        yy.NMC316DA_Scheme = data.NMC316DA_Scheme;
                        yy.NMC316DA_Agency = data.NMC316DA_Agency;
                        yy.NMC316DA_FoundProvided = data.NMC316DA_FoundProvided;
                        yy.NMC316DA_Year = data.asmaY_Id;
                        yy.MI_Id = data.MI_Id;
                        yy.NMC316DA_UpdatedDate = DateTime.Now;
                        _context.Update(yy);
                        var CountRemoveFiles = _context.NAAC_HSU_316_Dept_Awards_FilesDMO.Where(t => t.NMC316DA_Id == data.NMC316DA_Id).ToList();
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
                                        NAAC_HSU_316_Dept_Awards_FilesDMO obj2 = new NAAC_HSU_316_Dept_Awards_FilesDMO();
                                        obj2.NMC316DA_Id = yy.NMC316DA_Id;
                                        obj2.NMC316DAF_FileName = data.filelist[i].cfilename;
                                        obj2.NMC316DAF_FileDescription = data.filelist[i].cfiledesc;
                                        obj2.NMC316DAF_FilePath = data.filelist[i].cfilepath;
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
                                        NAAC_HSU_316_Dept_Awards_FilesDMO obj2 = new NAAC_HSU_316_Dept_Awards_FilesDMO();
                                        obj2.NMC316DA_Id = yy.NMC316DA_Id;
                                        obj2.NMC316DAF_FileName = data.filelist[i].cfilename;
                                        obj2.NMC316DAF_FileDescription = data.filelist[i].cfiledesc;
                                        obj2.NMC316DAF_FilePath = data.filelist[i].cfilepath;
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
        public HSU_316_Dept_AwardsDTO deactive(HSU_316_Dept_AwardsDTO data)
        {
            try
            {
                var u = _context.NAAC_HSU_316_Dept_AwardsDMO.Where(t => t.NMC316DA_Id == data.NMC316DA_Id).SingleOrDefault();
                if (u.NMC316DA_ActiveFlag == true)
                {
                    u.NMC316DA_ActiveFlag = false;
                }
                else if (u.NMC316DA_ActiveFlag == false)
                {
                    u.NMC316DA_ActiveFlag = true;
                }
                u.NMC316DA_UpdatedDate = DateTime.Now;
              //  u.NCMCTT343_UpdatedBy = data.UserId;
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
        public HSU_316_Dept_AwardsDTO EditData(HSU_316_Dept_AwardsDTO data)
        {
            try
            {
                data.editlist = (from a in _context.Academic
                                 from b in _context.NAAC_HSU_316_Dept_AwardsDMO
                                 from c in _context.HR_Master_Department
                                 where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NMC316DA_Year && b.MI_Id == data.MI_Id && b.NMC316DA_Id == data.NMC316DA_Id && c.HRMD_Id == b.HRMD_Id)
                                 select new HSU_316_Dept_AwardsDTO
                                 {
                                     NMC316DA_Id = b.NMC316DA_Id,
                                     NMC316DA_Scheme = b.NMC316DA_Scheme,
                                     NMC316DA_Agency = b.NMC316DA_Agency,
                                     NMC316DA_FoundProvided = b.NMC316DA_FoundProvided,
                                     NMC316DA_Duration = b.NMC316DA_Duration,
                                     HRMD_DepartmentName = c.HRMD_DepartmentName,
                                     NMC316DA_Year = b.NMC316DA_Year,
                                     MI_Id = data.MI_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();
                data.editFileslist = (from a in _context.NAAC_HSU_316_Dept_Awards_FilesDMO
                                      where (a.NMC316DA_Id == data.NMC316DA_Id)
                                      select new HSU_316_Dept_AwardsDTO
                                      {
                                          cfilename = a.NMC316DAF_FileName,
                                          cfilepath = a.NMC316DAF_FileDescription,
                                          cfiledesc = a.NMC316DAF_FilePath,
                                      }).Distinct().ToArray();
            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public HSU_316_Dept_AwardsDTO viewuploadflies(HSU_316_Dept_AwardsDTO data)
        {
            try
            {
                data.viewuploadflies = (from a in _context.NAAC_HSU_316_Dept_Awards_FilesDMO
                                        where (a.NMC316DA_Id == data.NMC316DA_Id)
                                        select new HSU_316_Dept_AwardsDTO
                                        {
                                            cfilename = a.NMC316DAF_FileName,
                                            cfilepath = a.NMC316DAF_FileDescription,
                                            cfiledesc = a.NMC316DAF_FilePath,
                                            NMC316DAF_Id = a.NMC316DAF_Id,
                                            NMC316DA_Id = a.NMC316DA_Id,
                                        }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public HSU_316_Dept_AwardsDTO deleteuploadfile(HSU_316_Dept_AwardsDTO data)
        {
            try
            {
                var res = _context.NAAC_HSU_316_Dept_Awards_FilesDMO.Where(t => t.NMC316DAF_Id == data.NMC316DAF_Id).SingleOrDefault();
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
                data.viewuploadflies = (from a in _context.NAAC_HSU_316_Dept_Awards_FilesDMO
                                        where (a.NMC316DA_Id == data.NMC316DA_Id)
                                        select new HSU_316_Dept_AwardsDTO
                                        {
                                            NMC316DA_Id = a.NMC316DA_Id,
                                            NMC316DAF_Id = a.NMC316DAF_Id,
                                            cfilename = a.NMC316DAF_FileName,
                                            cfilepath = a.NMC316DAF_FileDescription,
                                            cfiledesc = a.NMC316DAF_FilePath,
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
