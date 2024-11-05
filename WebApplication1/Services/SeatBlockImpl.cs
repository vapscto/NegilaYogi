using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;

namespace WebApplication1.Services
{
    public class SeatBlockImpl : Interfaces.SeatBlockInterface
    {

        private static ConcurrentDictionary<string, Preadmission_SeatBlocked_StudentDTO> _login =
           new ConcurrentDictionary<string, Preadmission_SeatBlocked_StudentDTO>();

        public DomainModelMsSqlServerContext _Context;

        public SeatBlockImpl(DomainModelMsSqlServerContext DomainModelContext)
        {
            _Context = DomainModelContext;
        }

        public async Task<Preadmission_SeatBlocked_StudentDTO> saveSeatBlock(Preadmission_SeatBlocked_StudentDTO SeatBlockDTO)
        {
            List<Preadmission_SeatBlocked_Student> allSeatBlockList = new List<Preadmission_SeatBlocked_Student>();
            try
            {
                Preadmission_SeatBlocked_Student SeatB = Mapper.Map<Preadmission_SeatBlocked_Student>(SeatBlockDTO);

                // enq.MO_Id = 12;
                if (SeatB.PASBS_Id > 0)
                {
                    //var result = _Context.Preadmission_SeatBlocked_Student.Single(t => t.PASBS_Id == SeatB.PASBS_Id);

                    //result.PASBS_Id = SeatB.PASBS_Id;
                    //result.PASR_Id = SeatB.PASR_Id;
                    //result.MI_Id = SeatB.MI_Id;
                    //result.PASBS_Date = SeatB.PASBS_Date;
                    //result.IVRMSTAUL_Id = SeatB.IVRMSTAUL_Id;

                    //_Context.Update(result);
                }
                else
                {

                    //added by 02/02/2017
                    SeatB.CreatedDate = DateTime.Now;
                    SeatB.UpdatedDate = DateTime.Now;
                    
                    _Context.Add(SeatB);

                }
                var flag = _Context.SaveChanges();
                if (flag == 1)
                {
                    SeatBlockDTO.returnVal = true;
                    var email = _Context.StudentApplication.Where(s => s.pasr_id == SeatBlockDTO.PASR_Id).ToList();
                    Email Email = new Email(_Context);
                    Email.sendmail(email.FirstOrDefault().MI_Id, email.FirstOrDefault().PASR_emailId, "SEAT_BLOCK", email.FirstOrDefault().pasr_id);
                    SMS sms = new SMS(_Context);
                    await sms.sendSms(email.FirstOrDefault().MI_Id, email.FirstOrDefault().PASR_MobileNo, "SEAT_BLOCK", email.FirstOrDefault().pasr_id);
                }
                else
                {
                    SeatBlockDTO.returnVal = false;
                }


                SeatBlockDTO.SeatBlockedList = (from sp in _Context.Preadmission_SeatBlocked_Student
                                                from mi in _Context.Institute
                                                from sa in _Context.StudentApplication
                                                from sul in _Context.Staff_User_Login
                                                from y in _Context.AcademicYear
                                                where (sp.MI_Id == mi.MI_Id && sp.PASR_Id == sa.pasr_id && sp.IVRMSTAUL_Id == sul.IVRMSTAUL_Id && sp.ASMAY_Id == y.ASMAY_Id && sp.MI_Id==SeatB.MI_Id)
                                                select new Preadmission_SeatBlocked_StudentDTO
                                                {
                                                    InstituteName = mi.MI_Name,
                                                    studentName = sa.PASR_FirstName,
                                                    staff = sul.IVRMSTAUL_UserName,
                                                    Year = y.ASMAY_Year,
                                                    PASBS_Id = sp.PASBS_Id,
                                                    PASBS_Date = sp.PASBS_Date
                                                }).ToArray();
                if (SeatBlockDTO.SeatBlockedList.Length > 0)
                {
                    SeatBlockDTO.count = SeatBlockDTO.SeatBlockedList.Length;
                }
                else
                {
                    SeatBlockDTO.count = 0;
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                SeatBlockDTO.SeatBlockedList = (from sp in _Context.Preadmission_SeatBlocked_Student
                                       from mi in _Context.Institute
                                       from sa in _Context.StudentApplication
                                       from sul in _Context.Staff_User_Login
                                       from y in _Context.AcademicYear
                                       where (sp.MI_Id == mi.MI_Id && sp.PASR_Id == sa.pasr_id && sp.IVRMSTAUL_Id == sul.IVRMSTAUL_Id && sp.ASMAY_Id == y.ASMAY_Id && sp.MI_Id== SeatBlockDTO.MI_Id)
                                       select new Preadmission_SeatBlocked_StudentDTO
                                       {
                                           InstituteName = mi.MI_Name,
                                           studentName = sa.PASR_FirstName,
                                           staff = sul.IVRMSTAUL_UserName,
                                           Year = y.ASMAY_Year,
                                           PASBS_Id = sp.PASBS_Id,
                                           PASBS_Date = sp.PASBS_Date
                                       }).ToArray();
            }

            return SeatBlockDTO;
        }
        //public EnqDTO countrydrp(EnqDTO stu)
        public Preadmission_SeatBlocked_StudentDTO AllDropdownList(int MI_Id)
        {
            Preadmission_SeatBlocked_StudentDTO stu = new Preadmission_SeatBlocked_StudentDTO();
            try
            {
                var mo_id = _Context.Institution.Where(d => d.MI_Id == MI_Id).ToList();

                List<Institution> allInstitution = new List<Institution>();
                allInstitution = _Context.Institution.Where(d=>d.MI_Id == MI_Id && d.MI_ActiveFlag==1).ToList();
                stu.InstuteDropDown = allInstitution.ToArray();

                List<MasterAcademic> allAcademicYear = new List<MasterAcademic>();
                allAcademicYear = _Context.AcademicYear.Where(d=>d.MI_Id==MI_Id && d.Is_Active==true).ToList();
                stu.YearDropdown = allAcademicYear.ToArray();

                List<Staff_User_Login> allStaff_User_Login = new List<Staff_User_Login>();
                allStaff_User_Login = _Context.Staff_User_Login.Where(d=>d.MI_Id==MI_Id && d.IVRMSTAUL_ActiveFlag==1).ToList();
                stu.staffUserDropdown = allStaff_User_Login.ToArray();

                List<StudentApplication> allRegStudent = new List<StudentApplication>();
                allRegStudent = _Context.StudentApplication.Where(d=>d.MI_Id==MI_Id && d.PASR_ActiveFlag==1).ToList();
                stu.RegstudentdropDown = allRegStudent.ToArray();

                stu.SeatBlockedList = (from sp in _Context.Preadmission_SeatBlocked_Student
                            from mi in _Context.Institute
                            from sa in _Context.StudentApplication
                            from sul in _Context.Staff_User_Login
                            from y in _Context.AcademicYear
                            where (sp.MI_Id == mi.MI_Id && sp.PASR_Id == sa.pasr_id && sp.IVRMSTAUL_Id == sul.IVRMSTAUL_Id && sp.ASMAY_Id ==y.ASMAY_Id && sp.MI_Id==MI_Id)
                            select new Preadmission_SeatBlocked_StudentDTO
                            {
                                InstituteName = mi.MI_Name,
                                studentName = sa.PASR_FirstName,
                                staff = sul.IVRMSTAUL_UserName,
                                Year = y.ASMAY_Year,
                                PASBS_Id = sp.PASBS_Id,
                                PASBS_Date = sp.PASBS_Date
                            }).ToArray();
                if(stu.SeatBlockedList.Length>0)
                {
                    stu.count = stu.SeatBlockedList.Length;
                }
                else
                {
                    stu.count = 0;
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return stu;
        }
       
        public async Task<Preadmission_SeatBlocked_StudentDTO> deleterec(int id)
        {
            int count = 0;
            Preadmission_SeatBlocked_StudentDTO stu = new Preadmission_SeatBlocked_StudentDTO();

            List<Preadmission_SeatBlocked_Student> lorg = new List<Preadmission_SeatBlocked_Student>();

            try
            {
                lorg = _Context.Preadmission_SeatBlocked_Student.Where(t => t.PASBS_Id.Equals(id)).ToList();

                if (lorg.Any())
                {
                    _Context.Remove(lorg.ElementAt(0));

                   count = _Context.SaveChanges();
                }

                if (count == 1)
                {
                    stu.returnVal = true;
                    var email = _Context.StudentApplication.Where(s => s.pasr_id == lorg.FirstOrDefault().PASR_Id).ToList();
                    Email Email = new Email(_Context);
                    Email.sendmail(email.FirstOrDefault().MI_Id, email.FirstOrDefault().PASR_emailId, "SEAT_UN_BLOCK", email.FirstOrDefault().pasr_id);
                    SMS sms = new SMS(_Context);
                    await sms.sendSms(email.FirstOrDefault().MI_Id, email.FirstOrDefault().PASR_MobileNo, "SEAT_UN_BLOCK", email.FirstOrDefault().pasr_id);
                }
                else
                {
                    stu.returnVal = false;
                }

                stu.SeatBlockedList = (from sp in _Context.Preadmission_SeatBlocked_Student
                                       from mi in _Context.Institute
                                       from sa in _Context.StudentApplication
                                       from sul in _Context.Staff_User_Login
                                       from y in _Context.AcademicYear
                                       where (sp.MI_Id == mi.MI_Id && sp.PASR_Id == sa.pasr_id && sp.IVRMSTAUL_Id == sul.IVRMSTAUL_Id && sp.ASMAY_Id == y.ASMAY_Id && sp.MI_Id==lorg.FirstOrDefault().MI_Id)
                                       select new Preadmission_SeatBlocked_StudentDTO
                                       {
                                           InstituteName = mi.MI_Name,
                                           studentName = sa.PASR_FirstName,
                                           staff = sul.IVRMSTAUL_UserName,
                                           Year = y.ASMAY_Year,
                                           PASBS_Id = sp.PASBS_Id,
                                           PASBS_Date = sp.PASBS_Date
                                       }).ToArray();
                if(stu.SeatBlockedList.Length>0)
                {
                    stu.count = stu.SeatBlockedList.Length;
                }
                else
                {
                    stu.count = 0;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                stu.message = "Sorry...You Can't delete this record because it is used in other operation.";
            }

            return stu;
        }

        public Preadmission_SeatBlocked_StudentDTO getdetails(int id)
        {
            Preadmission_SeatBlocked_StudentDTO org = new Preadmission_SeatBlocked_StudentDTO();
            // Institution_MobileDTO[] mobDTO = 0;
            try
            {
                List<Preadmission_SeatBlocked_Student> lorg = new List<Preadmission_SeatBlocked_Student>();

                lorg = _Context.Preadmission_SeatBlocked_Student.AsNoTracking().Where(t => t.PASBS_Id.Equals(id)).ToList();
                org.SeatBlockedList = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return org;
        }
        public Preadmission_SeatBlocked_StudentDTO getSeatConfirmedStud(Preadmission_SeatBlocked_StudentDTO stud)
        {
            Preadmission_SeatBlocked_StudentDTO org = new Preadmission_SeatBlocked_StudentDTO();
            // Institution_MobileDTO[] mobDTO = 0;
            try
            {
                var status = _Context.AdmissionStatus.Where(s => s.PAMST_StatusFlag == "CNF").ToList();
                var seatBlockedStud = _Context.Preadmission_SeatBlocked_Student.Select(d => d.PASR_Id).ToList();
                var seatBookedStudList = (from m in _Context.StudentApplication
                                          where (m.ASMAY_Id == stud.ASMAY_Id && m.MI_Id == stud.MI_Id && m.PAMS_Id == status.FirstOrDefault().PAMST_Id && !seatBlockedStud.Contains(m.pasr_id))
                                          select m).ToList();
                org.RegstudentdropDown = seatBookedStudList.ToArray();


                //List < StudentApplication > lorg = new List<StudentApplication>();
                //lorg = _Context.StudentApplication.AsNoTracking().Where(t =>t.ASMAY_Id==stud.ASMAY_Id && t.MI_Id==stud.MI_Id && t.PAMS_Id==status.FirstOrDefault().PAMS_Id).ToList();
                //org.RegstudentdropDown = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return org;
        }
    }
}
