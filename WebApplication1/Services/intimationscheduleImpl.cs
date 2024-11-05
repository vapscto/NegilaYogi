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
using CommonLibrary;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication1.Services
{
    public class intimationscheduleImpl : Interfaces.intimationscheduleInterface
    {
        private static ConcurrentDictionary<string, StudentDetailsDTO> _login =
        new ConcurrentDictionary<string, StudentDetailsDTO>();

        private readonly OralTestScheduleContext _OralTestScheduleContext;
        private readonly DomainModelMsSqlServerContext _context;
        public StudentApplicationContext _StudentApplicationContext;
        public FeeGroupContext _feecontext;
        public intimationscheduleImpl(StudentApplicationContext StudentApplicationContext, OralTestScheduleContext OralTestScheduleContext, DomainModelMsSqlServerContext db, FeeGroupContext feecontext)
        {
            _OralTestScheduleContext = OralTestScheduleContext;
            _context = db;
            _StudentApplicationContext = StudentApplicationContext;
            _feecontext = feecontext;
        }


        public StudentDetailsDTO GetOralTestScheduleData(StudentDetailsDTO StudentDetailsDTO)//int IVRMM_Id
        {
            try
            {
                var Acdemic_preadmission = _StudentApplicationContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == StudentDetailsDTO.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

                StudentDetailsDTO.ASMAY_Id = Acdemic_preadmission;


                List<StudentApplication> lorg2 = new List<StudentApplication>();
                List<StudentApplication> lorg5 = new List<StudentApplication>();
                List<StudentApplication> lorgfinal = new List<StudentApplication>();
                Array[] showdata1 = new Array[1];

                List<OralTestScheduleStudentInsertDMO> lorg1 = new List<OralTestScheduleStudentInsertDMO>();
                lorg1 = _OralTestScheduleContext.OralTestScheduleStudentInsertDMO.ToList();



                List<StudentApplication> Allname1 = new List<StudentApplication>();
                List<long> Allname2 = new List<long>();

                List<StudentApplication> Allname5 = new List<StudentApplication>();
                lorg5 = _OralTestScheduleContext.StudentApplication.ToList();


                //Year Class Status
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _context.AcademicYear.Where(t => t.MI_Id == StudentDetailsDTO.MI_Id && t.Is_Active == true).OrderByDescending(d => d.ASMAY_Order).ToList();
                StudentDetailsDTO.AcademicList = allyear.ToArray();

                List<School_M_Class> allclass = new List<School_M_Class>();
                allclass = _context.School_M_Class.Where(t => t.MI_Id == StudentDetailsDTO.MI_Id && t.ASMCL_ActiveFlag == true).OrderBy(d => d.ASMCL_Order).ToList();
                StudentDetailsDTO.classlist = allclass.ToArray();

                List<AdmissionStatus> status = new List<AdmissionStatus>();
                status = _context.status.Where(t => t.MI_Id == StudentDetailsDTO.MI_Id).ToList();
                StudentDetailsDTO.statuslist = status.ToArray();


                List<long> Allname3 = new List<long>();
                Allname5 = _OralTestScheduleContext.StudentApplication.Where(t => t.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id) && t.MI_Id.Equals(StudentDetailsDTO.MI_Id) && t.PASR_Adm_Confirm_Flag == false && t.PASRAPS_ID== 787928).ToList();

                List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                mstConfig = _StudentApplicationContext.masterConfig.Where(d => d.MI_Id.Equals(StudentDetailsDTO.MI_Id) && d.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id)).ToList();
                StudentDetailsDTO.mstConfig = mstConfig.ToArray();

                if (mstConfig[0].ISPAC_ApplFeeFlag == 1)
                {

                    if (mstConfig[0].ISPAC_ApplFeeFlag == 1)
                    {
                        foreach (var a in lorg5)
                        {
                            StudentDetailsDTO.payementcheck = _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO.Where(t => t.FYPPA_Type == "R" && t.PASA_Id == a.pasr_id).Count();

                            if (StudentDetailsDTO.payementcheck == 1)
                            {

                                Allname3.Add(a.pasr_id);

                            }
                        }

                        foreach (var a in lorg1)
                        {
                            Allname2.Add(a.PASR_Id);
                        }


                        Allname1 = _OralTestScheduleContext.StudentApplication.Where(t => t.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id) && t.MI_Id.Equals(StudentDetailsDTO.MI_Id) && t.PASR_Adm_Confirm_Flag == false && !Allname2.Contains(t.pasr_id) && Allname3.Contains(t.pasr_id)).ToList();

                        StudentDetailsDTO.studentDetails = Allname1.ToArray();

                    }
                }
                else
                {
                    foreach (var a in lorg1)
                    {
                        Allname2.Add(a.PASR_Id);
                    }

                    Allname1 = _OralTestScheduleContext.StudentApplication.Where(t => t.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id) && t.MI_Id.Equals(StudentDetailsDTO.MI_Id) && t.PASR_Adm_Confirm_Flag == false && !Allname2.Contains(t.pasr_id)).ToList();

                    StudentDetailsDTO.studentDetails = Allname1.ToArray();
                }

                Array[] showdata = new Array[50];
                List<OralTestScheduleDMO> Allname = new List<OralTestScheduleDMO>();
                Allname = _OralTestScheduleContext.OralTestScheduleDMO.Where(t => t.MI_Id.Equals(StudentDetailsDTO.MI_Id) && t.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id)).ToList();
                StudentDetailsDTO.OralTestSchedule = Allname.ToArray();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return StudentDetailsDTO;
        }

        public OralTestScheduleDTO GetSelectedRowDetails(int ID)
        {

            OralTestScheduleDTO OralTestScheduleDTO = new OralTestScheduleDTO();
            List<OralTestScheduleDMO> lorg = new List<OralTestScheduleDMO>();
            lorg = _OralTestScheduleContext.OralTestScheduleDMO.Where(t => t.PAOTS_Id.Equals(ID)).ToList();
            OralTestScheduleDTO.OralTestSchedule = lorg.ToArray();

            StudentDetailsDTO StudentDetailsDTO = new StudentDetailsDTO();

            List<OralTestScheduleStudentInsertDMO> lorg1 = new List<OralTestScheduleStudentInsertDMO>();
            List<StudentApplication> lorg2 = new List<StudentApplication>();
            List<StudentApplication> lorgfinal = new List<StudentApplication>();
            lorg1 = _OralTestScheduleContext.OralTestScheduleStudentInsertDMO.Where(t => t.PAOTS_Id.Equals(ID)).ToList();
            int j = 0;
            while (j < lorg1.Count())
            {
                lorg2.AddRange(_OralTestScheduleContext.StudentApplication.Where(t => t.pasr_id.Equals(lorg1[j].PASR_Id)).ToList());
                j++;
            }

            OralTestScheduleDTO.SelectedStudentDetails = lorg2.ToArray();
            OralTestScheduleDTO.OralTestSchedule = lorg.ToArray();

            return OralTestScheduleDTO;
        }

        public StudentDetailsDTO GetSelectedStudentData(int ID)
        {
            //int ID1 = Convert.ToInt32(ID);
            StudentDetailsDTO StudentDetailsDTO = new StudentDetailsDTO();

            List<StudentApplication> lorg = new List<StudentApplication>();


            lorg = _OralTestScheduleContext.StudentApplication.Where(t => t.pasr_id.Equals(ID)).ToList();



            StudentDetailsDTO.studentDetails = lorg.ToArray();

            return StudentDetailsDTO;
        }

        public OralTestScheduleDTO OralTestScheduleDeletesData(int ID)
        {

            OralTestScheduleDTO OralTestScheduleDTO = new OralTestScheduleDTO();


            List<OralTestScheduleStudentInsertDMO> masters = new List<OralTestScheduleStudentInsertDMO>();
            masters = _OralTestScheduleContext.OralTestScheduleStudentInsertDMO.Where(t => t.PAOTS_Id.Equals(ID)).ToList();



            if (masters.Any())
            {

                int j = 0;
                while (j < masters.Count())
                {
                    List<OralTestScheduleStudentInsertDMO> masters3 = new List<OralTestScheduleStudentInsertDMO>();
                    masters3 = _OralTestScheduleContext.OralTestScheduleStudentInsertDMO.Where(t => t.PAOTS_Id.Equals(masters[j].PAOTS_Id)).ToList();
                    _OralTestScheduleContext.Remove(masters3.ElementAt(0));
                    _OralTestScheduleContext.SaveChanges();

                    j++;
                }

                List<OralTestScheduleDMO> masters1 = new List<OralTestScheduleDMO>();
                masters1 = _OralTestScheduleContext.OralTestScheduleDMO.Where(t => t.PAOTS_Id.Equals(ID)).ToList();


                if (masters1.Any())
                {
                    _OralTestScheduleContext.Remove(masters1.ElementAt(0));
                    _OralTestScheduleContext.SaveChanges();

                }
                else
                {

                }
            }
            else
            {
                List<OralTestScheduleDMO> masters1 = new List<OralTestScheduleDMO>();
                masters1 = _OralTestScheduleContext.OralTestScheduleDMO.Where(t => t.PAOTS_Id.Equals(ID)).ToList();
                if (masters1.Any())
                {
                    _OralTestScheduleContext.Remove(masters1.ElementAt(0));
                    _OralTestScheduleContext.SaveChanges();

                }
                else
                {

                }
            }


            return OralTestScheduleDTO;
        }

        public OralTestScheduleDTO OralTestScheduleDeletesStudentData(OralTestScheduleDTO OralTestScheduleDTO)
        {
            List<OralTestScheduleStudentInsertDMO> masters = new List<OralTestScheduleStudentInsertDMO>();
            masters = _OralTestScheduleContext.OralTestScheduleStudentInsertDMO.Where(t => t.PASR_Id.Equals(OralTestScheduleDTO.PASR_Id) && t.PAOTS_Id.Equals(OralTestScheduleDTO.PAOTS_Id)).ToList();
            if (masters.Any())
            {
                _OralTestScheduleContext.Remove(masters.ElementAt(0));
                _OralTestScheduleContext.SaveChanges();
            }
            else
            {

            }
            return OralTestScheduleDTO;
        }
        public async Task<OralTestScheduleDTO> OralTestScheduleData(OralTestScheduleDTO mas)
        {

            var Acdemic_preadmission = _StudentApplicationContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == mas.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

            mas.ASMAY_Id = Acdemic_preadmission;

            OralTestScheduleDMO MM = Mapper.Map<OralTestScheduleDMO>(mas);
            long paotss = 0;
            if (mas.PAOTS_Id > 0)
            {
                

                var schedule = _OralTestScheduleContext.OralTestScheduleDMO.Where(t => t.PAOTS_Id != mas.PAOTS_Id && t.PAOTS_ScheduleName == mas.PAOTS_ScheduleName && t.PAOTS_ScheduleDate == mas.PAOTS_ScheduleDate && t.MI_Id == mas.MI_Id && t.ASMAY_Id == mas.ASMAY_Id).Count();
                if (schedule == 0)
                {
                    var result = _OralTestScheduleContext.OralTestScheduleDMO.Single(t => t.PAOTS_Id == mas.PAOTS_Id);
                    result.PAOTS_ScheduleName = mas.PAOTS_ScheduleName;
                    result.PAOTS_ScheduleDate = mas.PAOTS_ScheduleDate;
                    result.PAOTS_ScheduleTime = mas.PAOTS_ScheduleTime;



                    result.PAOTS_ScheduleTimeTo = mas.PAOTS_ScheduleTimeTo;
                    result.PAOTS_AM_PM = mas.PAOTS_AM_PM;
                    result.PAOTS_Remarks = mas.PAOTS_Remarks;
                    result.PAOTS_EntryDate = result.PAOTS_EntryDate;  //added by 02/02/2017
                    result.PAOTS_Skills = mas.PAOTS_Skills;
                    result.PAOTS_Superviser = mas.PAOTS_Superviser;
                  
                    result.UpdatedDate = DateTime.Now;
                    _OralTestScheduleContext.Update(result);
                    _OralTestScheduleContext.SaveChanges();
                    mas.PAOTS_Id = MM.PAOTS_Id;
                    var othd = mas.PAOTS_ScheduleTime;

                    int j = 0;
                    if (mas.SelectedStudentData != null)
                    {
                        while (j < mas.SelectedStudentData.Count())
                        {
                            OralTestScheduleStudentInsertDMO MM1 = new OralTestScheduleStudentInsertDMO();
                            MM1.PASR_Id = mas.SelectedStudentData[j].PASR_Id;
                            MM1.PAOTS_Id = mas.PAOTS_Id;
                            //added by 02/02/2017
                            MM1.CreatedDate = DateTime.Now;
                            MM1.UpdatedDate = DateTime.Now;
                            MM1.PAOTSS_StatusFlag = 3;
                            //added on 20/10/17
                            MM1.PAOTSS_Date = mas.PAOTS_ScheduleDate;
                            if (mas.PAOTS_TPFlag == false)
                            {
                                string iDate = mas.PAOTS_ScheduleTime;
                                DateTime oDate = DateTime.ParseExact(iDate, "HH:mm", null);
                                var addmin = mas.PAOTS_TimePeriod * j;
                                var start_tm = oDate.AddMinutes(addmin);
                                string finalstattim = start_tm.ToString("H:mm");
                                var end_tm = start_tm.AddMinutes(mas.PAOTS_TimePeriod);
                                string finalendtim = end_tm.ToString("H:mm");
                                MM1.PAOTSS_Time = finalstattim;
                                MM1.PAOTSS_Time_To = finalendtim;
                            }
                            else
                            {
                                string iDate = mas.PAOTS_ScheduleTime;
                                DateTime oDate = DateTime.ParseExact(iDate, "HH:mm", null);
                                var addhr = mas.PAOTS_TimePeriod * j;
                                var start_tm = oDate.AddHours(addhr);
                                string finalstattim = start_tm.ToString("H:mm");
                                var end_tm = start_tm.AddHours(mas.PAOTS_TimePeriod);
                                string finalendtim = end_tm.ToString("H:mm");
                                //var paotss_tm = oDate.AddHours(addhr);
                                //string finaltim = paotss_tm.ToString("H:mm");
                                MM1.PAOTSS_Time = finalstattim;
                                MM1.PAOTSS_Time_To = finalendtim;
                            }

                            var n = _OralTestScheduleContext.Add(MM1);
                            _OralTestScheduleContext.SaveChanges();

                            if (n != null)
                            {
                                //Email Email = new Email(_context);
                                //string m = Email.sendmail(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PASR_emailId, "ORAL_TEST_SCHEDULE", MM1.PASR_Id);
                                //paotss = MM1.PAOTSS_Id;

                                //SMS sms = new SMS(_context);
                                //string sss = await sms.sendSms(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PASR_MobileNo, "ORAL_TEST_SCHEDULE", MM1.PASR_Id);
                            }
                            j++;
                        }
                    }
                    int k = 0;
                    if (mas.SelectedStudentDataForEdit != null)
                    {
                        while (k < mas.SelectedStudentDataForEdit.Count())
                        {
                            OralTestScheduleStudentInsertDMO MM1 = new OralTestScheduleStudentInsertDMO();
                            MM1.PASR_Id = mas.SelectedStudentDataForEdit[k].PASR_Id;
                            MM1.PAOTS_Id = mas.PAOTS_Id;

                            //Email Email = new Email(_context);
                            //string m = Email.sendmail(mas.SelectedStudentDataForEdit[k].MI_Id, mas.SelectedStudentDataForEdit[k].PASR_emailId, "ORAL_TEST_SCHEDULE", MM1.PASR_Id);

                            //SMS sms = new SMS(_context);
                            //string sss = await sms.sendSms(mas.SelectedStudentDataForEdit[k].MI_Id, mas.SelectedStudentDataForEdit[k].PASR_MobileNo, "ORAL_TEST_SCHEDULE", MM1.PASR_Id);


                            k++;
                        }
                    }
                }
                else
                {
                    mas.returnvalue = "false";
                }


                mas.PAOTS_Id = 0;

            }
            else
            {
                try
                {


                    MM.IVRMSTAUL_Id = Convert.ToInt32(mas.IVRMSTAUL_Id);

                    MM.PAOTS_EntryDate = System.DateTime.Today.Date;

                    var schedule = _OralTestScheduleContext.OralTestScheduleDMO.Where(t => t.PAOTS_ScheduleName == mas.PAOTS_ScheduleName && t.PAOTS_ScheduleDate == mas.PAOTS_ScheduleDate && t.MI_Id == mas.MI_Id && t.ASMAY_Id == mas.ASMAY_Id).Count();
                    if (schedule == 0)
                    {
                        //added by 02/02/2017
                        MM.CreatedDate = DateTime.Now;
                        MM.UpdatedDate = DateTime.Now;
                        _OralTestScheduleContext.Add(MM);
                        _OralTestScheduleContext.SaveChanges();

                        mas.PAOTS_Id = MM.PAOTS_Id;

                        int j = 0;
                        if (mas.SelectedStudentData != null)
                        {
                            while (j < mas.SelectedStudentData.Count())
                            {
                                OralTestScheduleStudentInsertDMO MM1 = new OralTestScheduleStudentInsertDMO();
                                MM1.PASR_Id = mas.SelectedStudentData[j].PASR_Id;
                                MM1.PAOTS_Id = mas.PAOTS_Id;
                                MM1.PAOTSS_StatusFlag = 3;
                                //added by 02/02/2017
                                MM1.CreatedDate = DateTime.Now;
                                MM1.UpdatedDate = DateTime.Now;

                                //added on 20/10/17
                                MM1.PAOTSS_Date = mas.PAOTS_ScheduleDate;
                                if (mas.PAOTS_TPFlag == false)
                                {
                                    string iDate = mas.PAOTS_ScheduleTime;
                                    DateTime oDate = DateTime.ParseExact(iDate, "HH:mm", null);
                                    var addmin = mas.PAOTS_TimePeriod * j;
                                    var start_tm = oDate.AddMinutes(addmin);
                                    string finalstattim = start_tm.ToString("H:mm");
                                    var end_tm = start_tm.AddMinutes(mas.PAOTS_TimePeriod);
                                    string finalendtim = end_tm.ToString("H:mm");
                                    MM1.PAOTSS_Time = finalstattim;
                                    MM1.PAOTSS_Time_To = finalendtim;
                                }
                                else
                                {
                                    string iDate = mas.PAOTS_ScheduleTime;
                                    DateTime oDate = DateTime.ParseExact(iDate, "HH:mm", null);
                                    var addhr = mas.PAOTS_TimePeriod * j;
                                    var start_tm = oDate.AddHours(addhr);
                                    string finalstattim = start_tm.ToString("H:mm");
                                    var end_tm = start_tm.AddHours(mas.PAOTS_TimePeriod);
                                    string finalendtim = end_tm.ToString("H:mm");
                                    //var paotss_tm = oDate.AddHours(addhr);
                                    //string finaltim = paotss_tm.ToString("H:mm");
                                    MM1.PAOTSS_Time = finalstattim;
                                    MM1.PAOTSS_Time_To = finalendtim;
                                }


                                _OralTestScheduleContext.Add(MM1);
                                int n = _OralTestScheduleContext.SaveChanges();
                                if (n > 0)
                                {


                                    string template_type="";
                                    var statustudent  = _OralTestScheduleContext.StudentApplication.Single(d => d.pasr_id == MM1.PASR_Id);

                                    var satus_get = _OralTestScheduleContext.status.Single(d => d.MI_Id == mas.MI_Id && d.PAMST_Id == statustudent.PAMS_Id);
                                    if (satus_get.PAMST_StatusFlag.ToString() == "SELECT")
                                    {
                                        template_type = "SELECT";
                                    }
                                    else if (satus_get.PAMST_StatusFlag.ToString() == "CNF")
                                    {
                                        template_type = "ORAL_TEST_SCHEDULE";
                                    }
                                    else if (satus_get.PAMST_StatusFlag.ToString() == "rej")
                                    {
                                        template_type = "REJ";
                                    }
                                    else if (satus_get.PAMST_StatusFlag.ToString() == "WAITING")
                                    {
                                        template_type = "WAITING";
                                    }
                                    else if (satus_get.PAMST_StatusFlag.ToString() == "INP")
                                    {
                                        template_type = "INP";
                                    }
                                   
                                    Email Email = new Email(_context);
                                    Email.sendmail(statustudent.MI_Id, statustudent.PASR_emailId, template_type, MM1.PASR_Id);

                                    SMS sms = new SMS(_context);
                                    await sms.sendSms(statustudent.MI_Id, statustudent.PASR_MobileNo, template_type, MM1.PASR_Id);


                                    //Email Email = new Email(_context);
                                    //string m = Email.sendmail(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PASR_emailId, "ORAL_TEST_SCHEDULE", MM1.PASR_Id);
                                    //SMS sms = new SMS(_context);
                                    //string sss = await sms.sendSms(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PASR_MobileNo, "ORAL_TEST_SCHEDULE", MM1.PASR_Id);
                                }
                                j++;
                            }
                        }



                    }
                    else
                    {
                        mas.returnvalue = "false";
                    }
                    mas.PAOTS_Id = 0;
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }


            }

            return mas;
        }

        public CommonDTO getdataonsearchfilter(CommonDTO cdto)
        {
            List<StudentApplicationDTO> stulist = new List<StudentApplicationDTO>();
            try
            {
                List<StudentApplicationDTO> result = new List<StudentApplicationDTO>();
                //to get data according to search criteria.
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cdto.status_type = "";
                    //changed by suryan
                    cmd.CommandText = "Get_Student_Status";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Ids", SqlDbType.Int) { Value = cdto.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Ids", SqlDbType.Int) { Value = cdto.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@PAMST_Ids", SqlDbType.Int) { Value = cdto.PAMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.Int) { Value = cdto.IVRM_MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@type_", SqlDbType.VarChar) { Value = cdto.status_type });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new StudentApplicationDTO
                                {
                                    pasR_Id = Convert.ToInt64(dataReader["PASR_Id"]),
                                    PAMST_Id = Convert.ToInt64(dataReader["PAMS_Id"]),
                                    remark = (dataReader["Remark"]).ToString(),
                                    PASR_FirstName = (dataReader["PASR_FirstName"]).ToString(),
                                    PASR_MiddleName = Convert.ToString(dataReader["PASR_MiddleName"]).ToString(),
                                    PASR_LastName = Convert.ToString(dataReader["PASR_LastName"]).ToString(),
                                    ASMCL_Id = Convert.ToInt64(dataReader["ASMCL_Id"]),
                                    className = dataReader["ASMCL_ClassName"].ToString(),
                                    statusName = dataReader["PAMST_Status"].ToString(),
                                    statusFlag = dataReader["PAMST_StatusFlag"].ToString(),
                                    PASR_Sex = dataReader["PASR_Sex"].ToString(),
                                    PASR_RegistrationNo = dataReader["PASR_RegistrationNo"].ToString(),
                                    Repeat_Class_Id = Convert.ToInt64(dataReader["Repeat_Class_Id"])
                                });
                                cdto.getstudentDetails = result.ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return cdto;
        }

    }
}
