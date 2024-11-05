using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class NAAC_AC_AwardsImpl : Interface.NAAC_AC_AwardsInterface
    {
        public GeneralContext _context;
        public NAAC_AC_AwardsImpl(GeneralContext w)
        {
            _context = w;
        }
        public NAAC_AC_Awards_342_DTO loaddata(NAAC_AC_Awards_342_DTO data)
        {
            try
            {

                var institutionlist = (from a in _context.Institution
                                       from b in _context.UserRoleWithInstituteDMO
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

                data.allacademicyear = _context.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Year).ToArray();
                data.alldata1 = (from a in _context.Academic
                                 from b in _context.NAAC_AC_Awards_342_DMO
                                 where (a.MI_Id == b.MI_Id && a.Is_Active == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == b.NCACAW342_AwardYear)
                                 select new NAAC_AC_Awards_342_DTO
                                 {
                                     NCACAW342_Id = b.NCACAW342_Id,
                                     NCACAW342_ActivityName = b.NCACAW342_ActivityName,
                                     NCACAW342_AwardName = b.NCACAW342_AwardName,
                                     NCACAW342_AwardingBody = b.NCACAW342_AwardingBody,
                                     NCACAW342_AwardYear = b.NCACAW342_AwardYear,
                                     NCACAW342_CategoryName = b.NCACAW342_CategoryName,
                                     NCACAW342_AgencyName = b.NCACAW342_AgencyName,
                                     NCACAW342_StatusFlg = b.NCACAW342_StatusFlg,
                                     ASMAY_Year = a.ASMAY_Year,
                                     NCACAW342_ActiveFlg = b.NCACAW342_ActiveFlg,
                                     MI_Id = b.MI_Id,
                                 }).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_AC_Awards_342_DTO save(NAAC_AC_Awards_342_DTO data)
        {
            try
            {
                if (data.NCACAW342_Id == 0)
                {
                    var duplicate = _context.NAAC_AC_Awards_342_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCACAW342_ActivityName == data.NCACAW342_ActivityName && t.NCACAW342_AwardName == data.NCACAW342_AwardName && t.NCACAW342_AwardingBody == data.NCACAW342_AwardingBody && t.NCACAW342_Id != 0 && t.NCACAW342_CategoryName == data.NCACAW342_CategoryName && t.NCACAW342_AgencyName == data.NCACAW342_AgencyName).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        NAAC_AC_Awards_342_DMO u = new NAAC_AC_Awards_342_DMO();
                        u.MI_Id = data.MI_Id;
                        u.NCACAW342_ActivityName = data.NCACAW342_ActivityName;
                        u.NCACAW342_AwardName = data.NCACAW342_AwardName;
                        u.NCACAW342_AwardingBody = data.NCACAW342_AwardingBody;
                        u.NCACAW342_AgencyName = data.NCACAW342_AgencyName;
                        u.NCACAW342_CategoryName = data.NCACAW342_CategoryName;
                        u.NCACAW342_CreatedBy = data.UserId;
                        u.NCACAW342_UpdatedBy = data.UserId;
                        u.NCACAW342_StatusFlg = "";
                        u.NCACAW342_CreatedDate = DateTime.Now;
                        u.NCACAW342_UpdatedDate = DateTime.Now;
                        u.NCACAW342_AwardYear = data.ASMAY_Id;
                        u.NCACAW342_ActiveFlg = true;
                        _context.Add(u);
                        if (data.filelist.Length > 0)
                        {
                            for (int i = 0; i < data.filelist.Length; i++)
                            {
                                if (data.filelist[i].cfilepath != null)
                                {
                                    NAAC_AC_Awards_342_Files_DMO obj2 = new NAAC_AC_Awards_342_Files_DMO();
                                    obj2.NCACAW342_Id = u.NCACAW342_Id;
                                    obj2.NCACAW342F_FileName = data.filelist[i].cfilename;
                                    obj2.NCACAW342F_Filedesc = data.filelist[i].cfiledesc;
                                    obj2.NCACAW342F_FilePath = data.filelist[i].cfilepath;
                                    obj2.NCACAW342F_StatusFlg = "";
                                    obj2.NCACAW342F_ActiveFlg = true;

                                    _context.Add(obj2);
                                }
                            }
                        }
                        var w = _context.SaveChanges();
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

                else if (data.NCACAW342_Id > 0)
                {
                    var duplicate = _context.NAAC_AC_Awards_342_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCACAW342_Id != data.NCACAW342_Id && t.NCACAW342_ActivityName == data.NCACAW342_ActivityName && t.NCACAW342_AwardName == data.NCACAW342_AwardName && t.NCACAW342_AwardingBody == data.NCACAW342_AwardingBody && t.NCACAW342_AwardYear == data.NCACAW342_AwardYear && t.NCACAW342_CategoryName == data.NCACAW342_CategoryName && t.NCACAW342_AgencyName == data.NCACAW342_AgencyName).Distinct().ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        var j = _context.NAAC_AC_Awards_342_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCACAW342_Id == data.NCACAW342_Id).SingleOrDefault();
                        j.NCACAW342_ActivityName = data.NCACAW342_ActivityName;
                        j.NCACAW342_AwardName = data.NCACAW342_AwardName;
                        j.NCACAW342_AwardingBody = data.NCACAW342_AwardingBody;
                        j.NCACAW342_AgencyName = data.NCACAW342_AgencyName;
                        j.NCACAW342_CategoryName = data.NCACAW342_CategoryName;
                        j.NCACAW342_AwardYear = data.ASMAY_Id;
                        j.MI_Id = data.MI_Id;
                        j.NCACAW342_UpdatedDate = DateTime.Now;
                        j.NCACAW342_UpdatedBy = data.UserId;
                        _context.Update(j);
                        if (data.filelist.Count() > 0)
                        {
                            List<long> Fid = new List<long>();
                            foreach (var item in data.filelist)
                            {
                                Fid.Add(item.NCACAW342F_Id);
                            }
                            var removefile11 = _context.NAAC_AC_Awards_342_Files_DMO.Where(t => t.NCACAW342_Id == data.NCACAW342_Id && !Fid.Contains(t.NCACAW342F_Id)).Distinct().ToList();

                            if (removefile11.Count > 0)
                            {
                                foreach (var item2 in removefile11)
                                {
                                    var deactfile = _context.NAAC_AC_Awards_342_Files_DMO.Single(t => t.NCACAW342_Id == data.NCACAW342_Id && t.NCACAW342F_Id == item2.NCACAW342F_Id);
                                    deactfile.NCACAW342F_ActiveFlg = false;
                                    _context.Update(deactfile);

                                }

                            }
                            //var CountRemoveFiles = _GeneralContext.NAAC_AC_413_ICT_FilesDMO.Where(t => t.NCAC413ICT_Id == data.NCAC413ICT_Id && t.NCAC413ICTF_StatusFlg != "approved").ToList();
                            //if (CountRemoveFiles.Count > 0)
                            //{
                            //    foreach (var RemoveFiles in CountRemoveFiles)
                            //    {
                            //        _GeneralContext.Remove(RemoveFiles);
                            //    }
                            //}

                            foreach (NAAC_AC_Awards_342_DTO DocumentsDTO in data.filelist)
                            {

                                if (DocumentsDTO.NCACAW342F_Id > 0 && DocumentsDTO.NCACAW342F_StatusFlg != "approved")
                                {
                                    if (DocumentsDTO.cfilepath != null)
                                    {

                                        var filesdata = _context.NAAC_AC_Awards_342_Files_DMO.Where(t => t.NCACAW342F_Id == DocumentsDTO.NCACAW342F_Id).FirstOrDefault();
                                        filesdata.NCACAW342F_Filedesc = DocumentsDTO.cfiledesc;
                                        filesdata.NCACAW342F_FileName = DocumentsDTO.cfilename;
                                        filesdata.NCACAW342F_FilePath = DocumentsDTO.cfilepath;


                                        _context.Update(filesdata);
                                        
                                    }
                                }
                                else
                                {

                                    if (DocumentsDTO.NCACAW342F_Id == 0)
                                    {
                                        if (DocumentsDTO.cfilepath != null)
                                        {
                                            NAAC_AC_Awards_342_Files_DMO obj2 = new NAAC_AC_Awards_342_Files_DMO();
                                            obj2.NCACAW342F_FileName = DocumentsDTO.cfilename;
                                            obj2.NCACAW342F_Filedesc = DocumentsDTO.cfiledesc;
                                            obj2.NCACAW342F_FilePath = DocumentsDTO.cfilepath;
                                            obj2.NCACAW342F_StatusFlg = "";
                                            obj2.NCACAW342F_ActiveFlg = true;
                                            obj2.NCACAW342_Id = data.NCACAW342_Id;
                                            _context.Add(obj2);
                                           
                                        }
                                    }
                                }
                            }
                        }
                        int flag = _context.SaveChanges();
                        if (flag > 0)
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
        public NAAC_AC_Awards_342_DTO deactive(NAAC_AC_Awards_342_DTO data)
        {
            try
            {
                var g = _context.NAAC_AC_Awards_342_DMO.Where(t => t.NCACAW342_Id == data.NCACAW342_Id).SingleOrDefault();
                if (g.NCACAW342_ActiveFlg == true)
                {
                    g.NCACAW342_ActiveFlg = false;
                }
                else
                {
                    g.NCACAW342_ActiveFlg = true;
                }
                g.NCACAW342_UpdatedDate = DateTime.Now;
                g.NCACAW342_UpdatedBy = data.UserId;
                g.MI_Id = data.MI_Id;
                _context.Update(g);
                int s = _context.SaveChanges();
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
        public NAAC_AC_Awards_342_DTO EditData(NAAC_AC_Awards_342_DTO data)
        {
            try
            {

                data.editlist = (from a in _context.Academic
                                 from b in _context.NAAC_AC_Awards_342_DMO
                                 where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && b.NCACAW342_Id == data.NCACAW342_Id && a.ASMAY_Id == b.NCACAW342_AwardYear)
                                 select new NAAC_AC_Awards_342_DTO
                                 {
                                     NCACAW342_Id = b.NCACAW342_Id,
                                     NCACAW342_ActivityName = b.NCACAW342_ActivityName,
                                     NCACAW342_AwardName = b.NCACAW342_AwardName,
                                     NCACAW342_AwardingBody = b.NCACAW342_AwardingBody,
                                     NCACAW342_AwardYear = b.NCACAW342_AwardYear,
                                     NCACAW342_CategoryName = b.NCACAW342_CategoryName,
                                     NCACAW342_AgencyName = b.NCACAW342_AgencyName,
                                     NCACAW342_StatusFlg = b.NCACAW342_StatusFlg,
                                     MI_Id = b.MI_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();

                data.editFileslist = (from a in _context.NAAC_AC_Awards_342_Files_DMO
                                      where (a.NCACAW342_Id == data.NCACAW342_Id&&a.NCACAW342F_ActiveFlg==true)
                                      select new NAAC_AC_Awards_342_DTO
                                      {
                                          cfilename = a.NCACAW342F_FileName,
                                          cfilepath = a.NCACAW342F_FilePath,
                                          cfiledesc = a.NCACAW342F_Filedesc,
                                          NCACAW342F_StatusFlg = a.NCACAW342F_StatusFlg,
                                          NCACAW342F_Id = a.NCACAW342F_Id,
                                          NCACAW342_Id = a.NCACAW342_Id,
                                      }).Distinct().ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }

        public NAAC_AC_Awards_342_DTO getcomment(NAAC_AC_Awards_342_DTO data)
        {
            try
            {
                data.commentlist = (from a in _context.NAAC_AC_Awards_342_Comments_DMO
                                    from b in _context.ApplUser
                                    where (a.NCACAW342C_RemarksBy == b.Id && a.NCACAW342_Id == data.NCACAW342_Id)
                                    select new NAAC_AC_Awards_342_DTO
                                    {
                                        NCACAW342C_Remarks = a.NCACAW342C_Remarks,
                                        NCACAW342C_Id = a.NCACAW342C_Id,
                                        NCACAW342C_RemarksBy = a.NCACAW342C_RemarksBy,
                                        NCACAW342C_StatusFlg = a.NCACAW342C_StatusFlg,
                                        NCACAW342C_ActiveFlag = a.NCACAW342C_ActiveFlag,
                                        NCACAW342C_CreatedBy = a.NCACAW342C_CreatedBy,
                                        NCACAW342C_CreatedDate = a.NCACAW342C_CreatedDate,
                                        NCACAW342C_UpdatedBy = a.NCACAW342C_UpdatedBy,
                                        NCACAW342C_UpdatedDate = a.NCACAW342C_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NCACAW342C_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        // for file
        public NAAC_AC_Awards_342_DTO getfilecomment(NAAC_AC_Awards_342_DTO data)
        {
            try
            {
                data.commentlist1 = (from a in _context.NAAC_AC_Awards_342_File_Comments_DMO
                                     from b in _context.ApplUser
                                     where (a.NCACAW342FC_RemarksBy == b.Id && a.NCACAW342F_Id == data.NCACAW342F_Id)
                                     select new NAAC_AC_Awards_342_DTO
                                     {
                                         NCACAW342F_Id = a.NCACAW342F_Id,
                                         NCACAW342FC_Remarks = a.NCACAW342FC_Remarks,
                                         NCACAW342FC_Id = a.NCACAW342FC_Id,
                                         NCACAW342FC_RemarksBy = a.NCACAW342FC_RemarksBy,
                                         NCACAW342FC_StatusFlg = a.NCACAW342FC_StatusFlg,
                                         NCACAW342FC_ActiveFlag = a.NCACAW342FC_ActiveFlag,
                                         NCACAW342FC_CreatedBy = a.NCACAW342FC_CreatedBy,
                                         NCACAW342FC_CreatedDate = a.NCACAW342FC_CreatedDate,
                                         NCACAW342FC_UpdatedBy = a.NCACAW342FC_UpdatedBy,
                                         NCACAW342FC_UpdatedDate = a.NCACAW342FC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().OrderByDescending(a => a.NCACAW342FC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_Awards_342_DTO savemedicaldatawisecomments(NAAC_AC_Awards_342_DTO data)
        {
            try
            {
                NAAC_AC_Awards_342_Comments_DMO obj1 = new NAAC_AC_Awards_342_Comments_DMO();
                obj1.NCACAW342C_Remarks = data.Remarks;
                obj1.NCACAW342C_RemarksBy = data.UserId;
                obj1.NCACAW342C_StatusFlg = "";
                obj1.NCACAW342_Id = data.filefkid;
                obj1.NCACAW342C_ActiveFlag = true;
                obj1.NCACAW342C_CreatedBy = data.UserId;
                obj1.NCACAW342C_UpdatedBy = data.UserId;
                obj1.NCACAW342C_CreatedDate = DateTime.Now;
                obj1.NCACAW342C_UpdatedDate = DateTime.Now;
                _context.Add(obj1);
                int s = _context.SaveChanges();
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

        // for file adding
        public NAAC_AC_Awards_342_DTO savefilewisecomments(NAAC_AC_Awards_342_DTO data)
        {
            try
            {
                NAAC_AC_Awards_342_File_Comments_DMO obj1 = new NAAC_AC_Awards_342_File_Comments_DMO();
                obj1.NCACAW342FC_Remarks = data.Remarks;
                obj1.NCACAW342FC_RemarksBy = data.UserId;
                obj1.NCACAW342FC_StatusFlg = "";
                obj1.NCACAW342F_Id = data.filefkid;
                obj1.NCACAW342FC_ActiveFlag = true;
                obj1.NCACAW342FC_CreatedBy = data.UserId;
                obj1.NCACAW342FC_UpdatedBy = data.UserId;
                obj1.NCACAW342FC_UpdatedDate = DateTime.Now;
                obj1.NCACAW342FC_CreatedDate = DateTime.Now;
                _context.Add(obj1);
                int s = _context.SaveChanges();
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
        public NAAC_AC_Awards_342_DTO viewuploadflies(NAAC_AC_Awards_342_DTO data)
        {
            try
            {
                data.viewuploadflies = (from a in _context.NAAC_AC_Awards_342_Files_DMO
                                        where (a.NCACAW342_Id == data.NCACAW342_Id&&a.NCACAW342F_ActiveFlg==true)
                                        select new NAAC_AC_Awards_342_DTO
                                        {
                                            cfilename = a.NCACAW342F_FileName,
                                            cfilepath = a.NCACAW342F_FilePath,
                                            cfiledesc = a.NCACAW342F_Filedesc,
                                            NCACAW342F_Id = a.NCACAW342F_Id,
                                            NCACAW342_Id = a.NCACAW342_Id,
                                            NCACAW342F_StatusFlg = a.NCACAW342F_StatusFlg,
                                        }).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;

        }
        public NAAC_AC_Awards_342_DTO deleteuploadfile(NAAC_AC_Awards_342_DTO data)
        {
            try
            {
                var res = _context.NAAC_AC_Awards_342_Files_DMO.Where(t => t.NCACAW342F_Id == data.NCACAW342F_Id).SingleOrDefault();
                res.NCACAW342F_ActiveFlg = false;
                _context.Update(res);
                int s = _context.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
                data.viewuploadflies = (from a in _context.NAAC_AC_Awards_342_Files_DMO
                                        where (a.NCACAW342_Id == data.NCACAW342_Id&&a.NCACAW342F_ActiveFlg==true)
                                        select new NAAC_AC_Awards_342_DTO
                                        {
                                            cfilename = a.NCACAW342F_FileName,
                                            cfilepath = a.NCACAW342F_FilePath,
                                            cfiledesc = a.NCACAW342F_Filedesc,
                                            NCACAW342F_Id = a.NCACAW342F_Id,
                                            NCACAW342_Id = a.NCACAW342_Id,
                                            NCACAW342F_StatusFlg = a.NCACAW342F_StatusFlg,
                                        }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
    }
}
