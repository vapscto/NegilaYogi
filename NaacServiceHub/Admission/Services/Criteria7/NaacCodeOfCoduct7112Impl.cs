using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using DomainModel.Model.NAAC.Admission.Criteria7;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services.Criteria7
{
    public class NaacCodeOfCoduct7112Impl:Interface.Criteria7.NaacCodeOfCoduct7112Interface
    {

        public GeneralContext _GeneralContext;
        public NaacCodeOfCoduct7112Impl(GeneralContext para)
        {
            _GeneralContext = para;
        }

        public async Task<NAAC_AC_7112_CodeOfCoduct_DTO> loaddata(NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            try
            {
                data.institutionlist = (from a in _GeneralContext.Institution
                                        from b in _GeneralContext.UserRoleWithInstituteDMO
                                        where (a.MI_Id == b.MI_Id && a.MI_ActiveFlag == 1 && b.Id == data.UserId)
                                        select new NAAC_AC_7112_CodeOfCoduct_DTO
                                        {
                                            MI_Id = a.MI_Id,
                                            MI_Name = a.MI_Name
                                        }).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



        //public NAAC_AC_7112_CodeOfCoduct_DTO save(NAAC_AC_7112_CodeOfCoduct_DTO data)
        //{
        //    long s = 0;
        //    try
        //    {
        //    if (data.NCAC7112CODCON_Id == 0)
        //    {
        //        var duplicate = _GeneralContext.NAAC_AC_7112_CodeOfCoductDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC7112CODCON_Year == data.NCAC7112CODCON_Year && t.NCAC7112CODCON_URL == data.NCAC7112CODCON_URL).ToList();
        //        if (duplicate.Count > 0)
        //        {
        //            data.duplicate = true;
        //        }
        //        else
        //        {


        //            NAAC_AC_7112_CodeOfCoductDMO obj1 = new NAAC_AC_7112_CodeOfCoductDMO();
        //            obj1.MI_Id = data.MI_Id;
        //            obj1.NCAC7112CODCON_Year = data.ASMAY_Id;
        //            obj1.NCAC7112CODCON_URL = data.NCAC7112CODCON_URL;

        //            obj1.NCAC7112CODCON_ActiveFlg = true;
        //            obj1.NCAC7112CODCON_CreatedBy = data.UserId;
        //            obj1.NCAC7112CODCON_UpdatedBy = data.UserId;
        //            obj1.NCAC7112CODCON_CreatedDate = DateTime.Now;
        //            obj1.NCAC7112CODCON_UpdatedDate = DateTime.Now;
        //            _GeneralContext.Add(obj1);
        //            _GeneralContext.SaveChanges();
        //            s = obj1.NCAC7112CODCON_Id;
        //            if (data.filelist.Count() > 0)
        //            {
        //                foreach (NAAC_AC_7112_CodeOfCoduct_DTO DocumentsDTO in data.filelist)
        //                {
        //                    NAAC_AC_7112_CodeOfCoduct_FilesDMO obj2 = new NAAC_AC_7112_CodeOfCoduct_FilesDMO();
        //                    obj2.NCAC7112CODCONF_FileName = DocumentsDTO.NCAC7112CODCONF_FileName;
        //                    obj2.NCAC7112CODCONF_Filedesc = DocumentsDTO.NCAC7112CODCONF_Filedesc;
        //                    obj2.NCAC7112CODCONF_FilePath = DocumentsDTO.NCAC7112CODCONF_FilePath;
        //                    obj2.NCAC7112CODCON_Id = s;
        //                    _GeneralContext.Add(obj2);
        //                    int y = _GeneralContext.SaveChanges();
        //                    if (s > 0)
        //                    {
        //                        data.returnval = true;
        //                    }
        //                    else
        //                    {
        //                        data.returnval = false;
        //                    }
        //                }
        //            }
        //        }

        //    }
        //    else if (data.NCAC7112CODCON_Id > 0)
        //    {
        //        var duplicate = _GeneralContext.NAAC_AC_7112_CodeOfCoductDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC7112CODCON_Year == data.NCAC7112CODCON_Year && t.NCAC7112CODCON_URL == data.NCAC7112CODCON_URL).ToList();
        //        if (duplicate.Count > 0)
        //        {
        //            data.duplicate = true;
        //        }
        //        else
        //        {
        //            var update = _GeneralContext.NAAC_AC_7112_CodeOfCoductDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC7112CODCON_Id == data.NCAC7112CODCON_Id).SingleOrDefault();

        //            update.NCAC7112CODCON_Year = data.ASMAY_Id;
        //            update.NCAC7112CODCON_URL = data.NCAC7112CODCON_URL;

        //            update.NCAC7112CODCON_UpdatedBy = data.UserId;
        //            update.NCAC7112CODCON_UpdatedDate = DateTime.Now;
        //            _GeneralContext.Update(update);
        //            _GeneralContext.SaveChanges();
        //            s = update.NCAC7112CODCON_Id;
        //            if (data.filelist.Count() > 0)
        //            {
        //                foreach (NAAC_AC_7112_CodeOfCoduct_DTO DocumentsDTO in data.filelist)
        //                {
        //                    if (DocumentsDTO.NCAC7112CODCON_Id > 0)
        //                    {
        //                        var filesdata = _GeneralContext.NAAC_AC_7112_CodeOfCoduct_FilesDMO.Where(t => t.NCAC7112CODCONF_Id == DocumentsDTO.NCAC7112CODCONF_Id).FirstOrDefault();
        //                        filesdata.NCAC7112CODCONF_Filedesc = DocumentsDTO.NCAC7112CODCONF_Filedesc;
        //                        filesdata.NCAC7112CODCONF_FileName = DocumentsDTO.NCAC7112CODCONF_FileName;
        //                        filesdata.NCAC7112CODCONF_FilePath = DocumentsDTO.NCAC7112CODCONF_FilePath;
        //                        _GeneralContext.Update(filesdata);
        //                        int flag = _GeneralContext.SaveChanges();
        //                        if (flag > 0)
        //                        {
        //                            data.returnval = true;
        //                        }
        //                        else
        //                        {
        //                            data.returnval = false;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        NAAC_AC_7112_CodeOfCoduct_FilesDMO obj2 = new NAAC_AC_7112_CodeOfCoduct_FilesDMO();
        //                        obj2.NCAC7112CODCONF_FileName = DocumentsDTO.NCAC7112CODCONF_FileName;
        //                        obj2.NCAC7112CODCONF_Filedesc = DocumentsDTO.NCAC7112CODCONF_Filedesc;
        //                        obj2.NCAC7112CODCONF_FilePath = DocumentsDTO.NCAC7112CODCONF_FilePath;
        //                        obj2.NCAC7112CODCONF_Id = s;
        //                        _GeneralContext.Add(obj2);
        //                        int flag = _GeneralContext.SaveChanges();
        //                        if (flag > 0)
        //                        {
        //                            data.returnval = true;
        //                        }
        //                        else
        //                        {
        //                            data.returnval = false;
        //                        }
        //                    }
        //                }
        //            }

        //        }
        //    }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return data;
        //}


        public NAAC_AC_7112_CodeOfCoduct_DTO save(NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            long s = 0;
            try
            {
                if (data.NCAC7112CODCON_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_7112_CodeOfCoductDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC7112CODCON_Year == data.ASMAY_Id && t.NCAC7112CODCON_URL == data.NCAC7112CODCON_URL).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_7112_CodeOfCoductDMO obj1 = new NAAC_AC_7112_CodeOfCoductDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCAC7112CODCON_Year = data.ASMAY_Id;
                        obj1.NCAC7112CODCON_URL = data.NCAC7112CODCON_URL;

                        obj1.NCAC7112CODCON_ActiveFlg = true;
                        obj1.NCAC7112CODCON_CreatedBy = data.UserId;
                        obj1.NCAC7112CODCON_UpdatedBy = data.UserId;
                        obj1.NCAC7112CODCON_CreatedDate = DateTime.Now;
                        obj1.NCAC7112CODCON_UpdatedDate = DateTime.Now;
                        _GeneralContext.Add(obj1);
                        _GeneralContext.SaveChanges();
                        s = obj1.NCAC7112CODCON_Id;
                        if (data.filelist.Count() > 0)
                        {
                            foreach (NAAC_AC_7112_CodeOfCoduct_DTO DocumentsDTO in data.filelist)
                            {

                                if (DocumentsDTO.NCAC7112CODCONF_FileName!=null)
                                {
                                    NAAC_AC_7112_CodeOfCoduct_FilesDMO obj2 = new NAAC_AC_7112_CodeOfCoduct_FilesDMO();
                                    obj2.NCAC7112CODCONF_FileName = DocumentsDTO.NCAC7112CODCONF_FileName;
                                    obj2.NCAC7112CODCONF_Filedesc = DocumentsDTO.NCAC7112CODCONF_Filedesc;
                                    obj2.NCAC7112CODCONF_FilePath = DocumentsDTO.NCAC7112CODCONF_FilePath;
                                    obj2.NCAC7112CODCONF_StatusFlg = "";
                                    obj2.NCAC7112CODCONF_ActiveFlg = true;

                                    obj2.NCAC7112CODCON_Id = s;
                                    _GeneralContext.Add(obj2);
                                    int flag = _GeneralContext.SaveChanges();
                                    if (flag > 0)
                                    {
                                        data.returnval = true;
                                    }
                                    else
                                    {
                                        data.returnval = false;
                                    }
                                }
                            }
                        }
                        else if (s > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else if (data.NCAC7112CODCON_Id > 0)
                {
                    var update = _GeneralContext.NAAC_AC_7112_CodeOfCoductDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC7112CODCON_Id == data.NCAC7112CODCON_Id).SingleOrDefault();
                    update.NCAC7112CODCON_Year = data.ASMAY_Id;
                    update.NCAC7112CODCON_URL = data.NCAC7112CODCON_URL;
                    update.NCAC7112CODCON_UpdatedBy = data.UserId;
                    update.NCAC7112CODCON_UpdatedDate = DateTime.Now;
                    _GeneralContext.Update(update);
                    _GeneralContext.SaveChanges();
                    s = update.NCAC7112CODCON_Id;
                    if (data.filelist.Count() > 0)
                    {

                        List<long> Fid = new List<long>();
                        foreach (var item in data.filelist)
                        {
                            Fid.Add(item.NCAC7112CODCONF_Id);
                        }
                        var removefile11 = _GeneralContext.NAAC_AC_7112_CodeOfCoduct_FilesDMO.Where(t => t.NCAC7112CODCON_Id == data.NCAC7112CODCON_Id && !Fid.Contains(t.NCAC7112CODCONF_Id)).Distinct().ToList();

                        if (removefile11.Count > 0)
                        {
                            foreach (var item2 in removefile11)
                            {
                                var deactfile = _GeneralContext.NAAC_AC_7112_CodeOfCoduct_FilesDMO.Single(t => t.NCAC7112CODCON_Id == data.NCAC7112CODCON_Id && t.NCAC7112CODCONF_Id == item2.NCAC7112CODCONF_Id);
                                deactfile.NCAC7112CODCONF_ActiveFlg = false;
                                _GeneralContext.Update(deactfile);

                            }

                        }

                        //var CountRemoveFiles = _GeneralContext.NAAC_AC_7112_CodeOfCoduct_FilesDMO.Where(t => t.NCAC7112CODCON_Id == data.NCAC7112CODCON_Id && t.NCAC7112CODCONF_StatusFlg != "approved").ToList();
                        //if (CountRemoveFiles.Count > 0)
                        //{
                        //    foreach (var RemoveFiles in CountRemoveFiles)
                        //    {
                        //        _GeneralContext.Remove(RemoveFiles);
                        //    }
                        //}

                        foreach (NAAC_AC_7112_CodeOfCoduct_DTO DocumentsDTO in data.filelist)
                        {
                            if (DocumentsDTO.NCAC7112CODCONF_Id > 0)
                            {
                                if (DocumentsDTO.NCAC7112CODCONF_FileName != null && DocumentsDTO.NCAC7112CODCONF_StatusFlg != "approved")
                                {

                                    var filesdata = _GeneralContext.NAAC_AC_7112_CodeOfCoduct_FilesDMO.Where(t => t.NCAC7112CODCONF_Id == DocumentsDTO.NCAC7112CODCONF_Id).FirstOrDefault();
                                    filesdata.NCAC7112CODCONF_Filedesc = DocumentsDTO.NCAC7112CODCONF_Filedesc;
                                    filesdata.NCAC7112CODCONF_FileName = DocumentsDTO.NCAC7112CODCONF_FileName;
                                    filesdata.NCAC7112CODCONF_FilePath = DocumentsDTO.NCAC7112CODCONF_FilePath;
                                   

                                    _GeneralContext.Update(filesdata);
                                    
                                }
                            }
                            else
                            {
                                if (DocumentsDTO.NCAC7112CODCONF_Id == 0)
                                {
                                    if (DocumentsDTO.NCAC7112CODCONF_FileName != null)
                                {
                                    NAAC_AC_7112_CodeOfCoduct_FilesDMO obj2 = new NAAC_AC_7112_CodeOfCoduct_FilesDMO();
                                    obj2.NCAC7112CODCONF_FileName = DocumentsDTO.NCAC7112CODCONF_FileName;
                                    obj2.NCAC7112CODCONF_Filedesc = DocumentsDTO.NCAC7112CODCONF_Filedesc;
                                    obj2.NCAC7112CODCONF_FilePath = DocumentsDTO.NCAC7112CODCONF_FilePath;
                                    obj2.NCAC7112CODCONF_StatusFlg = "";
                                    obj2.NCAC7112CODCONF_ActiveFlg = true;
                                        obj2.NCAC7112CODCON_Id = data.NCAC7112CODCON_Id; 
                                    _GeneralContext.Add(obj2);
                                   
                                }
                                }
                            }
                        }
                    }
                    int flag = _GeneralContext.SaveChanges();
                    if (flag > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }

                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public NAAC_AC_7112_CodeOfCoduct_DTO deactivate(NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_7112_CodeOfCoductDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC7112CODCON_Id == data.NCAC7112CODCON_Id).SingleOrDefault();

                if (result.NCAC7112CODCON_ActiveFlg == true)
                {
                    result.NCAC7112CODCON_ActiveFlg = false;
                }
                else if (result.NCAC7112CODCON_ActiveFlg == false)
                {
                    result.NCAC7112CODCON_ActiveFlg = true;
                }

                result.NCAC7112CODCON_UpdatedDate = DateTime.Now;
                result.NCAC7112CODCON_UpdatedBy = data.UserId;

                _GeneralContext.Update(result);
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
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }

        public NAAC_AC_7112_CodeOfCoduct_DTO EditData(NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            try
            {
                var test = _GeneralContext.NAAC_AC_7112_CodeOfCoductDMO.Where(t => t.NCAC7112CODCON_Id == data.NCAC7112CODCON_Id).ToList();
              
                data.editlisttab1 = (from a in _GeneralContext.Academic
                              from b in _GeneralContext.NAAC_AC_7112_CodeOfCoductDMO
                              where (b.NCAC7112CODCON_Id == data.NCAC7112CODCON_Id && a.ASMAY_Id == b.NCAC7112CODCON_Year && b.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id)
                              select new NAAC_AC_7112_CodeOfCoduct_DTO
                              {
                                  NCAC7112CODCON_Id=b.NCAC7112CODCON_Id,
                                  NCAC7112CODCON_URL = b.NCAC7112CODCON_URL,
                                  ASMAY_Year = a.ASMAY_Year,
                                  NCAC7112CODCON_Year = b.NCAC7112CODCON_Year,
                                  NCAC7112CODCON_StatusFlg = b.NCAC7112CODCON_StatusFlg,

                              }).Distinct().ToArray();
               
                var editfilelist = _GeneralContext.NAAC_AC_7112_CodeOfCoduct_FilesDMO.Where(t => t.NCAC7112CODCON_Id == data.NCAC7112CODCON_Id&&t.NCAC7112CODCONF_ActiveFlg==true).ToList();
               
                data.editfilelist = editfilelist.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        
        public NAAC_AC_7112_CodeOfCoduct_DTO getcomment(NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            try
            {
                data.commentlist = (from a in _GeneralContext.NAAC_AC_7112_CodeOfCoduct_Comments_DMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCAC7112CODCONC_RemarksBy == b.Id && a.NCAC7112CODCON_Id == data.NCAC7112CODCON_Id )
                                    select new NAAC_AC_7112_CodeOfCoduct_DTO
                                    {
                                        NCAC7112CODCONC_Remarks = a.NCAC7112CODCONC_Remarks,
                                        NCAC7112CODCONC_Id = a.NCAC7112CODCONC_Id,
                                        NCAC7112CODCONC_RemarksBy = a.NCAC7112CODCONC_RemarksBy,
                                        NCAC7112CODCONC_StatusFlg = a.NCAC7112CODCONC_StatusFlg,
                                        NCAC7112CODCONC_ActiveFlag = a.NCAC7112CODCONC_ActiveFlag,
                                        NCAC7112CODCONC_CreatedBy = a.NCAC7112CODCONC_CreatedBy,
                                        NCAC7112CODCONC_CreatedDate = a.NCAC7112CODCONC_CreatedDate,
                                        NCAC7112CODCONC_UpdatedBy = a.NCAC7112CODCONC_UpdatedBy,
                                        NCAC7112CODCONC_UpdatedDate = a.NCAC7112CODCONC_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NCAC7112CODCONC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        // for file
        public NAAC_AC_7112_CodeOfCoduct_DTO getfilecomment(NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            try
            {
                data.commentlist1 = (from a in _GeneralContext.NAAC_AC_7112_CodeOfCoduct_File_Comments_DMO
                                     from b in _GeneralContext.ApplUser
                                    where (a.NCAC7112CODCONFC_RemarksBy == b.Id && a.NCAC7112CODCONF_Id == data.NCAC7112CODCONF_Id)
                                    select new NAAC_AC_7112_CodeOfCoduct_DTO
                                    {
                                        NCAC7112CODCONF_Id=a.NCAC7112CODCONF_Id,
                                        NCAC7112CODCONFC_Remarks = a.NCAC7112CODCONFC_Remarks,
                                        NCAC7112CODCONFC_Id = a.NCAC7112CODCONFC_Id,
                                        NCAC7112CODCONFC_RemarksBy = a.NCAC7112CODCONFC_RemarksBy,
                                        NCAC7112CODCONFC_StatusFlg = a.NCAC7112CODCONFC_StatusFlg,
                                        NCAC7112CODCONFC_ActiveFlag = a.NCAC7112CODCONFC_ActiveFlag,
                                        NCAC7112CODCONFC_CreatedBy = a.NCAC7112CODCONFC_CreatedBy,
                                        NCAC7112CODCONFC_CreatedDate = a.NCAC7112CODCONFC_CreatedDate,
                                        NCAC7112CODCONFC_UpdatedBy = a.NCAC7112CODCONFC_UpdatedBy,
                                        NCAC7112CODCONFC_UpdatedDate = a.NCAC7112CODCONFC_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NCAC7112CODCONFC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_7112_CodeOfCoduct_DTO savemedicaldatawisecomments(NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            try
            {
                    NAAC_AC_7112_CodeOfCoduct_Comments_DMO obj1 = new NAAC_AC_7112_CodeOfCoduct_Comments_DMO();
                        obj1.NCAC7112CODCONC_Remarks = data.Remarks;
                        obj1.NCAC7112CODCONC_RemarksBy = data.UserId;
                        obj1.NCAC7112CODCONC_StatusFlg = "";
                    obj1.NCAC7112CODCON_Id = data.filefkid;
                        obj1.NCAC7112CODCONC_ActiveFlag = true;
                        obj1.NCAC7112CODCONC_CreatedBy = data.UserId;
                        obj1.NCAC7112CODCONC_UpdatedBy = data.UserId;
                        obj1.NCAC7112CODCONC_CreatedDate = DateTime.Now;
                        obj1.NCAC7112CODCONC_UpdatedDate = DateTime.Now;
                        _GeneralContext.Add(obj1);
                       int s= _GeneralContext.SaveChanges();
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
        public NAAC_AC_7112_CodeOfCoduct_DTO savefilewisecomments(NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            try
            {
                NAAC_AC_7112_CodeOfCoduct_File_Comments_DMO obj1 = new NAAC_AC_7112_CodeOfCoduct_File_Comments_DMO();
                obj1.NCAC7112CODCONFC_Remarks = data.Remarks;
                obj1.NCAC7112CODCONFC_RemarksBy = data.UserId;
                obj1.NCAC7112CODCONFC_StatusFlg = "";
                obj1.NCAC7112CODCONF_Id = data.filefkid;
                obj1.NCAC7112CODCONFC_ActiveFlag = true;
                obj1.NCAC7112CODCONFC_CreatedBy = data.UserId;
                obj1.NCAC7112CODCONFC_UpdatedBy = data.UserId;
                obj1.NCAC7112CODCONFC_CreatedDate = DateTime.Now;
                obj1.NCAC7112CODCONFC_UpdatedDate = DateTime.Now;
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

        public NAAC_AC_7112_CodeOfCoduct_DTO viewuploadflies(NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            try
            {
                data.view = _GeneralContext.NAAC_AC_7112_CodeOfCoduct_FilesDMO.Where(t => t.NCAC7112CODCON_Id == data.NCAC7112CODCON_Id&&t.NCAC7112CODCONF_ActiveFlg==true).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;

        }
        public NAAC_AC_7112_CodeOfCoduct_DTO deleteuploadfile(NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            try
            {
                var res = _GeneralContext.NAAC_AC_7112_CodeOfCoduct_FilesDMO.Where(t => t.NCAC7112CODCONF_Id == data.NCAC7112CODCONF_Id).SingleOrDefault();
                res.NCAC7112CODCONF_ActiveFlg = false;
                _GeneralContext.Update(res);
                int s = _GeneralContext.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
                data.view = _GeneralContext.NAAC_AC_7112_CodeOfCoduct_FilesDMO.Where(t => t.NCAC7112CODCON_Id == data.NCAC7112CODCON_Id&&t.NCAC7112CODCONF_ActiveFlg==false).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }

        public NAAC_AC_7112_CodeOfCoduct_DTO getData(NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            try
            {
                data.allacademicyear = _GeneralContext.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

                data.alldatalist = (from a in _GeneralContext.Academic
                                    from b in _GeneralContext.NAAC_AC_7112_CodeOfCoductDMO
                                    where (a.MI_Id == b.MI_Id && b.NCAC7112CODCON_Year == a.ASMAY_Id && a.Is_Active == true && a.MI_Id == data.MI_Id)
                                    select new NAAC_AC_7112_CodeOfCoduct_DTO
                                    {
                                        NCAC7112CODCON_Id = b.NCAC7112CODCON_Id,
                                        MI_Id = a.MI_Id,
                                        NCAC7112CODCON_URL = b.NCAC7112CODCON_URL,
                                        NCAC7112CODCON_Year = b.NCAC7112CODCON_Year,
                                        NCAC7112CODCON_ActiveFlg = b.NCAC7112CODCON_ActiveFlg,
                                        ASMAY_Year = a.ASMAY_Year,
                                        NCAC7112CODCON_StatusFlg = b.NCAC7112CODCON_StatusFlg,
                                    }).Distinct().OrderByDescending(t => t.NCAC7112CODCON_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
