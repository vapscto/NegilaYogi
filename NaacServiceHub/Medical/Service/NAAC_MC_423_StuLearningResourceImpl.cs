using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Medical;
using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Medical.Service
{
    public class NAAC_MC_423_StuLearningResourceImpl:Interface.NAAC_MC_423_StuLearningResourceInterface
    {


        public GeneralContext _context;
        public NAAC_MC_423_StuLearningResourceImpl(GeneralContext w)
        {
            _context = w;
        }
        public NAAC_MC_423_StuLearningResource_DTO loaddata(NAAC_MC_423_StuLearningResource_DTO data)
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
                                 from b in _context.NAAC_MC_423_StuLearningResourceDMO
                                 where (a.MI_Id == b.MI_Id && a.Is_Active == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == b.NCMCI423SLR_Year)
                                 select new NAAC_MC_423_StuLearningResource_DTO
                                 {
                                     NCMCI423SLR_Id = b.NCMCI423SLR_Id,
                                     NCMCI423SLR_ResourcesName = b.NCMCI423SLR_ResourcesName,
                                     NCMCI423SLR_NoOfPGStudentsExposed = b.NCMCI423SLR_NoOfPGStudentsExposed,
                                     NCMCI423SLR_NoOfUGStudentsExposed = b.NCMCI423SLR_NoOfUGStudentsExposed,
                                     NCMCI423SLR_Year = b.NCMCI423SLR_Year,
                                     ASMAY_Year = a.ASMAY_Year,
                                    
                                     MI_Id = b.MI_Id,
                                 }).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_MC_423_StuLearningResource_DTO save(NAAC_MC_423_StuLearningResource_DTO data)
        {
            try
            {
                if (data.NCMCI423SLR_Id == 0)
                {
                    var duplicate = _context.NAAC_MC_423_StuLearningResourceDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMCI423SLR_ResourcesName == data.NCMCI423SLR_ResourcesName && t.NCMCI423SLR_NoOfUGStudentsExposed == data.NCMCI423SLR_NoOfUGStudentsExposed && t.NCMCI423SLR_NoOfPGStudentsExposed == data.NCMCI423SLR_NoOfPGStudentsExposed && t.NCMCI423SLR_Id != 0).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        NAAC_MC_423_StuLearningResourceDMO u = new NAAC_MC_423_StuLearningResourceDMO();
                        u.MI_Id = data.MI_Id;
                        u.NCMCI423SLR_ResourcesName = data.NCMCI423SLR_ResourcesName;
                        u.NCMCI423SLR_NoOfUGStudentsExposed = data.NCMCI423SLR_NoOfUGStudentsExposed;
                        u.NCMCI423SLR_NoOfPGStudentsExposed = data.NCMCI423SLR_NoOfPGStudentsExposed;
                        u.NCMCI423SLR_CreatedBy = data.UserId;
                        u.NCMCI423SLR_UpdatedBy = data.UserId;
                        u.NCMCI423SLR_CreateDate = DateTime.Now;
                        u.NCMCI423SLR_UpdatedDate = DateTime.Now;
                        u.NCMCI423SLR_Year = data.ASMAY_Id;
                        _context.Add(u);
                        if (data.filelist.Length > 0)
                        {
                            for (int i = 0; i < data.filelist.Length; i++)
                            {
                                if (data.filelist[0].cfilepath != null)
                                {
                                    NAAC_MC_423_StuLearningResource_FilesDMO obj2 = new NAAC_MC_423_StuLearningResource_FilesDMO();
                                    obj2.NCMCI423SLR_Id = u.NCMCI423SLR_Id;
                                    obj2.NCMCI423SLRF_FileName = data.filelist[i].cfilename;
                                    obj2.NCMCI423SLRF_Filedesc = data.filelist[i].cfiledesc;
                                    obj2.NCMCI423SLRF_FilePath = data.filelist[i].cfilepath;
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

                else if (data.NCMCI423SLR_Id > 0)
                {
                    var duplicate = _context.NAAC_MC_423_StuLearningResourceDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMCI423SLR_Id != data.NCMCI423SLR_Id && t.NCMCI423SLR_ResourcesName == data.NCMCI423SLR_ResourcesName && t.NCMCI423SLR_NoOfUGStudentsExposed == data.NCMCI423SLR_NoOfUGStudentsExposed && t.NCMCI423SLR_NoOfPGStudentsExposed == data.NCMCI423SLR_NoOfPGStudentsExposed && t.NCMCI423SLR_Year == data.NCMCI423SLR_Year).Distinct().ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        var j = _context.NAAC_MC_423_StuLearningResourceDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMCI423SLR_Id == data.NCMCI423SLR_Id).SingleOrDefault();
                        j.NCMCI423SLR_ResourcesName = data.NCMCI423SLR_ResourcesName;
                        j.NCMCI423SLR_NoOfUGStudentsExposed = data.NCMCI423SLR_NoOfUGStudentsExposed;
                        j.NCMCI423SLR_NoOfPGStudentsExposed = data.NCMCI423SLR_NoOfPGStudentsExposed;
                        j.NCMCI423SLR_Year = data.ASMAY_Id;
                        j.MI_Id = data.MI_Id;
                        j.NCMCI423SLR_UpdatedDate = DateTime.Now;
                        j.NCMCI423SLR_UpdatedBy = data.UserId;
                        _context.Update(j);
                        var CountRemoveFiles = _context.NAAC_MC_423_StuLearningResource_FilesDMO.Where(t => t.NCMCI423SLR_Id == data.NCMCI423SLR_Id).ToList();
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

                                        NAAC_MC_423_StuLearningResource_FilesDMO obj2 = new NAAC_MC_423_StuLearningResource_FilesDMO();
                                        obj2.NCMCI423SLR_Id = j.NCMCI423SLR_Id;
                                        obj2.NCMCI423SLRF_FileName = data.filelist[i].cfilename;
                                        obj2.NCMCI423SLRF_Filedesc = data.filelist[i].cfiledesc;
                                        obj2.NCMCI423SLRF_FilePath = data.filelist[i].cfilepath;
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
                                        NAAC_MC_423_StuLearningResource_FilesDMO obj2 = new NAAC_MC_423_StuLearningResource_FilesDMO();
                                        obj2.NCMCI423SLR_Id = j.NCMCI423SLR_Id;
                                      obj2.NCMCI423SLRF_FileName = data.filelist[i].cfilename;
                                        obj2.NCMCI423SLRF_Filedesc = data.filelist[i].cfiledesc;
                                        obj2.NCMCI423SLRF_FilePath = data.filelist[i].cfilepath;
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
  
        public NAAC_MC_423_StuLearningResource_DTO EditData(NAAC_MC_423_StuLearningResource_DTO data)
        {
            try
            {
                data.editlist = (from a in _context.Academic
                                 from b in _context.NAAC_MC_423_StuLearningResourceDMO
                                 where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && b.NCMCI423SLR_Id == data.NCMCI423SLR_Id && a.ASMAY_Id == b.NCMCI423SLR_Year)
                                 select new NAAC_MC_423_StuLearningResource_DTO
                                 {
                                     NCMCI423SLR_Id = b.NCMCI423SLR_Id,
                                     NCMCI423SLR_ResourcesName = b.NCMCI423SLR_ResourcesName,
                                     NCMCI423SLR_NoOfUGStudentsExposed = b.NCMCI423SLR_NoOfUGStudentsExposed,
                                     NCMCI423SLR_NoOfPGStudentsExposed = b.NCMCI423SLR_NoOfPGStudentsExposed,
                                     NCMCI423SLR_Year = b.NCMCI423SLR_Year,
                                     MI_Id = b.MI_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();

                data.editFileslist = (from a in _context.NAAC_MC_423_StuLearningResource_FilesDMO
                                      where (a.NCMCI423SLR_Id == data.NCMCI423SLR_Id)
                                      select new NAAC_MC_423_StuLearningResource_DTO
                                      {
                                          cfilename = a.NCMCI423SLRF_FileName,
                                          cfilepath = a.NCMCI423SLRF_FilePath,
                                          cfiledesc = a.NCMCI423SLRF_Filedesc,
                                      }).Distinct().ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_MC_423_StuLearningResource_DTO viewuploadflies(NAAC_MC_423_StuLearningResource_DTO data)
        {
            try
            {
                data.viewuploadflies = (from a in _context.NAAC_MC_423_StuLearningResource_FilesDMO
                                        where (a.NCMCI423SLR_Id == data.NCMCI423SLR_Id)
                                        select new NAAC_MC_423_StuLearningResource_DTO
                                        {
                                            cfilename = a.NCMCI423SLRF_FileName,
                                            cfilepath = a.NCMCI423SLRF_FilePath,
                                            cfiledesc = a.NCMCI423SLRF_Filedesc,
                                            NCMCI423SLRF_Id = a.NCMCI423SLRF_Id,
                                            NCMCI423SLR_Id = a.NCMCI423SLR_Id,
                                        }).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;

        }
        public NAAC_MC_423_StuLearningResource_DTO deleteuploadfile(NAAC_MC_423_StuLearningResource_DTO data)
        {
            try
            {
                var res = _context.NAAC_MC_423_StuLearningResource_FilesDMO.Where(t => t.NCMCI423SLRF_Id == data.NCMCI423SLRF_Id).SingleOrDefault();
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
                data.viewuploadflies = (from a in _context.NAAC_MC_423_StuLearningResource_FilesDMO
                                        where (a.NCMCI423SLR_Id == data.NCMCI423SLR_Id)
                                        select new NAAC_MC_423_StuLearningResource_DTO
                                        {
                                            cfilename = a.NCMCI423SLRF_FileName,
                                            cfilepath = a.NCMCI423SLRF_FilePath,
                                            cfiledesc = a.NCMCI423SLRF_Filedesc,
                                            NCMCI423SLRF_Id = a.NCMCI423SLRF_Id,
                                            NCMCI423SLR_Id = a.NCMCI423SLR_Id,
                                        }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_MC_423_StuLearningResource_DTO loaddatainfra(NAAC_MC_423_StuLearningResource_DTO data)
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
                                 from b in _context.NAAC_MC_424_Infrastructure_DMO
                                 where (a.MI_Id == b.MI_Id && a.Is_Active == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == b.NCMCI424_Year)
                                 select new NAAC_MC_423_StuLearningResource_DTO
                                 {
                                     NCMCI424_Id = b.NCMCI424_Id,

                                     NCMCI424_Year = b.NCMCI424_Year,
                                     NCMCI424_AttchSatellitePrimaryHealthCenterFlag = b.NCMCI424_AttchSatellitePrimaryHealthCenterFlag,
                                     NCMCI424_AttchRuralHealthCenterFlag = b.NCMCI424_AttchRuralHealthCenterFlag,
                                     NCMCI424_ResFacilityForStudentsORtraineesFlag = b.NCMCI424_ResFacilityForStudentsORtraineesFlag,
                                     NCMC423ICBL_AttcurbanHCTrainingOfStudentsFlag = b.NCMC423ICBL_AttcurbanHCTrainingOfStudentsFlag,
                                     

                                     ASMAY_Year = a.ASMAY_Year,

                                     MI_Id = b.MI_Id,
                                 }).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_MC_423_StuLearningResource_DTO saveinfra(NAAC_MC_423_StuLearningResource_DTO data)
        {
            try
            {
                if (data.NCMCI424_Id == 0)
                {
                    //var duplicate = _context.NAAC_MC_424_Infrastructure_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCMCI424_Id != 0&&data.flag=="a"|| data.flag == "b" || data.flag == "c" || data.flag == "d"||t.NCMCI424_Year==data.ASMAY_Id).ToArray();
                    //if (duplicate.Count() > 0)
                    //{
                    //    data.returnval = true;
                    //}
                    //else
                    //{
                        NAAC_MC_424_Infrastructure_DMO u = new NAAC_MC_424_Infrastructure_DMO();
                        u.MI_Id = data.MI_Id;
                        if (data.flag == "a")
                        {
                            u.NCMCI424_AttchSatellitePrimaryHealthCenterFlag = true;
                            u.NCMCI424_AttchRuralHealthCenterFlag = false;
                            u.NCMCI424_ResFacilityForStudentsORtraineesFlag = false;
                            u.NCMC423ICBL_AttcurbanHCTrainingOfStudentsFlag = false;
                           
                        }
                        else if (data.flag == "b")
                        {
                            u.NCMCI424_AttchSatellitePrimaryHealthCenterFlag = false;
                            u.NCMCI424_AttchRuralHealthCenterFlag = true;
                            u.NCMCI424_ResFacilityForStudentsORtraineesFlag = false;
                            u.NCMC423ICBL_AttcurbanHCTrainingOfStudentsFlag = false;
                         

                        }
                        else if (data.flag == "c")
                        {
                            u.NCMCI424_AttchSatellitePrimaryHealthCenterFlag = false;
                            u.NCMCI424_AttchRuralHealthCenterFlag = false;
                            u.NCMCI424_ResFacilityForStudentsORtraineesFlag = true;
                            u.NCMC423ICBL_AttcurbanHCTrainingOfStudentsFlag = false;
                       

                        }
                        else if (data.flag == "d")
                        {
                            u.NCMCI424_AttchSatellitePrimaryHealthCenterFlag = false;
                            u.NCMCI424_AttchRuralHealthCenterFlag = false;
                            u.NCMCI424_ResFacilityForStudentsORtraineesFlag = false;
                            u.NCMC423ICBL_AttcurbanHCTrainingOfStudentsFlag = true;
                          

                        }
                       



                        u.NCMCI424_CreatedBy = data.UserId;
                        u.NCMCI424_UpdatedBy = data.UserId;
                        u.NCMCI424_CreateDate = DateTime.Now;
                        u.NCMCI424_UpdatedDate = DateTime.Now;
                        u.NCMCI424_Year = data.ASMAY_Id;

                        _context.Add(u);

                        var w = _context.SaveChanges();
                        if (w > 0)
                        {
                            data.msg = "saved";
                        }
                        else
                        {
                            data.msg = "failed";
                        }
                   // }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_MC_423_StuLearningResource_DTO EditDatainfra(NAAC_MC_423_StuLearningResource_DTO data)
        {
            try
            {
                           
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
    }
}
