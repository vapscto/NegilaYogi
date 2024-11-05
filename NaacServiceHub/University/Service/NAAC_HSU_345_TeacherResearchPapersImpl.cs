using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.University;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.University.Service
{
    public class NAAC_HSU_345_TeacherResearchPapersImpl:Interface.NAAC_HSU_345_TeacherResearchPapersInterface
    {

        public GeneralContext _context;
        public NAAC_HSU_345_TeacherResearchPapersImpl(GeneralContext w)
        {
            _context = w;
        }
        public NAAC_HSU_345_TeacherResearchPapers_DTO loaddata(NAAC_HSU_345_TeacherResearchPapers_DTO data)
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
                                 from b in _context.NAAC_HSU_345_TeacherResearchPapers_DMO
                                 where (a.MI_Id == b.MI_Id && a.Is_Active == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == b.NCHSU345TRP_Year)
                                 select new NAAC_HSU_345_TeacherResearchPapers_DTO
                                 {
                                     NCHSU345TRP_Id = b.NCHSU345TRP_Id,
                                     NCHSU345TRP_PaperTitle = b.NCHSU345TRP_PaperTitle,
                                     NCHSU345TRP_AuthorName = b.NCHSU345TRP_AuthorName,
                                     NCHSU345TRP_DepartmentName = b.NCHSU345TRP_DepartmentName,
                                     NCHSU345TRP_JournalName = b.NCHSU345TRP_JournalName,
                                     NCHSU345TRP_NamesOfIndexingDatabases = b.NCHSU345TRP_NamesOfIndexingDatabases,
                                     NCHSU345TRP_ISSNNumber = b.NCHSU345TRP_ISSNNumber,
                                     NCHSU345TRP_link = b.NCHSU345TRP_link,

                                     ASMAY_Year = a.ASMAY_Year,
                                     NCHSU345TRP_ActiveFlag = b.NCHSU345TRP_ActiveFlag,
                                     MI_Id = b.MI_Id,
                                 }).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_HSU_345_TeacherResearchPapers_DTO save(NAAC_HSU_345_TeacherResearchPapers_DTO data)
        {
            try
            {
                if (data.NCHSU345TRP_Id == 0)
                {
                    var duplicate = _context.NAAC_HSU_345_TeacherResearchPapers_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCHSU345TRP_AuthorName == data.NCHSU345TRP_AuthorName && t.NCHSU345TRP_DepartmentName == data.NCHSU345TRP_DepartmentName && t.NCHSU345TRP_JournalName == data.NCHSU345TRP_JournalName&&t.NCHSU345TRP_NamesOfIndexingDatabases==data.NCHSU345TRP_NamesOfIndexingDatabases&&t.NCHSU345TRP_ISSNNumber==data.NCHSU345TRP_ISSNNumber&&t.NCHSU345TRP_PaperTitle==data.NCHSU345TRP_PaperTitle && t.NCHSU345TRP_Id != 0).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        NAAC_HSU_345_TeacherResearchPapers_DMO u = new NAAC_HSU_345_TeacherResearchPapers_DMO();
                        u.MI_Id = data.MI_Id;
                        u.NCHSU345TRP_AuthorName = data.NCHSU345TRP_AuthorName;
                        u.NCHSU345TRP_PaperTitle = data.NCHSU345TRP_PaperTitle;
                        u.NCHSU345TRP_DepartmentName = data.NCHSU345TRP_DepartmentName;
                        u.NCHSU345TRP_JournalName = data.NCHSU345TRP_JournalName;
                        u.NCHSU345TRP_ISSNNumber = data.NCHSU345TRP_ISSNNumber;
                        u.NCHSU345TRP_link = data.NCHSU345TRP_link;
                        u.NCHSU345TRP_NamesOfIndexingDatabases = data.NCHSU345TRP_NamesOfIndexingDatabases;
                        u.NCHSU345TRP_CreatedBy = data.UserId;
                        u.NCHSU345TRP_UpdatedBy = data.UserId;
                        u.NCHSU345TRP_CreatedDate = DateTime.Now;
                        u.NCHSU345TRP_UpdatedDate = DateTime.Now;
                        u.NCHSU345TRP_Year = data.ASMAY_Id;
                        u.NCHSU345TRP_ActiveFlag = true;
                        _context.Add(u);
                        if (data.filelist.Length > 0)
                        {
                            for (int i = 0; i < data.filelist.Length; i++)
                            {
                                if (data.filelist[0].cfilepath != null)
                                {
                                    NAAC_HSU_345_TeacherResearchPapers_Files_DMO obj2 = new NAAC_HSU_345_TeacherResearchPapers_Files_DMO();
                                    obj2.NCHSU345TRP_Id = u.NCHSU345TRP_Id;
                                    obj2.NCHSU345TRPF_FileName = data.filelist[i].cfilename;
                                    obj2.NCHSU345TRPF_FileDesc = data.filelist[i].cfiledesc;
                                    obj2.NCHSU345TRPF_FilePath = data.filelist[i].cfilepath;
                                    obj2.NCHSU345TRPF_ActiveFlg = true;
                                    obj2.NCHSU345TRPF_CreatedDate = DateTime.Now;
                                    obj2.NCHSU345TRPF_UpdatedDate = DateTime.Now;
                                    obj2.NCHSU345TRPF_CreatedBy = data.UserId;
                                    obj2.NCHSU345TRPF_UpdatedBy = data.UserId;

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

                else if (data.NCHSU345TRP_Id > 0)
                {
                    var duplicate = _context.NAAC_HSU_345_TeacherResearchPapers_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCHSU345TRP_Id != data.NCHSU345TRP_Id && t.NCHSU345TRP_AuthorName == data.NCHSU345TRP_AuthorName && t.NCHSU345TRP_DepartmentName == data.NCHSU345TRP_DepartmentName && t.NCHSU345TRP_JournalName == data.NCHSU345TRP_JournalName&&t.NCHSU345TRP_NamesOfIndexingDatabases==data.NCHSU345TRP_NamesOfIndexingDatabases&&t.NCHSU345TRP_ISSNNumber==data.NCHSU345TRP_ISSNNumber&&t.NCHSU345TRP_PaperTitle==data.NCHSU345TRP_PaperTitle && t.NCHSU345TRP_Year == data.NCHSU345TRP_Year).Distinct().ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        var j = _context.NAAC_HSU_345_TeacherResearchPapers_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCHSU345TRP_Id == data.NCHSU345TRP_Id).SingleOrDefault();
                        j.NCHSU345TRP_AuthorName = data.NCHSU345TRP_AuthorName;
                        j.NCHSU345TRP_PaperTitle = data.NCHSU345TRP_PaperTitle;
                        j.NCHSU345TRP_DepartmentName = data.NCHSU345TRP_DepartmentName;
                        j.NCHSU345TRP_JournalName = data.NCHSU345TRP_JournalName;
                        j.NCHSU345TRP_ISSNNumber = data.NCHSU345TRP_ISSNNumber;
                        j.NCHSU345TRP_link = data.NCHSU345TRP_link;
                        j.NCHSU345TRP_NamesOfIndexingDatabases = data.NCHSU345TRP_NamesOfIndexingDatabases;
                        j.NCHSU345TRP_Year = data.ASMAY_Id;
                        j.MI_Id = data.MI_Id;
                        j.NCHSU345TRP_UpdatedDate = DateTime.Now;
                        j.NCHSU345TRP_UpdatedBy = data.UserId;
                        _context.Update(j);
                        var CountRemoveFiles = _context.NAAC_HSU_345_TeacherResearchPapers_Files_DMO.Where(t => t.NCHSU345TRP_Id == data.NCHSU345TRP_Id).ToList();
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

                                        NAAC_HSU_345_TeacherResearchPapers_Files_DMO obj2 = new NAAC_HSU_345_TeacherResearchPapers_Files_DMO();
                                        obj2.NCHSU345TRP_Id = j.NCHSU345TRP_Id;
                                        obj2.NCHSU345TRPF_FileName = data.filelist[i].cfilename;
                                        obj2.NCHSU345TRPF_FileDesc = data.filelist[i].cfiledesc;
                                        obj2.NCHSU345TRPF_FilePath = data.filelist[i].cfilepath;
                                        obj2.NCHSU345TRPF_CreatedBy = data.UserId;
                                        obj2.NCHSU345TRPF_UpdatedBy = data.UserId;
                                        obj2.NCHSU345TRPF_CreatedDate = DateTime.Now;
                                        obj2.NCHSU345TRPF_UpdatedDate = DateTime.Now;
                                        obj2.NCHSU345TRPF_ActiveFlg = true;
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
                                        NAAC_HSU_345_TeacherResearchPapers_Files_DMO obj2 = new NAAC_HSU_345_TeacherResearchPapers_Files_DMO();
                                        obj2.NCHSU345TRP_Id = j.NCHSU345TRP_Id;
                                        obj2.NCHSU345TRPF_FileName = data.filelist[i].cfilename;
                                        obj2.NCHSU345TRPF_FileDesc = data.filelist[i].cfiledesc;
                                        obj2.NCHSU345TRPF_FilePath = data.filelist[i].cfilepath;
                                        obj2.NCHSU345TRPF_CreatedBy = data.UserId;
                                        obj2.NCHSU345TRPF_UpdatedBy = data.UserId;
                                        obj2.NCHSU345TRPF_CreatedDate = DateTime.Now;
                                        obj2.NCHSU345TRPF_UpdatedDate = DateTime.Now;
                                        obj2.NCHSU345TRPF_ActiveFlg = true;
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
        public NAAC_HSU_345_TeacherResearchPapers_DTO deactive(NAAC_HSU_345_TeacherResearchPapers_DTO data)
        {
            try
            {
                var g = _context.NAAC_HSU_345_TeacherResearchPapers_DMO.Where(t => t.NCHSU345TRP_Id == data.NCHSU345TRP_Id).SingleOrDefault();
                if (g.NCHSU345TRP_ActiveFlag == true)
                {
                    g.NCHSU345TRP_ActiveFlag = false;
                }
                else
                {
                    g.NCHSU345TRP_ActiveFlag = true;
                }
                g.NCHSU345TRP_UpdatedDate = DateTime.Now;
                g.NCHSU345TRP_UpdatedBy = data.UserId;
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
        public NAAC_HSU_345_TeacherResearchPapers_DTO EditData(NAAC_HSU_345_TeacherResearchPapers_DTO data)
        {
            try
            {

                data.editlist = (from a in _context.Academic
                                 from b in _context.NAAC_HSU_345_TeacherResearchPapers_DMO
                                 where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && b.NCHSU345TRP_Id == data.NCHSU345TRP_Id && a.ASMAY_Id == b.NCHSU345TRP_Year)
                                 select new NAAC_HSU_345_TeacherResearchPapers_DTO
                                 {
                                     NCHSU345TRP_Id = b.NCHSU345TRP_Id,
                                     NCHSU345TRP_AuthorName = b.NCHSU345TRP_AuthorName,
                                     NCHSU345TRP_DepartmentName = b.NCHSU345TRP_DepartmentName,
                                     NCHSU345TRP_JournalName = b.NCHSU345TRP_JournalName,
                                     NCHSU345TRP_NamesOfIndexingDatabases = b.NCHSU345TRP_NamesOfIndexingDatabases,
                                     NCHSU345TRP_ISSNNumber = b.NCHSU345TRP_ISSNNumber,
                                     NCHSU345TRP_PaperTitle = b.NCHSU345TRP_PaperTitle,
                                     NCHSU345TRP_link = b.NCHSU345TRP_link,
                                     NCHSU345TRP_Year = b.NCHSU345TRP_Year,
                                     MI_Id = b.MI_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();

                data.editFileslist = (from a in _context.NAAC_HSU_345_TeacherResearchPapers_Files_DMO
                                      where (a.NCHSU345TRP_Id == data.NCHSU345TRP_Id)
                                      select new NAAC_HSU_345_TeacherResearchPapers_DTO
                                      {
                                          cfilename = a.NCHSU345TRPF_FileName,
                                          cfilepath = a.NCHSU345TRPF_FilePath,
                                          cfiledesc = a.NCHSU345TRPF_FileDesc,
                                      }).Distinct().ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_HSU_345_TeacherResearchPapers_DTO viewuploadflies(NAAC_HSU_345_TeacherResearchPapers_DTO data)
        {
            try
            {
                data.viewuploadflies = (from a in _context.NAAC_HSU_345_TeacherResearchPapers_Files_DMO
                                        where (a.NCHSU345TRP_Id == data.NCHSU345TRP_Id)
                                        select new NAAC_HSU_345_TeacherResearchPapers_DTO
                                        {
                                            cfilename = a.NCHSU345TRPF_FileName,
                                            cfilepath = a.NCHSU345TRPF_FilePath,
                                            cfiledesc = a.NCHSU345TRPF_FileDesc,
                                            NCHSU345TRPF_Id = a.NCHSU345TRPF_Id,
                                            NCHSU345TRP_Id = a.NCHSU345TRP_Id,
                                        }).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;

        }
        public NAAC_HSU_345_TeacherResearchPapers_DTO deleteuploadfile(NAAC_HSU_345_TeacherResearchPapers_DTO data)
        {
            try
            {
                var res = _context.NAAC_HSU_345_TeacherResearchPapers_Files_DMO.Where(t => t.NCHSU345TRPF_Id == data.NCHSU345TRPF_Id).SingleOrDefault();
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
                data.viewuploadflies = (from a in _context.NAAC_HSU_345_TeacherResearchPapers_Files_DMO
                                        where (a.NCHSU345TRP_Id == data.NCHSU345TRP_Id)
                                        select new NAAC_HSU_345_TeacherResearchPapers_DTO
                                        {
                                            cfilename = a.NCHSU345TRPF_FileName,
                                            cfilepath = a.NCHSU345TRPF_FilePath,
                                            cfiledesc = a.NCHSU345TRPF_FileDesc,
                                            NCHSU345TRPF_Id = a.NCHSU345TRPF_Id,
                                            NCHSU345TRP_Id = a.NCHSU345TRP_Id,
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
