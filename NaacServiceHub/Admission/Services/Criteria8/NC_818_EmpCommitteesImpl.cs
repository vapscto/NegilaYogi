using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission.Criteria8;
using PreadmissionDTOs.NAAC.Admission.Criteria8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services.Criteria8
{
    public class NC_818_EmpCommitteesImpl :Interface.Criteria8.NC_818_EmpCommitteesInterface
    {
        public GeneralContext _GeneralContext;
    public NC_818_EmpCommitteesImpl(GeneralContext para)
    {
        _GeneralContext = para;
    }
    public async Task<NC_818_EmpCommitteesDTO> loaddata(NC_818_EmpCommitteesDTO data)
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
                             select new NC_818_EmpCommitteesDTO
                             {
                                 ASMAY_Id = a.ASMAY_Id,
                                 ASMAY_Year = a.ASMAY_Year,
                             }).Distinct().ToArray();
            data.alldata = (from y in _GeneralContext.Academic
                            from b in _GeneralContext.NC_818_EmpCommitteesDMO
                            where (y.ASMAY_Id == b.NCDC8111EC_Year && b.MI_Id == data.MI_Id)
                            select new NC_818_EmpCommitteesDTO
                            {
                                NCNC8111EC_Id = b.NCNC8111EC_Id,
                                NCDC8111EC_Year = b.NCDC8111EC_Year,
                                ASMAY_Year = y.ASMAY_Year,
                                MI_Id = data.MI_Id,
                                NCDC8111EC_FacultyMemberName = b.NCDC8111EC_FacultyMemberName,
                                NCDC8111EC_CommitteesName = b.NCDC8111EC_CommitteesName,
                                NCDC8111EC_TenureOfService = b.NCDC8111EC_TenureOfService,
                                NCDC8111EC_ActiveFlag = b.NCDC8111EC_ActiveFlag,
                            }).Distinct().OrderBy(t => t.NCNC8111EC_Id).ToArray();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return data;
    }
    public NC_818_EmpCommitteesDTO savedata(NC_818_EmpCommitteesDTO data)
    {
        try
        {
            if (data.NCNC8111EC_Id == 0)
            {
                var duplicate = _GeneralContext.NC_818_EmpCommitteesDMO.Where(t => t.MI_Id == data.MI_Id && t.NCDC8111EC_Year == data.ASMAY_Id && t.NCDC8111EC_FacultyMemberName == data.NCDC8111EC_FacultyMemberName && t.NCDC8111EC_CommitteesName == data.NCDC8111EC_CommitteesName && t.NCDC8111EC_TenureOfService==data.NCDC8111EC_TenureOfService).ToList();
                if (duplicate.Count > 0)
                {
                    data.count += 1;
                    data.msg = "Duplicate";
                }
                else
                {
                    data.count1 += 1;
                        NC_818_EmpCommitteesDMO obj = new NC_818_EmpCommitteesDMO();
                    obj.MI_Id = data.MI_Id;
                    obj.NCDC8111EC_FacultyMemberName = data.NCDC8111EC_FacultyMemberName;
                    obj.NCDC8111EC_CommitteesName = data.NCDC8111EC_CommitteesName;
                    obj.NCDC8111EC_Year = data.ASMAY_Id;
                    obj.NCDC8111EC_TenureOfService = data.NCDC8111EC_TenureOfService;
                    obj.NCDC8111EC_ActiveFlag = true;
                    obj.NCDC8111EC_CreatedBy = data.UserId;
                    obj.NCDC8111EC_UpdatedBy = data.UserId;
                    obj.NCDC8111EC_CreatedDate = DateTime.Now;
                    obj.NCDC8111EC_UpdatedDate = DateTime.Now;
                    _GeneralContext.Add(obj);
                    if (data.filelist.Length > 0)
                    {
                        for (int j = 0; j < data.filelist.Length; j++)
                        {
                            if (data.filelist[0].cfilepath != null)
                            {


                                    NC_818_EmpCommitteesFilesDMO obj2 = new NC_818_EmpCommitteesFilesDMO();
                                //obj2.MI_Id = data.MI_Id;
                                obj2.NCNC8111EC_Id = obj.NCNC8111EC_Id;
                                obj2.NCNC8111ECF_FileName = data.filelist[j].cfilename;
                                obj2.NCNC8111ECF_FileDesc = data.filelist[j].cfiledesc;
                                obj2.NCNC8111ECF_FilePath = data.filelist[j].cfilepath;
                                obj2.NCNC8111EC_Id = obj.NCNC8111EC_Id;
                                obj2.NCNC8111ECF_ActiveFlg = true;
                                obj2.NCNC8111ECF_CreatedBy = data.UserId;
                                obj2.NCNC8111ECF_UpdatedBy = data.UserId;
                                obj2.NCNC8111ECF_CreatedDate = DateTime.Now;
                                obj2.NCNC8111ECF_UpdatedDate = DateTime.Now;
                                _GeneralContext.Add(obj2);
                            }
                        }
                    }
                    int s = _GeneralContext.SaveChanges();
                    if (s > 0)
                    {
                        data.msg = "saved";
                        data.returnval = true;
                    }
                    else
                    {
                        data.msg = "notsaved";
                        data.returnval = false;
                    }
                }
            }
            else if (data.NCNC8111EC_Id > 0)
            {
                var duplicate = _GeneralContext.NC_818_EmpCommitteesDMO.Where(b => b.MI_Id == data.MI_Id && b.NCNC8111EC_Id != data.NCNC8111EC_Id && b.NCDC8111EC_Year == data.ASMAY_Id && b.NCDC8111EC_FacultyMemberName == data.NCDC8111EC_FacultyMemberName && b.NCDC8111EC_CommitteesName == data.NCDC8111EC_CommitteesName && b.NCDC8111EC_TenureOfService == data.NCDC8111EC_TenureOfService).ToList();
                if (duplicate.Count > 0)
                {
                    data.count += 1;
                    data.msg = "Duplicate";
                }
                else
                {
                    var update1 = _GeneralContext.NC_818_EmpCommitteesDMO.Where(t => t.MI_Id == data.MI_Id && t.NCNC8111EC_Id == data.NCNC8111EC_Id).SingleOrDefault();
                    update1.NCDC8111EC_Year = data.ASMAY_Id;
                    update1.MI_Id = data.MI_Id;
                    update1.NCDC8111EC_FacultyMemberName = data.NCDC8111EC_FacultyMemberName;
                    update1.NCDC8111EC_CommitteesName = data.NCDC8111EC_CommitteesName;
                    update1.NCDC8111EC_TenureOfService = data.NCDC8111EC_TenureOfService;

                    update1.NCDC8111EC_UpdatedBy = data.UserId;
                    update1.NCDC8111EC_UpdatedDate = DateTime.Now;
                    _GeneralContext.Update(update1);

                    var CountRemoveFiles = _GeneralContext.NC_818_EmpCommitteesFilesDMO.Where(t => t.NCNC8111EC_Id == data.NCNC8111EC_Id).ToList();
                    if (CountRemoveFiles.Count > 0)
                    {
                        foreach (var RemoveFiles in CountRemoveFiles)
                        {
                            _GeneralContext.Remove(RemoveFiles);
                        }
                    }
                    if (data.filelist.Length > 0)
                    {
                        for (int k = 0; k < data.filelist.Length; k++)
                        {

                            if (data.filelist[0].cfilepath != null)
                            {


                                    NC_818_EmpCommitteesFilesDMO obj2 = new NC_818_EmpCommitteesFilesDMO();
                                // obj2.MI_Id = data.MI_Id;
                                obj2.NCNC8111EC_Id = update1.NCNC8111EC_Id;
                                obj2.NCNC8111ECF_FileName = data.filelist[k].cfilename;
                                obj2.NCNC8111ECF_FileDesc = data.filelist[k].cfiledesc;
                                obj2.NCNC8111ECF_FilePath = data.filelist[k].cfilepath;
                                obj2.NCNC8111ECF_ActiveFlg = true;
                                obj2.NCNC8111ECF_CreatedBy = data.UserId;
                                obj2.NCNC8111ECF_UpdatedBy = data.UserId;
                                obj2.NCNC8111ECF_CreatedDate = DateTime.Now;
                                obj2.NCNC8111ECF_UpdatedDate = DateTime.Now;
                                _GeneralContext.Add(obj2);
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


                                    NC_818_EmpCommitteesFilesDMO obj2 = new NC_818_EmpCommitteesFilesDMO();
                                    obj2.NCNC8111EC_Id = update1.NCNC8111EC_Id;
                                    // obj2.MI_Id = data.MI_Id;
                                    //obj2.NCMC811NEETF_ActiveFlg = true;
                                    obj2.NCNC8111ECF_FileName = data.filelist[i].cfilename;
                                    obj2.NCNC8111ECF_FileDesc = data.filelist[i].cfiledesc;
                                    obj2.NCNC8111ECF_FilePath = data.filelist[i].cfilepath;

                                    obj2.NCNC8111ECF_CreatedBy = data.UserId;
                                    obj2.NCNC8111ECF_UpdatedBy = data.UserId;
                                    obj2.NCNC8111ECF_CreatedDate = DateTime.Now;
                                    obj2.NCNC8111ECF_UpdatedDate = DateTime.Now;
                                    _GeneralContext.Add(obj2);
                                }
                            }
                        }
                    }
                    int s = _GeneralContext.SaveChanges();
                    if (s > 0)
                    {
                        data.msg = "updated";
                        data.returnval = true;
                    }
                    else
                    {
                        data.msg = "notupdated";
                        data.returnval = false;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            Console.WriteLine(Ex.Message);
        }
        return data;
    }
    public NC_818_EmpCommitteesDTO editdata(NC_818_EmpCommitteesDTO data)
    {
        try
        {
            var edit = _GeneralContext.NC_818_EmpCommitteesDMO.Where(t => t.NCNC8111EC_Id == data.NCNC8111EC_Id).ToList();
            data.editlist = edit.ToArray();
            data.yearlist = (from a in _GeneralContext.Academic
                             where (a.MI_Id == data.MI_Id && a.Is_Active == true)
                             select new NC_818_EmpCommitteesDTO
                             {
                                 ASMAY_Id = a.ASMAY_Id,
                                 ASMAY_Year = a.ASMAY_Year,
                             }).Distinct().ToArray();

            data.editFileslist = (from a in _GeneralContext.NC_818_EmpCommitteesFilesDMO
                                  where (a.NCNC8111EC_Id == data.NCNC8111EC_Id)
                                  select new NC_818_EmpCommitteesDTO
                                  {

                                      cfilename = a.NCNC8111ECF_FileName,
                                      cfilepath = a.NCNC8111ECF_FilePath,
                                      cfiledesc = a.NCNC8111ECF_FileDesc,
                                  }).Distinct().ToArray();
        }
        catch (Exception Ex)
        {
            Console.WriteLine(Ex.Message);
        }
        return data;
    }
    public NC_818_EmpCommitteesDTO deactivY(NC_818_EmpCommitteesDTO data)
    {
        try
        {
            var result = _GeneralContext.NC_818_EmpCommitteesDMO.Where(t => t.NCNC8111EC_Id == data.NCNC8111EC_Id).SingleOrDefault();
            if (result.NCDC8111EC_ActiveFlag == true)
            {
                result.NCDC8111EC_ActiveFlag = false;
            }
            else if (result.NCDC8111EC_ActiveFlag == false)
            {
                result.NCDC8111EC_ActiveFlag = true;
            }
            result.NCDC8111EC_UpdatedDate = DateTime.Now;
            result.NCDC8111EC_UpdatedBy = data.UserId;
            result.MI_Id = data.MI_Id;
            _GeneralContext.Update(result);
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
        catch (Exception Ex)
        {
            Console.WriteLine(Ex.Message);
        }
        return data;
    }
    public NC_818_EmpCommitteesDTO viewuploadflies(NC_818_EmpCommitteesDTO data)
    {
        try
        {
            data.viewuploadflies = (from t in _GeneralContext.NC_818_EmpCommitteesFilesDMO

                                    where (t.NCNC8111EC_Id == data.NCNC8111EC_Id && t.NCNC8111ECF_ActiveFlg==true)
                                    select new NC_818_EmpCommitteesDTO
                                    {
                                        cfilename = t.NCNC8111ECF_FileName,
                                        cfilepath = t.NCNC8111ECF_FilePath,
                                        cfiledesc = t.NCNC8111ECF_FileDesc,
                                        NCNC8111ECF_Id = t.NCNC8111ECF_Id,
                                        NCNC8111EC_Id = t.NCNC8111EC_Id,

                                        //NCMC811NEETF_ActiveFlg = true,             
                                    }).Distinct().ToArray();
        }
        catch (Exception f)
        {
            Console.WriteLine(f.Message);
        }
        return data;
    }
    public NC_818_EmpCommitteesDTO deleteuploadfile(NC_818_EmpCommitteesDTO data)
    {
        try
        {
            var result = _GeneralContext.NC_818_EmpCommitteesFilesDMO.Where(t => t.NCNC8111ECF_Id == data.NCNC8111ECF_Id).SingleOrDefault();
                // _GeneralContext.Remove(result);
                result.NCNC8111ECF_ActiveFlg = false;
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
            data.viewuploadflies = (from t in _GeneralContext.NC_818_EmpCommitteesFilesDMO

                                    where (t.NCNC8111EC_Id == data.NCNC8111EC_Id && t.NCNC8111ECF_ActiveFlg==true)
                                    select new NC_818_EmpCommitteesDTO
                                    {
                                        cfilename = t.NCNC8111ECF_FileName,
                                        cfilepath = t.NCNC8111ECF_FilePath,
                                        cfiledesc = t.NCNC8111ECF_FileDesc,
                                        NCNC8111ECF_Id = t.NCNC8111ECF_Id,
                                        NCNC8111EC_Id = t.NCNC8111EC_Id,
                                        //NCMC811NEETF_ActiveFlg = true,
                                    }).Distinct().ToArray();
        }
        catch (Exception f)
        {
            Console.WriteLine(f.Message);
        }
        return data;
    }
    public NC_818_EmpCommitteesDTO getcomment(NC_818_EmpCommitteesDTO data)
        {
            try
            {
                data.commentlist = (from a in _GeneralContext.NAAC_NC_818_EmpCommittees_CommentsDMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCNC8111ECC_RemarksBy == b.Id && a.NCNC8111EC_Id == data.NCNC8111EC_Id)
                                    select new NC_818_EmpCommitteesDTO
                                    {

                                        NCNC8111ECC_Remarks = a.NCNC8111ECC_Remarks,
                                        NCNC8111EC_Id = a.NCNC8111EC_Id,
                                        NCNC8111ECC_RemarksBy = a.NCNC8111ECC_RemarksBy,
                                        NCNC8111ECC_StatusFlg = a.NCNC8111ECC_StatusFlg,
                                        NCNC8111ECC_ActiveFlag = a.NCNC8111ECC_ActiveFlag,
                                        NCNC8111ECC_CreatedBy = a.NCNC8111ECC_CreatedBy,
                                        NCNC8111ECC_CreatedDate = a.NCNC8111ECC_CreatedDate,
                                        NCNC8111ECC_UpdatedBy = a.NCNC8111ECC_UpdatedBy,
                                        NCNC8111ECC_UpdatedDate = a.NCNC8111ECC_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().ToArray();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NC_818_EmpCommitteesDTO getfilecomment(NC_818_EmpCommitteesDTO data)
        {
            try
            {
                data.commentlist1 = (from a in _GeneralContext.NAAC_NC_818_EmpCommittees_File_CommentsDMO
                                     from b in _GeneralContext.ApplUser
                                     where (a.NCNC8111ECFC_RemarksBy == b.Id && a.NCNC8111ECF_Id == data.NCNC8111ECF_Id)
                                     select new NC_818_EmpCommitteesDTO
                                     {
                                         NCNC8111ECF_Id = a.NCNC8111ECF_Id,
                                         NCNC8111ECFC_Remarks = a.NCNC8111ECFC_Remarks,
                                         NCNC8111ECFC_Id = a.NCNC8111ECFC_Id,
                                         NCNC8111ECFC_RemarksBy = a.NCNC8111ECFC_RemarksBy,
                                         NCNC8111ECFC_StatusFlg = a.NCNC8111ECFC_StatusFlg,
                                         NCNC8111ECFC_ActiveFlag = a.NCNC8111ECFC_ActiveFlag,
                                         NCNC8111ECFC_CreatedBy = a.NCNC8111ECFC_CreatedBy,
                                         NCNC8111ECFC_CreatedDate = a.NCNC8111ECFC_CreatedDate,
                                         NCNC8111ECFC_UpdatedBy = a.NCNC8111ECFC_UpdatedBy,
                                         NCNC8111ECFC_UpdatedDate = a.NCNC8111ECFC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().ToArray();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NC_818_EmpCommitteesDTO savecomments(NC_818_EmpCommitteesDTO data)
        {
            try
            {
                NAAC_NC_818_EmpCommittees_CommentsDMO obj1 = new NAAC_NC_818_EmpCommittees_CommentsDMO();
                obj1.NCNC8111ECC_Remarks = data.Remarks;
                obj1.NCNC8111ECC_RemarksBy = data.UserId;
                obj1.NCNC8111ECC_StatusFlg = "";
                obj1.NCNC8111EC_Id = data.filefkid;
                obj1.NCNC8111ECC_ActiveFlag = true;
                obj1.NCNC8111ECC_CreatedBy = data.UserId;
                obj1.NCNC8111ECC_UpdatedBy = data.UserId;
                obj1.NCNC8111ECC_CreatedDate = DateTime.Now;
                obj1.NCNC8111ECC_UpdatedDate = DateTime.Now;
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
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NC_818_EmpCommitteesDTO savefilewisecomments(NC_818_EmpCommitteesDTO data)
        {
            try
            {
                NAAC_NC_818_EmpCommittees_File_CommentsDMO obj1 = new NAAC_NC_818_EmpCommittees_File_CommentsDMO();
                obj1.NCNC8111ECFC_Remarks = data.Remarks;
                obj1.NCNC8111ECFC_RemarksBy = data.UserId;
                obj1.NCNC8111ECFC_StatusFlg = "";
                obj1.NCNC8111ECF_Id = data.filefkid;
                obj1.NCNC8111ECFC_ActiveFlag = true;
                obj1.NCNC8111ECFC_CreatedBy = data.UserId;
                obj1.NCNC8111ECFC_UpdatedBy = data.UserId;
                obj1.NCNC8111ECFC_CreatedDate = DateTime.Now;
                obj1.NCNC8111ECFC_UpdatedDate = DateTime.Now;
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
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
    }
}
