
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
    public class MasterLifeSkillAreaImpl : Interfaces.MasterLifeSkillAreaInterface
    {
        private static ConcurrentDictionary<string, mastersubsubjectDTO> _login =
         new ConcurrentDictionary<string, mastersubsubjectDTO>();

        private readonly ExamContext _masterexamContext;

        public MasterLifeSkillAreaImpl(ExamContext masterexamContext)
        {
            _masterexamContext = masterexamContext;
        }

        public MasterLifeSkillAreaDTO Getdetails(MasterLifeSkillAreaDTO data)//int IVRMM_Id
        {

            //try
            //{
            //    data.filldata = _masterexamContext.CCE_Master_Life_Skill_AreasDMO.Where(t =>t.MI_Id==data.MI_Id).OrderBy(t => t.CCE_MLSA_Order).ToArray();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //}
            return data;
            
        }

        

        public MasterLifeSkillAreaDTO savedata(MasterLifeSkillAreaDTO data)
        {
            try
            {
                //if (data.CCE_MLSA_ID != 0)
                //{
                //    var res = _masterexamContext.CCE_Master_Life_Skill_AreasDMO.Where(t => t.CCE_MLSA_NAME == data.CCE_MLSA_NAME  && t.CCE_MLSA_ID != data.CCE_MLSA_ID && t.MI_Id==data.MI_Id).ToList();
                //    if (res.Count > 0)
                //    {
                //        data.returnduplicatestatus = "Duplicate";
                //    }
                //    else
                //    {
                //        var result = _masterexamContext.CCE_Master_Life_Skill_AreasDMO.Single(t => t.CCE_MLSA_ID == data.CCE_MLSA_ID && t.MI_Id==data.MI_Id);
                //        result.CCE_MLSA_NAME = data.CCE_MLSA_NAME;
                //        result.CCE_MLSA_Order = data.CCE_MLSA_Order;
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

                //    var res = _masterexamContext.CCE_Master_Life_Skill_AreasDMO.Where(t => t.CCE_MLSA_NAME == data.CCE_MLSA_NAME  && t.MI_Id==data.MI_Id).ToList();
                //    if (res.Count > 0)
                //    {
                //        data.returnduplicatestatus = "Duplicate";
                //    }
                //    else
                //    {
                //        var row_cnt = _masterexamContext.CCE_Master_Life_Skill_AreasDMO.Where(t => t.MI_Id == data.MI_Id).ToList().Count;
                //        CCE_Master_Life_Skill_AreasDMO skl = new CCE_Master_Life_Skill_AreasDMO();
                //        skl.CCE_MLSA_NAME = data.CCE_MLSA_NAME;
                //        skl.CCE_MLSA_Order = row_cnt+1;
                //        skl.CCE_MLSA_ActiveFlag = true;
                //        skl.MI_Id = data.MI_Id;
                //        skl.CreatedDate = DateTime.Now;
                //        skl.UpdatedDate = DateTime.Now;
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

        public MasterLifeSkillAreaDTO editdetails(int ID)
        {
            MasterLifeSkillAreaDTO editlt = new MasterLifeSkillAreaDTO();
            //try
            //{
            //    List<CCE_Master_Life_Skill_AreasDMO> list = new List<CCE_Master_Life_Skill_AreasDMO>();
            //    list = _masterexamContext.CCE_Master_Life_Skill_AreasDMO.Where(t => t.CCE_MLSA_ID == ID).ToList();
            //    editlt.editlist = list.ToArray();
            //}
            //catch (Exception ee)
            //{
            //    Console.WriteLine(ee);
            //}
            return editlt;
        }


        public MasterLifeSkillAreaDTO validateordernumber(MasterLifeSkillAreaDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                //if (dto.subexamDTO.Count() > 0)
                //{
                //    foreach (MasterLifeSkillAreaDTO mob in dto.subexamDTO)
                //    {
                //        if (mob.CCE_MLSA_ID > 0)
                //        {
                //            var result = _masterexamContext.CCE_Master_Life_Skill_AreasDMO.FirstOrDefault(t => t.CCE_MLSA_ID == mob.CCE_MLSA_ID);

                //            Mapper.Map(mob, result);
                //            result.UpdatedDate = DateTime.Now;
                //            _masterexamContext.Update(result);
                //            _masterexamContext.SaveChanges();
                //        }
                //    }
                //    dto.retrunMsg = "Order Updated Successfully";
                //}
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured"; ;
            }
            return dto;
        }


        public MasterLifeSkillAreaDTO deactivate(MasterLifeSkillAreaDTO data)
        {

            try
            {


                //data.already_cnt = false;

                //if (data.CCE_MLSA_ID > 0)
                //{
                //    var result = _masterexamContext.CCE_Master_Life_Skill_AreasDMO.Single(t => t.CCE_MLSA_ID == data.CCE_MLSA_ID);
                //    if (result.CCE_MLSA_ActiveFlag == true)
                //    {
                //    }
                //    else
                //    {
                //        result.CCE_MLSA_ActiveFlag = true;
                //        result.UpdatedDate = DateTime.Now;
                //        _masterexamContext.Update(result);
                //    }


                //    var flag = _masterexamContext.SaveChanges();
                //    if (flag == 1)
                //    {
                //        data.returnval = true;
                //    }
                //    else
                //    {
                //        data.returnval = false;
                //    }
                //}


            }
            catch (Exception ex)
            {

                throw ex;
            }
            
         
          return data;
        }
    }
}
