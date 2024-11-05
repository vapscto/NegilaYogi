using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission.Criteria8;
using PreadmissionDTOs.NAAC.Admission.Criteria8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services.Criteria8
{
    public class NAAC_811MC_NEETImpl : Interface.Criteria8.NAAC_811MC_NEETInterface
    {
        public GeneralContext _GeneralContext;
        public NAAC_811MC_NEETImpl(GeneralContext para)
        {
            _GeneralContext = para;
        }
        public async Task<NAAC_811MC_NEET_DTO> loaddata(NAAC_811MC_NEET_DTO data)
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
                                 select new NAAC_811MC_NEET_DTO
                                 {
                                     ASMAY_Id = a.ASMAY_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();
                data.alldata = (from y in _GeneralContext.Academic
                                from b in _GeneralContext.NAAC_811MC_NEET_DMO
                                where (y.ASMAY_Id == b.NCMC811NEET_Year && b.MI_Id == data.MI_Id)
                                select new NAAC_811MC_NEET_DTO
                                {
                                    NCMC811NEET_Id = b.NCMC811NEET_Id,
                                    NCMC811NEET_Year = b.NCMC811NEET_Year,
                                    ASMAY_Year = y.ASMAY_Year,
                                    MI_Id=data.MI_Id,
                                    NCMC811NEET_Mean = b.NCMC811NEET_Mean,
                                    NCMC811NEET_NoOfStudentsEnrolled = b.NCMC811NEET_NoOfStudentsEnrolled,
                                    NCMC811NEET_Range = b.NCMC811NEET_Range,
                                    NCMC811NEET_StandardDeviation = b.NCMC811NEET_StandardDeviation,
                                    NCMC811NEET_ActiveFlg = b.NCMC811NEET_ActiveFlg,
                                }).Distinct().OrderBy(t => t.NCMC811NEET_Id).ToArray();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_811MC_NEET_DTO savedata(NAAC_811MC_NEET_DTO data)
        {
            try 
            {
                if (data.NCMC811NEET_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_811MC_NEET_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCMC811NEET_Year == data.ASMAY_Id && t.NCMC811NEET_NoOfStudentsEnrolled==data.NCMC811NEET_NoOfStudentsEnrolled && t.NCMC811NEET_Range==data.NCMC811NEET_Range && t.NCMC811NEET_Mean==data.NCMC811NEET_Mean && t.NCMC811NEET_StandardDeviation==data.NCMC811NEET_StandardDeviation).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.count += 1;
                        data.msg = "Duplicate";
                    }
                    else
                    {
                        data.count1 += 1;
                        NAAC_811MC_NEET_DMO obj = new NAAC_811MC_NEET_DMO();
                        obj.MI_Id = data.MI_Id;
                        obj.NCMC811NEET_NoOfStudentsEnrolled = data.NCMC811NEET_NoOfStudentsEnrolled;
                        obj.NCMC811NEET_Range = data.NCMC811NEET_Range;
                        obj.NCMC811NEET_Year = data.ASMAY_Id;
                        obj.NCMC811NEET_Mean = data.NCMC811NEET_Mean;
                        obj.NCMC811NEET_StandardDeviation = data.NCMC811NEET_StandardDeviation;
                        obj.NCMC811NEET_ActiveFlg = true;
                        obj.NCMC811NEET_CreatedBy = data.UserId;
                        obj.NCMC811NEET_UpdatedBy = data.UserId;
                        obj.NCMC811NEET_CreatedDate = DateTime.Now;
                        obj.NCMC811NEET_UpdatedDate = DateTime.Now;
                        _GeneralContext.Add(obj);
                        if (data.filelist.Length > 0)
                        {
                            for (int j = 0; j < data.filelist.Length; j++)
                            {
                                if (data.filelist[0].cfilepath != null)
                                {

                              
                                NAAC_811MC_NEET_Files_DMO obj2 = new NAAC_811MC_NEET_Files_DMO();
                                obj2.MI_Id = data.MI_Id;
                                obj2.NCMC811NEET_Id = obj.NCMC811NEET_Id;
                                obj2.NCMC811NEETF_FileName = data.filelist[j].cfilename;
                                obj2.NCMC811NEETF_FileDesc = data.filelist[j].cfiledesc;
                                obj2.NCMC811NEETF_FilePath = data.filelist[j].cfilepath;
                                obj2.NCMC811NEET_Id = obj.NCMC811NEET_Id;
                                obj2.NCMC811NEETF_ActiveFlg = true;
                                obj2.NCMC811NEETF_CreatedBy = data.UserId;
                                obj2.NCMC811NEETF_UpdatedBy = data.UserId;
                                obj2.NCMC811NEETF_CreatedDate = DateTime.Now;
                                obj2.NCMC811NEETF_UpdatedDate = DateTime.Now;
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
                else if (data.NCMC811NEET_Id > 0)
                {
                    var duplicate = _GeneralContext.NAAC_811MC_NEET_DMO.Where(b => b.MI_Id == data.MI_Id && b.NCMC811NEET_Id != data.NCMC811NEET_Id && b.NCMC811NEET_Year == data.ASMAY_Id&& b.NCMC811NEET_NoOfStudentsEnrolled == data.NCMC811NEET_NoOfStudentsEnrolled && b.NCMC811NEET_Range == data.NCMC811NEET_Range && b.NCMC811NEET_Mean == data.NCMC811NEET_Mean && b.NCMC811NEET_StandardDeviation == data.NCMC811NEET_StandardDeviation).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.count += 1;
                        data.msg = "Duplicate";
                    }
                    else
                    {
                        var update1 = _GeneralContext.NAAC_811MC_NEET_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCMC811NEET_Id == data.NCMC811NEET_Id).SingleOrDefault();
                        update1.NCMC811NEET_Year = data.ASMAY_Id;
                        update1.MI_Id = data.MI_Id;
                        update1.NCMC811NEET_NoOfStudentsEnrolled = data.NCMC811NEET_NoOfStudentsEnrolled;
                        update1.NCMC811NEET_Range = data.NCMC811NEET_Range;
                        update1.NCMC811NEET_Mean = data.NCMC811NEET_Mean;
                        //update1.NCMC811NEET_Year = data.NCMC811NEET_Year;
                        update1.NCMC811NEET_StandardDeviation = data.NCMC811NEET_StandardDeviation;
                        update1.NCMC811NEET_UpdatedBy = data.UserId;
                        update1.NCMC811NEET_UpdatedDate = DateTime.Now;
                        _GeneralContext.Update(update1);

                        var CountRemoveFiles = _GeneralContext.NAAC_811MC_NEET_Files_DMO.Where(t => t.NCMC811NEET_Id == data.NCMC811NEET_Id).ToList();
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

                                if (data.filelist[0].cfilepath!= null)
                                {

                               
                                NAAC_811MC_NEET_Files_DMO obj2 = new NAAC_811MC_NEET_Files_DMO();
                                obj2.MI_Id = data.MI_Id;
                                obj2.NCMC811NEET_Id = update1.NCMC811NEET_Id;
                                obj2.NCMC811NEETF_FileName = data.filelist[k].cfilename;
                                obj2.NCMC811NEETF_FileDesc = data.filelist[k].cfiledesc;
                                obj2.NCMC811NEETF_FilePath = data.filelist[k].cfilepath;
                                obj2.NCMC811NEETF_ActiveFlg = true;
                                obj2.NCMC811NEETF_CreatedBy = data.UserId;
                                obj2.NCMC811NEETF_UpdatedBy = data.UserId;
                                obj2.NCMC811NEETF_CreatedDate = DateTime.Now;
                                obj2.NCMC811NEETF_UpdatedDate = DateTime.Now;
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

                                  
                                    NAAC_811MC_NEET_Files_DMO obj2 = new NAAC_811MC_NEET_Files_DMO();
                                    obj2.NCMC811NEET_Id = update1.NCMC811NEET_Id;
                                    obj2.MI_Id = data.MI_Id;
                                    //obj2.NCMC811NEETF_ActiveFlg = true;
                                    obj2.NCMC811NEETF_FileName = data.filelist[i].cfilename;
                                    obj2.NCMC811NEETF_FileDesc = data.filelist[i].cfiledesc;
                                    obj2.NCMC811NEETF_FilePath = data.filelist[i].cfilepath;
                                    obj2.NCMC811NEETF_CreatedBy = data.UserId;
                                    obj2.NCMC811NEETF_UpdatedBy = data.UserId;
                                    obj2.NCMC811NEETF_CreatedDate = DateTime.Now;
                                    obj2.NCMC811NEETF_UpdatedDate = DateTime.Now;
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
        public NAAC_811MC_NEET_DTO editdata(NAAC_811MC_NEET_DTO data)
        {
            try
            {
                var edit = _GeneralContext.NAAC_811MC_NEET_DMO.Where(t => t.NCMC811NEET_Id == data.NCMC811NEET_Id).ToList();
                data.editlist = edit.ToArray();
                data.yearlist = (from a in _GeneralContext.Academic
                                 where (a.MI_Id == data.MI_Id && a.Is_Active == true)
                                 select new NAAC_811MC_NEET_DTO
                                 {
                                     ASMAY_Id = a.ASMAY_Id,
                                     ASMAY_Year = a.ASMAY_Year,                                     
                                 }).Distinct().ToArray();

                data.editFileslist = (from a in _GeneralContext.NAAC_811MC_NEET_Files_DMO
                                      where (a.NCMC811NEET_Id == data.NCMC811NEET_Id)
                                      select new NAAC_811MC_NEET_DTO
                                      {
                                         
                                          cfilename = a.NCMC811NEETF_FileName,
                                          cfilepath = a.NCMC811NEETF_FilePath,
                                          cfiledesc = a.NCMC811NEETF_FileDesc,                                         
                                      }).Distinct().ToArray();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
        public NAAC_811MC_NEET_DTO deactivY(NAAC_811MC_NEET_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_811MC_NEET_DMO.Where(t => t.NCMC811NEET_Id == data.NCMC811NEET_Id).SingleOrDefault();
                if (result.NCMC811NEET_ActiveFlg == true)
                {
                    result.NCMC811NEET_ActiveFlg = false;
                }
                else if (result.NCMC811NEET_ActiveFlg == false)
                {
                    result.NCMC811NEET_ActiveFlg = true;
                }
                result.NCMC811NEET_UpdatedDate = DateTime.Now;
                result.NCMC811NEET_UpdatedBy = data.UserId;
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
        public NAAC_811MC_NEET_DTO viewuploadflies(NAAC_811MC_NEET_DTO data)
        {
            try
            {
                data.viewuploadflies = (from t in _GeneralContext.NAAC_811MC_NEET_Files_DMO
                                       
                                        where (t.NCMC811NEET_Id == data.NCMC811NEET_Id && t.NCMC811NEETF_ActiveFlg==true)
                                        select new NAAC_811MC_NEET_DTO
                                        {
                                            cfilename = t.NCMC811NEETF_FileName,
                                            cfilepath = t.NCMC811NEETF_FilePath,                                            
                                            cfiledesc = t.NCMC811NEETF_FileDesc,
                                            NCMC811NEETF_Id = t.NCMC811NEETF_Id,
                                            NCMC811NEET_Id = t.NCMC811NEET_Id,
                                            
                                            //NCMC811NEETF_ActiveFlg = true,             
            }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public NAAC_811MC_NEET_DTO deleteuploadfile(NAAC_811MC_NEET_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_811MC_NEET_Files_DMO.Where(t => t.NCMC811NEETF_Id == data.NCMC811NEETF_Id).SingleOrDefault();
                // _GeneralContext.Remove(result);
                result.NCMC811NEETF_ActiveFlg = false;
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
                data.viewuploadflies = (from t in _GeneralContext.NAAC_811MC_NEET_Files_DMO
                                     
                                        where (t.NCMC811NEET_Id == data.NCMC811NEET_Id && t.NCMC811NEETF_ActiveFlg==true)
                                        select new NAAC_811MC_NEET_DTO
                                        {
                                            cfilename = t.NCMC811NEETF_FileName,
                                            cfilepath = t.NCMC811NEETF_FilePath,                                            
                                            cfiledesc = t.NCMC811NEETF_FileDesc,
                                            NCMC811NEETF_Id = t.NCMC811NEETF_Id,
                                            NCMC811NEET_Id = t.NCMC811NEET_Id,
                                            //NCMC811NEETF_ActiveFlg = true,
                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public NAAC_811MC_NEET_DTO getcomment(NAAC_811MC_NEET_DTO data)
        {
            try
            {
                data.commentlist = (from a in _GeneralContext.MC_811_NEET_CommentsDMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCMC811NEETC_RemarksBy == b.Id && a.NCMC811NEET_Id == data.NCMC811NEET_Id)
                                    select new NAAC_811MC_NEET_DTO
                                    {

                                        NCMC811NEETC_Remarks = a.NCMC811NEETC_Remarks,
                                        NCMC811NEETC_Id = a.NCMC811NEETC_Id,
                                        NCMC811NEETC_RemarksBy = a.NCMC811NEETC_RemarksBy,
                                        NCMC811NEETC_StatusFlg = a.NCMC811NEETC_StatusFlg,
                                        NCMC811NEETC_ActiveFlag = a.NCMC811NEETC_ActiveFlag,
                                        NCMC811NEETC_CreatedBy = a.NCMC811NEETC_CreatedBy,
                                        NCMC811NEETC_CreatedDate = a.NCMC811NEETC_CreatedDate,
                                        NCMC811NEETC_UpdatedBy = a.NCMC811NEETC_UpdatedBy,
                                        NCMC811NEETC_UpdatedDate = a.NCMC811NEETC_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().ToArray();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_811MC_NEET_DTO getfilecomment(NAAC_811MC_NEET_DTO data)
        {
            try
            {
                data.commentlist1 = (from a in _GeneralContext.MC_811_NEET_File_CommentsDMO
                                     from b in _GeneralContext.ApplUser
                                     where (a.NCMC811NEETFC_RemarksBy == b.Id && a.NCMC811NEETF_Id == data.NCMC811NEETF_Id)
                                     select new NAAC_811MC_NEET_DTO
                                     {
                                         NCMC811NEETF_Id = a.NCMC811NEETF_Id,
                                         NCMC811NEETFC_Remarks = a.NCMC811NEETFC_Remarks,
                                         NCMC811NEETFC_Id = a.NCMC811NEETFC_Id,
                                         NCMC811NEETFC_RemarksBy = a.NCMC811NEETFC_RemarksBy,
                                         NCMC811NEETFC_StatusFlg = a.NCMC811NEETFC_StatusFlg,
                                         NCMC811NEETFC_ActiveFlag = a.NCMC811NEETFC_ActiveFlag,
                                         NCMC811NEETFC_CreatedBy = a.NCMC811NEETFC_CreatedBy,
                                         NCMC811NEETFC_CreatedDate = a.NCMC811NEETFC_CreatedDate,
                                         NCMC811NEETFC_UpdatedBy = a.NCMC811NEETFC_UpdatedBy,
                                         NCMC811NEETFC_UpdatedDate = a.NCMC811NEETFC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().ToArray();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_811MC_NEET_DTO savecomments(NAAC_811MC_NEET_DTO data)
        {
            try
            {
                MC_811_NEET_CommentsDMO obj1 = new MC_811_NEET_CommentsDMO();
                obj1.NCMC811NEETC_Remarks = data.Remarks;
                obj1.NCMC811NEETC_RemarksBy = data.UserId;
                obj1.NCMC811NEETC_StatusFlg = "";
                obj1.NCMC811NEET_Id = data.filefkid;
                obj1.NCMC811NEETC_ActiveFlag = true;
                obj1.NCMC811NEETC_CreatedBy = data.UserId;
                obj1.NCMC811NEETC_UpdatedBy = data.UserId;
                obj1.NCMC811NEETC_CreatedDate = DateTime.Now;
                obj1.NCMC811NEETC_UpdatedDate = DateTime.Now;
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
        public NAAC_811MC_NEET_DTO savefilewisecomments(NAAC_811MC_NEET_DTO data)
        {
            try
            {
                MC_811_NEET_File_CommentsDMO obj1 = new MC_811_NEET_File_CommentsDMO();
                obj1.NCMC811NEETFC_Remarks = data.Remarks;
                obj1.NCMC811NEETFC_RemarksBy = data.UserId;
                obj1.NCMC811NEETFC_StatusFlg = "";
                obj1.NCMC811NEETF_Id = data.filefkid;
                obj1.NCMC811NEETFC_ActiveFlag = true;
                obj1.NCMC811NEETFC_CreatedBy = data.UserId;
                obj1.NCMC811NEETFC_UpdatedBy = data.UserId;
                obj1.NCMC811NEETFC_CreatedDate = DateTime.Now;
                obj1.NCMC811NEETFC_UpdatedDate = DateTime.Now;
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
