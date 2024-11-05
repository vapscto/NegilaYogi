using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class NAAC_AC_653_IQACImpl : Interface.NAAC_AC_653_IQACInterface
    {
        public GeneralContext _GeneralContext;
        public NAAC_AC_653_IQACImpl(GeneralContext w)
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
                                 from b in _GeneralContext.NAAC_AC_653_IQAC_DMO
                                 where (a.MI_Id == b.MI_Id && a.Is_Active == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == b.NCAC653IQAC_Year)
                                 select new NAAC_Criteria_6_DTO
                                 {
                                     NCAC653IQAC_Id = b.NCAC653IQAC_Id,
                                     Name = b.NCAC653IQAC_QualityName,
                                     fdate = b.NCAC653IQAC_Date.ToString("dd/MM/yyyy"),
                                     duration = b.NCAC653IQAC_Duration,
                                     TotalCount = b.NCAC653IQAC_NoOfParticipants,
                                     ASMAY_Year = a.ASMAY_Year,
                                     flag1 = b.NCAC653IQAC_ActiveFlg,
                                     MI_Id = b.MI_Id,
                                     NCAC653IQAC_StatusFlg = b.NCAC653IQAC_StatusFlg,
                                     NCAC653IQAC_ApprovedFlg = b.NCAC653IQAC_ApprovedFlg,
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
                if (data.NCAC653IQAC_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_653_IQAC_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC653IQAC_QualityName == data.Name && t.NCAC653IQAC_Duration
                    == data.duration && t.NCAC653IQAC_NoOfParticipants == data.TotalCount && t.NCAC653IQAC_Date == data.fromdate && t.NCAC653IQAC_Id != 0).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        NAAC_AC_653_IQAC_DMO u = new NAAC_AC_653_IQAC_DMO();

                        u.MI_Id = data.MI_Id;
                        u.NCAC653IQAC_QualityName = data.Name;
                        u.NCAC653IQAC_Duration = data.duration;
                        u.NCAC653IQAC_NoOfParticipants = data.TotalCount;
                        u.NCAC653IQAC_NoOfTeacher = data.NCAC653IQAC_NoOfTeacher;
                        u.NCAC653IQAC_Venue = data.NCAC653IQAC_Venue;
                        u.NCAC653IQAC_Date = data.fromdate;
                        u.NCAC653IQAC_CreatedBy = data.UserId;
                        u.NCAC653IQAC_UpdatedBy = data.UserId;
                        u.NCAC653IQAC_CreatedDate = DateTime.Now;
                        u.NCAC653IQAC_UpdatedDate = DateTime.Now;
                        u.NCAC653IQAC_Year = data.ASMAY_Id;
                        u.NCAC653IQAC_ActiveFlg = true;
                        u.NCAC653IQAC_RegIQACFlg = data.NCAC653IQAC_RegIQACFlg;
                        u.NCAC653IQAC_FeedbackClgImprts = data.NCAC653IQAC_FeedbackClgImprts;
                        u.NCAC653IQAC_PrepOfDocAccBodiesFlg = data.NCAC653IQAC_PrepOfDocAccBodiesFlg;
                        u.NCAC653IQAC_StatusFlg = "";
                        u.NCAC653IQAC_Remarks = "";

                        _GeneralContext.Add(u);

                        if (data.pgTempDTO.Length > 0)
                        {
                            foreach (pgTempDTO x in data.pgTempDTO)
                            {
                                if (x.LPMTR_Resources != null)
                                {
                                    NAAC_AC_653_IQAC_files_DMO b = new NAAC_AC_653_IQAC_files_DMO();
                                    b.NCAC653IQAC_Id = u.NCAC653IQAC_Id;
                                    b.NCAC653IQACF_Filedesc = x.desc;
                                    b.NCAC653IQACF_FileName = x.file_name;
                                    b.NCAC653IQACF_FilePath = x.LPMTR_Resources;
                                    b.NCAC653IQACF_ActiveFlg = true;
                                    b.NCAC653IQACF_Remarks = "";
                                    b.NCAC653IQACF_StatusFlg = "";
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

                else if (data.NCAC653IQAC_Id > 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_653_IQAC_DMO.Where(t => t.MI_Id == data.MI_Id && data.NCAC653IQAC_Id != data.NCAC653IQAC_Id && t.NCAC653IQAC_QualityName == data.Name && t.NCAC653IQAC_Duration
                    == data.duration && t.NCAC653IQAC_NoOfParticipants == data.TotalCount && t.NCAC653IQAC_Date == data.fromdate && t.NCAC653IQAC_Year == data.ASMAY_Id).Distinct().ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        var u = _GeneralContext.NAAC_AC_653_IQAC_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC653IQAC_Id == data.NCAC653IQAC_Id).SingleOrDefault();

                        u.NCAC653IQAC_QualityName = data.Name;
                        u.NCAC653IQAC_Duration = data.duration;
                        u.NCAC653IQAC_NoOfParticipants = data.TotalCount;
                        u.NCAC653IQAC_Date = data.fromdate;
                        u.NCAC653IQAC_UpdatedBy = data.UserId;
                        u.NCAC653IQAC_NoOfTeacher = data.NCAC653IQAC_NoOfTeacher;
                        u.NCAC653IQAC_Venue = data.NCAC653IQAC_Venue;
                        u.NCAC653IQAC_UpdatedDate = DateTime.Now;
                        u.NCAC653IQAC_Year = data.ASMAY_Id;
                        u.NCAC653IQAC_ActiveFlg = true;
                        u.NCAC653IQAC_RegIQACFlg = data.NCAC653IQAC_RegIQACFlg;
                        u.NCAC653IQAC_FeedbackClgImprts = data.NCAC653IQAC_FeedbackClgImprts;
                        u.NCAC653IQAC_PrepOfDocAccBodiesFlg = data.NCAC653IQAC_PrepOfDocAccBodiesFlg;
                        _GeneralContext.Update(u);

                        //var removelist = _GeneralContext.NAAC_AC_653_IQAC_files_DMO.Where(t => t.NCAC653IQAC_Id == data.NCAC653IQAC_Id).ToList();
                        //if (removelist.Count > 0)
                        //{
                        //    foreach (var removeitem in removelist)
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
                        //            NAAC_AC_653_IQAC_files_DMO a = new NAAC_AC_653_IQAC_files_DMO();
                        //            a.NCAC653IQAC_Id = data.NCAC653IQAC_Id;
                        //            a.NCAC653IQACF_Filedesc = x.desc;
                        //            a.NCAC653IQACF_FileName = x.file_name;
                        //            a.NCAC653IQACF_FilePath = x.LPMTR_Resources;
                        //            _GeneralContext.Add(a);
                        //        }
                        //    }
                        //}


                        var CountRemoveFiles = _GeneralContext.NAAC_AC_653_IQAC_files_DMO.Where(b => b.NCAC653IQAC_Id == data.NCAC653IQAC_Id).ToList();

                        List<long> temparr = new List<long>();
                        //getting all mobilenumbers
                        foreach (var c in data.pgTempDTO)
                        {
                            temparr.Add(c.cfileid);
                        }


                        var Phone_Noresultremove = _GeneralContext.NAAC_AC_653_IQAC_files_DMO.Where(c => !temparr.Contains(c.NCAC653IQACF_Id)
                        && c.NCAC653IQAC_Id == data.NCAC653IQAC_Id).ToList();

                        foreach (var ph1 in Phone_Noresultremove)
                        {
                            var resultremove112 = _GeneralContext.NAAC_AC_653_IQAC_files_DMO.Single(b => b.NCAC653IQACF_Id == ph1.NCAC653IQACF_Id);
                            resultremove112.NCAC653IQACF_ActiveFlg = false;
                            _GeneralContext.Update(resultremove112);

                        }


                        if (data.pgTempDTO.Length > 0)
                        {
                            for (int p = 0; p < data.pgTempDTO.Length; p++)
                            {
                                var resultupload = _GeneralContext.NAAC_AC_653_IQAC_files_DMO.Where(c1 => c1.NCAC653IQAC_Id == data.NCAC653IQAC_Id
                                && c1.NCAC653IQACF_Id == data.pgTempDTO[p].cfileid).ToList();
                                if (resultupload.Count > 0)
                                {
                                    var resultupdateupload = _GeneralContext.NAAC_AC_653_IQAC_files_DMO.Single(d1 => d1.NCAC653IQAC_Id == data.NCAC653IQAC_Id
                                    && d1.NCAC653IQACF_Id == data.pgTempDTO[p].cfileid);
                                    resultupdateupload.NCAC653IQACF_FileName = data.pgTempDTO[p].file_name;
                                    resultupdateupload.NCAC653IQACF_Filedesc = data.pgTempDTO[p].desc;
                                    resultupdateupload.NCAC653IQACF_FilePath = data.pgTempDTO[p].LPMTR_Resources;
                                    _GeneralContext.Update(resultupdateupload);
                                }
                                else
                                {
                                    if (data.pgTempDTO[p].LPMTR_Resources != null && data.pgTempDTO[p].LPMTR_Resources != "")
                                    {
                                        NAAC_AC_653_IQAC_files_DMO obj2 = new NAAC_AC_653_IQAC_files_DMO();
                                        obj2.NCAC653IQACF_FileName = data.pgTempDTO[p].file_name;
                                        obj2.NCAC653IQACF_Filedesc = data.pgTempDTO[p].desc;
                                        obj2.NCAC653IQACF_FilePath = data.pgTempDTO[p].LPMTR_Resources;
                                        obj2.NCAC653IQAC_Id = data.NCAC653IQAC_Id;
                                        obj2.NCAC653IQACF_ActiveFlg = true;
                                        obj2.NCAC653IQACF_StatusFlg = "";
                                        obj2.NCAC653IQACF_Remarks = "";

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
                var g = _GeneralContext.NAAC_AC_653_IQAC_DMO.Where(t => t.NCAC653IQAC_Id == data.NCAC653IQAC_Id).SingleOrDefault();
                if (g.NCAC653IQAC_ActiveFlg == true)
                {
                    g.NCAC653IQAC_ActiveFlg = false;
                }
                else
                {
                    g.NCAC653IQAC_ActiveFlg = true;
                }
                g.NCAC653IQAC_UpdatedDate = DateTime.Now;
                g.NCAC653IQAC_UpdatedBy = data.UserId;
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
                                 from b in _GeneralContext.NAAC_AC_653_IQAC_DMO
                                 where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && b.NCAC653IQAC_Id == data.NCAC653IQAC_Id && a.ASMAY_Id == b.NCAC653IQAC_Year)
                                 select new NAAC_Criteria_6_DTO
                                 {
                                     NCAC653IQAC_Id = b.NCAC653IQAC_Id,
                                     Name = b.NCAC653IQAC_QualityName,
                                     fromdate = b.NCAC653IQAC_Date,
                                     duration = b.NCAC653IQAC_Duration,
                                     NCAC653IQAC_Venue = b.NCAC653IQAC_Venue,
                                     NCAC653IQAC_NoOfTeacher = b.NCAC653IQAC_NoOfTeacher,
                                     TotalCount = b.NCAC653IQAC_NoOfParticipants,
                                     ASMAY_Year = a.ASMAY_Year,

                                     flag1 = b.NCAC653IQAC_ActiveFlg,
                                     ASMAY_Id = b.NCAC653IQAC_Year,
                                     MI_Id = b.MI_Id,
                                     NCAC653IQAC_RegIQACFlg = b.NCAC653IQAC_RegIQACFlg,
                                     NCAC653IQAC_FeedbackClgImprts = b.NCAC653IQAC_FeedbackClgImprts,
                                     NCAC653IQAC_PrepOfDocAccBodiesFlg = b.NCAC653IQAC_PrepOfDocAccBodiesFlg,
                                 }).Distinct().ToArray();
                data.editfiles = (from a in _GeneralContext.NAAC_AC_653_IQAC_files_DMO
                                  from b in _GeneralContext.NAAC_AC_653_IQAC_DMO
                                  where (a.NCAC653IQAC_Id == data.NCAC653IQAC_Id && b.MI_Id == data.MI_Id && b.NCAC653IQAC_Id == a.NCAC653IQAC_Id && a.NCAC653IQACF_ActiveFlg == true)
                                  select new NAAC_Criteria_6_DTO
                                  {
                                      NCAC653IQACF_Id = a.NCAC653IQACF_Id,
                                      NCAC653IQAC_Id = a.NCAC653IQAC_Id,
                                      FileName = a.NCAC653IQACF_FileName,
                                      filepath = a.NCAC653IQACF_FilePath,
                                      description = a.NCAC653IQACF_Filedesc,
                                      cfileid = a.NCAC653IQACF_Id,
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

                data.savedresult = (from a in _GeneralContext.NAAC_AC_653_IQAC_files_DMO
                                    from b in _GeneralContext.NAAC_AC_653_IQAC_DMO
                                    where (a.NCAC653IQAC_Id == data.NCAC653IQAC_Id && b.MI_Id == data.MI_Id && b.NCAC653IQAC_Id == a.NCAC653IQAC_Id
                                    && a.NCAC653IQACF_ActiveFlg == true)
                                    select new NAAC_Criteria_6_DTO
                                    {
                                        NCAC653IQACF_Id = a.NCAC653IQACF_Id,
                                        NCAC653IQAC_Id = a.NCAC653IQAC_Id,
                                        FileName = a.NCAC653IQACF_FileName,
                                        filepath = a.NCAC653IQACF_FilePath,
                                        description = a.NCAC653IQACF_Filedesc,
                                        NCAC653IQACF_StatusFlg = a.NCAC653IQACF_StatusFlg,
                                        NCAC653IQACF_ApprovedFlg = a.NCAC653IQACF_ApprovedFlg,
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


                var check = _GeneralContext.NAAC_AC_653_IQAC_files_DMO.Where(a => a.NCAC653IQACF_Id == data.NCAC653IQACF_Id).ToList();

                if (check.Count > 0)
                {
                    var result = _GeneralContext.NAAC_AC_653_IQAC_files_DMO.Single(a => a.NCAC653IQACF_Id == data.NCAC653IQACF_Id);

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
                                     from b in _GeneralContext.NAAC_AC_653_IQAC_DMO
                                     where (a.MI_Id == b.MI_Id && a.Is_Active == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == b.NCAC653IQAC_Year)
                                     select new NAAC_Criteria_6_DTO
                                     {
                                         NCAC653IQAC_Id = b.NCAC653IQAC_Id,
                                         Name = b.NCAC653IQAC_QualityName,
                                         fdate = b.NCAC653IQAC_Date.ToString("dd/MM/yyyy"),
                                         duration = b.NCAC653IQAC_Duration,
                                         NCAC653IQAC_NoOfTeacher = b.NCAC653IQAC_NoOfTeacher,
                                         NCAC653IQAC_Venue = b.NCAC653IQAC_Venue,
                                         TotalCount = b.NCAC653IQAC_NoOfParticipants,
                                         ASMAY_Year = a.ASMAY_Year,
                                         flag1 = b.NCAC653IQAC_ActiveFlg,
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
                NAAC_AC_653_IQAC_Comments_DMO obj1 = new NAAC_AC_653_IQAC_Comments_DMO();

                obj1.NCAC653IQACC_Remarks = data.Remarks;
                obj1.NCAC653IQACC_RemarksBy = data.UserId;
                obj1.NCAC653IQACC_StatusFlg = "";
                obj1.NCAC653IQAC_Id = data.filefkid;
                obj1.NCAC653IQACC_ActiveFlag = true;
                obj1.NCAC653IQACC_CreatedBy = data.UserId;
                obj1.NCAC653IQACC_UpdatedBy = data.UserId;
                obj1.NCAC653IQACC_CreatedDate = DateTime.Now;
                obj1.NCAC653IQACC_UpdatedDate = DateTime.Now;

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
                NAAC_AC_653_IQAC_File_Comments_DMO obj1 = new NAAC_AC_653_IQAC_File_Comments_DMO();

                obj1.NCAC653IQACFC_Remarks = data.Remarks;
                obj1.NCAC653IQACFC_RemarksBy = data.UserId;
                obj1.NCAC653IQACFC_StatusFlg = "";
                obj1.NCAC653IQACF_Id = data.filefkid;
                obj1.NCAC653IQACFC_ActiveFlag = true;
                obj1.NCAC653IQACFC_CreatedBy = data.UserId;
                obj1.NCAC653IQACFC_UpdatedBy = data.UserId;
                obj1.NCAC653IQACFC_CreatedDate = DateTime.Now;
                obj1.NCAC653IQACFC_UpdatedDate = DateTime.Now;

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
                data.commentlist = (from a in _GeneralContext.NAAC_AC_653_IQAC_Comments_DMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCAC653IQACC_CreatedBy == b.Id && a.NCAC653IQAC_Id == data.NCAC653IQAC_Id)
                                    select new NAAC_Criteria_6_DTO
                                    {
                                        NCAC653IQACC_Remarks = a.NCAC653IQACC_Remarks,
                                        NCAC653IQAC_Id = a.NCAC653IQAC_Id,
                                        NCAC653IQACC_Id = a.NCAC653IQACC_Id,
                                        NCAC653IQACC_RemarksBy = a.NCAC653IQACC_RemarksBy,
                                        NCAC653IQACC_StatusFlg = a.NCAC653IQACC_StatusFlg,
                                        NCAC653IQACC_ActiveFlag = a.NCAC653IQACC_ActiveFlag,
                                        NCAC653IQACC_CreatedBy = a.NCAC653IQACC_CreatedBy,
                                        NCAC653IQACC_CreatedDate = a.NCAC653IQACC_CreatedDate,
                                        NCAC653IQACC_UpdatedBy = a.NCAC653IQACC_UpdatedBy,
                                        NCAC653IQACC_UpdatedDate = a.NCAC653IQACC_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NCAC653IQACC_CreatedDate).ToArray();
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
                data.commentlist1 = (from a in _GeneralContext.NAAC_AC_653_IQAC_File_Comments_DMO
                                     from b in _GeneralContext.ApplUser
                                     where (a.NCAC653IQACFC_RemarksBy == b.Id && a.NCAC653IQACF_Id == data.NCAC653IQACF_Id)
                                     select new NAAC_Criteria_6_DTO
                                     {
                                         NCAC653IQACF_Id = a.NCAC653IQACF_Id,
                                         NCAC653IQACFC_Remarks = a.NCAC653IQACFC_Remarks,
                                         NCAC653IQACFC_Id = a.NCAC653IQACFC_Id,
                                         NCAC653IQACFC_RemarksBy = a.NCAC653IQACFC_RemarksBy,
                                         NCAC653IQACFC_StatusFlg = a.NCAC653IQACFC_StatusFlg,
                                         NCAC653IQACFC_ActiveFlag = a.NCAC653IQACFC_ActiveFlag,
                                         NCAC653IQACFC_CreatedBy = a.NCAC653IQACFC_CreatedBy,
                                         NCAC653IQACFC_CreatedDate = a.NCAC653IQACFC_CreatedDate,
                                         NCAC653IQACFC_UpdatedBy = a.NCAC653IQACFC_UpdatedBy,
                                         NCAC653IQACFC_UpdatedDate = a.NCAC653IQACFC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().OrderByDescending(a => a.NCAC653IQACFC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }




    }
}
