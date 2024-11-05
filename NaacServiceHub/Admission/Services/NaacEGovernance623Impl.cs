using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class NaacEGovernance623Impl : Interface.NaacEGovernance623Interface
    {
        public GeneralContext _GeneralContext;
        public NaacEGovernance623Impl(GeneralContext y)
        {
            _GeneralContext = y;
        }

        public NAAC_AC_623_EGovernance_DTO loaddata(NAAC_AC_623_EGovernance_DTO data)
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

                data.allacademicyear = _GeneralContext.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Id).ToArray();
                data.alldata1 = (from a in _GeneralContext.Academic
                                 from b in _GeneralContext.NAAC_AC_623_EGovernance_DMO
                                 where (a.MI_Id == data.MI_Id && b.MI_Id == a.MI_Id && a.Is_Active == true && b.NCAC623EGOV_ImpYear == a.ASMAY_Id)
                                 select new NAAC_AC_623_EGovernance_DTO
                                 {
                                     NCAC623EGOV_Id = b.NCAC623EGOV_Id,
                                     NCAC623EGOV_ImpYear = b.NCAC623EGOV_ImpYear,
                                     ASMAY_Year = a.ASMAY_Year,
                                     NCAC623EGOV_GovernanceArea = b.NCAC623EGOV_GovernanceArea,
                                     NCAC623EGOV_VendorName = b.NCAC623EGOV_VendorName,
                                     NCAC623EGOV_VendorAddress = b.NCAC623EGOV_VendorAddress,
                                     MI_Id = data.MI_Id,
                                     NCAC623EGOV_VendorPhoneNo = b.NCAC623EGOV_VendorPhoneNo,
                                     NCAC623EGOV_VendorEmailId = b.NCAC623EGOV_VendorEmailId,
                                     NCAC623EGOV_ActiveFlg = b.NCAC623EGOV_ActiveFlg,
                                     NCAC623EGOV_StatusFlg = b.NCAC623EGOV_StatusFlg,
                                     NCAC623EGOV_ApprovedFlg = b.NCAC623EGOV_ApprovedFlg,

                                 }).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }

        public NAAC_AC_623_EGovernance_DTO save(NAAC_AC_623_EGovernance_DTO data)
        {
            try
            {
                if (data.NCAC623EGOV_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_623_EGovernance_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC623EGOV_Id != 0 && t.NCAC623EGOV_GovernanceArea == data.NCAC623EGOV_GovernanceArea && t.NCAC623EGOV_VendorName == data.NCAC623EGOV_VendorName && t.NCAC623EGOV_VendorAddress == data.NCAC623EGOV_VendorAddress && t.NCAC623EGOV_VendorPhoneNo == data.NCAC623EGOV_VendorPhoneNo && t.NCAC623EGOV_VendorEmailId == data.NCAC623EGOV_VendorEmailId).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_623_EGovernance_DMO rrr = new NAAC_AC_623_EGovernance_DMO();
                        rrr.MI_Id = data.MI_Id;
                        rrr.NCAC623EGOV_GovernanceArea = data.NCAC623EGOV_GovernanceArea;
                        rrr.NCAC623EGOV_VendorName = data.NCAC623EGOV_VendorName;
                        rrr.NCAC623EGOV_VendorAddress = data.NCAC623EGOV_VendorAddress;
                        rrr.NCAC623EGOV_VendorPhoneNo = data.NCAC623EGOV_VendorPhoneNo;
                        rrr.NCAC623EGOV_VendorEmailId = data.NCAC623EGOV_VendorEmailId;
                        rrr.NCAC623EGOV_ImpYear = data.ASMAY_Id;
                        rrr.NCAC623EGOV_CreatedDate = DateTime.Now;
                        rrr.NCAC623EGOV_UpdatedDate = DateTime.Now;
                        rrr.NCAC623EGOV_ActiveFlg = true;
                        rrr.NCAC623EGOV_CreatedBy = data.UserId;
                        rrr.NCAC623EGOV_UpdatedBy = data.UserId;
                        rrr.NCAC623EGOV_StatusFlg = "";
                        rrr.NCAC623EGOV_Remarks = "";



                        _GeneralContext.Add(rrr);

                        if (data.filelist.Length > 0)
                        {
                            for (int i = 0; i < data.filelist.Length; i++)
                            {
                                if (data.filelist[0].cfilepath != null)
                                {


                                    NAAC_AC_623_EGovernance_Files_DMO obj2 = new NAAC_AC_623_EGovernance_Files_DMO();
                                    obj2.NCAC623EGOV_Id = rrr.NCAC623EGOV_Id;

                                    obj2.NCAC623EGOVF_FileName = data.filelist[i].cfilename;
                                    obj2.NCAC623EGOVF_Filedesc = data.filelist[i].cfiledesc;
                                    obj2.NCAC623EGOVF_FilePath = data.filelist[i].cfilepath;
                                    obj2.NCAC623EGOVF_StatusFlg = "";
                                    obj2.NCAC623EGOVF_ActiveFlg = true;
                                    obj2.NCAC623EGOVF_Remarks = "";



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
                else if (data.NCAC623EGOV_Id > 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_623_EGovernance_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC623EGOV_GovernanceArea == data.NCAC623EGOV_GovernanceArea && t.NCAC623EGOV_VendorName == data.NCAC623EGOV_VendorName && t.NCAC623EGOV_VendorAddress == data.NCAC623EGOV_VendorAddress && t.NCAC623EGOV_VendorPhoneNo == data.NCAC623EGOV_VendorPhoneNo && t.NCAC623EGOV_VendorEmailId == data.NCAC623EGOV_VendorEmailId && t.NCAC623EGOV_ImpYear == data.NCAC623EGOV_ImpYear && t.NCAC623EGOV_Id != data.NCAC623EGOV_Id).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var yy = _GeneralContext.NAAC_AC_623_EGovernance_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC623EGOV_Id == data.NCAC623EGOV_Id).SingleOrDefault();

                        yy.NCAC623EGOV_UpdatedBy = data.UserId;
                        yy.MI_Id = data.MI_Id;
                        yy.NCAC623EGOV_GovernanceArea = data.NCAC623EGOV_GovernanceArea;
                        yy.NCAC623EGOV_VendorName = data.NCAC623EGOV_VendorName;
                        yy.NCAC623EGOV_VendorAddress = data.NCAC623EGOV_VendorAddress;
                        yy.NCAC623EGOV_VendorPhoneNo = data.NCAC623EGOV_VendorPhoneNo;
                        yy.NCAC623EGOV_VendorEmailId = data.NCAC623EGOV_VendorEmailId;
                        yy.NCAC623EGOV_UpdatedDate = DateTime.Now;
                        _GeneralContext.Update(yy);



                        var CountRemoveFiles = _GeneralContext.NAAC_AC_623_EGovernance_Files_DMO.Where(b => b.NCAC623EGOV_Id == data.NCAC623EGOV_Id).ToList();

                        List<long> temparr = new List<long>();
                        //getting all mobilenumbers
                        foreach (var c in data.filelist)
                        {
                            temparr.Add(c.cfileid);
                        }


                        var Phone_Noresultremove = _GeneralContext.NAAC_AC_623_EGovernance_Files_DMO.Where(c => !temparr.Contains(c.NCAC623EGOVF_Id)
                        && c.NCAC623EGOV_Id == data.NCAC623EGOV_Id).ToList();

                        foreach (var ph1 in Phone_Noresultremove)
                        {
                            var resultremove112 = _GeneralContext.NAAC_AC_623_EGovernance_Files_DMO.Single(a => a.NCAC623EGOVF_Id == ph1.NCAC623EGOVF_Id);
                            resultremove112.NCAC623EGOVF_ActiveFlg = false;
                            _GeneralContext.Update(resultremove112);

                        }


                        if (data.filelist.Length > 0)
                        {
                            for (int k = 0; k < data.filelist.Length; k++)
                            {
                                var resultupload = _GeneralContext.NAAC_AC_623_EGovernance_Files_DMO.Where(a => a.NCAC623EGOV_Id == data.NCAC623EGOV_Id
                                && a.NCAC623EGOVF_Id == data.filelist[k].cfileid).ToList();
                                if (resultupload.Count > 0)
                                {
                                    var resultupdateupload = _GeneralContext.NAAC_AC_623_EGovernance_Files_DMO.Single(a => a.NCAC623EGOV_Id == data.NCAC623EGOV_Id
                                    && a.NCAC623EGOVF_Id == data.filelist[k].cfileid);
                                    resultupdateupload.NCAC623EGOVF_FileName = data.filelist[k].cfilename;
                                    resultupdateupload.NCAC623EGOVF_Filedesc = data.filelist[k].cfiledesc;
                                    resultupdateupload.NCAC623EGOVF_FilePath = data.filelist[k].cfilepath;
                                    _GeneralContext.Update(resultupdateupload);
                                }
                                else
                                {
                                    if (data.filelist[k].cfilepath != null && data.filelist[k].cfilepath != "")
                                    {
                                        NAAC_AC_623_EGovernance_Files_DMO obj2 = new NAAC_AC_623_EGovernance_Files_DMO();
                                        obj2.NCAC623EGOVF_FileName = data.filelist[k].cfilename;
                                        obj2.NCAC623EGOVF_Filedesc = data.filelist[k].cfiledesc;
                                        obj2.NCAC623EGOVF_FilePath = data.filelist[k].cfilepath;
                                        obj2.NCAC623EGOV_Id = data.NCAC623EGOV_Id;
                                        obj2.NCAC623EGOVF_ActiveFlg = true;
                                        obj2.NCAC623EGOVF_StatusFlg = "";
                                        obj2.NCAC623EGOVF_Remarks = "";

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
        public NAAC_AC_623_EGovernance_DTO deactive(NAAC_AC_623_EGovernance_DTO data)
        {
            try
            {
                var u = _GeneralContext.NAAC_AC_623_EGovernance_DMO.Where(t => t.NCAC623EGOV_Id == data.NCAC623EGOV_Id).SingleOrDefault();
                if (u.NCAC623EGOV_ActiveFlg == true)
                {
                    u.NCAC623EGOV_ActiveFlg = false;
                }
                else if (u.NCAC623EGOV_ActiveFlg == false)
                {
                    u.NCAC623EGOV_ActiveFlg = true;
                }
                u.NCAC623EGOV_UpdatedDate = DateTime.Now;
                u.NCAC623EGOV_UpdatedBy = data.UserId;
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
        public NAAC_AC_623_EGovernance_DTO EditData(NAAC_AC_623_EGovernance_DTO data)
        {
            try
            {
                data.editlist = (from a in _GeneralContext.Academic
                                 from b in _GeneralContext.NAAC_AC_623_EGovernance_DMO
                                 where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCAC623EGOV_ImpYear && b.MI_Id == data.MI_Id && b.NCAC623EGOV_Id == data.NCAC623EGOV_Id)
                                 select new NAAC_AC_623_EGovernance_DTO
                                 {
                                     NCAC623EGOV_Id = b.NCAC623EGOV_Id,
                                     NCAC623EGOV_GovernanceArea = b.NCAC623EGOV_GovernanceArea,
                                     NCAC623EGOV_ImpYear = b.NCAC623EGOV_ImpYear,
                                     NCAC623EGOV_VendorName = b.NCAC623EGOV_VendorName,
                                     NCAC623EGOV_VendorAddress = b.NCAC623EGOV_VendorAddress,
                                     NCAC623EGOV_VendorPhoneNo = b.NCAC623EGOV_VendorPhoneNo,

                                     NCAC623EGOV_VendorEmailId = b.NCAC623EGOV_VendorEmailId,
                                     MI_Id = data.MI_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();


                data.editFileslist = (from a in _GeneralContext.NAAC_AC_623_EGovernance_Files_DMO
                                      where (a.NCAC623EGOV_Id == data.NCAC623EGOV_Id && a.NCAC623EGOVF_ActiveFlg == true)
                                      select new NAAC_AC_623_EGovernance_DTO
                                      {
                                          cfilename = a.NCAC623EGOVF_FileName,
                                          cfilepath = a.NCAC623EGOVF_FilePath,
                                          cfiledesc = a.NCAC623EGOVF_Filedesc,
                                          cfileid = a.NCAC623EGOVF_Id,
                                      }).Distinct().ToArray();
            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }


        public NAAC_AC_623_EGovernance_DTO viewuploadflies(NAAC_AC_623_EGovernance_DTO data)
        {
            try
            {
                data.viewuploadflies = (from a in _GeneralContext.NAAC_AC_623_EGovernance_Files_DMO
                                        where (a.NCAC623EGOV_Id == data.NCAC623EGOV_Id && a.NCAC623EGOVF_ActiveFlg == true)
                                        select new NAAC_AC_623_EGovernance_DTO
                                        {
                                            cfilename = a.NCAC623EGOVF_FileName,
                                            cfilepath = a.NCAC623EGOVF_FilePath,
                                            cfiledesc = a.NCAC623EGOVF_Filedesc,
                                            NCAC623EGOVF_Id = a.NCAC623EGOVF_Id,
                                            NCAC623EGOV_Id = a.NCAC623EGOV_Id,
                                            NCAC623EGOVF_StatusFlg = a.NCAC623EGOVF_StatusFlg,
                                            NCAC623EGOVF_ApprovedFlg = a.NCAC623EGOVF_ApprovedFlg,
                                        }).Distinct().ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;

        }
        public NAAC_AC_623_EGovernance_DTO deleteuploadfile(NAAC_AC_623_EGovernance_DTO data)
        {
            try
            {
                var res = _GeneralContext.NAAC_AC_623_EGovernance_Files_DMO.Where(t => t.NCAC623EGOVF_Id == data.NCAC623EGOVF_Id).SingleOrDefault();
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
                data.viewuploadflies = (from a in _GeneralContext.NAAC_AC_623_EGovernance_Files_DMO
                                        where (a.NCAC623EGOV_Id == data.NCAC623EGOV_Id)
                                        select new NAAC_AC_623_EGovernance_DTO
                                        {
                                            cfilename = a.NCAC623EGOVF_FileName,
                                            cfilepath = a.NCAC623EGOVF_FilePath,
                                            cfiledesc = a.NCAC623EGOVF_Filedesc,
                                            NCAC623EGOVF_Id = a.NCAC623EGOVF_Id,
                                            NCAC623EGOV_Id = a.NCAC623EGOV_Id,
                                        }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


        //add row wise comments
        public NAAC_AC_623_EGovernance_DTO savemedicaldatawisecomments(NAAC_AC_623_EGovernance_DTO data)
        {
            try
            {
                NAAC_AC_623_EGovernance_Comments_DMO obj1 = new NAAC_AC_623_EGovernance_Comments_DMO();

                obj1.NCAC623EGOVC_Remarks = data.Remarks;
                obj1.NCAC623EGOVC_RemarksBy = data.UserId;
                obj1.NCAC623EGOVC_StatusFlg = "";
                obj1.NCAC623EGOV_Id = data.filefkid;
                obj1.NCAC623EGOVC_ActiveFlag = true;
                obj1.NCAC623EGOVC_CreatedBy = data.UserId;
                obj1.NCAC623EGOVC_UpdatedBy = data.UserId;
                obj1.NCAC623EGOVC_CreatedDate = DateTime.Now;
                obj1.NCAC623EGOVC_UpdatedDate = DateTime.Now;

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
        public NAAC_AC_623_EGovernance_DTO savefilewisecomments(NAAC_AC_623_EGovernance_DTO data)
        {
            try
            {
                NAAC_AC_623_EGovernance_File_Comments_DMO obj1 = new NAAC_AC_623_EGovernance_File_Comments_DMO();

                obj1.NCAC623EGOVFC_Remarks = data.Remarks;
                obj1.NCAC623EGOVFC_RemarksBy = data.UserId;
                obj1.NCAC623EGOVFC_StatusFlg = "";
                obj1.NCAC623EGOVF_Id = data.filefkid;
                obj1.NCAC623EGOVFC_ActiveFlag = true;
                obj1.NCAC623EGOVFC_CreatedBy = data.UserId;
                obj1.NCAC623EGOVFC_UpdatedBy = data.UserId;
                obj1.NCAC623EGOVFC_CreatedDate = DateTime.Now;
                obj1.NCAC623EGOVFC_UpdatedDate = DateTime.Now;

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
        public NAAC_AC_623_EGovernance_DTO getcomment(NAAC_AC_623_EGovernance_DTO data)
        {
            try
            {
                data.commentlist = (from a in _GeneralContext.NAAC_AC_623_EGovernance_Comments_DMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCAC623EGOVC_CreatedBy == b.Id && a.NCAC623EGOV_Id == data.NCAC623EGOV_Id)
                                    select new NAAC_AC_623_EGovernance_DTO
                                    {
                                        NCAC623EGOVC_Remarks = a.NCAC623EGOVC_Remarks,
                                        NCAC623EGOVC_Id = a.NCAC623EGOVC_Id,
                                        NCAC623EGOV_Id = a.NCAC623EGOV_Id,
                                        NCAC623EGOVC_RemarksBy = a.NCAC623EGOVC_RemarksBy,
                                        NCAC623EGOVC_StatusFlg = a.NCAC623EGOVC_StatusFlg,
                                        NCAC623EGOVC_ActiveFlag = a.NCAC623EGOVC_ActiveFlag,
                                        NCAC623EGOVC_CreatedBy = a.NCAC623EGOVC_CreatedBy,
                                        NCAC623EGOVC_CreatedDate = a.NCAC623EGOVC_CreatedDate,
                                        NCAC623EGOVC_UpdatedBy = a.NCAC623EGOVC_UpdatedBy,
                                        NCAC623EGOVC_UpdatedDate = a.NCAC623EGOVC_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NCAC623EGOVC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // view file wise comments
        public NAAC_AC_623_EGovernance_DTO getfilecomment(NAAC_AC_623_EGovernance_DTO data)
        {
            try
            {
                data.commentlist1 = (from a in _GeneralContext.NAAC_AC_623_EGovernance_File_Comments_DMO
                                     from b in _GeneralContext.ApplUser
                                     where (a.NCAC623EGOVFC_RemarksBy == b.Id && a.NCAC623EGOVF_Id == data.NCAC623EGOVF_Id)
                                     select new NAAC_AC_623_EGovernance_DTO
                                     {
                                         NCAC623EGOVFC_Id = a.NCAC623EGOVFC_Id,
                                         NCAC623EGOVFC_Remarks = a.NCAC623EGOVFC_Remarks,
                                         NCAC623EGOVF_Id = a.NCAC623EGOVF_Id,
                                         NCAC623EGOVFC_RemarksBy = a.NCAC623EGOVFC_RemarksBy,
                                         NCAC623EGOVFC_StatusFlg = a.NCAC623EGOVFC_StatusFlg,
                                         NCAC623EGOVFC_ActiveFlag = a.NCAC623EGOVFC_ActiveFlag,
                                         NCAC623EGOVFC_CreatedBy = a.NCAC623EGOVFC_CreatedBy,
                                         NCAC623EGOVFC_CreatedDate = a.NCAC623EGOVFC_CreatedDate,
                                         NCAC623EGOVFC_UpdatedBy = a.NCAC623EGOVFC_UpdatedBy,
                                         NCAC623EGOVFC_UpdatedDate = a.NCAC623EGOVFC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().OrderByDescending(a => a.NCAC623EGOVFC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


    }
}
