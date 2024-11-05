using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class NAAC_AC_634_DevProgramsImpl : Interface.NAAC_AC_634_DevProgramsInterface
    {
        public GeneralContext _GeneralContext;
        public NAAC_AC_634_DevProgramsImpl(GeneralContext w)
        {
            _GeneralContext = w;
        }
        public NAAC_Criteria_6_DTO loaddata(NAAC_Criteria_6_DTO data)
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
                                 from b in _GeneralContext.NAAC_AC_634_DevPrograms_DMO
                                 where (a.MI_Id == b.MI_Id && a.Is_Active == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == b.NCAC634DEVPRG_Year)
                                 select new NAAC_Criteria_6_DTO
                                 {
                                     NCAC634DEVPRG_Id = b.NCAC634DEVPRG_Id,
                                     TotalCount = b.NCAC634DEVPRG_NoOfTeachersAttnd,
                                     ASMAY_Id = a.ASMAY_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                     Name = b.NCAC634DEVPRG_PDProgTitle,
                                     flag1 = b.NCAC634DEVPRG_ActiveFlg,
                                     fdate = b.NCAC634DEVPRG_FromDate.ToString("dd/MM/yyyy"),
                                     tdate = b.NCAC634DEVPRG_ToDate.ToString("dd/MM/yyyy"),
                                     MI_Id = b.MI_Id,
                                     NCAC634DEVPRG_StatusFlg = b.NCAC634DEVPRG_StatusFlg,
                                     NCAC634DEVPRG_ApprovedFlg = b.NCAC634DEVPRG_ApprovedFlg,
                                 }).Distinct().ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_Criteria_6_DTO save(NAAC_Criteria_6_DTO data)
        {
            try
            {
                if (data.NCAC634DEVPRG_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_634_DevPrograms_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC634DEVPRG_PDProgTitle == data.Name && t.NCAC634DEVPRG_NoOfTeachersAttnd == data.TotalCount && t.NCAC634DEVPRG_Year == data.ASMAY_Id && t.NCAC634DEVPRG_Id != 0).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        NAAC_AC_634_DevPrograms_DMO u = new NAAC_AC_634_DevPrograms_DMO();

                        u.MI_Id = data.MI_Id;
                        u.NCAC634DEVPRG_NoOfTeachersAttnd = data.TotalCount;
                        u.NCAC634DEVPRG_FromDate = data.fromdate;
                        u.NCAC634DEVPRG_ToDate = data.todate;
                        u.NCAC634DEVPRG_PDProgTitle = data.Name;
                        u.NCAC634DEVPRG_CreatedBy = data.UserId;
                        u.NCAC634DEVPRG_UpdatedBy = data.UserId;
                        u.NCAC634DEVPRG_NameOfTeachers = data.NCAC634DEVPRG_NameOfTeachers;
                        u.NCAC634DEVPRG_CreatedDate = DateTime.Now;
                        u.NCAC634DEVPRG_UpdatedDate = DateTime.Now;
                        u.NCAC634DEVPRG_Year = data.ASMAY_Id;
                        u.NCAC634DEVPRG_ActiveFlg = true;
                        u.NCAC634DEVPRG_StatusFlg = "";
                        u.NCAC634DEVPRG_Remarks = "";

                        _GeneralContext.Add(u);

                        if (data.pgTempDTO.Length > 0)
                        {
                            foreach (pgTempDTO x in data.pgTempDTO)
                            {
                                if (x.LPMTR_Resources != null)
                                {
                                    NAAC_AC_634_DevPrograms_files_DMO b = new NAAC_AC_634_DevPrograms_files_DMO();
                                    b.NCAC634DEVPRG_Id = u.NCAC634DEVPRG_Id;
                                    b.NCAC634DEVPRGF_Filedesc = x.desc;
                                    b.NCAC634DEVPRGF_FileName = x.file_name;
                                    b.NCAC634DEVPRGF_FilePath = x.LPMTR_Resources;
                                    b.NCAC634DEVPRGF_ActiveFlg = true;
                                    b.NCAC634DEVPRGF_StatusFlg = "";
                                    b.NCAC634DEVPRGF_Remarks = "";

                                    _GeneralContext.Add(b);
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

                else if (data.NCAC634DEVPRG_Id > 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_634_DevPrograms_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC634DEVPRG_PDProgTitle == data.Name && data.NCAC634DEVPRG_Id != data.NCAC634DEVPRG_Id && t.NCAC634DEVPRG_NoOfTeachersAttnd == data.TotalCount && t.NCAC634DEVPRG_Year == data.ASMAY_Id).Distinct().ToArray();

                    if (duplicate.Count() > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        var j = _GeneralContext.NAAC_AC_634_DevPrograms_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC634DEVPRG_Id == data.NCAC634DEVPRG_Id).SingleOrDefault();

                        j.NCAC634DEVPRG_NoOfTeachersAttnd = data.TotalCount;
                        j.NCAC634DEVPRG_Year = data.ASMAY_Id;
                        j.NCAC634DEVPRG_NameOfTeachers = data.NCAC634DEVPRG_NameOfTeachers;
                        j.NCAC634DEVPRG_PDProgTitle = data.Name;
                        j.NCAC634DEVPRG_UpdatedDate = DateTime.Now;
                        j.NCAC634DEVPRG_UpdatedBy = data.UserId;
                        _GeneralContext.Update(j);

                        //var remove = _GeneralContext.NAAC_AC_634_DevPrograms_files_DMO.Where(t => t.NCAC634DEVPRG_Id == data.NCAC634DEVPRG_Id).ToList();
                        //if (remove.Count > 0)
                        //{
                        //    foreach (var removelist in remove)
                        //    {
                        //        _GeneralContext.Remove(removelist);
                        //    }
                        //}
                        //if (data.pgTempDTO.Length > 0)
                        //{
                        //    foreach (pgTempDTO x in data.pgTempDTO)
                        //    {
                        //        if (x.LPMTR_Resources!= null)
                        //        {
                        //            NAAC_AC_634_DevPrograms_files_DMO k = new NAAC_AC_634_DevPrograms_files_DMO();
                        //            k.NCAC634DEVPRG_Id = data.NCAC634DEVPRG_Id;
                        //            k.NCAC634DEVPRGF_FileName = x.file_name;
                        //            k.NCAC634DEVPRGF_FilePath = x.LPMTR_Resources;
                        //            k.NCAC634DEVPRGF_Filedesc = x.desc;
                        //            _GeneralContext.Add(k);
                        //        }                                
                        //    }
                        //}


                        var CountRemoveFiles = _GeneralContext.NAAC_AC_634_DevPrograms_files_DMO.Where(b => b.NCAC634DEVPRG_Id == data.NCAC634DEVPRG_Id).ToList();

                        List<long> temparr = new List<long>();
                        //getting all mobilenumbers
                        foreach (var c in data.pgTempDTO)
                        {
                            temparr.Add(c.cfileid);
                        }


                        var Phone_Noresultremove = _GeneralContext.NAAC_AC_634_DevPrograms_files_DMO.Where(c => !temparr.Contains(c.NCAC634DEVPRGF_Id)
                        && c.NCAC634DEVPRG_Id == data.NCAC634DEVPRG_Id).ToList();

                        foreach (var ph1 in Phone_Noresultremove)
                        {
                            var resultremove112 = _GeneralContext.NAAC_AC_634_DevPrograms_files_DMO.Single(b => b.NCAC634DEVPRGF_Id == ph1.NCAC634DEVPRGF_Id);
                            resultremove112.NCAC634DEVPRGF_ActiveFlg = false;
                            _GeneralContext.Update(resultremove112);

                        }


                        if (data.pgTempDTO.Length > 0)
                        {
                            for (int k = 0; k < data.pgTempDTO.Length; k++)
                            {
                                var resultupload = _GeneralContext.NAAC_AC_634_DevPrograms_files_DMO.Where(c => c.NCAC634DEVPRG_Id == data.NCAC634DEVPRG_Id
                                && c.NCAC634DEVPRGF_Id == data.pgTempDTO[k].cfileid).ToList();
                                if (resultupload.Count > 0)
                                {
                                    var resultupdateupload = _GeneralContext.NAAC_AC_634_DevPrograms_files_DMO.Single(d => d.NCAC634DEVPRG_Id == data.NCAC634DEVPRG_Id
                                    && d.NCAC634DEVPRGF_Id == data.pgTempDTO[k].cfileid);
                                    resultupdateupload.NCAC634DEVPRGF_FileName = data.pgTempDTO[k].file_name;
                                    resultupdateupload.NCAC634DEVPRGF_Filedesc = data.pgTempDTO[k].desc;
                                    resultupdateupload.NCAC634DEVPRGF_FilePath = data.pgTempDTO[k].LPMTR_Resources;
                                    _GeneralContext.Update(resultupdateupload);
                                }
                                else
                                {
                                    if (data.pgTempDTO[k].LPMTR_Resources != null && data.pgTempDTO[k].LPMTR_Resources != "")
                                    {
                                        NAAC_AC_634_DevPrograms_files_DMO obj2 = new NAAC_AC_634_DevPrograms_files_DMO();
                                        obj2.NCAC634DEVPRGF_FileName = data.pgTempDTO[k].file_name;
                                        obj2.NCAC634DEVPRGF_Filedesc = data.pgTempDTO[k].desc;
                                        obj2.NCAC634DEVPRGF_FilePath = data.pgTempDTO[k].LPMTR_Resources;
                                        obj2.NCAC634DEVPRG_Id = data.NCAC634DEVPRG_Id;
                                        obj2.NCAC634DEVPRGF_ActiveFlg = true;
                                        obj2.NCAC634DEVPRGF_StatusFlg = "";
                                        obj2.NCAC634DEVPRGF_Remarks = "";

                                        _GeneralContext.Add(obj2);
                                    }
                                }
                            }
                        }


                        var a = _GeneralContext.SaveChanges();
                        if (a > 0)
                        {
                            data.msg = "updated";
                        }
                        else
                        {
                            data.msg = "upfailed";
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
        public NAAC_Criteria_6_DTO deactiveStudent(NAAC_Criteria_6_DTO data)
        {
            try
            {
                var g = _GeneralContext.NAAC_AC_634_DevPrograms_DMO.Where(t => t.NCAC634DEVPRG_Id == data.NCAC634DEVPRG_Id).SingleOrDefault();
                if (g.NCAC634DEVPRG_ActiveFlg == true)
                {
                    g.NCAC634DEVPRG_ActiveFlg = false;
                }
                else
                {
                    g.NCAC634DEVPRG_ActiveFlg = true;
                }
                g.NCAC634DEVPRG_UpdatedDate = DateTime.Now;
                g.NCAC634DEVPRG_UpdatedBy = data.UserId;
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
        public NAAC_Criteria_6_DTO EditData(NAAC_Criteria_6_DTO data)
        {
            try
            {

                data.editlist = (from a in _GeneralContext.Academic
                                 from b in _GeneralContext.NAAC_AC_634_DevPrograms_DMO
                                 where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && b.NCAC634DEVPRG_Id == data.NCAC634DEVPRG_Id && a.ASMAY_Id == b.NCAC634DEVPRG_Year)
                                 select new NAAC_Criteria_6_DTO
                                 {
                                     NCAC634DEVPRG_Id = b.NCAC634DEVPRG_Id,
                                     TotalCount = b.NCAC634DEVPRG_NoOfTeachersAttnd,
                                     ASMAY_Id = a.ASMAY_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                     Name = b.NCAC634DEVPRG_PDProgTitle,
                                     flag1 = b.NCAC634DEVPRG_ActiveFlg,
                                     fromdate = b.NCAC634DEVPRG_FromDate,
                                     todate = b.NCAC634DEVPRG_ToDate,
                                     MI_Id = b.MI_Id,
                                     NCAC634DEVPRG_NameOfTeachers = b.NCAC634DEVPRG_NameOfTeachers,
                                 }).Distinct().ToArray();
                data.editfiles = (from a in _GeneralContext.NAAC_AC_634_DevPrograms_DMO
                                  from b in _GeneralContext.NAAC_AC_634_DevPrograms_files_DMO
                                  where (b.NCAC634DEVPRG_Id == data.NCAC634DEVPRG_Id && a.MI_Id == data.MI_Id && a.NCAC634DEVPRG_Id == b.NCAC634DEVPRG_Id && b.NCAC634DEVPRGF_ActiveFlg == true)
                                  select new NAAC_Criteria_6_DTO
                                  {
                                      NCAC634DEVPRGF_Id = b.NCAC634DEVPRGF_Id,
                                      NCAC634DEVPRG_Id = a.NCAC634DEVPRG_Id,
                                      FileName = b.NCAC634DEVPRGF_FileName,
                                      filepath = b.NCAC634DEVPRGF_FilePath,
                                      description = b.NCAC634DEVPRGF_Filedesc,
                                      cfileid = b.NCAC634DEVPRGF_Id,
                                  }).Distinct().ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_Criteria_6_DTO viewuploadflies(NAAC_Criteria_6_DTO data)
        {
            try
            {

                data.savedresult = (from a in _GeneralContext.NAAC_AC_634_DevPrograms_files_DMO
                                    from b in _GeneralContext.NAAC_AC_634_DevPrograms_DMO
                                    where (a.NCAC634DEVPRG_Id == data.NCAC634DEVPRG_Id && b.MI_Id == data.MI_Id && b.NCAC634DEVPRG_Id == a.NCAC634DEVPRG_Id && a.NCAC634DEVPRGF_ActiveFlg == true)
                                    select new NAAC_Criteria_6_DTO
                                    {
                                        NCAC634DEVPRGF_Id = a.NCAC634DEVPRGF_Id,
                                        NCAC634DEVPRG_Id = a.NCAC634DEVPRG_Id,
                                        FileName = a.NCAC634DEVPRGF_FileName,
                                        filepath = a.NCAC634DEVPRGF_FilePath,
                                        description = a.NCAC634DEVPRGF_Filedesc,
                                        NCAC634DEVPRGF_StatusFlg = a.NCAC634DEVPRGF_StatusFlg,
                                        NCAC634DEVPRGF_ApprovedFlg = a.NCAC634DEVPRGF_ApprovedFlg,
                                    }).Distinct().ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_Criteria_6_DTO deleteuploadfile(NAAC_Criteria_6_DTO data)
        {
            try
            {


                var check = _GeneralContext.NAAC_AC_634_DevPrograms_files_DMO.Where(a => a.NCAC634DEVPRGF_Id == data.NCAC634DEVPRGF_Id).ToList();

                if (check.Count > 0)
                {
                    var result = _GeneralContext.NAAC_AC_634_DevPrograms_files_DMO.Single(a => a.NCAC634DEVPRGF_Id == data.NCAC634DEVPRGF_Id);

                    _GeneralContext.Remove(result);

                    var i = _GeneralContext.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }

                    data.alldata1 = (from a in _GeneralContext.Academic
                                     from b in _GeneralContext.NAAC_AC_634_DevPrograms_DMO
                                     where (a.MI_Id == b.MI_Id && a.Is_Active == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == b.NCAC634DEVPRG_Year)
                                     select new NAAC_Criteria_6_DTO
                                     {
                                         NCAC634DEVPRG_Id = b.NCAC634DEVPRG_Id,
                                         TotalCount = b.NCAC634DEVPRG_NoOfTeachersAttnd,
                                         ASMAY_Id = a.ASMAY_Id,
                                         ASMAY_Year = a.ASMAY_Year,
                                         Name = b.NCAC634DEVPRG_PDProgTitle,
                                         flag1 = b.NCAC634DEVPRG_ActiveFlg,
                                         fdate = b.NCAC634DEVPRG_FromDate.ToString("dd/MM/yyyy"),
                                         tdate = b.NCAC634DEVPRG_ToDate.ToString("dd/MM/yyyy"),
                                     }).Distinct().ToArray();
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



        //add row wise comments
        public NAAC_Criteria_6_DTO savemedicaldatawisecomments(NAAC_Criteria_6_DTO data)
        {
            try
            {
                NAAC_AC_634_DevPrograms_Comments_DMO obj1 = new NAAC_AC_634_DevPrograms_Comments_DMO();

                obj1.NCAC634DEVPRGC_Remarks = data.Remarks;
                obj1.NCAC634DEVPRGC_RemarksBy = data.UserId;
                obj1.NCAC634DEVPRGC_StatusFlg = "";
                obj1.NCAC634DEVPRG_Id = data.filefkid;
                obj1.NCAC634DEVPRGC_ActiveFlag = true;
                obj1.NCAC634DEVPRGC_CreatedBy = data.UserId;
                obj1.NCAC634DEVPRGC_UpdatedBy = data.UserId;
                obj1.NCAC634DEVPRGC_CreatedDate = DateTime.Now;
                obj1.NCAC634DEVPRGC_UpdatedDate = DateTime.Now;

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
        public NAAC_Criteria_6_DTO savefilewisecomments(NAAC_Criteria_6_DTO data)
        {
            try
            {
                NAAC_AC_634_DevPrograms_File_Comments_DMO obj1 = new NAAC_AC_634_DevPrograms_File_Comments_DMO();

                obj1.NCAC634DEVPRGFC_Remarks = data.Remarks;
                obj1.NCAC634DEVPRGFC_RemarksBy = data.UserId;
                obj1.NCAC634DEVPRGFC_StatusFlg = "";
                obj1.NCAC634DEVPRGF_Id = data.filefkid;
                obj1.NCAC634DEVPRGFC_ActiveFlag = true;
                obj1.NCAC634DEVPRGFC_CreatedBy = data.UserId;
                obj1.NCAC634DEVPRGFC_UpdatedBy = data.UserId;
                obj1.NCAC634DEVPRGFC_CreatedDate = DateTime.Now;
                obj1.NCAC634DEVPRGFC_UpdatedDate = DateTime.Now;

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
        public NAAC_Criteria_6_DTO getcomment(NAAC_Criteria_6_DTO data)
        {
            try
            {
                data.commentlist = (from a in _GeneralContext.NAAC_AC_634_DevPrograms_Comments_DMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCAC634DEVPRGC_CreatedBy == b.Id && a.NCAC634DEVPRG_Id == data.NCAC634DEVPRG_Id)
                                    select new NAAC_Criteria_6_DTO
                                    {
                                        NCAC634DEVPRGC_Remarks = a.NCAC634DEVPRGC_Remarks,
                                        NCAC634DEVPRG_Id = a.NCAC634DEVPRG_Id,
                                        NCAC634DEVPRGC_Id = a.NCAC634DEVPRGC_Id,
                                        NCAC634DEVPRGC_RemarksBy = a.NCAC634DEVPRGC_RemarksBy,
                                        NCAC634DEVPRGC_StatusFlg = a.NCAC634DEVPRGC_StatusFlg,
                                        NCAC634DEVPRGC_ActiveFlag = a.NCAC634DEVPRGC_ActiveFlag,
                                        NCAC634DEVPRGC_CreatedBy = a.NCAC634DEVPRGC_CreatedBy,
                                        NCAC634DEVPRGC_CreatedDate = a.NCAC634DEVPRGC_CreatedDate,
                                        NCAC634DEVPRGC_UpdatedBy = a.NCAC634DEVPRGC_UpdatedBy,
                                        NCAC634DEVPRGC_UpdatedDate = a.NCAC634DEVPRGC_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NCAC634DEVPRGC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // view file wise comments
        public NAAC_Criteria_6_DTO getfilecomment(NAAC_Criteria_6_DTO data)
        {
            try
            {
                data.commentlist1 = (from a in _GeneralContext.NAAC_AC_634_DevPrograms_File_Comments_DMO
                                     from b in _GeneralContext.ApplUser
                                     where (a.NCAC634DEVPRGFC_RemarksBy == b.Id && a.NCAC634DEVPRGF_Id == data.NCAC634DEVPRGF_Id)
                                     select new NAAC_Criteria_6_DTO
                                     {
                                         NCAC634DEVPRGFC_Id = a.NCAC634DEVPRGFC_Id,
                                         NCAC634DEVPRGFC_Remarks = a.NCAC634DEVPRGFC_Remarks,
                                         NCAC634DEVPRGF_Id = a.NCAC634DEVPRGF_Id,
                                         NCAC634DEVPRGFC_RemarksBy = a.NCAC634DEVPRGFC_RemarksBy,
                                         NCAC634DEVPRGFC_StatusFlg = a.NCAC634DEVPRGFC_StatusFlg,
                                         NCAC634DEVPRGFC_ActiveFlag = a.NCAC634DEVPRGFC_ActiveFlag,
                                         NCAC634DEVPRGFC_CreatedBy = a.NCAC634DEVPRGFC_CreatedBy,
                                         NCAC634DEVPRGFC_CreatedDate = a.NCAC634DEVPRGFC_CreatedDate,
                                         NCAC634DEVPRGFC_UpdatedBy = a.NCAC634DEVPRGFC_UpdatedBy,
                                         NCAC634DEVPRGFC_UpdatedDate = a.NCAC634DEVPRGFC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().OrderByDescending(a => a.NCAC634DEVPRGFC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



    }
}
