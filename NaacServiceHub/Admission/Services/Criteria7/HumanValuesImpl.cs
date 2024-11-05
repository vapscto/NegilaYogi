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
    public class HumanValuesImpl : Interface.Criteria7.HumanValuesInterface
    {

        public GeneralContext _GeneralContext;
        public HumanValuesImpl(GeneralContext para)
        {
            _GeneralContext = para;
        }

        public async  Task<NAAC_AC_7114_HumanValues_DTO> loaddata(NAAC_AC_7114_HumanValues_DTO data)
        {
            try
            {
                data.institutionlist = (from a in _GeneralContext.Institution
                                        from b in _GeneralContext.UserRoleWithInstituteDMO
                                        where (a.MI_Id == b.MI_Id && a.MI_ActiveFlag == 1 && b.Id == data.UserId)
                                        select new NAAC_AC_7114_HumanValues_DTO
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

        public NAAC_AC_7114_HumanValues_DTO savedatatab1(NAAC_AC_7114_HumanValues_DTO data)
        {
            long s = 0;
            try
            {
                if (data.NCAC7114HUVAL_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_7114_HumanValuesDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC7114HUVAL_Year == data.NCAC7114HUVAL_Year && t.NCAC7114HUVAL_ProgramTitle == data.NCAC7114HUVAL_ProgramTitle && t.NCAC7114HUVAL_FromDate == data.NCAC7114HUVAL_FromDate && t.NCAC7114HUVAL_ToDate == data.NCAC7114HUVAL_ToDate && t.NCAC7114HUVAL_NoOfPartcipants == data.NCAC7114HUVAL_NoOfPartcipants).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_7114_HumanValuesDMO obj1 = new NAAC_AC_7114_HumanValuesDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCAC7114HUVAL_Year = data.NCAC7114HUVAL_Year;
                        obj1.NCAC7114HUVAL_ProgramTitle = data.NCAC7114HUVAL_ProgramTitle;
                        obj1.NCAC7114HUVAL_FromDate = data.NCAC7114HUVAL_FromDate;
                        obj1.NCAC7114HUVAL_ToDate = data.NCAC7114HUVAL_ToDate;
                        obj1.NCAC7114HUVAL_NoOfPartcipants = data.NCAC7114HUVAL_NoOfPartcipants;
                        obj1.NCAC7114HUVAL_ActiveFlg = true;
                        obj1.NCAC7114HUVAL_CreatedBy = data.UserId;
                        obj1.NCAC7114HUVAL_UpdatedBy = data.UserId;
                        obj1.NCAC7114HUVAL_CreatedDate = DateTime.Now;
                        obj1.NCAC7114HUVAL_UpdatedDate = DateTime.Now;
                        _GeneralContext.Add(obj1);
                        _GeneralContext.SaveChanges();
                        s = obj1.NCAC7114HUVAL_Id;
                        if (data.NAACAC7114DTO.Count() > 0)
                        {
                            foreach (NAAC_AC_7114_HumanValues_DTO DocumentsDTO in data.NAACAC7114DTO)
                            {
                                NAAC_AC_7114_HumanValues_FilesDMO obj2 = new NAAC_AC_7114_HumanValues_FilesDMO();
                                obj2.NCAC7114HUVALF_FileName = DocumentsDTO.NCAC7114HUVALF_FileName;
                                obj2.NCAC7114HUVALF_Filedesc = DocumentsDTO.NCAC7114HUVALF_Filedesc;
                                obj2.NCAC7114HUVALF_FilePath = DocumentsDTO.NCAC7114HUVALF_FilePath;
                                obj2.NCAC7114HUVAL_Id = s;
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
                else if (data.NCAC7114HUVAL_Id > 0)
                {
                    var update = _GeneralContext.NAAC_AC_7114_HumanValuesDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC7114HUVAL_Id == data.NCAC7114HUVAL_Id).SingleOrDefault();

                    update.NCAC7114HUVAL_Year = data.NCAC7114HUVAL_Year;
                    update.NCAC7114HUVAL_ProgramTitle = data.NCAC7114HUVAL_ProgramTitle;
                    update.NCAC7114HUVAL_FromDate = data.NCAC7114HUVAL_FromDate;
                    update.NCAC7114HUVAL_ToDate = data.NCAC7114HUVAL_ToDate;
                    update.NCAC7114HUVAL_NoOfPartcipants = data.NCAC7114HUVAL_NoOfPartcipants;
                    update.NCAC7114HUVAL_UpdatedBy = data.UserId;
                    update.NCAC7114HUVAL_UpdatedDate = DateTime.Now;
                    _GeneralContext.Update(update);
                    _GeneralContext.SaveChanges();
                    s = update.NCAC7114HUVAL_Id;
                    if (data.NAACAC7114DTO.Count() > 0)
                    {
                        foreach (NAAC_AC_7114_HumanValues_DTO DocumentsDTO in data.NAACAC7114DTO)
                        {
                            if (DocumentsDTO.NCAC7114HUVALF_Id > 0)
                            {
                                var filesdata = _GeneralContext.NAAC_AC_7114_HumanValues_FilesDMO.Where(t => t.NCAC7114HUVALF_Id == DocumentsDTO.NCAC7114HUVALF_Id).FirstOrDefault();
                                filesdata.NCAC7114HUVALF_Filedesc = DocumentsDTO.NCAC7114HUVALF_Filedesc;
                                filesdata.NCAC7114HUVALF_FileName = DocumentsDTO.NCAC7114HUVALF_FileName;
                                filesdata.NCAC7114HUVALF_FilePath = DocumentsDTO.NCAC7114HUVALF_FilePath;
                                _GeneralContext.Update(filesdata);
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
                            else
                            {
                                NAAC_AC_7114_HumanValues_FilesDMO obj2 = new NAAC_AC_7114_HumanValues_FilesDMO();
                                obj2.NCAC7114HUVALF_FileName = DocumentsDTO.NCAC7114HUVALF_FileName;
                                obj2.NCAC7114HUVALF_Filedesc = DocumentsDTO.NCAC7114HUVALF_Filedesc;
                                obj2.NCAC7114HUVALF_FilePath = DocumentsDTO.NCAC7114HUVALF_FilePath;
                                obj2.NCAC7114HUVAL_Id = s;
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

                    //var duplicate = _GeneralContext.NAAC_AC_7114_HumanValuesDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC7114HUVAL_Year == data.NCAC7114HUVAL_Year && t.NCAC7114HUVAL_ProgramTitle == data.NCAC7114HUVAL_ProgramTitle && t.NCAC7114HUVAL_FromDate == data.NCAC7114HUVAL_FromDate && t.NCAC7114HUVAL_ToDate == data.NCAC7114HUVAL_ToDate && t.NCAC7114HUVAL_NoOfPartcipants == data.NCAC7114HUVAL_NoOfPartcipants && t.NCAC711GENEQ_NoOfParticipantsFeMale == data.NCAC711GENEQ_NoOfParticipantsFeMale).ToList();
                    //if (duplicate.Count > 0)
                    //{
                    //    data.duplicate = true;
                    //}
                    //else
                    //{

                    //}
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public NAAC_AC_7114_HumanValues_DTO deactivYTab1(NAAC_AC_7114_HumanValues_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_7114_HumanValuesDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC7114HUVAL_Id == data.NCAC7114HUVAL_Id).SingleOrDefault();

                if (result.NCAC7114HUVAL_ActiveFlg == true)
                {
                    result.NCAC7114HUVAL_ActiveFlg = false;
                }
                else if (result.NCAC7114HUVAL_ActiveFlg == false)
                {
                    result.NCAC7114HUVAL_ActiveFlg = true;
                }

                result.NCAC7114HUVAL_UpdatedDate = DateTime.Now;
                result.NCAC7114HUVAL_UpdatedBy = data.UserId;

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

        public NAAC_AC_7114_HumanValues_DTO editTab1(NAAC_AC_7114_HumanValues_DTO data)
        {
            try
            {
                var edit = _GeneralContext.NAAC_AC_7114_HumanValuesDMO.Where(t => t.NCAC7114HUVAL_Id == data.NCAC7114HUVAL_Id).ToList();
                data.editlisttab1 = (from a in _GeneralContext.Academic
                                    from b in _GeneralContext.NAAC_AC_7114_HumanValuesDMO
                                    where (b.NCAC7114HUVAL_Id == data.NCAC7114HUVAL_Id && a.ASMAY_Id == b.NCAC7114HUVAL_Year && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id)
                                    select new NAAC_AC_7114_HumanValues_DTO
                                    {
                                        NCAC7114HUVAL_Id = b.NCAC7114HUVAL_Id,
                                        NCAC7114HUVAL_Year = b.NCAC7114HUVAL_Year,
                                        ASMAY_Year = a.ASMAY_Year,
                                        NCAC7114HUVAL_NoOfPartcipants = b.NCAC7114HUVAL_NoOfPartcipants,
                                        NCAC7114HUVAL_ProgramTitle = b.NCAC7114HUVAL_ProgramTitle,
                                        NCAC7114HUVAL_StatusFlg = b.NCAC7114HUVAL_StatusFlg
                                    }).Distinct().ToArray();
                var editfilelist = _GeneralContext.NAAC_AC_7114_HumanValues_FilesDMO.Where(t => t.NCAC7114HUVAL_Id == data.NCAC7114HUVAL_Id).ToList();
                data.editfilelist = editfilelist.ToArray();
                var testfile = _GeneralContext.NAAC_AC_7114_HumanValues_FilesDMO.Where(t => t.NCAC7114HUVAL_Id == data.NCAC7114HUVAL_Id).ToList();
                for (int t = 0; t < testfile.Count; t++)
                {
                    if (testfile[t].NCAC7114HUVALF_StatusFlg == "approved")
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
            
        }

        public NAAC_AC_7114_HumanValues_DTO deleteuploadfile(NAAC_AC_7114_HumanValues_DTO data)
        {
            try
            {
                List<NAAC_AC_7114_HumanValues_FilesDMO> removelist = new List<NAAC_AC_7114_HumanValues_FilesDMO>();
                removelist = _GeneralContext.NAAC_AC_7114_HumanValues_FilesDMO.Where(t => t.NCAC7114HUVALF_Id == data.NCAC7114HUVALF_Id).ToList();
                foreach(NAAC_AC_7114_HumanValues_FilesDMO obj1 in removelist)
                {
                    _GeneralContext.Remove(obj1);
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

        public NAAC_AC_7114_HumanValues_DTO getData(NAAC_AC_7114_HumanValues_DTO data)
        {
            try
            {
                data.allacademicyear = _GeneralContext.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

                data.alldatalist = (from a in _GeneralContext.NAAC_AC_7114_HumanValuesDMO
                                    from b in _GeneralContext.Academic
                                    where (a.MI_Id == data.MI_Id && a.NCAC7114HUVAL_Year == b.ASMAY_Id && b.MI_Id == a.MI_Id)
                                    select new NAAC_AC_7114_HumanValues_DTO
                                    {
                                        NCAC7114HUVAL_Id = a.NCAC7114HUVAL_Id,
                                        MI_Id = a.MI_Id,
                                        NCAC7114HUVAL_Year = a.NCAC7114HUVAL_Year,
                                        ASMAY_Year = b.ASMAY_Year,
                                        NCAC7114HUVAL_ProgramTitle = a.NCAC7114HUVAL_ProgramTitle,
                                        NCAC7114HUVAL_FromDate = a.NCAC7114HUVAL_FromDate,
                                        NCAC7114HUVAL_ToDate = a.NCAC7114HUVAL_ToDate,
                                        NCAC7114HUVAL_NoOfPartcipants = a.NCAC7114HUVAL_NoOfPartcipants,
                                        NCAC7114HUVAL_ActiveFlg = a.NCAC7114HUVAL_ActiveFlg,
                                        NCAC7114HUVAL_StatusFlg = a.NCAC7114HUVAL_StatusFlg,
                                    }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_7114_HumanValues_DTO getcomment(NAAC_AC_7114_HumanValues_DTO data)
        {
            try
            {

                data.commentlist = (from a in _GeneralContext.NAAC_AC_7114_HumanValues_CommentsDMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCAC7114HUVALC_RemarksBy == b.Id && a.NCAC7114HUVAL_Id == data.NCAC7114HUVAL_Id)
                                    select new NAAC_AC_7114_HumanValues_DTO
                                    {

                                        NCAC7114HUVALC_Remarks = a.NCAC7114HUVALC_Remarks,
                                        NCAC7114HUVAL_Id = a.NCAC7114HUVAL_Id,
                                        NCAC7114HUVALC_RemarksBy = a.NCAC7114HUVALC_RemarksBy,
                                        NCAC7114HUVALC_StatusFlg = a.NCAC7114HUVALC_StatusFlg,
                                        NCAC7114HUVALC_ActiveFlag = a.NCAC7114HUVALC_ActiveFlag,
                                        NCAC7114HUVALC_CreatedBy = a.NCAC7114HUVALC_CreatedBy,
                                        NCAC7114HUVALC_CreatedDate = a.NCAC7114HUVALC_CreatedDate,
                                        NCAC7114HUVALC_UpdatedBy = a.NCAC7114HUVALC_UpdatedBy,
                                        NCAC7114HUVALC_UpdatedDate = a.NCAC7114HUVALC_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_7114_HumanValues_DTO getfilecomment(NAAC_AC_7114_HumanValues_DTO data)
        {
            try
            {

                data.commentlist1 = (from a in _GeneralContext.NAAC_AC_7114_HumanValues_File_CommentsDMO
                                     from b in _GeneralContext.ApplUser
                                     where (a.NCAC7114HUVALFC_RemarksBy == b.Id && a.NCAC7114HUVALF_Id == data.NCAC7114HUVALF_Id)
                                     select new NAAC_AC_7114_HumanValues_DTO
                                     {
                                         NCAC7114HUVALF_Id = a.NCAC7114HUVALF_Id,
                                         NCAC7114HUVALFC_Remarks = a.NCAC7114HUVALFC_Remarks,
                                         NCAC7114HUVALFC_Id = a.NCAC7114HUVALFC_Id,
                                         NCAC7114HUVALFC_RemarksBy = a.NCAC7114HUVALFC_RemarksBy,
                                         NCAC7114HUVALFC_StatusFlg = a.NCAC7114HUVALFC_StatusFlg,
                                         NCAC7114HUVALFC_ActiveFlag = a.NCAC7114HUVALFC_ActiveFlag,
                                         NCAC7114HUVALFC_CreatedBy = a.NCAC7114HUVALFC_CreatedBy,
                                         NCAC7114HUVALFC_CreatedDate = a.NCAC7114HUVALFC_CreatedDate,
                                         NCAC7114HUVALFC_UpdatedBy = a.NCAC7114HUVALFC_UpdatedBy,
                                         NCAC7114HUVALFC_UpdatedDate = a.NCAC7114HUVALFC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_7114_HumanValues_DTO savecomments(NAAC_AC_7114_HumanValues_DTO data)
        {
            try
            {
                NAAC_AC_7114_HumanValues_CommentsDMO obj1 = new NAAC_AC_7114_HumanValues_CommentsDMO();
                obj1.NCAC7114HUVALC_Remarks = data.Remarks;
                obj1.NCAC7114HUVALC_RemarksBy = data.UserId;
                obj1.NCAC7114HUVALC_StatusFlg = "";
                obj1.NCAC7114HUVAL_Id = data.filefkid;
                obj1.NCAC7114HUVALC_ActiveFlag = true;
                obj1.NCAC7114HUVALC_CreatedBy = data.UserId;
                obj1.NCAC7114HUVALC_UpdatedBy = data.UserId;
                obj1.NCAC7114HUVALC_CreatedDate = DateTime.Now;
                obj1.NCAC7114HUVALC_UpdatedDate = DateTime.Now;
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
        public NAAC_AC_7114_HumanValues_DTO savefilewisecomments(NAAC_AC_7114_HumanValues_DTO data)
        {
            try
            {
                NAAC_AC_7114_HumanValues_File_CommentsDMO obj1 = new NAAC_AC_7114_HumanValues_File_CommentsDMO();
                obj1.NCAC7114HUVALFC_Remarks = data.Remarks;
                obj1.NCAC7114HUVALFC_RemarksBy = data.UserId;
                obj1.NCAC7114HUVALFC_StatusFlg = "";
                obj1.NCAC7114HUVALF_Id = data.filefkid;
                obj1.NCAC7114HUVALFC_ActiveFlag = true;
                obj1.NCAC7114HUVALFC_CreatedBy = data.UserId;
                obj1.NCAC7114HUVALFC_UpdatedBy = data.UserId;
                obj1.NCAC7114HUVALFC_CreatedDate = DateTime.Now;
                obj1.NCAC7114HUVALFC_UpdatedDate = DateTime.Now;
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
