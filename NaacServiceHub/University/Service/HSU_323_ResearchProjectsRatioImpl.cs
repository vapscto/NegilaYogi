using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.University;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.University.Service
{
    public class HSU_323_ResearchProjectsRatioImpl:Interface.HSU_323_ResearchProjectsRatioInterface
    {
        public GeneralContext _context;
        public HSU_323_ResearchProjectsRatioImpl(GeneralContext y)
        {
            _context = y;
        }
        public HSU_323_ResearchProjectsRatioDTO loaddata(HSU_323_ResearchProjectsRatioDTO data)
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
                                 from b in _context.HSU_323_ResearchProjectsRatioDMO                                 
                                 where (a.MI_Id == data.MI_Id && b.MI_Id == a.MI_Id && a.Is_Active == true && b.NC323RPR_Year == a.ASMAY_Id)
                                 select new HSU_323_ResearchProjectsRatioDTO
                                 {
                                     NC323RPR_Id = b.NC323RPR_Id,
                                     NC323RPR_Year = b.NC323RPR_Year,
                                     ASMAY_Year = a.ASMAY_Year,
                                     NC323RPR_ProjName = b.NC323RPR_ProjName,
                                     NC323RPR_PricipalName = b.NC323RPR_PricipalName,
                                     NC323RPR_AgencyName = b.NC323RPR_AgencyName,
                                     NC323RPR_Type = b.NC323RPR_Type,
                                     NC323RPR_DeptName = b.NC323RPR_DeptName,
                                     NC323RPR_FundProvided = b.NC323RPR_FundProvided,
                                     NC323RPR_ProjDuration = b.NC323RPR_ProjDuration,
                                     NC323RPR_ActiveFlag = b.NC323RPR_ActiveFlag,
                                     MI_Id = data.MI_Id

                                 }).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public HSU_323_ResearchProjectsRatioDTO save(HSU_323_ResearchProjectsRatioDTO data)
        {
            try
            {
                if (data.NC323RPR_Id == 0)
                {
                    var duplicate = _context.HSU_323_ResearchProjectsRatioDMO.Where(t => t.MI_Id == data.MI_Id && t.NC323RPR_Year == data.asmaY_Id && t.NC323RPR_Id != 0 && t.NC323RPR_ProjName == data.NC323RPR_ProjName && t.NC323RPR_PricipalName == data.NC323RPR_PricipalName && t.NC323RPR_AgencyName == data.NC323RPR_AgencyName && t.NC323RPR_Type == data.NC323RPR_Type && t.NC323RPR_DeptName == data.NC323RPR_DeptName && t.NC323RPR_FundProvided==data.NC323RPR_FundProvided && t.NC323RPR_ProjDuration==data.NC323RPR_ProjDuration).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        HSU_323_ResearchProjectsRatioDMO rrr = new HSU_323_ResearchProjectsRatioDMO();
                        rrr.MI_Id = data.MI_Id;
                        rrr.NC323RPR_ProjName = data.NC323RPR_ProjName;
                        rrr.NC323RPR_PricipalName = data.NC323RPR_PricipalName;
                        rrr.NC323RPR_AgencyName = data.NC323RPR_AgencyName;
                        rrr.NC323RPR_Type = data.NC323RPR_Type;
                        rrr.NC323RPR_DeptName = data.NC323RPR_DeptName;
                        rrr.NC323RPR_FundProvided = data.NC323RPR_FundProvided;
                        rrr.NC323RPR_ProjDuration = data.NC323RPR_ProjDuration;
                        rrr.NC323RPR_Year = data.asmaY_Id;
                        rrr.NC323RPR_CreatedDate = DateTime.Now;
                        rrr.NC323RPR_UpdatedDate = DateTime.Now;
                        rrr.NC323RPR_CreatedBy = data.UserId;
                        rrr.NC323RPR_UpdatedBy = data.UserId;
                        rrr.NC323RPR_ActiveFlag = true;

                        _context.Add(rrr);
                        if (data.filelist.Length > 0)
                        {
                            for (int i = 0; i < data.filelist.Length; i++)
                            {
                                if (data.filelist[0].cfilepath != null)
                                {
                                    HSU_323_ResearchProjectsRatio_FilesDMO obj2 = new HSU_323_ResearchProjectsRatio_FilesDMO();
                                    obj2.NC323RPR_Id = rrr.NC323RPR_Id;
                                    obj2.NC323RPRF_FileName = data.filelist[i].cfilename;
                                    obj2.NC323RPRF_Filedesc = data.filelist[i].cfiledesc;
                                    obj2.NC323RPRF_FilePath = data.filelist[i].cfilepath;
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
                else if (data.NC323RPR_Id > 0)
                {
                    var duplicate = _context.HSU_323_ResearchProjectsRatioDMO.Where(t => t.MI_Id == data.MI_Id && t.NC323RPR_ProjName == data.NC323RPR_ProjName && t.NC323RPR_PricipalName == data.NC323RPR_PricipalName && t.NC323RPR_AgencyName == data.NC323RPR_AgencyName && t.NC323RPR_Type == data.NC323RPR_Type && t.NC323RPR_DeptName == data.NC323RPR_DeptName && t.NC323RPR_Id != data.NC323RPR_Id && t.NC323RPR_Year == data.asmaY_Id && t.NC323RPR_FundProvided==data.NC323RPR_FundProvided && t.NC323RPR_ProjDuration==data.NC323RPR_ProjDuration).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var yy = _context.HSU_323_ResearchProjectsRatioDMO.Where(t => t.MI_Id == data.MI_Id && t.NC323RPR_Id == data.NC323RPR_Id).SingleOrDefault();                       
                        yy.NC323RPR_ProjName = data.NC323RPR_ProjName;
                        yy.NC323RPR_PricipalName = data.NC323RPR_PricipalName;
                        yy.NC323RPR_AgencyName = data.NC323RPR_AgencyName;
                        yy.NC323RPR_Type = data.NC323RPR_Type;
                        yy.NC323RPR_DeptName = data.NC323RPR_DeptName;
                        yy.NC323RPR_FundProvided = data.NC323RPR_FundProvided;
                        yy.NC323RPR_ProjDuration = data.NC323RPR_ProjDuration;
                        yy.NC323RPR_Year = data.asmaY_Id;
                        yy.MI_Id = data.MI_Id;
                        yy.NC323RPR_UpdatedDate = DateTime.Now;
                        yy.NC323RPR_UpdatedBy = data.UserId;
                        _context.Update(yy);
                        var CountRemoveFiles = _context.HSU_323_ResearchProjectsRatio_FilesDMO.Where(t => t.NC323RPR_Id == data.NC323RPR_Id).ToList();
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
                                        HSU_323_ResearchProjectsRatio_FilesDMO obj2 = new HSU_323_ResearchProjectsRatio_FilesDMO();
                                        obj2.NC323RPR_Id = yy.NC323RPR_Id;
                                        obj2.NC323RPRF_FileName = data.filelist[i].cfilename;
                                        obj2.NC323RPRF_Filedesc = data.filelist[i].cfiledesc;
                                        obj2.NC323RPRF_FilePath = data.filelist[i].cfilepath;
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
                                        HSU_323_ResearchProjectsRatio_FilesDMO obj2 = new HSU_323_ResearchProjectsRatio_FilesDMO();
                                        obj2.NC323RPR_Id = yy.NC323RPR_Id;
                                        obj2.NC323RPRF_FileName = data.filelist[i].cfilename;
                                        obj2.NC323RPRF_Filedesc = data.filelist[i].cfiledesc;
                                        obj2.NC323RPRF_FilePath = data.filelist[i].cfilepath;
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
        public HSU_323_ResearchProjectsRatioDTO deactive(HSU_323_ResearchProjectsRatioDTO data)
        {
            try
            {
                var u = _context.HSU_323_ResearchProjectsRatioDMO.Where(t => t.NC323RPR_Id == data.NC323RPR_Id).SingleOrDefault();
                if (u.NC323RPR_ActiveFlag == true)
                {
                    u.NC323RPR_ActiveFlag = false;
                }
                else if (u.NC323RPR_ActiveFlag == false)
                {
                    u.NC323RPR_ActiveFlag = true;
                }
                u.NC323RPR_UpdatedDate = DateTime.Now;
                u.NC323RPR_UpdatedBy = data.UserId;
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
        public HSU_323_ResearchProjectsRatioDTO EditData(HSU_323_ResearchProjectsRatioDTO data)
        {
            try
            {
                data.editlist = (from a in _context.Academic
                                 from b in _context.HSU_323_ResearchProjectsRatioDMO                                 
                                 where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NC323RPR_Year && b.MI_Id == data.MI_Id && b.NC323RPR_Id == data.NC323RPR_Id)
                                 select new HSU_323_ResearchProjectsRatioDTO
                                 {
                                     NC323RPR_Id = b.NC323RPR_Id,
                                     NC323RPR_ProjName = b.NC323RPR_ProjName,
                                     NC323RPR_PricipalName = b.NC323RPR_PricipalName,
                                     NC323RPR_AgencyName = b.NC323RPR_AgencyName,
                                     NC323RPR_Type = b.NC323RPR_Type,
                                     NC323RPR_DeptName = b.NC323RPR_DeptName,
                                     NC323RPR_Year = b.NC323RPR_Year,
                                     NC323RPR_FundProvided = b.NC323RPR_FundProvided,
                                     NC323RPR_ProjDuration = b.NC323RPR_ProjDuration,
                                     MI_Id = data.MI_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();
                data.editFileslist = (from a in _context.HSU_323_ResearchProjectsRatio_FilesDMO
                                      where (a.NC323RPR_Id == data.NC323RPR_Id)
                                      select new HSU_323_ResearchProjectsRatioDTO
                                      {
                                          cfilename = a.NC323RPRF_FileName,
                                          cfiledesc = a.NC323RPRF_Filedesc,                                          
                                          cfilepath = a.NC323RPRF_FilePath,
                                      }).Distinct().ToArray();
            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public HSU_323_ResearchProjectsRatioDTO viewuploadflies(HSU_323_ResearchProjectsRatioDTO data)
        {
            try
            {
                data.viewuploadflies = (from a in _context.HSU_323_ResearchProjectsRatio_FilesDMO
                                        where (a.NC323RPR_Id == data.NC323RPR_Id)
                                        select new HSU_323_ResearchProjectsRatioDTO
                                        {
                                            cfilename = a.NC323RPRF_FileName,
                                            cfiledesc = a.NC323RPRF_Filedesc,
                                            cfilepath = a.NC323RPRF_FilePath,
                                            NC323RPRF_Id = a.NC323RPRF_Id,
                                            NC323RPR_Id = a.NC323RPR_Id,
                                        }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public HSU_323_ResearchProjectsRatioDTO deleteuploadfile(HSU_323_ResearchProjectsRatioDTO data)
        {
            try
            {
                var res = _context.HSU_323_ResearchProjectsRatio_FilesDMO.Where(t => t.NC323RPRF_Id == data.NC323RPRF_Id).SingleOrDefault();
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
                data.viewuploadflies = (from a in _context.HSU_323_ResearchProjectsRatio_FilesDMO
                                        where (a.NC323RPR_Id == data.NC323RPR_Id)
                                        select new HSU_323_ResearchProjectsRatioDTO
                                        {
                                            NC323RPR_Id = a.NC323RPR_Id,
                                            NC323RPRF_Id = a.NC323RPRF_Id,
                                            cfilename = a.NC323RPRF_FileName,
                                            cfiledesc = a.NC323RPRF_Filedesc,
                                            cfilepath = a.NC323RPRF_FilePath,
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
