using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Medical;
using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Medical.Service
{
    public class NAAC_MC_436_EContentImpl : Interface.NAAC_MC_436_EContentInterface
    {
        public GeneralContext _GeneralContext;
        public NAAC_MC_436_EContentImpl(GeneralContext para)
        {
            _GeneralContext = para;
        }
        public NAAC_MC_436_EContent_DTO loaddata(NAAC_MC_436_EContent_DTO data)
        {
            try
            {

                var institutionlist = (from a in _GeneralContext.Institution
                                       from b in _GeneralContext.UserRoleWithInstituteDMO
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
                data.yearlist = (from a in _GeneralContext.Academic
                                 where (a.MI_Id == data.MI_Id && a.Is_Active == true)
                                 select new NAAC_MC_436_EContent_DTO
                                 {
                                     ASMAY_Id = a.ASMAY_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();

                data.emplylist = (from a in _GeneralContext.HR_Master_Employee_DMO
                                  where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                  select new NAAC_MC_436_EContent_DTO
                                  {
                                      empname = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                      HRME_Id = a.HRME_Id,
                                      HRME_EmployeeCode = a.HRME_EmployeeCode,
                                      HRME_EmployeeOrder = a.HRME_EmployeeOrder,
                                  }).Distinct().OrderBy(t => t.HRME_EmployeeOrder).ToArray();
                data.allgridlist = (from a in _GeneralContext.NAAC_MC_436_EContentDMO
                                    from b in _GeneralContext.Academic
                                    from e in _GeneralContext.HR_Master_Employee_DMO
                                    where (a.MI_Id == data.MI_Id && a.NCMCMEC436_Year == b.ASMAY_Id && e.HRME_Id == a.HRME_Id)
                                    select new NAAC_MC_436_EContent_DTO
                                    {
                                        ASMAY_Year = b.ASMAY_Year,
                                        ASMAY_Id = b.ASMAY_Id,
                                        NCMCMEC436_Id = a.NCMCMEC436_Id,
                                        HRME_Id = a.HRME_Id,
                                        NCMCMEC436_ModuleName = a.NCMCMEC436_ModuleName,
                                        NCMCMEC436_PlatformModuleUsed = a.NCMCMEC436_PlatformModuleUsed,
                                        NCMCMEC436_Date = a.NCMCMEC436_Date,
                                        NCMCMEC436_WebLink = a.NCMCMEC436_WebLink,
                                        MI_Id = a.MI_Id,
                                        NCMCMEC436_Year = a.NCMCMEC436_Year,
                                        NCMCMEC436_ActiveFlag = a.NCMCMEC436_ActiveFlag,
                                        empname = ((e.HRME_EmployeeFirstName == null ? " " : e.HRME_EmployeeFirstName) + " " + (e.HRME_EmployeeMiddleName == null ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null ? " " : e.HRME_EmployeeLastName)).Trim(),

                                        HRME_EmployeeCode = e.HRME_EmployeeCode,
                                    }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_MC_436_EContent_DTO savedata(NAAC_MC_436_EContent_DTO data)
        {
            try
            {
                if (data.NCMCMEC436_Id == 0)
                {
                    for (int i = 0; i < data.empchecklist.Length; i++)
                    {
                        var tempid = data.empchecklist[i].HRME_Id;

                        var duplicate = _GeneralContext.NAAC_MC_436_EContentDMO.Where(t => t.NCMCMEC436_ModuleName == data.NCMCMEC436_ModuleName && t.NCMCMEC436_PlatformModuleUsed == data.NCMCMEC436_PlatformModuleUsed && t.MI_Id == data.MI_Id && t.NCMCMEC436_Date == data.NCMCMEC436_Date && t.HRME_Id == tempid && t.NCMCMEC436_Year == data.NCMCMEC436_Year && t.NCMCMEC436_WebLink == data.NCMCMEC436_WebLink).ToList();
                        if (duplicate.Count > 0)
                        {
                            data.msg = "duplicate";
                            data.count += 1;
                        }
                        else
                        {
                            data.count1 += 1;
                            NAAC_MC_436_EContentDMO obj1 = new NAAC_MC_436_EContentDMO();

                            obj1.MI_Id = data.MI_Id;
                            obj1.HRME_Id = tempid;
                            obj1.NCMCMEC436_ModuleName = data.NCMCMEC436_ModuleName;
                            obj1.NCMCMEC436_PlatformModuleUsed = data.NCMCMEC436_PlatformModuleUsed;
                            obj1.NCMCMEC436_Date = data.NCMCMEC436_Date;
                            obj1.NCMCMEC436_WebLink = data.NCMCMEC436_WebLink;
                            obj1.NCMCMEC436_Year = data.NCMCMEC436_Year;
                            obj1.NCMCMEC436_ActiveFlag = true;
                            obj1.NCMCMEC436_CreatedBy = data.UserId;
                            obj1.NCMCMEC436_UpdatedBy = data.UserId;
                            obj1.NCMCMEC436_CreatedDate = DateTime.Now;
                            obj1.NCMCMEC436_UpdatedDate = DateTime.Now;

                            _GeneralContext.Add(obj1);

                            if (data.filelist.Length > 0)
                            {
                                for (int s = 0; s < data.filelist.Length; s++)
                                {
                                    if (data.filelist[s].cfilepath != null)
                                    {
                                        NAAC_MC_436_EContent_FilesDMO obj2 = new NAAC_MC_436_EContent_FilesDMO();

                                        obj2.NCAC434ECTF_FileName = data.filelist[s].cfilename;
                                        obj2.NCAC434ECTF_Filedesc = data.filelist[s].cfiledesc;
                                        obj2.NCAC434ECTF_FilePath = data.filelist[s].cfilepath;
                                        obj2.NCMCMEC436_Id = obj1.NCMCMEC436_Id;

                                        _GeneralContext.Add(obj2);
                                    }
                                }
                            }

                           
                        }
                    }
                    if (data.count1 != 0)
                    {
                        int s = _GeneralContext.SaveChanges();
                        if (s > 0)
                        {
                            data.msg = "saved";
                        }
                        else
                        {
                            data.msg = "savingFailed";
                        }
                    }
                    else
                    {
                        data.msg = "duplicate";
                    }
                }
                else if (data.NCMCMEC436_Id > 0)
                {
                    for (int s = 0; s < data.empchecklist.Length; s++)
                    {
                        var tempid = data.empchecklist[s].HRME_Id;
                        var duplicate = _GeneralContext.NAAC_MC_436_EContentDMO.Where(t => t.NCMCMEC436_Id != data.NCMCMEC436_Id && t.NCMCMEC436_ModuleName == data.NCMCMEC436_ModuleName && t.NCMCMEC436_PlatformModuleUsed == data.NCMCMEC436_PlatformModuleUsed && t.MI_Id == data.MI_Id
                   && t.NCMCMEC436_Date == data.NCMCMEC436_Date && t.HRME_Id == tempid && t.NCMCMEC436_Year == data.NCMCMEC436_Year && t.NCMCMEC436_WebLink == data.NCMCMEC436_WebLink).ToList();
                        if (duplicate.Count > 0)
                        {
                            data.msg = "duplicate";
                            data.count += 1;
                        }
                        else
                        {
                            data.count1 += 1;
                            var update = _GeneralContext.NAAC_MC_436_EContentDMO.Where(t => t.NCMCMEC436_Id == data.NCMCMEC436_Id && t.MI_Id == data.MI_Id).Single();

                            update.HRME_Id = tempid;
                            update.NCMCMEC436_ModuleName = data.NCMCMEC436_ModuleName;
                            update.NCMCMEC436_PlatformModuleUsed = data.NCMCMEC436_PlatformModuleUsed;
                            update.NCMCMEC436_Date = data.NCMCMEC436_Date;
                            update.NCMCMEC436_WebLink = data.NCMCMEC436_WebLink;
                            update.NCMCMEC436_Year = data.NCMCMEC436_Year;
                            update.NCMCMEC436_UpdatedBy = data.UserId;
                            update.NCMCMEC436_UpdatedDate = DateTime.Now;

                            _GeneralContext.Update(update);

                            var CountRemoveFiles = _GeneralContext.NAAC_MC_436_EContent_FilesDMO.Where(t => t.NCMCMEC436_Id == data.NCMCMEC436_Id).ToList();
                            if (CountRemoveFiles.Count > 0)
                            {
                                foreach (var RemoveFiles in CountRemoveFiles)
                                {
                                    _GeneralContext.Remove(RemoveFiles);
                                }
                            }
                            if (data.filelist.Length > 0)
                            {
                                for (int i = 0; i < data.filelist.Length; i++)
                                {
                                    if (data.filelist[0].cfilepath != null)
                                    {
                                        NAAC_MC_436_EContent_FilesDMO obj2 = new NAAC_MC_436_EContent_FilesDMO();

                                        obj2.NCAC434ECTF_FileName = data.filelist[i].cfilename;
                                        obj2.NCAC434ECTF_Filedesc = data.filelist[i].cfiledesc;
                                        obj2.NCAC434ECTF_FilePath = data.filelist[i].cfilepath;
                                        obj2.NCMCMEC436_Id = update.NCMCMEC436_Id;

                                        _GeneralContext.Add(obj2);
                                    }
                                }
                            }
                            
                        }
                    }
                    if (data.count1 != 0)
                    {
                        int row = _GeneralContext.SaveChanges();
                        if (row > 0)
                        {
                            data.msg = "updated";
                        }
                        else
                        {
                            data.msg = "updateFailed";
                        }
                    }
                    else
                    {
                        data.msg = "duplicate";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_MC_436_EContent_DTO editdata(NAAC_MC_436_EContent_DTO data)
        {
            try
            {
                data.editlist = _GeneralContext.NAAC_MC_436_EContentDMO.Where(t => t.NCMCMEC436_Id == data.NCMCMEC436_Id && t.MI_Id == data.MI_Id).ToArray();
                data.editFileslist = (from t in _GeneralContext.NAAC_MC_436_EContent_FilesDMO

                                      where (t.NCMCMEC436_Id == data.NCMCMEC436_Id)
                                      select new NAAC_MC_436_EContent_DTO
                                      {
                                          cfilename = t.NCAC434ECTF_FileName,
                                          cfilepath = t.NCAC434ECTF_FilePath,
                                          cfiledesc = t.NCAC434ECTF_Filedesc,

                                      }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_MC_436_EContent_DTO deactiveStudent(NAAC_MC_436_EContent_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_MC_436_EContentDMO.Where(t => t.NCMCMEC436_Id == data.NCMCMEC436_Id && t.MI_Id == data.MI_Id).Single();
                if (result.NCMCMEC436_ActiveFlag == true)
                {
                    result.NCMCMEC436_ActiveFlag = false;
                }
                else if (result.NCMCMEC436_ActiveFlag == false)
                {
                    result.NCMCMEC436_ActiveFlag = true;
                }
                result.NCMCMEC436_UpdatedDate = DateTime.Now;
                result.NCMCMEC436_UpdatedBy = data.UserId;
                result.MI_Id = data.MI_Id;
                _GeneralContext.Update(result);
                int row = _GeneralContext.SaveChanges();
                if (row > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_MC_436_EContent_DTO viewuploadflies(NAAC_MC_436_EContent_DTO data)
        {
            try
            {
                data.viewuploadflies = (from t in _GeneralContext.NAAC_MC_436_EContent_FilesDMO

                                        where (t.NCMCMEC436_Id == data.NCMCMEC436_Id)
                                        select new NAAC_MC_436_EContent_DTO
                                        {
                                            cfilename = t.NCAC434ECTF_FileName,
                                            cfilepath = t.NCAC434ECTF_FilePath,
                                            cfiledesc = t.NCAC434ECTF_Filedesc,
                                            NCMCMEC436F_Id = t.NCMCMEC436F_Id,
                                            NCMCMEC436_Id = t.NCMCMEC436_Id,

                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public NAAC_MC_436_EContent_DTO deleteuploadfile(NAAC_MC_436_EContent_DTO data)
        {
            try
            {

                var result = _GeneralContext.NAAC_MC_436_EContent_FilesDMO.Where(t => t.NCMCMEC436F_Id == data.NCMCMEC436F_Id).SingleOrDefault();
                _GeneralContext.Remove(result);

                int row = _GeneralContext.SaveChanges();
                if (row > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
                data.viewuploadflies = (from t in _GeneralContext.NAAC_MC_436_EContent_FilesDMO

                                        where (t.NCMCMEC436_Id == data.NCMCMEC436_Id)
                                        select new NAAC_MC_436_EContent_DTO
                                        {
                                            cfilename = t.NCAC434ECTF_FileName,
                                            cfilepath = t.NCAC434ECTF_FilePath,
                                            cfiledesc = t.NCAC434ECTF_Filedesc,
                                            NCMCMEC436F_Id = t.NCMCMEC436F_Id,
                                            NCMCMEC436_Id = t.NCMCMEC436_Id,

                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
    }
}
