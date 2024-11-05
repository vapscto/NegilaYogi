using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class NaacFinanceSupport632Impl : Interface.NaacFinanceSupport632Interface
    {
        public GeneralContext _GeneralContext;
        public NaacFinanceSupport632Impl(GeneralContext w)
        {
            _GeneralContext = w;
        }
        public NAAC_AC_632_FinanceSupport_DTO loaddata(NAAC_AC_632_FinanceSupport_DTO data)
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
                                 from b in _GeneralContext.NAAC_AC_632_FinanceSupport_DMO
                                 where (a.MI_Id == b.MI_Id && b.NCAC632FINSUP_Year == a.ASMAY_Id && a.Is_Active == true && a.MI_Id == data.MI_Id)
                                 select new NAAC_AC_632_FinanceSupport_DTO
                                 {
                                     NCAC632FINSUP_Id = b.NCAC632FINSUP_Id,
                                     NCAC632FINSUP_Year = b.NCAC632FINSUP_Year,
                                     ASMAY_Year = a.ASMAY_Year,
                                     NCAC632FINSUP_TeacherName = b.NCAC632FINSUP_TeacherName,
                                     NCAC632FINSUP_Name = b.NCAC632FINSUP_Name,
                                     NCAC632FINSUP_NameOfMembership = b.NCAC632FINSUP_NameOfMembership,
                                     NCAC632FINSUP_AmountPaid = b.NCAC632FINSUP_AmountPaid,
                                     NCAC632FINSUP_PAN = b.NCAC632FINSUP_PAN,
                                     NCAC632FINSUP_ConferenceProfBodyFlg = b.NCAC632FINSUP_ConferenceProfBodyFlg,
                                     NCAC632FINSUP_ActiveFlg = b.NCAC632FINSUP_ActiveFlg,
                                     MI_Id = data.MI_Id,
                                     NCAC632FINSUP_StatusFlg = b.NCAC632FINSUP_StatusFlg,
                                     NCAC632FINSUP_ApprovedFlg = b.NCAC632FINSUP_ApprovedFlg,
                                 }).Distinct().OrderByDescending(t => t.NCAC632FINSUP_Id).ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_AC_632_FinanceSupport_DTO save(NAAC_AC_632_FinanceSupport_DTO data)
        {
            try
            {
                if (data.NCAC632FINSUP_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_632_FinanceSupport_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC632FINSUP_Id != 0 && t.NCAC632FINSUP_TeacherName == data.NCAC632FINSUP_TeacherName && t.NCAC632FINSUP_Name == data.NCAC632FINSUP_Name && t.NCAC632FINSUP_Year == data.asmaY_Id).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_632_FinanceSupport_DMO rrr = new NAAC_AC_632_FinanceSupport_DMO();
                        rrr.MI_Id = data.MI_Id;
                        rrr.NCAC632FINSUP_TeacherName = data.NCAC632FINSUP_TeacherName;
                        rrr.NCAC632FINSUP_Year = data.asmaY_Id;
                        rrr.NCAC632FINSUP_Name = data.NCAC632FINSUP_Name;
                        rrr.NCAC632FINSUP_PAN = data.NCAC632FINSUP_PAN;

                        rrr.NCAC632FINSUP_ConferenceProfBodyFlg = data.NCAC632FINSUP_ConferenceProfBodyFlg;
                        rrr.NCAC632FINSUP_CreatedDate = DateTime.Now;
                        rrr.NCAC632FINSUP_UpdatedDate = DateTime.Now;
                        rrr.NCAC632FINSUP_StatusFlg = "";
                        rrr.NCAC632FINSUP_Remarks = "";
                        rrr.NCAC632FINSUP_ActiveFlg = true;
                        if (data.NCAC632FINSUP_ConferenceProfBodyFlg == true)
                        {
                            rrr.NCAC632FINSUP_AmountPaid = data.NCAC632FINSUP_AmountPaid;
                            rrr.NCAC632FINSUP_NameOfMembership = data.NCAC632FINSUP_NameOfMembership;
                        }
                        rrr.NCAC632FINSUP_ActiveFlg = true;
                        rrr.NCAC632FINSUP_CreatedBy = data.UserId;
                        rrr.NCAC632FINSUP_UpdatedBy = data.UserId;
                        _GeneralContext.Add(rrr);
                        if (data.filelist.Length > 0)
                        {
                            for (int i = 0; i < data.filelist.Length; i++)
                            {
                                if (data.filelist[0].cfilepath != null)
                                {


                                    NAAC_AC_632_FinanceSupport_Files_DMO obj2 = new NAAC_AC_632_FinanceSupport_Files_DMO();
                                    obj2.NCAC632FINSUP_Id = rrr.NCAC632FINSUP_Id;

                                    obj2.NCAC632FINSUPF_FileName = data.filelist[i].cfilename;
                                    obj2.NCAC632FINSUPF_Filedesc = data.filelist[i].cfiledesc;
                                    obj2.NCAC632FINSUPF_FilePath = data.filelist[i].cfilepath;
                                    obj2.NCAC632FINSUPF_ActiveFlg = true;
                                    obj2.NCAC632FINSUPF_StatusFlg = "";
                                    obj2.NCAC632FINSUPF_Remarks = "";

                                    _GeneralContext.Add(obj2);
                                }
                            }
                        }
                        int y = _GeneralContext.SaveChanges();
                        if (y > 0)
                        {
                            data.msg = "saved";
                        }
                        else
                        {
                            data.msg = "Failed";
                        }
                    }
                }
                else if (data.NCAC632FINSUP_Id > 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_632_FinanceSupport_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC632FINSUP_TeacherName == data.NCAC632FINSUP_TeacherName && t.NCAC632FINSUP_Name == data.NCAC632FINSUP_Name && t.NCAC632FINSUP_Year == data.NCAC632FINSUP_Year && t.NCAC632FINSUP_Id != data.NCAC632FINSUP_Id).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var yy = _GeneralContext.NAAC_AC_632_FinanceSupport_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC632FINSUP_Id == data.NCAC632FINSUP_Id).SingleOrDefault();
                        yy.NCAC632FINSUP_UpdatedBy = data.UserId;

                        yy.NCAC632FINSUP_TeacherName = data.NCAC632FINSUP_TeacherName;


                        if (data.NCAC632FINSUP_ConferenceProfBodyFlg == true)
                        {
                            yy.NCAC632FINSUP_AmountPaid = data.NCAC632FINSUP_AmountPaid;
                            yy.NCAC632FINSUP_NameOfMembership = data.NCAC632FINSUP_NameOfMembership;
                        }
                        else
                        {
                            yy.NCAC632FINSUP_AmountPaid = 0;
                            yy.NCAC632FINSUP_NameOfMembership = "";
                        }

                        yy.NCAC632FINSUP_PAN = data.NCAC632FINSUP_PAN;

                        yy.NCAC632FINSUP_ConferenceProfBodyFlg = data.NCAC632FINSUP_ConferenceProfBodyFlg;
                        yy.NCAC632FINSUP_Name = data.NCAC632FINSUP_Name;
                        yy.NCAC632FINSUP_UpdatedDate = DateTime.Now;
                        yy.MI_Id = data.MI_Id;
                        _GeneralContext.Update(yy);


                        //var CountRemoveFiles = _GeneralContext.NAAC_AC_632_FinanceSupport_Files_DMO.Where(t => t.NCAC632FINSUP_Id == data.NCAC632FINSUP_Id).ToList();
                        //if (CountRemoveFiles.Count > 0)
                        //{
                        //    foreach (var RemoveFiles in CountRemoveFiles)
                        //    {
                        //        _GeneralContext.Remove(RemoveFiles);
                        //    }
                        //}
                        //if (data.filelist.Length > 0)
                        //{
                        //    for (int i = 0; i < data.filelist.Length; i++)
                        //    {
                        //        if (data.filelist[0].cfilepath != null)
                        //        {


                        //        NAAC_AC_632_FinanceSupport_Files_DMO obj2 = new NAAC_AC_632_FinanceSupport_Files_DMO();

                        //        obj2.NCAC632FINSUP_Id = yy.NCAC632FINSUP_Id;
                        //        obj2.NCAC632FINSUPF_FileName = data.filelist[i].cfilename;
                        //        obj2.NCAC632FINSUPF_Filedesc = data.filelist[i].cfiledesc;
                        //        obj2.NCAC632FINSUPF_FilePath = data.filelist[i].cfilepath;
                        //        _GeneralContext.Add(obj2);
                        //        }
                        //    }
                        //}



                        var CountRemoveFiles = _GeneralContext.NAAC_AC_632_FinanceSupport_Files_DMO.Where(b => b.NCAC632FINSUP_Id == data.NCAC632FINSUP_Id).ToList();

                        List<long> temparr = new List<long>();
                        //getting all mobilenumbers
                        foreach (var c in data.filelist)
                        {
                            temparr.Add(c.cfileid);
                        }


                        var Phone_Noresultremove = _GeneralContext.NAAC_AC_632_FinanceSupport_Files_DMO.Where(c => !temparr.Contains(c.NCAC632FINSUPF_Id)
                        && c.NCAC632FINSUP_Id == data.NCAC632FINSUP_Id).ToList();

                        foreach (var ph1 in Phone_Noresultremove)
                        {
                            var resultremove112 = _GeneralContext.NAAC_AC_632_FinanceSupport_Files_DMO.Single(a => a.NCAC632FINSUPF_Id == ph1.NCAC632FINSUPF_Id);
                            resultremove112.NCAC632FINSUPF_ActiveFlg = false;
                            _GeneralContext.Update(resultremove112);

                        }


                        if (data.filelist.Length > 0)
                        {
                            for (int k = 0; k < data.filelist.Length; k++)
                            {
                                var resultupload = _GeneralContext.NAAC_AC_632_FinanceSupport_Files_DMO.Where(a => a.NCAC632FINSUP_Id == data.NCAC632FINSUP_Id
                                && a.NCAC632FINSUPF_Id == data.filelist[k].cfileid).ToList();
                                if (resultupload.Count > 0)
                                {
                                    var resultupdateupload = _GeneralContext.NAAC_AC_632_FinanceSupport_Files_DMO.Single(a => a.NCAC632FINSUP_Id == data.NCAC632FINSUP_Id
                                    && a.NCAC632FINSUPF_Id == data.filelist[k].cfileid);
                                    resultupdateupload.NCAC632FINSUPF_FileName = data.filelist[k].cfilename;
                                    resultupdateupload.NCAC632FINSUPF_Filedesc = data.filelist[k].cfiledesc;
                                    resultupdateupload.NCAC632FINSUPF_FilePath = data.filelist[k].cfilepath;
                                    _GeneralContext.Update(resultupdateupload);
                                }
                                else
                                {
                                    if (data.filelist[k].cfilepath != null && data.filelist[k].cfilepath != "")
                                    {
                                        NAAC_AC_632_FinanceSupport_Files_DMO obj2 = new NAAC_AC_632_FinanceSupport_Files_DMO();
                                        obj2.NCAC632FINSUPF_FileName = data.filelist[k].cfilename;
                                        obj2.NCAC632FINSUPF_Filedesc = data.filelist[k].cfiledesc;
                                        obj2.NCAC632FINSUPF_FilePath = data.filelist[k].cfilepath;
                                        obj2.NCAC632FINSUP_Id = data.NCAC632FINSUP_Id;
                                        obj2.NCAC632FINSUPF_ActiveFlg = true;
                                        obj2.NCAC632FINSUPF_StatusFlg = "";
                                        obj2.NCAC632FINSUPF_Remarks = "";

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
        public NAAC_AC_632_FinanceSupport_DTO deactive(NAAC_AC_632_FinanceSupport_DTO data)
        {
            try
            {
                var u = _GeneralContext.NAAC_AC_632_FinanceSupport_DMO.Where(t => t.NCAC632FINSUP_Id == data.NCAC632FINSUP_Id).SingleOrDefault();
                if (u.NCAC632FINSUP_ActiveFlg == true)
                {
                    u.NCAC632FINSUP_ActiveFlg = false;
                }
                else if (u.NCAC632FINSUP_ActiveFlg == false)
                {
                    u.NCAC632FINSUP_ActiveFlg = true;
                }
                u.NCAC632FINSUP_UpdatedDate = DateTime.Now;
                u.NCAC632FINSUP_UpdatedBy = data.UserId;
                u.MI_Id = data.MI_Id;
                _GeneralContext.Update(u);
                int o = _GeneralContext.SaveChanges();
                if (o > 0)
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
        public NAAC_AC_632_FinanceSupport_DTO EditData(NAAC_AC_632_FinanceSupport_DTO data)
        {
            try
            {
                data.editlist = (from a in _GeneralContext.Academic
                                 from b in _GeneralContext.NAAC_AC_632_FinanceSupport_DMO
                                 where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCAC632FINSUP_Year && b.MI_Id == data.MI_Id && b.NCAC632FINSUP_Id == data.NCAC632FINSUP_Id && b.NCAC632FINSUP_ActiveFlg == true)
                                 select new NAAC_AC_632_FinanceSupport_DTO
                                 {
                                     NCAC632FINSUP_Id = b.NCAC632FINSUP_Id,
                                     NCAC632FINSUP_TeacherName = b.NCAC632FINSUP_TeacherName,
                                     NCAC632FINSUP_PAN = b.NCAC632FINSUP_PAN,
                                     NCAC632FINSUP_NameOfMembership = b.NCAC632FINSUP_NameOfMembership,
                                     MI_Id = data.MI_Id,
                                     NCAC632FINSUP_Name = b.NCAC632FINSUP_Name,
                                     NCAC632FINSUP_Year = b.NCAC632FINSUP_Year,
                                     NCAC632FINSUP_AmountPaid = b.NCAC632FINSUP_AmountPaid,
                                     NCAC632FINSUP_ConferenceProfBodyFlg = b.NCAC632FINSUP_ConferenceProfBodyFlg,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();

                data.editFileslist = (from a in _GeneralContext.NAAC_AC_632_FinanceSupport_Files_DMO
                                      where (a.NCAC632FINSUP_Id == data.NCAC632FINSUP_Id)
                                      select new NAAC_AC_632_FinanceSupport_DTO
                                      {
                                          cfilename = a.NCAC632FINSUPF_FileName,
                                          cfilepath = a.NCAC632FINSUPF_FilePath,
                                          cfiledesc = a.NCAC632FINSUPF_Filedesc,
                                          cfileid = a.NCAC632FINSUPF_Id,
                                      }).Distinct().ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAAC_AC_632_FinanceSupport_DTO viewuploadflies(NAAC_AC_632_FinanceSupport_DTO data)
        {
            try
            {
                data.viewuploadflies = (from a in _GeneralContext.NAAC_AC_632_FinanceSupport_Files_DMO
                                        where (a.NCAC632FINSUP_Id == data.NCAC632FINSUP_Id && a.NCAC632FINSUPF_ActiveFlg == true)
                                        select new NAAC_AC_632_FinanceSupport_DTO
                                        {
                                            cfilename = a.NCAC632FINSUPF_FileName,
                                            cfilepath = a.NCAC632FINSUPF_FilePath,
                                            cfiledesc = a.NCAC632FINSUPF_Filedesc,
                                            NCAC632FINSUPF_Id = a.NCAC632FINSUPF_Id,
                                            NCAC632FINSUP_Id = a.NCAC632FINSUP_Id,
                                            NCAC632FINSUPF_StatusFlg = a.NCAC632FINSUPF_StatusFlg,
                                            NCAC632FINSUPF_ApprovedFlg = a.NCAC632FINSUPF_ApprovedFlg,
                                        }).Distinct().ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;

        }
        public NAAC_AC_632_FinanceSupport_DTO deleteuploadfile(NAAC_AC_632_FinanceSupport_DTO data)
        {
            try
            {
                var res = _GeneralContext.NAAC_AC_632_FinanceSupport_Files_DMO.Where(t => t.NCAC632FINSUPF_Id == data.NCAC632FINSUPF_Id).SingleOrDefault();
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
                data.viewuploadflies = (from a in _GeneralContext.NAAC_AC_632_FinanceSupport_Files_DMO
                                        where (a.NCAC632FINSUP_Id == data.NCAC632FINSUP_Id)
                                        select new NAAC_AC_632_FinanceSupport_DTO
                                        {
                                            cfilename = a.NCAC632FINSUPF_FileName,
                                            cfilepath = a.NCAC632FINSUPF_FilePath,
                                            cfiledesc = a.NCAC632FINSUPF_Filedesc,
                                            NCAC632FINSUPF_Id = a.NCAC632FINSUPF_Id,
                                            NCAC632FINSUP_Id = a.NCAC632FINSUP_Id,
                                            NCAC632FINSUPF_StatusFlg = a.NCAC632FINSUPF_StatusFlg,
                                            NCAC632FINSUPF_ApprovedFlg = a.NCAC632FINSUPF_ApprovedFlg,
                                        }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }



        //add row wise comments
        public NAAC_AC_632_FinanceSupport_DTO savemedicaldatawisecomments(NAAC_AC_632_FinanceSupport_DTO data)
        {
            try
            {
                NAAC_AC_632_FinanceSupport_Comments_DMO obj1 = new NAAC_AC_632_FinanceSupport_Comments_DMO();

                obj1.NCAC632FINSUPC_Remarks = data.Remarks;
                obj1.NCAC632FINSUPC_RemarksBy = data.UserId;
                obj1.NCAC632FINSUPC_StatusFlg = "";
                obj1.NCAC632FINSUP_Id = data.filefkid;
                obj1.NCAC632FINSUPC_ActiveFlag = true;
                obj1.NCAC632FINSUPC_CreatedBy = data.UserId;
                obj1.NCAC632FINSUPC_UpdatedBy = data.UserId;
                obj1.NCAC632FINSUPC_CreatedDate = DateTime.Now;
                obj1.NCAC632FINSUPC_UpdatedDate = DateTime.Now;

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
        public NAAC_AC_632_FinanceSupport_DTO savefilewisecomments(NAAC_AC_632_FinanceSupport_DTO data)
        {
            try
            {
                NAAC_AC_632_FinanceSupport_File_Comments_DMO obj1 = new NAAC_AC_632_FinanceSupport_File_Comments_DMO();

                obj1.NCAC632FINSUPFC_Remarks = data.Remarks;
                obj1.NCAC632FINSUPFC_RemarksBy = data.UserId;
                obj1.NCAC632FINSUPFC_StatusFlg = "";
                obj1.NCAC632FINSUPF_Id = data.filefkid;
                obj1.NCAC632FINSUPFC_ActiveFlag = true;
                obj1.NCAC632FINSUPFC_CreatedBy = data.UserId;
                obj1.NCAC632FINSUPFC_UpdatedBy = data.UserId;
                obj1.NCAC632FINSUPFC_CreatedDate = DateTime.Now;
                obj1.NCAC632FINSUPFC_UpdatedDate = DateTime.Now;

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
        public NAAC_AC_632_FinanceSupport_DTO getcomment(NAAC_AC_632_FinanceSupport_DTO data)
        {
            try
            {
                data.commentlist = (from a in _GeneralContext.NAAC_AC_632_FinanceSupport_Comments_DMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCAC632FINSUPC_CreatedBy == b.Id && a.NCAC632FINSUP_Id == data.NCAC632FINSUP_Id)
                                    select new NAAC_AC_632_FinanceSupport_DTO
                                    {
                                        NCAC632FINSUPC_Remarks = a.NCAC632FINSUPC_Remarks,
                                        NCAC632FINSUPC_Id = a.NCAC632FINSUPC_Id,
                                        NCAC632FINSUP_Id = a.NCAC632FINSUP_Id,
                                        NCAC632FINSUPC_RemarksBy = a.NCAC632FINSUPC_RemarksBy,
                                        NCAC632FINSUPC_StatusFlg = a.NCAC632FINSUPC_StatusFlg,
                                        NCAC632FINSUPC_ActiveFlag = a.NCAC632FINSUPC_ActiveFlag,
                                        NCAC632FINSUPC_CreatedBy = a.NCAC632FINSUPC_CreatedBy,
                                        NCAC632FINSUPC_CreatedDate = a.NCAC632FINSUPC_CreatedDate,
                                        NCAC632FINSUPC_UpdatedBy = a.NCAC632FINSUPC_UpdatedBy,
                                        NCAC632FINSUPC_UpdatedDate = a.NCAC632FINSUPC_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NCAC632FINSUPC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // view file wise comments
        public NAAC_AC_632_FinanceSupport_DTO getfilecomment(NAAC_AC_632_FinanceSupport_DTO data)
        {
            try
            {
                data.commentlist1 = (from a in _GeneralContext.NAAC_AC_632_FinanceSupport_File_Comments_DMO
                                     from b in _GeneralContext.ApplUser
                                     where (a.NCAC632FINSUPFC_RemarksBy == b.Id && a.NCAC632FINSUPF_Id == data.NCAC632FINSUPF_Id)
                                     select new NAAC_AC_632_FinanceSupport_DTO
                                     {
                                         NCAC632FINSUPFC_Id = a.NCAC632FINSUPFC_Id,
                                         NCAC632FINSUPFC_Remarks = a.NCAC632FINSUPFC_Remarks,
                                         NCAC632FINSUPF_Id = a.NCAC632FINSUPF_Id,
                                         NCAC632FINSUPFC_RemarksBy = a.NCAC632FINSUPFC_RemarksBy,
                                         NCAC632FINSUPFC_StatusFlg = a.NCAC632FINSUPFC_StatusFlg,
                                         NCAC632FINSUPFC_ActiveFlag = a.NCAC632FINSUPFC_ActiveFlag,
                                         NCAC632FINSUPFC_CreatedBy = a.NCAC632FINSUPFC_CreatedBy,
                                         NCAC632FINSUPFC_CreatedDate = a.NCAC632FINSUPFC_CreatedDate,
                                         NCAC632FINSUPFC_UpdatedBy = a.NCAC632FINSUPFC_UpdatedBy,
                                         NCAC632FINSUPFC_UpdatedDate = a.NCAC632FINSUPFC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().OrderByDescending(a => a.NCAC632FINSUPFC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}
