
using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using System;
using DomainModel.Model.com.vaps.Exam;
using AutoMapper;
using PreadmissionDTOs.com.vaps.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using Microsoft.Extensions.Logging;

namespace ExamServiceHub.com.vaps.Services
{
    public class MasterLifeSkillImpl : Interfaces.MasterLifeSkillInterface
    {
        private static ConcurrentDictionary<string, mastersubsubjectDTO> _login =
         new ConcurrentDictionary<string, mastersubsubjectDTO>();

        private readonly ExamContext _masterexamContext;
        ILogger<MasterLifeSkillImpl> _log;

        public MasterLifeSkillImpl(ExamContext masterexamContext, ILogger<MasterLifeSkillImpl> log)
        {
            _masterexamContext = masterexamContext;
            _log = log;
        }

        public MasterLifeSkillDTO Getdetails(MasterLifeSkillDTO data)//int IVRMM_Id
        {
            //MasterLifeSkillDTO getdata = new MasterLifeSkillDTO();
            try
            {
                data.filldata = _masterexamContext.CCE_Master_Life_SkillsDMO.Where(t => t.MI_Id == data.MI_Id).ToArray();

                data.getskilldata = _masterexamContext.CCE_Master_Life_Skill_AreasDMO.Where(t => t.MI_Id == data.MI_Id).OrderBy(a => a.ECSA_SkillOrder).ToArray();

                data.fillskill = _masterexamContext.CCE_Master_Life_SkillsDMO.Where(t => t.ECS_ActiveFlag == true && t.MI_Id == data.MI_Id).
                    OrderBy(t => t.ECS_SkillName).ToArray();

                data.fillskillarea = _masterexamContext.CCE_Master_Life_Skill_AreasDMO.Where(t => t.ECSA_ActiveFlag == true && t.MI_Id == data.MI_Id).
                   OrderBy(t => t.ECSA_SkillOrder).ToArray();

                data.fillMastergrade = _masterexamContext.Exm_Master_GradeDMO.Where(t => t.EMGR_ActiveFlag == true && t.MI_Id == data.MI_Id).ToArray();

                data.getyear = _masterexamContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.filldatamapping = (from a in _masterexamContext.CCE_Master_Life_SkillsDMO
                                        from b in _masterexamContext.CCE_Master_Life_Skill_AreasDMO
                                        from c in _masterexamContext.Exm_Master_GradeDMO
                                        from d in _masterexamContext.CCE_Master_Life_Skill_Areas_MappingDMO
                                        from e in _masterexamContext.AcademicYear
                                        where (d.ECS_Id == a.ECS_Id && d.ECSA_Id == b.ECSA_Id && e.ASMAY_Id == d.ASMAY_Id && d.EMGR_Id == c.EMGR_Id
                                        && a.MI_Id == data.MI_Id)
                                        select new MasterLifeSkillDTO
                                        {
                                            ECSAM_Id = d.ECSAM_Id,
                                            ECSA_Id = b.ECSA_Id,
                                            ECSA_SkillArea = b.ECSA_SkillArea,
                                            ECS_Id = a.ECS_Id,
                                            ECS_SkillName = a.ECS_SkillName,
                                            ECSAM_IndicatorDescription = d.ECSAM_IndicatorDescription,
                                            EMGR_Id = c.EMGR_Id,
                                            ECSAM_ActiveFlag = d.ECSAM_ActiveFlag,
                                            EMGR_NAME = c.EMGR_GradeName,
                                            ASMAY_Year = e.ASMAY_Year,
                                            ASMAY_Order = e.ASMAY_Order
                                        }).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();

            }
            catch (Exception ex)
            {
                _log.LogInformation("Exam CCE Master Skill Getdetails" + ex.Message);
                Console.WriteLine(ex);
            }
            return data;

        }
        public MasterLifeSkillDTO savedata(MasterLifeSkillDTO data)
        {
            try
            {
                if (data.ECS_Id != 0)
                {
                    var res = _masterexamContext.CCE_Master_Life_SkillsDMO.Where(t => (t.ECS_SkillName == data.ECS_SkillName || t.ECS_SkillCode == data.ECS_SkillCode) && t.ECS_Id != data.ECS_Id && t.MI_Id == data.MI_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        var result = _masterexamContext.CCE_Master_Life_SkillsDMO.Single(t => t.ECS_Id == data.ECS_Id && t.MI_Id == data.MI_Id);
                        result.ECS_SkillCode = data.ECS_SkillCode;
                        result.ECS_SkillName = data.ECS_SkillName;

                        result.UpdatedDate = DateTime.Now;
                        _masterexamContext.Update(result);
                        var contactExists = _masterexamContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            data.message = "Update";
                            data.returnval = true;
                        }
                        else
                        {
                            data.message = "Update";
                            data.returnval = false;
                        }

                    }
                }
                else
                {
                    var res = _masterexamContext.CCE_Master_Life_SkillsDMO.Where(t => (t.ECS_SkillName == data.ECS_SkillName || t.ECS_SkillCode == data.ECS_SkillCode) && t.MI_Id == data.MI_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {

                        CCE_Master_Life_SkillsDMO skl = new CCE_Master_Life_SkillsDMO();
                        skl.ECS_SkillCode = data.ECS_SkillCode;
                        skl.ECS_SkillName = data.ECS_SkillName;
                        skl.MI_Id = data.MI_Id;
                        skl.ECS_ActiveFlag = true;
                        skl.CreatedDate = DateTime.Now;
                        skl.UpdatedDate = DateTime.Now;
                        _masterexamContext.Add(skl);
                        var contactExists = _masterexamContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            data.message = "Add";
                            data.returnval = true;
                        }
                        else
                        {
                            data.message = "Add";
                            data.returnval = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("Exam CCE Master Skill Save" + ex.Message);
                throw ex;
            }




            return data;
        }
        public MasterLifeSkillDTO editdetails(MasterLifeSkillDTO editlt)
        {
            try
            {
                List<CCE_Master_Life_SkillsDMO> list = new List<CCE_Master_Life_SkillsDMO>();
                list = _masterexamContext.CCE_Master_Life_SkillsDMO.Where(t => t.ECS_Id == editlt.ECS_Id).ToList();
                editlt.editlist = list.ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Exam CCE Master Skill Editdetails" + ex.Message);
                Console.WriteLine(ex.Message);
            }
            return editlt;
        }
        public MasterLifeSkillDTO deactivate(MasterLifeSkillDTO data)
        {
            try
            {
                if (data.ECS_Id > 0)
                {
                    var result = _masterexamContext.CCE_Master_Life_SkillsDMO.Single(t => t.ECS_Id == data.ECS_Id);
                    if (result.ECS_ActiveFlag == true)
                    {
                        var skill_cnt = _masterexamContext.CCE_Master_Life_Skill_Areas_MappingDMO.Where(t => t.ECS_Id == data.ECS_Id).ToList();

                        if (skill_cnt.Count == 0) //&& Exm_Student_Marks_Pro_Sub_SubExamDMO_cnt.Count==0
                        {
                            result.ECS_ActiveFlag = false;
                            result.UpdatedDate = DateTime.Now;
                            _masterexamContext.Update(result);
                        }
                        else
                        {
                            data.ECS_ActiveFlag = true;
                        }
                    }
                    else
                    {
                        result.ECS_ActiveFlag = true;
                        result.UpdatedDate = DateTime.Now;
                        _masterexamContext.Update(result);
                    }
                    var flag = _masterexamContext.SaveChanges();
                    if (flag == 1)
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
                _log.LogInformation("Exam CCE Master Skill activedeactive" + ex.Message);
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //Master Life Skill Area
        public MasterLifeSkillDTO Savedataarea(MasterLifeSkillDTO data)
        {
            try
            {
                if (data.ECSA_Id != 0)
                {
                    var res = _masterexamContext.CCE_Master_Life_Skill_AreasDMO.Where(t => t.ECSA_SkillArea == data.ECSA_SkillArea && t.ECSA_Id != data.ECSA_Id && t.MI_Id == data.MI_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        var result = _masterexamContext.CCE_Master_Life_Skill_AreasDMO.Single(t => t.ECSA_Id == data.ECSA_Id && t.MI_Id == data.MI_Id);
                        result.ECSA_SkillArea = data.ECSA_SkillArea;
                        result.UpdatedDate = DateTime.Now;
                        _masterexamContext.Update(result);
                        var contactExists = _masterexamContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            data.message = "Update";
                            data.returnval = true;
                        }
                        else
                        {
                            data.message = "Update";
                            data.returnval = false;
                        }
                    }
                }
                else
                {

                    var res = _masterexamContext.CCE_Master_Life_Skill_AreasDMO.Where(t => t.ECSA_SkillArea == data.ECSA_SkillArea && t.MI_Id == data.MI_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        var row_cnt = _masterexamContext.CCE_Master_Life_Skill_AreasDMO.Where(t => t.MI_Id == data.MI_Id).ToList().Count;
                        CCE_Master_Life_Skill_AreasDMO skl = new CCE_Master_Life_Skill_AreasDMO();
                        skl.ECSA_SkillArea = data.ECSA_SkillArea;
                        skl.ECSA_SkillOrder = data.ECSA_SkillOrder;
                        skl.ECSA_ActiveFlag = true;
                        skl.MI_Id = data.MI_Id;
                        skl.CreatedDate = DateTime.Now;
                        skl.UpdatedDate = DateTime.Now;
                        _masterexamContext.Add(skl);
                        var contactExists = _masterexamContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            data.message = "Add";
                            data.returnval = true;
                        }
                        else
                        {
                            data.message = "Add";
                            data.returnval = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("Exam CCE Master Life Skill Area Savedataarea" + ex.Message);
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MasterLifeSkillDTO editdetailsarea(MasterLifeSkillDTO editlt)
        {
            try
            {
                List<CCE_Master_Life_Skill_AreasDMO> list = new List<CCE_Master_Life_Skill_AreasDMO>();
                list = _masterexamContext.CCE_Master_Life_Skill_AreasDMO.Where(t => t.ECSA_Id == editlt.ECSA_Id).ToList();
                editlt.editlist = list.ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Exam CCE Master Life Skill Area editdetailsarea" + ex.Message);
                Console.WriteLine(ex.Message);
            }
            return editlt;
        }
        public MasterLifeSkillDTO deactivatearea(MasterLifeSkillDTO data)
        {
            try
            {

                if (data.ECSA_Id > 0)
                {
                    var result = _masterexamContext.CCE_Master_Life_Skill_AreasDMO.Single(t => t.ECSA_Id == data.ECSA_Id);
                    if (result.ECSA_ActiveFlag == true)
                    {
                        result.ECSA_ActiveFlag = false;
                    }
                    else
                    {
                        result.ECSA_ActiveFlag = true;
                        result.UpdatedDate = DateTime.Now;
                        _masterexamContext.Update(result);
                    }


                    var flag = _masterexamContext.SaveChanges();
                    if (flag == 1)
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
                _log.LogInformation("Exam CCE Master Life Skill Area deactivatearea" + ex.Message);
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MasterLifeSkillDTO validateordernumber(MasterLifeSkillDTO data)
        {
            try
            {
                int id = 0;
                for (int i = 0; i < data.subexamDTO.Length; i++)
                {
                    CCE_Master_Life_Skill_AreasDMO dmo = new CCE_Master_Life_Skill_AreasDMO();
                    var result = _masterexamContext.CCE_Master_Life_Skill_AreasDMO.Single(a => a.MI_Id == data.MI_Id && a.ECSA_Id == data.subexamDTO[i].ECSA_Id);
                    id = id + 1;
                    result.ECSA_SkillOrder = id;
                    result.UpdatedDate = DateTime.Now;
                    _masterexamContext.Update(result);
                }
                if (data.subexamDTO.Length > 0)
                {
                    var ik = _masterexamContext.SaveChanges();
                    if (ik > 0)
                    {
                        data.message = "Record Updated Successfully";
                        data.returnval = true;
                    }
                    else
                    {
                        data.message = "Failed To Update Order";
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                data.message = "Failed To Update Order";
                _log.LogInformation("Exam CCE Master Life Skill Area validateordernumber" + ex.Message);
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //Master Life Skill Area Mapping
        public MasterLifeSkillDTO Savedataareamapping(MasterLifeSkillDTO data)
        {
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");           

            try
            {
                if (data.ECSAM_Id > 0)
                {
                    var res = _masterexamContext.CCE_Master_Life_Skill_Areas_MappingDMO.Where(t => t.ECS_Id == data.ECS_Id && t.ECSA_Id == data.ECSA_Id
                    && t.ECSAM_IndicatorDescription == data.ECSAM_IndicatorDescription && t.ECSAM_Id != data.ECSAM_Id && t.MI_Id == data.MI_Id
                    && t.ASMAY_Id == data.ASMAY_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        var result = _masterexamContext.CCE_Master_Life_Skill_Areas_MappingDMO.Single(t => t.ECSAM_Id == data.ECSAM_Id);
                        result.ECSA_Id = data.ECSA_Id;
                        result.ECS_Id = data.ECS_Id;
                        result.ECSAM_IndicatorDescription = data.ECSAM_IndicatorDescription;
                        result.EMGR_Id = data.EMGR_Id;
                        DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                        result.UpdatedDate = indianTime;
                        _masterexamContext.Update(result);
                        var contactExists = _masterexamContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            data.message = "Update";
                            data.returnval = true;
                        }
                        else
                        {
                            data.message = "Update";
                            data.returnval = false;
                        }
                    }
                }
                else
                {
                    var res = _masterexamContext.CCE_Master_Life_Skill_Areas_MappingDMO.Where(t => t.ECS_Id == data.ECS_Id
                    && t.ECSA_Id == data.ECSA_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        CCE_Master_Life_Skill_Areas_MappingDMO skl = new CCE_Master_Life_Skill_Areas_MappingDMO();
                        skl.ECSA_Id = data.ECSA_Id;
                        skl.ECS_Id = data.ECS_Id;
                        skl.MI_Id = data.MI_Id;
                        skl.ASMAY_Id = data.ASMAY_Id;
                        skl.ECSAM_IndicatorDescription = data.ECSAM_IndicatorDescription;
                        skl.EMGR_Id = data.EMGR_Id;
                        DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                        skl.CreatedDate = indianTime;
                        skl.UpdatedDate = indianTime;
                        skl.ECSAM_ActiveFlag = true;
                        _masterexamContext.Add(skl);
                        var contactExists = _masterexamContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            data.message = "Add";
                            data.returnval = true;
                        }
                        else
                        {
                            data.message = "Add";
                            data.returnval = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("Exam CCE Master Life Skill Area Savedataareamapping" + ex.Message);
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MasterLifeSkillDTO editdetailsareamapping(MasterLifeSkillDTO editlt)
        {
            try
            {
                editlt.editlist = (from a in _masterexamContext.CCE_Master_Life_SkillsDMO
                                   from b in _masterexamContext.CCE_Master_Life_Skill_AreasDMO
                                   from d in _masterexamContext.CCE_Master_Life_Skill_Areas_MappingDMO
                                   from e in _masterexamContext.Exm_Master_GradeDMO
                                   from f in _masterexamContext.AcademicYear
                                   where (d.ECS_Id == a.ECS_Id && d.ECSA_Id == b.ECSA_Id && d.ASMAY_Id == f.ASMAY_Id
                                   && d.ECSAM_Id == editlt.ECSAM_Id && e.EMGR_Id == d.EMGR_Id && d.MI_Id == editlt.MI_Id)
                                   select new MasterLifeSkillDTO
                                   {
                                       ECSAM_Id = d.ECSAM_Id,
                                       ECSA_Id = b.ECSA_Id,
                                       ECSA_SkillArea = b.ECSA_SkillArea,
                                       ECS_Id = a.ECS_Id,
                                       ECS_SkillName = a.ECS_SkillName,
                                       ECSAM_IndicatorDescription = d.ECSAM_IndicatorDescription,
                                       EMGR_Id = e.EMGR_Id,
                                       ECSAM_ActiveFlag = d.ECSAM_ActiveFlag,
                                       ASMAY_Id = d.ASMAY_Id
                                   }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Exam CCE Master Life Skill Area editdetailsareamapping" + ex.Message);
                Console.WriteLine(ex.Message);
            }
            return editlt;
        }
        public MasterLifeSkillDTO deactivateareamapping(MasterLifeSkillDTO data)
        {
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            try
            {
                if (data.ECSAM_Id != 0)
                {
                    var result = _masterexamContext.CCE_Master_Life_Skill_Areas_MappingDMO.Single(t => t.ECSAM_Id == data.ECSAM_Id);
                    if (data.ECSAM_ActiveFlag == true)
                    {
                        result.ECSAM_ActiveFlag = false;
                    }
                    if (data.ECSAM_ActiveFlag == false)
                    {
                        result.ECSAM_ActiveFlag = true;
                    }
                    DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                    result.UpdatedDate = indianTime;
                    _masterexamContext.Update(result);
                    var contactExists = _masterexamContext.SaveChanges();
                    if (contactExists == 1)
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
                _log.LogInformation("Exam CCE Master Life Skill Area deactivateareamapping" + ex.Message);
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MasterLifeSkillDTO getgrade(MasterLifeSkillDTO data)
        {
            try
            {
                if (data.EMGR_Id > 0)
                {
                    data.fillgradename = _masterexamContext.Exm_Master_Grade_DetailsDMO.Where(t => t.EMGR_Id == data.EMGR_Id).ToArray();

                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("Exam CCE Master Life Skill Area getgrade" + ex.Message);
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}
