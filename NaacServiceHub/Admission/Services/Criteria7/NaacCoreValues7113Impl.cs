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
    public class NaacCoreValues7113Impl:Interface.Criteria7.NaacCoreValues7113Interface
    {
        public GeneralContext _GeneralContext;
        public NaacCoreValues7113Impl(GeneralContext para)
        {
            _GeneralContext = para;
        }

        public async Task<NAAC_AC_7113_CoreValues_DTO> loaddata(NAAC_AC_7113_CoreValues_DTO data)
        {
            try
            {
                data.institutionlist = (from a in _GeneralContext.Institution
                                        from b in _GeneralContext.UserRoleWithInstituteDMO
                                        where (a.MI_Id == b.MI_Id && a.MI_ActiveFlag == 1 && b.Id == data.UserId)
                                        select new NAAC_AC_7113_CoreValues_DTO
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

        public NAAC_AC_7113_CoreValues_DTO save(NAAC_AC_7113_CoreValues_DTO data)
        {
            long s = 0;
            try
            {
                if (data.NCAC7113CORVAL_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_7113_CoreValuesDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC7113CORVAL_Year == data.ASMAY_Id && t.NCAC7113CORVAL_URL == data.NCAC7113CORVAL_URL).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_7113_CoreValuesDMO obj1 = new NAAC_AC_7113_CoreValuesDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCAC7113CORVAL_Year = data.ASMAY_Id;
                        obj1.NCAC7113CORVAL_URL = data.NCAC7113CORVAL_URL;

                        obj1.NCAC7113CORVAL_ActiveFlg = true;
                        obj1.NCAC7113CORVAL_CreatedBy = data.UserId;
                        obj1.NCAC7113CORVAL_UpdatedBy = data.UserId;
                        obj1.NCAC7113CORVAL_CreatedDate = DateTime.Now;
                        obj1.NCAC7113CORVAL_UpdatedDate = DateTime.Now;
                        obj1.NCAC7113CORVAL_StatusFlg = "";
                        _GeneralContext.Add(obj1);
                        _GeneralContext.SaveChanges();
                        s = obj1.NCAC7113CORVAL_Id;
                        if (data.filelist.Count() > 0)
                        {
                            foreach (NAAC_AC_7113_CoreValues_DTO DocumentsDTO in data.filelist)
                            {
                                if (DocumentsDTO.NCAC7113CORVALF_FilePath != null)
                                {

                                    NAAC_AC_7113_CoreValues_FilesDMO obj2 = new NAAC_AC_7113_CoreValues_FilesDMO();
                                    obj2.NCAC7113CORVALF_FileName = DocumentsDTO.NCAC7113CORVALF_FileName;
                                    obj2.NCAC7113CORVALF_Filedesc = DocumentsDTO.NCAC7113CORVALF_Filedesc;
                                    obj2.NCAC7113CORVALF_FilePath = DocumentsDTO.NCAC7113CORVALF_FilePath;
                                    obj2.NCAC7113CORVALF_ActiveFlg = true;
                                    obj2.NCAC7113CORVAL_Id = s;
                                    _GeneralContext.Add(obj2);
                                    int flag = _GeneralContext.SaveChanges();
                                    if (flag > 0)
                                    {
                                        data.returnval = true;
                                    }
                                    
                                }
                            }
                        }
                        else if (s>0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else if (data.NCAC7113CORVAL_Id > 0)
                {
                    var update = _GeneralContext.NAAC_AC_7113_CoreValuesDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC7113CORVAL_Id == data.NCAC7113CORVAL_Id).SingleOrDefault();

                    update.NCAC7113CORVAL_Year = data.ASMAY_Id;
                    update.NCAC7113CORVAL_URL = data.NCAC7113CORVAL_URL;
                    update.NCAC7113CORVAL_UpdatedBy = data.UserId;
                    update.NCAC7113CORVAL_UpdatedDate = DateTime.Now;
                    _GeneralContext.Update(update);
                    _GeneralContext.SaveChanges();
                    s = update.NCAC7113CORVAL_Id;
                    if (data.filelist.Count() > 0)
                    {

                        List<long> Fid = new List<long>();
                        foreach (var item in data.filelist)
                        {
                            Fid.Add(item.NCAC7113CORVALF_Id);
                        }
                        var removefile11 = _GeneralContext.NAAC_AC_7113_CoreValues_FilesDMO.Where(t => t.NCAC7113CORVAL_Id == data.NCAC7113CORVAL_Id && !Fid.Contains(t.NCAC7113CORVALF_Id)).Distinct().ToList();
                        if (removefile11.Count > 0)
                        {
                            foreach (var item2 in removefile11)
                            {
                                var deactfile = _GeneralContext.NAAC_AC_7113_CoreValues_FilesDMO.Single(t => t.NCAC7113CORVAL_Id == data.NCAC7113CORVAL_Id && t.NCAC7113CORVALF_Id == item2.NCAC7113CORVALF_Id);
                                deactfile.NCAC7113CORVALF_ActiveFlg = false;
                                _GeneralContext.Update(deactfile);
                            }
                        }
                        //var removefile = _GeneralContext.NAAC_AC_7113_CoreValues_FilesDMO.Where(t => t.NCAC7113CORVAL_Id == data.NCAC7113CORVAL_Id && t.NCAC7113CORVALF_StatusFlg != "approved").ToList();

                        //if (removefile.Count > 0)
                        //{
                        //    foreach(var aa in removefile)
                        //    {
                        //        _GeneralContext.Remove(aa);
                        //    }
                        //}


                        foreach (NAAC_AC_7113_CoreValues_DTO DocumentsDTO in data.filelist)
                        {
                            if (DocumentsDTO.NCAC7113CORVALF_Id > 0 && DocumentsDTO.NCAC7113CORVALF_StatusFlg != "approved")
                            {

                                if (DocumentsDTO.NCAC7113CORVALF_FilePath != null)
                                {
                                    var filesdata = _GeneralContext.NAAC_AC_7113_CoreValues_FilesDMO.Where(t => t.NCAC7113CORVALF_Id == DocumentsDTO.NCAC7113CORVALF_Id).FirstOrDefault();
                                    filesdata.NCAC7113CORVALF_Filedesc = DocumentsDTO.NCAC7113CORVALF_Filedesc;
                                    filesdata.NCAC7113CORVALF_FileName = DocumentsDTO.NCAC7113CORVALF_FileName;
                                    filesdata.NCAC7113CORVALF_FilePath = DocumentsDTO.NCAC7113CORVALF_FilePath;
                                    _GeneralContext.Update(filesdata);
                                    
                                }
                            }
                            else
                            {
                                if (DocumentsDTO.NCAC7113CORVALF_Id == 0) { 
                                if (DocumentsDTO.NCAC7113CORVALF_FilePath != null)
                                {
                                    NAAC_AC_7113_CoreValues_FilesDMO obj2 = new NAAC_AC_7113_CoreValues_FilesDMO();
                                    obj2.NCAC7113CORVALF_FileName = DocumentsDTO.NCAC7113CORVALF_FileName;
                                    obj2.NCAC7113CORVALF_Filedesc = DocumentsDTO.NCAC7113CORVALF_Filedesc;
                                    obj2.NCAC7113CORVALF_FilePath = DocumentsDTO.NCAC7113CORVALF_FilePath;
                                    obj2.NCAC7113CORVALF_ActiveFlg = true;
                                    obj2.NCAC7113CORVALF_StatusFlg = "";
                                    obj2.NCAC7113CORVAL_Id = s;
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


        public NAAC_AC_7113_CoreValues_DTO deactivate(NAAC_AC_7113_CoreValues_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_7113_CoreValuesDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC7113CORVAL_Id == data.NCAC7113CORVAL_Id).SingleOrDefault();

                if (result.NCAC7113CORVAL_ActiveFlg == true)
                {
                    result.NCAC7113CORVAL_ActiveFlg = false;
                }
                else if (result.NCAC7113CORVAL_ActiveFlg == false)
                {
                    result.NCAC7113CORVAL_ActiveFlg = true;
                }

                result.NCAC7113CORVAL_UpdatedDate = DateTime.Now;
                result.NCAC7113CORVAL_UpdatedBy = data.UserId;

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
        public NAAC_AC_7113_CoreValues_DTO EditData(NAAC_AC_7113_CoreValues_DTO data)
        {
            try
            {
                data.editlisttab1 = (from a in _GeneralContext.Academic
                                     from b in _GeneralContext.NAAC_AC_7113_CoreValuesDMO
                                     where (b.NCAC7113CORVAL_Id == data.NCAC7113CORVAL_Id && a.ASMAY_Id == b.NCAC7113CORVAL_Year && b.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id)
                                     select new NAAC_AC_7113_CoreValues_DTO
                                     {
                                         NCAC7113CORVAL_Id = b.NCAC7113CORVAL_Id,
                                         NCAC7113CORVAL_URL = b.NCAC7113CORVAL_URL,
                                         ASMAY_Year = a.ASMAY_Year,
                                         NCAC7113CORVAL_Year = b.NCAC7113CORVAL_Year,
                                     }).Distinct().ToArray();
                //var edit = _GeneralContext.NAAC_AC_7112_CodeOfCoductDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC7112CODCON_Id == data.NCAC7112CODCON_Id).ToList();
                var editfilelist = _GeneralContext.NAAC_AC_7113_CoreValues_FilesDMO.Where(t => t.NCAC7113CORVAL_Id == data.NCAC7113CORVAL_Id&&t.NCAC7113CORVALF_ActiveFlg==true).ToList();
                //data.editlisttab1 = edit.ToArray();
                data.editfilelist = editfilelist.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public NAAC_AC_7113_CoreValues_DTO viewuploadflies(NAAC_AC_7113_CoreValues_DTO data)
        {
            try
            {
                data.view = _GeneralContext.NAAC_AC_7113_CoreValues_FilesDMO.Where(t => t.NCAC7113CORVAL_Id == data.NCAC7113CORVAL_Id&&t.NCAC7113CORVALF_ActiveFlg==true).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;

        }

        public NAAC_AC_7113_CoreValues_DTO deleteuploadfile(NAAC_AC_7113_CoreValues_DTO data)
        {
            try
            {
                var res = _GeneralContext.NAAC_AC_7113_CoreValues_FilesDMO.Where(t => t.NCAC7113CORVALF_Id == data.NCAC7113CORVALF_Id).SingleOrDefault();
                res.NCAC7113CORVALF_ActiveFlg = false;
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
                data.view = _GeneralContext.NAAC_AC_7113_CoreValues_FilesDMO.Where(t => t.NCAC7113CORVAL_Id == data.NCAC7113CORVAL_Id&&t.NCAC7113CORVALF_ActiveFlg==true).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }

        public NAAC_AC_7113_CoreValues_DTO getcomment(NAAC_AC_7113_CoreValues_DTO data)
        {
            try
            {
                data.commentlist = (from a in _GeneralContext.NAAC_AC_7113_CoreValues_Comments_DMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCAC7113CORVALC_RemarksBy == b.Id && a.NCAC7113CORVAL_Id == data.NCAC7113CORVAL_Id)
                                    select new NAAC_AC_7113_CoreValues_DTO
                                    {
                                        NCAC7113CORVALC_Remarks = a.NCAC7113CORVALC_Remarks,
                                        NCAC7113CORVAL_Id = a.NCAC7113CORVAL_Id,
                                        NCAC7113CORVALC_Id = a.NCAC7113CORVALC_Id,
                                        NCAC7113CORVALC_RemarksBy = a.NCAC7113CORVALC_RemarksBy,
                                        NCAC7113CORVALC_StatusFlg = a.NCAC7113CORVALC_StatusFlg,
                                        NCAC7113CORVALC_ActiveFlag = a.NCAC7113CORVALC_ActiveFlag,
                                        NCAC7113CORVALC_CreatedBy = a.NCAC7113CORVALC_CreatedBy,
                                        NCAC7113CORVALC_CreatedDate = a.NCAC7113CORVALC_CreatedDate,
                                        NCAC7113CORVALC_UpdatedBy = a.NCAC7113CORVALC_UpdatedBy,
                                        NCAC7113CORVALC_UpdatedDate = a.NCAC7113CORVALC_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NCAC7113CORVALC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // for file
        public NAAC_AC_7113_CoreValues_DTO getfilecomment(NAAC_AC_7113_CoreValues_DTO data)
        {

            try
            {

                data.commentlist1 = (from a in _GeneralContext.NAAC_AC_7113_CoreValues_File_Comments_DMO
                                     from b in _GeneralContext.ApplUser
                                     where (a.NCAC7113CORVALFC_RemarksBy == b.Id && a.NCAC7113CORVALF_Id == data.NCAC7113CORVALF_Id)
                                     select new NAAC_AC_7113_CoreValues_DTO
                                     {
                                         NCAC7113CORVALF_Id = a.NCAC7113CORVALF_Id,
                                         NCAC7113CORVALFC_Remarks = a.NCAC7113CORVALFC_Remarks,
                                         NCAC7113CORVALFC_Id = a.NCAC7113CORVALFC_Id,
                                         NCAC7113CORVALFC_RemarksBy = a.NCAC7113CORVALFC_RemarksBy,
                                         NCAC7113CORVALFC_StatusFlg = a.NCAC7113CORVALFC_StatusFlg,
                                         NCAC7113CORVALFC_ActiveFlag = a.NCAC7113CORVALFC_ActiveFlag,
                                         NCAC7113CORVALFC_CreatedBy = a.NCAC7113CORVALFC_CreatedBy,
                                         NCAC7113CORVALFC_CreatedDate = a.NCAC7113CORVALFC_CreatedDate,
                                         NCAC7113CORVALFC_UpdatedBy = a.NCAC7113CORVALFC_UpdatedBy,
                                         NCAC7113CORVALFC_UpdatedDate = a.NCAC7113CORVALFC_UpdatedDate,
                                         UserName = b.UserName,

                                     }).Distinct().OrderByDescending(a => a.NCAC7113CORVALFC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public NAAC_AC_7113_CoreValues_DTO savemedicaldatawisecomments(NAAC_AC_7113_CoreValues_DTO data)
        {
           
            try
            {
                NAAC_AC_7113_CoreValues_Comments_DMO obj1 = new NAAC_AC_7113_CoreValues_Comments_DMO();


                obj1.NCAC7113CORVALC_Remarks = data.Remarks;
                obj1.NCAC7113CORVALC_RemarksBy = data.UserId;
                obj1.NCAC7113CORVALC_StatusFlg = "";
                obj1.NCAC7113CORVAL_Id = data.filefkid;

                obj1.NCAC7113CORVALC_ActiveFlag = true;
                obj1.NCAC7113CORVALC_CreatedBy = data.UserId;
                obj1.NCAC7113CORVALC_UpdatedBy = data.UserId;
                obj1.NCAC7113CORVALC_CreatedDate = DateTime.Now;
                obj1.NCAC7113CORVALC_UpdatedDate = DateTime.Now;
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

        // for file adding
        public NAAC_AC_7113_CoreValues_DTO savefilewisecomments(NAAC_AC_7113_CoreValues_DTO data)
        {

            try
            {



                NAAC_AC_7113_CoreValues_File_Comments_DMO obj1 = new NAAC_AC_7113_CoreValues_File_Comments_DMO();


                obj1.NCAC7113CORVALFC_Remarks = data.Remarks;
                obj1.NCAC7113CORVALFC_RemarksBy = data.UserId;
                obj1.NCAC7113CORVALFC_StatusFlg = "";
                obj1.NCAC7113CORVALF_Id = data.filefkid;

                obj1.NCAC7113CORVALFC_ActiveFlag = true;
                obj1.NCAC7113CORVALFC_CreatedBy = data.UserId;
                obj1.NCAC7113CORVALFC_UpdatedBy = data.UserId;
                obj1.NCAC7113CORVALFC_CreatedDate = DateTime.Now;
                obj1.NCAC7113CORVALFC_UpdatedDate = DateTime.Now;
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

        public NAAC_AC_7113_CoreValues_DTO getData(NAAC_AC_7113_CoreValues_DTO data)
        {
            try
            {
                data.allacademicyear = _GeneralContext.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

                data.alldatalist = (from a in _GeneralContext.Academic
                                    from b in _GeneralContext.NAAC_AC_7113_CoreValuesDMO
                                    where (a.MI_Id == b.MI_Id && b.NCAC7113CORVAL_Year == a.ASMAY_Id && a.Is_Active == true && a.MI_Id == data.MI_Id)
                                    select new NAAC_AC_7113_CoreValues_DTO
                                    {
                                        NCAC7113CORVAL_Id = b.NCAC7113CORVAL_Id,
                                        MI_Id = a.MI_Id,
                                        NCAC7113CORVAL_URL = b.NCAC7113CORVAL_URL,
                                        NCAC7113CORVAL_Year = b.NCAC7113CORVAL_Year,
                                        NCAC7113CORVAL_ActiveFlg = b.NCAC7113CORVAL_ActiveFlg,
                                        ASMAY_Year = a.ASMAY_Year,
                                        NCAC7113CORVAL_StatusFlg = b.NCAC7113CORVAL_StatusFlg
                                    }).Distinct().OrderByDescending(t => t.NCAC7113CORVAL_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
