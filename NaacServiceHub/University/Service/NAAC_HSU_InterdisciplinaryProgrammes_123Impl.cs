using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.University;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.University.Service
{
    public class NAAC_HSU_InterdisciplinaryProgrammes_123Impl:Interface.NAAC_HSU_InterdisciplinaryProgrammes_123Interface
    {

        public GeneralContext _context;
        public NAAC_HSU_InterdisciplinaryProgrammes_123Impl(GeneralContext w)
        {
            _context = w;
        }
        public NAAC_HSU_InterdisciplinaryProgrammes_123_DTO loaddata(NAAC_HSU_InterdisciplinaryProgrammes_123_DTO data)
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

                data.allacademicyear = _context.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Year).ToArray();
                data.alldata1 = (from a in _context.Academic
                                 from b in _context.NAAC_HSU_InterdisciplinaryProgrammes_123_DMO
                                 where (a.MI_Id == b.MI_Id && a.Is_Active == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == b.NCHSUIP123_Year)
                                 select new NAAC_HSU_InterdisciplinaryProgrammes_123_DTO
                                 {
                                     NCHSUIP123_Id = b.NCHSUIP123_Id,
                                     NCHSUIP123_TotalNoOfProg = b.NCHSUIP123_TotalNoOfProg,
                                     NCHSUIP123_TotalNoOfCoursesAcrossProgs = b.NCHSUIP123_TotalNoOfCoursesAcrossProgs,
                                     NCHSUIP123_NoOfInterdisciplinaryCoursesAcsProg = b.NCHSUIP123_NoOfInterdisciplinaryCoursesAcsProg,

                                     NCHSUIP123_Year = b.NCHSUIP123_Year,

                                     ASMAY_Year = a.ASMAY_Year,
                                     NCHSUIP123_ActiveFlag = b.NCHSUIP123_ActiveFlag,
                                     MI_Id = b.MI_Id,
                                 }).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_HSU_InterdisciplinaryProgrammes_123_DTO save(NAAC_HSU_InterdisciplinaryProgrammes_123_DTO data)
        {
            try
            {
                if (data.NCHSUIP123_Id == 0)
                {
                    var duplicate = _context.NAAC_HSU_InterdisciplinaryProgrammes_123_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCHSUIP123_TotalNoOfProg == data.NCHSUIP123_TotalNoOfProg && t.NCHSUIP123_TotalNoOfCoursesAcrossProgs == data.NCHSUIP123_TotalNoOfCoursesAcrossProgs&&t.NCHSUIP123_NoOfInterdisciplinaryCoursesAcsProg==data.NCHSUIP123_NoOfInterdisciplinaryCoursesAcsProg && t.NCHSUIP123_Id != 0).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        NAAC_HSU_InterdisciplinaryProgrammes_123_DMO u = new NAAC_HSU_InterdisciplinaryProgrammes_123_DMO();
                        u.MI_Id = data.MI_Id;
                        u.NCHSUIP123_TotalNoOfProg = data.NCHSUIP123_TotalNoOfProg;
                        u.NCHSUIP123_TotalNoOfCoursesAcrossProgs = data.NCHSUIP123_TotalNoOfCoursesAcrossProgs;
                        u.NCHSUIP123_NoOfInterdisciplinaryCoursesAcsProg = data.NCHSUIP123_NoOfInterdisciplinaryCoursesAcsProg;

                        u.NCHSUIP123_CreatedBy = data.UserId;
                        u.NCHSUIP123_UpdatedBy = data.UserId;
                        u.NCHSUIP123_CreatedDate = DateTime.Now;
                        u.NCHSUIP123_UpdatedDate = DateTime.Now;
                        u.NCHSUIP123_Year = data.ASMAY_Id;
                        u.NCHSUIP123_ActiveFlag = true;
                        _context.Add(u);
                        if (data.filelist.Length > 0)
                        {
                            for (int i = 0; i < data.filelist.Length; i++)
                            {
                                if (data.filelist[0].cfilepath != null)
                                {
                                    NAAC_HSU_InterdisciplinaryProgrammes_123_Files_DMO obj2 = new NAAC_HSU_InterdisciplinaryProgrammes_123_Files_DMO();
                                    obj2.NCHSUIP123_Id = u.NCHSUIP123_Id;
                                    obj2.NCHSUIP123F_FileName = data.filelist[i].cfilename;
                                    obj2.NCHSUIP123F_FileDesc = data.filelist[i].cfiledesc;
                                    obj2.NCHSUIP123F_FilePath = data.filelist[i].cfilepath;

                                    _context.Add(obj2);
                                }
                            }
                        }
                        var w = _context.SaveChanges();
                        if (w > 0)
                        {
                            data.msg = "saved";
                        }
                        else
                        {
                            data.msg = "failed";
                        }
                    }
                }

                else if (data.NCHSUIP123_Id > 0)
                {
                    var duplicate = _context.NAAC_HSU_InterdisciplinaryProgrammes_123_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCHSUIP123_Id != data.NCHSUIP123_Id && t.NCHSUIP123_TotalNoOfProg == data.NCHSUIP123_TotalNoOfProg && t.NCHSUIP123_TotalNoOfCoursesAcrossProgs == data.NCHSUIP123_TotalNoOfCoursesAcrossProgs&&t.NCHSUIP123_NoOfInterdisciplinaryCoursesAcsProg==data.NCHSUIP123_NoOfInterdisciplinaryCoursesAcsProg && t.NCHSUIP123_Year == data.NCHSUIP123_Year).Distinct().ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        var j = _context.NAAC_HSU_InterdisciplinaryProgrammes_123_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCHSUIP123_Id == data.NCHSUIP123_Id).SingleOrDefault();
                        j.NCHSUIP123_TotalNoOfProg = data.NCHSUIP123_TotalNoOfProg;
                        j.NCHSUIP123_TotalNoOfCoursesAcrossProgs = data.NCHSUIP123_TotalNoOfCoursesAcrossProgs;
                        j.NCHSUIP123_NoOfInterdisciplinaryCoursesAcsProg = data.NCHSUIP123_NoOfInterdisciplinaryCoursesAcsProg;

                        j.NCHSUIP123_Year = data.ASMAY_Id;
                        j.MI_Id = data.MI_Id;
                        j.NCHSUIP123_UpdatedDate = DateTime.Now;
                        j.NCHSUIP123_UpdatedBy = data.UserId;
                        _context.Update(j);
                        var CountRemoveFiles = _context.NAAC_HSU_InterdisciplinaryProgrammes_123_Files_DMO.Where(t => t.NCHSUIP123_Id == data.NCHSUIP123_Id).ToList();
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

                                        NAAC_HSU_InterdisciplinaryProgrammes_123_Files_DMO obj2 = new NAAC_HSU_InterdisciplinaryProgrammes_123_Files_DMO();
                                        obj2.NCHSUIP123_Id = j.NCHSUIP123_Id;
                                        obj2.NCHSUIP123F_FileName = data.filelist[i].cfilename;
                                        obj2.NCHSUIP123F_FileDesc = data.filelist[i].cfiledesc;
                                        obj2.NCHSUIP123F_FilePath = data.filelist[i].cfilepath;
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
                                        NAAC_HSU_InterdisciplinaryProgrammes_123_Files_DMO obj2 = new NAAC_HSU_InterdisciplinaryProgrammes_123_Files_DMO();
                                        obj2.NCHSUIP123_Id = j.NCHSUIP123_Id;
                                        obj2.NCHSUIP123F_FileName = data.filelist[i].cfilename;
                                        obj2.NCHSUIP123F_FileDesc = data.filelist[i].cfiledesc;
                                        obj2.NCHSUIP123F_FilePath = data.filelist[i].cfilepath;
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
        public NAAC_HSU_InterdisciplinaryProgrammes_123_DTO deactive(NAAC_HSU_InterdisciplinaryProgrammes_123_DTO data)
        {
            try
            {
                var g = _context.NAAC_HSU_InterdisciplinaryProgrammes_123_DMO.Where(t => t.NCHSUIP123_Id == data.NCHSUIP123_Id).SingleOrDefault();
                if (g.NCHSUIP123_ActiveFlag == true)
                {
                    g.NCHSUIP123_ActiveFlag = false;
                }
                else
                {
                    g.NCHSUIP123_ActiveFlag = true;
                }
                g.NCHSUIP123_UpdatedDate = DateTime.Now;
                g.NCHSUIP123_UpdatedBy = data.UserId;
                g.MI_Id = data.MI_Id;
                _context.Update(g);
                int s = _context.SaveChanges();
                if (s > 0)
                {
                    data.ret = true;
                }
                else
                {
                    data.ret = false;
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_HSU_InterdisciplinaryProgrammes_123_DTO EditData(NAAC_HSU_InterdisciplinaryProgrammes_123_DTO data)
        {
            try
            {

                data.editlist = (from a in _context.Academic
                                 from b in _context.NAAC_HSU_InterdisciplinaryProgrammes_123_DMO
                                 where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && b.NCHSUIP123_Id == data.NCHSUIP123_Id && a.ASMAY_Id == b.NCHSUIP123_Year)
                                 select new NAAC_HSU_InterdisciplinaryProgrammes_123_DTO
                                 {
                                     NCHSUIP123_Id = b.NCHSUIP123_Id,
                                     NCHSUIP123_TotalNoOfProg = b.NCHSUIP123_TotalNoOfProg,
                                     NCHSUIP123_TotalNoOfCoursesAcrossProgs = b.NCHSUIP123_TotalNoOfCoursesAcrossProgs,
                                     NCHSUIP123_NoOfInterdisciplinaryCoursesAcsProg = b.NCHSUIP123_NoOfInterdisciplinaryCoursesAcsProg,

                                     NCHSUIP123_Year = b.NCHSUIP123_Year,

                                     MI_Id = b.MI_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();

                data.editFileslist = (from a in _context.NAAC_HSU_InterdisciplinaryProgrammes_123_Files_DMO
                                      where (a.NCHSUIP123_Id == data.NCHSUIP123_Id)
                                      select new NAAC_HSU_InterdisciplinaryProgrammes_123_DTO
                                      {
                                          cfilename = a.NCHSUIP123F_FileName,
                                          cfilepath = a.NCHSUIP123F_FilePath,
                                          cfiledesc = a.NCHSUIP123F_FileDesc,
                                      }).Distinct().ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_HSU_InterdisciplinaryProgrammes_123_DTO viewuploadflies(NAAC_HSU_InterdisciplinaryProgrammes_123_DTO data)
        {
            try
            {
                data.viewuploadflies = (from a in _context.NAAC_HSU_InterdisciplinaryProgrammes_123_Files_DMO
                                        where (a.NCHSUIP123_Id == data.NCHSUIP123_Id)
                                        select new NAAC_HSU_InterdisciplinaryProgrammes_123_DTO
                                        {
                                            cfilename = a.NCHSUIP123F_FileName,
                                            cfilepath = a.NCHSUIP123F_FilePath,
                                            cfiledesc = a.NCHSUIP123F_FileDesc,
                                            NCHSUIP123F_Id = a.NCHSUIP123F_Id,
                                            NCHSUIP123_Id = a.NCHSUIP123_Id,
                                        }).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;

        }
        public NAAC_HSU_InterdisciplinaryProgrammes_123_DTO deleteuploadfile(NAAC_HSU_InterdisciplinaryProgrammes_123_DTO data)
        {
            try
            {
                var res = _context.NAAC_HSU_InterdisciplinaryProgrammes_123_Files_DMO.Where(t => t.NCHSUIP123F_Id == data.NCHSUIP123F_Id).SingleOrDefault();
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
                data.viewuploadflies = (from a in _context.NAAC_HSU_InterdisciplinaryProgrammes_123_Files_DMO
                                        where (a.NCHSUIP123_Id == data.NCHSUIP123_Id)
                                        select new NAAC_HSU_InterdisciplinaryProgrammes_123_DTO
                                        {
                                            cfilename = a.NCHSUIP123F_FileName,
                                            cfilepath = a.NCHSUIP123F_FilePath,
                                            cfiledesc = a.NCHSUIP123F_FileDesc,
                                            NCHSUIP123F_Id = a.NCHSUIP123F_Id,
                                            NCHSUIP123_Id = a.NCHSUIP123_Id,
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
