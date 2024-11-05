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
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.NAAC.Admission;

namespace NaacServiceHub.Admission.Services.Criteria7
{
    public class DifferentlyAbledImpl : Interface.Criteria7.DifferentlyAbledInterface
    {

        public GeneralContext _GeneralContext;
        public DifferentlyAbledImpl(GeneralContext para)
        {
            _GeneralContext = para;
        }

        public async  Task<NAAC_AC_719_DifferentlyAbled_DTO> loaddata(NAAC_AC_719_DifferentlyAbled_DTO data)
        {
            try
            {
                data.institutionlist = (from a in _GeneralContext.Institution
                                        from b in _GeneralContext.UserRoleWithInstituteDMO
                                        where (a.MI_Id == b.MI_Id && a.MI_ActiveFlag == 1 && b.Id == data.UserId)
                                        select new NAAC_AC_719_DifferentlyAbled_DTO
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

        public NAAC_AC_719_DifferentlyAbled_DTO savedatatab1(NAAC_AC_719_DifferentlyAbled_DTO data)
        {
            long s = 0;
            try
            {
                if (data.NCAC719DIFFAB_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_719_DifferentlyAbledDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC719DIFFAB_Year == data.NCAC719DIFFAB_Year && t.NCAC719DIFFAB_LIFTFacilityFlg == data.NCAC719DIFFAB_LIFTFacilityFlg && t.NCAC719DIFFAB_PhysicalFacilityFlg == data.NCAC719DIFFAB_PhysicalFacilityFlg && t.NCAC719DIFFAB_BrailleSaoftFlg == data.NCAC719DIFFAB_BrailleSaoftFlg && t.NCAC719DIFFAB_RestRoomFlg == data.NCAC719DIFFAB_RestRoomFlg && t.NCAC719DIFFAB_ExamScribeFlg == data.NCAC719DIFFAB_ExamScribeFlg && t.NCAC719DIFFAB_SPLSkilDevFlg == data.NCAC719DIFFAB_SPLSkilDevFlg && t.NCAC719DIFFAB_RampFacilityFlg == data.NCAC719DIFFAB_RampFacilityFlg && t.NCAC719DIFFAB_OtherFacility == data.NCAC719DIFFAB_OtherFacility).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_719_DifferentlyAbledDMO obj1 = new NAAC_AC_719_DifferentlyAbledDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCAC719DIFFAB_Year = data.NCAC719DIFFAB_Year;
                        obj1.NCAC719DIFFAB_LIFTFacilityFlg = data.NCAC719DIFFAB_LIFTFacilityFlg;
                        obj1.NCAC719DIFFAB_PhysicalFacilityFlg = data.NCAC719DIFFAB_PhysicalFacilityFlg;
                        obj1.NCAC719DIFFAB_BrailleSaoftFlg = data.NCAC719DIFFAB_BrailleSaoftFlg;
                        obj1.NCAC719DIFFAB_RestRoomFlg = data.NCAC719DIFFAB_RestRoomFlg;
                        obj1.NCAC719DIFFAB_ExamScribeFlg = data.NCAC719DIFFAB_ExamScribeFlg;
                        obj1.NCAC719DIFFAB_SPLSkilDevFlg = data.NCAC719DIFFAB_SPLSkilDevFlg;
                        obj1.NCAC719DIFFAB_RampFacilityFlg = data.NCAC719DIFFAB_RampFacilityFlg;
                        obj1.NCAC719DIFFAB_OtherFacility = data.NCAC719DIFFAB_OtherFacility;
                        obj1.NCAC719DIFFAB_ActiveFlg = true;
                        obj1.NCAC719DIFFAB_CreatedBy = data.UserId;
                        obj1.NCAC719DIFFAB_UpdatedBy = data.UserId;
                        obj1.NCAC719DIFFAB_CreatedDate = DateTime.Now;
                        obj1.NCAC719DIFFAB_UpdatedDate = DateTime.Now;
                        _GeneralContext.Add(obj1);
                        _GeneralContext.SaveChanges();
                        s = obj1.NCAC719DIFFAB_Id;
                        if (data.NAACAC7DTO.Count() > 0)
                        {
                            foreach (NAAC_AC_719_DifferentlyAbled_DTO DocumentsDTO in data.NAACAC7DTO)
                            {
                                NAAC_AC_719_DifferentlyAbled_FilesDMO obj2 = new NAAC_AC_719_DifferentlyAbled_FilesDMO();
                                obj2.NCAC719DIFFABF_FileName = DocumentsDTO.NCAC719DIFFABF_FileName;
                                obj2.NCAC719DIFFABF_Filedesc = DocumentsDTO.NCAC719DIFFABF_Filedesc;
                                obj2.NCAC719DIFFABF_FilePath = DocumentsDTO.NCAC719DIFFABF_FilePath;
                                obj2.NCAC719DIFFAB_Id = s;
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
                else if (data.NCAC719DIFFAB_Id > 0)
                {
                    var update = _GeneralContext.NAAC_AC_719_DifferentlyAbledDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC719DIFFAB_Id == data.NCAC719DIFFAB_Id).SingleOrDefault();

                    update.NCAC719DIFFAB_Year = data.NCAC719DIFFAB_Year;
                    update.NCAC719DIFFAB_LIFTFacilityFlg = data.NCAC719DIFFAB_LIFTFacilityFlg;
                    update.NCAC719DIFFAB_PhysicalFacilityFlg = data.NCAC719DIFFAB_PhysicalFacilityFlg;
                    update.NCAC719DIFFAB_BrailleSaoftFlg = data.NCAC719DIFFAB_BrailleSaoftFlg;
                    update.NCAC719DIFFAB_RestRoomFlg = data.NCAC719DIFFAB_RestRoomFlg;
                    update.NCAC719DIFFAB_ExamScribeFlg = data.NCAC719DIFFAB_ExamScribeFlg;
                    update.NCAC719DIFFAB_SPLSkilDevFlg = data.NCAC719DIFFAB_SPLSkilDevFlg;
                    update.NCAC719DIFFAB_RampFacilityFlg = data.NCAC719DIFFAB_RampFacilityFlg;
                    update.NCAC719DIFFAB_OtherFacility = data.NCAC719DIFFAB_OtherFacility;
                    update.NCAC719DIFFAB_UpdatedBy = data.UserId;
                    update.NCAC719DIFFAB_UpdatedDate = DateTime.Now;
                    _GeneralContext.Update(update);
                    _GeneralContext.SaveChanges();
                    s = update.NCAC719DIFFAB_Id;
                    if (data.NAACAC7DTO.Count() > 0)
                    {
                        foreach (NAAC_AC_719_DifferentlyAbled_DTO DocumentsDTO in data.NAACAC7DTO)
                        {
                            if (DocumentsDTO.NCAC719DIFFABF_Id > 0)
                            {
                                var filesdata = _GeneralContext.NAAC_AC_719_DifferentlyAbled_FilesDMO.Where(t => t.NCAC719DIFFABF_Id == DocumentsDTO.NCAC719DIFFABF_Id).FirstOrDefault();
                                filesdata.NCAC719DIFFABF_Filedesc = DocumentsDTO.NCAC719DIFFABF_Filedesc;
                                filesdata.NCAC719DIFFABF_FileName = DocumentsDTO.NCAC719DIFFABF_FileName;
                                filesdata.NCAC719DIFFABF_FilePath = DocumentsDTO.NCAC719DIFFABF_FilePath;
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
                                NAAC_AC_719_DifferentlyAbled_FilesDMO obj2 = new NAAC_AC_719_DifferentlyAbled_FilesDMO();
                                obj2.NCAC719DIFFABF_FileName = DocumentsDTO.NCAC719DIFFABF_FileName;
                                obj2.NCAC719DIFFABF_Filedesc = DocumentsDTO.NCAC719DIFFABF_Filedesc;
                                obj2.NCAC719DIFFABF_FilePath = DocumentsDTO.NCAC719DIFFABF_FilePath;
                                obj2.NCAC719DIFFAB_Id = s;
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

                    //var duplicate = _GeneralContext.NAAC_AC_719_DifferentlyAbledDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC719DIFFAB_Year == data.NCAC719DIFFAB_Year && t.NCAC719DIFFAB_LIFTFacilityFlg == data.NCAC719DIFFAB_LIFTFacilityFlg && t.NCAC719DIFFAB_PhysicalFacilityFlg == data.NCAC719DIFFAB_PhysicalFacilityFlg && t.NCAC719DIFFAB_BrailleSaoftFlg == data.NCAC719DIFFAB_BrailleSaoftFlg && t.NCAC719DIFFAB_RestRoomFlg == data.NCAC719DIFFAB_RestRoomFlg && t.NCAC719DIFFAB_ExamScribeFlg == data.NCAC719DIFFAB_ExamScribeFlg && t.NCAC719DIFFAB_SPLSkilDevFlg == data.NCAC719DIFFAB_SPLSkilDevFlg && t.NCAC719DIFFAB_RampFacilityFlg == data.NCAC719DIFFAB_RampFacilityFlg && t.NCAC719DIFFAB_OtherFacility == data.NCAC719DIFFAB_OtherFacility).ToList();
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

        public NAAC_AC_719_DifferentlyAbled_DTO deactivYTab1(NAAC_AC_719_DifferentlyAbled_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_719_DifferentlyAbledDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC719DIFFAB_Id == data.NCAC719DIFFAB_Id).SingleOrDefault();

                if (result.NCAC719DIFFAB_ActiveFlg == true)
                {
                    result.NCAC719DIFFAB_ActiveFlg = false;
                }
                else if (result.NCAC719DIFFAB_ActiveFlg == false)
                {
                    result.NCAC719DIFFAB_ActiveFlg = true;
                }

                result.NCAC719DIFFAB_UpdatedDate = DateTime.Now;
                result.NCAC719DIFFAB_UpdatedBy = data.UserId;

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

        public NAAC_AC_719_DifferentlyAbled_DTO editTab1(NAAC_AC_719_DifferentlyAbled_DTO data)
        {
            try
            {
                var edit = _GeneralContext.NAAC_AC_719_DifferentlyAbledDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC719DIFFAB_Id == data.NCAC719DIFFAB_Id && t.NCAC719DIFFAB_CodeOfConductDisplayedWebsiteFlag == null && t.NCAC719DIFFAB_CommitteeMonitorAdherenceCodeConductFlag == null && t.NCAC719DIFFAB_ProfProgOrgStuStaffFlag == null && t.NCAC719DIFFAB_AnnualAwsProgConductOrganizedFlag == null).ToList();
                var editfile = _GeneralContext.NAAC_AC_719_DifferentlyAbled_FilesDMO.Where(t => t.NCAC719DIFFAB_Id == data.NCAC719DIFFAB_Id).ToList();

                data.editlisttab1 = edit.ToArray();
                data.editfilelist = editfile.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public NAAC_AC_719_DifferentlyAbled_DTO deleteuploadfile(NAAC_AC_719_DifferentlyAbled_DTO data)
        {
            try
            {
                List<NAAC_AC_719_DifferentlyAbled_FilesDMO> removelist = new List<NAAC_AC_719_DifferentlyAbled_FilesDMO>();
                removelist = _GeneralContext.NAAC_AC_719_DifferentlyAbled_FilesDMO.Where(t => t.NCAC719DIFFABF_Id == data.NCAC719DIFFABF_Id).ToList();
                foreach (NAAC_AC_719_DifferentlyAbled_FilesDMO obj1 in removelist)
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

        public NAAC_AC_719_DifferentlyAbled_DTO getData(NAAC_AC_719_DifferentlyAbled_DTO data)
        {
            try
            {
                data.allacademicyear = _GeneralContext.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

                data.alldatalist = (from a in _GeneralContext.NAAC_AC_719_DifferentlyAbledDMO
                                    from b in _GeneralContext.Academic
                                    where (a.MI_Id == data.MI_Id && a.NCAC719DIFFAB_Year == b.ASMAY_Id && b.MI_Id == a.MI_Id && a.NCAC719DIFFAB_CodeOfConductDisplayedWebsiteFlag == null && a.NCAC719DIFFAB_CommitteeMonitorAdherenceCodeConductFlag == null && a.NCAC719DIFFAB_ProfProgOrgStuStaffFlag == null && a.NCAC719DIFFAB_AnnualAwsProgConductOrganizedFlag == null)
                                    select new NAAC_AC_719_DifferentlyAbled_DTO
                                    {
                                        NCAC719DIFFAB_Id = a.NCAC719DIFFAB_Id,
                                        MI_Id = a.MI_Id,
                                        NCAC719DIFFAB_Year = a.NCAC719DIFFAB_Year,
                                        ASMAY_Year = b.ASMAY_Year,
                                        NCAC719DIFFAB_LIFTFacilityFlg = a.NCAC719DIFFAB_LIFTFacilityFlg,
                                        NCAC719DIFFAB_PhysicalFacilityFlg = a.NCAC719DIFFAB_PhysicalFacilityFlg,
                                        NCAC719DIFFAB_BrailleSaoftFlg = a.NCAC719DIFFAB_BrailleSaoftFlg,
                                        NCAC719DIFFAB_RestRoomFlg = a.NCAC719DIFFAB_RestRoomFlg,
                                        NCAC719DIFFAB_ExamScribeFlg = a.NCAC719DIFFAB_ExamScribeFlg,
                                        NCAC719DIFFAB_SPLSkilDevFlg = a.NCAC719DIFFAB_SPLSkilDevFlg,
                                        NCAC719DIFFAB_RampFacilityFlg = a.NCAC719DIFFAB_RampFacilityFlg,
                                        NCAC719DIFFAB_OtherFacility = a.NCAC719DIFFAB_OtherFacility,
                                        NCAC719DIFFAB_ActiveFlg = a.NCAC719DIFFAB_ActiveFlg
                                    }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public NAAC_AC_719_DifferentlyAbled_DTO getDataMC(NAAC_AC_719_DifferentlyAbled_DTO data)
        {
            try
            {
                data.allacademicyear = _GeneralContext.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

                data.alldatalist = (from a in _GeneralContext.NAAC_AC_719_DifferentlyAbledDMO
                                    from b in _GeneralContext.Academic
                                    where (a.MI_Id == data.MI_Id && a.NCAC719DIFFAB_Year == b.ASMAY_Id && b.MI_Id == a.MI_Id && a.NCAC719DIFFAB_CodeOfConductDisplayedWebsiteFlag != null && a.NCAC719DIFFAB_CommitteeMonitorAdherenceCodeConductFlag != null && a.NCAC719DIFFAB_ProfProgOrgStuStaffFlag != null && a.NCAC719DIFFAB_AnnualAwsProgConductOrganizedFlag != null)
                                    select new NAAC_AC_719_DifferentlyAbled_DTO
                                    {
                                        NCAC719DIFFAB_Id = a.NCAC719DIFFAB_Id,
                                        MI_Id = a.MI_Id,
                                        NCAC719DIFFAB_Year = a.NCAC719DIFFAB_Year,
                                        ASMAY_Year = b.ASMAY_Year,
                                        NCAC719DIFFAB_CodeOfConductDisplayedWebsiteFlag = a.NCAC719DIFFAB_CodeOfConductDisplayedWebsiteFlag,
                                        NCAC719DIFFAB_CommitteeMonitorAdherenceCodeConductFlag = a.NCAC719DIFFAB_CommitteeMonitorAdherenceCodeConductFlag,
                                        NCAC719DIFFAB_ProfProgOrgStuStaffFlag = a.NCAC719DIFFAB_ProfProgOrgStuStaffFlag,
                                        NCAC719DIFFAB_AnnualAwsProgConductOrganizedFlag = a.NCAC719DIFFAB_AnnualAwsProgConductOrganizedFlag,
                                        NCAC719DIFFAB_ActiveFlg = a.NCAC719DIFFAB_ActiveFlg
                                    }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public NAAC_AC_719_DifferentlyAbled_DTO saveMC(NAAC_AC_719_DifferentlyAbled_DTO data)
        {
            long s = 0;
            try
            {
                if (data.NCAC719DIFFAB_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_719_DifferentlyAbledDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC719DIFFAB_Year == data.NCAC719DIFFAB_Year && t.NCAC719DIFFAB_CodeOfConductDisplayedWebsiteFlag == data.NCAC719DIFFAB_CodeOfConductDisplayedWebsiteFlag && t.NCAC719DIFFAB_CommitteeMonitorAdherenceCodeConductFlag == data.NCAC719DIFFAB_CommitteeMonitorAdherenceCodeConductFlag && t.NCAC719DIFFAB_ProfProgOrgStuStaffFlag == data.NCAC719DIFFAB_ProfProgOrgStuStaffFlag && t.NCAC719DIFFAB_AnnualAwsProgConductOrganizedFlag == data.NCAC719DIFFAB_AnnualAwsProgConductOrganizedFlag).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_719_DifferentlyAbledDMO obj1 = new NAAC_AC_719_DifferentlyAbledDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCAC719DIFFAB_Year = data.NCAC719DIFFAB_Year;
                        obj1.NCAC719DIFFAB_CodeOfConductDisplayedWebsiteFlag = data.NCAC719DIFFAB_CodeOfConductDisplayedWebsiteFlag;
                        obj1.NCAC719DIFFAB_CommitteeMonitorAdherenceCodeConductFlag = data.NCAC719DIFFAB_CommitteeMonitorAdherenceCodeConductFlag;
                        obj1.NCAC719DIFFAB_ProfProgOrgStuStaffFlag = data.NCAC719DIFFAB_ProfProgOrgStuStaffFlag;
                        obj1.NCAC719DIFFAB_AnnualAwsProgConductOrganizedFlag = data.NCAC719DIFFAB_AnnualAwsProgConductOrganizedFlag;
                        obj1.NCAC719DIFFAB_ActiveFlg = true;
                        obj1.NCAC719DIFFAB_CreatedBy = data.UserId;
                        obj1.NCAC719DIFFAB_UpdatedBy = data.UserId;
                        obj1.NCAC719DIFFAB_CreatedDate = DateTime.Now;
                        obj1.NCAC719DIFFAB_UpdatedDate = DateTime.Now;
                        _GeneralContext.Add(obj1);
                        _GeneralContext.SaveChanges();
                        s = obj1.NCAC719DIFFAB_Id;
                        if (s > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else if (data.NCAC719DIFFAB_Id > 0)
                {
                    var update = _GeneralContext.NAAC_AC_719_DifferentlyAbledDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC719DIFFAB_Id == data.NCAC719DIFFAB_Id).SingleOrDefault();

                    update.NCAC719DIFFAB_Year = data.NCAC719DIFFAB_Year;
                    update.NCAC719DIFFAB_CodeOfConductDisplayedWebsiteFlag = data.NCAC719DIFFAB_CodeOfConductDisplayedWebsiteFlag;
                    update.NCAC719DIFFAB_CommitteeMonitorAdherenceCodeConductFlag = data.NCAC719DIFFAB_CommitteeMonitorAdherenceCodeConductFlag;
                    update.NCAC719DIFFAB_ProfProgOrgStuStaffFlag = data.NCAC719DIFFAB_ProfProgOrgStuStaffFlag;
                    update.NCAC719DIFFAB_AnnualAwsProgConductOrganizedFlag = data.NCAC719DIFFAB_AnnualAwsProgConductOrganizedFlag;
                    update.NCAC719DIFFAB_UpdatedBy = data.UserId;
                    update.NCAC719DIFFAB_UpdatedDate = DateTime.Now;
                    _GeneralContext.Update(update);
                    _GeneralContext.SaveChanges();
                    s = update.NCAC719DIFFAB_Id;
                    if (s > 0)
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

        public NAAC_AC_719_DifferentlyAbled_DTO EditDataMC(NAAC_AC_719_DifferentlyAbled_DTO data)
        {
            try
            {
                var edit = _GeneralContext.NAAC_AC_719_DifferentlyAbledDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC719DIFFAB_Id == data.NCAC719DIFFAB_Id && t.NCAC719DIFFAB_CodeOfConductDisplayedWebsiteFlag != null && t.NCAC719DIFFAB_CommitteeMonitorAdherenceCodeConductFlag != null && t.NCAC719DIFFAB_ProfProgOrgStuStaffFlag != null && t.NCAC719DIFFAB_AnnualAwsProgConductOrganizedFlag != null).ToList();
                data.editlisttab1 = edit.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
