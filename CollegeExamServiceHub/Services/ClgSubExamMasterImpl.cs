using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.College.Exam;
using DomainModel.Model.com.vaps.Exam;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeExamServiceHub.Services
{
    public class ClgSubExamMasterImpl:Interfaces.ClgSubExamMasterInterface
    {
        private static ConcurrentDictionary<string, mastersubsubjectDTO> _login =
       new ConcurrentDictionary<string, mastersubsubjectDTO>();

        private readonly ClgExamContext _masterexamContext;

        public ClgSubExamMasterImpl(ClgExamContext masterexamContext)
        {
            _masterexamContext = masterexamContext;
        }

        public mastersubexamDTO Getdetails(mastersubexamDTO data)
        {
            mastersubexamDTO getdata = new mastersubexamDTO();
            try
            {
                List<mastersubexamDMO> gtlist = new List<mastersubexamDMO>();
                gtlist = _masterexamContext.clg_mastersubexam.Where(t => t.MI_Id == data.MI_Id).OrderBy(t => t.EMSE_SubExamOrder).ToList();
                getdata.getlist = gtlist.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return getdata;
        }
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
                            var result = _masterexamContext.clg_mastersubexam.FirstOrDefault(t => t.EMSE_Id == mob.EMSE_Id);
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

        public mastersubexamDTO editdetails(int ID)
        {
            mastersubexamDTO editlt = new mastersubexamDTO();
            try
            {
                List<mastersubexamDMO> gtlist = new List<mastersubexamDMO>();
                gtlist = _masterexamContext.clg_mastersubexam.Where(t => t.EMSE_Id == ID).ToList();
                editlt.editlist = gtlist.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return editlt;
        }

        public mastersubexamDTO deactivate(mastersubexamDTO data)
        {
            data.already_cnt = false;
            mastersubexamDMO master = Mapper.Map<mastersubexamDMO>(data);
            if (master.EMSE_Id > 0)
            {
                var result = _masterexamContext.clg_mastersubexam.Single(t => t.EMSE_Id == master.EMSE_Id);
                if (result.EMSE_ActiveFlag == true)
                {
                    var Exm_Yrly_Cat_Exams_Subwise_SubExamsDMO_cnt = _masterexamContext.Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO.Where(t => t.EMSE_Id == data.EMSE_Id).ToList();
                    if (Exm_Yrly_Cat_Exams_Subwise_SubExamsDMO_cnt.Count == 0) 
                    {
                        result.EMSE_ActiveFlag = false;
                        result.UpdatedDate = DateTime.Now;
                        _masterexamContext.Update(result);
                    }
                    else
                    {
                        data.already_cnt = true;
                    }
                }
                else
                {
                    result.EMSE_ActiveFlag = true;
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

            return data;
        }

        public mastersubexamDTO savedetails(mastersubexamDTO data)
        {
            if (data.EMSE_Id != 0)
            {
                var result = _masterexamContext.clg_mastersubexam.Where(t => t.MI_Id == data.MI_Id && (t.EMSE_SubExamCode == data.EMSE_SubExamCode || t.EMSE_SubExamName == data.EMSE_SubExamName) && t.EMSE_Id != data.EMSE_Id).ToList();
                if (result.Count > 0)
                {
                    data.returnduplicatestatus = "Duplicate";
                }
                else
                {
                    var result101 = _masterexamContext.clg_mastersubexam.Single(t => t.MI_Id == data.MI_Id && t.EMSE_Id == data.EMSE_Id);
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
                var result = _masterexamContext.clg_mastersubexam.Where(t => t.MI_Id == data.MI_Id && (t.EMSE_SubExamCode == data.EMSE_SubExamCode || t.EMSE_SubExamName == data.EMSE_SubExamName)).ToList();
                if (result.Count > 0)
                {
                    data.returnduplicatestatus = "Duplicate";
                }
                else
                {
                    var row_cnt = _masterexamContext.clg_mastersubexam.Where(t => t.MI_Id == data.MI_Id).ToList().Count;
                    mastersubexamDMO MM3 = new mastersubexamDMO();
                    MM3.EMSE_SubExamName = data.EMSE_SubExamName;
                    MM3.EMSE_SubExamCode = data.EMSE_SubExamCode;
                    MM3.EMSE_SubExamOrder = row_cnt + 1; ;
                    MM3.MI_Id = data.MI_Id;
                    MM3.EMSE_ActiveFlag = true;
                    MM3.CreatedDate = DateTime.Now;
                    MM3.UpdatedDate = DateTime.Now;
                    _masterexamContext.Add(MM3);
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
            return data;
        }

    }
}
