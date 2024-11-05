using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
namespace WebApplication1.Services
{
    public class PointsImpl : Interfaces.PointsInterface
    {
        public DomainModelMsSqlServerContext _db;
        public StudentApplicationContext _StudentApplicationContext;
        public ScheduleReportContext _SReportContext;
        public FeeGroupContext _feecontext;
        public PointsImpl(ScheduleReportContext DomainModelContext, DomainModelMsSqlServerContext db, StudentApplicationContext StudentApplicationContext, FeeGroupContext feecontext)
        {
            _StudentApplicationContext = StudentApplicationContext;
            _SReportContext = DomainModelContext;
            _feecontext = feecontext;
            _db = db;
        }

        public PointsDTO getdetails(PointsDTO data)
        {
            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _SReportContext.AcademicYear.Where(t => t.MI_Id == data.mi_id && t.Is_Active == true).OrderByDescending(f => f.ASMAY_Order).ToList();
                data.yeardropDown = year.ToArray();

                List<School_M_Class> classname = new List<School_M_Class>();
                classname = _SReportContext.admissioncls.Where(t => t.MI_Id == data.mi_id && t.ASMCL_ActiveFlag == true).OrderBy(f => f.ASMCL_Order).ToList();
                data.fillclass = classname.ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public PointsDTO Getreportdetails(PointsDTO data)
        {
            try
            {

                var Acdemic_preadmission = _StudentApplicationContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true 
                && t.MI_Id == data.mi_id).Select(d => d.ASMAY_Id).FirstOrDefault();

                data.ASMAY_Id = Acdemic_preadmission;

                if (data.configurationsettings == 1)
                {
                    List<PointsDTO> allRegStudentmain = new List<PointsDTO>();
                    List<PointsDTO> allRegStudent = new List<PointsDTO>();
                    allRegStudent = (from a in _StudentApplicationContext.Enq
                                     from b in _StudentApplicationContext.PointsDMO
                                     from c in _StudentApplicationContext.caste
                                     where (a.pasr_id == b.PASR_Id && a.Caste_Id == c.IMC_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id)
                                     select new PointsDTO
                                     {
                                         PASR_Id = a.pasr_id,
                                         PASR_FirstName = a.PASR_FirstName,
                                         PASR_MiddleName = a.PASR_MiddleName,
                                         PASR_LastName = a.PASR_LastName,
                                         PASR_Age = a.PASR_Age,
                                         PASR_FatherEducation = a.PASR_FatherEducation,
                                         PASR_MotherEducation = a.PASR_MotherEducation,
                                         PASR_TotalIncome = a.PASR_FatherIncome+a.PASR_MotherIncome,
                                         PASR_ConArea = a.PASR_ConArea,
                                         Caste_Id = a.Caste_Id,
                                         Caste_Name = c.IMC_CasteName,
                                         PASAP_AGE = b.PASAP_AGE,
                                         PASAP_INCOME = b.PASAP_INCOME,
                                         PASAP_CASTE = b.PASAP_CASTE,
                                         PASAP_ADRESS = b.PASAP_ADRESS,
                                         PASAP_QA = b.PASAP_QA,
                                         PASAP_TOTAL = b.PASAP_TOTAL,
                                         PASRAPS_ID = a.PASRAPS_ID
                                     }
                      ).ToList();

                    for (int i = 0; i < allRegStudent.Count; i++)
                    {
                        //payment checking 
                        data.payementcheck = _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO.Where(t => t.FYPPA_Type == "R" && t.PASA_Id == allRegStudent[i].PASR_Id).Count();
                        if (data.payementcheck > 0)
                        {
                            allRegStudentmain.Add(allRegStudent[i]);
                            data.studentDetails = allRegStudentmain.ToArray();
                        }
                    }
                }
                else
                {
                    data.studentDetails = (from a in _StudentApplicationContext.Enq
                                           from b in _StudentApplicationContext.PointsDMO
                                           from c in _StudentApplicationContext.caste
                                           where (a.pasr_id == b.PASR_Id && a.Caste_Id == c.IMC_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id)
                                           select new PointsDTO
                                           {
                                               PASR_Id = a.pasr_id,
                                               PASR_FirstName = a.PASR_FirstName,
                                               PASR_MiddleName = a.PASR_MiddleName,
                                               PASR_LastName = a.PASR_LastName,
                                               PASR_Age = a.PASR_Age,
                                               PASR_FatherEducation = a.PASR_FatherEducation,
                                               PASR_MotherEducation = a.PASR_MotherEducation,
                                               PASR_TotalIncome = a.PASR_FatherIncome + a.PASR_MotherIncome,
                                               PASR_ConArea = a.PASR_ConArea,
                                               Caste_Id = a.Caste_Id,
                                               Caste_Name = c.IMC_CasteName,
                                               PASAP_AGE = b.PASAP_AGE,
                                               PASAP_INCOME = b.PASAP_INCOME,
                                               PASAP_CASTE = b.PASAP_CASTE,
                                               PASAP_ADRESS = b.PASAP_ADRESS,
                                               PASAP_QA = b.PASAP_QA,
                                               PASAP_TOTAL = b.PASAP_TOTAL,
                                               PASRAPS_ID = a.PASRAPS_ID
                                           }
                    ).ToList().ToArray();
                }



            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return data;

        }
        public async Task<PointsDTO> savedata(PointsDTO stu)
        {
            PointsDMO pge = Mapper.Map<PointsDMO>(stu);
            var Acdemic_preadmission = _StudentApplicationContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true
                && t.MI_Id == stu.mi_id).Select(d => d.ASMAY_Id).FirstOrDefault();

            stu.ASMAY_Id = Acdemic_preadmission;
            try
            {
                for (int i = 0; i < stu.TempararyArrayList.Length; i++)
                {
                    var result1 = _StudentApplicationContext.Enq.Single(t => t.pasr_id == stu.TempararyArrayList[i].PASR_Id);
                    result1.PASRAPS_ID = stu.TempararyArrayList[i].PASRAPS_ID;
                    result1.UpdatedDate = DateTime.Now;
                    string email = result1.PASR_emailId;
                    long mobile = result1.PASR_MobileNo;
                    _StudentApplicationContext.Update(result1);
                    var contactExists1 = _StudentApplicationContext.SaveChanges();
                    if (contactExists1 == 1)
                    {
                        stu.returnval = true;
                        var result = _StudentApplicationContext.PointsDMO.Single(t => t.PASR_Id == stu.TempararyArrayList[i].PASR_Id);
                        result.PASAP_QA = stu.TempararyArrayList[i].PASAP_QA;
                        result.PASAP_ADRESS = stu.TempararyArrayList[i].PASAP_ADRESS;
                        result.PASAP_TOTAL = stu.TempararyArrayList[i].PASAP_TOTAL;
                        result.CreatedDate = DateTime.Now;
                        result.UpdatedDate = DateTime.Now;
                        _StudentApplicationContext.Update(result);
                        var contactExists = _StudentApplicationContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            stu.returnval = true;
                            Email Email = new Email(_db);
                            Email.sendmail(stu.mi_id, email, "STATUS", stu.TempararyArrayList[i].PASR_Id);
                            SMS sms = new SMS(_db);
                            await sms.sendSms(stu.mi_id, mobile, "STATUS", stu.TempararyArrayList[i].PASR_Id);
                        }
                        else
                        {
                            stu.returnval = false;
                        }
                    }
                    else
                    {
                        stu.returnval = false;
                    }
                    StudentStatusHistory ssh = new StudentStatusHistory();
                    ssh.PASR_Id = stu.TempararyArrayList[i].PASR_Id;
                    ssh.PASSH_Status = Convert.ToString(stu.TempararyArrayList[i].PASRAPS_ID);
                    ssh.PASSH_Date = DateTime.Now;
                    //added by 02/02/2017
                    ssh.CreatedDate = DateTime.Now;
                    ssh.UpdatedDate = DateTime.Now;
                    _db.studentstatushistory.Add(ssh);
                    _db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return stu;
        }


    }
}
