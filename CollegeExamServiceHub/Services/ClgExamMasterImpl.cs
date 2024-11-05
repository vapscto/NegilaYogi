using AutoMapper;
using CollegeExamServiceHub.Interfaces;
using DataAccessMsSqlServerProvider.com.vapstech.College.Exam;
using DomainModel.Model.com.vaps.Exam;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeExamServiceHub.Services
{
    public class ClgExamMasterImpl : ClgExamMasterInterface
    {
        private readonly ClgExamContext _masterexamContext;

        public ClgExamMasterImpl(ClgExamContext masterexamContext)
        {
            _masterexamContext = masterexamContext;
        }

        public exammasterDTO Getdetails(exammasterDTO data)//int IVRMM_Id
        {
            exammasterDTO getdata = new exammasterDTO();
            try
            {
                List<exammasterDMO> list = new List<exammasterDMO>();
                list = _masterexamContext.col_exammasterDMO.Where(t => t.MI_Id == data.MI_Id).OrderBy(t => t.EME_ExamOrder).ToList();
                getdata.exammastername = list.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return getdata;
        }

        public exammasterDTO savedetails(exammasterDTO data)
        {
            if (data.EME_Id != 0)
            {
                var res = _masterexamContext.col_exammasterDMO.Where(t => t.MI_Id == data.MI_Id && (t.EME_ExamName == data.EME_ExamName || t.EME_ExamCode == data.EME_ExamCode) && t.EME_Id != data.EME_Id).ToList();
                if (res.Count > 0)
                {
                    data.returnduplicatestatus = "Duplicate";
                }
                else
                {
                    var result = _masterexamContext.col_exammasterDMO.Single(t => t.MI_Id == data.MI_Id && t.EME_Id == data.EME_Id);
                    result.EME_ExamCode = data.EME_ExamCode;
                    result.EME_ExamName = data.EME_ExamName;
                    result.EME_IVRSExamName = data.EME_IVRSExamName;
                    result.EME_ExamDescription = data.EME_ExamDescription;
                    result.EME_FinalExamFlag = data.EME_FinalExamFlag;
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

                var res = _masterexamContext.col_exammasterDMO.Where(t => t.MI_Id == data.MI_Id && (t.EME_ExamName == data.EME_ExamName || t.EME_ExamCode == data.EME_ExamCode)).ToList();
                if (res.Count > 0)
                {
                    data.returnduplicatestatus = "Duplicate";
                }
                else
                {
                    var row_cnt = _masterexamContext.col_exammasterDMO.Where(t => t.MI_Id == data.MI_Id).ToList().Count;
                    exammasterDMO exm = new exammasterDMO();
                    exm.EME_ExamCode = data.EME_ExamCode;
                    exm.EME_ExamName = data.EME_ExamName;
                    exm.EME_IVRSExamName = data.EME_IVRSExamName;
                    exm.EME_ExamDescription = data.EME_ExamDescription;
                    exm.EME_ExamOrder = row_cnt + 1;
                    exm.EME_FinalExamFlag = data.EME_FinalExamFlag;
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
                            var result = _masterexamContext.col_exammasterDMO.Single(t => t.EME_Id.Equals(mob.EME_Id));
                            // result.UpdatedDate = DateTime.Now;
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

        public exammasterDTO editdetails(int ID)
        {
            exammasterDTO editlt = new exammasterDTO();
            try
            {
                List<exammasterDMO> list = new List<exammasterDMO>();
                list = _masterexamContext.col_exammasterDMO.Where(t => t.EME_Id == ID).ToList();
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
            //exammasterDTO deact = new exammasterDTO();
            exammasterDMO master = Mapper.Map<exammasterDMO>(data);
            if (master.EME_Id > 0)
            {
                var result = _masterexamContext.col_exammasterDMO.Single(t => t.EME_Id == master.EME_Id);
                if (result.EME_ActiveFlag == true)
                {
                    var Exm_Yearly_Category_ExamsDMO_cnt = _masterexamContext.Exm_Col_Yearly_Scheme_ExamsDMO.Where(t => t.EME_Id == data.EME_Id).ToList();
                    var ExamMarksDMO_cnt = _masterexamContext.ExamMarksDMO.Where(t => t.EME_Id == data.EME_Id).ToList();
                    var Exm_M_Prom_Subj_Group_ExamsDMO_cnt = _masterexamContext.Exm_M_Prom_Subj_Group_ExamsDMO.Where(t => t.EME_Id == data.EME_Id).ToList();
                    var ExmStudentMarksProcessDMO_cnt = _masterexamContext.ExmStudentMarksProcessDMO.Where(t => t.EME_Id == data.EME_Id).ToList();
                    var ExmStudentMarksProcessSubjectwiseDMO_cnt = _masterexamContext.ExmStudentMarksProcessSubjectwiseDMO.Where(t => t.EME_Id == data.EME_Id).ToList();
                    if (Exm_Yearly_Category_ExamsDMO_cnt.Count == 0 && ExamMarksDMO_cnt.Count == 0 && Exm_M_Prom_Subj_Group_ExamsDMO_cnt.Count == 0 && ExmStudentMarksProcessDMO_cnt.Count == 0 && ExmStudentMarksProcessSubjectwiseDMO_cnt.Count == 0)
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
    }
}
