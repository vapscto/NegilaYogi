using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.University;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.University.Service
{
    public class HSU_346_EMPApprovedJournalListImpl:Interface.HSU_346_EMPApprovedJournalListInterface
    {


        public GeneralContext _context;
        public HSU_346_EMPApprovedJournalListImpl(GeneralContext w)
        {
            _context = w;
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

                data.allacademicyear = _context.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Year).ToArray();
                data.alldata1 = (from a in _context.Academic
                                 from b in _context.HSU_346_EMPApprovedJournalList_DMO
                                 where (a.MI_Id == b.MI_Id && a.Is_Active == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == b.NCHSU346EAJL_year)
                                 select new HSU_346_EMPApprovedJournalList_DTO
                                 {
                                     NCHSU346EAJL_Id = b.NCHSU346EAJL_Id,
                                     NCHSU346EAJL_EmpName = b.NCHSU346EAJL_EmpName,
                                     NCHSU346EAJL_BookTitle = b.NCHSU346EAJL_BookTitle,
                                     NCHSU346EAJL_PaperTitle = b.NCHSU346EAJL_PaperTitle,
                                     NCHSU346EAJL_TitleOfProcConference = b.NCHSU346EAJL_TitleOfProcConference,
                                     NCHSU346EAJL_NameOfConference = b.NCHSU346EAJL_NameOfConference,
                                     NCHSU346EAJL_NationalORInternational = b.NCHSU346EAJL_NationalORInternational,
                                     NCHSU346EAJL_ISBNNo = b.NCHSU346EAJL_ISBNNo,
                                     NCHSU346EAJL_AffiliatingInsttimeOfPublication = b.NCHSU346EAJL_AffiliatingInsttimeOfPublication,
                                     NCHSU346EAJL_PublisherName = b.NCHSU346EAJL_PublisherName,
                                     ASMAY_Year = a.ASMAY_Year,
                                     NCHSU346EAJL_ActiveFlag = b.NCHSU346EAJL_ActiveFlag,
                                     MI_Id = b.MI_Id,
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
                    var duplicate = _context.HSU_346_EMPApprovedJournalList_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCHSU346EAJL_EmpName == data.NCHSU346EAJL_EmpName && t.NCHSU346EAJL_BookTitle == data.NCHSU346EAJL_BookTitle && t.NCHSU346EAJL_PaperTitle == data.NCHSU346EAJL_PaperTitle && t.NCHSU346EAJL_TitleOfProcConference == data.NCHSU346EAJL_TitleOfProcConference && t.NCHSU346EAJL_NameOfConference == data.NCHSU346EAJL_NameOfConference && t.NCHSU346EAJL_NationalORInternational == data.NCHSU346EAJL_NationalORInternational&&t.NCHSU346EAJL_ISBNNo==data.NCHSU346EAJL_ISBNNo&&t.NCHSU346EAJL_AffiliatingInsttimeOfPublication==data.NCHSU346EAJL_AffiliatingInsttimeOfPublication&&t.NCHSU346EAJL_PublisherName==data.NCHSU346EAJL_PublisherName && t.NCHSU346EAJL_Id != 0).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        HSU_346_EMPApprovedJournalList_DMO u = new HSU_346_EMPApprovedJournalList_DMO();
                        u.MI_Id = data.MI_Id;
                        u.NCHSU346EAJL_EmpName = data.NCHSU346EAJL_EmpName;
                        u.NCHSU346EAJL_BookTitle = data.NCHSU346EAJL_BookTitle;
                        u.NCHSU346EAJL_PaperTitle = data.NCHSU346EAJL_PaperTitle;
                        u.NCHSU346EAJL_TitleOfProcConference = data.NCHSU346EAJL_TitleOfProcConference;
                        u.NCHSU346EAJL_NameOfConference = data.NCHSU346EAJL_NameOfConference;
                        u.NCHSU346EAJL_NationalORInternational = data.NCHSU346EAJL_NationalORInternational;
                        u.NCHSU346EAJL_ISBNNo = data.NCHSU346EAJL_ISBNNo;
                        u.NCHSU346EAJL_AffiliatingInsttimeOfPublication = data.NCHSU346EAJL_AffiliatingInsttimeOfPublication;
                        u.NCHSU346EAJL_PublisherName = data.NCHSU346EAJL_PublisherName;
                        u.NCHSU346EAJL_CreatedBy = data.UserId;
                        u.NCHSU346EAJL_UpdatedBy = data.UserId;
                        u.NCHSU346EAJL_CreatedDate = DateTime.Now;
                        u.NCHSU346EAJL_UpdatedDate = DateTime.Now;
                        u.NCHSU346EAJL_year = data.ASMAY_Id;
                        u.NCHSU346EAJL_ActiveFlag = true;
                        _context.Add(u);
                        if (data.filelist.Length > 0)
                        {
                            for (int i = 0; i < data.filelist.Length; i++)
                            {
                                if (data.filelist[0].cfilepath != null)
                                {
                                    HSU_346_EmpApprovedJournalLists_Files_DMO obj2 = new HSU_346_EmpApprovedJournalLists_Files_DMO();
                                    obj2.NCHSU346EAJL_Id = u.NCHSU346EAJL_Id;
                                    obj2.NCHSU346EAJLF_FileName = data.filelist[i].cfilename;
                                    obj2.NCHSU346EAJLF_FileDesc = data.filelist[i].cfiledesc;
                                    obj2.NCHSU346EAJLF_FilePath = data.filelist[i].cfilepath;
                                    obj2.NCHSU346EAJLF_ActiveFlg = true;
                                    obj2.NCHSU346EAJLF_CreatedDate = DateTime.Now;
                                    obj2.NCHSU346EAJLF_UpdatedDate = DateTime.Now;
                                    obj2.NCHSU346EAJLF_CreatedBy = data.UserId;
                                    obj2.NCHSU346EAJLF_UpdatedBy = data.UserId;

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

                else if (data.NCHSU346EAJL_Id > 0)
                {
                    var duplicate = _context.HSU_346_EMPApprovedJournalList_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCHSU346EAJL_Id != data.NCHSU346EAJL_Id && t.NCHSU346EAJL_EmpName == data.NCHSU346EAJL_EmpName && t.NCHSU346EAJL_BookTitle == data.NCHSU346EAJL_BookTitle && t.NCHSU346EAJL_PaperTitle == data.NCHSU346EAJL_PaperTitle && t.NCHSU346EAJL_TitleOfProcConference == data.NCHSU346EAJL_TitleOfProcConference && t.NCHSU346EAJL_NameOfConference == data.NCHSU346EAJL_NameOfConference && t.NCHSU346EAJL_NationalORInternational == data.NCHSU346EAJL_NationalORInternational && t.NCHSU346EAJL_ISBNNo == data.NCHSU346EAJL_ISBNNo && t.NCHSU346EAJL_AffiliatingInsttimeOfPublication == data.NCHSU346EAJL_AffiliatingInsttimeOfPublication && t.NCHSU346EAJL_PublisherName == data.NCHSU346EAJL_PublisherName && t.NCHSU346EAJL_year == data.NCHSU346EAJL_year).Distinct().ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        var j = _context.HSU_346_EMPApprovedJournalList_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCHSU346EAJL_Id == data.NCHSU346EAJL_Id).SingleOrDefault();
                        j.NCHSU346EAJL_EmpName = data.NCHSU346EAJL_EmpName;
                        j.NCHSU346EAJL_BookTitle = data.NCHSU346EAJL_BookTitle;
                        j.NCHSU346EAJL_PaperTitle = data.NCHSU346EAJL_PaperTitle;
                        j.NCHSU346EAJL_TitleOfProcConference = data.NCHSU346EAJL_TitleOfProcConference;
                        j.NCHSU346EAJL_NameOfConference = data.NCHSU346EAJL_NameOfConference;
                        j.NCHSU346EAJL_NationalORInternational = data.NCHSU346EAJL_NationalORInternational;
                        j.NCHSU346EAJL_ISBNNo = data.NCHSU346EAJL_ISBNNo;
                        j.NCHSU346EAJL_AffiliatingInsttimeOfPublication = data.NCHSU346EAJL_AffiliatingInsttimeOfPublication;
                        j.NCHSU346EAJL_PublisherName = data.NCHSU346EAJL_PublisherName;
                        j.NCHSU346EAJL_year = data.ASMAY_Id;
                        j.MI_Id = data.MI_Id;
                        j.NCHSU346EAJL_UpdatedDate = DateTime.Now;
                        j.NCHSU346EAJL_UpdatedBy = data.UserId;
                        _context.Update(j);
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
                                        obj2.NCHSU346EAJL_Id = j.NCHSU346EAJL_Id;
                                        obj2.NCHSU346EAJLF_FileName = data.filelist[i].cfilename;
                                        obj2.NCHSU346EAJLF_FileDesc = data.filelist[i].cfiledesc;
                                        obj2.NCHSU346EAJLF_FilePath = data.filelist[i].cfilepath;
                                        obj2.NCHSU346EAJLF_CreatedBy = data.UserId;
                                        obj2.NCHSU346EAJLF_UpdatedBy = data.UserId;
                                        obj2.NCHSU346EAJLF_CreatedDate = DateTime.Now;
                                        obj2.NCHSU346EAJLF_UpdatedDate = DateTime.Now;
                                        obj2.NCHSU346EAJLF_ActiveFlg = true;
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
                                        obj2.NCHSU346EAJL_Id = j.NCHSU346EAJL_Id;
                                        obj2.NCHSU346EAJLF_FileName = data.filelist[i].cfilename;
                                        obj2.NCHSU346EAJLF_FileDesc = data.filelist[i].cfiledesc;
                                        obj2.NCHSU346EAJLF_FilePath = data.filelist[i].cfilepath;
                                        obj2.NCHSU346EAJLF_CreatedBy = data.UserId;
                                        obj2.NCHSU346EAJLF_UpdatedBy = data.UserId;
                                        obj2.NCHSU346EAJLF_CreatedDate = DateTime.Now;
                                        obj2.NCHSU346EAJLF_UpdatedDate = DateTime.Now;
                                        obj2.NCHSU346EAJLF_ActiveFlg = true;
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
                var g = _context.HSU_346_EMPApprovedJournalList_DMO.Where(t => t.NCHSU346EAJL_Id == data.NCHSU346EAJL_Id).SingleOrDefault();
                if (g.NCHSU346EAJL_ActiveFlag == true)
                {
                    g.NCHSU346EAJL_ActiveFlag = false;
                }
                else
                {
                    g.NCHSU346EAJL_ActiveFlag = true;
                }
                g.NCHSU346EAJL_UpdatedDate = DateTime.Now;
                g.NCHSU346EAJL_UpdatedBy = data.UserId;
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
        public HSU_346_EMPApprovedJournalList_DTO EditData(HSU_346_EMPApprovedJournalList_DTO data)
        {
            try
            {

                data.editlist = (from a in _context.Academic
                                 from b in _context.HSU_346_EMPApprovedJournalList_DMO
                                 where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && b.NCHSU346EAJL_Id == data.NCHSU346EAJL_Id && a.ASMAY_Id == b.NCHSU346EAJL_year)
                                 select new HSU_346_EMPApprovedJournalList_DTO
                                 {
                                     NCHSU346EAJL_Id = b.NCHSU346EAJL_Id,
                                     NCHSU346EAJL_EmpName = b.NCHSU346EAJL_EmpName,
                                     NCHSU346EAJL_BookTitle = b.NCHSU346EAJL_BookTitle,
                                     NCHSU346EAJL_PaperTitle = b.NCHSU346EAJL_PaperTitle,
                                     NCHSU346EAJL_TitleOfProcConference = b.NCHSU346EAJL_TitleOfProcConference,
                                     NCHSU346EAJL_NameOfConference = b.NCHSU346EAJL_NameOfConference,
                                     NCHSU346EAJL_NationalORInternational = b.NCHSU346EAJL_NationalORInternational,
                                     NCHSU346EAJL_ISBNNo = b.NCHSU346EAJL_ISBNNo,
                                     NCHSU346EAJL_AffiliatingInsttimeOfPublication = b.NCHSU346EAJL_AffiliatingInsttimeOfPublication,
                                     NCHSU346EAJL_PublisherName = b.NCHSU346EAJL_PublisherName,
                                     NCHSU346EAJL_year = b.NCHSU346EAJL_year,
                                     MI_Id = b.MI_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();

                data.editFileslist = (from a in _context.HSU_346_EmpApprovedJournalLists_Files_DMO
                                      where (a.NCHSU346EAJL_Id == data.NCHSU346EAJL_Id)
                                      select new HSU_346_EMPApprovedJournalList_DTO
                                      {
                                          cfilename = a.NCHSU346EAJLF_FileName,
                                          cfilepath = a.NCHSU346EAJLF_FilePath,
                                          cfiledesc = a.NCHSU346EAJLF_FileDesc,
                                      }).Distinct().ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
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
                                            cfilepath = a.NCHSU346EAJLF_FilePath,
                                            cfiledesc = a.NCHSU346EAJLF_FileDesc,
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
                                            cfilename = a.NCHSU346EAJLF_FileName,
                                            cfilepath = a.NCHSU346EAJLF_FilePath,
                                            cfiledesc = a.NCHSU346EAJLF_FileDesc,
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


    }
}
