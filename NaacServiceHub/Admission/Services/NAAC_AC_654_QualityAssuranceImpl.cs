using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class NAAC_AC_654_QualityAssuranceImpl : Interface.NAAC_AC_654_QualityAssuranceInterface
    {
        public GeneralContext _GeneralContext;
        public NAAC_AC_654_QualityAssuranceImpl(GeneralContext w)
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
                                 from b in _GeneralContext.NAAC_AC_654_QualityAssurance_DMO
                                 where (a.MI_Id == b.MI_Id && a.Is_Active == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == b.NCAC654QUAS_Year)
                                 select new NAAC_Criteria_6_DTO
                                 {
                                     NCAC654QUAS_Id = b.NCAC654QUAS_Id,
                                     ASMAY_Id = b.NCAC654QUAS_Year,
                                     ASMAY_Year = a.ASMAY_Year,
                                     flag1 = b.NCAC654QUAS_AAAFlg,
                                     flag2 = b.NCAC654QUAS_AQARFlg,
                                     flag3 = b.NCAC654QUAS_ISOFlg,
                                     flag4 = b.NCAC654QUAS_NBAFlg,
                                     flag5 = b.NCAC654QUAS_NIRFFlg,
                                     flag51 = b.NCAC654QUAS_FkStsCollectedAnlreportFlag,
                                     flag52 = b.NCAC654QUAS_OrgWsSsPrgAdmStaffFlag,
                                     flag6 = b.NCAC654QUAS_ActiveFlg,
                                     MI_Id = b.MI_Id,
                                     NCAC654QUAS_StatusFlg = b.NCAC654QUAS_StatusFlg,
                                     NCAC654QUAS_ApprovedFlg = b.NCAC654QUAS_ApprovedFlg,

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
                if (data.NCAC654QUAS_Id == 0)
                {


                    NAAC_AC_654_QualityAssurance_DMO u = new NAAC_AC_654_QualityAssurance_DMO();

                    u.MI_Id = data.MI_Id;
                    u.NCAC654QUAS_AAAFlg = data.flag2;
                    u.NCAC654QUAS_AQARFlg = data.flag1;
                    u.NCAC654QUAS_ISOFlg = data.flag3;
                    u.NCAC654QUAS_NBAFlg = data.flag4;
                    u.NCAC654QUAS_NIRFFlg = data.flag5;
                    u.NCAC654QUAS_FkStsCollectedAnlreportFlag = data.flag51;
                    u.NCAC654QUAS_OrgWsSsPrgAdmStaffFlag = data.flag52;
                    u.NCAC654QUAS_CreatedBy = data.UserId;
                    u.NCAC654QUAS_UpdatedBy = data.UserId;
                    u.NCAC654QUAS_CreatedDate = DateTime.Now;
                    u.NCAC654QUAS_UpdatedDate = DateTime.Now;
                    u.NCAC654QUAS_Year = data.ASMAY_Id;
                    u.NCAC654QUAS_ActiveFlg = true;
                    _GeneralContext.Add(u);


                    if (data.pgTempDTO.Length > 0)
                    {
                        foreach (pgTempDTO x in data.pgTempDTO)
                        {
                            if (x.LPMTR_Resources != null)
                            {
                                NAAC_AC_654_QualityAssurance_files_DMO a = new NAAC_AC_654_QualityAssurance_files_DMO();
                                a.NCAC654QUAS_Id = u.NCAC654QUAS_Id;
                                a.NCAC654QUASF_FileName = x.file_name;
                                a.NCAC654QUASF_Filedesc = x.desc;
                                a.NCAC654QUASF_FilePath = x.LPMTR_Resources;
                                a.NCAC653IQACF_ActiveFlg = true;
                                a.NCAC654QUASF_StatusFlg = "";
                                a.NCAC654QUASF_Remarks = "";

                                _GeneralContext.Add(a);
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

                else if (data.NCAC654QUAS_Id > 0)
                {

                    var u = _GeneralContext.NAAC_AC_654_QualityAssurance_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC654QUAS_Id == data.NCAC654QUAS_Id).SingleOrDefault();
                    u.NCAC654QUAS_AAAFlg = data.flag2;
                    u.NCAC654QUAS_AQARFlg = data.flag1;
                    u.NCAC654QUAS_ISOFlg = data.flag3;
                    u.NCAC654QUAS_NBAFlg = data.flag4;
                    u.NCAC654QUAS_NIRFFlg = data.flag5;
                    u.NCAC654QUAS_FkStsCollectedAnlreportFlag = data.flag51;
                    u.NCAC654QUAS_OrgWsSsPrgAdmStaffFlag = data.flag52;
                    u.NCAC654QUAS_UpdatedBy = data.UserId;
                    u.NCAC654QUAS_UpdatedDate = DateTime.Now;
                    u.NCAC654QUAS_Year = data.ASMAY_Id;
                    u.NCAC654QUAS_ActiveFlg = true;
                    u.NCAC654QUAS_StatusFlg = "";
                    u.NCAC654QUAS_Remarks = "";

                    _GeneralContext.Update(u);

                    //var Removelist = _GeneralContext.NAAC_AC_654_QualityAssurance_files_DMO.Where(t => t.NCAC654QUAS_Id == data.NCAC654QUAS_Id).ToList();
                    //if (Removelist.Count > 0)
                    //{
                    //    foreach (var removeitem in Removelist)
                    //    {
                    //        _GeneralContext.Remove(removeitem);
                    //    }
                    //}

                    //if (data.pgTempDTO.Length > 0)
                    //{
                    //    foreach (pgTempDTO x in data.pgTempDTO)
                    //    {
                    //        if (x.LPMTR_Resources != null)
                    //        {
                    //            NAAC_AC_654_QualityAssurance_files_DMO a = new NAAC_AC_654_QualityAssurance_files_DMO();
                    //            a.NCAC654QUAS_Id = data.NCAC654QUAS_Id;
                    //            a.NCAC654QUASF_FileName = x.file_name;
                    //            a.NCAC654QUASF_Filedesc = x.desc;
                    //            a.NCAC654QUASF_FilePath = x.LPMTR_Resources;
                    //            _GeneralContext.Add(a);
                    //        }
                    //    }
                    //}


                    var CountRemoveFiles = _GeneralContext.NAAC_AC_654_QualityAssurance_files_DMO.Where(b => b.NCAC654QUAS_Id == data.NCAC654QUAS_Id).ToList();

                    List<long> temparr = new List<long>();
                    //getting all mobilenumbers
                    foreach (var c in data.pgTempDTO)
                    {
                        temparr.Add(c.cfileid);
                    }


                    var Phone_Noresultremove = _GeneralContext.NAAC_AC_654_QualityAssurance_files_DMO.Where(c => !temparr.Contains(c.NCAC654QUASF_Id)
                    && c.NCAC654QUAS_Id == data.NCAC654QUAS_Id).ToList();

                    foreach (var ph1 in Phone_Noresultremove)
                    {
                        var resultremove112 = _GeneralContext.NAAC_AC_654_QualityAssurance_files_DMO.Single(b => b.NCAC654QUASF_Id == ph1.NCAC654QUASF_Id);
                        resultremove112.NCAC653IQACF_ActiveFlg = false;
                        _GeneralContext.Update(resultremove112);

                    }


                    if (data.pgTempDTO.Length > 0)
                    {
                        for (int k = 0; k < data.pgTempDTO.Length; k++)
                        {
                            var resultupload = _GeneralContext.NAAC_AC_654_QualityAssurance_files_DMO.Where(a1 => a1.NCAC654QUAS_Id == data.NCAC654QUAS_Id
                            && a1.NCAC654QUASF_Id == data.pgTempDTO[k].cfileid).ToList();
                            if (resultupload.Count > 0)
                            {
                                var resultupdateupload = _GeneralContext.NAAC_AC_654_QualityAssurance_files_DMO.Single(d1 => d1.NCAC654QUAS_Id == data.NCAC654QUAS_Id
                                && d1.NCAC654QUASF_Id == data.pgTempDTO[k].cfileid);
                                resultupdateupload.NCAC654QUASF_FileName = data.pgTempDTO[k].file_name;
                                resultupdateupload.NCAC654QUASF_Filedesc = data.pgTempDTO[k].desc;
                                resultupdateupload.NCAC654QUASF_FilePath = data.pgTempDTO[k].LPMTR_Resources;
                                _GeneralContext.Update(resultupdateupload);
                            }
                            else
                            {
                                if (data.pgTempDTO[k].LPMTR_Resources != null && data.pgTempDTO[k].LPMTR_Resources != "")
                                {
                                    NAAC_AC_654_QualityAssurance_files_DMO obj2 = new NAAC_AC_654_QualityAssurance_files_DMO();
                                    obj2.NCAC654QUASF_FileName = data.pgTempDTO[k].file_name;
                                    obj2.NCAC654QUASF_Filedesc = data.pgTempDTO[k].desc;
                                    obj2.NCAC654QUASF_FilePath = data.pgTempDTO[k].LPMTR_Resources;
                                    obj2.NCAC654QUAS_Id = data.NCAC654QUAS_Id;
                                    obj2.NCAC653IQACF_ActiveFlg = true;
                                    obj2.NCAC654QUASF_StatusFlg = "";
                                    obj2.NCAC654QUASF_Remarks = "";

                                    _GeneralContext.Add(obj2);
                                }
                            }
                        }
                    }

                    var a2 = _GeneralContext.SaveChanges();
                    if (a2 > 0)
                    {
                        data.msg = "updated";
                    }
                    else
                    {
                        data.msg = "upfailed";
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
                var g = _GeneralContext.NAAC_AC_654_QualityAssurance_DMO.Where(t => t.NCAC654QUAS_Id == data.NCAC654QUAS_Id).SingleOrDefault();
                if (g.NCAC654QUAS_ActiveFlg == true)
                {
                    g.NCAC654QUAS_ActiveFlg = false;
                }
                else
                {
                    g.NCAC654QUAS_ActiveFlg = true;
                }
                g.NCAC654QUAS_UpdatedDate = DateTime.Now;
                g.NCAC654QUAS_UpdatedBy = data.UserId;
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
                                 from b in _GeneralContext.NAAC_AC_654_QualityAssurance_DMO
                                 where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && b.NCAC654QUAS_Id == data.NCAC654QUAS_Id && a.ASMAY_Id == b.NCAC654QUAS_Year)
                                 select new NAAC_Criteria_6_DTO
                                 {
                                     NCAC654QUAS_Id = b.NCAC654QUAS_Id,
                                     ASMAY_Id = b.NCAC654QUAS_Year,
                                     ASMAY_Year = a.ASMAY_Year,
                                     flag1 = b.NCAC654QUAS_AAAFlg,
                                     flag2 = b.NCAC654QUAS_AQARFlg,
                                     flag3 = b.NCAC654QUAS_ISOFlg,
                                     flag51 = b.NCAC654QUAS_FkStsCollectedAnlreportFlag,
                                     flag52 = b.NCAC654QUAS_OrgWsSsPrgAdmStaffFlag,
                                     flag4 = b.NCAC654QUAS_NBAFlg,
                                     flag5 = b.NCAC654QUAS_NIRFFlg,
                                     flag6 = b.NCAC654QUAS_ActiveFlg,
                                     MI_Id = b.MI_Id,
                                 }).Distinct().ToArray();
                data.editfiles = (from a in _GeneralContext.NAAC_AC_654_QualityAssurance_files_DMO
                                  from b in _GeneralContext.NAAC_AC_654_QualityAssurance_DMO
                                  where (a.NCAC654QUAS_Id == data.NCAC654QUAS_Id && b.MI_Id == data.MI_Id && b.NCAC654QUAS_Id == a.NCAC654QUAS_Id && a.NCAC653IQACF_ActiveFlg == true)
                                  select new NAAC_Criteria_6_DTO
                                  {
                                      NCAC654QUASF_Id = a.NCAC654QUASF_Id,
                                      NCAC654QUAS_Id = a.NCAC654QUAS_Id,
                                      FileName = a.NCAC654QUASF_FileName,
                                      filepath = a.NCAC654QUASF_FilePath,
                                      description = a.NCAC654QUASF_Filedesc,
                                      cfileid = a.NCAC654QUASF_Id,
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

                data.savedresult = (from a in _GeneralContext.NAAC_AC_654_QualityAssurance_files_DMO
                                    from b in _GeneralContext.NAAC_AC_654_QualityAssurance_DMO
                                    where (a.NCAC654QUAS_Id == data.NCAC654QUAS_Id && b.MI_Id == data.MI_Id && b.NCAC654QUAS_Id == a.NCAC654QUAS_Id && a.NCAC653IQACF_ActiveFlg == true)
                                    select new NAAC_Criteria_6_DTO
                                    {
                                        NCAC654QUASF_Id = a.NCAC654QUASF_Id,
                                        NCAC654QUAS_Id = a.NCAC654QUAS_Id,
                                        FileName = a.NCAC654QUASF_FileName,
                                        filepath = a.NCAC654QUASF_FilePath,
                                        description = a.NCAC654QUASF_Filedesc,
                                        NCAC654QUASF_StatusFlg = a.NCAC654QUASF_StatusFlg,
                                        NCAC654QUASF_ApprovedFlg = a.NCAC654QUASF_ApprovedFlg,
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


                var check = _GeneralContext.NAAC_AC_654_QualityAssurance_files_DMO.Where(a => a.NCAC654QUASF_Id == data.NCAC654QUASF_Id).ToList();

                if (check.Count > 0)
                {
                    var result = _GeneralContext.NAAC_AC_654_QualityAssurance_files_DMO.Single(a => a.NCAC654QUASF_Id == data.NCAC654QUASF_Id);

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
                                     from b in _GeneralContext.NAAC_AC_654_QualityAssurance_DMO
                                     where (a.MI_Id == b.MI_Id && a.Is_Active == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == b.NCAC654QUAS_Year)
                                     select new NAAC_Criteria_6_DTO
                                     {
                                         NCAC654QUAS_Id = b.NCAC654QUAS_Id,
                                         ASMAY_Id = b.NCAC654QUAS_Year,
                                         ASMAY_Year = a.ASMAY_Year,
                                         flag1 = b.NCAC654QUAS_AAAFlg,
                                         flag2 = b.NCAC654QUAS_AQARFlg,
                                         flag3 = b.NCAC654QUAS_ISOFlg,
                                         flag4 = b.NCAC654QUAS_NBAFlg,
                                         flag5 = b.NCAC654QUAS_NIRFFlg,
                                         flag51 = b.NCAC654QUAS_FkStsCollectedAnlreportFlag,
                                         flag52 = b.NCAC654QUAS_OrgWsSsPrgAdmStaffFlag,
                                         flag6 = b.NCAC654QUAS_ActiveFlg,
                                         MI_Id = b.MI_Id,

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
                NAAC_AC_654_QualityAssurance_Comments_DMO obj1 = new NAAC_AC_654_QualityAssurance_Comments_DMO();

                obj1.NCAC654QUASC_Remarks = data.Remarks;
                obj1.NCAC654QUASC_RemarksBy = data.UserId;
                obj1.NCAC654QUASC_StatusFlg = "";
                obj1.NCAC654QUAS_Id = data.filefkid;
                obj1.NCAC654QUASC_ActiveFlag = true;
                obj1.NCAC654QUASC_CreatedBy = data.UserId;
                obj1.NCAC654QUASC_UpdatedBy = data.UserId;
                obj1.NCAC654QUASC_CreatedDate = DateTime.Now;
                obj1.NCAC654QUASC_UpdatedDate = DateTime.Now;

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
                NAAC_AC_654_QualityAssurance_File_Comments_DMO obj1 = new NAAC_AC_654_QualityAssurance_File_Comments_DMO();

                obj1.NCAC654QUASFC_Remarks = data.Remarks;
                obj1.NCAC654QUASFC_RemarksBy = data.UserId;
                obj1.NCAC654QUASFC_StatusFlg = "";
                obj1.NCAC654QUASF_Id = data.filefkid;
                obj1.NCAC654QUASFC_ActiveFlag = true;
                obj1.NCAC654QUASFC_CreatedBy = data.UserId;
                obj1.NCAC654QUASFC_UpdatedBy = data.UserId;
                obj1.NCAC654QUASFC_CreatedDate = DateTime.Now;
                obj1.NCAC654QUASFC_UpdatedDate = DateTime.Now;

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
                data.commentlist = (from a in _GeneralContext.NAAC_AC_654_QualityAssurance_Comments_DMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCAC654QUASC_CreatedBy == b.Id && a.NCAC654QUAS_Id == data.NCAC654QUAS_Id)
                                    select new NAAC_Criteria_6_DTO
                                    {
                                        NCAC654QUASC_Remarks = a.NCAC654QUASC_Remarks,
                                        NCAC654QUAS_Id = a.NCAC654QUAS_Id,
                                        NCAC654QUASC_Id = a.NCAC654QUASC_Id,
                                        NCAC654QUASC_RemarksBy = a.NCAC654QUASC_RemarksBy,
                                        NCAC654QUASC_StatusFlg = a.NCAC654QUASC_StatusFlg,
                                        NCAC654QUASC_ActiveFlag = a.NCAC654QUASC_ActiveFlag,
                                        NCAC654QUASC_CreatedBy = a.NCAC654QUASC_CreatedBy,
                                        NCAC654QUASC_CreatedDate = a.NCAC654QUASC_CreatedDate,
                                        NCAC654QUASC_UpdatedBy = a.NCAC654QUASC_UpdatedBy,
                                        NCAC654QUASC_UpdatedDate = a.NCAC654QUASC_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NCAC654QUASC_CreatedDate).ToArray();
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
                data.commentlist1 = (from a in _GeneralContext.NAAC_AC_654_QualityAssurance_File_Comments_DMO
                                     from b in _GeneralContext.ApplUser
                                     where (a.NCAC654QUASFC_RemarksBy == b.Id && a.NCAC654QUASF_Id == data.NCAC654QUASF_Id)
                                     select new NAAC_Criteria_6_DTO
                                     {
                                         NCAC654QUASFC_Id = a.NCAC654QUASFC_Id,
                                         NCAC654QUASFC_Remarks = a.NCAC654QUASFC_Remarks,
                                         NCAC654QUASF_Id = a.NCAC654QUASF_Id,
                                         NCAC654QUASFC_RemarksBy = a.NCAC654QUASFC_RemarksBy,
                                         NCAC654QUASFC_StatusFlg = a.NCAC654QUASFC_StatusFlg,
                                         NCAC654QUASFC_ActiveFlag = a.NCAC654QUASFC_ActiveFlag,
                                         NCAC654QUASFC_CreatedBy = a.NCAC654QUASFC_CreatedBy,
                                         NCAC654QUASFC_CreatedDate = a.NCAC654QUASFC_CreatedDate,
                                         NCAC654QUASFC_UpdatedBy = a.NCAC654QUASFC_UpdatedBy,
                                         NCAC654QUASFC_UpdatedDate = a.NCAC654QUASFC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().OrderByDescending(a => a.NCAC654QUASFC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


    }
}
