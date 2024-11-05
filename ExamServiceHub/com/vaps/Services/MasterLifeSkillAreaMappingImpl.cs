
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

namespace ExamServiceHub.com.vaps.Services
{
    public class MasterLifeSkillAreaMappingImpl : Interfaces.MasterLifeSkillAreaMappingInterface
    {
        private static ConcurrentDictionary<string, mastersubsubjectDTO> _login =
         new ConcurrentDictionary<string, mastersubsubjectDTO>();

        private readonly ExamContext _masterexamContext;

        public MasterLifeSkillAreaMappingImpl(ExamContext masterexamContext)
        {
            _masterexamContext = masterexamContext;
        }

        public MasterLifeSkillAreaMappingDTO Getdetails(MasterLifeSkillAreaMappingDTO data)//int IVRMM_Id
        {

            try
            {
                //data.fillskill = _masterexamContext.CCE_Master_Life_SkillsDMO.Where(t => t.ECS_ActiveFlag == true && t.MI_Id == data.MI_Id).
                //    OrderBy(t => t.ECS_SkillName).ToArray();
                //data.fillskillarea = _masterexamContext.CCE_Master_Life_Skill_AreasDMO.Where(t => t.CCE_MLSA_ActiveFlag == true && t.MI_Id == data.MI_Id).
                //   OrderBy(t => t.CCE_MLSA_Order).ToArray();

                //data.fillMastergrade = _masterexamContext.Exm_Master_GradeDMO.Where(t => t.EMGR_ActiveFlag == true && t.MI_Id == data.MI_Id).ToArray();


                //data.filldata = (from a in _masterexamContext.CCE_Master_Life_SkillsDMO
                //                 from b in _masterexamContext.CCE_Master_Life_Skill_AreasDMO
                //                 from c in _masterexamContext.Exm_Master_Grade_DetailsDMO
                //                 from d in _masterexamContext.CCE_Master_Life_Skill_Areas_MappingDMO
                //                 where (d.CCE_MLS_ID == a.ECS_Id && d.CCE_MLSA_ID == b.CCE_MLSA_ID && d.EMGD_Id == c.EMGD_Id)
                //                 select new MasterLifeSkillAreaMappingDTO
                //                 {
                //                     CCE_MLSAMap_id = d.CCE_MLSAMap_id,
                //                     CCE_MLSA_ID = b.CCE_MLSA_ID,
                //                     CCE_MLSA_NAME = b.CCE_MLSA_NAME,
                //                     CCE_MLS_ID = a.ECS_Id,
                //                     CCE_MLS_NAME = a.ECS_SkillName,
                //                     CCE_Indicator_Description = d.CCE_Indicator_Description,
                //                     EMGD_Id = c.EMGD_Id,
                //                     CCE_MLSAMap_ActiveFlag = d.CCE_MLSAMap_ActiveFlag,
                //                     EMGR_NAME = c.EMGD_Name,
                //                     EMGR_Point = c.EMGD_GradePoints.ToString()
                //                 }
                //    ).ToArray();





            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;

        }




        public MasterLifeSkillAreaMappingDTO getgrade(MasterLifeSkillAreaMappingDTO data)
        {
            try
            {
                if (data.EMGD_Id > 0)
                {
                    data.fillgradename = _masterexamContext.Exm_Master_Grade_DetailsDMO.Where(t => t.EMGR_Id == data.EMGD_Id).ToArray();

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }




            return data;
        }


        public MasterLifeSkillAreaMappingDTO savedata(MasterLifeSkillAreaMappingDTO data)
        {
            try
            {
                //if (data.CCE_MLSAMap_id != 0)
                //{
                //    var res = _masterexamContext.CCE_Master_Life_Skill_Areas_MappingDMO.Where(t => (t.ECSAM_Id == data.CCE_MLS_ID && 
                //    t.CCE_MLSA_ID == data.CCE_MLSA_ID && t.EMGR_Id == data.EMGD_Id && t.ECSAM_IndicatorDescription==data.CCE_Indicator_Description) && t.CCE_MLSAMap_id != data.CCE_MLSAMap_id).ToList();
                //    if (res.Count > 0)
                //    {
                //        data.returnduplicatestatus = "Duplicate";
                //    }
                //    else
                //    {
                //        var result = _masterexamContext.CCE_Master_Life_Skill_Areas_MappingDMO.Single(t => t.CCE_MLSAMap_id == data.CCE_MLSAMap_id);
                //        result.CCE_MLSA_ID = data.CCE_MLSA_ID;
                //        result.CCE_MLS_ID = data.CCE_MLS_ID;
                //        result.CCE_Indicator_Description = data.CCE_Indicator_Description;
                //        result.EMGD_Id = data.EMGD_Id;
                //        result.UpdatedDate = DateTime.Now;
                //        _masterexamContext.Update(result);
                //        var contactExists = _masterexamContext.SaveChanges();
                //        if (contactExists == 1)
                //        {
                //            data.returnval = true;
                //        }
                //        else
                //        {
                //            data.returnval = false;
                //        }

                //    }
                //}
                //else
                //{

                //    var res = _masterexamContext.CCE_Master_Life_Skill_Areas_MappingDMO.Where(t => (t.CCE_MLS_ID == data.CCE_MLS_ID && t.CCE_MLSA_ID == data.CCE_MLSA_ID && t.EMGD_Id == data.EMGD_Id && t.CCE_Indicator_Description == data.CCE_Indicator_Description)).ToList();
                //    if (res.Count > 0)
                //    {
                //        data.returnduplicatestatus = "Duplicate";
                //    }
                //    else
                //    {

                //        CCE_Master_Life_Skill_Areas_MappingDMO skl = new CCE_Master_Life_Skill_Areas_MappingDMO();
                //        skl.CCE_MLSA_ID = data.CCE_MLSA_ID;
                //        skl.CCE_MLS_ID = data.CCE_MLS_ID;
                //        skl.CCE_Indicator_Description = data.CCE_Indicator_Description;
                //        skl.EMGD_Id = data.EMGD_Id;
                //        skl.CreatedDate = DateTime.Now;
                //        skl.UpdatedDate = DateTime.Now;
                //        skl.CCE_MLSAMap_ActiveFlag = true;
                //        _masterexamContext.Add(skl);
                //        var contactExists = _masterexamContext.SaveChanges();
                //        if (contactExists == 1)
                //        {

                //            data.returnval = true;
                //        }
                //        else
                //        {
                //            data.returnval = false;
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {

                throw ex;
            }




            return data;
        }

        public MasterLifeSkillAreaMappingDTO editdetails(int ID)
        {
            MasterLifeSkillAreaMappingDTO editlt = new MasterLifeSkillAreaMappingDTO();
            try
            {
                //editlt.editlist = (from a in _masterexamContext.CCE_Master_Life_SkillsDMO
                //                   from b in _masterexamContext.CCE_Master_Life_Skill_AreasDMO
                //                   from c in _masterexamContext.Exm_Master_Grade_DetailsDMO
                //                   from d in _masterexamContext.CCE_Master_Life_Skill_Areas_MappingDMO
                //                   from e in _masterexamContext.Exm_Master_GradeDMO
                //                   where (d.CCE_MLS_ID == a.ECS_Id && d.CCE_MLSA_ID == b.CCE_MLSA_ID && d.EMGD_Id == c.EMGD_Id && d.CCE_MLSAMap_id == ID && c.EMGR_Id == e.EMGR_Id)
                //                   select new MasterLifeSkillAreaMappingDTO
                //                   {
                //                       CCE_MLSAMap_id = d.CCE_MLSAMap_id,
                //                       CCE_MLSA_ID = b.CCE_MLSA_ID,
                //                       CCE_MLSA_NAME = b.CCE_MLSA_NAME,
                //                       CCE_MLS_ID = a.ECS_Id,
                //                       CCE_MLS_NAME = a.ECS_SkillName,
                //                       CCE_Indicator_Description = d.CCE_Indicator_Description,
                //                       EMGD_Id = c.EMGD_Id,
                //                       EMGR_NAME = c.EMGD_Name,
                //                       EMGR_Id = e.EMGR_Id,
                //                       CCE_MLSAMap_ActiveFlag = d.CCE_MLSAMap_ActiveFlag,
                //                       EMGR_Point = c.EMGD_GradePoints.ToString()
                //                   }
                //    ).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
            return editlt;
        }

        public MasterLifeSkillAreaMappingDTO deactivate(MasterLifeSkillAreaMappingDTO data)
        {

            try
            {

                //if (data.CCE_MLSAMap_id != 0)
                //{
                //    var result = _masterexamContext.CCE_Master_Life_Skill_Areas_MappingDMO.Single(t => t.CCE_MLSAMap_id == data.CCE_MLSAMap_id);
                //    if (data.CCE_MLSAMap_ActiveFlag == true)
                //    {
                //        result.CCE_MLSAMap_ActiveFlag = false;
                //    }


                //    if (data.CCE_MLSAMap_ActiveFlag == false)
                //    {
                //        result.CCE_MLSAMap_ActiveFlag = true;
                //    }
                //    //result.UpdatedDate = DateTime.Now;
                //    _masterexamContext.Update(result);
                //    var contactExists = _masterexamContext.SaveChanges();
                //    if (contactExists == 1)
                //    {
                //        data.returnval = true;
                //    }
                //    else
                //    {
                //        data.returnval = false;
                //    }

                //}

            }
            catch (Exception)
            {

                throw;
            }


            return data;
        }
    }
}
