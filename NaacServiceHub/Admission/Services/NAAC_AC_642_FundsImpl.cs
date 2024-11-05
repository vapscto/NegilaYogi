using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class NAAC_AC_642_FundsImpl : Interface.NAAC_AC_642_FundsInterface
    {
        public GeneralContext _GeneralContext;
        public NAAC_AC_642_FundsImpl(GeneralContext w)
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
                                 from b in _GeneralContext.NAAC_AC_642_Funds_DMO
                                 where (a.MI_Id == b.MI_Id && a.Is_Active == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == b.NCAC642FUND_Year)
                                 select new NAAC_Criteria_6_DTO
                                 {
                                     NCAC642FUND_Id = b.NCAC642FUND_Id,
                                     Name = b.NCAC642FUND_AgencyName,
                                     amount = b.NCAC642FUND_Amount,
                                     org = b.NCAC642FUND_Initiative,
                                     ASMAY_Year = a.ASMAY_Year,
                                     ASMAY_Id = a.ASMAY_Id,
                                     flag1 = b.NCAC642FUND_ActiveFlg,
                                     MI_Id = b.MI_Id,
                                     NCAC642FUND_StatusFlg = b.NCAC642FUND_StatusFlg,
                                     NCAC642FUND_ApprovedFlg = b.NCAC642FUND_ApprovedFlg,
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
                if (data.NCAC642FUND_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_642_Funds_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC642FUND_AgencyName == data.Name && t.NCAC642FUND_Amount == data.amount && t.NCAC642FUND_Initiative == data.org && t.NCAC642FUND_Id != 0).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {


                        NAAC_AC_642_Funds_DMO u = new NAAC_AC_642_Funds_DMO();
                        if (data.flagradio == "S")
                        {
                            u.NCAC642FUND_GovORNongovFlag = "G";
                        }
                        if (data.flagradio == "L")
                        {
                            u.NCAC642FUND_GovORNongovFlag = "N";
                        }
                        u.MI_Id = data.MI_Id;
                        u.NCAC642FUND_AgencyName = data.Name;
                        u.NCAC642FUND_Amount = data.amount;
                        u.NCAC642FUND_Initiative = data.org;
                        u.NCAC642FUND_CreatedBy = data.UserId;
                        u.NCAC642FUND_UpdatedBy = data.UserId;
                        u.NCAC642FUND_CreatedDate = DateTime.Now;
                        u.NCAC642FUND_UpdatedDate = DateTime.Now;
                        u.NCAC642FUND_Year = data.ASMAY_Id;
                        u.NCAC642FUND_ActiveFlg = true;
                        u.NCAC642FUND_StatusFlg = "";
                        u.NCAC642FUND_Remarks = "";

                        _GeneralContext.Add(u);

                        foreach (pgTempDTO x in data.pgTempDTO)
                        {
                            NAAC_AC_642_Funds_files_DMO b = new NAAC_AC_642_Funds_files_DMO();
                            b.NCAC642FUND_Id = u.NCAC642FUND_Id;
                            b.NCAC642FUNDF_Filedesc = x.desc;
                            b.NCAC642FUNDF_FileName = x.file_name;
                            b.NCAC642FUNDF_FilePath = x.LPMTR_Resources;
                            b.NCAC642FUNDF_StatusFlg = "";
                            b.NCAC642FUNDF_Remarks = "";
                            b.NCAC642FUNDF_ActiveFlg = true;
                            _GeneralContext.Add(b);
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

                else if (data.NCAC642FUND_Id > 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_642_Funds_DMO.Where(t => t.MI_Id == data.MI_Id && data.NCAC642FUND_Id != data.NCAC642FUND_Id && t.NCAC642FUND_AgencyName == data.Name && t.NCAC642FUND_Amount == data.amount && t.NCAC642FUND_Initiative == data.org && t.NCAC642FUND_Year == data.ASMAY_Id).Distinct().ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        var j = _GeneralContext.NAAC_AC_642_Funds_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC642FUND_Id == data.NCAC642FUND_Id).SingleOrDefault();


                        if (data.flagradio == "S")
                        {
                            j.NCAC642FUND_GovORNongovFlag = "G";
                        }
                        if (data.flagradio == "L")
                        {
                            j.NCAC642FUND_GovORNongovFlag = "N";
                        }
                        j.NCAC642FUND_AgencyName = data.Name;
                        j.NCAC642FUND_Amount = data.amount;
                        j.NCAC642FUND_Initiative = data.org;
                        j.NCAC642FUND_Year = data.ASMAY_Id;
                        j.NCAC642FUND_UpdatedDate = DateTime.Now;
                        j.NCAC642FUND_UpdatedBy = data.UserId;
                        _GeneralContext.Update(j);

                        //if (data.NCAC642FUND_Id > 0)
                        //{
                        //    var lorg3 = _GeneralContext.NAAC_AC_642_Funds_files_DMO.Where(t => t.NCAC642FUND_Id == data.NCAC642FUND_Id).ToList();
                        //    foreach (var c in lorg3)
                        //    {
                        //        var checkresult = _GeneralContext.NAAC_AC_642_Funds_files_DMO.Single(a => a.NCAC642FUNDF_Id == c.NCAC642FUNDF_Id);
                        //        _GeneralContext.Remove(checkresult);
                        //    }
                        //}

                        //var contactexisttransaction = 0;
                        //using (var dbCtxTxn = _GeneralContext.Database.BeginTransaction())
                        //{
                        //    try
                        //    {
                        //        contactexisttransaction = _GeneralContext.SaveChanges();
                        //        dbCtxTxn.Commit();
                        //        data.returnvaledit = "true";


                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        dbCtxTxn.Rollback();
                        //        data.returnvaledit = "false";
                        //    }
                        //}



                        //foreach (pgTempDTO x in data.pgTempDTO)
                        //{
                        //    NAAC_AC_642_Funds_files_DMO a = new NAAC_AC_642_Funds_files_DMO();
                        //    a.NCAC642FUND_Id = data.NCAC642FUND_Id;
                        //    a.NCAC642FUNDF_FileName = x.file_name;
                        //    a.NCAC642FUNDF_FilePath = x.LPMTR_Resources;
                        //    a.NCAC642FUNDF_Filedesc = x.desc;
                        //    _GeneralContext.Add(a);
                        //}


                        var CountRemoveFiles = _GeneralContext.NAAC_AC_642_Funds_files_DMO.Where(b => b.NCAC642FUND_Id == data.NCAC642FUND_Id).ToList();

                        List<long> temparr = new List<long>();
                        //getting all mobilenumbers
                        foreach (var c in data.pgTempDTO)
                        {
                            temparr.Add(c.cfileid);
                        }


                        var Phone_Noresultremove = _GeneralContext.NAAC_AC_642_Funds_files_DMO.Where(c => !temparr.Contains(c.NCAC642FUNDF_Id)
                        && c.NCAC642FUND_Id == data.NCAC642FUND_Id).ToList();

                        foreach (var ph1 in Phone_Noresultremove)
                        {
                            var resultremove112 = _GeneralContext.NAAC_AC_642_Funds_files_DMO.Single(a => a.NCAC642FUNDF_Id == ph1.NCAC642FUNDF_Id);
                            resultremove112.NCAC642FUNDF_ActiveFlg = false;
                            _GeneralContext.Update(resultremove112);

                        }


                        if (data.pgTempDTO.Length > 0)
                        {
                            for (int k1 = 0; k1 < data.pgTempDTO.Length; k1++)
                            {
                                var resultupload = _GeneralContext.NAAC_AC_642_Funds_files_DMO.Where(a => a.NCAC642FUND_Id == data.NCAC642FUND_Id
                                && a.NCAC642FUNDF_Id == data.pgTempDTO[k1].cfileid).ToList();
                                if (resultupload.Count > 0)
                                {
                                    var resultupdateupload = _GeneralContext.NAAC_AC_642_Funds_files_DMO.Single(a => a.NCAC642FUND_Id == data.NCAC642FUND_Id
                                    && a.NCAC642FUNDF_Id == data.pgTempDTO[k1].cfileid);
                                    resultupdateupload.NCAC642FUNDF_FileName = data.pgTempDTO[k1].file_name;
                                    resultupdateupload.NCAC642FUNDF_Filedesc = data.pgTempDTO[k1].desc;
                                    resultupdateupload.NCAC642FUNDF_FilePath = data.pgTempDTO[k1].LPMTR_Resources;
                                    _GeneralContext.Update(resultupdateupload);
                                }
                                else
                                {
                                    if (data.pgTempDTO[k1].LPMTR_Resources != null && data.pgTempDTO[k1].LPMTR_Resources != "")
                                    {
                                        NAAC_AC_642_Funds_files_DMO obj2 = new NAAC_AC_642_Funds_files_DMO();
                                        obj2.NCAC642FUNDF_FileName = data.pgTempDTO[k1].file_name;
                                        obj2.NCAC642FUNDF_Filedesc = data.pgTempDTO[k1].desc;
                                        obj2.NCAC642FUNDF_FilePath = data.pgTempDTO[k1].LPMTR_Resources;
                                        obj2.NCAC642FUND_Id = data.NCAC642FUND_Id;
                                        obj2.NCAC642FUNDF_ActiveFlg = true;
                                        obj2.NCAC642FUNDF_StatusFlg = "";
                                        obj2.NCAC642FUNDF_Remarks = "";

                                        _GeneralContext.Add(obj2);
                                    }
                                }
                            }
                        }


                        var k = _GeneralContext.SaveChanges();
                        if (k > 0)
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
                var g = _GeneralContext.NAAC_AC_642_Funds_DMO.Where(t => t.NCAC642FUND_Id == data.NCAC642FUND_Id).SingleOrDefault();
                if (g.NCAC642FUND_ActiveFlg == true)
                {
                    g.NCAC642FUND_ActiveFlg = false;
                }
                else
                {
                    g.NCAC642FUND_ActiveFlg = true;
                }
                g.NCAC642FUND_UpdatedDate = DateTime.Now;
                g.NCAC642FUND_UpdatedBy = data.UserId;
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
                                 from b in _GeneralContext.NAAC_AC_642_Funds_DMO
                                 where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && b.NCAC642FUND_Id == data.NCAC642FUND_Id && a.ASMAY_Id == b.NCAC642FUND_Year)
                                 select new NAAC_Criteria_6_DTO
                                 {
                                     NCAC642FUND_Id = b.NCAC642FUND_Id,
                                     Name = b.NCAC642FUND_AgencyName,
                                     amount = b.NCAC642FUND_Amount,
                                     org = b.NCAC642FUND_Initiative,
                                     NCAC642FUND_GovORNongovFlag = b.NCAC642FUND_GovORNongovFlag,
                                     ASMAY_Year = a.ASMAY_Year,
                                     ASMAY_Id = a.ASMAY_Id,
                                     flag1 = b.NCAC642FUND_ActiveFlg,
                                     MI_Id = b.MI_Id,
                                 }).Distinct().ToArray();
                data.editfiles = (from a in _GeneralContext.NAAC_AC_642_Funds_DMO
                                  from b in _GeneralContext.NAAC_AC_642_Funds_files_DMO
                                  where (b.NCAC642FUND_Id == data.NCAC642FUND_Id && a.MI_Id == data.MI_Id && a.NCAC642FUND_Id == b.NCAC642FUND_Id && b.NCAC642FUNDF_ActiveFlg == true)
                                  select new NAAC_Criteria_6_DTO
                                  {
                                      NCAC642FUNDF_Id = b.NCAC642FUNDF_Id,
                                      NCAC642FUND_Id = a.NCAC642FUND_Id,
                                      FileName = b.NCAC642FUNDF_FileName,
                                      filepath = b.NCAC642FUNDF_FilePath,
                                      description = b.NCAC642FUNDF_Filedesc,
                                      cfileid = b.NCAC642FUNDF_Id,
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

                data.savedresult = (from a in _GeneralContext.NAAC_AC_642_Funds_files_DMO
                                    where (a.NCAC642FUND_Id == data.NCAC642FUND_Id && a.NCAC642FUNDF_ActiveFlg == true)
                                    select new NAAC_Criteria_6_DTO
                                    {
                                        NCAC642FUNDF_Id = a.NCAC642FUNDF_Id,
                                        NCAC642FUND_Id = a.NCAC642FUND_Id,
                                        FileName = a.NCAC642FUNDF_FileName,
                                        filepath = a.NCAC642FUNDF_FilePath,
                                        description = a.NCAC642FUNDF_Filedesc,
                                        NCAC642FUNDF_StatusFlg = a.NCAC642FUNDF_StatusFlg,
                                        NCAC642FUNDF_ApprovedFlg = a.NCAC642FUNDF_ApprovedFlg,
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


                var check = _GeneralContext.NAAC_AC_642_Funds_files_DMO.Where(a => a.NCAC642FUNDF_Id == data.NCAC642FUNDF_Id).ToList();

                if (check.Count > 0)
                {
                    var result = _GeneralContext.NAAC_AC_642_Funds_files_DMO.Single(a => a.NCAC642FUNDF_Id == data.NCAC642FUNDF_Id);

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
                                     from b in _GeneralContext.NAAC_AC_642_Funds_DMO
                                     where (a.MI_Id == b.MI_Id && a.Is_Active == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == b.NCAC642FUND_Year)
                                     select new NAAC_Criteria_6_DTO
                                     {
                                         NCAC642FUND_Id = b.NCAC642FUND_Id,
                                         Name = b.NCAC642FUND_AgencyName,
                                         amount = b.NCAC642FUND_Amount,
                                         org = b.NCAC642FUND_Initiative,
                                         NCAC642FUND_GovORNongovFlag = b.NCAC642FUND_GovORNongovFlag,
                                         ASMAY_Year = a.ASMAY_Year,
                                         ASMAY_Id = a.ASMAY_Id,
                                         flag1 = b.NCAC642FUND_ActiveFlg,
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
                NAAC_AC_642_Funds_Comments_DMO obj1 = new NAAC_AC_642_Funds_Comments_DMO();

                obj1.NCAC642FUNDC_Remarks = data.Remarks;
                obj1.NCAC642FUNDC_RemarksBy = data.UserId;
                obj1.NCAC642FUNDC_StatusFlg = "";
                obj1.NCAC642FUND_Id = data.filefkid;
                obj1.NCAC642FUNDC_ActiveFlag = true;
                obj1.NCAC642FUNDC_CreatedBy = data.UserId;
                obj1.NCAC642FUNDC_UpdatedBy = data.UserId;
                obj1.NCAC642FUNDC_CreatedDate = DateTime.Now;
                obj1.NCAC642FUNDC_UpdatedDate = DateTime.Now;

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
                NAAC_AC_642_Funds_File_Comments_DMO obj1 = new NAAC_AC_642_Funds_File_Comments_DMO();

                obj1.NCAC642FUNDFC_Remarks = data.Remarks;
                obj1.NCAC642FUNDFC_RemarksBy = data.UserId;
                obj1.NCAC642FUNDFC_StatusFlg = "";
                obj1.NCAC642FUNDF_Id = data.filefkid;
                obj1.NCAC642FUNDFC_ActiveFlag = true;
                obj1.NCAC642FUNDFC_CreatedBy = data.UserId;
                obj1.NCAC642FUNDFC_UpdatedBy = data.UserId;
                obj1.NCAC642FUNDFC_CreatedDate = DateTime.Now;
                obj1.NCAC642FUNDFC_UpdatedDate = DateTime.Now;

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
                data.commentlist = (from a in _GeneralContext.NAAC_AC_642_Funds_Comments_DMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCAC642FUNDC_CreatedBy == b.Id && a.NCAC642FUND_Id == data.NCAC642FUND_Id)
                                    select new NAAC_Criteria_6_DTO
                                    {
                                        NCAC642FUNDC_Remarks = a.NCAC642FUNDC_Remarks,
                                        NCAC642FUND_Id = a.NCAC642FUND_Id,
                                        NCAC642FUNDC_Id = a.NCAC642FUNDC_Id,
                                        NCAC642FUNDC_RemarksBy = a.NCAC642FUNDC_RemarksBy,
                                        NCAC642FUNDC_StatusFlg = a.NCAC642FUNDC_StatusFlg,
                                        NCAC642FUNDC_ActiveFlag = a.NCAC642FUNDC_ActiveFlag,
                                        NCAC642FUNDC_CreatedBy = a.NCAC642FUNDC_CreatedBy,
                                        NCAC642FUNDC_CreatedDate = a.NCAC642FUNDC_CreatedDate,
                                        NCAC642FUNDC_UpdatedBy = a.NCAC642FUNDC_UpdatedBy,
                                        NCAC642FUNDC_UpdatedDate = a.NCAC642FUNDC_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NCAC642FUNDC_CreatedDate).ToArray();
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
                data.commentlist1 = (from a in _GeneralContext.NAAC_AC_642_Funds_File_Comments_DMO
                                     from b in _GeneralContext.ApplUser
                                     where (a.NCAC642FUNDFC_RemarksBy == b.Id && a.NCAC642FUNDF_Id == data.NCAC642FUNDF_Id)
                                     select new NAAC_Criteria_6_DTO
                                     {
                                         NCAC642FUNDFC_Id = a.NCAC642FUNDFC_Id,
                                         NCAC642FUNDFC_Remarks = a.NCAC642FUNDFC_Remarks,
                                         NCAC642FUNDF_Id = a.NCAC642FUNDF_Id,
                                         NCAC642FUNDFC_RemarksBy = a.NCAC642FUNDFC_RemarksBy,
                                         NCAC642FUNDFC_StatusFlg = a.NCAC642FUNDFC_StatusFlg,
                                         NCAC642FUNDFC_ActiveFlag = a.NCAC642FUNDFC_ActiveFlag,
                                         NCAC642FUNDFC_CreatedBy = a.NCAC642FUNDFC_CreatedBy,
                                         NCAC642FUNDFC_CreatedDate = a.NCAC642FUNDFC_CreatedDate,
                                         NCAC642FUNDFC_UpdatedBy = a.NCAC642FUNDFC_UpdatedBy,
                                         NCAC642FUNDFC_UpdatedDate = a.NCAC642FUNDFC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().OrderByDescending(a => a.NCAC642FUNDFC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}
