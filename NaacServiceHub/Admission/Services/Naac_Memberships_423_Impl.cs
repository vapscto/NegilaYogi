using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class Naac_Memberships_423_Impl : Interface.Naac_Memberships_423_Interface
    {

        public GeneralContext _GeneralContext;
        public Naac_Memberships_423_Impl(GeneralContext w)
        {
            _GeneralContext = w;
        }

        public Naac_Memberships_423_DTO loaddata(Naac_Memberships_423_DTO data)
      
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
                                 select new Naac_Memberships_423_DTO
                                 {
                                     ASMAY_Id = a.ASMAY_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();
                data.alldata1 = _GeneralContext.NAAC_AC_423_Memberships_DMO.Where(t => t.MI_Id == data.MI_Id).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public Naac_Memberships_423_DTO save(Naac_Memberships_423_DTO data)
        {
            try
            {
                if (data.NCAC423MEM_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_423_Memberships_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC423MEM_Id != 0 && t.NCAC423MEM_Membership == data.NCAC423MEM_Membership
                    && t.NCAC423MEM_Subscription == data.NCAC423MEM_Subscription && t.NCAC423MEM_ValidityPeriod == data.NCAC423MEM_ValidityPeriod && t.NCAC423MEM_UsageReport == data.NCAC423MEM_UsageReport
                    && t.NCAC423MEM_NoOfEResources == data.NCAC423MEM_NoOfEResources && t.NCAC423MEM_RemoteAccessFlg ==
                    data.NCAC423MEM_RemoteAccessFlg).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_423_Memberships_DMO obj1 = new NAAC_AC_423_Memberships_DMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCAC423MEM_Membership = data.NCAC423MEM_Membership;
                        obj1.NCAC423MEM_Subscription = data.NCAC423MEM_Subscription;
                        obj1.NCAC423MEM_Year = data.ASMAY_Id;
                        obj1.NCAC423MEM_Fulltextaccess = data.NCAC423MEM_Fulltextaccess;
                        obj1.NCAC423MEM_WeblinkOfRemoteAccess = data.NCAC423MEM_WeblinkOfRemoteAccess;
                        obj1.NCAC423MEM_ValidityPeriod = data.NCAC423MEM_ValidityPeriod;
                        obj1.NCAC423MEM_UsageReport = data.NCAC423MEM_UsageReport;
                        obj1.NCAC423MEM_NoOfEResources = data.NCAC423MEM_NoOfEResources;
                        obj1.NCAC423MEM_RemoteAccessFlg = data.NCAC423MEM_RemoteAccessFlg;
                        obj1.CreatedDate = DateTime.Now;
                        obj1.UpdatedDate = DateTime.Now;
                        obj1.NCAC423MEM_ActiveFlg = true;
                        obj1.NCAC423MEM_CreatedBy = data.UserId;
                        obj1.NCAC423MEM_UpdatedBy = data.UserId;
                        
                        obj1.MI_Id = data.MI_Id;
                        _GeneralContext.Add(obj1);

                        if (data.filelist.Length > 0)
                        {
                            for (int i = 0; i < data.filelist.Length; i++)
                            {
                                if (data.filelist[i].cfilepath != null)
                                {
                                    NAAC_AC_423_Memberships_Files_DMO obj2 = new NAAC_AC_423_Memberships_Files_DMO();

                                    obj2.NCAC423MEMF_FileName = data.filelist[i].cfilename;
                                    obj2.NCAC423MEMF_Filedesc = data.filelist[i].cfiledesc;
                                    obj2.NCAC423MEMF_FilePath = data.filelist[i].cfilepath;
                                    obj2.NCAC423MEM_Id = obj1.NCAC423MEM_Id;
                                    obj2.NCAC423MEMF_StatusFlg = "";
                                    obj2.NCAC423MEMF_ActiveFlg = true;

                                    _GeneralContext.Add(obj2);
                                }
                            }
                        }

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
                }
                else if (data.NCAC423MEM_Id > 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_423_Memberships_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC423MEM_Membership == data.NCAC423MEM_Membership && t.NCAC423MEM_Subscription == data.NCAC423MEM_Subscription && t.NCAC423MEM_ValidityPeriod == data.NCAC423MEM_ValidityPeriod && t.NCAC423MEM_Id != data.NCAC423MEM_Id && t.NCAC423MEM_UsageReport == data.NCAC423MEM_UsageReport && t.NCAC423MEM_NoOfEResources == data.NCAC423MEM_NoOfEResources).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _GeneralContext.NAAC_AC_423_Memberships_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC423MEM_Id == data.NCAC423MEM_Id).SingleOrDefault();

                        update.NCAC423MEM_UpdatedBy = data.UserId;
                        update.NCAC423MEM_Year = data.ASMAY_Id;
                        update.NCAC423MEM_Fulltextaccess = data.NCAC423MEM_Fulltextaccess;
                        update.NCAC423MEM_WeblinkOfRemoteAccess = data.NCAC423MEM_WeblinkOfRemoteAccess;
                        update.NCAC423MEM_Membership = data.NCAC423MEM_Membership;
                        update.NCAC423MEM_Subscription = data.NCAC423MEM_Subscription;
                        update.NCAC423MEM_ValidityPeriod = data.NCAC423MEM_ValidityPeriod;
                        update.NCAC423MEM_UsageReport = data.NCAC423MEM_UsageReport;
                        update.NCAC423MEM_NoOfEResources = data.NCAC423MEM_NoOfEResources;
                        update.NCAC423MEM_RemoteAccessFlg = data.NCAC423MEM_RemoteAccessFlg;
                        update.MI_Id = data.MI_Id;
                        update.UpdatedDate = DateTime.Now;
                        _GeneralContext.Update(update);

                       
                        if (data.filelist.Length > 0)
                        {

                            List<long> Fid = new List<long>();
                            foreach (var item in data.filelist)
                            {
                                Fid.Add(item.NCAC423MEMF_Id);
                            }
                            var removefile11 = _GeneralContext.NAAC_AC_423_Memberships_Files_DMO.Where(t => t.NCAC423MEM_Id == data.NCAC423MEM_Id && !Fid.Contains(t.NCAC423MEMF_Id)).Distinct().ToList();

                            if (removefile11.Count > 0)
                            {
                                foreach (var item2 in removefile11)
                                {
                                    var deactfile = _GeneralContext.NAAC_AC_423_Memberships_Files_DMO.Single(t => t.NCAC423MEM_Id == data.NCAC423MEM_Id && t.NCAC423MEMF_Id == item2.NCAC423MEMF_Id);
                                    deactfile.NCAC423MEMF_ActiveFlg = false;
                                    _GeneralContext.Update(deactfile);

                                }

                            }
                            //var CountRemoveFiles = _GeneralContext.NAAC_AC_423_Memberships_Files_DMO.Where(t => t.NCAC423MEM_Id == data.NCAC423MEM_Id&&t.NCAC423MEMF_StatusFlg!="approved").ToList();
                            //if (CountRemoveFiles.Count > 0)
                            //{
                            //    foreach (var RemoveFiles in CountRemoveFiles)
                            //    {
                            //        _GeneralContext.Remove(RemoveFiles);
                            //    }
                            //}
                            foreach (Naac_Memberships_423_DTO DocumentsDTO in data.filelist)
                            {

                                if (DocumentsDTO.NCAC423MEMF_Id > 0 && DocumentsDTO.NCAC423MEMF_StatusFlg != "approved")
                                {
                                    if (DocumentsDTO.cfilepath != null)
                                    {

                                        var filesdata = _GeneralContext.NAAC_AC_423_Memberships_Files_DMO.Where(t => t.NCAC423MEMF_Id == DocumentsDTO.NCAC423MEMF_Id).FirstOrDefault();
                                        filesdata.NCAC423MEMF_Filedesc = DocumentsDTO.cfiledesc;
                                        filesdata.NCAC423MEMF_FileName = DocumentsDTO.cfilename;
                                        filesdata.NCAC423MEMF_FilePath = DocumentsDTO.cfilepath;


                                        _GeneralContext.Update(filesdata);
                                        
                                    }
                                }
                                else
                                {

                                    if (DocumentsDTO.NCAC423MEMF_Id == 0)
                                    {
                                        if (DocumentsDTO.cfilepath != null)
                                        {
                                            NAAC_AC_423_Memberships_Files_DMO obj2 = new NAAC_AC_423_Memberships_Files_DMO();
                                            obj2.NCAC423MEMF_FileName = DocumentsDTO.cfilename;
                                            obj2.NCAC423MEMF_Filedesc = DocumentsDTO.cfiledesc;
                                            obj2.NCAC423MEMF_FilePath = DocumentsDTO.cfilepath;
                                            obj2.NCAC423MEMF_StatusFlg = "";
                                            obj2.NCAC423MEMF_ActiveFlg = true;
                                            obj2.NCAC423MEM_Id = data.NCAC423MEM_Id;
                                            _GeneralContext.Add(obj2);
                                            
                                        }
                                    }
                                }
                            }

                        }

                        int flag = _GeneralContext.SaveChanges();
                        if (flag > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
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
        public Naac_Memberships_423_DTO EditData(Naac_Memberships_423_DTO data)
        {
            try
            {
                data.editlist = (from b in _GeneralContext.NAAC_AC_423_Memberships_DMO
                                 where (b.MI_Id == data.MI_Id && b.NCAC423MEM_Id == data.NCAC423MEM_Id)
                                 select new Naac_Memberships_423_DTO
                                 {
                                     NCAC423MEM_Id = b.NCAC423MEM_Id,
                                     NCAC423MEM_Membership = b.NCAC423MEM_Membership,
                                     NCAC423MEM_Subscription = b.NCAC423MEM_Subscription,
                                     NCAC423MEM_NoOfEResources = b.NCAC423MEM_NoOfEResources,
                                     NCAC423MEM_ValidityPeriod = b.NCAC423MEM_ValidityPeriod,
                                     NCAC423MEM_UsageReport = b.NCAC423MEM_UsageReport,
                                     NCAC423MEM_RemoteAccessFlg = b.NCAC423MEM_RemoteAccessFlg,
                                     NCAC423MEM_Year = b.NCAC423MEM_Year,
                                     NCAC423MEM_Fulltextaccess = b.NCAC423MEM_Fulltextaccess,
                                     NCAC423MEM_WeblinkOfRemoteAccess = b.NCAC423MEM_WeblinkOfRemoteAccess,
                                     MI_Id = b.MI_Id
                                 }).Distinct().ToArray();

                data.editFileslist = (from t in _GeneralContext.NAAC_AC_423_Memberships_Files_DMO
                                      where (t.NCAC423MEM_Id == data.NCAC423MEM_Id&&t.NCAC423MEMF_ActiveFlg==true)
                                      select new Naac_Memberships_423_DTO
                                      {
                                          cfilename = t.NCAC423MEMF_FileName,
                                          cfilepath = t.NCAC423MEMF_FilePath,
                                          cfiledesc = t.NCAC423MEMF_Filedesc,
                                          NCAC423MEMF_StatusFlg = t.NCAC423MEMF_StatusFlg,
                                          NCAC423MEMF_Id = t.NCAC423MEMF_Id,

                                      }).Distinct().ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public Naac_Memberships_423_DTO deactiveStudent(Naac_Memberships_423_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_423_Memberships_DMO.Where(t => t.NCAC423MEM_Id == data.NCAC423MEM_Id).SingleOrDefault();
                if (result.NCAC423MEM_ActiveFlg == true)
                {
                    result.NCAC423MEM_ActiveFlg = false;
                }
                else if (result.NCAC423MEM_ActiveFlg == false)
                {
                    result.NCAC423MEM_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                result.NCAC423MEM_UpdatedBy = data.UserId;
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
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public Naac_Memberships_423_DTO viewuploadflies(Naac_Memberships_423_DTO data)
        {
            try
            {
                data.viewuploadflies = (from t in _GeneralContext.NAAC_AC_423_Memberships_Files_DMO

                                        where (t.NCAC423MEM_Id == data.NCAC423MEM_Id&&t.NCAC423MEMF_ActiveFlg==true)
                                        select new Naac_Memberships_423_DTO
                                        {
                                            cfilename = t.NCAC423MEMF_FileName,
                                            cfilepath = t.NCAC423MEMF_FilePath,
                                            cfiledesc = t.NCAC423MEMF_Filedesc,
                                            NCAC423MEMF_Id = t.NCAC423MEMF_Id,
                                            NCAC423MEM_Id = t.NCAC423MEM_Id,
                                            NCAC423MEMF_StatusFlg = t.NCAC423MEMF_StatusFlg,
                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }

        public Naac_Memberships_423_DTO getcomment(Naac_Memberships_423_DTO data)
        {
            try
            {
                data.commentlist = (from a in _GeneralContext.NAAC_AC_423_Memberships_Comments_DMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCAC423MEMC_RemarksBy == b.Id && a.NCAC423MEM_Id == data.NCAC423MEM_Id)
                                    select new Naac_Memberships_423_DTO
                                    {
                                        NCAC423MEMC_Remarks = a.NCAC423MEMC_Remarks,
                                        NCAC423MEMC_Id = a.NCAC423MEMC_Id,
                                        NCAC423MEMC_RemarksBy = a.NCAC423MEMC_RemarksBy,
                                        NCAC423MEMC_StatusFlg = a.NCAC423MEMC_StatusFlg,
                                        NCAC423MEMC_ActiveFlag = a.NCAC423MEMC_ActiveFlag,
                                        NCAC423MEMC_CreatedBy = a.NCAC423MEMC_CreatedBy,
                                        NCAC423MEMC_CreatedDate = a.NCAC423MEMC_CreatedDate,
                                        NCAC423MEMC_UpdatedBy = a.NCAC423MEMC_UpdatedBy,
                                        NCAC423MEMC_UpdatedDate = a.NCAC423MEMC_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NCAC423MEMC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        // for file
        public Naac_Memberships_423_DTO getfilecomment(Naac_Memberships_423_DTO data)
        {
            try
            {
                data.commentlist1 = (from a in _GeneralContext.NAAC_AC_423_Memberships_File_Comments_DMO
                                     from b in _GeneralContext.ApplUser
                                     where (a.NCAC423MEMFC_RemarksBy == b.Id && a.NCAC423MEMF_Id == data.NCAC423MEMF_Id)
                                     select new Naac_Memberships_423_DTO
                                     {
                                         NCAC423MEMF_Id = a.NCAC423MEMF_Id,
                                         NCAC423MEMFC_Remarks = a.NCAC423MEMFC_Remarks,
                                         NCAC423MEMFC_Id = a.NCAC423MEMFC_Id,
                                         NCAC423MEMFC_RemarksBy = a.NCAC423MEMFC_RemarksBy,
                                         NCAC423MEMFC_StatusFlg = a.NCAC423MEMFC_StatusFlg,
                                         NCAC423MEMFC_ActiveFlag = a.NCAC423MEMFC_ActiveFlag,
                                         NCAC423MEMFC_CreatedBy = a.NCAC423MEMFC_CreatedBy,
                                         NCAC423MEMFC_CreatedDate = a.NCAC423MEMFC_CreatedDate,
                                         NCAC423MEMFC_UpdatedBy = a.NCAC423MEMFC_UpdatedBy,
                                         NCAC423MEMFC_UpdatedDate = a.NCAC423MEMFC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().OrderByDescending(a => a.NCAC423MEMFC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Naac_Memberships_423_DTO savemedicaldatawisecomments(Naac_Memberships_423_DTO data)
        {
            try
            {
                NAAC_AC_423_Memberships_Comments_DMO obj1 = new NAAC_AC_423_Memberships_Comments_DMO();
                obj1.NCAC423MEMC_Remarks = data.Remarks;
                obj1.NCAC423MEMC_RemarksBy = data.UserId;
                obj1.NCAC423MEMC_StatusFlg = "";
                obj1.NCAC423MEM_Id = data.filefkid;
                obj1.NCAC423MEMC_ActiveFlag = true;
                obj1.NCAC423MEMC_CreatedBy = data.UserId;
                obj1.NCAC423MEMC_UpdatedBy = data.UserId;
                obj1.NCAC423MEMC_CreatedDate = DateTime.Now;
                obj1.NCAC423MEMC_UpdatedDate = DateTime.Now;
                _GeneralContext.Add(obj1);
                int s = _GeneralContext.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        // for file adding
        public Naac_Memberships_423_DTO savefilewisecomments(Naac_Memberships_423_DTO data)
        {
            try
            {
                NAAC_AC_423_Memberships_File_Comments_DMO obj1 = new NAAC_AC_423_Memberships_File_Comments_DMO();
                obj1.NCAC423MEMFC_Remarks = data.Remarks;
                obj1.NCAC423MEMFC_RemarksBy = data.UserId;
                obj1.NCAC423MEMFC_StatusFlg = "";
                obj1.NCAC423MEMF_Id = data.filefkid;
                obj1.NCAC423MEMFC_ActiveFlag = true;
                obj1.NCAC423MEMFC_CreatedBy = data.UserId;
                obj1.NCAC423MEMFC_UpdatedBy = data.UserId;
                obj1.NCAC423MEMFC_UpdatedDate = DateTime.Now;
                obj1.NCAC423MEMFC_CreatedDate = DateTime.Now;
                _GeneralContext.Add(obj1);
                int s = _GeneralContext.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }

        public Naac_Memberships_423_DTO deleteuploadfile(Naac_Memberships_423_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_423_Memberships_Files_DMO.Where(t => t.NCAC423MEMF_Id == data.NCAC423MEMF_Id).SingleOrDefault();
                result.NCAC423MEMF_ActiveFlg = false;
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
                data.viewuploadflies = (from t in _GeneralContext.NAAC_AC_423_Memberships_Files_DMO

                                        where (t.NCAC423MEM_Id == data.NCAC423MEM_Id&&t.NCAC423MEMF_ActiveFlg==true)
                                        select new Naac_Memberships_423_DTO
                                        {
                                            cfilename = t.NCAC423MEMF_FileName,
                                            cfilepath = t.NCAC423MEMF_FilePath,
                                            cfiledesc = t.NCAC423MEMF_Filedesc,
                                            NCAC423MEMF_Id = t.NCAC423MEMF_Id,
                                            NCAC423MEM_Id = t.NCAC423MEM_Id,
                                            NCAC423MEMF_StatusFlg = t.NCAC423MEMF_StatusFlg,
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
