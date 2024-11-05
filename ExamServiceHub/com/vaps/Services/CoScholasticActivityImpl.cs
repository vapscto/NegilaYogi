
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
    public class CoScholasticActivityImpl : Interfaces.CoScholasticActivityInterface
    {
        private static ConcurrentDictionary<string, mastersubsubjectDTO> _login =
         new ConcurrentDictionary<string, mastersubsubjectDTO>();

        private readonly ExamContext _masterexamContext;

        public CoScholasticActivityImpl(ExamContext masterexamContext)
        {
            _masterexamContext = masterexamContext;
        }
        public CoScholasticActivityDTO Getdetails(CoScholasticActivityDTO data)//int IVRMM_Id
        {
            try
            {

                List<Exm_CCE_ActivitiesDMO> list = new List<Exm_CCE_ActivitiesDMO>();
                list = _masterexamContext.Exm_CCE_ActivitiesDMO.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.gridlist = list.ToArray();

                List<EXM_CCE_Activities_AREADMO> list1 = new List<EXM_CCE_Activities_AREADMO>();
                list1 = _masterexamContext.EXM_CCE_Activities_AREADMO.Where(t => t.MI_Id == data.MI_Id).OrderBy(a=>a.ECACTA_SkillOrder).ToList();
                data.gridlist1 = list1.ToArray();

                data.getactiviteslist = _masterexamContext.Exm_CCE_ActivitiesDMO.Where(a => a.MI_Id == data.MI_Id && a.ECACT_ActiveFlag == true).ToArray();

                data.getactivitesarealist = _masterexamContext.EXM_CCE_Activities_AREADMO.Where(a => a.MI_Id == data.MI_Id && a.ECACTA_ActiveFlag == true).OrderBy(a => a.ECACTA_SkillOrder).ToArray();

                data.getyear = _masterexamContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.getactivitesareamappinglist = (from a in _masterexamContext.Exm_CCE_Activities_AREA_MappingDMO
                                                    from b in _masterexamContext.Exm_CCE_ActivitiesDMO
                                                    from c in _masterexamContext.EXM_CCE_Activities_AREADMO
                                                    from d in _masterexamContext.Exm_Master_GradeDMO
                                                    from e in _masterexamContext.AcademicYear
                                                    where (a.ECACT_Id == b.ECACT_Id && a.ECACTA_Id == c.ECACTA_Id && a.EMGR_Id == d.EMGR_Id
                                                    && e.ASMAY_Id == a.ASMAY_Id && a.MI_Id == data.MI_Id)
                                                    select new CoScholasticActivityDTO
                                                    {
                                                        ECACT_SkillName = b.ECACT_SkillName,
                                                        ECACTA_SkillArea = c.ECACTA_SkillArea,
                                                        EMGR_GradeName = d.EMGR_GradeName,
                                                        ECACTAM_Id = a.ECACTAM_Id,
                                                        EMGR_Id = a.EMGR_Id,
                                                        ECACTAM_ActiveFlag = a.ECACTAM_ActiveFlag,
                                                        ECACTAM_IndicatorDescription = a.ECACTAM_IndicatorDescription,
                                                        ASMAY_Year = e.ASMAY_Year,
                                                        ASMAY_Order = e.ASMAY_Order
                                                    }).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.fillMastergrade = _masterexamContext.Exm_Master_GradeDMO.Where(a => a.MI_Id == data.MI_Id && a.EMGR_ActiveFlag == true).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;

        }
        public CoScholasticActivityDTO savedetail(CoScholasticActivityDTO data)
        {
            CoScholasticActivityDTO savedata = new CoScholasticActivityDTO();
            try
            {

                if (data.ECACT_Id != 0)
                {
                    var res = _masterexamContext.Exm_CCE_ActivitiesDMO.Where(t => t.MI_Id == data.MI_Id && t.ECACT_SkillName == data.ECACT_SkillName && t.ECACT_Id != data.ECACT_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _masterexamContext.Exm_CCE_ActivitiesDMO.Single(t => t.ECACT_Id == data.ECACT_Id);
                        result.MI_Id = data.MI_Id;
                        result.ECACT_SkillName = data.ECACT_SkillName;
                        result.ECACT_SkillCode = data.ECACT_SkillCode;
                        result.ECACT_ActiveFlag = true;
                        result.CreatedDate = DateTime.Now;
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
                    var res = _masterexamContext.Exm_CCE_ActivitiesDMO.Where(t => t.MI_Id==data.MI_Id &&  (t.ECACT_SkillName == data.ECACT_SkillName)).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var row_cnt = _masterexamContext.Exm_CCE_ActivitiesDMO.Where(t => t.ECACT_Id == data.ECACT_Id).ToList().Count;
                        Exm_CCE_ActivitiesDMO exm = new Exm_CCE_ActivitiesDMO();
                        exm.MI_Id = data.MI_Id;
                        exm.ECACT_SkillName = data.ECACT_SkillName;
                        exm.ECACT_SkillCode = data.ECACT_SkillCode;
                        exm.ECACT_ActiveFlag = true;
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }
        public CoScholasticActivityDTO deactivate(CoScholasticActivityDTO data)
        {
            data.already_cnt = false;
            if (data.ECACT_Id > 0)
            {
                var result = _masterexamContext.Exm_CCE_ActivitiesDMO.Single(t => t.ECACT_Id == data.ECACT_Id && t.MI_Id == data.MI_Id);
                if (result.ECACT_ActiveFlag == true)
                {
                    result.ECACT_ActiveFlag = false;

                }
                else
                {
                    result.ECACT_ActiveFlag = true;

                }
                _masterexamContext.Update(result);
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
        public CoScholasticActivityDTO editdetails(int ID)
        {
            CoScholasticActivityDTO data = new CoScholasticActivityDTO();
            try
            {
                List<Exm_CCE_ActivitiesDMO> list = new List<Exm_CCE_ActivitiesDMO>();
                list = _masterexamContext.Exm_CCE_ActivitiesDMO.Where(t => t.ECACT_Id == ID).ToList();
                data.editlist = list.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
            return data;
        }

        //-----------------------------------Activites Area ----------------------- //
        public CoScholasticActivityDTO savedetail1(CoScholasticActivityDTO data)
        {
            CoScholasticActivityDTO savedata = new CoScholasticActivityDTO();
            try
            {

                if (data.ECACTA_Id != 0)
                {
                    var res = _masterexamContext.EXM_CCE_Activities_AREADMO.Where(t => t.ECACTA_SkillArea == data.ECACTA_SkillArea && t.ECACTA_Id != data.ECACTA_Id && t.MI_Id == data.MI_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _masterexamContext.EXM_CCE_Activities_AREADMO.Single(t => t.ECACTA_Id == data.ECACTA_Id);
                        result.MI_Id = data.MI_Id;
                        result.ECACTA_SkillArea = data.ECACTA_SkillArea;
                        result.ECACTA_SkillOrder = data.ECACTA_SkillOrder;
                        result.ECACTA_ActiveFlag = true;
                        result.CreatedDate = DateTime.Now;
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
                    var res = _masterexamContext.EXM_CCE_Activities_AREADMO.Where(t => t.MI_Id == data.MI_Id && (t.ECACTA_SkillArea == data.ECACTA_SkillArea)).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var row_cnt = _masterexamContext.EXM_CCE_Activities_AREADMO.Where(t => t.ECACTA_Id == data.ECACTA_Id).ToList().Count;
                        EXM_CCE_Activities_AREADMO exm = new EXM_CCE_Activities_AREADMO();
                        exm.MI_Id = data.MI_Id;
                        exm.ECACTA_SkillArea = data.ECACTA_SkillArea;
                        exm.ECACTA_SkillOrder = data.ECACTA_SkillOrder;
                        exm.ECACTA_ActiveFlag = true;
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CoScholasticActivityDTO deactivate1(CoScholasticActivityDTO data)
        {
            data.already_cnt = false;
            if (data.ECACTA_Id > 0)
            {
                var result = _masterexamContext.EXM_CCE_Activities_AREADMO.Single(t => t.ECACTA_Id == data.ECACTA_Id && t.MI_Id == data.MI_Id);
                if (result.ECACTA_ActiveFlag == true)
                {
                    result.ECACTA_ActiveFlag = false;

                }
                else
                {
                    result.ECACTA_ActiveFlag = true;

                }
                _masterexamContext.Update(result);
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
        public CoScholasticActivityDTO validateordernumber(CoScholasticActivityDTO data)
        {
            try
            {
                int id = 0;
                for (int i = 0; i < data.temp_activiteSkillArea.Length; i++)
                {                   
                    var result = _masterexamContext.EXM_CCE_Activities_AREADMO.Single(a => a.MI_Id == data.MI_Id 
                    && a.ECACTA_Id == data.temp_activiteSkillArea[i].ECACTA_Id);

                    id = id + 1;
                    result.ECACTA_SkillOrder = id;
                    result.UpdatedDate = DateTime.Now;
                    _masterexamContext.Update(result);
                }
                if (data.temp_activiteSkillArea.Length > 0)
                {
                    var ik = _masterexamContext.SaveChanges();
                    if (ik > 0)
                    {
                        data.message = "Record Updated Successfully";
                        data.returnval = true;
                    }
                    else
                    {
                        data.message = "Failed To Update Order";
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                data.message = "Failed To Update Order";             
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CoScholasticActivityDTO editdetails1(int ID)
        {
            CoScholasticActivityDTO data = new CoScholasticActivityDTO();
            try
            {
                List<EXM_CCE_Activities_AREADMO> list = new List<EXM_CCE_Activities_AREADMO>();
                list = _masterexamContext.EXM_CCE_Activities_AREADMO.Where(t => t.ECACTA_Id == ID).ToList();
                data.editlist1 = list.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
            return data;
        }

        //----- Activites Area Mapping
        public CoScholasticActivityDTO savedetail2(CoScholasticActivityDTO data)
        {
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            CoScholasticActivityDTO savedata = new CoScholasticActivityDTO();
            try
            {
                if (data.ECACTAM_Id > 0)
                {
                    var res = _masterexamContext.Exm_CCE_Activities_AREA_MappingDMO.Where(t => t.ECACT_Id == data.ECACT_Id && t.ECACTA_Id == data.ECACTA_Id
                    && t.ECACTAM_Id != data.ECACTAM_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _masterexamContext.Exm_CCE_Activities_AREA_MappingDMO.Single(t => t.ECACTAM_Id == data.ECACTAM_Id);
                        result.ECACT_Id = data.ECACT_Id;
                        result.ECACTA_Id = data.ECACTA_Id;
                        result.ECACTAM_IndicatorDescription = data.ECACTAM_IndicatorDescription;
                        result.EMGR_Id = data.EMGR_Id;
                        DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                        result.UpdatedDate = indianTime;
                        _masterexamContext.Update(result);
                        var contactExists = _masterexamContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            data.returnduplicatestatus = "Update";
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnduplicatestatus = "Update";
                            data.returnval = false;
                        }
                    }
                }
                else
                {
                    var res = _masterexamContext.Exm_CCE_Activities_AREA_MappingDMO.Where(t => t.ECACT_Id == data.ECACT_Id && t.ECACTA_Id == data.ECACTA_Id
                    && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        Exm_CCE_Activities_AREA_MappingDMO skl = new Exm_CCE_Activities_AREA_MappingDMO();
                        skl.ECACT_Id = data.ECACT_Id;
                        skl.ECACTA_Id = data.ECACTA_Id;
                        skl.ASMAY_Id = data.ASMAY_Id;
                        skl.ECACTAM_IndicatorDescription = data.ECACTAM_IndicatorDescription;
                        skl.MI_Id = data.MI_Id;
                        skl.EMGR_Id = data.EMGR_Id;
                        DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                        skl.CreatedDate = indianTime;
                        skl.UpdatedDate = indianTime;
                        skl.ECACTAM_ActiveFlag = true;
                        _masterexamContext.Add(skl);
                        var contactExists = _masterexamContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            data.returnduplicatestatus = "Add";
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnduplicatestatus = "Add";
                            data.returnval = false;
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
        public CoScholasticActivityDTO deactivate2(CoScholasticActivityDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                data.already_cnt = false;
                if (data.ECACTA_Id > 0)
                {
                    var result = _masterexamContext.Exm_CCE_Activities_AREA_MappingDMO.Single(t => t.ECACTAM_Id == data.ECACTAM_Id && t.MI_Id == data.MI_Id);
                    if (result.ECACTAM_ActiveFlag == true)
                    {
                        result.ECACTAM_ActiveFlag = false;
                    }
                    else
                    {
                        result.ECACTAM_ActiveFlag = true;
                    }
                    DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                    result.UpdatedDate = indianTime;
                    _masterexamContext.Update(result);
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
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CoScholasticActivityDTO editdetails2(int ID)
        {
            CoScholasticActivityDTO data = new CoScholasticActivityDTO();
            try
            {
                List<Exm_CCE_Activities_AREA_MappingDMO> list = new List<Exm_CCE_Activities_AREA_MappingDMO>();
                list = _masterexamContext.Exm_CCE_Activities_AREA_MappingDMO.Where(t => t.ECACTAM_Id == ID).ToList();
                data.editlist1 = list.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
            return data;
        }
        public CoScholasticActivityDTO get_exam(CoScholasticActivityDTO data)
        {
            try
            {
                //data.editlist = (from a in _masterexamContext.masterexam
                //                 from b in _masterexamContext.Exm_Yearly_CategoryDMO
                //                 from c in _masterexamContext.Exm_Yearly_Category_ExamsDMO
                //                 where (a.EME_Id == c.EME_Id && b.MI_Id == a.MI_Id && b.EYC_Id == data.EYC_Id && b.ASMAY_Id == data.ASMAY_Id)
                //                 select new CoScholasticActivityDTO
                //                 {
                //                     EME_Id = Convert.ToInt32(a.EME_Id),
                //                     EME_ExamName = a.EME_ExamName

                //                 }).Distinct().ToArray();
                //data.examnamelist = (from a in _masterexamContext.masterexam
                //                     from b in _masterexamContext.Exm_Yearly_CategoryDMO
                //                     from c in _masterexamContext.Exm_Yearly_Category_ExamsDMO
                //                     where (a.EME_Id == c.EME_Id && b.MI_Id == a.MI_Id && b.EYC_Id == data.EYC_Id && b.ASMAY_Id == data.ASMAY_Id)
                //                     select new CoScholasticActivityDTO
                //                     {
                //                         EME_Id = Convert.ToInt32(a.EME_Id),
                //                         EME_ExamName = a.EME_ExamName

                //                     }).Distinct().ToArray();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }
        public CoScholasticActivityDTO getexampopup(int ID)
        {
            CoScholasticActivityDTO data = new CoScholasticActivityDTO();
            try
            {
                //data.exampopup = (from a in _masterexamContext.Exm_CCE_TERMS_MP_EXAMSDMO
                //                  from b in _masterexamContext.CCE_Exam_Term_MappingDMO
                //                  from c in _masterexamContext.masterexam
                //                  where (b.MI_Id == c.MI_Id && a.EME_ID == c.EME_Id && a.ECTMP_Id == b.ECTMP_Id && b.ECTMP_Id == ID)
                //                  select new CoScholasticActivityDTO
                //                  {
                //                      ECTMPE_Id = a.ECTMPE_Id,
                //                      EME_ID = a.EME_ID,
                //                      EME_ExamName = c.EME_ExamName,
                //                      ECTMPE_ActiveFlag = a.ECTMPE_ActiveFlag

                //                  }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
            return data;
        }
    }
}
