
using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model;
using System.Collections.Concurrent;
using System;
using DomainModel.Model.com.vaps.Exam;
using AutoMapper;
using PreadmissionDTOs.com.vaps.Exam;



namespace ExamServiceHub.com.vaps.Services
{
    public class mastersubsubjectImpl : Interfaces.mastersubsubjectInterface
    {
         private static ConcurrentDictionary<string, mastersubsubjectDTO> _login =
         new ConcurrentDictionary<string, mastersubsubjectDTO>();

         private readonly ExamContext _mastersubsubjectContext;

        public mastersubsubjectImpl(ExamContext mastersubsubjectContext)
        {
            _mastersubsubjectContext = mastersubsubjectContext;
        }

        public mastersubsubjectDTO Getdetails(mastersubsubjectDTO data)//int IVRMM_Id
        {
            mastersubsubjectDTO TTMC = new mastersubsubjectDTO();
            try
            {
                List<mastersubsubjectDMO> obj = new List<mastersubsubjectDMO>();
                obj = _mastersubsubjectContext.mastersubsubject.Where(t => t.MI_Id == data.MI_Id).OrderBy(t => t.EMSS_Order).ToList(); 
                TTMC.getlist = obj.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return TTMC;
        }        
        public mastersubsubjectDTO validateordernumber(mastersubsubjectDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.subsubjectDTO.Count() > 0)
                {
                    foreach (mastersubsubjectDTO mob in dto.subsubjectDTO)
                    {
                        if (mob.EMSS_Id > 0)
                        {
                            var result = _mastersubsubjectContext.mastersubsubject.Single(t => t.EMSS_Id.Equals(mob.EMSS_Id));
                           // result.UpdatedDate = DateTime.Now;
                            Mapper.Map(mob, result);
                            result.UpdatedDate = DateTime.Now;
                            _mastersubsubjectContext.Update(result);
                            _mastersubsubjectContext.SaveChanges();
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

        public mastersubsubjectDTO savedetails(mastersubsubjectDTO data)
        {
           // mastersubsubjectDTO mass = new mastersubsubjectDTO();
            try
            {
                if(data.EMSS_Id > 0)
                {
                    var res = _mastersubsubjectContext.mastersubsubject.Where(t => t.MI_Id == data.MI_Id && (t.EMSS_SubSubjectName == data.EMSS_SubSubjectName || t.EMSS_SubSubjectCode == data.EMSS_SubSubjectCode) && t.EMSS_Id != data.EMSS_Id).ToList();
                    if (res.Count() > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _mastersubsubjectContext.mastersubsubject.Single(t => t.MI_Id == data.MI_Id && t.EMSS_Id == data.EMSS_Id);
                        result.EMSS_SubSubjectName = data.EMSS_SubSubjectName;
                        result.EMSS_SubSubjectCode = data.EMSS_SubSubjectCode;
                        result.UpdatedDate = DateTime.Now;
                        _mastersubsubjectContext.Update(result);
                        var contactExists = _mastersubsubjectContext.SaveChanges();
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
                    var result= _mastersubsubjectContext.mastersubsubject.Where(t => t.MI_Id == data.MI_Id && (t.EMSS_SubSubjectName==data.EMSS_SubSubjectName || t.EMSS_SubSubjectCode==data.EMSS_SubSubjectCode)).ToList();
                    if (result.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var row_cnt = _mastersubsubjectContext.mastersubsubject.Where(t => t.MI_Id == data.MI_Id).ToList().Count;
                        mastersubsubjectDMO obj = new mastersubsubjectDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.EMSS_SubSubjectName = data.EMSS_SubSubjectName;
                        obj.EMSS_SubSubjectCode = data.EMSS_SubSubjectCode;
                        obj.EMSS_Order = row_cnt + 1;
                        obj.EMSS_ActiveFlag = true;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;
                        _mastersubsubjectContext.Add(obj);
                        var contactExists = _mastersubsubjectContext.SaveChanges();
                        if (contactExists == 1)
                        {
                           // mastersubsubjectDMO DTO = Mapper.Map<mastersubsubjectDMO>(obj);
                          //  DTO.EMSS_Order = DTO.EMSS_Id;
                          //  var result1 = _mastersubsubjectContext.mastersubsubject.Single(t => t.EMSS_Id == DTO.EMSS_Id);
                         //   Mapper.Map(DTO, result1);
                          //  _mastersubsubjectContext.Update(result1);
                          //  _mastersubsubjectContext.SaveChanges();
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public mastersubsubjectDTO editdeatils(int ID)
        {
            mastersubsubjectDTO edit = new mastersubsubjectDTO();
            try
            {
                List<mastersubsubjectDMO> masters = new List<mastersubsubjectDMO>();
                masters = _mastersubsubjectContext.mastersubsubject.Where(t => t.EMSS_Id.Equals(ID)).ToList();
                edit.editlist = masters.ToArray();
            }
            catch(Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return edit;
        }

        public mastersubsubjectDTO deactivate(mastersubsubjectDTO data)
        {
            //  mastersubsubjectDTO deact = new mastersubsubjectDTO();
            data.already_cnt = false;
            mastersubsubjectDMO master = Mapper.Map<mastersubsubjectDMO>(data);
            if (master.EMSS_Id > 0)
            {
                var result = _mastersubsubjectContext.mastersubsubject.Single(t => t.EMSS_Id == master.EMSS_Id);
                if (result.EMSS_ActiveFlag == true)
                {
                    var Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO_cnt = _mastersubsubjectContext.Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO.Where(t => t.EMSS_Id == data.EMSS_Id).ToList();
                    var Exm_Login_Privilege_SubSubjectsDMO_cnt = _mastersubsubjectContext.Exm_Login_Privilege_SubSubjectsDMO.Where(t => t.EMSS_Id == data.EMSS_Id).ToList();
                       //  var Exm_Student_Marks_SubExamDMO_cnt = _masterexamContext.Exm_Student_Marks_SubExamDMO.Where(t => t.EMSS_Id == data.EMSS_Id).ToList();
                    // var Exm_Student_Marks_SubSubjectDMO_cnt=_masterexamContext.Exm_Student_Marks_SubSubjectDMO.Where(t => t.EMSS_Id == data.EMSS_Id).ToList();
                    // var Exm_Student_Marks_Pro_Sub_SubSubjectDMO_cnt = _masterexamContext.Exm_Student_Marks_Pro_Sub_SubSubjectDMO.Where(t => t.EMSS_Id == data.EMSS_Id).ToList();
                    if (Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO_cnt.Count == 0 && Exm_Login_Privilege_SubSubjectsDMO_cnt.Count==0) //&& Exm_Student_Marks_Pro_Sub_SubSubjectDMO_cnt.Count==0
                    {
                        result.EMSS_ActiveFlag = false;
                        result.UpdatedDate = DateTime.Now;
                        _mastersubsubjectContext.Update(result);
                    }
                    else
                    {
                        data.already_cnt = true;
                    }
                   // result.EMSS_ActiveFlag = false;
                }
                else
                {
                    result.EMSS_ActiveFlag = true;
                    result.UpdatedDate = DateTime.Now;
                    _mastersubsubjectContext.Update(result);
                }
              //  result.UpdatedDate = DateTime.Now;
              //  _mastersubsubjectContext.Update(result);
                var flag = _mastersubsubjectContext.SaveChanges();
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

    }
}
