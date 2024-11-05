
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
    public class MasterScholasticActivityImpl : Interfaces.MasterScholasticActivityInterface
    {
        private static ConcurrentDictionary<string, mastersubsubjectDTO> _login =
         new ConcurrentDictionary<string, mastersubsubjectDTO>();

        private readonly ExamContext _masterexamContext;

        public MasterScholasticActivityImpl(ExamContext masterexamContext)
        {
            _masterexamContext = masterexamContext;
        }

        public MasterScholasticActivityDTO Getdetails(MasterScholasticActivityDTO data)//int IVRMM_Id
        {

            try
            {
               data.filldata = _masterexamContext.CCE_M_CoScholasticActivitiesDMO.Where(t =>t.MI_Id==data.MI_Id).OrderBy(t => t.CCE_M_CoA_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
            
        }

        

        public MasterScholasticActivityDTO savedata(MasterScholasticActivityDTO data)
        {
            try
            {
                if (data.CCE_M_CoA_Id != 0)
                {
                    var res = _masterexamContext.CCE_M_CoScholasticActivitiesDMO.Where(t => (t.CCE_M_CoA_Name == data.CCE_M_CoA_Name || t.CCE_M_CoA_Code == data.CCE_M_CoA_Code ) && t.CCE_M_CoA_Id != data.CCE_M_CoA_Id && t.MI_Id == data.MI_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _masterexamContext.CCE_M_CoScholasticActivitiesDMO.Single(t => t.CCE_M_CoA_Id == data.CCE_M_CoA_Id && t.MI_Id == data.MI_Id);
                        result.CCE_M_CoA_Name = data.CCE_M_CoA_Name;
                        result.CCE_M_CoA_Code = data.CCE_M_CoA_Code;
                        result.UpdatedDate = DateTime.Now;
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
                else
                {

                    var res = _masterexamContext.CCE_M_CoScholasticActivitiesDMO.Where(t => (t.CCE_M_CoA_Name == data.CCE_M_CoA_Name || t.CCE_M_CoA_Code == data.CCE_M_CoA_Code ) && t.MI_Id == data.MI_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var row_cnt = _masterexamContext.CCE_M_CoScholasticActivitiesDMO.Where(t => t.MI_Id == data.MI_Id).ToList().Count;
                        CCE_M_CoScholasticActivitiesDMO skl = new CCE_M_CoScholasticActivitiesDMO();
                        skl.CCE_M_CoA_Name = data.CCE_M_CoA_Name;
                        skl.CCE_M_CoA_Code = data.CCE_M_CoA_Code;
                        skl.CCE_M_CoA_Order = row_cnt + 1; ;
                        skl.Active_flag = true;
                        skl.MI_Id = data.MI_Id;
                        skl.CreatedDate = DateTime.Now;
                        skl.UpdatedDate = DateTime.Now;
                        _masterexamContext.Add(skl);
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
            }
            catch (Exception ex)
            {

                throw ex;
            }
           

           

            return data;
        }

        public MasterScholasticActivityDTO validateordernumber(MasterScholasticActivityDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.subexamDTO.Count() > 0)
                {
                    foreach (MasterScholasticActivityDTO mob in dto.subexamDTO)
                    {
                        if (mob.CCE_M_CoA_Id > 0)
                        {
                            var result = _masterexamContext.CCE_M_CoScholasticActivitiesDMO.FirstOrDefault(t => t.CCE_M_CoA_Id == mob.CCE_M_CoA_Id);
                          
                            Mapper.Map(mob, result);
                            result.UpdatedDate = DateTime.Now;
                            _masterexamContext.Update(result);
                            _masterexamContext.SaveChanges();
                        }
                    }
                    dto.retrunMsg = "Order Updated Successfully";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured"; ;
            }
            return dto;
        }





        public MasterScholasticActivityDTO editdetails(int ID)
        {
            MasterScholasticActivityDTO editlt = new MasterScholasticActivityDTO();
            try
            {
                List<CCE_M_CoScholasticActivitiesDMO> list = new List<CCE_M_CoScholasticActivitiesDMO>();
                list = _masterexamContext.CCE_M_CoScholasticActivitiesDMO.Where(t => t.CCE_M_CoA_Id == ID).ToList();
                editlt.editlist = list.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
            return editlt;
        }

        public MasterScholasticActivityDTO deactivate(MasterScholasticActivityDTO data)
        {

            try
            {


                data.already_cnt = false;

                if (data.CCE_M_CoA_Id > 0)
                {
                    var result = _masterexamContext.CCE_M_CoScholasticActivitiesDMO.Single(t => t.CCE_M_CoA_Id == data.CCE_M_CoA_Id);
                    if (result.Active_flag == true)
                    {
                        //var skillarea_cnt = _masterexamContext.CCE_Master_Life_Skill_Areas_MappingDMO.Where(t => t.CCE_MLSA_ID == data.CCE_M_CoA_Id).ToList();

                        //if (skillarea_cnt.Count == 0) //&& Exm_Student_Marks_Pro_Sub_SubExamDMO_cnt.Count==0
                        //{
                        //    result.Active_flag = false;
                        //    result.UpdatedDate = DateTime.Now;
                        //    _masterexamContext.Update(result);
                        //}
                        //else
                        //{
                        //    data.already_cnt = true;
                        //}



                    }
                    else
                    {
                        result.Active_flag = true;
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

                throw ex;
            }
            
         
          return data;
        }
    }
}
