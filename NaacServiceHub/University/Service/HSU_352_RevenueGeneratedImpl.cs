using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.University;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.University.Service
{
    public class HSU_352_RevenueGeneratedImpl:Interface.HSU_352_RevenueGeneratedInterface
    {
        public GeneralContext _context;
        public HSU_352_RevenueGeneratedImpl(GeneralContext y)
        {
            _context = y;
        }
        public HSU_352_RevenueGeneratedDTO loaddata(HSU_352_RevenueGeneratedDTO data)
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
                                 from b in _context.HSU_352_RevenueGeneratedDMO
                                 where (a.MI_Id == data.MI_Id && b.MI_Id == a.MI_Id && a.Is_Active == true && b.NCMCRG352_Year == a.ASMAY_Id)
                                 select new HSU_352_RevenueGeneratedDTO
                                 {
                                     NCMCRG352_Id = b.NCMCRG352_Id,
                                     NCMCRG352_Year = b.NCMCRG352_Year,
                                     ASMAY_Year = a.ASMAY_Year,
                                     NCMCRG352_ConsultantName = b.NCMCRG352_ConsultantName,
                                     NCMCRG352_AdvisoryName = b.NCMCRG352_AdvisoryName,
                                     NCMCRG352_ConsultingORSpnAgencyCD = b.NCMCRG352_ConsultingORSpnAgencyCD,
                                     NCMCRG352_RevenueGeneratedAmount = b.NCMCRG352_RevenueGeneratedAmount,
                                     NCMCRG352_ActiveFlag = b.NCMCRG352_ActiveFlag,
                                     MI_Id = data.MI_Id

                                 }).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public HSU_352_RevenueGeneratedDTO save(HSU_352_RevenueGeneratedDTO data)
        {
            try
            {
                if (data.NCMCRG352_Id == 0)
                {
                    var duplicate = _context.HSU_352_RevenueGeneratedDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMCRG352_Year == data.asmaY_Id && t.NCMCRG352_Id != 0 && t.NCMCRG352_ConsultantName == data.NCMCRG352_ConsultantName && t.NCMCRG352_AdvisoryName == data.NCMCRG352_AdvisoryName && t.NCMCRG352_ConsultingORSpnAgencyCD == data.NCMCRG352_ConsultingORSpnAgencyCD && t.NCMCRG352_RevenueGeneratedAmount == data.NCMCRG352_RevenueGeneratedAmount).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        HSU_352_RevenueGeneratedDMO rrr = new HSU_352_RevenueGeneratedDMO();
                        rrr.MI_Id = data.MI_Id;
                        rrr.NCMCRG352_ConsultantName = data.NCMCRG352_ConsultantName;
                        rrr.NCMCRG352_AdvisoryName = data.NCMCRG352_AdvisoryName;
                        rrr.NCMCRG352_ConsultingORSpnAgencyCD = data.NCMCRG352_ConsultingORSpnAgencyCD;
                        rrr.NCMCRG352_RevenueGeneratedAmount = data.NCMCRG352_RevenueGeneratedAmount;
                        rrr.NCMCRG352_Year = data.asmaY_Id;
                        rrr.NCMCRG352_CreatedDate = DateTime.Now;
                        rrr.NCMCRG352_UpdatedDate = DateTime.Now;
                        rrr.NCMCRG352_ActiveFlag = true;
                        rrr.NCMCRG352_CreatedBy = data.UserId;
                        rrr.NCMCRG352_UpdatedBy = data.UserId;
                        _context.Add(rrr);
                        if (data.filelist.Length > 0)
                        {
                            for (int i = 0; i < data.filelist.Length; i++)
                            {
                                if (data.filelist[0].cfilepath != null)
                                {
                                    HSU_352_RevenueGenerated_FilesDMO obj2 = new HSU_352_RevenueGenerated_FilesDMO();
                                    obj2.NCMCRG352_Id = rrr.NCMCRG352_Id;
                                    obj2.NCMCRG352F_FileName = data.filelist[i].cfilename;
                                    obj2.NCMCRG352F_Filedesc = data.filelist[i].cfiledesc;
                                    obj2.NCMCRG352F_FilePath = data.filelist[i].cfilepath;
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
                else if (data.NCMCRG352_Id > 0)
                {
                    var duplicate = _context.HSU_352_RevenueGeneratedDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMCRG352_ConsultantName == data.NCMCRG352_ConsultantName && t.NCMCRG352_AdvisoryName == data.NCMCRG352_AdvisoryName && t.NCMCRG352_Year == data.asmaY_Id && t.NCMCRG352_Id != data.NCMCRG352_Id && t.NCMCRG352_ConsultingORSpnAgencyCD == data.NCMCRG352_ConsultingORSpnAgencyCD && t.NCMCRG352_RevenueGeneratedAmount == data.NCMCRG352_RevenueGeneratedAmount).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var yy = _context.HSU_352_RevenueGeneratedDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMCRG352_Id == data.NCMCRG352_Id).SingleOrDefault();
                        yy.NCMCRG352_UpdatedBy = data.UserId;
                        yy.NCMCRG352_ConsultantName = data.NCMCRG352_ConsultantName;
                        yy.NCMCRG352_AdvisoryName = data.NCMCRG352_AdvisoryName;
                        yy.NCMCRG352_ConsultingORSpnAgencyCD = data.NCMCRG352_ConsultingORSpnAgencyCD;
                        yy.NCMCRG352_RevenueGeneratedAmount = data.NCMCRG352_RevenueGeneratedAmount;
                        yy.NCMCRG352_Year = data.asmaY_Id;
                        yy.MI_Id = data.MI_Id;
                        yy.NCMCRG352_UpdatedDate = DateTime.Now;
                        _context.Update(yy);
                        var CountRemoveFiles = _context.HSU_352_RevenueGenerated_FilesDMO.Where(t => t.NCMCRG352_Id == data.NCMCRG352_Id).ToList();
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
                                        HSU_352_RevenueGenerated_FilesDMO obj2 = new HSU_352_RevenueGenerated_FilesDMO();
                                        obj2.NCMCRG352_Id = yy.NCMCRG352_Id;
                                        obj2.NCMCRG352F_FileName = data.filelist[i].cfilename;
                                        obj2.NCMCRG352F_Filedesc = data.filelist[i].cfiledesc;
                                        obj2.NCMCRG352F_FilePath = data.filelist[i].cfilepath;
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
                                        HSU_352_RevenueGenerated_FilesDMO obj2 = new HSU_352_RevenueGenerated_FilesDMO();
                                        obj2.NCMCRG352_Id = yy.NCMCRG352_Id;
                                        obj2.NCMCRG352F_FileName = data.filelist[i].cfilename;
                                        obj2.NCMCRG352F_Filedesc = data.filelist[i].cfiledesc;
                                        obj2.NCMCRG352F_FilePath = data.filelist[i].cfilepath;
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
        public HSU_352_RevenueGeneratedDTO deactive(HSU_352_RevenueGeneratedDTO data)
        {
            try
            {
                var u = _context.HSU_352_RevenueGeneratedDMO.Where(t => t.NCMCRG352_Id == data.NCMCRG352_Id).SingleOrDefault();
                if (u.NCMCRG352_ActiveFlag == true)
                {
                    u.NCMCRG352_ActiveFlag = false;
                }
                else if (u.NCMCRG352_ActiveFlag == false)
                {
                    u.NCMCRG352_ActiveFlag = true;
                }
                u.NCMCRG352_UpdatedDate = DateTime.Now;
                u.NCMCRG352_UpdatedBy = data.UserId;
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
        public HSU_352_RevenueGeneratedDTO EditData(HSU_352_RevenueGeneratedDTO data)
        {
            try
            {
                data.editlist = (from a in _context.Academic
                                 from b in _context.HSU_352_RevenueGeneratedDMO
                                 where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCMCRG352_Year && b.MI_Id == data.MI_Id && b.NCMCRG352_Id == data.NCMCRG352_Id)
                                 select new HSU_352_RevenueGeneratedDTO
                                 {
                                     NCMCRG352_Id = b.NCMCRG352_Id,
                                     NCMCRG352_ConsultantName = b.NCMCRG352_ConsultantName,
                                     NCMCRG352_AdvisoryName = b.NCMCRG352_AdvisoryName,
                                     NCMCRG352_ConsultingORSpnAgencyCD = b.NCMCRG352_ConsultingORSpnAgencyCD,
                                     NCMCRG352_RevenueGeneratedAmount = b.NCMCRG352_RevenueGeneratedAmount,
                                     NCMCRG352_Year = b.NCMCRG352_Year,
                                     MI_Id = data.MI_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();
                data.editFileslist = (from a in _context.HSU_352_RevenueGenerated_FilesDMO
                                      where (a.NCMCRG352_Id == data.NCMCRG352_Id)
                                      select new HSU_352_RevenueGeneratedDTO
                                      {
                                          cfilename = a.NCMCRG352F_FileName,
                                          cfilepath = a.NCMCRG352F_FilePath,
                                          cfiledesc = a.NCMCRG352F_Filedesc,
                                      }).Distinct().ToArray();
            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public HSU_352_RevenueGeneratedDTO viewuploadflies(HSU_352_RevenueGeneratedDTO data)
        {
            try
            {
                data.viewuploadflies = (from a in _context.HSU_352_RevenueGenerated_FilesDMO
                                        where (a.NCMCRG352_Id == data.NCMCRG352_Id)
                                        select new HSU_352_RevenueGeneratedDTO
                                        {
                                            cfilename = a.NCMCRG352F_FileName,
                                            cfilepath = a.NCMCRG352F_FilePath,
                                            cfiledesc = a.NCMCRG352F_Filedesc,
                                            NCMCRG352F_Id = a.NCMCRG352F_Id,
                                            NCMCRG352_Id = a.NCMCRG352_Id,
                                        }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public HSU_352_RevenueGeneratedDTO deleteuploadfile(HSU_352_RevenueGeneratedDTO data)
        {
            try
            {
                var res = _context.HSU_352_RevenueGenerated_FilesDMO.Where(t => t.NCMCRG352F_Id == data.NCMCRG352F_Id).SingleOrDefault();
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
                data.viewuploadflies = (from a in _context.HSU_352_RevenueGenerated_FilesDMO
                                        where (a.NCMCRG352_Id == data.NCMCRG352_Id)
                                        select new HSU_352_RevenueGeneratedDTO
                                        {
                                            NCMCRG352_Id = a.NCMCRG352_Id,
                                            NCMCRG352F_Id = a.NCMCRG352F_Id,
                                            cfilename = a.NCMCRG352F_FileName,
                                            cfilepath = a.NCMCRG352F_FilePath,
                                            cfiledesc = a.NCMCRG352F_Filedesc,
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
