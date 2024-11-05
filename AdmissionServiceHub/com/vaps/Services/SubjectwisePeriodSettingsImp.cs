using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using DomainModel.Model.com.vaps.admission;
using System;
using DomainModel.Model.com.vaps.Exam;
using DomainModel.Model.com.vapstech.TT;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class SubjectwisePeriodSettingsImp : Interfaces.SubjectwisePeriodSettingsInterface
    {
        private static ConcurrentDictionary<string, SubjectwisePeriodSettingsDTO> _login =
      new ConcurrentDictionary<string, SubjectwisePeriodSettingsDTO>();

        private readonly SubjectwisePeriodSettingsContext _SubjectwisePeriodSettingsContext;


        public SubjectwisePeriodSettingsImp(SubjectwisePeriodSettingsContext SubjectwisePeriodSettingsContext)
        {
            _SubjectwisePeriodSettingsContext = SubjectwisePeriodSettingsContext;
        }

        public SubjectwisePeriodSettingsDTO GetData(SubjectwisePeriodSettingsDTO SubjectwisePeriodSettingsDTO)//int IVRMM_Id
        {
            List<MasterAcademic> allyear = new List<MasterAcademic>();
            allyear = _SubjectwisePeriodSettingsContext.year.Where(d=>d.MI_Id==SubjectwisePeriodSettingsDTO.MI_Id && d.Is_Active==true).OrderByDescending(d=>d.ASMAY_Order).ToList();
            SubjectwisePeriodSettingsDTO.yeardropDown = allyear.ToArray();

            List<MasterAcademic> currentYear = new List<MasterAcademic>();
            currentYear = _SubjectwisePeriodSettingsContext.year.Where(d => d.MI_Id == SubjectwisePeriodSettingsDTO.MI_Id && d.Is_Active == true && d.ASMAY_Id== SubjectwisePeriodSettingsDTO.ASMAY_Id).ToList();
            SubjectwisePeriodSettingsDTO.currentAcdYear = currentYear.ToArray();


            List<School_M_Class> allclass = new List<School_M_Class>();
            allclass = _SubjectwisePeriodSettingsContext.AdmClass.Where(d=>d.MI_Id== SubjectwisePeriodSettingsDTO.MI_Id && d.ASMCL_ActiveFlag==true).ToList();
            SubjectwisePeriodSettingsDTO.classdropDown = allclass.OrderBy(c => c.ASMCL_Order).ToArray();

            List<School_M_Section> allSection = new List<School_M_Section>();
            allSection = _SubjectwisePeriodSettingsContext.AdmSection.Where(d=>d.MI_Id== SubjectwisePeriodSettingsDTO.MI_Id && d.ASMC_ActiveFlag==1).ToList();
            SubjectwisePeriodSettingsDTO.sectiondropDown = allSection.OrderBy(s => s.ASMC_Order).ToArray();

            List<IVRM_Master_SubjectsDMO> Allname3 = new List<IVRM_Master_SubjectsDMO>();
            Allname3 = _SubjectwisePeriodSettingsContext.allSubject.Where(d=>d.MI_Id== SubjectwisePeriodSettingsDTO.MI_Id && d.ISMS_ActiveFlag==1 && d.ISMS_AttendanceFlag==true).ToList();

            List<SubjectwisePeriodSettingsDTO> AllInOne = new List<SubjectwisePeriodSettingsDTO>();

            for (int i = 0; i < Allname3.Count; i++)
            {
                SubjectwisePeriodSettingsDTO temp = new SubjectwisePeriodSettingsDTO();

                temp.SubjectName = Allname3[i].ISMS_SubjectName;
                temp.SubjectCode = Allname3[i].ISMS_SubjectCode;
                temp.PAMS_Id = Allname3[i].ISMS_Id;
                AllInOne.Add(temp);
            }
            SubjectwisePeriodSettingsDTO.GridviewList = AllInOne.ToArray();
            if(SubjectwisePeriodSettingsDTO.GridviewList.Length > 0)
            {
                SubjectwisePeriodSettingsDTO.count = SubjectwisePeriodSettingsDTO.GridviewList.Length;
            }
            else
            {
                SubjectwisePeriodSettingsDTO.count = 0;
            }
            return SubjectwisePeriodSettingsDTO;
        }

        public SubjectwisePeriodSettingsDTO SaveData(SubjectwisePeriodSettingsDTO mas)
        {
           
                for (int p = 0; p < mas.SelectedSubjectMaxPeriods.Count; p++)
                {
                    List<SubjectwisePeriodSettingsDMO> Allname1 = new List<SubjectwisePeriodSettingsDMO>();

                    Allname1 = _SubjectwisePeriodSettingsContext.SubjectwisePeriodSettingsDMO.Where(t => t.MI_Id==mas.MI_Id && t.ISMS_Id==mas.SelectedSubjectMaxPeriods[p].PAMS_Id && t.ASMAY_Id==mas.ASMAY_Id && t.ASMS_Id==mas.ASMC_Id && t.ASMCL_Id==mas.ASMCL_Id).ToList();


                    if (Allname1.Count > 0)
                    {
                     
                        var result = _SubjectwisePeriodSettingsContext.SubjectwisePeriodSettingsDMO.Single(t => t.MI_Id==mas.MI_Id && t.ISMS_Id==mas.SelectedSubjectMaxPeriods[p].PAMS_Id && t.ASMAY_Id==mas.ASMAY_Id && t.ASMS_Id==mas.ASMC_Id && t.ASMCL_Id==mas.ASMCL_Id);
                        result.MI_Id = mas.MI_Id;
                        result.ASMAY_Id = mas.ASMAY_Id;
                        result.ASMCL_Id = mas.ASMCL_Id;
                        result.ASMS_Id = mas.ASMC_Id;
                        result.ISMS_Id = mas.SelectedSubjectMaxPeriods[p].PAMS_Id;
                        result.ASASMP_MaxPeriod = mas.SelectedSubjectMaxPeriods[p].ASASMP_MaxPeriod;
                        result.CreatedDate = result.CreatedDate; 
                        result.UpdatedDate = DateTime.Now;              

                        _SubjectwisePeriodSettingsContext.Update(result);
                        int flag =_SubjectwisePeriodSettingsContext.SaveChanges();
                        if(flag > 0)
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
                        SubjectwisePeriodSettingsDMO MM2 = new SubjectwisePeriodSettingsDMO();

                        MM2.MI_Id = mas.MI_Id;
                        MM2.ASMAY_Id = mas.ASMAY_Id;
                        MM2.ASMCL_Id = mas.ASMCL_Id;
                        MM2.ASMS_Id = mas.ASMC_Id;
                        MM2.ISMS_Id = mas.SelectedSubjectMaxPeriods[p].PAMS_Id;
                        MM2.ASASMP_MaxPeriod = mas.SelectedSubjectMaxPeriods[p].ASASMP_MaxPeriod;
                        MM2.CreatedDate = DateTime.Now;          
                        MM2.UpdatedDate = DateTime.Now;           
                        _SubjectwisePeriodSettingsContext.Add(MM2);
                       int flag= _SubjectwisePeriodSettingsContext.SaveChanges();
                        if(flag > 0)
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
       public SubjectwisePeriodSettingsDTO subjectMaxPeriod(SubjectwisePeriodSettingsDTO dto)
        {
            if(dto.ASMAY_Id > 0 && dto.ASMCL_Id > 0 && dto.ASMC_Id > 0)
            {
                dto.subjectwisePeriodCount = _SubjectwisePeriodSettingsContext.SubjectwisePeriodSettingsDMO.Where(d => d.ASMAY_Id == dto.ASMAY_Id && d.ASMCL_Id == dto.ASMCL_Id && d.ASMS_Id == dto.ASMC_Id).ToArray();
            }
            
            return dto;
        }


    }
}
