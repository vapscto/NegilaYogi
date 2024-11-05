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
    public class GenderEquityImpl : Interface.Criteria7.GenderEquityInterface
    {

        public GeneralContext _GeneralContext;
        public GenderEquityImpl(GeneralContext para)
        {
            _GeneralContext = para;
        }

        public async  Task<NAAC_AC_711_GenderEquity_DTO> loaddata(NAAC_AC_711_GenderEquity_DTO data)
        {
            try
            {
                data.institutionlist = (from a in _GeneralContext.Institution
                                        from b in _GeneralContext.UserRoleWithInstituteDMO
                                        where (a.MI_Id == b.MI_Id && a.MI_ActiveFlag == 1 && b.Id == data.UserId)
                                        select new NAAC_AC_711_GenderEquity_DTO
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

        public NAAC_AC_711_GenderEquity_DTO savedatatab1(NAAC_AC_711_GenderEquity_DTO data)
        {
            long s = 0;
            int flag = 0;
            try
            {
                if (data.NCAC711GENEQ_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_711_GenderEquityDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC711GENEQ_Year == data.NCAC711GENEQ_Year && t.NCAC711GENEQ_ProgramTitle == data.NCAC711GENEQ_ProgramTitle && t.NCAC711GENEQ_FromDate == data.NCAC711GENEQ_FromDate && t.NCAC711GENEQ_ToDate == data.NCAC711GENEQ_ToDate && t.NCAC711GENEQ_NoOfParticipantsMale == data.NCAC711GENEQ_NoOfParticipantsMale && t.NCAC711GENEQ_NoOfParticipantsFeMale == data.NCAC711GENEQ_NoOfParticipantsFeMale).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_711_GenderEquityDMO obj1 = new NAAC_AC_711_GenderEquityDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCAC711GENEQ_Year = data.NCAC711GENEQ_Year;
                        obj1.NCAC711GENEQ_ProgramTitle = data.NCAC711GENEQ_ProgramTitle;
                        obj1.NCAC711GENEQ_FromDate = data.NCAC711GENEQ_FromDate;
                        obj1.NCAC711GENEQ_ToDate = data.NCAC711GENEQ_ToDate;
                        obj1.NCAC711GENEQ_NoOfParticipantsMale = data.NCAC711GENEQ_NoOfParticipantsMale;
                        obj1.NCAC711GENEQ_NoOfParticipantsFeMale = data.NCAC711GENEQ_NoOfParticipantsFeMale;
                        obj1.NCAC711GENEQ_ActiveFlg = true;
                        obj1.NCAC711GENEQ_StatusFlg = "Pending";
                        obj1.NCAC711GENEQ_CreatedBy = data.UserId;
                        obj1.NCAC711GENEQ_UpdatedBy = data.UserId;
                        obj1.NCAC711GENEQ_CreatedDate = DateTime.Now;
                        obj1.NCAC711GENEQ_UpdatedDate = DateTime.Now;
                        _GeneralContext.Add(obj1);
                        _GeneralContext.SaveChanges();
                        s = obj1.NCAC711GENEQ_Id;
                        if (data.NAACAC711GenderEquityDTO.Count() > 0)
                        {
                            foreach (NAAC_AC_711_GenderEquity_DTO DocumentsDTO in data.NAACAC711GenderEquityDTO)
                            {
                                NAAC_AC_711_GenderEquity_FilesDMO obj2 = new NAAC_AC_711_GenderEquity_FilesDMO();
                                obj2.NCAC711GENEQF_FileName = DocumentsDTO.NCAC711GENEQF_FileName;
                                obj2.NCAC711GENEQF_Filedesc = DocumentsDTO.NCAC711GENEQF_Filedesc;
                                obj2.NCAC711GENEQF_FilePath = DocumentsDTO.NCAC711GENEQF_FilePath;
                                obj2.NCAC711GENEQ_Id = s;
                                _GeneralContext.Add(obj2);
                                flag = _GeneralContext.SaveChanges();
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
                else if (data.NCAC711GENEQ_Id > 0)
                {
                    var update = _GeneralContext.NAAC_AC_711_GenderEquityDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC711GENEQ_Id == data.NCAC711GENEQ_Id).SingleOrDefault();

                    update.NCAC711GENEQ_Year = data.NCAC711GENEQ_Year;
                    update.NCAC711GENEQ_ProgramTitle = data.NCAC711GENEQ_ProgramTitle;
                    update.NCAC711GENEQ_FromDate = data.NCAC711GENEQ_FromDate;
                    update.NCAC711GENEQ_ToDate = data.NCAC711GENEQ_ToDate;
                    update.NCAC711GENEQ_NoOfParticipantsMale = data.NCAC711GENEQ_NoOfParticipantsMale;
                    update.NCAC711GENEQ_NoOfParticipantsFeMale = data.NCAC711GENEQ_NoOfParticipantsFeMale;
                    update.NCAC711GENEQ_UpdatedBy = data.UserId;
                    update.NCAC711GENEQ_UpdatedDate = DateTime.Now;
                    _GeneralContext.Update(update);
                    _GeneralContext.SaveChanges();
                    s = update.NCAC711GENEQ_Id;
                    if (data.NAACAC711GenderEquityDTO.Count() > 0)
                    {
                        foreach (NAAC_AC_711_GenderEquity_DTO DocumentsDTO in data.NAACAC711GenderEquityDTO)
                        {
                            if (DocumentsDTO.NCAC711GENEQF_Id > 0)
                            {
                                var filesdata = _GeneralContext.NAAC_AC_711_GenderEquity_FilesDMO.Where(t => t.NCAC711GENEQF_Id == DocumentsDTO.NCAC711GENEQF_Id).FirstOrDefault();
                                filesdata.NCAC711GENEQF_FileName = DocumentsDTO.NCAC711GENEQF_FileName;
                                filesdata.NCAC711GENEQF_Filedesc = DocumentsDTO.NCAC711GENEQF_Filedesc;
                                filesdata.NCAC711GENEQF_FilePath = DocumentsDTO.NCAC711GENEQF_FilePath;
                                _GeneralContext.Update(filesdata);
                                flag = _GeneralContext.SaveChanges();
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
                                NAAC_AC_711_GenderEquity_FilesDMO obj2 = new NAAC_AC_711_GenderEquity_FilesDMO();
                                obj2.NCAC711GENEQF_FileName = DocumentsDTO.NCAC711GENEQF_FileName;
                                obj2.NCAC711GENEQF_Filedesc = DocumentsDTO.NCAC711GENEQF_Filedesc;
                                obj2.NCAC711GENEQF_FilePath = DocumentsDTO.NCAC711GENEQF_FilePath;
                                obj2.NCAC711GENEQ_Id = s;
                                _GeneralContext.Add(obj2);
                                flag = _GeneralContext.SaveChanges();
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public NAAC_AC_711_GenderEquity_DTO deactivYTab1(NAAC_AC_711_GenderEquity_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_711_GenderEquityDMO.Where(t =>t.MI_Id == data.MI_Id && t.NCAC711GENEQ_Id == data.NCAC711GENEQ_Id).SingleOrDefault();

                if (result.NCAC711GENEQ_ActiveFlg == true)
                {
                    result.NCAC711GENEQ_ActiveFlg = false;
                }
                else if (result.NCAC711GENEQ_ActiveFlg == false)
                {
                    result.NCAC711GENEQ_ActiveFlg = true;
                }

                result.NCAC711GENEQ_UpdatedDate = DateTime.Now;
                result.NCAC711GENEQ_UpdatedBy = data.UserId;

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

        public NAAC_AC_711_GenderEquity_DTO editTab1(NAAC_AC_711_GenderEquity_DTO data)
        {
            try
            {
                var edit = _GeneralContext.NAAC_AC_711_GenderEquityDMO.Where(t => t.NCAC711GENEQ_Id == data.NCAC711GENEQ_Id).ToList();
                data.editlisttab1 = (from a in _GeneralContext.Academic
                                    from b in _GeneralContext.NAAC_AC_711_GenderEquityDMO
                                    where (b.NCAC711GENEQ_Id == data.NCAC711GENEQ_Id && a.ASMAY_Id == b.NCAC711GENEQ_Year && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id)
                                    select new NAAC_AC_711_GenderEquity_DTO
                                    {
                                        NCAC711GENEQ_Id = b.NCAC711GENEQ_Id,
                                        NCAC711GENEQ_Year = b.NCAC711GENEQ_Year,
                                        ASMAY_Year = a.ASMAY_Year,
                                        NCAC711GENEQ_ProgramTitle = b.NCAC711GENEQ_ProgramTitle,
                                        NCAC711GENEQ_NoOfParticipantsMale = b.NCAC711GENEQ_NoOfParticipantsMale,
                                        NCAC711GENEQ_NoOfParticipantsFeMale = b.NCAC711GENEQ_NoOfParticipantsFeMale,
                                        NCAC711GENEQ_StatusFlg = b.NCAC711GENEQ_StatusFlg,
                                        NCAC711GENEQ_FromDate=b.NCAC711GENEQ_FromDate,
                                       NCAC711GENEQ_ToDate=b.NCAC711GENEQ_ToDate
                                    }).Distinct().ToArray();
                var editfilelist = _GeneralContext.NAAC_AC_711_GenderEquity_FilesDMO.Where(t => t.NCAC711GENEQ_Id == data.NCAC711GENEQ_Id).ToList();
                data.editfilelist = editfilelist.ToArray();
                var testfile = _GeneralContext.NAAC_AC_711_GenderEquity_FilesDMO.Where(t => t.NCAC711GENEQ_Id == data.NCAC711GENEQ_Id).ToList();
                for (int t = 0; t < testfile.Count; t++)
                {
                    if (testfile[t].NCAC711GENEQF_StatusFlg == "approved")
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

        public NAAC_AC_711_GenderEquity_DTO deleteuploadfile(NAAC_AC_711_GenderEquity_DTO data)
        {
            try
            {
                List<NAAC_AC_711_GenderEquity_FilesDMO> removelist = new List<NAAC_AC_711_GenderEquity_FilesDMO>();
                removelist = _GeneralContext.NAAC_AC_711_GenderEquity_FilesDMO.Where(t => t.NCAC711GENEQF_Id == data.NCAC711GENEQF_Id).ToList();
                foreach (NAAC_AC_711_GenderEquity_FilesDMO obj1 in removelist)
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

        public NAAC_AC_711_GenderEquity_DTO getData(NAAC_AC_711_GenderEquity_DTO data)
        {
            try
            {
                data.allacademicyear = _GeneralContext.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

                data.alldatalist = (from a in _GeneralContext.NAAC_AC_711_GenderEquityDMO
                                    from b in _GeneralContext.Academic
                                    where (a.MI_Id == data.MI_Id && a.NCAC711GENEQ_Year == b.ASMAY_Id && b.MI_Id == a.MI_Id)
                                    select new NAAC_AC_711_GenderEquity_DTO
                                    {
                                        NCAC711GENEQ_Id = a.NCAC711GENEQ_Id,
                                        MI_Id = a.MI_Id,
                                        NCAC711GENEQ_Year = a.NCAC711GENEQ_Year,
                                        ASMAY_Year = b.ASMAY_Year,
                                        NCAC711GENEQ_ProgramTitle = a.NCAC711GENEQ_ProgramTitle,
                                        NCAC711GENEQ_FromDate = a.NCAC711GENEQ_FromDate,
                                        NCAC711GENEQ_ToDate = a.NCAC711GENEQ_ToDate,
                                        NCAC711GENEQ_NoOfParticipantsMale = a.NCAC711GENEQ_NoOfParticipantsMale,
                                        NCAC711GENEQ_NoOfParticipantsFeMale = a.NCAC711GENEQ_NoOfParticipantsFeMale,
                                        NCAC711GENEQ_ActiveFlg = a.NCAC711GENEQ_ActiveFlg,
                                        NCAC711GENEQ_StatusFlg = a.NCAC711GENEQ_StatusFlg
                                    }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_711_GenderEquity_DTO getcomment(NAAC_AC_711_GenderEquity_DTO data)
        {
            try
            {

                data.commentlist = (from a in _GeneralContext.NAAC_AC_711_GenderEquity_CommentsDMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCAC711GENEQC_RemarksBy == b.Id && a.NCAC711GENEQ_Id == data.NCAC711GENEQ_Id)
                                    select new NAAC_AC_711_GenderEquity_DTO
                                    {

                                        NCAC711GENEQC_Remarks = a.NCAC711GENEQC_Remarks,
                                        NCAC711GENEQC_Id = a.NCAC711GENEQC_Id,
                                        NCAC711GENEQC_RemarksBy = a.NCAC711GENEQC_RemarksBy,
                                        NCAC711GENEQC_StatusFlg = a.NCAC711GENEQC_StatusFlg,
                                        NCAC711GENEQC_ActiveFlag = a.NCAC711GENEQC_ActiveFlag,
                                        NCAC711GENEQC_CreatedBy = a.NCAC711GENEQC_CreatedBy,
                                        NCAC711GENEQC_CreatedDate = a.NCAC711GENEQC_CreatedDate,
                                        NCAC711GENEQC_UpdatedBy = a.NCAC711GENEQC_UpdatedBy,
                                        NCAC711GENEQC_UpdatedDate = a.NCAC711GENEQC_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_711_GenderEquity_DTO getfilecomment(NAAC_AC_711_GenderEquity_DTO data)
        {
            try
            {

                data.commentlist1 = (from a in _GeneralContext.NAAC_AC_711_GenderEquity_File_CommentsDMO
                                     from b in _GeneralContext.ApplUser
                                     where (a.NCAC711GENEQFC_RemarksBy == b.Id && a.NCAC711GENEQF_Id == data.NCAC711GENEQF_Id)
                                     select new NAAC_AC_711_GenderEquity_DTO
                                     {
                                         NCAC711GENEQF_Id = a.NCAC711GENEQF_Id,
                                         NCAC711GENEQFC_Remarks = a.NCAC711GENEQFC_Remarks,
                                         NCAC711GENEQFC_Id = a.NCAC711GENEQFC_Id,
                                         NCAC711GENEQFC_RemarksBy = a.NCAC711GENEQFC_RemarksBy,
                                         NCAC711GENEQFC_StatusFlg = a.NCAC711GENEQFC_StatusFlg,
                                         NCAC711GENEQFC_ActiveFlag = a.NCAC711GENEQFC_ActiveFlag,
                                         NCAC711GENEQFC_CreatedBy = a.NCAC711GENEQFC_CreatedBy,
                                         NCAC711GENEQFC_CreatedDate = a.NCAC711GENEQFC_CreatedDate,
                                         NCAC711GENEQFC_UpdatedBy = a.NCAC711GENEQFC_UpdatedBy,
                                         NCAC711GENEQFC_UpdatedDate = a.NCAC711GENEQFC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_711_GenderEquity_DTO savecomments(NAAC_AC_711_GenderEquity_DTO data)
        {
            try
            {
                NAAC_AC_711_GenderEquity_CommentsDMO obj1 = new NAAC_AC_711_GenderEquity_CommentsDMO();
                obj1.NCAC711GENEQC_Remarks = data.Remarks;
                obj1.NCAC711GENEQC_RemarksBy = data.UserId;
                obj1.NCAC711GENEQC_StatusFlg = "";
                obj1.NCAC711GENEQ_Id = data.filefkid;
                obj1.NCAC711GENEQC_ActiveFlag = true;
                obj1.NCAC711GENEQC_CreatedBy = data.UserId;
                obj1.NCAC711GENEQC_UpdatedBy = data.UserId;
                obj1.NCAC711GENEQC_CreatedDate = DateTime.Now;
                obj1.NCAC711GENEQC_UpdatedDate = DateTime.Now;
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
        public NAAC_AC_711_GenderEquity_DTO savefilewisecomments(NAAC_AC_711_GenderEquity_DTO data)
        {
            try
            {
                NAAC_AC_711_GenderEquity_File_CommentsDMO obj1 = new NAAC_AC_711_GenderEquity_File_CommentsDMO();
                obj1.NCAC711GENEQFC_Remarks = data.Remarks;
                obj1.NCAC711GENEQFC_RemarksBy = data.UserId;
                obj1.NCAC711GENEQFC_StatusFlg = "";
                obj1.NCAC711GENEQF_Id = data.filefkid;
                obj1.NCAC711GENEQFC_ActiveFlag = true;
                obj1.NCAC711GENEQFC_CreatedBy = data.UserId;
                obj1.NCAC711GENEQFC_UpdatedBy = data.UserId;
                obj1.NCAC711GENEQFC_CreatedDate = DateTime.Now;
                obj1.NCAC711GENEQFC_UpdatedDate = DateTime.Now;
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
       public NAAC_AC_711_GenderEquity_DTO viewuploadflies(NAAC_AC_711_GenderEquity_DTO data)
        {

            try
            {
                data.view = _GeneralContext.NAAC_AC_711_GenderEquity_FilesDMO.Where(t => t.NCAC711GENEQ_Id == data.NCAC711GENEQ_Id).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }

    }
}
