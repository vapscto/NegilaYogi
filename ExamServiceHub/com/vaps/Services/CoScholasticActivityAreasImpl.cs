
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
    public class CoScholasticActivityAreasImpl : Interfaces.CoScholasticActivityAreasInterface
    {
        private static ConcurrentDictionary<string, mastersubsubjectDTO> _login =
         new ConcurrentDictionary<string, mastersubsubjectDTO>();

        private readonly ExamContext _masterexamContext;

        public CoScholasticActivityAreasImpl(ExamContext masterexamContext)
        {
            _masterexamContext = masterexamContext;
        }

        public CoScholasticActivityAreasDTO Getdetails(CoScholasticActivityAreasDTO data)//int IVRMM_Id
        {
            CoScholasticActivityAreasDTO getdata = new CoScholasticActivityAreasDTO();
            try
            {
                List<CCE_M_Scholastic_AreasDMO> list = new List<CCE_M_Scholastic_AreasDMO>();
                list = _masterexamContext.CCE_M_Scholastic_AreasDMO.Where(t => t.MI_Id == data.MI_Id).OrderBy(t=> t.CCE_M_Sch_Area_Order).ToList();
                getdata.exammastername = list.ToArray();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return getdata;
            
        }

        //Onchange 
        //Added by Ramesh
        public CoScholasticActivityAreasDTO validateordernumber(CoScholasticActivityAreasDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.examDTO.Count() > 0)
                {
                    foreach (CoScholasticActivityAreasDTO mob in dto.examDTO)
                    {
                        if (mob.CCE_M_Sch_Area_Id > 0)
                        {
                            var result = _masterexamContext.CCE_M_Scholastic_AreasDMO.Single(t => t.CCE_M_Sch_Area_Id.Equals(mob.CCE_M_Sch_Area_Id));
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

        public CoScholasticActivityAreasDTO savedetails(CoScholasticActivityAreasDTO data)
        {
            //exammasterDTO savedata = new exammasterDTO();
            try
            {

                if (data.CCE_M_Sch_Area_Id != 0)
                {
                    var res = _masterexamContext.CCE_M_Scholastic_AreasDMO.Where(t => t.MI_Id == data.MI_Id && (t.CCE_M_Sch_Area_Name == data.CCE_M_Sch_Area_Name) && t.CCE_M_Sch_Area_Id != data.CCE_M_Sch_Area_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicat";
                    }
                    else
                    {
                        var result = _masterexamContext.CCE_M_Scholastic_AreasDMO.Single(t => t.MI_Id == data.MI_Id && t.CCE_M_Sch_Area_Id == data.CCE_M_Sch_Area_Id);
                        // result.EME_ExamCode = data.EME_ExamCode;
                        result.CCE_M_Sch_Area_Name = data.CCE_M_Sch_Area_Name;
                        result.Active_flag = data.Active_flag;
                        //if (data.EME_FinalExamFlag == true)
                        //{
                        //    result.EME_ActiveFlag = true;
                        //}
                        // result.EME_ExamOrder = result.EME_ExamOrder;
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
                    //  var res = _masterexamContext.masterexam.Where(t => t.MI_Id == data.MI_Id  && t.EME_ExamName == data.EME_ExamName && t.EME_ExamCode == data.EME_ExamCode).ToList();
                    var res = _masterexamContext.CCE_M_Scholastic_AreasDMO.Where(t => t.MI_Id == data.MI_Id && (t.CCE_M_Sch_Area_Name == data.CCE_M_Sch_Area_Name)).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var row_cnt = _masterexamContext.CCE_M_Scholastic_AreasDMO.Where(t => t.MI_Id == data.MI_Id).ToList().Count;
                        CCE_M_Scholastic_AreasDMO exm = new CCE_M_Scholastic_AreasDMO();
                        // exm.EME_ExamCode = data.EME_ExamCode;
                        exm.CCE_M_Sch_Area_Name = data.CCE_M_Sch_Area_Name;
                        exm.CCE_M_Sch_Area_Order = row_cnt + 1;
                        exm.Active_flag = data.Active_flag;
                        exm.MI_Id = data.MI_Id;
                        exm.Active_flag = true;
                        //exm.EME_FinalExamFlag = true;
                        exm.CreatedDate = DateTime.Now;
                        exm.UpdatedDate = DateTime.Now;
                        _masterexamContext.Add(exm);
                        var contactExists = _masterexamContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            // exammasterDMO dmo = Mapper.Map<exammasterDMO>(exm);
                            // dmo.EME_ExamOrder = dmo.EME_Id;
                            // var result1 = _masterexamContext.masterexam.Single(t=>t.EME_Id==dmo.EME_Id);
                            // Mapper.Map(dmo, result1);
                            // _masterexamContext.Update(result1);
                            // _masterexamContext.SaveChanges();
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }

                }
            }

            catch(Exception ex) {
                throw ex;
              
            }

            return data;
        }

        public CoScholasticActivityAreasDTO editdetails(int ID)
        {
            CoScholasticActivityAreasDTO editlt = new CoScholasticActivityAreasDTO();
            try
            {
                List<CCE_M_Scholastic_AreasDMO> list = new List<CCE_M_Scholastic_AreasDMO>();
                list = _masterexamContext.CCE_M_Scholastic_AreasDMO.Where(t => t.CCE_M_Sch_Area_Id == ID).ToList();
                editlt.exammastername = list.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
            return editlt;
        }

        public CoScholasticActivityAreasDTO deactivate(CoScholasticActivityAreasDTO data)
        {
            data.already_cnt = false;
            //exammasterDTO deact = new exammasterDTO();
            CoScholasticActivityAreasDTO master = Mapper.Map<CoScholasticActivityAreasDTO>(data);
            if (master.CCE_M_Sch_Area_Id > 0)
            {
                var result = _masterexamContext.CCE_M_Scholastic_AreasDMO.Single(t => t.CCE_M_Sch_Area_Id == master.CCE_M_Sch_Area_Id);
                if (result.Active_flag == true)
                {
                    //var Exm_Yearly_Category_ExamsDMO_cnt = _masterexamContext.Exm_Yearly_Category_ExamsDMO.Where(t => t.EME_Id == data.EME_Id).ToList();
                    //var ExamMarksDMO_cnt = _masterexamContext.ExamMarksDMO.Where(t => t.EME_Id == data.EME_Id).ToList();
                    //var Exm_M_Prom_Subj_Group_ExamsDMO_cnt = _masterexamContext.Exm_M_Prom_Subj_Group_ExamsDMO.Where(t => t.EME_Id == data.EME_Id).ToList();
                    //var ExmStudentMarksProcessDMO_cnt = _masterexamContext.ExmStudentMarksProcessDMO.Where(t => t.EME_Id == data.EME_Id).ToList();
                    //var ExmStudentMarksProcessSubjectwiseDMO_cnt = _masterexamContext.ExmStudentMarksProcessSubjectwiseDMO.Where(t => t.EME_Id == data.EME_Id).ToList();
                    //if (Exm_Yearly_Category_ExamsDMO_cnt.Count == 0 &&  ExamMarksDMO_cnt.Count == 0 && Exm_M_Prom_Subj_Group_ExamsDMO_cnt.Count==0  && ExmStudentMarksProcessDMO_cnt.Count==0 && ExmStudentMarksProcessSubjectwiseDMO_cnt.Count==0) //Exm_Yrly_Cat_Exams_SubwiseDMO_cnt.Count == 0 &&
                   // {
                        result.Active_flag = false;
                        result.UpdatedDate = DateTime.Now;
                        _masterexamContext.Update(result);
                    }
                    else
                {
                    result.Active_flag = true;
                    result.UpdatedDate = DateTime.Now;
                    _masterexamContext.Update(result);
                    // data.already_cnt = true;
                }

                // result.EME_ActiveFlag = false;

            }
                else
                {
                    //result.Active_flag = true;
                    //result.UpdatedDate = DateTime.Now;
                    //_masterexamContext.Update(result);
                }
              //  result.UpdatedDate = DateTime.Now;
             //   _masterexamContext.Update(result);
                var flag = _masterexamContext.SaveChanges();
                if (flag == 1)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }

          return data;
        }
    }
}
