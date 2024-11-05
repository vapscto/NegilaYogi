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
using System.Dynamic;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication1.Services
{
    public class WrittenTestScheduleImp : Interfaces.WrittenTestScheduleInterface
    {
        private static ConcurrentDictionary<string, StudentDetailsDTO> _login =
      new ConcurrentDictionary<string, StudentDetailsDTO>();

        private readonly WrittenTestScheduleContext _WrittenTestScheduleContext;
        private readonly DomainModelMsSqlServerContext _context;
        public FeeGroupContext _feecontext;
        public StudentApplicationContext _StudentApplicationContext;
        public WrittenTestScheduleImp(StudentApplicationContext StudentApplicationContext, WrittenTestScheduleContext WrittenTestScheduleContext, DomainModelMsSqlServerContext db, FeeGroupContext feecontext)
        {
            _WrittenTestScheduleContext = WrittenTestScheduleContext;
            _context = db;
            _feecontext = feecontext;
            _StudentApplicationContext = StudentApplicationContext;
        }


        public async Task<WrittenTestScheduleDTO> WrittenTestScheduleData(WrittenTestScheduleDTO mas)
        {
            try
            {

                var Acdemic_preadmission = _StudentApplicationContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == mas.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

            mas.ASMAY_Id = Acdemic_preadmission;


                var user_id= _StudentApplicationContext.Staff_User_Login.Where(t => t.Id == mas.IVRMSTAUL_Id &&  t.MI_Id == mas.MI_Id).Select(d => d.IVRMSTAUL_Id).FirstOrDefault();

                WrittenTestScheduleDMO MM = Mapper.Map<WrittenTestScheduleDMO>(mas);
            long pawtss = 0;
            if (mas.PAWTS_Id != 0)
            {
                var schedule = _WrittenTestScheduleContext.WrittenTestScheduleDMO.Where(t => t.PAWTS_Id != mas.PAWTS_Id && t.PAWTS_ScheduleName == mas.PAWTS_ScheduleName && t.PAWTS_ScheduleDate == mas.PAWTS_ScheduleDate && t.MI_Id == mas.MI_Id && t.ASMAY_Id == mas.ASMAY_Id).Count();
                if (schedule == 0)
                {
                    var result = _WrittenTestScheduleContext.WrittenTestScheduleDMO.Single(t => t.PAWTS_Id == mas.PAWTS_Id);
                    result.PAWTS_ScheduleName = mas.PAWTS_ScheduleName;
                    result.PAWTS_EntryDate = result.PAWTS_EntryDate;
                    result.PAWTS_ScheduleDate = Convert.ToDateTime(mas.PAWTS_ScheduleDate); //mas.PAWTS_ScheduleDate;
                    result.PAWTS_ScheduleTime = mas.PAWTS_ScheduleTime;
                    result.PAWTS_ScheduleTimeTo = mas.PAWTS_ScheduleTimeTo;
                    result.PAWTS_AM_PM = mas.PAWTS_AM_PM;
                    result.PAWTS_Remarks = mas.PAWTS_Remarks;
                    result.PAWTS_Skills = mas.PAWTS_Skills;
                    result.PAWTS_Superviser = mas.PAWTS_Superviser;
                        result.IVRMSTAUL_Id = user_id;
                        //added by 02/02/2017
                        result.UpdatedDate = DateTime.Now;
                    _WrittenTestScheduleContext.Update(result);
                    _WrittenTestScheduleContext.SaveChanges();
                    mas.PAWTS_Id = MM.PAWTS_Id;
                    int j = 0;
                    if (mas.SelectedStudentData != null)
                    {
                        while (j < mas.SelectedStudentData.Count())
                        {
                            WrittenTestScheduleStudentInsertDMO MM1 = new WrittenTestScheduleStudentInsertDMO();
                            MM1.PASR_Id = mas.SelectedStudentData[j].PASR_Id;
                            MM1.PAWTS_Id = mas.PAWTS_Id;
                            //added by 02/02/2017
                            MM1.CreatedDate = DateTime.Now;
                            MM1.UpdatedDate = DateTime.Now;

                            var n = _WrittenTestScheduleContext.Add(MM1);
                            _WrittenTestScheduleContext.SaveChanges();
                            //   int n = _WrittenTestScheduleContext.SaveChanges();
                            if (n != null)
                            {
                                Email Email = new Email(_context);
                                string m = Email.sendmail(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PASR_emailId, "WRITTEN_TEST_SCHEDULE", MM1.PASR_Id);
                                pawtss = MM1.PAWTSS_Id;
                                SMS sms = new SMS(_context);
                                string sss = await sms.sendSms(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PASR_MobileNo, "WRITTEN_TEST_SCHEDULE", MM1.PASR_Id);

                                    SMS smsS = new SMS(_context);
                                    string sssS = await smsS.sendSms(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PASR_MobileNo, "ORAL_TEST_SCHEDULE_1", MM1.PASR_Id);

                                }
                            j++;
                        }

                    }
                    int k = 0;
                    if (mas.SelectedStudentDataForEdit != null)
                    {
                        while (k < mas.SelectedStudentDataForEdit.Count())
                        {
                            WrittenTestScheduleStudentInsertDMO MM1 = new WrittenTestScheduleStudentInsertDMO();
                            MM1.PASR_Id = mas.SelectedStudentDataForEdit[k].PASR_Id;
                            MM1.PAWTS_Id = mas.PAWTS_Id;

                            //Email Email = new Email(_context);
                            //string m = Email.sendmail(mas.SelectedStudentDataForEdit[k].MI_Id, mas.SelectedStudentDataForEdit[k].PASR_emailId, "WRITTEN_TEST_SCHEDULE", MM1.PASR_Id);

                            //SMS sms = new SMS(_context);
                            //string sss = await sms.sendSms(mas.SelectedStudentDataForEdit[k].MI_Id, mas.SelectedStudentDataForEdit[k].PASR_MobileNo, "WRITTEN_TEST_SCHEDULE", MM1.PASR_Id);

                            //    SMS smsS = new SMS(_context);
                            //    string sssS = await smsS.sendSms(mas.SelectedStudentDataForEdit[k].MI_Id, mas.SelectedStudentDataForEdit[k].PASR_MobileNo, "ORAL_TEST_SCHEDULE_1", MM1.PASR_Id);


                                k++;
                        }
                    }

                }
                else
                {
                    mas.returnvalue = "false";
                }



                mas.PAWTS_Id = 0;
            }
            else
            {

                MM.PAWTS_ScheduleDate = Convert.ToDateTime(MM.PAWTS_ScheduleDate); //MM.PAWTS_ScheduleDate; 
                MM.PAWTS_EntryDate = DateTime.Now.ToString();

                var schedule = _WrittenTestScheduleContext.WrittenTestScheduleDMO.Where(t => t.PAWTS_ScheduleName == mas.PAWTS_ScheduleName && t.PAWTS_ScheduleDate == mas.PAWTS_ScheduleDate && t.MI_Id == mas.MI_Id && t.ASMAY_Id == mas.ASMAY_Id).Count();
                if (schedule == 0)
                {
                        //added by 02/02/2017
                        MM.IVRMSTAUL_Id = user_id;
                        MM.CreatedDate = DateTime.Now;
                    MM.UpdatedDate = DateTime.Now;
                    _WrittenTestScheduleContext.Add(MM);
                    _WrittenTestScheduleContext.SaveChanges();
                    mas.PAWTS_Id = MM.PAWTS_Id;

                    int j = 0;

                    if (mas.SelectedStudentData != null)
                    {
                        while (j < mas.SelectedStudentData.Count())
                        {
                            WrittenTestScheduleStudentInsertDMO MM1 = new WrittenTestScheduleStudentInsertDMO();
                            MM1.PASR_Id = mas.SelectedStudentData[j].PASR_Id;
                            MM1.PAWTS_Id = mas.PAWTS_Id;
                             
                                //added by 02/02/2017
                                MM1.CreatedDate = DateTime.Now;
                            MM1.UpdatedDate = DateTime.Now;
                            _WrittenTestScheduleContext.Add(MM1);
                            int n = _WrittenTestScheduleContext.SaveChanges();
                            if (n > 0)
                            {
                                SMS sms = new SMS(_context);

                                string sss = await sms.sendSms(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PASR_MobileNo, "WRITTEN_TEST_SCHEDULE", MM1.PASR_Id);

                                    SMS smsS = new SMS(_context);

                                    string sssS = await smsS.sendSms(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PASR_MobileNo, "ORAL_TEST_SCHEDULE_1", MM1.PASR_Id);
                                    Email Email = new Email(_context);
                                string m = Email.sendmail(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PASR_emailId, "WRITTEN_TEST_SCHEDULE", MM1.PASR_Id);
                            }
                            j++;
                        }
                    }
                }
                else
                {
                    mas.returnvalue = "false";
                }
                mas.PAWTS_Id = 0;
            }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return mas;
        }

        public WrittenTestScheduleDTO GetSelectedRowDetails(int ID)
        {

            WrittenTestScheduleDTO WrittenTestScheduleDTO = new WrittenTestScheduleDTO();
            List<WrittenTestScheduleDMO> lorg = new List<WrittenTestScheduleDMO>();
            lorg = _WrittenTestScheduleContext.WrittenTestScheduleDMO.Where(t => t.PAWTS_Id.Equals(ID)).ToList();
            WrittenTestScheduleDTO.WrittenTestSchedule = lorg.ToArray();

            StudentDetailsDTO StudentDetailsDTO = new StudentDetailsDTO();

            List<WrittenTestScheduleStudentInsertDMO> lorg1 = new List<WrittenTestScheduleStudentInsertDMO>();
            List<StudentApplication> lorg2 = new List<StudentApplication>();

            lorg1 = _WrittenTestScheduleContext.WrittenTestScheduleStudentInsertDMO.Where(t => t.PAWTS_Id.Equals(ID)).ToList();
            int j = 0;
            while (j < lorg1.Count())
            {
                lorg2.AddRange(_WrittenTestScheduleContext.StudentApplication.Where(t => t.pasr_id.Equals(lorg1[j].PASR_Id)).ToList());
                j++;
            }

            WrittenTestScheduleDTO.SelectedStudentDetails = lorg2.ToArray();
            WrittenTestScheduleDTO.WrittenTestSchedule = lorg.ToArray();

            return WrittenTestScheduleDTO;
        }

        public StudentDetailsDTO GetSelectedStudentData(int ID)
        {
            //int ID1 = Convert.ToInt32(ID);
            StudentDetailsDTO StudentDetailsDTO = new StudentDetailsDTO();

            List<StudentApplication> lorg = new List<StudentApplication>();


            lorg = _WrittenTestScheduleContext.StudentApplication.Where(t => t.pasr_id.Equals(ID)).ToList();

            //    if (i == 0)
            //    {

            //    }
            //    else
            //    {
            //        lorg.AddRange(lorg);
            //    }

            //    i = i + 1;

            //}

            StudentDetailsDTO.studentDetails = lorg.ToArray();
            return StudentDetailsDTO;
        }

        public WrittenTestScheduleDTO WrittenTestScheduleDeletesData(int ID)
        {


            WrittenTestScheduleDTO WrittenTestScheduleDTO = new WrittenTestScheduleDTO();

            List<WrittenTestScheduleStudentInsertDMO> masters = new List<WrittenTestScheduleStudentInsertDMO>();
            masters = _WrittenTestScheduleContext.WrittenTestScheduleStudentInsertDMO.Where(t => t.PAWTS_Id.Equals(ID)).ToList();

            if (masters.Any())
            {

                int j = 0;
                while (j < masters.Count())
                {
                    List<WrittenTestScheduleStudentInsertDMO> masters3 = new List<WrittenTestScheduleStudentInsertDMO>();
                    masters3 = _WrittenTestScheduleContext.WrittenTestScheduleStudentInsertDMO.Where(t => t.PAWTSS_Id.Equals(masters[j].PAWTSS_Id)).ToList();
                    _WrittenTestScheduleContext.Remove(masters3.ElementAt(0));
                    _WrittenTestScheduleContext.SaveChanges();

                    j++;
                }


                List<WrittenTestScheduleDMO> masters1 = new List<WrittenTestScheduleDMO>();
                masters1 = _WrittenTestScheduleContext.WrittenTestScheduleDMO.Where(t => t.PAWTS_Id.Equals(ID)).ToList();



                if (masters1.Any())
                {
                    _WrittenTestScheduleContext.Remove(masters1.ElementAt(0));
                    _WrittenTestScheduleContext.SaveChanges();

                }
                else
                {

                }
            }
            else
            {
                List<WrittenTestScheduleDMO> masters1 = new List<WrittenTestScheduleDMO>();
                masters1 = _WrittenTestScheduleContext.WrittenTestScheduleDMO.Where(t => t.PAWTS_Id.Equals(ID)).ToList();



                if (masters1.Any())
                {
                    _WrittenTestScheduleContext.Remove(masters1.ElementAt(0));
                    _WrittenTestScheduleContext.SaveChanges();

                }
                else
                {

                }
            }


            return WrittenTestScheduleDTO;
        }

        //public WrittenTestScheduleStudentInsertDMO WrittenTestScheduleDeletesStudentData(WrittenTestScheduleStudentInsertDMO WrittenTestScheduleStudentInsertDMO)
        //{

        //    List<WrittenTestScheduleStudentInsertDMO> masters = new List<WrittenTestScheduleStudentInsertDMO>();
        //    masters = _WrittenTestScheduleContext.WrittenTestScheduleStudentInsertDMO.Where(t => t.PASR_Id.Equals(WrittenTestScheduleStudentInsertDMO.PASR_Id) && t.PAWTS_Id.Equals(WrittenTestScheduleStudentInsertDMO.PAWTS_Id)).ToList();
        //    if (masters.Any())
        //    {              
        //            _WrittenTestScheduleContext.Remove(masters.ElementAt(0));
        //            _WrittenTestScheduleContext.SaveChanges();
        //    }
        //    else
        //    {

        //    }
        //    return WrittenTestScheduleStudentInsertDMO;
        //}


        public WrittenTestScheduleDTO WrittenTestScheduleDeletesStudentData(WrittenTestScheduleDTO WrittenTestScheduleDTO)
        {
            // WrittenTestScheduleDTO WrittenTestScheduleDTO = new WrittenTestScheduleDTO();
            List<WrittenTestScheduleStudentInsertDMO> masters = new List<WrittenTestScheduleStudentInsertDMO>();
            masters = _WrittenTestScheduleContext.WrittenTestScheduleStudentInsertDMO.Where(t => t.PASR_Id.Equals(WrittenTestScheduleDTO.PASR_Id) && t.PAWTS_Id.Equals(WrittenTestScheduleDTO.PAWTS_Id)).ToList();
            if (masters.Any())
            {
                _WrittenTestScheduleContext.Remove(masters.ElementAt(0));
                _WrittenTestScheduleContext.SaveChanges();
            }
            else
            {

            }
            return WrittenTestScheduleDTO;
        }
        public StudentDetailsDTO GetWrittenTestScheduleData(StudentDetailsDTO StudentDetailsDTO)//int IVRMM_Id
        {
            var Acdemic_preadmission = _StudentApplicationContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == StudentDetailsDTO.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();
            StudentDetailsDTO.ASMAY_Id = Acdemic_preadmission;

            Array[] showdata1 = new Array[1];
            List<StudentApplication> Allname1 = new List<StudentApplication>();
            List<StudentApplication> lorg5 = new List<StudentApplication>();
           
            List<WrittenTestScheduleStudentInsertDMO> lorg1 = new List<WrittenTestScheduleStudentInsertDMO>();
            lorg1 = _WrittenTestScheduleContext.WrittenTestScheduleStudentInsertDMO.ToList();

            List<long> Allname2 = new List<long>();
            List<long> Allname3 = new List<long>();
            List<StudentApplication> Allname5 = new List<StudentApplication>();
            if (StudentDetailsDTO.ASMCL_ID==0)
            {
                lorg5 = _WrittenTestScheduleContext.StudentApplication.Where(t => t.MI_Id == StudentDetailsDTO.MI_Id && t.ASMAY_Id == StudentDetailsDTO.ASMAY_Id && t.PASR_Adm_Confirm_Flag == false).ToList();

               
                Allname5 = _WrittenTestScheduleContext.StudentApplication.Where(t => t.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id) && t.MI_Id.Equals(StudentDetailsDTO.MI_Id) && t.PASR_Adm_Confirm_Flag == false).ToList();
            }
            else
            {
                lorg5 = _WrittenTestScheduleContext.StudentApplication.Where(t => t.MI_Id == StudentDetailsDTO.MI_Id && t.ASMAY_Id == StudentDetailsDTO.ASMAY_Id && t.PASR_Adm_Confirm_Flag == false && t.ASMCL_Id== StudentDetailsDTO.ASMCL_ID).ToList();

                
                Allname5 = _WrittenTestScheduleContext.StudentApplication.Where(t => t.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id) && t.MI_Id.Equals(StudentDetailsDTO.MI_Id) && t.PASR_Adm_Confirm_Flag == false && t.ASMCL_Id == StudentDetailsDTO.ASMCL_ID).ToList();
            }

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

                        if (StudentDetailsDTO.payementcheck >= 1)
                        {
                            Allname3.Add(a.pasr_id);
                        }
                    }
                    foreach (var a in lorg1)
                    {
                        Allname2.Add(a.PASR_Id);
                    }

                    if(StudentDetailsDTO.ASMCL_ID==0)
                    {
                        Allname1 = _WrittenTestScheduleContext.StudentApplication.Where(t => t.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id) && t.MI_Id.Equals(StudentDetailsDTO.MI_Id) && t.PASR_Adm_Confirm_Flag == false && !Allname2.Contains(t.pasr_id) && Allname3.Contains(t.pasr_id)).ToList();
                    }
                    else
                    {
                        Allname1 = _WrittenTestScheduleContext.StudentApplication.Where(t => t.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id) && t.MI_Id.Equals(StudentDetailsDTO.MI_Id) && t.PASR_Adm_Confirm_Flag == false && t.ASMCL_Id== StudentDetailsDTO.ASMCL_ID && !Allname2.Contains(t.pasr_id) && Allname3.Contains(t.pasr_id)).ToList();
                    }
                    StudentDetailsDTO.studentDetails = Allname1.ToArray();
                }
            }
            else
            {
                foreach (var a in lorg1)
                {
                    Allname2.Add(a.PASR_Id);
                }
                if(StudentDetailsDTO.ASMCL_ID==0)
                {
                    Allname1 = _WrittenTestScheduleContext.StudentApplication.Where(t => t.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id) && t.MI_Id.Equals(StudentDetailsDTO.MI_Id) && t.PASR_Adm_Confirm_Flag == false && !Allname2.Contains(t.pasr_id)).ToList();
                }
                else
                {
                    Allname1 = _WrittenTestScheduleContext.StudentApplication.Where(t => t.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id) && t.MI_Id.Equals(StudentDetailsDTO.MI_Id) && t.PASR_Adm_Confirm_Flag == false && t.ASMCL_Id== StudentDetailsDTO.ASMCL_ID && !Allname2.Contains(t.pasr_id)).ToList();
                }
                StudentDetailsDTO.studentDetails = Allname1.ToArray();
            }
            Array[] showdata = new Array[50];
            List<WrittenTestScheduleDMO> Allname = new List<WrittenTestScheduleDMO>();
            Allname = _WrittenTestScheduleContext.WrittenTestScheduleDMO.Where(t => t.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id) && t.MI_Id.Equals(StudentDetailsDTO.MI_Id)).ToList();
            StudentDetailsDTO.WrittenTestSchedule = Allname.ToArray();

            List<AdmissionClass> allclass = new List<AdmissionClass>();

            allclass = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == StudentDetailsDTO.MI_Id && t.ASMCL_ActiveFlag == true && t.ASMCL_PreadmFlag == 1).ToList();
            StudentDetailsDTO.admissioncatdrp = allclass.ToArray();

            allclass = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == StudentDetailsDTO.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
            StudentDetailsDTO.admissioncatdrpall = allclass.ToArray();

            using (var cmd = _StudentApplicationContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "writtentestschedulecount";
                cmd.CommandType = CommandType.StoredProcedure;
              
                cmd.Parameters.Add(new SqlParameter("@year",
            SqlDbType.BigInt)
                {
                    Value = StudentDetailsDTO.ASMAY_Id
                });
                cmd.Parameters.Add(new SqlParameter("@miid",
         SqlDbType.BigInt)
                {
                    Value = StudentDetailsDTO.MI_Id
                });
               

                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                var retObject = new List<dynamic>();
                //  var data = cmd.ExecuteNonQuery();

                try
                {
                    //   var data = cmd.ExecuteNonQuery();

                    using (var dataReader = cmd.ExecuteReader())
                    {
                        while ( dataReader.Read())
                        {
                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                            {
                                var datatype = dataReader.GetFieldType(iFiled);
                                if (datatype.Name == "DateTime")
                                {
                                    var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                    dataRow.Add(
                                    dataReader.GetName(iFiled),
                                    dataReader.IsDBNull(iFiled) ? "" : dateval  // use null instead of {}
                                );
                                }
                                else
                                {
                                    dataRow.Add(
                                   dataReader.GetName(iFiled),
                                   dataReader.IsDBNull(iFiled) ? "" : dataReader[iFiled] // use null instead of {}
                               );
                                }
                            }

                            retObject.Add((ExpandoObject)dataRow);
                        }
                    }
                    StudentDetailsDTO.schedulecount = retObject.ToArray();
                }
                catch (Exception ex)
                {

                }
            }

            return StudentDetailsDTO;
        }

    }
}
