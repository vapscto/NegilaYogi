
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
    public class mastersubexamImpl : Interfaces.mastersubexamInterface
    {
        private static ConcurrentDictionary<string, mastersubsubjectDTO> _login =
        new ConcurrentDictionary<string, mastersubsubjectDTO>();

        private readonly ExamContext _masterexamContext;

        public mastersubexamImpl(ExamContext masterexamContext)
        {
            _masterexamContext = masterexamContext;
        }

        public mastersubexamDTO Getdetails(mastersubexamDTO data)//int IVRMM_Id
        {
            mastersubexamDTO getdata = new mastersubexamDTO();
            try
            {
                List<mastersubexamDMO> gtlist = new List<mastersubexamDMO>();
                gtlist = _masterexamContext.mastersubexam.Where(t => t.MI_Id == data.MI_Id).OrderBy(t=> t.EMSE_SubExamOrder).ToList();
                getdata.getlist = gtlist.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return getdata;
        }

        //Onchange 
        //Added by Ramesh
        public mastersubexamDTO validateordernumber(mastersubexamDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.subexamDTO.Count() > 0)
                {
                    foreach (mastersubexamDTO mob in dto.subexamDTO)
                    {
                        if (mob.EMSE_Id > 0)
                        {
                            var result = _masterexamContext.mastersubexam.FirstOrDefault(t => t.EMSE_Id == mob.EMSE_Id);
                            //result.UpdatedDate = DateTime.Now;
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

        public mastersubexamDTO editdeatils(int ID)
        {
            mastersubexamDTO editlt = new mastersubexamDTO();
            try
            {
                List<mastersubexamDMO> gtlist = new List<mastersubexamDMO>();
                gtlist = _masterexamContext.mastersubexam.Where(t => t.EMSE_Id == ID).ToList();
                editlt.editlist = gtlist.ToArray();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return editlt;
        }

        public mastersubexamDTO deactivate(mastersubexamDTO data)
        {
            //mastersubexamDTO deact = new mastersubexamDTO();
            data.already_cnt = false;
            mastersubexamDMO master = Mapper.Map<mastersubexamDMO>(data);
            if (master.EMSE_Id > 0)
            {
                var result = _masterexamContext.mastersubexam.Single(t => t.EMSE_Id == master.EMSE_Id);
                if (result.EMSE_ActiveFlag == true)
                {
                    var Exm_Yrly_Cat_Exams_Subwise_SubExamsDMO_cnt = _masterexamContext.Exm_Yrly_Cat_Exams_Subwise_SubExamsDMO.Where(t => t.EMSE_Id == data.EMSE_Id).ToList();
                   // var Exm_Student_Marks_Pro_Sub_SubExamDMO_cnt = _masterexamContext.Exm_Student_Marks_Pro_Sub_SubExamDMO.Where(t => t.EMSE_Id == data.EMSE_Id).ToList();
                    if (Exm_Yrly_Cat_Exams_Subwise_SubExamsDMO_cnt.Count == 0) //&& Exm_Student_Marks_Pro_Sub_SubExamDMO_cnt.Count==0
                    {
                        result.EMSE_ActiveFlag = false;
                        result.UpdatedDate = DateTime.Now;
                        _masterexamContext.Update(result);
                    }
                    else
                    {
                        data.already_cnt = true;
                    }

                    
                   // result.EMSE_ActiveFlag = false;
                }
                else
                {
                    result.EMSE_ActiveFlag = true;
                    result.UpdatedDate = DateTime.Now;
                    _masterexamContext.Update(result);
                }

                //result.UpdatedDate = DateTime.Now;
                //_masterexamContext.Update(result);
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

            return data;
        }

        public mastersubexamDTO savedetails(mastersubexamDTO data)
        {
            //mastersubexamDTO sada = new mastersubexamDTO();

            if (data.EMSE_Id != 0)
            {
                var result = _masterexamContext.mastersubexam.Where(t => t.MI_Id == data.MI_Id && (t.EMSE_SubExamCode == data.EMSE_SubExamCode || t.EMSE_SubExamName == data.EMSE_SubExamName) && t.EMSE_Id != data.EMSE_Id).ToList();
                if (result.Count > 0)
                {
                    data.returnduplicatestatus = "Duplicate";
                }
                else
                {
                    var result101 = _masterexamContext.mastersubexam.Single(t => t.MI_Id == data.MI_Id && t.EMSE_Id == data.EMSE_Id);
                    result101.EMSE_SubExamName = data.EMSE_SubExamName;
                    result101.EMSE_SubExamCode = data.EMSE_SubExamCode;
                    result101.UpdatedDate = DateTime.Now;
                    _masterexamContext.Update(result101);
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
            else
            {
                var result = _masterexamContext.mastersubexam.Where(t => t.MI_Id == data.MI_Id && (t.EMSE_SubExamCode == data.EMSE_SubExamCode || t.EMSE_SubExamName == data.EMSE_SubExamName)).ToList();
                if (result.Count > 0)
                {
                    data.returnduplicatestatus = "Duplicate";
                }
                else
                {
                    var row_cnt = _masterexamContext.mastersubexam.Where(t => t.MI_Id == data.MI_Id).ToList().Count;
                    mastersubexamDMO MM3 = new mastersubexamDMO();
                    MM3.EMSE_SubExamName = data.EMSE_SubExamName;
                    MM3.EMSE_SubExamCode = data.EMSE_SubExamCode;
                    MM3.EMSE_SubExamOrder= row_cnt + 1; ;
                    MM3.MI_Id = data.MI_Id;
                    MM3.EMSE_ActiveFlag = true;
                    MM3.CreatedDate = DateTime.Now;
                    MM3.UpdatedDate = DateTime.Now;
                    _masterexamContext.Add(MM3);
                    var flag = _masterexamContext.SaveChanges();
                    if (flag == 1)
                    {
                        // Order ID
                     //   mastersubexamDMO DTO = Mapper.Map<mastersubexamDMO>(MM3);
                     //   DTO.EMSE_SubExamOrder = DTO.EMSE_Id;
                     //   var result1 = _masterexamContext.mastersubexam.Single(t => t.EMSE_Id == DTO.EMSE_Id);
                    //    Mapper.Map(DTO, result1);
                    //    _masterexamContext.Update(DTO);
                     //   _masterexamContext.SaveChanges();
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }


            return data;
        }

    }
}
