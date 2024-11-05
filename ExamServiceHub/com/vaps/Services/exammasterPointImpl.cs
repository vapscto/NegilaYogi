
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
    public class exammasterPointImpl : Interfaces.exammasterPointInterface
    {
        private static ConcurrentDictionary<string, exammasterpointDTO> _login =
         new ConcurrentDictionary<string, exammasterpointDTO>();

        private readonly ExamContext _examContext;

        public exammasterPointImpl(ExamContext examContext)
        {
            _examContext = examContext;
        }

        public exammasterpointDTO Getdetails(exammasterpointDTO data)//int IVRMM_Id
        {
            exammasterpointDTO getdata = new exammasterpointDTO();
            try
            {
                List<exammasterPointDMO> list = new List<exammasterPointDMO>();
                list = _examContext.exammasterPointDMO.Where(t => t.MI_Id == data.MI_Id).OrderBy(t => t.Point_Order).ToList();
                getdata.exammasterpointname = list.ToArray();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return getdata;
            
        }

        //Onchange 
        //Added by Ramesh
        public exammasterpointDTO validateordernumber(exammasterpointDTO dto)
        {
           dto.retrunMsg = "";
            try
            {
                if (dto.exampointDTO.Count() > 0)
                {
                    foreach (exammasterpointDTO mob in dto.exampointDTO)
                    {
                        if (mob.Point_Id > 0)
                        {
                            var result = _examContext.exammasterPointDMO.Single(t => t.Point_Id.Equals(mob.Point_Id));
                            Mapper.Map(mob, result);
                            result.UpdatedDate = DateTime.Now;
                            _examContext.Update(result);
                            _examContext.SaveChanges();
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

        public exammasterpointDTO savedetails(exammasterpointDTO data)
        {

            if (data.Point_Id != 0)
            {
                var res = _examContext.exammasterPointDMO.Where(t => t.MI_Id == data.MI_Id && (t.Point_Name == data.Point_Name) && t.Point_Id != data.Point_Id).ToList();
                if (res.Count > 0)
                {
                    data.returnduplicatestatus = "Duplicate";
                }
                else
                {
                    var result = _examContext.exammasterPointDMO.Single(t => t.MI_Id == data.MI_Id && t.Point_Id == data.Point_Id);
                    result.Point_Name = data.Point_Name;         
                    result.UpdatedDate = DateTime.Now;
                    _examContext.Update(result);
                    var contactExists = _examContext.SaveChanges();
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
                var res = _examContext.exammasterPointDMO.Where(t => t.MI_Id == data.MI_Id && t.Point_Name == data.Point_Name).ToList();
                if (res.Count > 0)
                {
                    data.returnduplicatestatus = "Duplicate";
                }
                else
                {
                    var row_cnt = _examContext.exammasterPointDMO.Where(t => t.MI_Id == data.MI_Id).ToList().Count;
                    exammasterPointDMO exm = new exammasterPointDMO();
                    exm.Point_Name = data.Point_Name;         
                    exm.Point_Order = row_cnt + 1;
                    exm.MI_Id = data.MI_Id;
                    exm.Active_flag = true;

                    exm.CreatedDate = DateTime.Now;
                    exm.UpdatedDate = DateTime.Now;
                    _examContext.Add(exm);
                    var contactExists = _examContext.SaveChanges();
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

        public exammasterpointDTO editdetails(int ID)
        {
            exammasterpointDTO editlt = new exammasterpointDTO();
            try
            {
                List<exammasterPointDMO> list = new List<exammasterPointDMO>();
                list = _examContext.exammasterPointDMO.Where(t => t.Point_Id == ID).ToList();
                editlt.exammpointname = list.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
            return editlt;
        }

        public exammasterpointDTO deactivate(exammasterpointDTO data)
        {
            data.already_cnt = false;
            exammasterPointDMO master = Mapper.Map<exammasterPointDMO>(data);
            if (master.Point_Id > 0)
            {
                var result = _examContext.exammasterPointDMO.Single(t => t.Point_Id == master.Point_Id);
                if (result.Active_flag == true)
                {
                    //var Exm_Yearly_Category_ExamsDMO_cnt = _examContext.Exm_Yearly_Category_ExamsDMO.Where(t => t.EME_Id == data.EME_Id).ToList();
                    //var ExamMarksDMO_cnt = _examContext.ExamMarksDMO.Where(t => t.EME_Id == data.EME_Id).ToList();
                    //var Exm_M_Prom_Subj_Group_ExamsDMO_cnt = _examContext.Exm_M_Prom_Subj_Group_ExamsDMO.Where(t => t.EME_Id == data.EME_Id).ToList();
                    //var ExmStudentMarksProcessDMO_cnt = _examContext.ExmStudentMarksProcessDMO.Where(t => t.EME_Id == data.EME_Id).ToList();
                    //var ExmStudentMarksProcessSubjectwiseDMO_cnt = _examContext.ExmStudentMarksProcessSubjectwiseDMO.Where(t => t.EME_Id == data.EME_Id).ToList();
                    //if (Exm_Yearly_Category_ExamsDMO_cnt.Count == 0 && ExamMarksDMO_cnt.Count == 0 && Exm_M_Prom_Subj_Group_ExamsDMO_cnt.Count == 0 && ExmStudentMarksProcessDMO_cnt.Count == 0 && ExmStudentMarksProcessSubjectwiseDMO_cnt.Count == 0) //Exm_Yrly_Cat_Exams_SubwiseDMO_cnt.Count == 0 &&
                    //{
                    result.Active_flag = false;
                    result.UpdatedDate = DateTime.Now;
                    _examContext.Update(result);
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
                    _examContext.Update(result);
                }

                var flag = _examContext.SaveChanges();
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
