using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using DomainModel.Model.NAAC.Admission.Criteria7;
using PreadmissionDTOs.NAAC.Admission.Criteria7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services.Criteria7
{
    public class LocalCommunityImpl : Interface.Criteria7.LocalCommunityInterface
    {
        public GeneralContext _GeneralContext;
        public LocalCommunityImpl(GeneralContext para)
        {
            _GeneralContext = para;
        }
        public async Task<LocalCommunityDTO> loaddata(LocalCommunityDTO data)
        {
            try
            {               
                data.institutionlist = (from a in _GeneralContext.Institution
                                        from b in _GeneralContext.UserRoleWithInstituteDMO
                                        where (a.MI_Id == b.MI_Id && b.Id == data.UserId)
                                        select new LocalCommunityDTO
                                        {
                                            MI_Id = a.MI_Id,
                                            MI_Name = a.MI_Name,
                                            UserId=b.Id
                                        }).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public LocalCommunityDTO getdata(LocalCommunityDTO data)
        {
            try
            {
                data.allacademicyear = _GeneralContext.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

                data.alldatalist = (from a in _GeneralContext.NAAC_AC_7111_LocalCommunityDMO
                                    from b in _GeneralContext.Academic

                                    where (a.MI_Id == data.MI_Id && a.NCAC7111LOCCOM_Year == b.ASMAY_Id && b.MI_Id == a.MI_Id)
                                    select new LocalCommunityDTO
                                    {
                                        NCAC7111LOCCOM_Id = a.NCAC7111LOCCOM_Id,                                       
                                        NCAC7111LOCCOM_Year = a.NCAC7111LOCCOM_Year,
                                        ASMAY_Year = b.ASMAY_Year,
                                        MI_Id = a.MI_Id,
                                        NCAC7111LOCCOM_NoOfEngage = a.NCAC7111LOCCOM_NoOfEngage,
                                        NCAC7111LOCCOM_Duration = a.NCAC7111LOCCOM_Duration,
                                        NCAC7111LOCCOM_IssuesAddressed = a.NCAC7111LOCCOM_IssuesAddressed,
                                        NCAC7111LOCCOM_NoOfAddress = a.NCAC7111LOCCOM_NoOfAddress,
                                        NCAC7111LOCCOM_Date = a.NCAC7111LOCCOM_Date,
                                        NCAC7111LOCCOM_InitiativeName = a.NCAC7111LOCCOM_InitiativeName,
                                        NCAC7111LOCCOM_NoOfParticipant = a.NCAC7111LOCCOM_NoOfParticipant,
                                        NCAC7111LOCCOM_ActiveFlg = a.NCAC7111LOCCOM_ActiveFlg,
                                        NCAC7111LOCCOM_StatusFlg = a.NCAC7111LOCCOM_StatusFlg
                                    }).ToArray();
            }
            catch(Exception exe)
            {
                Console.WriteLine(exe.Message);
            }
            return data;
        }
        public LocalCommunityDTO savedatatab1(LocalCommunityDTO data)
        {
            long s = 0;
            int flag = 0;
            try
            {
                if (data.NCAC7111LOCCOM_Id == 0)
                 {
                    var duplicate = _GeneralContext.NAAC_AC_7111_LocalCommunityDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC7111LOCCOM_Year == data.NCAC7111LOCCOM_Year && t.NCAC7111LOCCOM_NoOfEngage == data.NCAC7111LOCCOM_NoOfEngage && t.NCAC7111LOCCOM_Duration == data.NCAC7111LOCCOM_Duration && t.NCAC7111LOCCOM_IssuesAddressed == data.NCAC7111LOCCOM_IssuesAddressed && t.NCAC7111LOCCOM_NoOfAddress == data.NCAC7111LOCCOM_NoOfAddress && t.NCAC7111LOCCOM_Date == data.NCAC7111LOCCOM_Date && t.NCAC7111LOCCOM_InitiativeName == data.NCAC7111LOCCOM_InitiativeName && t.NCAC7111LOCCOM_NoOfParticipant == data.NCAC7111LOCCOM_NoOfParticipant).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_7111_LocalCommunityDMO obj = new NAAC_AC_7111_LocalCommunityDMO();
                        obj.MI_Id = data.MI_Id;                        
                        obj.NCAC7111LOCCOM_Year = data.NCAC7111LOCCOM_Year;
                        obj.NCAC7111LOCCOM_NoOfEngage = data.NCAC7111LOCCOM_NoOfEngage;
                        obj.NCAC7111LOCCOM_Duration = data.NCAC7111LOCCOM_Duration;
                        obj.NCAC7111LOCCOM_IssuesAddressed = data.NCAC7111LOCCOM_IssuesAddressed;
                        obj.NCAC7111LOCCOM_NoOfAddress = data.NCAC7111LOCCOM_NoOfAddress;
                        obj.NCAC7111LOCCOM_Date = data.NCAC7111LOCCOM_Date;
                        obj.NCAC7111LOCCOM_InitiativeName = data.NCAC7111LOCCOM_InitiativeName;
                        obj.NCAC7111LOCCOM_NoOfParticipant = data.NCAC7111LOCCOM_NoOfParticipant;
                        obj.NCAC7111LOCCOM_ActiveFlg = true;
                        obj.NCAC7111LOCCOM_CreatedBy = data.UserId;
                        obj.NCAC7111LOCCOM_UpdatedBy = data.UserId;
                        obj.NCAC7111LOCCOM_CreatedDate = DateTime.Now;
                        obj.NCAC7111LOCCOM_UpdatedDate = DateTime.Now;
                        _GeneralContext.Add(obj);
                        _GeneralContext.SaveChanges();
                        s = obj.NCAC7111LOCCOM_Id;
                        if (data.NAACAC711LocalCommunityDTO.Count() > 0)
                        {
                            foreach (LocalCommunityDTO DocumentsDTO in data.NAACAC711LocalCommunityDTO)
                            {
                                NAAC_AC_7111_LocalCommunity_FilesDMO obj2 = new NAAC_AC_7111_LocalCommunity_FilesDMO();
                                obj2.NCAC7111LOCCOMF_FileName = DocumentsDTO.NCAC7111LOCCOMF_FileName;
                                obj2.NCAC7111LOCCOMF_Filedesc = DocumentsDTO.NCAC7111LOCCOMF_Filedesc;
                                obj2.NCAC7111LOCCOMF_FilePath = DocumentsDTO.NCAC7111LOCCOMF_FilePath;
                                obj2.NCAC7111LOCCOM_Id = s;
                                _GeneralContext.Add(obj2);
                                flag = _GeneralContext.SaveChanges();
                            }
                            if (flag > 0)
                            {
                                data.returnval = true;
                            }
                            else
                            {
                                data.returnval = false;
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
                else if (data.NCAC7111LOCCOM_Id > 0)
                {
                    var update = _GeneralContext.NAAC_AC_7111_LocalCommunityDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC7111LOCCOM_Id == data.NCAC7111LOCCOM_Id).SingleOrDefault();
                    update.NCAC7111LOCCOM_Year = data.NCAC7111LOCCOM_Year;
                    update.NCAC7111LOCCOM_NoOfEngage = data.NCAC7111LOCCOM_NoOfEngage;
                    update.NCAC7111LOCCOM_Duration = data.NCAC7111LOCCOM_Duration;
                    update.NCAC7111LOCCOM_IssuesAddressed = data.NCAC7111LOCCOM_IssuesAddressed;
                    update.NCAC7111LOCCOM_NoOfAddress = data.NCAC7111LOCCOM_NoOfAddress;
                    update.NCAC7111LOCCOM_Date = data.NCAC7111LOCCOM_Date;
                    update.NCAC7111LOCCOM_InitiativeName = data.NCAC7111LOCCOM_InitiativeName;
                    update.NCAC7111LOCCOM_NoOfParticipant = data.NCAC7111LOCCOM_NoOfParticipant;
                    update.NCAC7111LOCCOM_UpdatedBy = data.UserId;
                    update.NCAC7111LOCCOM_UpdatedDate = DateTime.Now;
                    _GeneralContext.Update(update);
                    s = update.NCAC7111LOCCOM_Id;
                    if (data.NAACAC711LocalCommunityDTO.Count() > 0)
                    {
                        foreach (LocalCommunityDTO DocumentsDTO in data.NAACAC711LocalCommunityDTO)
                        {
                            if (DocumentsDTO.NCAC7111LOCCOMF_Id > 0)
                            {
                                var doclist = _GeneralContext.NAAC_AC_7111_LocalCommunity_FilesDMO.Where(t => t.NCAC7111LOCCOMF_Id == DocumentsDTO.NCAC7111LOCCOMF_Id).FirstOrDefault();
                                doclist.NCAC7111LOCCOMF_FileName = DocumentsDTO.NCAC7111LOCCOMF_FileName;
                                doclist.NCAC7111LOCCOMF_Filedesc = DocumentsDTO.NCAC7111LOCCOMF_Filedesc;
                                doclist.NCAC7111LOCCOMF_FilePath = DocumentsDTO.NCAC7111LOCCOMF_FilePath;
                                _GeneralContext.Update(doclist);
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
                                NAAC_AC_7111_LocalCommunity_FilesDMO obj2 = new NAAC_AC_7111_LocalCommunity_FilesDMO();
                                obj2.NCAC7111LOCCOMF_FileName = DocumentsDTO.NCAC7111LOCCOMF_FileName;
                                obj2.NCAC7111LOCCOMF_Filedesc = DocumentsDTO.NCAC7111LOCCOMF_Filedesc;
                                obj2.NCAC7111LOCCOMF_FilePath = DocumentsDTO.NCAC7111LOCCOMF_FilePath;
                                obj2.NCAC7111LOCCOM_Id = s;
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
        public LocalCommunityDTO edittab1(LocalCommunityDTO data)
        {
            try
            {
                var edit = _GeneralContext.NAAC_AC_7111_LocalCommunityDMO.Where(t => t.NCAC7111LOCCOM_Id == data.NCAC7111LOCCOM_Id).ToList();
                data.editlisttab1 = (from a in _GeneralContext.Academic
                                    from b in _GeneralContext.NAAC_AC_7111_LocalCommunityDMO
                                    where (b.NCAC7111LOCCOM_Id == data.NCAC7111LOCCOM_Id && a.ASMAY_Id == b.NCAC7111LOCCOM_Year && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id)
                                    select new LocalCommunityDTO
                                    {
                                        NCAC7111LOCCOM_Id = b.NCAC7111LOCCOM_Id,
                                        NCAC7111LOCCOM_Year = b.NCAC7111LOCCOM_Year,
                                        ASMAY_Year = a.ASMAY_Year,
                                        NCAC7111LOCCOM_NoOfAddress = b.NCAC7111LOCCOM_NoOfAddress,
                                        NCAC7111LOCCOM_NoOfEngage = b.NCAC7111LOCCOM_NoOfEngage,
                                        NCAC7111LOCCOM_Duration = b.NCAC7111LOCCOM_Duration,
                                        NCAC7111LOCCOM_InitiativeName = b.NCAC7111LOCCOM_InitiativeName,
                                        NCAC7111LOCCOM_IssuesAddressed = b.NCAC7111LOCCOM_IssuesAddressed,
                                        NCAC7111LOCCOM_NoOfParticipant = b.NCAC7111LOCCOM_NoOfParticipant,
                                        NCAC7111LOCCOM_StatusFlg = b.NCAC7111LOCCOM_StatusFlg
                                    }).Distinct().ToArray();
                var editfilelist = _GeneralContext.NAAC_AC_7111_LocalCommunity_FilesDMO.Where(t => t.NCAC7111LOCCOM_Id == data.NCAC7111LOCCOM_Id).ToList();
                data.editfilelist = editfilelist.ToArray();
                var testfile = _GeneralContext.NAAC_AC_7111_LocalCommunity_FilesDMO.Where(t => t.NCAC7111LOCCOM_Id == data.NCAC7111LOCCOM_Id).ToList();
                for (int t = 0; t < testfile.Count; t++)
                {
                    if (testfile[t].NCAC7111LOCCOMF_StatusFlg == "approved")
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
        public LocalCommunityDTO deactivYTab1(LocalCommunityDTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_7111_LocalCommunityDMO.Where(t =>t.NCAC7111LOCCOM_Id == data.NCAC7111LOCCOM_Id).SingleOrDefault();

                if (result.NCAC7111LOCCOM_ActiveFlg == true)
                {
                    result.NCAC7111LOCCOM_ActiveFlg = false;
                }
                else if (result.NCAC7111LOCCOM_ActiveFlg == false)
                {
                    result.NCAC7111LOCCOM_ActiveFlg = true;
                }

                result.NCAC7111LOCCOM_UpdatedDate = DateTime.Now;
                result.NCAC7111LOCCOM_UpdatedBy = data.UserId;

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
        public LocalCommunityDTO deleteuploadfile(LocalCommunityDTO data)
        {
            try
            {
                List<NAAC_AC_7111_LocalCommunity_FilesDMO> removelist = new List<NAAC_AC_7111_LocalCommunity_FilesDMO>();
                removelist = _GeneralContext.NAAC_AC_7111_LocalCommunity_FilesDMO.Where(t => t.NCAC7111LOCCOMF_Id == data.NCAC7111LOCCOMF_Id).ToList();
                foreach (NAAC_AC_7111_LocalCommunity_FilesDMO obj1 in removelist)
                {
                    _GeneralContext.Remove(obj1);
                    _GeneralContext.SaveChanges();
                    data.returnval = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LocalCommunityDTO viewuploadflies(LocalCommunityDTO data)
        {
            try
            {
                data.view = _GeneralContext.NAAC_AC_7111_LocalCommunity_FilesDMO.Where(t => t.NCAC7111LOCCOM_Id == data.NCAC7111LOCCOM_Id).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public LocalCommunityDTO getcomment(LocalCommunityDTO data)
        {
            try
            {

                data.commentlist = (from a in _GeneralContext.NAAC_AC_7111_LocalCommunity_CommentsDMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCAC7111LOCCOMC_RemarksBy == b.Id && a.NCAC7111LOCCOM_Id == data.NCAC7111LOCCOM_Id)
                                    select new LocalCommunityDTO
                                    {

                                        NCAC7111LOCCOMC_Remarks = a.NCAC7111LOCCOMC_Remarks,
                                        NCAC7111LOCCOMC_Id = a.NCAC7111LOCCOMC_Id,
                                        NCAC7111LOCCOMC_RemarksBy = a.NCAC7111LOCCOMC_RemarksBy,
                                        NCAC7111LOCCOMC_StatusFlg = a.NCAC7111LOCCOMC_StatusFlg,
                                        NCAC7111LOCCOMC_ActiveFlag = a.NCAC7111LOCCOMC_ActiveFlag,
                                        NCAC7111LOCCOMC_CreatedBy = a.NCAC7111LOCCOMC_CreatedBy,
                                        NCAC7111LOCCOMC_CreatedDate = a.NCAC7111LOCCOMC_CreatedDate,
                                        NCAC7111LOCCOMC_UpdatedBy = a.NCAC7111LOCCOMC_UpdatedBy,
                                        NCAC7111LOCCOMC_UpdatedDate = a.NCAC7111LOCCOMC_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LocalCommunityDTO getfilecomment(LocalCommunityDTO data)
        {
            try
            {

                data.commentlist1 = (from a in _GeneralContext.NAAC_AC_7111_LocalCommunity_File_CommentsDMO
                                     from b in _GeneralContext.ApplUser
                                     where (a.NCAC7111LOCCOMFC_RemarksBy == b.Id && a.NCAC7111LOCCOMF_Id == data.NCAC7111LOCCOMF_Id)
                                     select new LocalCommunityDTO
                                     {
                                         NCAC7111LOCCOMF_Id = a.NCAC7111LOCCOMF_Id,
                                         NCAC7111LOCCOMFC_Remarks = a.NCAC7111LOCCOMFC_Remarks,
                                         NCAC7111LOCCOMFC_Id = a.NCAC7111LOCCOMFC_Id,
                                         NCAC7111LOCCOMFC_RemarksBy = a.NCAC7111LOCCOMFC_RemarksBy,
                                         NCAC7111LOCCOMFC_StatusFlg = a.NCAC7111LOCCOMFC_StatusFlg,
                                         NCAC7111LOCCOMFC_ActiveFlag = a.NCAC7111LOCCOMFC_ActiveFlag,
                                         NCAC7111LOCCOMFC_CreatedBy = a.NCAC7111LOCCOMFC_CreatedBy,
                                         NCAC7111LOCCOMFC_CreatedDate = a.NCAC7111LOCCOMFC_CreatedDate,
                                         NCAC7111LOCCOMFC_UpdatedBy = a.NCAC7111LOCCOMFC_UpdatedBy,
                                         NCAC7111LOCCOMFC_UpdatedDate = a.NCAC7111LOCCOMFC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LocalCommunityDTO savecomments(LocalCommunityDTO data)
        {
            try
            {
                NAAC_AC_7111_LocalCommunity_CommentsDMO obj1 = new NAAC_AC_7111_LocalCommunity_CommentsDMO();
                obj1.NCAC7111LOCCOMC_Remarks = data.Remarks;
                obj1.NCAC7111LOCCOMC_RemarksBy = data.UserId;
                obj1.NCAC7111LOCCOMC_StatusFlg = "";
                obj1.NCAC7111LOCCOM_Id = data.filefkid;
                obj1.NCAC7111LOCCOMC_ActiveFlag = true;
                obj1.NCAC7111LOCCOMC_CreatedBy = data.UserId;
                obj1.NCAC7111LOCCOMC_UpdatedBy = data.UserId;
                obj1.NCAC7111LOCCOMC_CreatedDate = DateTime.Now;
                obj1.NCAC7111LOCCOMC_UpdatedDate = DateTime.Now;
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
        public LocalCommunityDTO savefilewisecomments(LocalCommunityDTO data)
        {
            try
            {
                NAAC_AC_7111_LocalCommunity_File_CommentsDMO obj1 = new NAAC_AC_7111_LocalCommunity_File_CommentsDMO();
                obj1.NCAC7111LOCCOMFC_Remarks = data.Remarks;
                obj1.NCAC7111LOCCOMFC_RemarksBy = data.UserId;
                obj1.NCAC7111LOCCOMFC_StatusFlg = "";
                obj1.NCAC7111LOCCOMF_Id = data.filefkid;
                obj1.NCAC7111LOCCOMFC_ActiveFlag = true;
                obj1.NCAC7111LOCCOMFC_CreatedBy = data.UserId;
                obj1.NCAC7111LOCCOMFC_UpdatedBy = data.UserId;
                obj1.NCAC7111LOCCOMFC_CreatedDate = DateTime.Now;
                obj1.NCAC7111LOCCOMFC_UpdatedDate = DateTime.Now;
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
