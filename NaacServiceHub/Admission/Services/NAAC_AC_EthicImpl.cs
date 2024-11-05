using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class NAAC_AC_EthicImpl : Interface.NAAC_AC_EthicInterface
    {

        public GeneralContext _context;
        public NAAC_AC_EthicImpl(GeneralContext w)
        {
            _context = w;
        }
        public NAAC_AC_331_DTO loaddata(NAAC_AC_331_DTO data)
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

                data.alldata1 = (from a in _context.NAAC_AC_331_DMO
                                 where (a.MI_Id == data.MI_Id)
                                 select new NAAC_AC_331_DTO
                                 {
                                     NCAC331_Id = a.NCAC331_Id,
                                     NCAC331_EthicsURL = a.NCAC331_EthicsURL,
                                     NCAC331_PDMecanism = a.NCAC331_PDMecanism,
                                     NCAC331_ActiveFlg = a.NCAC331_ActiveFlg,
                                     MI_Id=a.MI_Id,
                                     NCAC331_StatusFlg = a.NCAC331_StatusFlg,
                                     
                                 }).Distinct().OrderByDescending(y => y.NCAC331_Id).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_AC_331_DTO save(NAAC_AC_331_DTO data)
        {
            try
            {
                if (data.NCAC331_Id == 0)
                {
                    var duplicate = _context.NAAC_AC_331_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC331_Id != 0 && t.NCAC331_EthicsURL == data.NCAC331_EthicsURL && t.NCAC331_PDMecanism == data.NCAC331_PDMecanism).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_331_DMO rr = new NAAC_AC_331_DMO();
                        rr.MI_Id = data.MI_Id;
                        rr.NCAC331_CreatedDate = DateTime.Now;
                        rr.NCAC331_UpdatedDate = DateTime.Now;
                        rr.NCAC331_ActiveFlg = true;
                        rr.NCAC331_PDSFlg = data.NCAC331_PDSFlg;
                        rr.NCAC331_CreatedBy = data.UserId;
                        rr.NCAC331_UpdatedBy = data.UserId;
                        rr.MI_Id = data.MI_Id;
                        rr.NCAC331_StatusFlg = "";
                        rr.NCAC331_EthicsURL = data.NCAC331_EthicsURL;
                        if (data.NCAC331_PDSFlg == true)
                        {
                            rr.NCAC331_PDMecanism = data.NCAC331_PDMecanism;
                        }
                        rr.NCAC331_ActiveFlg = true;
                        _context.Add(rr);
                        if (data.filelist.Length > 0)
                        {
                            for (int i = 0; i < data.filelist.Length; i++)
                            {
                                if (data.filelist[i].cfilepath != null)
                                {
                                    NAAC_AC_331_Files_DMO obj2 = new NAAC_AC_331_Files_DMO();

                                    obj2.NCAC331_Id = rr.NCAC331_Id;
                                    obj2.NCAC331F_FileName = data.filelist[i].cfilename;
                                    obj2.NCAC331F_Filedesc = data.filelist[i].cfiledesc;
                                    obj2.NCAC331F_FilePath = data.filelist[i].cfilepath;
                                    obj2.NCAC331F_StatusFlg = "";
                                    obj2.NCAC331F_ActiveFlg = true;
                                    _context.Add(obj2);
                                }
                            }
                        }
                        int y = _context.SaveChanges();
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
                else if (data.NCAC331_Id > 0)
                {
                    var duplicate = _context.NAAC_AC_331_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC331_EthicsURL == data.NCAC331_EthicsURL && t.NCAC331_PDMecanism == data.NCAC331_PDMecanism && t.NCAC331_Id != data.NCAC331_Id).ToArray();

                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var yy = _context.NAAC_AC_331_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC331_Id == data.NCAC331_Id).SingleOrDefault();

                        yy.NCAC331_CreatedDate = DateTime.Now;
                        yy.NCAC331_UpdatedDate = DateTime.Now;
                        yy.NCAC331_PDSFlg = data.NCAC331_PDSFlg;
                        yy.NCAC331_UpdatedBy = data.UserId;
                        yy.NCAC331_PDMecanism = data.NCAC331_PDMecanism;
                        yy.NCAC331_EthicsURL = data.NCAC331_EthicsURL;
                        yy.MI_Id = data.MI_Id;
                        _context.Update(yy);
                        if (data.filelist.Count() > 0)
                        {

                            List<long> Fid = new List<long>();
                            foreach (var item in data.filelist)
                            {
                                Fid.Add(item.NCAC331F_Id);
                            }
                            var removefile11 = _context.NAAC_AC_331_Files_DMO.Where(t => t.NCAC331_Id == data.NCAC331_Id && !Fid.Contains(t.NCAC331F_Id)).Distinct().ToList();

                            if (removefile11.Count > 0)
                            {
                                foreach (var item2 in removefile11)
                                {
                                    var deactfile = _context.NAAC_AC_331_Files_DMO.Single(t => t.NCAC331_Id == data.NCAC331_Id && t.NCAC331F_Id == item2.NCAC331F_Id);
                                    deactfile.NCAC331F_ActiveFlg = false;
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

                            foreach (NAAC_AC_331_DTO DocumentsDTO in data.filelist)
                            {

                                if (DocumentsDTO.NCAC331F_Id > 0 && DocumentsDTO.NCAC331F_StatusFlg != "approved")
                                {
                                    if (DocumentsDTO.cfilepath != null)
                                    {

                                        var filesdata = _context.NAAC_AC_331_Files_DMO.Where(t => t.NCAC331F_Id == DocumentsDTO.NCAC331F_Id).FirstOrDefault();
                                        filesdata.NCAC331F_Filedesc = DocumentsDTO.cfiledesc;
                                        filesdata.NCAC331F_FileName = DocumentsDTO.cfilename;
                                        filesdata.NCAC331F_FilePath = DocumentsDTO.cfilepath;


                                        _context.Update(filesdata);
                                       
                                    }
                                }
                                else
                                {

                                    if (DocumentsDTO.NCAC331F_Id == 0)
                                    {
                                        if (DocumentsDTO.cfilepath != null)
                                        {
                                            NAAC_AC_331_Files_DMO obj2 = new NAAC_AC_331_Files_DMO();
                                            obj2.NCAC331F_FileName = DocumentsDTO.cfilename;
                                            obj2.NCAC331F_Filedesc = DocumentsDTO.cfiledesc;
                                            obj2.NCAC331F_FilePath = DocumentsDTO.cfilepath;
                                            obj2.NCAC331F_StatusFlg = "";
                                            obj2.NCAC331F_ActiveFlg = true;
                                            obj2.NCAC331_Id = data.NCAC331_Id;
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
        public NAAC_AC_331_DTO deactive(NAAC_AC_331_DTO data)
        {
            try
            {
                var u = _context.NAAC_AC_331_DMO.Where(t => t.NCAC331_Id == data.NCAC331_Id).SingleOrDefault();

                if (u.NCAC331_ActiveFlg == true)
                {
                    u.NCAC331_ActiveFlg = false;
                }
                else if (u.NCAC331_ActiveFlg == false)
                {
                    u.NCAC331_ActiveFlg = true;
                }
                u.NCAC331_UpdatedDate = DateTime.Now;
                u.NCAC331_UpdatedBy = data.UserId;
                u.MI_Id = data.MI_Id;
                _context.Update(u);
                int o = _context.SaveChanges();
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
        public NAAC_AC_331_DTO EditData(NAAC_AC_331_DTO data)
        {
            try
            {
                data.editlist = (from a in _context.NAAC_AC_331_DMO
                                 where (a.MI_Id == data.MI_Id && a.NCAC331_Id == data.NCAC331_Id)
                                 select new NAAC_AC_331_DTO
                                 {
                                     NCAC331_Id = a.NCAC331_Id,
                                     NCAC331_PDMecanism = a.NCAC331_PDMecanism,
                                     NCAC331_EthicsURL = a.NCAC331_EthicsURL,
                                     MI_Id=a.MI_Id,
                                     NCAC331_PDSFlg = a.NCAC331_PDSFlg,
                                     NCAC331_ActiveFlg = a.NCAC331_ActiveFlg,
                                     NCAC331_StatusFlg = a.NCAC331_StatusFlg,

                                 }).Distinct().ToArray();


                data.editFileslist = (from a in _context.NAAC_AC_331_Files_DMO
                                      where (a.NCAC331_Id == data.NCAC331_Id&&a.NCAC331F_ActiveFlg==true)
                                      select new NAAC_AC_331_DTO
                                      {
                                          cfilename = a.NCAC331F_FileName,
                                          cfilepath = a.NCAC331F_FilePath,
                                          cfiledesc = a.NCAC331F_Filedesc,
                                          NCAC331F_StatusFlg = a.NCAC331F_StatusFlg,
                                          NCAC331F_Id = a.NCAC331F_Id,
                                          NCAC331_Id = a.NCAC331_Id,
                                      }).Distinct().ToArray();
            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }

        public NAAC_AC_331_DTO getcomment(NAAC_AC_331_DTO data)
        {
            try
            {
                data.commentlist = (from a in _context.NAAC_AC_331_Comments_DMO
                                    from b in _context.ApplUser
                                    where (a.NCAC331C_RemarksBy == b.Id && a.NCAC331_Id == data.NCAC331_Id)
                                    select new NAAC_AC_331_DTO
                                    {
                                        NCAC331C_Remarks = a.NCAC331C_Remarks,
                                        NCAC331C_Id = a.NCAC331C_Id,
                                        NCAC331C_RemarksBy = a.NCAC331C_RemarksBy,
                                        NCAC331C_StatusFlg = a.NCAC331C_StatusFlg,
                                        NCAC331C_ActiveFlag = a.NCAC331C_ActiveFlag,
                                        NCAC331C_CreatedBy = a.NCAC331C_CreatedBy,
                                        NCAC331C_CreatedDate = a.NCAC331C_CreatedDate,
                                        NCAC331C_UpdatedBy = a.NCAC331C_UpdatedBy,
                                        NCAC331C_UpdatedDate = a.NCAC331C_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NCAC331C_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        // for file
        public NAAC_AC_331_DTO getfilecomment(NAAC_AC_331_DTO data)
        {
            try
            {
                data.commentlist1 = (from a in _context.NAAC_AC_331_File_Comments_DMO
                                     from b in _context.ApplUser
                                     where (a.NCAC331FC_RemarksBy == b.Id && a.NCAC331F_Id == data.NCAC331F_Id)
                                     select new NAAC_AC_331_DTO
                                     {
                                         NCAC331F_Id = a.NCAC331F_Id,
                                         NCAC331FC_Remarks = a.NCAC331FC_Remarks,
                                         NCAC331FC_Id = a.NCAC331FC_Id,
                                         NCAC331FC_RemarksBy = a.NCAC331FC_RemarksBy,
                                         NCAC331FC_StatusFlg = a.NCAC331FC_StatusFlg,
                                         NCAC331FC_ActiveFlag = a.NCAC331FC_ActiveFlag,
                                         NCAC331FC_CreatedBy = a.NCAC331FC_CreatedBy,
                                         NCAC331FC_CreatedDate = a.NCAC331FC_CreatedDate,
                                         NCAC331FC_UpdatedBy = a.NCAC331FC_UpdatedBy,
                                         NCAC331FC_UpdatedDate = a.NCAC331FC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().OrderByDescending(a => a.NCAC331FC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_331_DTO savemedicaldatawisecomments(NAAC_AC_331_DTO data)
        {
            try
            {
                NAAC_AC_331_Comments_DMO obj1 = new NAAC_AC_331_Comments_DMO();
                obj1.NCAC331C_Remarks = data.Remarks;
                obj1.NCAC331C_RemarksBy = data.UserId;
                obj1.NCAC331C_StatusFlg = "";
                obj1.NCAC331_Id = data.filefkid;
                obj1.NCAC331C_ActiveFlag = true;
                obj1.NCAC331C_CreatedBy = data.UserId;
                obj1.NCAC331C_UpdatedBy = data.UserId;
                obj1.NCAC331C_CreatedDate = DateTime.Now;
                obj1.NCAC331C_UpdatedDate = DateTime.Now;
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
        public NAAC_AC_331_DTO savefilewisecomments(NAAC_AC_331_DTO data)
        {
            try
            {
                NAAC_AC_331_File_Comments_DMO obj1 = new NAAC_AC_331_File_Comments_DMO();
                obj1.NCAC331FC_Remarks = data.Remarks;
                obj1.NCAC331FC_RemarksBy = data.UserId;
                obj1.NCAC331FC_StatusFlg = "";
                obj1.NCAC331F_Id = data.filefkid;
                obj1.NCAC331FC_ActiveFlag = true;
                obj1.NCAC331FC_CreatedBy = data.UserId;
                obj1.NCAC331FC_UpdatedBy = data.UserId;
                obj1.NCAC331FC_UpdatedDate = DateTime.Now;
                obj1.NCAC331FC_CreatedDate = DateTime.Now;
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

        public NAAC_AC_331_DTO viewuploadflies(NAAC_AC_331_DTO data)
        {
            try
            {
                data.viewuploadflies = (from a in _context.NAAC_AC_331_Files_DMO
                                        where (a.NCAC331_Id == data.NCAC331_Id&&a.NCAC331F_ActiveFlg==true)
                                        select new NAAC_AC_331_DTO
                                        {
                                            cfilename = a.NCAC331F_FileName,
                                            cfilepath = a.NCAC331F_FilePath,
                                            cfiledesc = a.NCAC331F_Filedesc,
                                            NCAC331F_Id = a.NCAC331F_Id,
                                            NCAC331_Id = a.NCAC331_Id,
                                            NCAC331F_StatusFlg = a.NCAC331F_StatusFlg,
                                        }).Distinct().ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;

        }
        public NAAC_AC_331_DTO deleteuploadfile(NAAC_AC_331_DTO data)
        {
            try
            {
                var res = _context.NAAC_AC_331_Files_DMO.Where(t => t.NCAC331F_Id == data.NCAC331F_Id).SingleOrDefault();
                res.NCAC331F_ActiveFlg = false;
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
                data.viewuploadflies = (from a in _context.NAAC_AC_331_Files_DMO
                                        where (a.NCAC331_Id == data.NCAC331_Id&&a.NCAC331F_ActiveFlg==true)
                                        select new NAAC_AC_331_DTO
                                        {
                                            cfilename = a.NCAC331F_FileName,
                                            cfilepath = a.NCAC331F_FilePath,
                                            cfiledesc = a.NCAC331F_Filedesc,
                                            NCAC331F_Id = a.NCAC331F_Id,
                                            NCAC331_Id = a.NCAC331_Id,
                                            NCAC331F_StatusFlg = a.NCAC331F_StatusFlg,
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
