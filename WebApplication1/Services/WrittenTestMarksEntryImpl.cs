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
    public class WrittenTestMarksEntryImpl : Interfaces.WrittenTestMarksEntryInterface
    {
        private static ConcurrentDictionary<string, StudentDetailsDTO> _login =
         new ConcurrentDictionary<string, StudentDetailsDTO>();

        private readonly WrittenTestMarksEntryContext _WrittenTestMarksEntryContext;

        public WrittenTestMarksEntryImpl(WrittenTestMarksEntryContext WrittenTestMarksEntryContext)
        {
            _WrittenTestMarksEntryContext = WrittenTestMarksEntryContext;

        }

        public WrittenTestMarksBindDataDTO GetWrittenTestMarksEntryData(WrittenTestMarksBindDataDTO WrittenTestMarksBindDataDTO)//int IVRMM_Id
        {
            //WrittenTestMarksBindDataDTO WrittenTestMarksBindDataDTO = new WrittenTestMarksBindDataDTO();
            var Acdemic_preadmission = _WrittenTestMarksEntryContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == WrittenTestMarksBindDataDTO.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

            WrittenTestMarksBindDataDTO.ASMAY_Id = Acdemic_preadmission;

            Array[] showdata3 = new Array[1];
            List<MasterConfiguration> Allname3 = new List<MasterConfiguration>();
            Allname3 = _WrittenTestMarksEntryContext.MasterConfiguration.Where(t => t.MI_Id.Equals(WrittenTestMarksBindDataDTO.MI_Id)).ToList().ToList();

            if (Allname3.Count > 0)
            {
                WrittenTestMarksBindDataDTO.MasterConfiguration = Allname3.ToArray();
                WrittenTestMarksBindDataDTO.WrittenTestScheduleAppFlag = Allname3[0].ISPAC_WrittenTestApplFlag;


                //Array[] showdata2 = new Array[1];
                //List<MasterSubjectDMO> Allname2 = new List<MasterSubjectDMO>();
                //Allname2 = _WrittenTestMarksEntryContext.MasterSubjectDMO.Where(t => t.MI_Id.Equals(WrittenTestMarksBindDataDTO.MI_Id)).OrderBy(t => t.PAMSU_SubjectName).ToList();
                //WrittenTestMarksBindDataDTO.SubjectNames = Allname2.ToArray();


                Array[] showdata2 = new Array[1];
                List<IVRM_Master_SubjectsDMO> Allname2 = new List<IVRM_Master_SubjectsDMO>();
                Allname2 = _WrittenTestMarksEntryContext.allSubject.Where(t => t.MI_Id.Equals(WrittenTestMarksBindDataDTO.MI_Id) && t.ISMS_PreadmFlag == 1 && t.ISMS_ActiveFlag == 1).OrderBy(t => t.ISMS_Id).ToList();
                WrittenTestMarksBindDataDTO.SubjectNames = Allname2.ToArray();

                Array[] showdata1 = new Array[1];
                List<StudentApplication> Allname1 = new List<StudentApplication>();
                Allname1 = _WrittenTestMarksEntryContext.StudentDetailsDMO.Where(t => t.MI_Id.Equals(WrittenTestMarksBindDataDTO.MI_Id) && t.ASMAY_Id.Equals(WrittenTestMarksBindDataDTO.ASMAY_Id)).OrderBy(t => t.PASR_FirstName).ToList();
                WrittenTestMarksBindDataDTO.studentDetails = Allname1.ToArray();




                Array[] showdata = new Array[50];
                List<WrittenTestScheduleDMO> Allname = new List<WrittenTestScheduleDMO>();
                Allname = _WrittenTestMarksEntryContext.WrittenTestScheduleDMO.Where(t => t.MI_Id.Equals(WrittenTestMarksBindDataDTO.MI_Id) && t.ASMAY_Id.Equals(WrittenTestMarksBindDataDTO.ASMAY_Id)).OrderBy(s => s.PAWTS_ScheduleName).ToList();
                WrittenTestMarksBindDataDTO.WrittenTestSchedule = Allname.ToArray();

                WrittenTestMarksBindDataDTO.Chq_config = true;
            }
            else
            {
                WrittenTestMarksBindDataDTO.Chq_config = false;
            }


            return WrittenTestMarksBindDataDTO;
        }

        public WirttenTestSubjectWiseMarksEntryDTO GetdetailsOnSchedule(WirttenTestSubjectWiseMarksEntryDTO mas)
        {
            // OralTestMarksBindDataDTO OralTestMarksBindDataDTO = new OralTestMarksBindDataDTO();
            var Acdemic_preadmission = _WrittenTestMarksEntryContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == mas.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

            mas.ASMAY_Id = Acdemic_preadmission;

            if (mas.PAWTS_Id == 0)
            {
                Array[] showdata1 = new Array[1];
                List<StudentApplication> Allname1 = new List<StudentApplication>();
                Allname1 = _WrittenTestMarksEntryContext.StudentDetailsDMO.Where(t => t.MI_Id.Equals(mas.MI_Id) && t.ASMAY_Id.Equals(mas.ASMAY_Id)).OrderBy(t => t.PASR_FirstName).ToList();
                mas.studentDetails = Allname1.ToArray();
            }
            else
            {

                var scheduleIds = _WrittenTestMarksEntryContext.WrittenTestScheduleDMO.Where(t => t.MI_Id.Equals(mas.MI_Id) && t.ASMAY_Id.Equals(mas.ASMAY_Id) && t.PAWTS_Id.Equals(mas.PAWTS_Id)).Select(m => m.PAWTS_Id).ToArray();

                var StdIds = _WrittenTestMarksEntryContext.WrittenTestScheduleStudentInsertDMO.Where(t => scheduleIds.Contains(t.PAWTS_Id)).Select(t => t.PASR_Id).ToArray().ToArray();

                Array[] showdata1 = new Array[1];
                List<StudentApplication> Allname1 = new List<StudentApplication>();
                Allname1 = _WrittenTestMarksEntryContext.StudentDetailsDMO.Where(t => StdIds.Contains(t.pasr_id)).OrderBy(t => t.PASR_FirstName).ToList();
                mas.studentDetails = Allname1.ToArray();
            }

            return mas;
        }
        public WirttenTestSubjectWiseMarksEntryDTO WrittenTestMarksEntryData(WirttenTestSubjectWiseMarksEntryDTO mas)
        {

            decimal obtTotal = 0;
            string passfail = "Pass";
            long PASR_Id_new = 0;

            var Acdemic_preadmission = _WrittenTestMarksEntryContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == mas.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

            mas.ASMAY_Id = Acdemic_preadmission;

            for (int i = 0; i < mas.SelectedStudentMarksData.Count; i++)
            {

                WIrttenTestSubjectWiseMarksDMO MM = Mapper.Map<WIrttenTestSubjectWiseMarksDMO>(mas);


                List<WIrttenTestSubjectWiseMarksDMO> Allname2 = new List<WIrttenTestSubjectWiseMarksDMO>();

                Allname2 = _WrittenTestMarksEntryContext.WIrttenTestSubjectWiseMarksDMO.Where(t => t.ISMS_ID.Equals(mas.SelectedStudentMarksData[i].ISMS_Id) && t.MI_Id.Equals(mas.MI_Id)).ToList();

                if (Allname2.Count > 0)
                {
                    mas.PASWM_Id = Allname2[0].PASWM_Id;
                }
                else
                {
                    MM.PASWM_Id = 0;

                    MM.PASWM_Date = System.DateTime.Today;
                    MM.PASWM_Date = Convert.ToDateTime(MM.PASWM_Date);
                    MM.MI_Id = mas.MI_Id;
                    MM.ASMAY_Id = mas.ASMAY_Id;
                    MM.ISMS_ID = mas.SelectedStudentMarksData[i].ISMS_Id;
                    MM.CreatedDate = DateTime.Now;
                    MM.UpdatedDate = DateTime.Now;
                    _WrittenTestMarksEntryContext.Add(MM);
                    _WrittenTestMarksEntryContext.SaveChanges();

                    mas.PASWM_Id = MM.PASWM_Id;

                    if (mas.WrittenTestScheduleAppFlag == "true")
                    {
                        List<WrittenTestScheduleMarksDMO> Allname22 = new List<WrittenTestScheduleMarksDMO>();

                        Allname22 = _WrittenTestMarksEntryContext.WrittenTestScheduleMarksDMO.Where(t => t.PAWTS_Id.Equals(mas.PAWTS_Id) && t.PASWM_Id.Equals(mas.PASWM_Id)).ToList();

                        if (Allname22.Count > 0)
                        {

                        }
                        else
                        {
                            WrittenTestScheduleMarksDMO MM5 = new WrittenTestScheduleMarksDMO();
                            MM5.PASHWTM_Id = 0;
                            MM5.PAWTS_Id = mas.PAWTS_Id;
                            MM5.PASWM_Id = mas.PASWM_Id;


                            //added by 02/02/2017
                            MM5.CreatedDate = DateTime.Now;
                            MM5.UpdatedDate = DateTime.Now;
                            _WrittenTestMarksEntryContext.Add(MM5);
                            _WrittenTestMarksEntryContext.SaveChanges();

                        }
                    }
                }

                List<WrittenTestStudentSubjectWiseMarksDMO> Allname1 = new List<WrittenTestStudentSubjectWiseMarksDMO>();

                Allname1 = _WrittenTestMarksEntryContext.WrittenTestStudentSubjectWiseMarksDMO.Where(t => t.PASWM_Id.Equals(mas.PASWM_Id) && t.PASR_Id.Equals(mas.SelectedStudentMarksData[i].PASR_Id)).ToList();

                List<IVRM_Master_SubjectsDMO> subMaxMarks = new List<IVRM_Master_SubjectsDMO>();

                subMaxMarks = _WrittenTestMarksEntryContext.allSubject.Where(t => t.ISMS_Id.Equals(mas.SelectedStudentMarksData[i].ISMS_Id)).ToList();

                // long PASWMS_Id = 0;

                if (Allname1.Count > 0)
                {
                    // PASWMS_Id = Allname1[0].PASWMS_Id;

                    var result = _WrittenTestMarksEntryContext.WrittenTestStudentSubjectWiseMarksDMO.Single(t => t.PASWM_Id.Equals(mas.PASWM_Id) && t.PASR_Id.Equals(mas.SelectedStudentMarksData[i].PASR_Id));

                    result.PASWMS_Id = result.PASWMS_Id;
                    result.MI_Id = mas.MI_Id;
                    result.PASWM_Id = mas.PASWM_Id;
                    result.PASR_Id = mas.SelectedStudentMarksData[i].PASR_Id;
                    result.PASWMS_MarksScored = mas.SelectedStudentMarksData[i].ObtMarks;

                    if (mas.SelectedStudentMarksData[i].ObtMarks > subMaxMarks[0].ISMS_Min_Marks)
                    {
                        result.PASWMS_PassFail = "Pass";
                    }
                    else
                    {
                        result.PASWMS_PassFail = "Fail";
                        passfail = "Fail";
                    }


                    //added by 02/02/2017

                    result.UpdatedDate = DateTime.Now;
                    _WrittenTestMarksEntryContext.Update(result);
                    _WrittenTestMarksEntryContext.SaveChanges();
                }
                else
                {
                    WrittenTestStudentSubjectWiseMarksDMO MM2 = new WrittenTestStudentSubjectWiseMarksDMO();
                    MM2.MI_Id = mas.MI_Id;
                    MM2.PASWM_Id = mas.PASWM_Id;
                    MM2.PASR_Id = mas.SelectedStudentMarksData[i].PASR_Id;
                    MM2.PASWMS_MarksScored = mas.SelectedStudentMarksData[i].ObtMarks;

                    if (mas.SelectedStudentMarksData[i].ObtMarks > subMaxMarks[0].ISMS_Min_Marks)
                    {
                        MM2.PASWMS_PassFail = "Pass";
                    }
                    else
                    {
                        MM2.PASWMS_PassFail = "Fail";
                        passfail = "Fail";
                    }


                    //added by 02/02/2017
                    MM2.CreatedDate = DateTime.Now;
                    MM2.UpdatedDate = DateTime.Now;
                    _WrittenTestMarksEntryContext.Add(MM2);
                    _WrittenTestMarksEntryContext.SaveChanges();

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

                    List<WrittenTestStudentWiseTotalMarksDMO> Allname12 = new List<WrittenTestStudentWiseTotalMarksDMO>();

                    Allname12 = _WrittenTestMarksEntryContext.WrittenTestStudentWiseTotalMarksDMO.Where(t => t.MI_Id.Equals(mas.MI_Id) && t.ASMAY_Id.Equals(mas.ASMAY_Id) && t.PASR_Id.Equals(mas.SelectedStudentMarksData[i].PASR_Id)).ToList();

                    if (Allname12.Count > 0)
                    {

                        var result = _WrittenTestMarksEntryContext.WrittenTestStudentWiseTotalMarksDMO.Single(t => t.MI_Id.Equals(mas.MI_Id) && t.ASMAY_Id.Equals(mas.ASMAY_Id) && t.PASR_Id.Equals(mas.SelectedStudentMarksData[i].PASR_Id));

                        result.MI_Id = mas.MI_Id;
                        result.ASMAY_Id = mas.ASMAY_Id;
                        result.PASR_Id = mas.SelectedStudentMarksData[i].PASR_Id;
                        result.PASR_TotalMarksScored = obtTotal;

                        if (passfail == "Pass")
                        {
                            result.PASR_Status = "Pass";
                        }
                        else
                        {
                            result.PASR_Status = "Fail";
                        }
                        //added by 02/02/2017

                        result.UpdatedDate = DateTime.Now;
                        _WrittenTestMarksEntryContext.Update(result);
                        _WrittenTestMarksEntryContext.SaveChanges();
                    }
                    else
                    {
                        WrittenTestStudentWiseTotalMarksDMO MM2 = new WrittenTestStudentWiseTotalMarksDMO();

                        MM2.MI_Id = mas.MI_Id;
                        MM2.ASMAY_Id = mas.ASMAY_Id;
                        MM2.PASR_Id = mas.SelectedStudentMarksData[i].PASR_Id;
                        MM2.PASR_TotalMarksScored = obtTotal;

                        if (passfail == "Pass")
                        {
                            MM2.PASR_Status = "Pass";
                        }
                        else
                        {
                            MM2.PASR_Status = "Fail";
                        }
                        //added by 02/02/2017
                        MM2.CreatedDate = DateTime.Now;
                        MM2.UpdatedDate = DateTime.Now;
                        _WrittenTestMarksEntryContext.Add(MM2);
                        _WrittenTestMarksEntryContext.SaveChanges();

                    }


                }


            }


            return mas;
        }

        public WrittenTestMarksBindDataDTO GetWrittenTestMarks(WrittenTestMarksBindDataDTO mas)
        {

            var Acdemic_preadmission = _WrittenTestMarksEntryContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == mas.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

            mas.ASMAY_Id = Acdemic_preadmission;

            List<StudentApplication> allRegStudent = new List<StudentApplication>();
            allRegStudent =  _WrittenTestMarksEntryContext.StudentDetailsDMO.Where(d => d.MI_Id.Equals(mas.MI_Id) && d.ASMAY_Id== mas.ASMAY_Id).ToList();
            mas.registrationList = allRegStudent.ToArray();

            List<AdmissionClass> allclass = new List<AdmissionClass>();
            //*****MAXMINAGE****
            //   allclass = await _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id.Equals(stu.MI_Id)).ToListAsync();
            allclass = _WrittenTestMarksEntryContext.AdmissionClass.Where(t => t.MI_Id == mas.MI_Id && t.ASMCL_ActiveFlag == true && t.ASMCL_PreadmFlag == 1).ToList();
            mas.admissioncatdrp = allclass.ToArray();

            allclass = _WrittenTestMarksEntryContext.AdmissionClass.Where(t => t.MI_Id == mas.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
            mas.admissioncatdrpall  = allclass.ToArray();




            int Person = 0;
            List<WrittenTestMarksBindDataDTO> AllInOne = new List<WrittenTestMarksBindDataDTO>();

            for (int Q = 0; Q < mas.SelectedStudentDetails.Count; Q++)
            {
                List<IVRM_Master_SubjectsDMO> Allname3 = new List<IVRM_Master_SubjectsDMO>();
                if (mas.ISMS_Id == 0)
                {
                    Allname3 = _WrittenTestMarksEntryContext.allSubject.Where(t => t.MI_Id.Equals(mas.MI_Id) && t.ISMS_PreadmFlag == 1 && t.ISMS_ActiveFlag == 1).OrderBy(t => t.ISMS_Id).ToList();
                }
                else
                {
                    Allname3 = _WrittenTestMarksEntryContext.allSubject.Where(t => t.ISMS_Id.Equals(mas.ISMS_Id)).OrderBy(t => t.ISMS_Id).ToList();
                }

                mas.SelectedSubjectNames = Allname3.ToArray();

                //List<WIrttenTestSubjectWiseMarksDMO> Allname2 = new List<WIrttenTestSubjectWiseMarksDMO>();
                List<WrittenTestMarksBindDataDTO> Allname2 = new List<WrittenTestMarksBindDataDTO>();

                if (mas.ISMS_Id == 0)
                {
                    // Allname2 = _WrittenTestMarksEntryContext.WIrttenTestSubjectWiseMarksDMO.Where(t => t.MI_Id.Equals(mas.MI_Id) && t.ASMAY_Id.Equals(mas.ASMAY_Id)).OrderBy(t => t.ISMS_ID).ToList();
                    Allname2 = (from a in _WrittenTestMarksEntryContext.WIrttenTestSubjectWiseMarksDMO
                                from c in _WrittenTestMarksEntryContext.allSubject
                                where (a.ISMS_ID == c.ISMS_Id && a.MI_Id == mas.MI_Id)  && c.ISMS_PreadmFlag == 1 && c.ISMS_ActiveFlag == 1
                                select new WrittenTestMarksBindDataDTO
                                {
                                    PASWM_Id = a.PASWM_Id
                                }
                       ).OrderBy(c => c.ISMS_Id).ToList();
                }
                else
                {
                    // Allname2 = _WrittenTestMarksEntryContext.WIrttenTestSubjectWiseMarksDMO.Where(t => t.ISMS_ID == mas.ISMS_Id && t.MI_Id.Equals(mas.MI_Id) && t.ASMAY_Id.Equals(mas.ASMAY_Id)).OrderBy(t => t.ISMS_ID).ToList();
                    Allname2 = (from a in _WrittenTestMarksEntryContext.WIrttenTestSubjectWiseMarksDMO
                                from c in _WrittenTestMarksEntryContext.allSubject
                                where (a.ISMS_ID == c.ISMS_Id && a.MI_Id == mas.MI_Id)  && a.ISMS_ID == mas.ISMS_Id && c.ISMS_PreadmFlag == 1 && c.ISMS_ActiveFlag == 1
                                select new WrittenTestMarksBindDataDTO
                                {
                                    PASWM_Id = a.PASWM_Id
                                }
                     ).OrderBy(c => c.ISMS_Id).ToList();
                }
                mas._list_dto = Allname2.ToArray();
                List<long> temparr = new List<long>();
                foreach (WrittenTestMarksBindDataDTO ph in mas._list_dto)
                {
                    temparr.Add(ph.PASWM_Id);
                }

                try
                {
                    List<WrittenTestMarksBindDataDTO> Allname1 = new List<WrittenTestMarksBindDataDTO>();
                    // List<WrittenTestStudentSubjectWiseMarksDMO> Allname1 = new List<WrittenTestStudentSubjectWiseMarksDMO>();
                    if (mas.ISMS_Id == 0)
                    {
                        //  Allname1 = _WrittenTestMarksEntryContext.WrittenTestStudentSubjectWiseMarksDMO.Where(t => t.PASR_Id.Equals(mas.SelectedStudentDetails[Q].pasR_Id)).ToList().ToList();


                        Allname1 = (from a in _WrittenTestMarksEntryContext.WrittenTestStudentSubjectWiseMarksDMO
                                    from b in _WrittenTestMarksEntryContext.WIrttenTestSubjectWiseMarksDMO
                                    from c in _WrittenTestMarksEntryContext.allSubject
                                    where (a.PASWM_Id == b.PASWM_Id && b.ISMS_ID == c.ISMS_Id && a.PASR_Id.Equals(mas.SelectedStudentDetails[Q].pasR_Id) && temparr.Contains(a.PASWM_Id))
                                    select new WrittenTestMarksBindDataDTO
                                    {
                                        PASR_Id = a.PASR_Id,
                                        PASWMS_Id = a.PASWMS_Id,
                                        PASWMS_MarksScored = a.PASWMS_MarksScored,
                                        PASWMS_PassFail = a.PASWMS_PassFail,
                                        PASWM_Id = a.PASWM_Id,
                                        ISMS_Id = b.ISMS_ID
                                    }
                          ).OrderBy(t => t.ISMS_Id).ToList();
                    }
                    else
                    {
                        Allname1 = (from a in _WrittenTestMarksEntryContext.WrittenTestStudentSubjectWiseMarksDMO
                                    where (a.PASWM_Id.Equals(Allname2[0].PASWM_Id) && a.PASR_Id.Equals(mas.SelectedStudentDetails[Q].pasR_Id))
                                    select new WrittenTestMarksBindDataDTO
                                    {
                                        PASR_Id = a.PASR_Id,
                                        PASWMS_Id = a.PASWMS_Id,
                                        PASWMS_MarksScored = a.PASWMS_MarksScored,
                                        PASWMS_PassFail = a.PASWMS_PassFail,
                                        PASWM_Id = a.PASWM_Id,
                                        ISMS_Id = mas.ISMS_Id
                                    }
                             ).ToList();
                        //  Allname1 = _WrittenTestMarksEntryContext.WrittenTestStudentSubjectWiseMarksDMO.Where(t => t.PASWM_Id.Equals(Allname2[0].PASWM_Id) && t.PASR_Id.Equals(mas.SelectedStudentDetails[Q].pasR_Id)).ToList();
                    }


                    mas.WirettenTestSubjectWiseStudentMarks = Allname1.ToArray();

                    int count = 0;

                    if (Allname3.Count > Allname1.Count)
                    {
                        count = Allname3.Count;
                    }
                    else
                    {
                        count = Allname1.Count;
                    }

                    for (int i = 0; i < count; i++)
                    {
                        WrittenTestMarksBindDataDTO temp = new WrittenTestMarksBindDataDTO();

                        List<StudentApplication> Allname10 = new List<StudentApplication>();
                        Allname10 = _WrittenTestMarksEntryContext.StudentDetailsDMO.Where(t => t.pasr_id.Equals(mas.SelectedStudentDetails[Q].pasR_Id)).ToList().ToList();
                        mas.studentDetails = Allname10.ToArray();





                        if (i == 0)
                        {
                            temp.PASR_FirstName = Allname10[0].PASR_FirstName;
                            temp.PASR_MiddleName = Allname10[0].PASR_MiddleName;
                            temp.PASR_LastName = Allname10[0].PASR_LastName;
                            temp.PASR_Id = Allname10[0].pasr_id;
                        }
                        else
                        {
                            temp.PASR_FirstName = "";
                            temp.PASR_MiddleName = "";
                            temp.PASR_LastName = "";
                            temp.PASR_Id = Allname10[0].pasr_id;
                        }

                        if (i < Allname1.Count)
                        {

                            temp.ObtMarks = Allname1[i].PASWMS_MarksScored;
                        }
                        else
                        {
                            temp.ObtMarks = 0;
                        }


                        if (Person < Allname3.Count)
                        {
                            if (i < Allname3.Count)
                            {
                                temp.OralTestByPerson = i + 1;
                                temp.ISMS_SubjectName = Allname3[i].ISMS_SubjectName;
                                temp.ISMS_Max_Marks = Allname3[i].ISMS_Max_Marks;
                                temp.ISMS_Id = Allname3[i].ISMS_Id;
                                temp.flagsubject = "Common";
                                Person = Person + 1;
                            }
                            else
                            {
                                temp.ISMS_SubjectName = "";
                                temp.ISMS_Max_Marks = Allname3[i].ISMS_Max_Marks;
                                temp.ISMS_Id = 0;
                                temp.flagsubject = "";
                            }
                        }
                        else
                        {
                            temp.ISMS_SubjectName = "";
                            temp.ISMS_Max_Marks = Allname3[i].ISMS_Max_Marks;
                            temp.ISMS_Id = Allname3[i].ISMS_Id;
                        }
                        AllInOne.Add(temp);

                    }
                    mas.AllInOne1 = AllInOne;
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }

            }



            return mas;
        }

        //public WrittenTestMarksBindDataDTO[] GetWrittenTestMarks(WrittenTestMarksBindDataDTO mas)
        //{

        //    List<WrittenTestMarksBindDataDTO> AllInOne = new List<WrittenTestMarksBindDataDTO>();

        //    List<long> SelectedStudentDet = new List<long>();

        //    foreach (var item in mas.SelectedStudentDetails)
        //    {
        //        SelectedStudentDet.Add(item.pasR_Id);
        //    }

        //    List<WrittenTestMarksBindDataDTO> temp = new List<WrittenTestMarksBindDataDTO>();


        //    if (mas.ISMS_Id == 0)
        //    {
        //        temp = (from a in _WrittenTestMarksEntryContext.WrittenTestStudentSubjectWiseMarksDMO
        //                from b in _WrittenTestMarksEntryContext.StudentDetailsDMO
        //                from c in _WrittenTestMarksEntryContext.WrittenTestScheduleStudentInsertDMO
        //                from d in _WrittenTestMarksEntryContext.WIrttenTestSubjectWiseMarksDMO
        //                from e in _WrittenTestMarksEntryContext.allSubject
        //                where (a.PASWM_Id == d.PASWM_Id && a.PASR_Id == b.PASR_Id && c.PASR_Id == b.PASR_Id && b.MI_Id == mas.MI_Id && c.PAWTS_Id == mas.PAWTS_ID && SelectedStudentDet.Contains(b.PASR_Id))
        //                select new WrittenTestMarksBindDataDTO
        //                {
        //                    ISMS_Id = d.ISMS_ID,
        //                    PASR_Id = b.PASR_Id,
        //                    PASR_FirstName = b.PASR_FirstName,
        //                    PASR_MiddleName = b.PASR_MiddleName,
        //                    PASR_LastName = b.PASR_LastName,
        //                    ObtMarks = a.PASWMS_MarksScored,
        //                    ISMS_SubjectName = e.ISMS_SubjectName,
        //                    PAWTS_ID = c.PAWTS_Id

        //                }).ToList();
        //    }
        //    else
        //    {
        //        temp = (from a in _WrittenTestMarksEntryContext.WrittenTestStudentSubjectWiseMarksDMO
        //                from b in _WrittenTestMarksEntryContext.StudentDetailsDMO
        //                from c in _WrittenTestMarksEntryContext.WrittenTestScheduleStudentInsertDMO
        //                from d in _WrittenTestMarksEntryContext.WIrttenTestSubjectWiseMarksDMO
        //                from e in _WrittenTestMarksEntryContext.allSubject
        //                where (a.PASWM_Id == d.PASWM_Id && a.PASR_Id == b.PASR_Id && c.PASR_Id == b.PASR_Id && b.MI_Id == mas.MI_Id && c.PAWTS_Id == mas.PAWTS_ID && d.ISMS_ID == e.ISMS_Id && SelectedStudentDet.Contains(b.PASR_Id))
        //                select new WrittenTestMarksBindDataDTO
        //                {
        //                    ISMS_Id = d.ISMS_ID,
        //                    PASR_Id = b.PASR_Id,
        //                    PASR_FirstName = b.PASR_FirstName,
        //                    PASR_MiddleName = b.PASR_MiddleName,
        //                    PASR_LastName = b.PASR_LastName,
        //                    ObtMarks = a.PASWMS_MarksScored,
        //                    ISMS_SubjectName = e.ISMS_SubjectName,
        //                    PAWTS_ID = c.PAWTS_Id

        //                }).ToList();

        //    }

        //    AllInOne = temp;


        //    return AllInOne.ToArray();
        //}

    }
}
