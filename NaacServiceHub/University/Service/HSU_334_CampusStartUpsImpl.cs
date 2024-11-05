using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.University;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.University.Service
{
    public class HSU_334_CampusStartUpsImpl:Interface.HSU_334_CampusStartUpsInterface
    {
        public GeneralContext _context;
        public HSU_334_CampusStartUpsImpl(GeneralContext y)
        {
            _context = y;
        }
        public HSU_334_CampusStartUpsDTO loaddata(HSU_334_CampusStartUpsDTO data)
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
                                 from b in _context.HSU_334_CampusStartUpsDMO
                                 where (a.MI_Id == data.MI_Id && b.MI_Id == a.MI_Id && a.Is_Active == true && b.NCHSU324CS_Year == a.ASMAY_Id)
                                 select new HSU_334_CampusStartUpsDTO
                                 {
                                     NCHSU324CS_Id = b.NCHSU324CS_Id,
                                     NCHSU324CS_Year = b.NCHSU324CS_Year,
                                     ASMAY_Year = a.ASMAY_Year,
                                     NCHSU324CS_StartUpName = b.NCHSU324CS_StartUpName,
                                     NCHSU324CS_NatureOfStartUp = b.NCHSU324CS_NatureOfStartUp,
                                     NCHSU324CS_Contactinfo = b.NCHSU324CS_Contactinfo,
                                     NCHSU324CS_ActiveFlag = b.NCHSU324CS_ActiveFlag,
                                     MI_Id = data.MI_Id

                                 }).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public HSU_334_CampusStartUpsDTO save(HSU_334_CampusStartUpsDTO data)
        {
            try
            {
                if (data.NCHSU324CS_Id == 0)
                {
                    var duplicate = _context.HSU_334_CampusStartUpsDMO.Where(t => t.MI_Id == data.MI_Id && t.NCHSU324CS_Year == data.asmaY_Id && t.NCHSU324CS_Id != 0 && t.NCHSU324CS_StartUpName == data.NCHSU324CS_StartUpName && t.NCHSU324CS_NatureOfStartUp == data.NCHSU324CS_NatureOfStartUp && t.NCHSU324CS_Contactinfo == data.NCHSU324CS_Contactinfo).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        HSU_334_CampusStartUpsDMO rrr = new HSU_334_CampusStartUpsDMO();
                        rrr.MI_Id = data.MI_Id;
                        rrr.NCHSU324CS_StartUpName = data.NCHSU324CS_StartUpName;
                        rrr.NCHSU324CS_NatureOfStartUp = data.NCHSU324CS_NatureOfStartUp;
                        rrr.NCHSU324CS_Contactinfo = data.NCHSU324CS_Contactinfo;                       
                        rrr.NCHSU324CS_Year = data.asmaY_Id;
                        rrr.NCHSU324CS_CreatedDate = DateTime.Now;
                        rrr.NCHSU324CS_UpdatedDate = DateTime.Now;
                        rrr.NCHSU324CS_CreatedBy = data.UserId;
                        rrr.NCHSU324CS_UpdatedBy = data.UserId;
                        rrr.NCHSU324CS_ActiveFlag = true;

                        _context.Add(rrr);
                        if (data.filelist.Length > 0)
                        {
                            for (int i = 0; i < data.filelist.Length; i++)
                            {
                                if (data.filelist[0].cfilepath != null)
                                {
                                    HSU_334_CampusStartUps_FilesDMO obj2 = new HSU_334_CampusStartUps_FilesDMO();
                                    obj2.NCHSU324CS_Id = rrr.NCHSU324CS_Id;
                                    obj2.NCHSU324CSF_FileName = data.filelist[i].cfilename;
                                    obj2.NCHSU324CSF_Filedesc = data.filelist[i].cfiledesc;
                                    obj2.NCHSU324CSF_FilePath = data.filelist[i].cfilepath;
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
                else if (data.NCHSU324CS_Id > 0)
                {
                    var duplicate = _context.HSU_334_CampusStartUpsDMO.Where(t => t.MI_Id == data.MI_Id && t.NCHSU324CS_StartUpName == data.NCHSU324CS_StartUpName && t.NCHSU324CS_NatureOfStartUp == data.NCHSU324CS_NatureOfStartUp && t.NCHSU324CS_Contactinfo == data.NCHSU324CS_Contactinfo && t.NCHSU324CS_Id != data.NCHSU324CS_Id && t.NCHSU324CS_Year == data.asmaY_Id).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var yy = _context.HSU_334_CampusStartUpsDMO.Where(t => t.MI_Id == data.MI_Id && t.NCHSU324CS_Id == data.NCHSU324CS_Id).SingleOrDefault();                       
                        yy.NCHSU324CS_StartUpName = data.NCHSU324CS_StartUpName;
                        yy.NCHSU324CS_NatureOfStartUp = data.NCHSU324CS_NatureOfStartUp;
                        yy.NCHSU324CS_Contactinfo = data.NCHSU324CS_Contactinfo;                        
                        yy.NCHSU324CS_Year = data.asmaY_Id;
                        yy.MI_Id = data.MI_Id;
                        yy.NCHSU324CS_UpdatedDate = DateTime.Now;
                        yy.NCHSU324CS_UpdatedBy = data.UserId;
                        _context.Update(yy);
                        var CountRemoveFiles = _context.HSU_334_CampusStartUps_FilesDMO.Where(t => t.NCHSU324CS_Id == data.NCHSU324CS_Id).ToList();
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
                                        HSU_334_CampusStartUps_FilesDMO obj2 = new HSU_334_CampusStartUps_FilesDMO();
                                        obj2.NCHSU324CS_Id = yy.NCHSU324CS_Id;
                                        obj2.NCHSU324CSF_FileName = data.filelist[i].cfilename;
                                        obj2.NCHSU324CSF_Filedesc = data.filelist[i].cfiledesc;
                                        obj2.NCHSU324CSF_FilePath = data.filelist[i].cfilepath;
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
                                        HSU_334_CampusStartUps_FilesDMO obj2 = new HSU_334_CampusStartUps_FilesDMO();
                                        obj2.NCHSU324CS_Id = yy.NCHSU324CS_Id;
                                        obj2.NCHSU324CSF_FileName = data.filelist[i].cfilename;
                                        obj2.NCHSU324CSF_Filedesc = data.filelist[i].cfiledesc;
                                        obj2.NCHSU324CSF_FilePath = data.filelist[i].cfilepath;
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
        public HSU_334_CampusStartUpsDTO deactive(HSU_334_CampusStartUpsDTO data)
        {
            try
            {
                var u = _context.HSU_334_CampusStartUpsDMO.Where(t => t.NCHSU324CS_Id == data.NCHSU324CS_Id).SingleOrDefault();
                if (u.NCHSU324CS_ActiveFlag == true)
                {
                    u.NCHSU324CS_ActiveFlag = false;
                }
                else if (u.NCHSU324CS_ActiveFlag == false)
                {
                    u.NCHSU324CS_ActiveFlag = true;
                }
                u.NCHSU324CS_UpdatedDate = DateTime.Now;
                u.NCHSU324CS_UpdatedBy = data.UserId;
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
        public HSU_334_CampusStartUpsDTO EditData(HSU_334_CampusStartUpsDTO data)
        {
            try
            {
                data.editlist = (from a in _context.Academic
                                 from b in _context.HSU_334_CampusStartUpsDMO
                                 where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCHSU324CS_Year && b.MI_Id == data.MI_Id && b.NCHSU324CS_Id == data.NCHSU324CS_Id)
                                 select new HSU_334_CampusStartUpsDTO
                                 {
                                     NCHSU324CS_Id = b.NCHSU324CS_Id,
                                     NCHSU324CS_StartUpName = b.NCHSU324CS_StartUpName,
                                     NCHSU324CS_NatureOfStartUp = b.NCHSU324CS_NatureOfStartUp,
                                     NCHSU324CS_Contactinfo = b.NCHSU324CS_Contactinfo,
                                     NCHSU324CS_Year = b.NCHSU324CS_Year,                                     
                                     MI_Id = data.MI_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();
                data.editFileslist = (from a in _context.HSU_334_CampusStartUps_FilesDMO
                                      where (a.NCHSU324CS_Id == data.NCHSU324CS_Id)
                                      select new HSU_334_CampusStartUpsDTO
                                      {
                                          cfilename = a.NCHSU324CSF_FileName,
                                          cfiledesc = a.NCHSU324CSF_Filedesc,
                                          cfilepath = a.NCHSU324CSF_FilePath,
                                      }).Distinct().ToArray();
            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public HSU_334_CampusStartUpsDTO viewuploadflies(HSU_334_CampusStartUpsDTO data)
        {
            try
            {
                data.viewuploadflies = (from a in _context.HSU_334_CampusStartUps_FilesDMO
                                        where (a.NCHSU324CS_Id == data.NCHSU324CS_Id)
                                        select new HSU_334_CampusStartUpsDTO
                                        {
                                            cfilename = a.NCHSU324CSF_FileName,
                                            cfiledesc = a.NCHSU324CSF_Filedesc,                                            
                                            cfilepath = a.NCHSU324CSF_FilePath,
                                            NCHSU324CSF_Id = a.NCHSU324CSF_Id,
                                            NCHSU324CS_Id = a.NCHSU324CS_Id,
                                        }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public HSU_334_CampusStartUpsDTO deleteuploadfile(HSU_334_CampusStartUpsDTO data)
        {
            try
            {
                var res = _context.HSU_334_CampusStartUps_FilesDMO.Where(t => t.NCHSU324CSF_Id == data.NCHSU324CSF_Id).SingleOrDefault();
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
                data.viewuploadflies = (from a in _context.HSU_334_CampusStartUps_FilesDMO
                                        where (a.NCHSU324CS_Id == data.NCHSU324CS_Id)
                                        select new HSU_334_CampusStartUpsDTO
                                        {
                                            NCHSU324CS_Id = a.NCHSU324CS_Id,
                                            NCHSU324CSF_Id = a.NCHSU324CSF_Id,
                                            cfilename = a.NCHSU324CSF_FileName,
                                            cfiledesc = a.NCHSU324CSF_Filedesc,
                                            cfilepath = a.NCHSU324CSF_FilePath,
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
