using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class NAAC_AC_633_AdmTrainingImpl : Interface.NAAC_AC_633_AdmTrainingInterface
    {
        public GeneralContext _GeneralContext;
        public NAAC_AC_633_AdmTrainingImpl(GeneralContext w)
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
                                 from b in _GeneralContext.NAAC_AC_633_AdmTraining_DMO
                                 where (a.MI_Id == b.MI_Id && a.Is_Active == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == b.NCAC633ADMTRG_Year)
                                 select new NAAC_Criteria_6_DTO
                                 {
                                     NCAC633ADMTRG_Id = b.NCAC633ADMTRG_Id,
                                     Name = b.NCAC633ADMTRG_Title,
                                     ASMAY_Id = b.NCAC633ADMTRG_Year,
                                     TotalCount = b.NCAC633ADMTRG_NoOfParticipants,
                                     fdate = b.NCAC633ADMTRG_FromDate.ToString("dd/MM/yyyy"),
                                     tdate = b.NCAC633ADMTRG_ToDate.ToString("dd/MM/yyyy"),
                                     flag7 = b.NCAC633ADMTRG_ProfDevAdmTrgFlg,
                                     ASMAY_Year = a.ASMAY_Year,
                                     flag2 = b.NCAC633ADMTRG_ActiveFlg,
                                     MI_Id = b.MI_Id,
                                     NCAC633ADMTRG_ApprovedFlg = b.NCAC633ADMTRG_ApprovedFlg,
                                     NCAC633ADMTRG_StatusFlg = b.NCAC633ADMTRG_StatusFlg,
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
                if (data.NCAC633ADMTRG_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_633_AdmTraining_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC633ADMTRG_Title == data.Name).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        NAAC_AC_633_AdmTraining_DMO u = new NAAC_AC_633_AdmTraining_DMO();

                        u.MI_Id = data.MI_Id;
                        u.NCAC633ADMTRG_Year = data.ASMAY_Id;
                        u.NCAC633ADMTRG_Title = data.Name;
                        u.NCAC633ADMTRG_FromDate = data.fromdate;
                        u.NCAC633ADMTRG_ToDate = data.todate;
                        u.NCAC633ADMTRG_NoOfParticipants = data.TotalCount;
                        u.NCAC633ADMTRG_ProfDevAdmTrgFlg = data.flag7;
                        u.NCAC633ADMTRG_CreatedBy = data.UserId;
                        u.NCAC633ADMTRG_UpdatedBy = data.UserId;
                        u.NCAC633ADMTRG_CreatedDate = DateTime.Now;
                        u.NCAC633ADMTRG_UpdatedDate = DateTime.Now;
                        u.NCAC633ADMTRG_Year = data.ASMAY_Id;
                        u.NCAC633ADMTRG_ActiveFlg = true;
                        u.NCAC633ADMTRG_StatusFlg = "";
                        u.NCAC633ADMTRG_Remarks = "";


                        _GeneralContext.Add(u);

                        if (data.pgTempDTO.Length > 0)
                        {
                            foreach (pgTempDTO x in data.pgTempDTO)
                            {
                                if (x.LPMTR_Resources != null)
                                {
                                    NAAC_AC_633_AdmTraining_files_DMO b = new NAAC_AC_633_AdmTraining_files_DMO();
                                    b.NCAC633ADMTRG_Id = u.NCAC633ADMTRG_Id;
                                    b.NCAC633ADMTRGF_Filedesc = x.desc;
                                    b.NCAC633ADMTRGF_FileName = x.file_name;
                                    b.NCAC633ADMTRGF_FilePath = x.LPMTR_Resources;
                                    b.NCAC633ADMTRGF_StatusFlg = "";
                                    b.NCAC633ADMTRGF_Remarks = "";
                                    b.NCAC633ADMTRGF_ActiveFlg = true;
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

                else if (data.NCAC633ADMTRG_Id > 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_633_AdmTraining_DMO.Where(t => t.MI_Id == data.MI_Id && data.NCAC633ADMTRG_Id != data.NCAC633ADMTRG_Id && t.NCAC633ADMTRG_Title == data.Name && t.NCAC633ADMTRG_NoOfParticipants == data.TotalCount && t.NCAC633ADMTRG_Year == data.ASMAY_Id).Distinct().ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        var j = _GeneralContext.NAAC_AC_633_AdmTraining_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC633ADMTRG_Id == data.NCAC633ADMTRG_Id).SingleOrDefault();

                        j.NCAC633ADMTRG_Title = data.Name;
                        j.NCAC633ADMTRG_FromDate = data.fromdate;
                        j.NCAC633ADMTRG_ToDate = data.todate;
                        j.NCAC633ADMTRG_NoOfParticipants = data.TotalCount;
                        j.NCAC633ADMTRG_ProfDevAdmTrgFlg = data.flag7;
                        j.NCAC633ADMTRG_UpdatedDate = DateTime.Now;
                        j.NCAC633ADMTRG_UpdatedBy = data.UserId;
                        _GeneralContext.Update(j);


                        //var remove = _GeneralContext.NAAC_AC_633_AdmTraining_files_DMO.Where(t => t.NCAC633ADMTRG_Id == data.NCAC633ADMTRG_Id).ToList();

                        //if (remove.Count > 0)
                        //{
                        //    foreach (var item in remove)
                        //    {
                        //        _GeneralContext.Remove(item);
                        //    }
                        //}

                        //if (data.pgTempDTO.Length  > 0)
                        //{
                        //    foreach (pgTempDTO x in data.pgTempDTO)
                        //    {
                        //        if (x.LPMTR_Resources != null)
                        //        {
                        //            NAAC_AC_633_AdmTraining_files_DMO b = new NAAC_AC_633_AdmTraining_files_DMO();
                        //            b.NCAC633ADMTRG_Id = data.NCAC633ADMTRG_Id;
                        //            b.NCAC633ADMTRGF_FileName = x.file_name;
                        //            b.NCAC633ADMTRGF_FilePath = x.LPMTR_Resources;
                        //            b.NCAC633ADMTRGF_Filedesc = x.desc;
                        //            b.NCAC633ADMTRGF_StatusFlg = "";
                        //            b.NCAC633ADMTRGF_Remarks = "";
                        //            _GeneralContext.Add(b);
                        //        }                                
                        //    }
                        //}

                        var CountRemoveFiles = _GeneralContext.NAAC_AC_633_AdmTraining_files_DMO.Where(b => b.NCAC633ADMTRG_Id == data.NCAC633ADMTRG_Id).ToList();

                        List<long> temparr = new List<long>();
                        //getting all mobilenumbers
                        foreach (var c in data.pgTempDTO)
                        {
                            temparr.Add(c.cfileid);
                        }


                        var Phone_Noresultremove = _GeneralContext.NAAC_AC_633_AdmTraining_files_DMO.Where(c => !temparr.Contains(c.NCAC633ADMTRGF_Id)
                        && c.NCAC633ADMTRG_Id == data.NCAC633ADMTRG_Id).ToList();

                        foreach (var ph1 in Phone_Noresultremove)
                        {
                            var resultremove112 = _GeneralContext.NAAC_AC_633_AdmTraining_files_DMO.Single(a => a.NCAC633ADMTRGF_Id == ph1.NCAC633ADMTRGF_Id);
                            resultremove112.NCAC633ADMTRGF_ActiveFlg = false;
                            _GeneralContext.Update(resultremove112);

                        }


                        if (data.pgTempDTO.Length > 0)
                        {
                            for (int k = 0; k < data.pgTempDTO.Length; k++)
                            {
                                var resultupload = _GeneralContext.NAAC_AC_633_AdmTraining_files_DMO.Where(a => a.NCAC633ADMTRG_Id == data.NCAC633ADMTRG_Id
                                && a.NCAC633ADMTRGF_Id == data.pgTempDTO[k].cfileid).ToList();
                                if (resultupload.Count > 0)
                                {
                                    var resultupdateupload = _GeneralContext.NAAC_AC_633_AdmTraining_files_DMO.Single(a => a.NCAC633ADMTRG_Id == data.NCAC633ADMTRG_Id
                                    && a.NCAC633ADMTRGF_Id == data.pgTempDTO[k].cfileid);
                                    resultupdateupload.NCAC633ADMTRGF_Filedesc = data.pgTempDTO[k].desc;
                                    resultupdateupload.NCAC633ADMTRGF_FileName = data.pgTempDTO[k].file_name;
                                    resultupdateupload.NCAC633ADMTRGF_FilePath = data.pgTempDTO[k].LPMTR_Resources;
                                    _GeneralContext.Update(resultupdateupload);
                                }
                                else
                                {
                                    if (data.pgTempDTO[k].LPMTR_Resources != null && data.pgTempDTO[k].LPMTR_Resources != "")
                                    {
                                        NAAC_AC_633_AdmTraining_files_DMO obj2 = new NAAC_AC_633_AdmTraining_files_DMO();
                                        obj2.NCAC633ADMTRGF_FileName = data.pgTempDTO[k].file_name;
                                        obj2.NCAC633ADMTRGF_Filedesc = data.pgTempDTO[k].desc;
                                        obj2.NCAC633ADMTRGF_FilePath = data.pgTempDTO[k].LPMTR_Resources;
                                        obj2.NCAC633ADMTRG_Id = data.NCAC633ADMTRG_Id;
                                        obj2.NCAC633ADMTRGF_ActiveFlg = true;
                                        obj2.NCAC633ADMTRGF_StatusFlg = "";
                                        obj2.NCAC633ADMTRGF_Remarks = "";

                                        _GeneralContext.Add(obj2);
                                    }
                                }
                            }
                        }

                        var i = _GeneralContext.SaveChanges();
                        if (i > 0)
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
                var g = _GeneralContext.NAAC_AC_633_AdmTraining_DMO.Where(t => t.NCAC633ADMTRG_Id == data.NCAC633ADMTRG_Id).SingleOrDefault();
                if (g.NCAC633ADMTRG_ActiveFlg == true)
                {
                    g.NCAC633ADMTRG_ActiveFlg = false;
                }
                else
                {
                    g.NCAC633ADMTRG_ActiveFlg = true;
                }
                g.NCAC633ADMTRG_UpdatedDate = DateTime.Now;
                g.NCAC633ADMTRG_UpdatedBy = data.UserId;
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
                                 from b in _GeneralContext.NAAC_AC_633_AdmTraining_DMO
                                 where (b.NCAC633ADMTRG_Id == data.NCAC633ADMTRG_Id && a.MI_Id == data.MI_Id
                                 && a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCAC633ADMTRG_Year)
                                 select new NAAC_Criteria_6_DTO
                                 {
                                     NCAC633ADMTRG_Id = b.NCAC633ADMTRG_Id,
                                     Name = b.NCAC633ADMTRG_Title,
                                     ASMAY_Year = a.ASMAY_Year,
                                     ASMAY_Id = a.ASMAY_Id,
                                     TotalCount = b.NCAC633ADMTRG_NoOfParticipants,
                                     fromdate = b.NCAC633ADMTRG_FromDate,
                                     todate = b.NCAC633ADMTRG_ToDate,
                                     flag7 = b.NCAC633ADMTRG_ProfDevAdmTrgFlg,
                                     flag1 = b.NCAC633ADMTRG_ActiveFlg,
                                     MI_Id = b.MI_Id,
                                 }).Distinct().ToArray();

                data.editfiles = (from a in _GeneralContext.NAAC_AC_633_AdmTraining_DMO
                                  from b in _GeneralContext.NAAC_AC_633_AdmTraining_files_DMO
                                  where (a.NCAC633ADMTRG_Id == data.NCAC633ADMTRG_Id && a.MI_Id == data.MI_Id && a.NCAC633ADMTRG_Id == b.NCAC633ADMTRG_Id && b.NCAC633ADMTRGF_ActiveFlg == true)
                                  select new NAAC_Criteria_6_DTO
                                  {
                                      NCAC633ADMTRGF_Id = b.NCAC633ADMTRGF_Id,
                                      NCAC633ADMTRG_Id = a.NCAC633ADMTRG_Id,
                                      FileName = b.NCAC633ADMTRGF_FileName,
                                      filepath = b.NCAC633ADMTRGF_FilePath,
                                      description = b.NCAC633ADMTRGF_Filedesc,
                                      MI_Id = a.MI_Id,
                                      cfileid = b.NCAC633ADMTRGF_Id,
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

                data.savedresult = (from a in _GeneralContext.NAAC_AC_633_AdmTraining_files_DMO
                                    from b in _GeneralContext.NAAC_AC_633_AdmTraining_DMO
                                    where (a.NCAC633ADMTRG_Id == data.NCAC633ADMTRG_Id && b.NCAC633ADMTRG_Id == b.NCAC633ADMTRG_Id && b.MI_Id == data.MI_Id && a.NCAC633ADMTRGF_ActiveFlg == true)
                                    select new NAAC_Criteria_6_DTO
                                    {
                                        NCAC633ADMTRGF_Id = a.NCAC633ADMTRGF_Id,
                                        NCAC633ADMTRG_Id = a.NCAC633ADMTRG_Id,
                                        FileName = a.NCAC633ADMTRGF_FileName,
                                        filepath = a.NCAC633ADMTRGF_FilePath,
                                        description = a.NCAC633ADMTRGF_Filedesc,
                                        MI_Id = b.MI_Id,
                                        NCAC633ADMTRGF_StatusFlg = a.NCAC633ADMTRGF_StatusFlg,
                                        NCAC633ADMTRGF_ApprovedFlg = a.NCAC633ADMTRGF_ApprovedFlg,
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


                var check = _GeneralContext.NAAC_AC_633_AdmTraining_files_DMO.Where(a => a.NCAC633ADMTRGF_Id == data.NCAC633ADMTRGF_Id).ToList();

                if (check.Count > 0)
                {
                    var result = _GeneralContext.NAAC_AC_633_AdmTraining_files_DMO.Single(a => a.NCAC633ADMTRGF_Id == data.NCAC633ADMTRGF_Id);

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
                                     from b in _GeneralContext.NAAC_AC_633_AdmTraining_DMO
                                     where (a.MI_Id == b.MI_Id && a.Is_Active == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == b.NCAC633ADMTRG_Year)
                                     select new NAAC_Criteria_6_DTO
                                     {
                                         NCAC633ADMTRG_Id = b.NCAC633ADMTRG_Id,
                                         Name = b.NCAC633ADMTRG_Title,
                                         ASMAY_Id = b.NCAC633ADMTRG_Year,
                                         TotalCount = b.NCAC633ADMTRG_NoOfParticipants,
                                         fdate = b.NCAC633ADMTRG_FromDate.ToString("dd/MM/yyyy"),
                                         tdate = b.NCAC633ADMTRG_ToDate.ToString("dd/MM/yyyy"),
                                         flag7 = b.NCAC633ADMTRG_ProfDevAdmTrgFlg,
                                         ASMAY_Year = a.ASMAY_Year,
                                         flag2 = b.NCAC633ADMTRG_ActiveFlg,
                                         MI_Id = b.MI_Id,
                                         NCAC633ADMTRG_StatusFlg = b.NCAC633ADMTRG_StatusFlg,
                                         NCAC633ADMTRG_ApprovedFlg = b.NCAC633ADMTRG_ApprovedFlg

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
                NAAC_AC_633_AdmTraining_Comments_DMO obj1 = new NAAC_AC_633_AdmTraining_Comments_DMO();

                obj1.NCAC633ADMTRGC_Remarks = data.Remarks;
                obj1.NCAC633ADMTRGC_RemarksBy = data.UserId;
                obj1.NCAC633ADMTRGC_StatusFlg = "";
                obj1.NCAC633ADMTRG_Id = data.filefkid;
                obj1.NCAC633ADMTRGC_ActiveFlag = true;
                obj1.NCAC633ADMTRGC_CreatedBy = data.UserId;
                obj1.NCAC633ADMTRGC_UpdatedBy = data.UserId;
                obj1.NCAC633ADMTRGC_CreatedDate = DateTime.Now;
                obj1.NCAC633ADMTRGC_UpdatedDate = DateTime.Now;

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
                NAAC_AC_633_AdmTraining_File_Comments_DMO obj1 = new NAAC_AC_633_AdmTraining_File_Comments_DMO();

                obj1.NCAC633ADMTRGFC_Remarks = data.Remarks;
                obj1.NCAC633ADMTRGFC_RemarksBy = data.UserId;
                obj1.NCAC633ADMTRGFC_StatusFlg = "";
                obj1.NCAC633ADMTRGF_Id = data.filefkid;
                obj1.NCAC633ADMTRGFC_ActiveFlag = true;
                obj1.NCAC633ADMTRGFC_CreatedBy = data.UserId;
                obj1.NCAC633ADMTRGFC_UpdatedBy = data.UserId;
                obj1.NCAC633ADMTRGFC_CreatedDate = DateTime.Now;
                obj1.NCAC633ADMTRGFC_UpdatedDate = DateTime.Now;

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
                data.commentlist = (from a in _GeneralContext.NAAC_AC_633_AdmTraining_Comments_DMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCAC633ADMTRGC_RemarksBy == b.Id && a.NCAC633ADMTRG_Id == data.NCAC633ADMTRG_Id)
                                    select new NAAC_Criteria_6_DTO
                                    {
                                        NCAC633ADMTRGC_Remarks = a.NCAC633ADMTRGC_Remarks,
                                        NCAC633ADMTRGC_Id = a.NCAC633ADMTRGC_Id,
                                        NCAC633ADMTRG_Id = a.NCAC633ADMTRG_Id,
                                        NCAC633ADMTRGC_RemarksBy = a.NCAC633ADMTRGC_RemarksBy,
                                        NCAC633ADMTRGC_StatusFlg = a.NCAC633ADMTRGC_StatusFlg,
                                        NCAC633ADMTRGC_ActiveFlag = a.NCAC633ADMTRGC_ActiveFlag,
                                        NCAC633ADMTRGC_CreatedBy = a.NCAC633ADMTRGC_CreatedBy,
                                        NCAC633ADMTRGC_CreatedDate = a.NCAC633ADMTRGC_CreatedDate,
                                        NCAC633ADMTRGC_UpdatedBy = a.NCAC633ADMTRGC_UpdatedBy,
                                        NCAC633ADMTRGC_UpdatedDate = a.NCAC633ADMTRGC_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NCAC633ADMTRGC_CreatedDate).ToArray();
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
                data.commentlist1 = (from a in _GeneralContext.NAAC_AC_633_AdmTraining_File_Comments_DMO
                                     from b in _GeneralContext.ApplUser
                                     where (a.NCAC633ADMTRGFC_RemarksBy == b.Id && a.NCAC633ADMTRGF_Id == data.NCAC633ADMTRGF_Id)
                                     select new NAAC_Criteria_6_DTO
                                     {
                                         NCAC633ADMTRGFC_Id = a.NCAC633ADMTRGFC_Id,
                                         NCAC633ADMTRGFC_Remarks = a.NCAC633ADMTRGFC_Remarks,
                                         NCAC633ADMTRGF_Id = a.NCAC633ADMTRGF_Id,
                                         NCAC633ADMTRGFC_RemarksBy = a.NCAC633ADMTRGFC_RemarksBy,
                                         NCAC633ADMTRGFC_StatusFlg = a.NCAC633ADMTRGFC_StatusFlg,
                                         NCAC633ADMTRGFC_ActiveFlag = a.NCAC633ADMTRGFC_ActiveFlag,
                                         NCAC633ADMTRGFC_CreatedBy = a.NCAC633ADMTRGFC_CreatedBy,
                                         NCAC633ADMTRGFC_CreatedDate = a.NCAC633ADMTRGFC_CreatedDate,
                                         NCAC633ADMTRGFC_UpdatedBy = a.NCAC633ADMTRGFC_UpdatedBy,
                                         NCAC633ADMTRGFC_UpdatedDate = a.NCAC633ADMTRGFC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().OrderByDescending(a => a.NCAC633ADMTRGFC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}
