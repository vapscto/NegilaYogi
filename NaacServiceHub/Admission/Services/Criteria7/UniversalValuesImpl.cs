using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using DomainModel.Model.NAAC.Admission.Criteria7;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.NAAC.Admission;

namespace NaacServiceHub.Admission.Services.Criteria7
{
    public class UniversalValuesImpl : Interface.Criteria7.UniversalValuesInterface
    {

        public GeneralContext _GeneralContext;
        public UniversalValuesImpl(GeneralContext para)
        {
            _GeneralContext = para;
        }

        public async  Task<NAAC_AC_7117_UniversalValues_DTO> loaddata(NAAC_AC_7117_UniversalValues_DTO data)
        {
            try
            {
                data.institutionlist = (from a in _GeneralContext.Institution
                                        from b in _GeneralContext.UserRoleWithInstituteDMO
                                        where (a.MI_Id == b.MI_Id && a.MI_ActiveFlag == 1 && b.Id == data.UserId)
                                        select new NAAC_AC_7117_UniversalValues_DTO
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

        public NAAC_AC_7117_UniversalValues_DTO savedatatab1(NAAC_AC_7117_UniversalValues_DTO data)
        {
            long s = 0;
            try
            {
                if (data.NCAC7117UNIVAL_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_7117_UniversalValuesDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC7117UNIVAL_Year == data.NCAC7117UNIVAL_Year && t.NCAC7117UNIVAL_ProgramTitle == data.NCAC7117UNIVAL_ProgramTitle && t.NCAC7117UNIVAL_FromDate == data.NCAC7117UNIVAL_FromDate && t.NCAC7117UNIVAL_ToDate == data.NCAC7117UNIVAL_ToDate && t.NCAC7117UNIVAL_NoOfPartcipants == data.NCAC7117UNIVAL_NoOfPartcipants).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_7117_UniversalValuesDMO obj1 = new NAAC_AC_7117_UniversalValuesDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCAC7117UNIVAL_Year = data.NCAC7117UNIVAL_Year;
                        obj1.NCAC7117UNIVAL_ProgramTitle = data.NCAC7117UNIVAL_ProgramTitle;
                        obj1.NCAC7117UNIVAL_FromDate = data.NCAC7117UNIVAL_FromDate;
                        obj1.NCAC7117UNIVAL_ToDate = data.NCAC7117UNIVAL_ToDate;
                        obj1.NCAC7117UNIVAL_NoOfPartcipants = data.NCAC7117UNIVAL_NoOfPartcipants;
                        obj1.NCAC7117UNIVAL_ActiveFlg = true;
                        obj1.NCAC7117UNIVAL_CreatedBy = data.UserId;
                        obj1.NCAC7117UNIVAL_UpdatedBy = data.UserId;
                        obj1.NCAC7117UNIVAL_StatusFlg = "";
                        obj1.NCAC7117UNIVAL_CreatedDate = DateTime.Now;
                        obj1.NCAC7117UNIVAL_UpdatedDate = DateTime.Now;
                        _GeneralContext.Add(obj1);
                        _GeneralContext.SaveChanges();
                        s = obj1.NCAC7117UNIVAL_Id;
                        if (data.NAACAC7117DTO.Count() > 0)
                        {
                            foreach (NAAC_AC_7117_UniversalValues_DTO DocumentsDTO in data.NAACAC7117DTO)
                            {
                                NAAC_AC_7117_UniversalValues_FilesDMO obj2 = new NAAC_AC_7117_UniversalValues_FilesDMO();
                                obj2.NCAC7117UNIVALF_FileName = DocumentsDTO.NCAC7117UNIVALF_FileName;
                                obj2.NCAC7117UNIVALF_Filedesc = DocumentsDTO.NCAC7117UNIVALF_Filedesc;
                                obj2.NCAC7117UNIVALF_FilePath = DocumentsDTO.NCAC7117UNIVALF_FilePath;
                                obj2.NCAC7117UNIVALF_StatusFlg = "";
                                obj2.NCAC7117UNIVALF_ActiveFlg = true;
                                obj2.NCAC7117UNIVAL_Id = s;
                                _GeneralContext.Add(obj2);
                                
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
                else if (data.NCAC7117UNIVAL_Id > 0)
                {
                    var update = _GeneralContext.NAAC_AC_7117_UniversalValuesDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC7117UNIVAL_Id == data.NCAC7117UNIVAL_Id).SingleOrDefault();

                    update.NCAC7117UNIVAL_Year = data.NCAC7117UNIVAL_Year;
                    update.NCAC7117UNIVAL_ProgramTitle = data.NCAC7117UNIVAL_ProgramTitle;
                    update.NCAC7117UNIVAL_FromDate = data.NCAC7117UNIVAL_FromDate;
                    update.NCAC7117UNIVAL_ToDate = data.NCAC7117UNIVAL_ToDate;
                    update.NCAC7117UNIVAL_NoOfPartcipants = data.NCAC7117UNIVAL_NoOfPartcipants;
                    update.NCAC7117UNIVAL_UpdatedBy = data.UserId;
                    update.NCAC7117UNIVAL_UpdatedDate = DateTime.Now;
                    _GeneralContext.Update(update);
                    _GeneralContext.SaveChanges();
                    s = update.NCAC7117UNIVAL_Id;
                    if (data.NAACAC7117DTO.Count() > 0)
                    {

                        List<long> Fid = new List<long>();
                        foreach (var item in data.NAACAC7117DTO)
                        {
                            Fid.Add(item.NCAC7117UNIVALF_Id);
                        }
                        var removefile11 = _GeneralContext.NAAC_AC_7117_UniversalValues_FilesDMO.Where(t => t.NCAC7117UNIVAL_Id == data.NCAC7117UNIVAL_Id && !Fid.Contains(t.NCAC7117UNIVALF_Id)).Distinct().ToList();
                        if (removefile11.Count > 0)
                        {
                            foreach (var item2 in removefile11)
                            {
                                var deactfile = _GeneralContext.NAAC_AC_7117_UniversalValues_FilesDMO.Single(t => t.NCAC7117UNIVAL_Id == data.NCAC7117UNIVAL_Id && t.NCAC7117UNIVALF_Id == item2.NCAC7117UNIVALF_Id);
                                deactfile.NCAC7117UNIVALF_ActiveFlg = false;
                                _GeneralContext.Update(deactfile);
                            }
                        }
                        foreach (NAAC_AC_7117_UniversalValues_DTO DocumentsDTO in data.NAACAC7117DTO)
                        {
                            if (DocumentsDTO.NCAC7117UNIVALF_Id > 0)
                            {
                                var filesdata = _GeneralContext.NAAC_AC_7117_UniversalValues_FilesDMO.Where(t => t.NCAC7117UNIVALF_Id == DocumentsDTO.NCAC7117UNIVALF_Id).FirstOrDefault();
                                filesdata.NCAC7117UNIVALF_Filedesc = DocumentsDTO.NCAC7117UNIVALF_Filedesc;
                                filesdata.NCAC7117UNIVALF_FileName = DocumentsDTO.NCAC7117UNIVALF_FileName;
                                filesdata.NCAC7117UNIVALF_FilePath = DocumentsDTO.NCAC7117UNIVALF_FilePath;
                                _GeneralContext.Update(filesdata);
                                
                            }
                            else
                            {
                                NAAC_AC_7117_UniversalValues_FilesDMO obj2 = new NAAC_AC_7117_UniversalValues_FilesDMO();
                                obj2.NCAC7117UNIVALF_FileName = DocumentsDTO.NCAC7117UNIVALF_FileName;
                                obj2.NCAC7117UNIVALF_Filedesc = DocumentsDTO.NCAC7117UNIVALF_Filedesc;
                                obj2.NCAC7117UNIVALF_FilePath = DocumentsDTO.NCAC7117UNIVALF_FilePath;
                                obj2.NCAC7117UNIVALF_StatusFlg = "";
                                obj2.NCAC7117UNIVALF_ActiveFlg = true;
                                obj2.NCAC7117UNIVAL_Id = s;
                                _GeneralContext.Add(obj2);
                               
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

        public NAAC_AC_7117_UniversalValues_DTO deactivYTab1(NAAC_AC_7117_UniversalValues_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_7117_UniversalValuesDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC7117UNIVAL_Id == data.NCAC7117UNIVAL_Id).SingleOrDefault();

                if (result.NCAC7117UNIVAL_ActiveFlg == true)
                {
                    result.NCAC7117UNIVAL_ActiveFlg = false;
                }
                else if (result.NCAC7117UNIVAL_ActiveFlg == false)
                {
                    result.NCAC7117UNIVAL_ActiveFlg = true;
                }

                result.NCAC7117UNIVAL_UpdatedDate = DateTime.Now;
                result.NCAC7117UNIVAL_UpdatedBy = data.UserId;

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

        public NAAC_AC_7117_UniversalValues_DTO editTab1(NAAC_AC_7117_UniversalValues_DTO data)
        {
            try
            {
                var edit = _GeneralContext.NAAC_AC_7117_UniversalValuesDMO.Where(t =>t.NCAC7117UNIVAL_Id == data.NCAC7117UNIVAL_Id).ToList();
                var editfile = _GeneralContext.NAAC_AC_7117_UniversalValues_FilesDMO.Where(t => t.NCAC7117UNIVAL_Id == data.NCAC7117UNIVAL_Id&&t.NCAC7117UNIVALF_ActiveFlg==true).ToList();

                data.editlisttab1 = edit.ToArray();
                data.editfilelist = editfile.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public NAAC_AC_7117_UniversalValues_DTO deleteuploadfile(NAAC_AC_7117_UniversalValues_DTO data)
        {
            try
            {
                List<NAAC_AC_7117_UniversalValues_FilesDMO> removelist = new List<NAAC_AC_7117_UniversalValues_FilesDMO>();
                removelist = _GeneralContext.NAAC_AC_7117_UniversalValues_FilesDMO.Where(t => t.NCAC7117UNIVALF_Id == data.NCAC7117UNIVALF_Id).ToList();
                foreach (NAAC_AC_7117_UniversalValues_FilesDMO obj1 in removelist)
                {
                    obj1.NCAC7117UNIVALF_ActiveFlg = false;
                    _GeneralContext.Update(obj1);
                   // _GeneralContext.Remove(obj1);
                    _GeneralContext.SaveChanges();
                    data.retrunMsg = "Deleted";
                    data.returnval = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public NAAC_AC_7117_UniversalValues_DTO getData(NAAC_AC_7117_UniversalValues_DTO data)
        {
            try
            {
                data.allacademicyear = _GeneralContext.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

                data.alldatalist = (from a in _GeneralContext.NAAC_AC_7117_UniversalValuesDMO
                                    from b in _GeneralContext.Academic
                                    where (a.MI_Id == data.MI_Id && a.NCAC7117UNIVAL_Year == b.ASMAY_Id && b.MI_Id == a.MI_Id)
                                    select new NAAC_AC_7117_UniversalValues_DTO
                                    {
                                        NCAC7117UNIVAL_Id = a.NCAC7117UNIVAL_Id,
                                        MI_Id = a.MI_Id,
                                        NCAC7117UNIVAL_Year = a.NCAC7117UNIVAL_Year,
                                        ASMAY_Year = b.ASMAY_Year,
                                        NCAC7117UNIVAL_ProgramTitle = a.NCAC7117UNIVAL_ProgramTitle,
                                        NCAC7117UNIVAL_FromDate = a.NCAC7117UNIVAL_FromDate,
                                        NCAC7117UNIVAL_ToDate = a.NCAC7117UNIVAL_ToDate,
                                        NCAC7117UNIVAL_NoOfPartcipants = a.NCAC7117UNIVAL_NoOfPartcipants,
                                        NCAC7117UNIVAL_ActiveFlg = a.NCAC7117UNIVAL_ActiveFlg,
                                        NCAC7117UNIVAL_StatusFlg = a.NCAC7117UNIVAL_StatusFlg

                                    }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public NAAC_AC_7117_UniversalValues_DTO getcomment(NAAC_AC_7117_UniversalValues_DTO data)
        {
            try
            {
                data.commentlist = (from a in _GeneralContext.NAAC_AC_7117_UniversalValues_Comments_DMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCAC7117UNIVALC_RemarksBy == b.Id && a.NCAC7117UNIVAL_Id == data.NCAC7117UNIVAL_Id)
                                    select new NAAC_AC_7117_UniversalValues_DTO
                                    {
                                        NCAC7117UNIVALC_Remarks = a.NCAC7117UNIVALC_Remarks,
                                        NCAC7117UNIVALC_Id = a.NCAC7117UNIVALC_Id,
                                        NCAC7117UNIVALC_RemarksBy = a.NCAC7117UNIVALC_RemarksBy,
                                        NCAC7117UNIVALC_StatusFlg = a.NCAC7117UNIVALC_StatusFlg,
                                        NCAC7117UNIVALC_ActiveFlag = a.NCAC7117UNIVALC_ActiveFlag,
                                        NCAC7117UNIVALC_CreatedBy = a.NCAC7117UNIVALC_CreatedBy,
                                        NCAC7117UNIVALC_CreatedDate = a.NCAC7117UNIVALC_CreatedDate,
                                        NCAC7117UNIVALC_UpdatedBy = a.NCAC7117UNIVALC_UpdatedBy,
                                        NCAC7117UNIVALC_UpdatedDate = a.NCAC7117UNIVALC_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NCAC7117UNIVALC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        // for file
        public NAAC_AC_7117_UniversalValues_DTO getfilecomment(NAAC_AC_7117_UniversalValues_DTO data)
        {
            try
            {
                data.commentlist1 = (from a in _GeneralContext.NAAC_AC_7117_UniversalValues_File_Comments_DMO
                                     from b in _GeneralContext.ApplUser
                                     where (a.NCAC7117UNIVALFC_RemarksBy == b.Id && a.NCAC7117UNIVALF_Id == data.NCAC7117UNIVALF_Id)
                                     select new NAAC_AC_7117_UniversalValues_DTO
                                     {
                                         NCAC7117UNIVALF_Id = a.NCAC7117UNIVALF_Id,
                                         NCAC7117UNIVALFC_Remarks = a.NCAC7117UNIVALFC_Remarks,
                                         NCAC7117UNIVALFC_Id = a.NCAC7117UNIVALFC_Id,
                                         NCAC7117UNIVALFC_RemarksBy = a.NCAC7117UNIVALFC_RemarksBy,
                                         NCAC7117UNIVALFC_StatusFlg = a.NCAC7117UNIVALFC_StatusFlg,
                                         NCAC7117UNIVALFC_ActiveFlag = a.NCAC7117UNIVALFC_ActiveFlag,
                                         NCAC7117UNIVALFC_CreatedBy = a.NCAC7117UNIVALFC_CreatedBy,
                                         NCAC7117UNIVALFC_CreatedDate = a.NCAC7117UNIVALFC_CreatedDate,
                                         NCAC7117UNIVALFC_UpdatedBy = a.NCAC7117UNIVALFC_UpdatedBy,
                                         NCAC7117UNIVALFC_UpdatedDate = a.NCAC7117UNIVALFC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().OrderByDescending(a => a.NCAC7117UNIVALFC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_7117_UniversalValues_DTO savemedicaldatawisecomments(NAAC_AC_7117_UniversalValues_DTO data)
        {
            try
            {
                NAAC_AC_7117_UniversalValues_Comments_DMO obj1 = new NAAC_AC_7117_UniversalValues_Comments_DMO();
                obj1.NCAC7117UNIVALC_Remarks = data.Remarks;
                obj1.NCAC7117UNIVALC_RemarksBy = data.UserId;
                obj1.NCAC7117UNIVALC_StatusFlg = "";
                obj1.NCAC7117UNIVAL_Id = data.filefkid;
                obj1.NCAC7117UNIVALC_ActiveFlag = true;
                obj1.NCAC7117UNIVALC_CreatedBy = data.UserId;
                obj1.NCAC7117UNIVALC_UpdatedBy = data.UserId;
                obj1.NCAC7117UNIVALC_CreatedDate = DateTime.Now;
                obj1.NCAC7117UNIVALC_UpdatedDate = DateTime.Now;
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
        public NAAC_AC_7117_UniversalValues_DTO savefilewisecomments(NAAC_AC_7117_UniversalValues_DTO data)
        {
            try
            {
                NAAC_AC_7117_UniversalValues_File_Comments_DMO obj1 = new NAAC_AC_7117_UniversalValues_File_Comments_DMO();
                obj1.NCAC7117UNIVALFC_Remarks = data.Remarks;
                obj1.NCAC7117UNIVALFC_RemarksBy = data.UserId;
                obj1.NCAC7117UNIVALFC_StatusFlg = "";
                obj1.NCAC7117UNIVALF_Id = data.filefkid;
                obj1.NCAC7117UNIVALFC_ActiveFlag = true;
                obj1.NCAC7117UNIVALFC_CreatedBy = data.UserId;
                obj1.NCAC7117UNIVALFC_UpdatedBy = data.UserId;
                obj1.NCAC7117UNIVALFC_UpdatedDate = DateTime.Now;
                obj1.NCAC7117UNIVALFC_CreatedDate = DateTime.Now;
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
    }
}
