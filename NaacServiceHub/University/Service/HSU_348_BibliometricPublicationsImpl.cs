using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.University;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.University.Service
{
    public class HSU_348_BibliometricPublicationsImpl:Interface.HSU_348_BibliometricPublicationsInterface
    {
        public GeneralContext _context;
        public HSU_348_BibliometricPublicationsImpl(GeneralContext y)
        {
            _context = y;
        }
        public HSU_346_EMPApprovedJournalList_DTO loaddata(HSU_346_EMPApprovedJournalList_DTO data)
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
                                 from b in _context.HSU_346_EMPApprovedJournalList_DMO
                                 where (a.MI_Id == data.MI_Id && b.MI_Id == a.MI_Id && a.Is_Active == true && b.NCHSU346EAJL_year == a.ASMAY_Id)
                                 select new HSU_346_EMPApprovedJournalList_DTO
                                 {
                                     NCHSU346EAJL_Id = b.NCHSU346EAJL_Id,
                                     NCHSU346EAJL_year = b.NCHSU346EAJL_year,
                                     ASMAY_Year = a.ASMAY_Year,
                                     NCHSU346EAJL_EmpName = b.NCHSU346EAJL_EmpName,
                                     NCHSU346EAJL_PaperTitle = b.NCHSU346EAJL_PaperTitle,
                                     NCHSU346EAJL_NoOfCitationsExcludeCitations = b.NCHSU346EAJL_NoOfCitationsExcludeCitations,
                                     NCHSU346EAJL_InstAffMenPub = b.NCHSU346EAJL_InstAffMenPub,
                                     NCHSU346EAJL_JournalTitle = b.NCHSU346EAJL_JournalTitle,
                                     NCHSU346EAJL_NoOfCitationsScopus = b.NCHSU346EAJL_NoOfCitationsScopus,
                                     NCHSU346EAJL_NoOfCitationsWebOfScience = b.NCHSU346EAJL_NoOfCitationsWebOfScience,
                                     NCHSU346EAJL_ActiveFlag = b.NCHSU346EAJL_ActiveFlag,
                                     MI_Id = data.MI_Id

                                 }).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public HSU_346_EMPApprovedJournalList_DTO save(HSU_346_EMPApprovedJournalList_DTO data)
        {
            try
            {
                if (data.NCHSU346EAJL_Id == 0)
                {
                    var duplicate = _context.HSU_346_EMPApprovedJournalList_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCHSU346EAJL_year == data.ASMAY_Id && t.NCHSU346EAJL_Id != 0 && t.NCHSU346EAJL_EmpName == data.NCHSU346EAJL_EmpName && t.NCHSU346EAJL_PaperTitle == data.NCHSU346EAJL_PaperTitle && t.NCHSU346EAJL_NoOfCitationsExcludeCitations == data.NCHSU346EAJL_NoOfCitationsExcludeCitations && t.NCHSU346EAJL_InstAffMenPub == data.NCHSU346EAJL_InstAffMenPub && t.NCHSU346EAJL_JournalTitle == data.NCHSU346EAJL_JournalTitle && t.NCHSU346EAJL_NoOfCitationsWebOfScience==data.NCHSU346EAJL_NoOfCitationsWebOfScience && t.NCHSU346EAJL_NoOfCitationsScopus==data.NCHSU346EAJL_NoOfCitationsScopus).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        HSU_346_EMPApprovedJournalList_DMO rrr = new HSU_346_EMPApprovedJournalList_DMO();
                        rrr.MI_Id = data.MI_Id;
                        rrr.NCHSU346EAJL_EmpName = data.NCHSU346EAJL_EmpName;
                        rrr.NCHSU346EAJL_PaperTitle = data.NCHSU346EAJL_PaperTitle;
                        rrr.NCHSU346EAJL_NoOfCitationsExcludeCitations = data.NCHSU346EAJL_NoOfCitationsExcludeCitations;
                        rrr.NCHSU346EAJL_InstAffMenPub = data.NCHSU346EAJL_InstAffMenPub;
                        rrr.NCHSU346EAJL_JournalTitle = data.NCHSU346EAJL_JournalTitle;
                        rrr.NCHSU346EAJL_NoOfCitationsScopus = data.NCHSU346EAJL_NoOfCitationsScopus;
                        rrr.NCHSU346EAJL_NoOfCitationsWebOfScience = data.NCHSU346EAJL_NoOfCitationsWebOfScience;
                        rrr.NCHSU346EAJL_year = data.ASMAY_Id;
                        rrr.NCHSU346EAJL_CreatedDate = DateTime.Now;
                        rrr.NCHSU346EAJL_UpdatedDate = DateTime.Now;
                        rrr.NCHSU346EAJL_CreatedBy = data.UserId;
                        rrr.NCHSU346EAJL_UpdatedBy = data.UserId;
                        rrr.NCHSU346EAJL_ActiveFlag = true;

                        _context.Add(rrr);
                        if (data.filelist.Length > 0)
                        {
                            for (int i = 0; i < data.filelist.Length; i++)
                            {
                                if (data.filelist[0].cfilepath != null)
                                {
                                    HSU_346_EmpApprovedJournalLists_Files_DMO obj2 = new HSU_346_EmpApprovedJournalLists_Files_DMO();
                                    obj2.NCHSU346EAJL_Id = rrr.NCHSU346EAJL_Id;
                                    obj2.NCHSU346EAJLF_FileName = data.filelist[i].cfilename;
                                    obj2.NCHSU346EAJLF_FileDesc = data.filelist[i].cfiledesc;
                                    obj2.NCHSU346EAJLF_FilePath = data.filelist[i].cfilepath;
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
                else if (data.NCHSU346EAJL_Id > 0)
                {
                    var duplicate = _context.HSU_346_EMPApprovedJournalList_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCHSU346EAJL_EmpName == data.NCHSU346EAJL_EmpName && t.NCHSU346EAJL_PaperTitle == data.NCHSU346EAJL_PaperTitle && t.NCHSU346EAJL_NoOfCitationsExcludeCitations == data.NCHSU346EAJL_NoOfCitationsExcludeCitations && t.NCHSU346EAJL_InstAffMenPub == data.NCHSU346EAJL_InstAffMenPub && t.NCHSU346EAJL_JournalTitle == data.NCHSU346EAJL_JournalTitle && t.NCHSU346EAJL_Id != data.NCHSU346EAJL_Id && t.NCHSU346EAJL_year == data.ASMAY_Id && t.NCHSU346EAJL_NoOfCitationsScopus == data.NCHSU346EAJL_NoOfCitationsScopus && t.NCHSU346EAJL_NoOfCitationsWebOfScience == data.NCHSU346EAJL_NoOfCitationsWebOfScience).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var yy = _context.HSU_346_EMPApprovedJournalList_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCHSU346EAJL_Id == data.NCHSU346EAJL_Id).SingleOrDefault();
                        yy.NCHSU346EAJL_EmpName = data.NCHSU346EAJL_EmpName;
                        yy.NCHSU346EAJL_PaperTitle = data.NCHSU346EAJL_PaperTitle;
                        yy.NCHSU346EAJL_NoOfCitationsExcludeCitations = data.NCHSU346EAJL_NoOfCitationsExcludeCitations;
                        yy.NCHSU346EAJL_InstAffMenPub = data.NCHSU346EAJL_InstAffMenPub;
                        yy.NCHSU346EAJL_JournalTitle = data.NCHSU346EAJL_JournalTitle;
                        yy.NCHSU346EAJL_NoOfCitationsScopus = data.NCHSU346EAJL_NoOfCitationsScopus;
                        yy.NCHSU346EAJL_NoOfCitationsWebOfScience = data.NCHSU346EAJL_NoOfCitationsWebOfScience;
                        yy.NCHSU346EAJL_year = data.ASMAY_Id;
                        yy.MI_Id = data.MI_Id;
                        yy.NCHSU346EAJL_UpdatedDate = DateTime.Now;
                        yy.NCHSU346EAJL_UpdatedBy = data.UserId;
                        _context.Update(yy);
                        var CountRemoveFiles = _context.HSU_346_EmpApprovedJournalLists_Files_DMO.Where(t => t.NCHSU346EAJL_Id == data.NCHSU346EAJL_Id).ToList();
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
                                        HSU_346_EmpApprovedJournalLists_Files_DMO obj2 = new HSU_346_EmpApprovedJournalLists_Files_DMO();
                                        obj2.NCHSU346EAJL_Id = yy.NCHSU346EAJL_Id;
                                        obj2.NCHSU346EAJLF_FileName = data.filelist[i].cfilename;
                                        obj2.NCHSU346EAJLF_FileDesc = data.filelist[i].cfiledesc;
                                        obj2.NCHSU346EAJLF_FilePath = data.filelist[i].cfilepath;
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
                                        HSU_346_EmpApprovedJournalLists_Files_DMO obj2 = new HSU_346_EmpApprovedJournalLists_Files_DMO();
                                        obj2.NCHSU346EAJL_Id = yy.NCHSU346EAJL_Id;
                                        obj2.NCHSU346EAJLF_FileName = data.filelist[i].cfilename;
                                        obj2.NCHSU346EAJLF_FileDesc = data.filelist[i].cfiledesc;
                                        obj2.NCHSU346EAJLF_FilePath = data.filelist[i].cfilepath;
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
        public HSU_346_EMPApprovedJournalList_DTO deactive(HSU_346_EMPApprovedJournalList_DTO data)
        {
            try
            {
                var u = _context.HSU_346_EMPApprovedJournalList_DMO.Where(t => t.NCHSU346EAJL_Id == data.NCHSU346EAJL_Id).SingleOrDefault();
                if (u.NCHSU346EAJL_ActiveFlag == true)
                {
                    u.NCHSU346EAJL_ActiveFlag = false;
                }
                else if (u.NCHSU346EAJL_ActiveFlag == false)
                {
                    u.NCHSU346EAJL_ActiveFlag = true;
                }
                u.NCHSU346EAJL_UpdatedDate = DateTime.Now;
                u.NCHSU346EAJL_UpdatedBy = data.UserId;
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
        public HSU_346_EMPApprovedJournalList_DTO EditData(HSU_346_EMPApprovedJournalList_DTO data)
        {
            try
            {
                data.editlist = (from a in _context.Academic
                                 from b in _context.HSU_346_EMPApprovedJournalList_DMO
                                 where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCHSU346EAJL_year && b.MI_Id == data.MI_Id && b.NCHSU346EAJL_Id == data.NCHSU346EAJL_Id)
                                 select new HSU_346_EMPApprovedJournalList_DTO
                                 {
                                     NCHSU346EAJL_Id = b.NCHSU346EAJL_Id,
                                     NCHSU346EAJL_EmpName = b.NCHSU346EAJL_EmpName,
                                     NCHSU346EAJL_PaperTitle = b.NCHSU346EAJL_PaperTitle,
                                     NCHSU346EAJL_NoOfCitationsExcludeCitations = b.NCHSU346EAJL_NoOfCitationsExcludeCitations,
                                     NCHSU346EAJL_InstAffMenPub = b.NCHSU346EAJL_InstAffMenPub,
                                     NCHSU346EAJL_JournalTitle = b.NCHSU346EAJL_JournalTitle,
                                     NCHSU346EAJL_year = b.NCHSU346EAJL_year,
                                     NCHSU346EAJL_NoOfCitationsScopus = b.NCHSU346EAJL_NoOfCitationsScopus,
                                     NCHSU346EAJL_NoOfCitationsWebOfScience = b.NCHSU346EAJL_NoOfCitationsWebOfScience,
                                     MI_Id = data.MI_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();
                data.editFileslist = (from a in _context.HSU_346_EmpApprovedJournalLists_Files_DMO
                                      where (a.NCHSU346EAJL_Id == data.NCHSU346EAJL_Id)
                                      select new HSU_346_EMPApprovedJournalList_DTO
                                      {
                                          cfilename = a.NCHSU346EAJLF_FileName,
                                          cfiledesc = a.NCHSU346EAJLF_FileDesc,
                                          cfilepath = a.NCHSU346EAJLF_FilePath,
                                      }).Distinct().ToArray();
            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public HSU_346_EMPApprovedJournalList_DTO viewuploadflies(HSU_346_EMPApprovedJournalList_DTO data)
        {
            try
            {
                data.viewuploadflies = (from a in _context.HSU_346_EmpApprovedJournalLists_Files_DMO
                                        where (a.NCHSU346EAJL_Id == data.NCHSU346EAJL_Id)
                                        select new HSU_346_EMPApprovedJournalList_DTO
                                        {
                                            cfilename = a.NCHSU346EAJLF_FileName,
                                            cfiledesc = a.NCHSU346EAJLF_FileDesc,
                                            cfilepath = a.NCHSU346EAJLF_FilePath,
                                            NCHSU346EAJLF_Id = a.NCHSU346EAJLF_Id,
                                            NCHSU346EAJL_Id = a.NCHSU346EAJL_Id,
                                        }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public HSU_346_EMPApprovedJournalList_DTO deleteuploadfile(HSU_346_EMPApprovedJournalList_DTO data)
        {
            try
            {
                var res = _context.HSU_346_EmpApprovedJournalLists_Files_DMO.Where(t => t.NCHSU346EAJLF_Id == data.NCHSU346EAJLF_Id).SingleOrDefault();
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
                data.viewuploadflies = (from a in _context.HSU_346_EmpApprovedJournalLists_Files_DMO
                                        where (a.NCHSU346EAJL_Id == data.NCHSU346EAJL_Id)
                                        select new HSU_346_EMPApprovedJournalList_DTO
                                        {
                                            NCHSU346EAJL_Id = a.NCHSU346EAJL_Id,
                                            NCHSU346EAJLF_Id = a.NCHSU346EAJLF_Id,
                                            cfilename = a.NCHSU346EAJLF_FileName,
                                            cfiledesc = a.NCHSU346EAJLF_FileDesc,
                                            cfilepath = a.NCHSU346EAJLF_FilePath,
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
