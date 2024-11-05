
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
using DomainModel.Model.com.vapstech.Exam;

namespace ExamServiceHub.com.vaps.Services
{
    public class exammasterImpl : Interfaces.exammasterInterface
    {
        private readonly ExamContext _masterexamContext;
        public exammasterImpl(ExamContext masterexamContext)
        {
            _masterexamContext = masterexamContext;
        }
        public exammasterDTO Getdetails(exammasterDTO data)
        {
            exammasterDTO getdata = new exammasterDTO();
            try
            {
                List<exammasterDMO> list = new List<exammasterDMO>();
                list = _masterexamContext.masterexam.Where(t => t.MI_Id == data.MI_Id).OrderBy(t => t.EME_ExamOrder).ToList();
                getdata.exammastername = list.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return getdata;

        }
        public exammasterDTO validateordernumber(exammasterDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.examDTO.Count() > 0)
                {
                    foreach (exammasterDTO mob in dto.examDTO)
                    {
                        if (mob.EME_Id > 0)
                        {
                            var result = _masterexamContext.masterexam.Single(t => t.EME_Id.Equals(mob.EME_Id));
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
        public exammasterDTO savedetails(exammasterDTO data)
        {
            if (data.EME_Id != 0)
            {
                var res = _masterexamContext.masterexam.Where(t => t.MI_Id == data.MI_Id && (t.EME_ExamName == data.EME_ExamName || t.EME_ExamCode == data.EME_ExamCode) && t.EME_Id != data.EME_Id).ToList();
                if (res.Count > 0)
                {
                    data.returnduplicatestatus = "Duplicate";
                }
                else
                {
                    var result = _masterexamContext.masterexam.Single(t => t.MI_Id == data.MI_Id && t.EME_Id == data.EME_Id);
                    result.EME_ExamCode = data.EME_ExamCode;
                    result.EME_ExamName = data.EME_ExamName;
                    result.EME_FinalExamFlag = data.EME_FinalExamFlag;
                    result.EME_IVRSExamName = data.EME_IVRSExamName;
                    result.EME_ExamDescription = data.EME_ExamDescription;
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
                var res = _masterexamContext.masterexam.Where(t => t.MI_Id == data.MI_Id && (t.EME_ExamName == data.EME_ExamName || t.EME_ExamCode == data.EME_ExamCode)).ToList();
                if (res.Count > 0)
                {
                    data.returnduplicatestatus = "Duplicate";
                }
                else
                {
                    var row_cnt = _masterexamContext.exammasterDMO.Where(t => t.MI_Id == data.MI_Id).ToList().Count;
                    exammasterDMO exm = new exammasterDMO();
                    exm.EME_ExamCode = data.EME_ExamCode;
                    exm.EME_ExamName = data.EME_ExamName;
                    exm.EME_ExamOrder = row_cnt + 1;
                    exm.EME_FinalExamFlag = data.EME_FinalExamFlag;
                    exm.EME_IVRSExamName = data.EME_IVRSExamName;
                    exm.EME_ExamDescription = data.EME_ExamDescription;
                    exm.MI_Id = data.MI_Id;
                    exm.EME_ActiveFlag = true;
                    //exm.EME_FinalExamFlag = true;
                    exm.CreatedDate = DateTime.Now;
                    exm.UpdatedDate = DateTime.Now;
                    _masterexamContext.Add(exm);
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
            return data;
        }
        public exammasterDTO editdetails(int ID)
        {
            exammasterDTO editlt = new exammasterDTO();
            try
            {
                List<exammasterDMO> list = new List<exammasterDMO>();
                list = _masterexamContext.masterexam.Where(t => t.EME_Id == ID).ToList();
                editlt.editlist = list.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
            return editlt;
        }
        public exammasterDTO deactivate(exammasterDTO data)
        {
            data.already_cnt = false;
            exammasterDMO master = Mapper.Map<exammasterDMO>(data);
            if (master.EME_Id > 0)
            {
                var result = _masterexamContext.masterexam.Single(t => t.EME_Id == master.EME_Id);
                if (result.EME_ActiveFlag == true)
                {
                    var Exm_Yearly_Category_ExamsDMO_cnt = _masterexamContext.Exm_Yearly_Category_ExamsDMO.Where(t => t.EME_Id == data.EME_Id).ToList();
                    var ExamMarksDMO_cnt = _masterexamContext.ExamMarksDMO.Where(t => t.EME_Id == data.EME_Id).ToList();
                    var Exm_M_Prom_Subj_Group_ExamsDMO_cnt = _masterexamContext.Exm_M_Prom_Subj_Group_ExamsDMO.Where(t => t.EME_Id == data.EME_Id).ToList();
                    var ExmStudentMarksProcessDMO_cnt = _masterexamContext.ExmStudentMarksProcessDMO.Where(t => t.EME_Id == data.EME_Id).ToList();
                    var ExmStudentMarksProcessSubjectwiseDMO_cnt = _masterexamContext.ExmStudentMarksProcessSubjectwiseDMO.Where(t => t.EME_Id == data.EME_Id).ToList();
                    if (Exm_Yearly_Category_ExamsDMO_cnt.Count == 0 && ExamMarksDMO_cnt.Count == 0 && Exm_M_Prom_Subj_Group_ExamsDMO_cnt.Count == 0 && ExmStudentMarksProcessDMO_cnt.Count == 0 && ExmStudentMarksProcessSubjectwiseDMO_cnt.Count == 0) //Exm_Yrly_Cat_Exams_SubwiseDMO_cnt.Count == 0 &&
                    {
                        result.EME_ActiveFlag = false;
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
                    result.EME_ActiveFlag = true;
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

        // Master Exam Paper Type
        public exammasterDTO BindData_PaperType(exammasterDTO data)
        {
            try
            {
                data.GetExamPTLoadDetails = _masterexamContext.Exm_Master_PaperTypeDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public exammasterDTO Saveddata_PT(exammasterDTO data)
        {
            try
            {
                data.returnval = false;
                if (data.EMPATY_Id > 0)
                {
                    data.retrunMsg = "Update";

                    var checkresult_duplicate = _masterexamContext.Exm_Master_PaperTypeDMO.Where(a => a.EMPATY_PaperTypeName.Equals(data.EMPATY_PaperTypeName)
                    && a.EMPATY_Id != data.EMPATY_Id && a.MI_Id == data.MI_Id).ToList();

                    if (checkresult_duplicate.Count > 0)
                    {
                        data.retrunMsg = "Duplicate";
                    }
                    else
                    {
                        var checkresult = _masterexamContext.Exm_Master_PaperTypeDMO.Single(a => a.EMPATY_Id == data.EMPATY_Id && a.MI_Id == data.MI_Id);
                        checkresult.EMPATY_PaperTypeName = data.EMPATY_PaperTypeName;
                        checkresult.EMPATY_Color = data.EMPATY_Color;
                        checkresult.EMPATY_PaperTypeDescription = data.EMPATY_PaperTypeDescription;
                        checkresult.EMPATY_UpdatedDate = DateTime.Now;
                        checkresult.EMPATY_UpdatedBy = data.UserId;
                        _masterexamContext.Update(checkresult);

                        var i = _masterexamContext.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                    }
                }
                else
                {
                    data.retrunMsg = "Add";

                    var checkresult_duplicate = _masterexamContext.Exm_Master_PaperTypeDMO.Where(a => a.EMPATY_PaperTypeName.Equals(data.EMPATY_PaperTypeName)
                    && a.MI_Id == data.MI_Id).ToList();

                    if (checkresult_duplicate.Count > 0)
                    {
                        data.retrunMsg = "Duplicate";
                    }
                    else
                    {
                        Exm_Master_PaperTypeDMO exm_Master_PaperTypeDMO = new Exm_Master_PaperTypeDMO
                        {
                            MI_Id = data.MI_Id,
                            EMPATY_PaperTypeName = data.EMPATY_PaperTypeName,
                            EMPATY_Color = data.EMPATY_Color,
                            EMPATY_PaperTypeDescription = data.EMPATY_PaperTypeDescription,
                            EMPATY_CreatedDate = DateTime.Now,
                            EMPATY_CreatedBy = data.UserId,
                            EMPATY_UpdatedDate = DateTime.Now,
                            EMPATY_UpdatedBy = data.MI_Id,
                            EMPATY_ActiveFlag = true
                        };

                        _masterexamContext.Add(exm_Master_PaperTypeDMO);
                        var i = _masterexamContext.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public exammasterDTO Editdata_PT(exammasterDTO data)
        {
            try
            {
                data.GetExamPTEditDetails = _masterexamContext.Exm_Master_PaperTypeDMO.Where(a => a.EMPATY_Id == data.EMPATY_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public exammasterDTO DeactivateActivateMasterExam_PT(exammasterDTO data)
        {
            try
            {
                data.already_cnt = false;
                var result = _masterexamContext.Exm_Master_PaperTypeDMO.Single(a => a.EMPATY_Id == data.EMPATY_Id);
                result.EMPATY_UpdatedDate = DateTime.Now;
                result.EMPATY_UpdatedBy = data.UserId;
                if (result.EMPATY_ActiveFlag == true)
                {
                    var checkresult = _masterexamContext.Exm_Student_Examwise_PTDMO.Where(a => a.MI_Id == data.MI_Id && a.EMPATY_Id == data.EMPATY_Id).ToList();

                    if (checkresult.Count > 0)
                    {
                        data.already_cnt = true;                        
                    }
                }
                if(data.already_cnt == false)
                {
                    result.EMPATY_ActiveFlag = result.EMPATY_ActiveFlag == true ? false : true;
                    _masterexamContext.Update(result);
                    var i = _masterexamContext.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                }                

                data.GetExamPTLoadDetails = _masterexamContext.Exm_Master_PaperTypeDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}