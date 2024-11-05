using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Medical;
using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Medical.Service
{
    public class NAAC_MC_EmpTrainedDevelopment244Impl:Interface.NAAC_MC_EmpTrainedDevelopment244Interface
    {


        public GeneralContext _GeneralContext;
        public NAAC_MC_EmpTrainedDevelopment244Impl(GeneralContext w)
        {
            _GeneralContext = w;
        }
        public NAAC_MC_EmpTrainedDevelopment244_DTO loaddata(NAAC_MC_EmpTrainedDevelopment244_DTO data)
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

                data.allacademicyear = _GeneralContext.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Year).ToArray();
                data.alldata1 = (from a in _GeneralContext.Academic
                                 from b in _GeneralContext.NAAC_MC_EmpTrainedDevelopment244_DMO
                                 where (a.MI_Id == b.MI_Id && a.Is_Active == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == b.NCMCETD244_Year)
                                 select new NAAC_MC_EmpTrainedDevelopment244_DTO
                                 {
                                     NCMCETD244_Id = b.NCMCETD244_Id,
                                     NCMCETD244_NoOfTechersTrainedForDevOfEcontents = b.NCMCETD244_NoOfTechersTrainedForDevOfEcontents,
                                     NCMCETD244_TotalNoOfTeachers = b.NCMCETD244_TotalNoOfTeachers,
                                   
                                     NCMCETD244_Year = b.NCMCETD244_Year,
                                     ASMAY_Year = a.ASMAY_Year,
                                     NCMCETD244_ActiveFlag = b.NCMCETD244_ActiveFlag,
                                     MI_Id = b.MI_Id,
                                     NCMCETD244_StatusFlg = b.NCMCETD244_StatusFlg,
                                     NCMCETD244_ApprovedFlg = b.NCMCETD244_ApprovedFlg,

                                 }).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_MC_EmpTrainedDevelopment244_DTO save(NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            try
            {
                if (data.NCMCETD244_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_MC_EmpTrainedDevelopment244_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCMCETD244_NoOfTechersTrainedForDevOfEcontents == data.NCMCETD244_NoOfTechersTrainedForDevOfEcontents && t.NCMCETD244_TotalNoOfTeachers == data.NCMCETD244_TotalNoOfTeachers  && t.NCMCETD244_Id != 0).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        NAAC_MC_EmpTrainedDevelopment244_DMO u = new NAAC_MC_EmpTrainedDevelopment244_DMO();
                        u.NCMCETD244_Id = data.NCMCETD244_Id;
                        u.MI_Id = data.MI_Id;
                        u.NCMCETD244_NoOfTechersTrainedForDevOfEcontents = data.NCMCETD244_NoOfTechersTrainedForDevOfEcontents;
                        u.NCMCETD244_TotalNoOfTeachers = data.NCMCETD244_TotalNoOfTeachers;                      
                        u.NCMCETD244_CreatedBy = data.UserId;
                        u.NCMCETD244_UpdatedBy = data.UserId;
                        u.NCMCETD244_CreatedDate = DateTime.Now;
                        u.NCMCETD244_UpdatedDate = DateTime.Now;
                        u.NCMCETD244_Year = data.ASMAY_Id;
                        u.NCMCETD244_ActiveFlag = true;
                        u.NCMCETD244_StatusFlg = "";

                        _GeneralContext.Add(u);

                        if (data.filelist.Length > 0)
                        {
                            for (int i = 0; i < data.filelist.Length; i++)
                            {
                                if (data.filelist[i].cfilepath != null)
                                {
                                    NAAC_MC_EmpTrainedDevelopment244_files_DMO obj2 = new NAAC_MC_EmpTrainedDevelopment244_files_DMO();

                                    obj2.NCMCETD244_Id =u.NCMCETD244_Id;
                                    obj2.NCMCETD244F_FileName = data.filelist[i].cfilename;
                                    obj2.NCMCETD244F_FileDesc = data.filelist[i].cfiledesc;
                                    obj2.NCMCETD244F_FilePath = data.filelist[i].cfilepath;
                                    obj2.NCMCETD244F_ActiveFlg = true;
                                    obj2.NCMCETD244F_CreatedBy = data.UserId;
                                    obj2.NCMCETD244F_UpdatedBy = data.UserId;
                                    obj2.NCMCETD244F_CreatedDate = DateTime.Now;
                                    obj2.NCMCETD244F_UpdatedDate = DateTime.Now;
                                    obj2.NCMCETD244F_StatusFlg = "";

                                    _GeneralContext.Add(obj2);
                                }
                            }
                        }
                        var w = _GeneralContext.SaveChanges();
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

                else if (data.NCMCETD244_Id > 0)
                {
                    var duplicate = _GeneralContext.NAAC_MC_EmpTrainedDevelopment244_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCMCETD244_Id != data.NCMCETD244_Id && t.NCMCETD244_NoOfTechersTrainedForDevOfEcontents == data.NCMCETD244_NoOfTechersTrainedForDevOfEcontents && t.NCMCETD244_TotalNoOfTeachers == data.NCMCETD244_TotalNoOfTeachers && t.NCMCETD244_Year == data.NCMCETD244_Year).Distinct().ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        var j = _GeneralContext.NAAC_MC_EmpTrainedDevelopment244_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCMCETD244_Id == data.NCMCETD244_Id).SingleOrDefault();
                        j.NCMCETD244_NoOfTechersTrainedForDevOfEcontents = data.NCMCETD244_NoOfTechersTrainedForDevOfEcontents;
                        j.NCMCETD244_TotalNoOfTeachers = data.NCMCETD244_TotalNoOfTeachers;
                      
                        j.NCMCETD244_Year = data.ASMAY_Id;
                        j.MI_Id = data.MI_Id;
                        j.NCMCETD244_UpdatedDate = DateTime.Now;
                        j.NCMCETD244_UpdatedBy = data.UserId;

                        _GeneralContext.Update(j);

                     

                        var CountRemoveFiles = _GeneralContext.NAAC_MC_EmpTrainedDevelopment244_files_DMO.Where(t => t.NCMCETD244_Id == data.NCMCETD244_Id).ToList();

                        List<long> temparr = new List<long>();
                        //getting all mobilenumbers
                        foreach (var c in data.filelist)
                        {
                            temparr.Add(c.cfileid);
                        }


                        var Phone_Noresultremove = _GeneralContext.NAAC_MC_EmpTrainedDevelopment244_files_DMO.Where(t => !temparr.Contains(t.NCMCETD244F_Id)
                        && t.NCMCETD244_Id == data.NCMCETD244_Id).ToList();

                        foreach (var ph1 in Phone_Noresultremove)
                        {
                            var resultremove112 = _GeneralContext.NAAC_MC_EmpTrainedDevelopment244_files_DMO.Single(a => a.NCMCETD244F_Id == ph1.NCMCETD244F_Id);
                            resultremove112.NCMCETD244F_ActiveFlg = false;
                            _GeneralContext.Update(resultremove112);

                        }

                        //if (CountRemoveFiles.Count > 0)
                        //{
                        // foreach (var RemoveFiles in CountRemoveFiles)
                        // {
                        // _GeneralContext.Remove(RemoveFiles);
                        // }
                        //}

                        if (data.filelist.Length > 0)
                        {
                            for (int k = 0; k < data.filelist.Length; k++)
                            {
                                var resultupload = _GeneralContext.NAAC_MC_EmpTrainedDevelopment244_files_DMO.Where(a => a.NCMCETD244_Id == data.NCMCETD244_Id
                                && a.NCMCETD244F_Id == data.filelist[k].cfileid).ToList();
                                if (resultupload.Count > 0)
                                {
                                    var resultupdateupload = _GeneralContext.NAAC_MC_EmpTrainedDevelopment244_files_DMO.Single(a => a.NCMCETD244_Id == data.NCMCETD244_Id && a.NCMCETD244F_Id == data.filelist[k].cfileid);
                                    resultupdateupload.NCMCETD244F_FileDesc = data.filelist[k].cfiledesc;
                                    resultupdateupload.NCMCETD244F_FileName = data.filelist[k].cfilename;
                                    resultupdateupload.NCMCETD244F_FilePath = data.filelist[k].cfilepath;
                                    _GeneralContext.Update(resultupdateupload);
                                }
                                else
                                {
                                    if (data.filelist[k].cfilepath != null && data.filelist[k].cfilepath != "")
                                    {
                                        NAAC_MC_EmpTrainedDevelopment244_files_DMO obj2 = new NAAC_MC_EmpTrainedDevelopment244_files_DMO();
                                        obj2.NCMCETD244F_FileDesc = data.filelist[k].cfiledesc;
                                        obj2.NCMCETD244F_FileName = data.filelist[k].cfilename;
                                        obj2.NCMCETD244F_FilePath = data.filelist[k].cfilepath;
                                        obj2.NCMCETD244_Id = data.NCMCETD244_Id;
                                        obj2.NCMCETD244F_CreatedDate = DateTime.Now;
                                        obj2.NCMCETD244F_UpdatedDate = DateTime.Now;
                                        obj2.NCMCETD244F_CreatedBy = data.UserId;
                                        obj2.NCMCETD244F_UpdatedBy = data.UserId;
                                        obj2.NCMCETD244F_ActiveFlg = true;
                                        obj2.NCMCETD244F_StatusFlg = "";
                                        _GeneralContext.Add(obj2);
                                    }
                                }
                            }
                        }

                        var r = _GeneralContext.SaveChanges();
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
        public NAAC_MC_EmpTrainedDevelopment244_DTO deactive(NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            try
            {
                var g = _GeneralContext.NAAC_MC_EmpTrainedDevelopment244_DMO.Where(t => t.NCMCETD244_Id == data.NCMCETD244_Id).SingleOrDefault();
                if (g.NCMCETD244_ActiveFlag == true)
                {
                    g.NCMCETD244_ActiveFlag = false;
                }
                else
                {
                    g.NCMCETD244_ActiveFlag = true;
                }
                g.NCMCETD244_UpdatedDate = DateTime.Now;
                g.NCMCETD244_UpdatedBy = data.UserId;
                g.MI_Id = data.MI_Id;
                _GeneralContext.Update(g);
                int s = _GeneralContext.SaveChanges();
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
        public NAAC_MC_EmpTrainedDevelopment244_DTO EditData(NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            try
            {

                data.editlist = (from a in _GeneralContext.Academic
                                 from b in _GeneralContext.NAAC_MC_EmpTrainedDevelopment244_DMO
                                 where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && b.NCMCETD244_Id == data.NCMCETD244_Id && a.ASMAY_Id == b.NCMCETD244_Year)
                                 select new NAAC_MC_EmpTrainedDevelopment244_DTO
                                 {
                                     NCMCETD244_Id = b.NCMCETD244_Id,
                                     NCMCETD244_NoOfTechersTrainedForDevOfEcontents = b.NCMCETD244_NoOfTechersTrainedForDevOfEcontents,
                                     NCMCETD244_TotalNoOfTeachers = b.NCMCETD244_TotalNoOfTeachers,
                                 
                                     NCMCETD244_Year = b.NCMCETD244_Year,
                                     MI_Id = b.MI_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();

                data.editFileslist = (from a in _GeneralContext.NAAC_MC_EmpTrainedDevelopment244_files_DMO
                                      where (a.NCMCETD244_Id == data.NCMCETD244_Id && a.NCMCETD244F_ActiveFlg==true)
                                      select new NAAC_MC_EmpTrainedDevelopment244_DTO
                                      {
                                          cfilename = a.NCMCETD244F_FileName,
                                          cfilepath = a.NCMCETD244F_FilePath,
                                          cfiledesc = a.NCMCETD244F_FileDesc,
                                          cfileid = a.NCMCETD244F_Id,
                                          cfileactive = a.NCMCETD244F_ActiveFlg
                                      }).Distinct().ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_MC_EmpTrainedDevelopment244_DTO viewuploadflies(NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            try
            {
                data.viewuploadflies = (from a in _GeneralContext.NAAC_MC_EmpTrainedDevelopment244_files_DMO
                                        from b in _GeneralContext.NAAC_MC_EmpTrainedDevelopment244_DMO
                                        where (a.NCMCETD244_Id == data.NCMCETD244_Id && b.NCMCETD244_Id==a.NCMCETD244_Id && a.NCMCETD244F_ActiveFlg==true)
                                        select new NAAC_MC_EmpTrainedDevelopment244_DTO
                                        {
                                            cfilename = a.NCMCETD244F_FileName,
                                            cfilepath = a.NCMCETD244F_FilePath,
                                            cfiledesc = a.NCMCETD244F_FileDesc,
                                            NCMCETD244F_Id = a.NCMCETD244F_Id,
                                            NCMCETD244_Id = a.NCMCETD244_Id,
                                            NCMCETD244F_StatusFlg = a.NCMCETD244F_StatusFlg,
                                            NCMCETD244F_ApprovedFlg = a.NCMCETD244F_ApprovedFlg,
                                            MI_Id = b.MI_Id,
                                        }).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;

        }
        public NAAC_MC_EmpTrainedDevelopment244_DTO deleteuploadfile(NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            try
            {
                var res = _GeneralContext.NAAC_MC_EmpTrainedDevelopment244_files_DMO.Where(t => t.NCMCETD244F_Id == data.NCMCETD244F_Id).SingleOrDefault();
                _GeneralContext.Remove(res);
                int s = _GeneralContext.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
                data.viewuploadflies = (from a in _GeneralContext.NAAC_MC_EmpTrainedDevelopment244_files_DMO
                                        from b in _GeneralContext.NAAC_MC_EmpTrainedDevelopment244_DMO
                                        where (a.NCMCETD244_Id == data.NCMCETD244_Id && b.NCMCETD244_Id==a.NCMCETD244_Id && a.NCMCETD244F_ActiveFlg==true)
                                        select new NAAC_MC_EmpTrainedDevelopment244_DTO
                                        {
                                            cfilename = a.NCMCETD244F_FileName,
                                            cfilepath = a.NCMCETD244F_FilePath,
                                            cfiledesc = a.NCMCETD244F_FileDesc,
                                            NCMCETD244F_Id = a.NCMCETD244F_Id,
                                            NCMCETD244_Id = a.NCMCETD244_Id,
                                            NCMCETD244F_StatusFlg = a.NCMCETD244F_StatusFlg,
                                            NCMCETD244F_ApprovedFlg = a.NCMCETD244F_ApprovedFlg,
                                            MI_Id = b.MI_Id,
                                        }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }



        //add row wise comments
        public NAAC_MC_EmpTrainedDevelopment244_DTO savemedicaldatawisecomments(NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            try
            {
                NAAC_MC_EmpTrainedDevelopment_244_Comments_DMO obj1 = new NAAC_MC_EmpTrainedDevelopment_244_Comments_DMO();

                obj1.NCMCETD244C_Remarks = data.Remarks;
                obj1.NCMCETD244C_RemarksBy = data.UserId;
                obj1.NCMCETD244C_StatusFlg = "";
                obj1.NCMCETD244_Id = data.filefkid;
                obj1.NCMCETD244C_ActiveFlag = true;
                obj1.NCMCETD244C_CreatedBy = data.UserId;
                obj1.NCMCETD244C_UpdatedBy = data.UserId;
                obj1.NCMCETD244C_CreatedDate = DateTime.Now;
                obj1.NCMCETD244C_UpdatedDate = DateTime.Now;
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

        //add file wise comments
        public NAAC_MC_EmpTrainedDevelopment244_DTO savefilewisecomments(NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            try
            {
                NAAC_MC_EmpTrainedDevelopment_244_File_Comments_DMO obj1 = new NAAC_MC_EmpTrainedDevelopment_244_File_Comments_DMO();

                obj1.NCMCETD244FC_Remarks = data.Remarks;
                obj1.NCMCETD244FC_RemarksBy = data.UserId;
                obj1.NCMCETD244FC_StatusFlg = "";
                obj1.NCMCETD244F_Id = data.filefkid;
                obj1.NCMCETD244FC_ActiveFlag = true;
                obj1.NCMCETD244FC_CreatedBy = data.UserId;
                obj1.NCMCETD244FC_UpdatedBy = data.UserId;
                obj1.NCMCETD244FC_CreatedDate = DateTime.Now;
                obj1.NCMCETD244FC_UpdatedDate = DateTime.Now;

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

        // view row wise comments
        public NAAC_MC_EmpTrainedDevelopment244_DTO getcomment(NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            try
            {
                data.commentlist = (from a in _GeneralContext.NAAC_MC_EmpTrainedDevelopment_244_Comments_DMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCMCETD244C_RemarksBy == b.Id && a.NCMCETD244_Id == data.NCMCETD244_Id)
                                    select new NAAC_MC_EmpTrainedDevelopment244_DTO
                                    {
                                        NCMCETD244C_Remarks = a.NCMCETD244C_Remarks,
                                        NCMCETD244C_Id = a.NCMCETD244C_Id,
                                        NCMCETD244_Id = a.NCMCETD244_Id,
                                        NCMCETD244C_RemarksBy = a.NCMCETD244C_RemarksBy,
                                        NCMCETD244C_StatusFlg = a.NCMCETD244C_StatusFlg,
                                        NCMCETD244C_ActiveFlag = a.NCMCETD244C_ActiveFlag,
                                        NCMCETD244C_CreatedBy = a.NCMCETD244C_CreatedBy,
                                        NCMCETD244C_CreatedDate = a.NCMCETD244C_CreatedDate,
                                        NCMCETD244C_UpdatedDate = a.NCMCETD244C_UpdatedDate,
                                        NCMCETD244C_UpdatedBy = a.NCMCETD244C_UpdatedBy,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NCMCETD244C_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // view file wise comments
        public NAAC_MC_EmpTrainedDevelopment244_DTO getfilecomment(NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            try
            {
                data.commentlist1 = (from a in _GeneralContext.NAAC_MC_EmpTrainedDevelopment_244_File_Comments_DMO
                                     from b in _GeneralContext.ApplUser
                                     where (a.NCMCETD244FC_RemarksBy == b.Id && a.NCMCETD244F_Id == data.NCMCETD244F_Id)
                                     select new NAAC_MC_EmpTrainedDevelopment244_DTO
                                     {
                                         NCMCETD244F_Id = a.NCMCETD244F_Id,
                                         NCMCETD244FC_Remarks = a.NCMCETD244FC_Remarks,
                                         NCMCETD244FC_Id = a.NCMCETD244FC_Id,
                                         NCMCETD244FC_RemarksBy = a.NCMCETD244FC_RemarksBy,
                                         NCMCETD244FC_StatusFlg = a.NCMCETD244FC_StatusFlg,
                                         NCMCETD244FC_ActiveFlag = a.NCMCETD244FC_ActiveFlag,
                                         NCMCETD244FC_CreatedBy = a.NCMCETD244FC_CreatedBy,
                                         NCMCETD244FC_CreatedDate = a.NCMCETD244FC_CreatedDate,
                                         NCMCETD244FC_UpdatedBy = a.NCMCETD244FC_UpdatedBy,
                                         NCMCETD244FC_UpdatedDate = a.NCMCETD244FC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().OrderByDescending(a => a.NCMCETD244FC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


    }
}
