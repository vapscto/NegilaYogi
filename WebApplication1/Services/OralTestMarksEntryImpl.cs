using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using WebApplication1.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DomainModel.Model;
using System.Collections.Concurrent;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace WebApplication1.Services
{
    public class OralTestMarksEntryImpl : Interfaces.OralTestMarksEntryInterface
    {
        private static ConcurrentDictionary<string, StudentDetailsDTO> _login =
        new ConcurrentDictionary<string, StudentDetailsDTO>();

        private readonly OralTestMarksENtryContext _OralTestMarksENtryContext;

        public OralTestMarksEntryImpl(OralTestMarksENtryContext OralTestMarksENtryContext)
        {
            _OralTestMarksENtryContext = OralTestMarksENtryContext;
        }

        public OralTestMarksBindDataDTO GetOralTestMarksEntryData(OralTestOralByMarksDTO mas)
        {
            OralTestMarksBindDataDTO OralTestMarksBindDataDTO = new OralTestMarksBindDataDTO();

            var Acdemic_preadmission = _OralTestMarksENtryContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == mas.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

            mas.ASMAY_Id = Acdemic_preadmission;
            Array[] showdata3 = new Array[1];
            List<MasterConfiguration> Allname3 = new List<MasterConfiguration>();
            Allname3 = _OralTestMarksENtryContext.MasterConfiguration.Where(t => t.MI_Id.Equals(mas.MI_Id) && t.ASMAY_Id.Equals(mas.ASMAY_Id)).ToList().ToList();

            if (Allname3.Count > 0)
            {
                OralTestMarksBindDataDTO.MasterConfiguration = Allname3.ToArray();
                OralTestMarksBindDataDTO.OralTestScheduleAppFlag = Allname3[0].ISPAC_OralTestApplFlag;


                List<string> OraltestByArray = new List<string>();

                for (int i = 1; i <= Allname3[0].ISPAC_OralTestBy; i++)
                {
                    OraltestByArray.Add(i.ToString());
                }

                OralTestMarksBindDataDTO.OralTestBy = OraltestByArray.ToArray();

                Array[] showdata1 = new Array[1];
                List<StudentDetailsDMO> Allname1 = new List<StudentDetailsDMO>();
                Allname1 = _OralTestMarksENtryContext.StudentDetailsDMO.Where(t => t.MI_Id == mas.MI_Id && t.ASMAY_Id == mas.ASMAY_Id).OrderBy(t => t.PASR_FirstName).ToList();
                OralTestMarksBindDataDTO.studentDetails = Allname1.ToArray();


                Array[] showdata = new Array[50];
                List<OralTestScheduleDMO> Allname = new List<OralTestScheduleDMO>();
                Allname = _OralTestMarksENtryContext.ScheduleNameDMO.Where(t => t.MI_Id == mas.MI_Id && t.ASMAY_Id == mas.ASMAY_Id).OrderBy(t => t.PAOTS_ScheduleName).ToList();
                OralTestMarksBindDataDTO.OralTestSchedule = Allname.ToArray();
                OralTestMarksBindDataDTO.Chq_config = true;
            }
            else
            {
                OralTestMarksBindDataDTO.Chq_config = false;
            }

            return OralTestMarksBindDataDTO;
        }


        public OralTestOralByMarksDTO GetdetailsOnSchedule(OralTestOralByMarksDTO mas)
        {
            // OralTestMarksBindDataDTO OralTestMarksBindDataDTO = new OralTestMarksBindDataDTO();
            var Acdemic_preadmission = _OralTestMarksENtryContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == mas.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

            mas.ASMAY_Id = Acdemic_preadmission;

            if (mas.PAOTS_Id == 0)
            {
                Array[] showdata1 = new Array[1];
                List<StudentDetailsDMO> Allname1 = new List<StudentDetailsDMO>();
                Allname1 = _OralTestMarksENtryContext.StudentDetailsDMO.Where(t=>t.MI_Id == mas.MI_Id && t.ASMAY_Id==mas.ASMAY_Id).OrderBy(t => t.PASR_FirstName).ToList();
                mas.studentDetails = Allname1.ToArray();
            }
            else
            {
             
                var scheduleIds = _OralTestMarksENtryContext.ScheduleNameDMO.Where(t=> t.MI_Id.Equals(mas.MI_Id) && t.ASMAY_Id.Equals(mas.ASMAY_Id) && t.PAOTS_Id.Equals(mas.PAOTS_Id)).Select(m=>m.PAOTS_Id).ToArray();

                var StdIds = _OralTestMarksENtryContext.OralTestScheduleStudentInsertDMO.Where(t => scheduleIds.Contains(t.PAOTS_Id)).Select(t => t.PASR_Id).ToArray().ToArray();

                Array[] showdata1 = new Array[1];
                List<StudentDetailsDMO> Allname1 = new List<StudentDetailsDMO>();
                Allname1 = _OralTestMarksENtryContext.StudentDetailsDMO.Where(t => StdIds.Contains(t.PASR_Id)).OrderBy(t => t.PASR_FirstName).ToList();
                mas.studentDetails = Allname1.ToArray();
            }

            return mas;
        }

        public OralTestOralByMarksDTO OralTestMarksEntryData(OralTestOralByMarksDTO mas)
        {
            try
            {
                decimal obtTotal = 0;
                string passfail = "Pass";
                long PASR_Id_new = 0;

                var Acdemic_preadmission = _OralTestMarksENtryContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == mas.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

                mas.ASMAY_Id = Acdemic_preadmission;

                int OrderByPerson = 1;

                for (int i = 0; i < mas.SelectedStudentMarksData.Count; i++)
                {

                    if (PASR_Id_new == 0 || mas.SelectedStudentMarksData[i].PASR_Id != PASR_Id_new)
                    {
                        PASR_Id_new = mas.SelectedStudentMarksData[i].PASR_Id;
                        OrderByPerson = 0;
                    }

                    if (mas.SelectedStudentMarksData[i].PASR_Id == PASR_Id_new)
                    {
                        OrderByPerson = OrderByPerson + 1;
                    }

                    OralTestOralByMarksDMO MM = Mapper.Map<OralTestOralByMarksDMO>(mas);


                    List<OralTestOralByMarksDMO> Allname2 = new List<OralTestOralByMarksDMO>();

                    Allname2 = _OralTestMarksENtryContext.OralTestOralByMarksDMO.Where(t => t.PAOTM_OralBy.Equals(OrderByPerson) && t.MI_Id.Equals(mas.MI_Id) && t.ASMAY_Id.Equals(mas.ASMAY_Id)).ToList().ToList();

                    if (Allname2.Count > 0)
                    {
                        mas.PAOTM_Id = Allname2[0].PAOTM_Id;
                    }
                    else
                    {
                        MM.PAOTM_Id = 0;

                        MM.PAOTM_EntryDate = System.DateTime.Today;
                        MM.PAOTM_EntryDate = Convert.ToDateTime(MM.PAOTM_EntryDate);
                        MM.MI_Id = mas.MI_Id;
                        MM.ASMAY_Id = mas.ASMAY_Id;
                        MM.PAOTM_OralBy = OrderByPerson;
                        //added by 02/02/2017
                        MM.CreatedDate = DateTime.Now;
                        MM.UpdatedDate = DateTime.Now;
                        _OralTestMarksENtryContext.Add(MM);
                        _OralTestMarksENtryContext.SaveChanges();

                        mas.PAOTM_Id = MM.PAOTM_Id;

                        if (mas.OralTestScheduleAppFlag == "true")
                        {
                            List<OralTestScheduleMarksMapDMO> Allname22 = new List<OralTestScheduleMarksMapDMO>();

                            Allname22 = _OralTestMarksENtryContext.OralTestScheduleMarksMapDMO.Where(t => t.PAOTM_Id.Equals(mas.PAOTM_Id) && t.PAOTS_Id.Equals(mas.PAOTS_Id)).ToList().ToList();

                            if (Allname22.Count > 0)
                            {

                            }
                            else
                            {
                                OralTestScheduleMarksMapDMO MM5 = new OralTestScheduleMarksMapDMO();

                                MM5.PAOTM_Id = mas.PAOTM_Id;
                                MM5.PAOTS_Id = mas.PAOTS_Id;
                                //added by 02/02/2017
                                MM5.CreatedDate = DateTime.Now;
                                MM5.UpdatedDate = DateTime.Now;
                                _OralTestMarksENtryContext.Add(MM5);
                                _OralTestMarksENtryContext.SaveChanges();

                            }
                        }
                    }

                    List<OralTestStudentWiseMarksDMO> Allname1 = new List<OralTestStudentWiseMarksDMO>();

                    Allname1 = _OralTestMarksENtryContext.OralTestStudentWiseMarksDMO.Where(t => t.PAOTM_Id.Equals(mas.PAOTM_Id) && t.PASR_Id.Equals(mas.SelectedStudentMarksData[i].PASR_Id)).ToList().ToList();

                    List<MasterConfiguration> Allname24 = new List<MasterConfiguration>();

                    Allname24 = _OralTestMarksENtryContext.MasterConfiguration.Where(t => t.MI_Id.Equals(mas.MI_Id) && t.ASMAY_Id.Equals(mas.ASMAY_Id)).ToList().ToList();


                    // long PASWMS_Id = 0;

                    if (Allname1.Count > 0)
                    {
                        // PASWMS_Id = Allname1[0].PASWMS_Id;

                        var result = _OralTestMarksENtryContext.OralTestStudentWiseMarksDMO.Single(t => t.PAOTM_Id.Equals(mas.PAOTM_Id) && t.PASR_Id.Equals(mas.SelectedStudentMarksData[i].PASR_Id));


                        result.PAOTM_Id = mas.PAOTM_Id;
                        result.PASR_Id = mas.SelectedStudentMarksData[i].PASR_Id;
                        result.PAOTMS_Marks = mas.SelectedStudentMarksData[i].ObtMarks;

                        if (mas.SelectedStudentMarksData[i].ObtMarks > Allname24[0].ISPAC_OralByMin_Marks)
                        {

                        }
                        else
                        {
                            passfail = "Fail";
                        }
                        //added by 02/02/2017

                        result.UpdatedDate = DateTime.Now;
                        _OralTestMarksENtryContext.Update(result);
                        _OralTestMarksENtryContext.SaveChanges();
                    }
                    else
                    {
                        OralTestStudentWiseMarksDMO MM2 = new OralTestStudentWiseMarksDMO();
                        MM2.PAOTM_Id = mas.PAOTM_Id;
                        MM2.PASR_Id = mas.SelectedStudentMarksData[i].PASR_Id;
                        MM2.PAOTMS_Marks = mas.SelectedStudentMarksData[i].ObtMarks;

                        if (mas.SelectedStudentMarksData[i].ObtMarks > Allname24[0].ISPAC_OralByMin_Marks)
                        {

                        }
                        else
                        {
                            passfail = "Fail";
                        }
                        //added by 02/02/2017
                        MM2.CreatedDate = DateTime.Now;
                        MM2.UpdatedDate = DateTime.Now;
                        _OralTestMarksENtryContext.Add(MM2);
                        _OralTestMarksENtryContext.SaveChanges();

                    }

                    if (PASR_Id_new == 0)
                    {
                        PASR_Id_new = mas.SelectedStudentMarksData[i].PASR_Id;
                    }

                    if (mas.SelectedStudentMarksData[i].PASR_Id == PASR_Id_new)
                    {
                        obtTotal = obtTotal + mas.SelectedStudentMarksData[i].ObtMarks;
                    }

                    if (mas.SelectedStudentMarksData[i].PASR_Id != PASR_Id_new || i == mas.SelectedStudentMarksData.Count - 1)
                    {
                        PASR_Id_new = mas.SelectedStudentMarksData[i].PASR_Id;

                        List<OralTestStudentStatusDMO> Allname12 = new List<OralTestStudentStatusDMO>();

                        Allname12 = _OralTestMarksENtryContext.OralTestStudentStatusDMO.Where(t => t.MI_Id.Equals(mas.MI_Id) && t.ASMAY_Id.Equals(mas.ASMAY_Id) && t.PASR_Id.Equals(mas.SelectedStudentMarksData[i].PASR_Id)).ToList().ToList();

                        if (Allname12.Count > 0)
                        {

                            var result = _OralTestMarksENtryContext.OralTestStudentStatusDMO.Single(t => t.MI_Id.Equals(mas.MI_Id) && t.ASMAY_Id.Equals(mas.ASMAY_Id) && t.PASR_Id.Equals(mas.SelectedStudentMarksData[i].PASR_Id));

                            result.MI_Id = mas.MI_Id;
                            result.ASMAY_Id = mas.ASMAY_Id;
                            result.PASR_Id = mas.SelectedStudentMarksData[i].PASR_Id;
                            result.PAOTSS_OverallMarks = obtTotal;

                            if (passfail == "Pass")
                            {
                                result.PAOTSS_Status = "Pass";
                            }
                            else
                            {
                                result.PAOTSS_Status = "Fail";
                            }
                            //added by 02/02/2017

                            result.UpdatedDate = DateTime.Now;
                            _OralTestMarksENtryContext.Update(result);
                            _OralTestMarksENtryContext.SaveChanges();
                        }
                        else
                        {
                            OralTestStudentStatusDMO MM2 = new OralTestStudentStatusDMO();

                            MM2.MI_Id = mas.MI_Id;
                            MM2.ASMAY_Id = mas.ASMAY_Id;
                            MM2.PASR_Id = mas.SelectedStudentMarksData[i].PASR_Id;
                            MM2.PAOTSS_OverallMarks = obtTotal;

                            if (passfail == "Pass")
                            {
                                MM2.PAOTSS_Status = "Pass";
                            }
                            else
                            {
                                MM2.PAOTSS_Status = "Fail";
                            }
                            //added by 02/02/2017
                            MM2.CreatedDate = DateTime.Now;
                            MM2.UpdatedDate = DateTime.Now;
                            _OralTestMarksENtryContext.Add(MM2);
                            _OralTestMarksENtryContext.SaveChanges();

                        }


                    }


                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }


            return mas;
        }

        public OralTestMarksBindDataDTO[] GetOralTestMarks(OralTestMarksBindDataDTO mas)
        {
            int Person = 0;

            var Acdemic_preadmission = _OralTestMarksENtryContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == mas.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

            mas.ASMAY_Id = Acdemic_preadmission;

            List<OralTestMarksBindDataDTO> AllInOne = new List<OralTestMarksBindDataDTO>();

            List<MasterConfiguration> Allname24 = new List<MasterConfiguration>();

            Allname24 = _OralTestMarksENtryContext.MasterConfiguration.Where(t => t.MI_Id.Equals(mas.MI_Id) && t.ASMAY_Id.Equals(mas.ASMAY_Id)).ToList().ToList();
            decimal maxmarks = Allname24[0].ISPAC_OralByMax_Marks;

            var StudentId = mas.SelectedStudentDetails.Select(t => t.pasR_Id).ToList();

            for (int p = 0; p < mas.SelectedStudentDetails.Count; p++)
            {
                List<OralTestStudentWiseMarksDMO> Allname1 = new List<OralTestStudentWiseMarksDMO>();
                if (mas.PAOTM_Id == 0)
                {
                    Allname1 = _OralTestMarksENtryContext.OralTestStudentWiseMarksDMO.Where(t => t.PASR_Id.Equals(mas.SelectedStudentDetails[p].pasR_Id)).Take(mas.OralTestByPerson).ToList().ToList();
                }
                else
                {
                    Allname1 = _OralTestMarksENtryContext.OralTestStudentWiseMarksDMO.Where(t => t.PAOTM_Id.Equals(mas.PAOTM_Id) && t.PASR_Id.Equals(mas.SelectedStudentDetails[p].pasR_Id)).Take(mas.OralTestByPerson).ToList().ToList();
                }
                mas.WirettenTestSubjectWiseStudentMarks = Allname1.ToArray();


                int count = 0;

                if (mas.OralTestByPerson > Allname1.Count)
                {
                    count = mas.OralTestByPerson;
                }
                else
                {
                    count = Allname1.Count;
                }

                for (int i = 0; i < count; i++)
                {
                    OralTestMarksBindDataDTO temp = new OralTestMarksBindDataDTO();

                    List<StudentDetailsDMO> Allname10 = new List<StudentDetailsDMO>();
                    Allname10 = _OralTestMarksENtryContext.StudentDetailsDMO.Where(t => t.PASR_Id.Equals(mas.SelectedStudentDetails[p].pasR_Id)).ToList().ToList();
                    mas.studentDetails = Allname10.ToArray();

                    if (i == 0)
                    {
                        temp.PASR_FirstName = Allname10[0].PASR_FirstName;
                        temp.PASR_MiddleName = Allname10[0].PASR_MiddleName;
                        temp.PASR_LastName = Allname10[0].PASR_LastName;
                        temp.PASR_Id = Allname10[0].PASR_Id;
                    }
                    else
                    {
                        temp.PASR_FirstName ="";
                        temp.PASR_MiddleName = "";
                        temp.PASR_LastName = "";
                        temp.PASR_Id = Allname10[0].PASR_Id;
                    }

                    if (i < Allname1.Count)
                    {

                        temp.ObtMarks = Allname1[i].PAOTMS_Marks;
                    }
                    else
                    {
                        temp.ObtMarks = 0;
                    }

                    if (Person < mas.OralTestByPerson)
                    {
                        if (i < mas.OralTestByPerson)
                        {
                            temp.OralTestByPerson = i + 1;
                            temp.Oral_MaxMarks = Allname24[0].ISPAC_OralByMax_Marks;
                            Person = Person + 1;
                            temp.flagsubject = "Common";
                        }
                        else
                        {
                            temp.PAMS_SubjectName = "";
                            temp.Oral_MaxMarks = Allname24[0].ISPAC_OralByMax_Marks;
                            temp.PAMS_Id = 0;
                            temp.flagsubject = "";
                        }
                    }
                    else
                    {
                        temp.PAMS_SubjectName = "";
                        temp.Oral_MaxMarks = Allname24[0].ISPAC_OralByMax_Marks;
                        temp.PAMS_Id = 0;
                        temp.flagsubject = "";
                    }
                    AllInOne.Add(temp);
                }

            }

            return AllInOne.ToArray();
        }
    }
}
