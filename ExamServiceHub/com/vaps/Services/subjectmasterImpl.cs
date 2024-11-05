
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
using DomainModel.Model.com.vapstech.TT;

namespace ExamServiceHub.com.vaps.Services
{
    public class subjectmasterImpl : Interfaces.subjectmasterInterface
    {
         private static ConcurrentDictionary<string, subjectmasterDTO> _login =
         new ConcurrentDictionary<string, subjectmasterDTO>();

        private readonly subjectmasterContext _subjectmasterContext;
        public subjectmasterImpl(subjectmasterContext subjectmasterContext)
        {
            _subjectmasterContext = subjectmasterContext;
        }

        public subjectmasterDTO GetsubjectmasterData(subjectmasterDTO subjectmasterDTO)//int IVRMM_Id
        {
            try
            {
                List<IVRM_School_Master_SubjectsDMO> Allname = new List<IVRM_School_Master_SubjectsDMO>();
                Allname = _subjectmasterContext.subjectmasterDMO.Where(d => d.MI_Id == subjectmasterDTO.MI_Id).OrderByDescending(t => t.UpdatedDate).ToList();
                subjectmasterDTO.subjectmastername = Allname.ToArray();
                if (Allname.Count > 0)
                {
                    subjectmasterDTO.count = Allname.Count;
                }
                else
                {
                    subjectmasterDTO.count = 0;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
          
            return subjectmasterDTO;
        }

        public subjectmasterDTO GetSelectedRowDetails(int ID)
        {
            subjectmasterDTO subjectmasterDTO = new subjectmasterDTO();
            List<IVRM_School_Master_SubjectsDMO> lorg = new List<IVRM_School_Master_SubjectsDMO>();
            lorg = _subjectmasterContext.subjectmasterDMO.Where(t => t.ISMS_Id==ID).ToList();
            subjectmasterDTO.subjectmastername = lorg.ToArray();
            return subjectmasterDTO;
        }

        public subjectmasterDTO MasterDeleteModulesData(int ID)
        {
            subjectmasterDTO data = new subjectmasterDTO();
            try
            {
                var result = _subjectmasterContext.subjectmasterDMO.Single(t => t.ISMS_Id == ID);


                if (result.ISMS_ActiveFlag == 1)
                {
                    result.ISMS_ActiveFlag = 0;
                }
                else if (result.ISMS_ActiveFlag == 1)
                {
                    result.ISMS_ActiveFlag = 0;
                }

                result.UpdatedDate = DateTime.Now;
                _subjectmasterContext.Update(result);
                var flag = _subjectmasterContext.SaveChanges();

                if (result.ISMS_ActiveFlag == 1)
                {
                    data.msg = "Record Enabled Successfully";
                }
                else if (result.ISMS_ActiveFlag == 0)
                {
                    data.msg = "Record Disabled Successfully";
                }
            }

            catch(Exception e)
            {
                data.msg = "Operation Failed";
                Console.WriteLine(e.Message);
            }

            return data;
        }

        public subjectmasterDTO subjectmasterData(subjectmasterDTO mas)
        {
            IVRM_School_Master_SubjectsDMO MM = Mapper.Map<IVRM_School_Master_SubjectsDMO>(mas);
            var subjects = _subjectmasterContext.subjectmasterDMO.Where(d => d.MI_Id == mas.MI_Id).ToList();
           
            if (mas.ISMS_Id != 0)
            {
                
                var result = _subjectmasterContext.subjectmasterDMO.Single(t => t.ISMS_Id == mas.ISMS_Id);
                var duplicateresult = _subjectmasterContext.subjectmasterDMO.Where(t => t.ISMS_SubjectName.Equals(mas.ISMS_SubjectName) && t.MI_Id==mas.MI_Id);		                //result.IVRMM_Id = mas.IVRMM_Id;
              
                if (duplicateresult.Count() > 0)
                {
                    if(result.ISMS_SubjectCode!=mas.ISMS_SubjectCode || result.ISMS_OrderFlag!=mas.ISMS_OrderFlag || result.ISMS_BatchAppl!=mas.ISMS_BatchAppl || result.ISMS_PreadmFlag!=mas.ISMS_PreadmFlag)
                    {
                        if(result.ISMS_OrderFlag!=mas.ISMS_OrderFlag)
                        {
                            if (subjects.Count > 0)
                            {
                                for (int i = 0; i < subjects.Count; i++)
                                {
                                    if (subjects[i].ISMS_OrderFlag == mas.ISMS_OrderFlag)
                                    {
                                        mas.msg = "orderduplicate";
                                        return mas;
                                    }
                                }
                            }
                        }
                        
                        //result.IVRMM_Id = mas.IVRMM_Id;
                        result.ISMS_SubjectName = mas.ISMS_SubjectName;
                        result.ISMS_SubjectCode = mas.ISMS_SubjectCode;
                        result.ISMS_BatchAppl = mas.ISMS_BatchAppl;
                        //  result.ISMS_ExamFlag = mas.ISMS_ExamFlag;
                        result.ISMS_OrderFlag = mas.ISMS_OrderFlag;
                        result.ISMS_PreadmFlag = mas.ISMS_PreadmFlag;
                        result.ISMS_SubjectCode = mas.ISMS_SubjectCode;
                        //   result.ISMS_SubjectFlag = mas.ISMS_SubjectFlag;
                        result.UpdatedDate = DateTime.Now;
                        _subjectmasterContext.Update(result);
                        var flag = _subjectmasterContext.SaveChanges();
                        if (flag > 0)
                        {
                            mas.returnval = true;
                        }
                        else
                        {
                            mas.returnval = false;
                        }
                    }
                    else
                    {
                        mas.msg = "Duplicate Record Exist";
                    }
                }
                else
                {
                    if(result.ISMS_OrderFlag != mas.ISMS_OrderFlag)
                    {
                        if (subjects.Count > 0)
                        {
                            for (int i = 0; i < subjects.Count; i++)
                            {
                                if (subjects[i].ISMS_OrderFlag == mas.ISMS_OrderFlag)
                                {
                                    mas.msg = "orderduplicate";
                                    return mas;
                                }
                            }
                        }
                    }
                    //result.IVRMM_Id = mas.IVRMM_Id;
                    result.ISMS_SubjectName = mas.ISMS_SubjectName;
                    result.ISMS_SubjectCode = mas.ISMS_SubjectCode;
                    result.ISMS_BatchAppl = mas.ISMS_BatchAppl;
                  //  result.ISMS_ExamFlag = mas.ISMS_ExamFlag;
                    result.ISMS_OrderFlag = mas.ISMS_OrderFlag;
                    result.ISMS_PreadmFlag = mas.ISMS_PreadmFlag;
                    result.ISMS_SubjectCode = mas.ISMS_SubjectCode;
                 //   result.ISMS_SubjectFlag = mas.ISMS_SubjectFlag;
                    result.UpdatedDate = DateTime.Now;
                    _subjectmasterContext.Update(result);
                    var flag = _subjectmasterContext.SaveChanges();
                    if (flag > 0)
                    {
                        mas.returnval = true;
                    }
                    else
                    {
                        mas.returnval = false;
                    }
                }
            }
            else
            {
                if (subjects.Count > 0)
                {
                    for (int i = 0; i < subjects.Count; i++)
                    {
                        if (subjects[i].ISMS_OrderFlag == mas.ISMS_OrderFlag)
                        {
                            mas.msg = "orderduplicate";
                            return mas;
                        }
                    }
                }

                var duplicateresult = _subjectmasterContext.subjectmasterDMO.Where(t => t.ISMS_SubjectName.Equals(mas.ISMS_SubjectName) && t.MI_Id==mas.MI_Id);

                if (duplicateresult.Count() > 0)
                {
                    mas.msg = "Duplicate Record Exist";
                }
                else
                {
                    IVRM_School_Master_SubjectsDMO MM3 = new IVRM_School_Master_SubjectsDMO();
                    MM3.ISMS_SubjectName = mas.ISMS_SubjectName;
                    MM3.ISMS_SubjectCode = mas.ISMS_SubjectCode;
                    MM3.ISMS_ActiveFlag =1;
                    MM3.CreatedDate = DateTime.Now;
                    MM3.ISMS_BatchAppl = mas.ISMS_BatchAppl;
                   // MM3.ISMS_ExamFlag = mas.ISMS_ExamFlag;
                    MM3.ISMS_OrderFlag = mas.ISMS_OrderFlag;
                    MM3.ISMS_PreadmFlag = mas.ISMS_PreadmFlag;
                   // MM3.ISMS_SubjectFlag = mas.ISMS_SubjectFlag;
                    MM3.MI_Id = mas.MI_Id;
                    MM3.UpdatedDate = DateTime.Now;
                    _subjectmasterContext.Add(MM3);
                    var flag = _subjectmasterContext.SaveChanges();
                    if (flag == 1)
                    {
                        mas.returnval = true;
                    }
                    else
                    {
                        mas.returnval = false;
                    }
                }
            }        
            return mas;
        }

    }
}
