using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.University;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.University.Service
{
    public class MC_343_TechnologyTransferredImpl:Interface.MC_343_TechnologyTransferredInterface
    {
        public GeneralContext _context;
        public MC_343_TechnologyTransferredImpl(GeneralContext y)
        {
            _context = y;
        }
        public MC_343_TechnologyTransferredDTO loaddata(MC_343_TechnologyTransferredDTO data)
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
                                 from b in _context.MC_343_TechnologyTransferredDMO
                                 where (a.MI_Id == data.MI_Id && b.MI_Id == a.MI_Id && a.Is_Active == true && b.NCMCTT343_Year == a.ASMAY_Id)
                                 select new MC_343_TechnologyTransferredDTO
                                 {
                                     NCMCTT343_Id = b.NCMCTT343_Id,
                                     NCMCTT343_Year = b.NCMCTT343_Year,
                                     ASMAY_Year = a.ASMAY_Year,
                                     NCMCTT343_PatenterName = b.NCMCTT343_PatenterName,
                                     NCMCTT343_Patent = b.NCMCTT343_Patent,
                                     NCMCTT343_Title = b.NCMCTT343_Title,                                  
                                     NCMCTT343_ActiveFlag = b.NCMCTT343_ActiveFlag,
                                     MI_Id = data.MI_Id

                                 }).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public MC_343_TechnologyTransferredDTO save(MC_343_TechnologyTransferredDTO data)
        {
            try
            {
                if (data.NCMCTT343_Id == 0)
                {
                    var duplicate = _context.MC_343_TechnologyTransferredDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMCTT343_Year == data.asmaY_Id && t.NCMCTT343_Id != 0 && t.NCMCTT343_PatenterName == data.NCMCTT343_PatenterName && t.NCMCTT343_Patent == data.NCMCTT343_Patent && t.NCMCTT343_Title == data.NCMCTT343_Title).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        MC_343_TechnologyTransferredDMO rrr = new MC_343_TechnologyTransferredDMO();
                        rrr.MI_Id = data.MI_Id;
                        rrr.NCMCTT343_PatenterName = data.NCMCTT343_PatenterName;
                        rrr.NCMCTT343_Patent = data.NCMCTT343_Patent;
                        rrr.NCMCTT343_Title = data.NCMCTT343_Title;                       
                        rrr.NCMCTT343_Year = data.asmaY_Id;
                        rrr.NCMCTT343_CreatedDate = DateTime.Now;
                        rrr.NCMCTT343_UpdatedDate = DateTime.Now;
                        rrr.NCMCTT343_ActiveFlag = true;
                        rrr.NCMCTT343_CreatedBy = data.UserId;
                        rrr.NCMCTT343_UpdatedBy = data.UserId;
                        _context.Add(rrr);
                        if (data.filelist.Length > 0)
                        {
                            for (int i = 0; i < data.filelist.Length; i++)
                            {
                                if (data.filelist[0].cfilepath != null)
                                {
                                    MC_343_TechnologyTransferred_FilesDMO obj2 = new MC_343_TechnologyTransferred_FilesDMO();
                                    obj2.NCMCTT343_Id = rrr.NCMCTT343_Id;
                                    obj2.NCMCTT343F_FileName = data.filelist[i].cfilename;
                                    obj2.NCMCTT343F_Filedesc = data.filelist[i].cfiledesc;
                                    obj2.NCMCTT343F_FilePath = data.filelist[i].cfilepath;
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
                else if (data.NCMCTT343_Id > 0)
                {
                    var duplicate = _context.MC_343_TechnologyTransferredDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMCTT343_PatenterName == data.NCMCTT343_PatenterName && t.NCMCTT343_Patent == data.NCMCTT343_Patent && t.NCMCTT343_Year == data.asmaY_Id && t.NCMCTT343_Id != data.NCMCTT343_Id && t.NCMCTT343_Title == data.NCMCTT343_Title).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var yy = _context.MC_343_TechnologyTransferredDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMCTT343_Id == data.NCMCTT343_Id).SingleOrDefault();
                        yy.NCMCTT343_UpdatedBy = data.UserId;
                        yy.NCMCTT343_PatenterName = data.NCMCTT343_PatenterName;
                        yy.NCMCTT343_Patent = data.NCMCTT343_Patent;
                        yy.NCMCTT343_Title = data.NCMCTT343_Title;                       
                        yy.NCMCTT343_Year = data.asmaY_Id;
                        yy.MI_Id = data.MI_Id;
                        yy.NCMCTT343_UpdatedDate = DateTime.Now;
                        _context.Update(yy);
                        var CountRemoveFiles = _context.MC_343_TechnologyTransferred_FilesDMO.Where(t => t.NCMCTT343_Id == data.NCMCTT343_Id).ToList();
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
                                        MC_343_TechnologyTransferred_FilesDMO obj2 = new MC_343_TechnologyTransferred_FilesDMO();
                                        obj2.NCMCTT343_Id = yy.NCMCTT343_Id;
                                        obj2.NCMCTT343F_FileName = data.filelist[i].cfilename;
                                        obj2.NCMCTT343F_Filedesc = data.filelist[i].cfiledesc;
                                        obj2.NCMCTT343F_FilePath = data.filelist[i].cfilepath;
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
                                        MC_343_TechnologyTransferred_FilesDMO obj2 = new MC_343_TechnologyTransferred_FilesDMO();
                                        obj2.NCMCTT343_Id = yy.NCMCTT343_Id;
                                        obj2.NCMCTT343F_FileName = data.filelist[i].cfilename;
                                        obj2.NCMCTT343F_Filedesc = data.filelist[i].cfiledesc;
                                        obj2.NCMCTT343F_FilePath = data.filelist[i].cfilepath;
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
        public MC_343_TechnologyTransferredDTO deactive(MC_343_TechnologyTransferredDTO data)
        {
            try
            {
                var u = _context.MC_343_TechnologyTransferredDMO.Where(t => t.NCMCTT343_Id == data.NCMCTT343_Id).SingleOrDefault();
                if (u.NCMCTT343_ActiveFlag == true)
                {
                    u.NCMCTT343_ActiveFlag = false;
                }
                else if (u.NCMCTT343_ActiveFlag == false)
                {
                    u.NCMCTT343_ActiveFlag = true;
                }
                u.NCMCTT343_UpdatedDate = DateTime.Now;
                u.NCMCTT343_UpdatedBy = data.UserId;
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
        public MC_343_TechnologyTransferredDTO EditData(MC_343_TechnologyTransferredDTO data)
        {
            try
            {
                data.editlist = (from a in _context.Academic
                                 from b in _context.MC_343_TechnologyTransferredDMO
                                 where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCMCTT343_Year && b.MI_Id == data.MI_Id && b.NCMCTT343_Id == data.NCMCTT343_Id)
                                 select new MC_343_TechnologyTransferredDTO
                                 {
                                     NCMCTT343_Id = b.NCMCTT343_Id,
                                     NCMCTT343_PatenterName = b.NCMCTT343_PatenterName,
                                     NCMCTT343_Patent = b.NCMCTT343_Patent,
                                     NCMCTT343_Title = b.NCMCTT343_Title,
                                     NCMCTT343_Year = b.NCMCTT343_Year,
                                     MI_Id = data.MI_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();
                data.editFileslist = (from a in _context.MC_343_TechnologyTransferred_FilesDMO
                                      where (a.NCMCTT343_Id == data.NCMCTT343_Id)
                                      select new MC_343_TechnologyTransferredDTO
                                      {
                                          cfilename = a.NCMCTT343F_FileName,
                                          cfilepath = a.NCMCTT343F_Filedesc,
                                          cfiledesc = a.NCMCTT343F_FilePath,
                                      }).Distinct().ToArray();
            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public MC_343_TechnologyTransferredDTO viewuploadflies(MC_343_TechnologyTransferredDTO data)
        {
            try
            {
                data.viewuploadflies = (from a in _context.MC_343_TechnologyTransferred_FilesDMO
                                        where (a.NCMCTT343_Id == data.NCMCTT343_Id)
                                        select new MC_343_TechnologyTransferredDTO
                                        {
                                            cfilename = a.NCMCTT343F_FileName,
                                            cfilepath = a.NCMCTT343F_Filedesc,
                                            cfiledesc = a.NCMCTT343F_FilePath,
                                            NCMCTT343F_Id = a.NCMCTT343F_Id,
                                            NCMCTT343_Id = a.NCMCTT343_Id,
                                        }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public MC_343_TechnologyTransferredDTO deleteuploadfile(MC_343_TechnologyTransferredDTO data)
        {
            try
            {
                var res = _context.MC_343_TechnologyTransferred_FilesDMO.Where(t => t.NCMCTT343F_Id == data.NCMCTT343F_Id).SingleOrDefault();
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
                data.viewuploadflies = (from a in _context.MC_343_TechnologyTransferred_FilesDMO
                                        where (a.NCMCTT343_Id == data.NCMCTT343_Id)
                                        select new MC_343_TechnologyTransferredDTO
                                        {
                                            NCMCTT343_Id = a.NCMCTT343_Id,
                                            NCMCTT343F_Id = a.NCMCTT343F_Id,
                                            cfilename = a.NCMCTT343F_FileName,
                                            cfilepath = a.NCMCTT343F_Filedesc,
                                            cfiledesc = a.NCMCTT343F_FilePath,
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
