using DataAccessMsSqlServerProvider.com.vapstech.ClubManagement;
using DomainModel.Model.com.vapstech.ClubManagement;
using PreadmissionDTOs.com.vaps.ClubManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClubManagement.Services
{
    public class CMS_MasterMemberIMPL : Interfaces.CMS_MasterMemberInerface
    {

        public ClubManagementContext _CmsContext;
        public CMS_MasterMemberIMPL(ClubManagementContext cmsContext)
        {
            _CmsContext = cmsContext;
        }
        public CMS_MastermemberDTO loaddata(int id)
        {
            CMS_MastermemberDTO dto = new CMS_MastermemberDTO();
            try
            {
                dto.catlist = _CmsContext.CMS_Member_CategoryDMO.Where(M => M.MI_Id == id && M.CMSMCAT_ActiveFlag == true
            ).Distinct().ToArray();
                dto.memlist = _CmsContext.CMS_MembershipApplicationDMO.Where(P => P.MI_Id == id && P.CMSMAPPL_ActiveFlag == true).Distinct().ToArray();

                dto.getreport = (from a in _CmsContext.CMS_MasterMemberDMO
                                 from b in _CmsContext.CMS_Member_CategoryDMO
                                 from c in _CmsContext.CMS_MembershipApplicationDMO
                                 where (a.CMSMCAT_Id == b.CMSMCAT_Id && a.CMSMAPPL_Id == c.CMSMAPPL_Id && a.MI_Id == id)
                                 select new CMS_MastermemberDTO
                                 {
                                     CMSMMEM_MemberFirstName = a.CMSMMEM_MemberFirstName + (string.IsNullOrEmpty(a.CMSMMEM_MemberMiddleName) ? "" : ' ' + a.CMSMMEM_MemberMiddleName) + (string.IsNullOrEmpty(a.CMSMMEM_MemberLastName) ? "" : ' ' + a.CMSMMEM_MemberLastName),

                                     CMSMCAT_CategoryName = b.CMSMCAT_CategoryName,
                                     CMSMAPPL_ApplicantsName = c.CMSMAPPL_ApplicantsName,
                                     CMSMMEM_Id = a.CMSMMEM_Id,
                                     CMSMMEM_MembershipNo = a.CMSMMEM_MembershipNo,
                                     CMSMMEM_BiometricCode = a.CMSMMEM_BiometricCode,
                                     CMSMMEM_Proposedby = a.CMSMMEM_Proposedby,
                                     CMSMMEM_ActiveFlag = a.CMSMMEM_ActiveFlag,
                                     CMSMMEM_SpouseMobileNo = a.CMSMMEM_SpouseMobileNo,
                                     CMSMMEM_DOB = a.CMSMMEM_DOB,
                                     CMSMMEM_CreatedDate = a.CMSMMEM_CreatedDate,
                                     CMSMMEM_MembershipExpDate = a.CMSMMEM_MembershipExpDate,


                                 }
                               ).Distinct().OrderByDescending(R => R.CMSMMEM_CreatedDate).ToArray();
                dto.allstate = _CmsContext.State.Distinct().ToArray();
                dto.allcountry = _CmsContext.Country.Distinct().ToArray();
                dto.gender = _CmsContext.gender.Where(T => T.MI_Id == id && T.IVRMMG_ActiveFlag == true).Distinct().ToArray();

                dto.castecategory = _CmsContext.castecategoryDMO.Distinct().ToArray();
                dto.mastercaste = (from a in _CmsContext.castecategoryDMO
                                   from b in _CmsContext.mastercasteDMO
                                   where (a.IMCC_Id == b.IMCC_Id && b.MI_Id == id)
                                   select b
                                  ).Distinct().ToArray();
                dto.allReligion = _CmsContext.MasterReligionDMO.Where(M => M.Is_Active == true).Distinct().ToArray();

                dto.AccdemiYear = _CmsContext.AcademicYearDMO.Where(T => T.MI_Id == id).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                dto.returnval = "admin";
            }
            
            return dto;

        }
        //editmember
        public CMS_MastermemberDTO editmember(CMS_MastermemberDTO data)
        {
            try
            {
                if (data.CMSMMEM_Id > 0)
                {
                    data.editmember = (from a in _CmsContext.CMS_MasterMemberDMO
                                       from b in _CmsContext.CMS_Member_CategoryDMO
                                       from c in _CmsContext.CMS_MembershipApplicationDMO
                                       where (a.CMSMCAT_Id == b.CMSMCAT_Id && a.CMSMAPPL_Id == c.CMSMAPPL_Id && a.CMSMMEM_Id == data.CMSMMEM_Id)
                                       select a).Distinct().ToArray();
                    data.editqulify= (from a in _CmsContext.CMS_MasterMemberDMO
                                      from b in _CmsContext.CMS_Master_Member_QualificationDMO
                                     
                                      where (a.CMSMMEM_Id == data.CMSMMEM_Id &&  a.CMSMMEM_Id==b.CMSMMEM_Id)
                                      select b).Distinct().ToArray();

                    data.editexp = (from a in _CmsContext.CMS_MasterMemberDMO
                                    from b in _CmsContext.CMS_Master_MemberExperienceDMO

                                    where (a.CMSMMEM_Id == data.CMSMMEM_Id && a.CMSMMEM_Id == b.CMSMMEM_Id)
                                    select b).Distinct().ToArray();
                    data.editnumber= (from a in _CmsContext.CMS_MasterMemberDMO
                                      from b in _CmsContext.CMS_Master_MemberMobileNoDMO

                                      where (a.CMSMMEM_Id == data.CMSMMEM_Id && a.CMSMMEM_Id == b.CMSMMEM_Id)
                                      select b).Distinct().ToArray();
                    //editemail
                    data.editemail = (from a in _CmsContext.CMS_MasterMemberDMO
                                       from b in _CmsContext.CMS_Master_Member_EmailIDMO

                                       where (a.CMSMMEM_Id == data.CMSMMEM_Id && a.CMSMMEM_Id == b.CMSMMEM_Id)
                                       select b).Distinct().ToArray();


                    data.editdocument = (from a in _CmsContext.CMS_MasterMemberDMO
                                      from b in _CmsContext.CMS_MasterMember_DocumentsDMO

                                      where (a.CMSMMEM_Id == data.CMSMMEM_Id && a.CMSMMEM_Id == b.CMSMMEM_Id)
                                      select b).Distinct().ToArray();
                }
                else
                {
                    data.returnval = "admin";
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }
        public CMS_MastermemberDTO savedetail1(CMS_MastermemberDTO data)
        {
            try
            {
                if (data.CMSMMEM_Id > 0)
                {
                    var duplicate = _CmsContext.CMS_MasterMemberDMO.Where(P => P.CMSMMEM_MembershipNo == data.CMSMMEM_MembershipNo && P.CMSMMEM_Id != data.CMSMMEM_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "exit";
                    }
                    else
                    {
                        var result = _CmsContext.CMS_MasterMemberDMO.Where(R => R.CMSMMEM_Id != data.CMSMMEM_Id && R.CMSMMEM_MemberFirstName == data.CMSMMEM_MemberFirstName && R.MI_Id == data.MI_Id && R.CMSMMEM_SpouseMobileNo == data.CMSMMEM_SpouseMobileNo && R.CMSMMEM_Id != data.CMSMMEM_Id).ToList();
                        if (result.Count > 0)
                        {
                            data.returnval = "exit";
                        }
                        else
                        {
                            var update = _CmsContext.CMS_MasterMemberDMO.Where(R => R.CMSMMEM_Id == data.CMSMMEM_Id).FirstOrDefault();
                            if (update.CMSMMEM_Id > 0)
                            {
                                update.CMSMCAT_Id = data.CMSMCAT_Id;
                                update.CMSMAPPL_Id = data.CMSMAPPL_Id;
                                update.CMSMMEM_Proposedby = data.CMSMMEM_Proposedby;
                                update.CMSMMEM_MemberFirstName = data.CMSMMEM_MemberFirstName;
                                update.CMSMMEM_MemberMiddleName = data.CMSMMEM_MemberMiddleName;
                                update.CMSMMEM_MemberLastName = data.CMSMMEM_MemberLastName;
                                update.CMSMMEM_MembershipNo = data.CMSMMEM_MembershipNo;
                                update.CMSMMEM_BiometricCode = data.CMSMMEM_BiometricCode;
                                update.CMSMMEM_RFCardId = data.CMSMMEM_RFCardId;
                                update.CMSMMEM_PerAdd1 = data.CMSMMEM_PerAdd1;
                                update.CMSMMEM_PerAdd2 = data.CMSMMEM_PerAdd2;
                                update.CMSMMEM_PerAdd3 = data.CMSMMEM_PerAdd3;
                                update.CMSMMEM_PerAdd4 = data.CMSMMEM_PerAdd4;
                                update.CMSMMEM_PerState = data.CMSMMEM_PerState;
                                update.CMSMMEM_PerCountry = data.CMSMMEM_PerCountry;
                                update.CMSMMEM_PerPincode = data.CMSMMEM_PerPincode;
                                update.CMSMMEM_LacAdd2 = data.CMSMMEM_LacAdd2;
                                update.CMSMMEM_LocAdd1 = data.CMSMMEM_LocAdd1;
                                update.CMSMMEM_LocAdd3 = data.CMSMMEM_LocAdd3;
                                update.CMSMMEM_LocAdd4 = data.CMSMMEM_LocAdd4;
                                update.CMSMMEM_LocState = data.CMSMMEM_LocState;
                                update.CMSMMEM_LocCountry = data.CMSMMEM_LocCountry;
                                update.CMSMMEM_LocPincode = data.CMSMMEM_LocPincode;
                                update.IVRMMMS_Id = data.IVRMMMS_Id;
                                update.IMCC_Id = data.IMCC_Id;
                                update.IVRMMG_Id = data.IVRMMG_Id;
                                update.IMC_Id = data.IMC_Id;
                                update.CMSMMEM_SpouseName = data.CMSMMEM_SpouseName;
                                update.CMSMMEM_MotherName = data.CMSMMEM_MotherName;
                                update.CMSMMEM_FatherName = data.CMSMMEM_FatherName;
                                update.IVRMMR_Id = data.IVRMMR_Id;
                                update.CMSMMEM_SpouseOccupation = data.CMSMMEM_SpouseOccupation;
                                update.CMSMMEM_SpouseMobileNo = data.CMSMMEM_SpouseMobileNo;
                                update.CMSMMEM_SpouseEmailId = data.CMSMMEM_SpouseEmailId;
                                update.CMSMMEM_SpouseAddress = data.CMSMMEM_SpouseAddress;
                                update.CMSMMEM_DOB = data.CMSMMEM_DOB;
                                update.CMSMMEM_BloodGroup = data.CMSMMEM_BloodGroup;
                                update.CMSMMEM_Photo = data.CMSMMEM_Photo;
                                update.CMSMMEM_Height = data.CMSMMEM_Height;
                                update.CMSMMEM_HeightUOM = data.CMSMMEM_HeightUOM;
                                update.CMSMMEM_Weight = data.CMSMMEM_Weight;
                                update.CMSMMEM_WeightUOM = data.CMSMMEM_WeightUOM;
                                update.CMSMMEM_AnyHealthIssue = data.CMSMMEM_AnyHealthIssue;
                                update.CMSMMEM_EyeSightIssue = data.CMSMMEM_EyeSightIssue;
                                update.CMSMMEM_IdentificationMark = data.CMSMMEM_IdentificationMark;
                                update.CMSMMEM_ApproverNo = data.CMSMMEM_ApproverNo;
                                update.CMSMMEM_ApprovedOn = data.CMSMMEM_ApprovedOn;
                                update.CMSMMEM_PANCardNo = data.CMSMMEM_PANCardNo;
                                update.CMSMMEM_AadharCardNo = data.CMSMMEM_AadharCardNo;
                                update.CMSMMEM_NationalSSN = data.CMSMMEM_NationalSSN;
                                update.CMSMMEM_UINo = data.CMSMMEM_UINo;
                                update.CMSMMEM_MembershipExpDate = data.CMSMMEM_MembershipExpDate;
                                update.CMSMMEM_OtherClubMemberFlg = data.CMSMMEM_OtherClubMemberFlg;
                                update.CMSMMEM_BlockedFlg = data.CMSMMEM_BlockedFlg;
                                update.CMSMMEM_TerminatedFlg = data.CMSMMEM_TerminatedFlg;
                                update.CMSMMEM_TerminatedReason = data.CMSMMEM_TerminatedReason;
                                update.CMSMMEM_TerminatedDate = data.CMSMMEM_TerminatedDate;
                                update.CMSMMEM_LeftFlag = data.CMSMMEM_LeftFlag;
                                update.CMSMMEM_DOL = data.CMSMMEM_DOL;
                                update.CMSMMEM_LeavingReason = data.CMSMMEM_LeavingReason;
                                update.CMSMMEM_UpdatedDate = DateTime.Now;
                                update.CMSMMEM_UpdatedBy = data.UserId;
                                _CmsContext.Update(update);
                                var contactExists = _CmsContext.SaveChanges();
                                if (contactExists > 0)
                                {
                                    data.returnval = "update";
                                }
                                else
                                {
                                    data.returnval = "Notupdate";
                                }

                            }
                            else
                            {
                                data.returnval = "admin";
                            }
                        }
                    }

                }
                else
                {
                    var duplicate = _CmsContext.CMS_MasterMemberDMO.Where(P => P.CMSMMEM_MembershipNo == data.CMSMMEM_MembershipNo).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "exit";
                    }
                    else
                    {
                        var result = _CmsContext.CMS_MasterMemberDMO.Where(R => R.CMSMMEM_MemberFirstName == data.CMSMMEM_MemberFirstName && R.MI_Id == data.MI_Id && R.CMSMMEM_SpouseMobileNo == data.CMSMMEM_SpouseMobileNo).ToList();
                        if (result.Count > 0)
                        {
                            data.returnval = "exit";
                        }
                        else
                        {
                            CMS_MasterMemberDMO obj = new CMS_MasterMemberDMO();
                            obj.MI_Id = data.MI_Id;
                            obj.CMSMCAT_Id = data.CMSMCAT_Id;
                            obj.CMSMAPPL_Id = data.CMSMAPPL_Id;
                            obj.CMSMMEM_Proposedby = data.CMSMMEM_Proposedby;
                            obj.CMSMMEM_MemberFirstName = data.CMSMMEM_MemberFirstName;
                            obj.CMSMMEM_MemberMiddleName = data.CMSMMEM_MemberMiddleName;
                            obj.CMSMMEM_MemberLastName = data.CMSMMEM_MemberLastName;
                            obj.CMSMMEM_MembershipNo = data.CMSMMEM_MembershipNo;
                            obj.CMSMMEM_BiometricCode = data.CMSMMEM_BiometricCode;
                            obj.CMSMMEM_RFCardId = data.CMSMMEM_RFCardId;
                            obj.CMSMMEM_PerAdd1 = data.CMSMMEM_PerAdd1;
                            obj.CMSMMEM_PerAdd2 = data.CMSMMEM_PerAdd2;
                            obj.CMSMMEM_PerAdd3 = data.CMSMMEM_PerAdd3;
                            obj.CMSMMEM_PerAdd4 = data.CMSMMEM_PerAdd4;
                            obj.CMSMMEM_PerState = data.CMSMMEM_PerState;
                            obj.CMSMMEM_PerCountry = data.CMSMMEM_PerCountry;
                            obj.CMSMMEM_PerPincode = data.CMSMMEM_PerPincode;
                            obj.CMSMMEM_LacAdd2 = data.CMSMMEM_LacAdd2;
                            obj.CMSMMEM_LocAdd1 = data.CMSMMEM_LocAdd1;
                            obj.CMSMMEM_LocAdd3 = data.CMSMMEM_LocAdd3;
                            obj.CMSMMEM_LocAdd4 = data.CMSMMEM_LocAdd4;
                            obj.CMSMMEM_LocState = data.CMSMMEM_LocState;
                            obj.CMSMMEM_LocCountry = data.CMSMMEM_LocCountry;
                            obj.CMSMMEM_LocPincode = data.CMSMMEM_LocPincode;
                            obj.IVRMMMS_Id = data.IVRMMMS_Id;
                            obj.IMCC_Id = data.IMCC_Id;
                            obj.IVRMMG_Id = data.IVRMMG_Id;
                            obj.IMC_Id = data.IMC_Id;
                            obj.CMSMMEM_SpouseName = data.CMSMMEM_SpouseName;
                            obj.CMSMMEM_MotherName = data.CMSMMEM_MotherName;
                            obj.CMSMMEM_FatherName = data.CMSMMEM_FatherName;
                            obj.IVRMMR_Id = data.IVRMMR_Id;
                            obj.CMSMMEM_SpouseOccupation = data.CMSMMEM_SpouseOccupation;
                            obj.CMSMMEM_SpouseMobileNo = data.CMSMMEM_SpouseMobileNo;
                            obj.CMSMMEM_SpouseEmailId = data.CMSMMEM_SpouseEmailId;
                            obj.CMSMMEM_SpouseAddress = data.CMSMMEM_SpouseAddress;
                            obj.CMSMMEM_DOB = data.CMSMMEM_DOB;
                            obj.CMSMMEM_BloodGroup = data.CMSMMEM_BloodGroup;
                            obj.CMSMMEM_Photo = data.CMSMMEM_Photo;
                            obj.CMSMMEM_Height = data.CMSMMEM_Height;
                            obj.CMSMMEM_HeightUOM = data.CMSMMEM_HeightUOM;
                            obj.CMSMMEM_Weight = data.CMSMMEM_Weight;
                            obj.CMSMMEM_WeightUOM = data.CMSMMEM_WeightUOM;
                            obj.CMSMMEM_AnyHealthIssue = data.CMSMMEM_AnyHealthIssue;
                            obj.CMSMMEM_EyeSightIssue = data.CMSMMEM_EyeSightIssue;
                            obj.CMSMMEM_IdentificationMark = data.CMSMMEM_IdentificationMark;
                            obj.CMSMMEM_ApproverNo = data.CMSMMEM_ApproverNo;
                            obj.CMSMMEM_ApprovedOn = data.CMSMMEM_ApprovedOn;
                            obj.CMSMMEM_PANCardNo = data.CMSMMEM_PANCardNo;
                            obj.CMSMMEM_AadharCardNo = data.CMSMMEM_AadharCardNo;
                            obj.CMSMMEM_NationalSSN = data.CMSMMEM_NationalSSN;
                            obj.CMSMMEM_UINo = data.CMSMMEM_UINo;
                            obj.CMSMMEM_MembershipExpDate = data.CMSMMEM_MembershipExpDate;
                            obj.CMSMMEM_OtherClubMemberFlg = data.CMSMMEM_OtherClubMemberFlg;
                            obj.CMSMMEM_BlockedFlg = data.CMSMMEM_BlockedFlg;
                            obj.CMSMMEM_TerminatedFlg = data.CMSMMEM_TerminatedFlg;
                            obj.CMSMMEM_TerminatedReason = data.CMSMMEM_TerminatedReason;
                            obj.CMSMMEM_TerminatedDate = data.CMSMMEM_TerminatedDate;
                            obj.CMSMMEM_LeftFlag = data.CMSMMEM_LeftFlag;
                            obj.CMSMMEM_DOL = data.CMSMMEM_DOL;
                            obj.CMSMMEM_LeavingReason = data.CMSMMEM_LeavingReason;
                            obj.CMSMMEM_ActiveFlag = true;
                            obj.CMSMMEM_CreatedDate = DateTime.Now;
                            obj.CMSMMEM_CreatedBy = data.UserId;
                            obj.CMSMMEM_UpdatedDate = DateTime.Now;
                            obj.CMSMMEM_UpdatedBy = data.UserId;
                            _CmsContext.Add(obj);
                            var contactExists = _CmsContext.SaveChanges();
                            if (contactExists > 0)
                            {
                                data.CMSMMEM_Id = obj.CMSMMEM_Id;
                                data.returnval = "save";

                            }
                            else
                            {
                                data.returnval = "Notsave";
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;

        }
        //deactive
        public CMS_MastermemberDTO deactive(CMS_MastermemberDTO data)
        {
            try
            {
                if(data.CMSMMEM_Id > 0)
                {
                    var update = _CmsContext.CMS_MasterMemberDMO.Where(R => R.CMSMMEM_Id == data.CMSMMEM_Id).FirstOrDefault();
                   if(update.CMSMMEM_ActiveFlag==true)
                    {
                        update.CMSMMEM_ActiveFlag = false;
                    }
                    else
                    {
                        update.CMSMMEM_ActiveFlag = true;
                    }
                    update.CMSMMEM_UpdatedBy = data.UserId;
                    update.CMSMMEM_UpdatedDate = DateTime.Now;
                    _CmsContext.Update(update);
                    int i = _CmsContext.SaveChanges();
                    if(i > 0)
                    {
                        data.returnval = "active";
                    }
                    else
                    {
                        data.returnval = "notactive";
                    }
                }
                else
                {
                    data.returnval = "admin";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }
        //savedetail2
        public CMS_Master_Member_QualificationDTO savedetail2(CMS_Master_Member_QualificationDTO data)
        {
            try
            {
                if(data.CMSMMEMQULQ_Id > 0)
                {
                    var update = _CmsContext.CMS_Master_Member_QualificationDMO.Where(T => T.CMSMMEMQULQ_Id == data.CMSMMEMQULQ_Id).FirstOrDefault();
                    update.CMSMMEMQUL_QualificationName = data.CMSMMEMQUL_QualificationName;
                    update.CMSMMEMQULQ_CollegeName = data.CMSMMEMQULQ_CollegeName;
                    update.CMSMMEMQULQ_UniversityName = data.CMSMMEMQULQ_UniversityName;
                    update.CMSMMEMQULQ_YearOfPassing = data.CMSMMEMQULQ_YearOfPassing;
                    update.CMSMMEMQULQ_State = data.CMSMMEMQULQ_State;
                    update.CMSMMEMQULQ_Country = data.CMSMMEMQULQ_Country;
                    update.CMSMMEMQULQ_RegistrationNo = data.CMSMMEMQULQ_RegistrationNo;
                    update.CMSMMEMQULQ_Result = data.CMSMMEMQULQ_Result;
                    update.CMSMMEMQULQ_CGPAOrPerFlag = data.CMSMMEMQULQ_CGPAOrPerFlag;
                    update.CMSMMEMQULQ_PHDFlg = data.CMSMMEMQULQ_PHDFlg;
                    update.CMSMMEMQULQ_ThesisTitle = data.CMSMMEMQULQ_ThesisTitle;
                    update.CMSMMEMQULQ_RegistrationYear = data.CMSMMEMQULQ_RegistrationYear;
                    update.CMSMMEMQULQ_GuideName = data.CMSMMEMQULQ_GuideName;
                    update.CMSMMEMQULQ_CGPA = data.CMSMMEMQULQ_CGPA;
                    update.CMSMMEMQULQ_Percentage = data.CMSMMEMQULQ_Percentage;
                    update.CMSMMEMQULQ_AreaOfSpecialisation = data.CMSMMEMQULQ_AreaOfSpecialisation;
                    update.CMSMMEMQULQ_MedicalCouncil = data.CMSMMEMQULQ_MedicalCouncil;
                    update.CMSMMEMQULQ_Date = data.CMSMMEMQULQ_Date;
                    update.CMSMMEMQULQ_Hardcopy = data.CMSMMEMQULQ_Hardcopy;                   
                    update.CMSMMEMQULQ_UpdatedDate = DateTime.Now; ;
                    update.CMSMMEMQULQ_UpdatedBy = data.UserId;
                    _CmsContext.Update(update);
                    var contactExists = _CmsContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        data.returnval = "update";
                    }
                    else
                    {
                        data.returnval = "Notupdate";
                    }

                }
                else
                {
                    CMS_Master_Member_QualificationDMO obj = new CMS_Master_Member_QualificationDMO();
                    obj.CMSMMEM_Id = data.CMSMMEM_Id;
                    obj.CMSMMEMQUL_QualificationName = data.CMSMMEMQUL_QualificationName;
                    obj.CMSMMEMQULQ_CollegeName = data.CMSMMEMQULQ_CollegeName;
                    obj.CMSMMEMQULQ_UniversityName = data.CMSMMEMQULQ_UniversityName;
                    obj.CMSMMEMQULQ_YearOfPassing = data.CMSMMEMQULQ_YearOfPassing;
                    obj.CMSMMEMQULQ_State = data.CMSMMEMQULQ_State;
                    obj.CMSMMEMQULQ_Country = data.CMSMMEMQULQ_Country;
                    obj.CMSMMEMQULQ_RegistrationNo = data.CMSMMEMQULQ_RegistrationNo;
                    obj.CMSMMEMQULQ_Result = data.CMSMMEMQULQ_Result;
                    obj.CMSMMEMQULQ_CGPAOrPerFlag = data.CMSMMEMQULQ_CGPAOrPerFlag;
                    obj.CMSMMEMQULQ_PHDFlg = data.CMSMMEMQULQ_PHDFlg;
                    obj.CMSMMEMQULQ_ThesisTitle = data.CMSMMEMQULQ_ThesisTitle;
                    obj.CMSMMEMQULQ_RegistrationYear = data.CMSMMEMQULQ_RegistrationYear;
                    obj.CMSMMEMQULQ_GuideName = data.CMSMMEMQULQ_GuideName;
                    obj.CMSMMEMQULQ_CGPA = data.CMSMMEMQULQ_CGPA;
                    obj.CMSMMEMQULQ_Percentage = data.CMSMMEMQULQ_Percentage;
                    obj.CMSMMEMQULQ_AreaOfSpecialisation = data.CMSMMEMQULQ_AreaOfSpecialisation;
                    obj.CMSMMEMQULQ_MedicalCouncil = data.CMSMMEMQULQ_MedicalCouncil;
                    obj.CMSMMEMQULQ_Date = data.CMSMMEMQULQ_Date;
                    obj.CMSMMEMQULQ_Hardcopy = data.CMSMMEMQULQ_Hardcopy;
                    obj.CMSMMEMQULQ_ActiveFlg = true;
                    obj.CMSMMEMQULQ_CreatedDate = DateTime.Now;
                    obj.CMSMMEMQULQ_CreatedBy = data.UserId;
                    obj.CMSMMEMQULQ_UpdatedDate= DateTime.Now; ;
                    obj.CMSMMEMQULQ_UpdatedBy = data.UserId;
                    _CmsContext.Add(obj);
                    var contactExists = _CmsContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        data.CMSMMEMQULQ_Id = obj.CMSMMEMQULQ_Id;
                        data.returnval = "save";

                    }
                    else
                    {
                        data.returnval = "Notsave";
                    }



                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }
        //savedetail3
        public CMS_Master_Member_ExperienceDTO savedetail3(CMS_Master_Member_ExperienceDTO data)
        {
            try
            {
                if (data.CMSMMEMEXP_Id > 0)
                {
                    var update = _CmsContext.CMS_Master_MemberExperienceDMO.Where(M => M.CMSMMEMEXP_Id == data.CMSMMEMEXP_Id).FirstOrDefault();
                    update.CMSMMEM_Id = data.CMSMMEM_Id;
                    update.CMSMMEMEXP_OrganisationName = data.CMSMMEMEXP_OrganisationName;
                    update.CMSMMEMEXP_OrganisationAddress = data.CMSMMEMEXP_OrganisationAddress;
                    update.CMSMMEMEXP_Department = data.CMSMMEMEXP_Department;
                    update.CMSMMEMEXP_Designation = data.CMSMMEMEXP_Designation;
                    update.CMSMMEMEXP_JoinDate = data.CMSMMEMEXP_JoinDate;
                    update.CMSMMEMEXP_ExitDate = data.CMSMMEMEXP_ExitDate;
                    update.CMSMMEMEXP_NoOfYears = data.CMSMMEMEXP_NoOfYears;
                    update.CMSMMEMEXP_NoofMonths = data.CMSMMEMEXP_NoofMonths;
                    update.CMSMMEMEXP_AnnualSalary = data.CMSMMEMEXP_AnnualSalary;
                    update.CMSMMEMEXP_ReasonForLeaving = data.CMSMMEMEXP_ReasonForLeaving;
                    _CmsContext.Update(update);
                    var contactExists = _CmsContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        data.returnval = "update";
                    }
                    else
                    {
                        data.returnval = "Notupdate";
                    }

                }
                else
                {
                    CMS_Master_MemberExperienceDMO obj = new CMS_Master_MemberExperienceDMO();
                    obj.CMSMMEM_Id = data.CMSMMEM_Id;
                    obj.CMSMMEMEXP_OrganisationName = data.CMSMMEMEXP_OrganisationName;
                    obj.CMSMMEMEXP_OrganisationAddress = data.CMSMMEMEXP_OrganisationAddress;
                    obj.CMSMMEMEXP_Department = data.CMSMMEMEXP_Department;
                    obj.CMSMMEMEXP_Designation = data.CMSMMEMEXP_Designation;
                    obj.CMSMMEMEXP_JoinDate = data.CMSMMEMEXP_JoinDate;
                    obj.CMSMMEMEXP_ExitDate = data.CMSMMEMEXP_ExitDate;
                    obj.CMSMMEMEXP_NoOfYears = data.CMSMMEMEXP_NoOfYears;
                    obj.CMSMMEMEXP_NoofMonths = data.CMSMMEMEXP_NoofMonths;
                    obj.CMSMMEMEXP_AnnualSalary = data.CMSMMEMEXP_AnnualSalary;
                    obj.CMSMMEMEXP_ReasonForLeaving = data.CMSMMEMEXP_ReasonForLeaving;
                    obj.CMSMMEMEXP_ActiveFlg = true;
                    obj.CMSMMEMEXP_CreatedDate = DateTime.Now;
                    obj.CMSMMEMEXP_CreatedBy = data.UserId;
                    obj.CMSMMEMEXP_UpdatedDate = DateTime.Now;
                    obj.CMSMMEMEXP_UpdatedBy = data.UserId;
                    _CmsContext.Add(obj);
                    var contactExists = _CmsContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        data.CMSMMEMEXP_Id = obj.CMSMMEMEXP_Id;
                        data.returnval = "save";
                    }
                    else
                    {
                        data.returnval = "Notsave";
                    }



                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }
        //savedetail5
        public CMS_Master_MemberMobileNoDTO savedetail5(CMS_Master_MemberMobileNoDTO data)
        {
            try
            {
                if (data.CMSMMEMMN_Id > 0)
                {
                    var update = _CmsContext.CMS_Master_MemberMobileNoDMO.Where(R => R.CMSMMEMMN_Id == data.CMSMMEMMN_Id).FirstOrDefault();
                    update.CMSMMEMMN_MobileNo = data.CMSMMEMMN_MobileNo;
                    update.CMSMMEMMN_DeFaultFlag = data.CMSMMEMMN_DeFaultFlag;
                    update.CMSMMEMMN_UpdatedDate = DateTime.Now;
                    update.CMSMMEMMN_UpdatedBy = data.UserId;
                    _CmsContext.Update(update);
                    var contactExists = _CmsContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        data.returnval = "update";
                    }
                    else
                    {
                        data.returnval = "Notupdate";
                    }

                }
                else
                {
                    CMS_Master_MemberMobileNoDMO obj = new CMS_Master_MemberMobileNoDMO();
                    obj.CMSMMEM_Id = data.CMSMMEM_Id;
                    obj.CMSMMEMMN_MobileNo = data.CMSMMEMMN_MobileNo;
                    obj.CMSMMEMMN_DeFaultFlag = data.CMSMMEMMN_DeFaultFlag;
                    obj.CMSMMEMMN_ActiveFlg = true;
                    obj.CMSMMEMMN_CreatedDate = DateTime.Now;
                    obj.CMSMMEMMN_CreatedBy = data.UserId;
                    obj.CMSMMEMMN_UpdatedDate = DateTime.Now;
                    obj.CMSMMEMMN_UpdatedBy = data.UserId;
                    _CmsContext.Add(obj);
                    var contactExists = _CmsContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        data.CMSMMEMMN_Id = obj.CMSMMEMMN_Id;
                        data.returnval = "save";
                    }
                    else
                    {
                        data.returnval = "Notsave";
                    }



                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }
        public CMS_Master_Member_EmailDTO savedetail6(CMS_Master_Member_EmailDTO data)
        {
            try
            {
                if (data.CMSMMEMEID_Id > 0)
                {
                    var duplicate = _CmsContext.CMS_Master_Member_EmailIDMO.Where(R => R.CMSMMEMEID_EmailId == data.CMSMMEMEID_EmailId && R.CMSMMEMEID_Id !=data.CMSMMEMEID_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "exit";
                    }
                    else
                    {
                        var update = _CmsContext.CMS_Master_Member_EmailIDMO.Where(R => R.CMSMMEMEID_Id == data.CMSMMEMEID_Id).FirstOrDefault();
                        update.CMSMMEM_Id = data.CMSMMEM_Id;
                        update.CMSMMEMEID_EmailId = data.CMSMMEMEID_EmailId;
                        update.CMSMMEMEID_DeFaultFlag = data.CMSMMEMEID_DeFaultFlag;                                             
                        update.CMSMMEMEID_UpdatedDate = DateTime.Now;
                        update.CMSMMEMEID_UpdatedBy = data.UserId;
                        _CmsContext.Update(update);
                        var contactExists = _CmsContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = "update";
                        }
                        else
                        {
                            data.returnval = "Notupdate";
                        }
                    }
                    

                }
                else
                {
                    var duplicate = _CmsContext.CMS_Master_Member_EmailIDMO.Where(R => R.CMSMMEMEID_EmailId == data.CMSMMEMEID_EmailId).ToList();
                    if(duplicate .Count >0)
                    {
                        data.returnval = "exit";
                    }
                    else
                    {
                        CMS_Master_Member_EmailIDMO obj = new CMS_Master_Member_EmailIDMO();
                        obj.CMSMMEM_Id = data.CMSMMEM_Id;
                        obj.CMSMMEMEID_EmailId = data.CMSMMEMEID_EmailId;
                        obj.CMSMMEMEID_DeFaultFlag = data.CMSMMEMEID_DeFaultFlag;
                        obj.CMSMMEMEID_ActiveFlg = true;
                        obj.CMSMMEMEID_CreatedDate = DateTime.Now;
                        obj.CMSMMEMEID_CreatedBy = data.UserId;
                        obj.CMSMMEMEID_UpdatedDate = DateTime.Now;
                        obj.CMSMMEMEID_UpdatedBy = data.UserId;
                        _CmsContext.Add(obj);
                        var contactExists = _CmsContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.CMSMMEMEID_Id = obj.CMSMMEMEID_Id;
                            data.returnval = "save";
                        }
                        else
                        {
                            data.returnval = "Notsave";
                        }
                    }
                   



                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }
        //CMS_MasterMember_DocumentsDTO
        public CMS_MasterMember_DocumentsDTO savedetail7(CMS_MasterMember_DocumentsDTO data)
        {
            try
            {

                if(data.CMSMMEM_Id > 0)
                {
                   
                    if(data.documents !=null)
                    {
                        Array delete = _CmsContext.CMS_MasterMember_DocumentsDMO.Where(a => a.CMSMMEM_Id == data.CMSMMEM_Id).Distinct().ToArray();
                        foreach (var d in delete)
                        {
                            _CmsContext.Remove(d);
                        }
                        for (int f = 0; f < data.documents.Length; f++)
                        {
                            CMS_MasterMember_DocumentsDMO obj = new CMS_MasterMember_DocumentsDMO();
                            obj.CMSMMEM_Id = data.CMSMMEM_Id;
                            obj.CMSMMEMDOC_DocumentName = data.documents[f].CMSMMEMDOC_DocumentName;
                            obj.CMSMMEMDOC_FileName = data.documents[f].CMSMMEMDOC_FileName;
                            obj.CMSMMEMDOC_FilePath = data.documents[f].CMSMMEMDOC_FilePath;
                            obj.CMSMMEMDOC_ActiveFlg = true;
                            obj.CMSMMEMDOC_CreatedDate = DateTime.Now;
                            obj.CMSMMEMDOC_CreatedBy = data.UserId;
                            obj.CMSMMEMDOC_UpdatedDate = DateTime.Now;
                            obj.CMSMMEMDOC_UpdatedBy = data.UserId;
                            _CmsContext.Add(obj);
                        }
                        var contactExists = _CmsContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.CMSMMEM_Idtwo = data.CMSMMEM_Id;
                            data.returnval = "save";
                        }
                        else
                        {
                            data.returnval = "Notsave";
                        }
                    }
                    else
                    {
                        data.returnval = "admin";
                    }
                   
                }
                else
                {
                    data.returnval = "admin";
                }
               

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }
    }
}
