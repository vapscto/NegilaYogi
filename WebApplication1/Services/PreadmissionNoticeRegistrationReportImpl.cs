using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public class PreadmissionNoticeRegistrationReportImpl : Interfaces.PreadmissionNoticeRegistrationReportInterface
    {
        private readonly DomainModelMsSqlServerContext _db;
        public PreadmissionNoticeRegistrationReportImpl(DomainModelMsSqlServerContext db)
        {
            _db = db;
        }
        public PreadmissionNoticeRegistrationReportDTO get_intial_data(PreadmissionNoticeRegistrationReportDTO data)
        {
            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.yearlist = year.ToArray();

                data.alldata1 = (from a in _db.PreadmissionSchoolRegistrationAdmissionNoticeDMO
                                 from b in _db.PreadmissionSchoolRegistrationAdmNoticeStudentsDMO
                                 from c in _db.StudentApplication
                                 where (a.PASRAN_ID == b.PASRAN_ID && b.PASR_ID == c.pasr_id)
                                 select new PreadmissionNoticeRegistrationReportDTO
                                 {
                                   
                                     PASRAN_NAME = a.PASRAN_NAME,
                                     PASRAN_FEEAMOUNT = a.PASRAN_FEEAMOUNT,                                    
                                     PASRAN_ID = a.PASRAN_ID,                                   
                                 }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public PreadmissionNoticeRegistrationReportDTO getprospectusno(PreadmissionNoticeRegistrationReportDTO data)
        {
            try
            {
                data.prospectuslist = (from a in _db.AcademicYear
                                       from b in _db.StudentApplication
                                       where (a.ASMAY_Id == b.ASMAY_Id && a.MI_Id == b.MI_Id && b.ASMAY_Id == data.ASMAY_Id)
                                       select new PreadmissionNoticeRegistrationReportDTO
                                       {
                                          pasr_id = b.pasr_id,
                                           PASR_RegistrationNo = b.PASR_RegistrationNo,
                                           ASMAY_Year = a.ASMAY_Year
                                       }).Distinct().ToArray();



            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public PreadmissionNoticeRegistrationReportDTO getstudentlist(PreadmissionNoticeRegistrationReportDTO data)
        {
            try
            {

                var pk = _db.StudentApplication.Where(t => t.PASR_RegistrationNo == data.PASR_RegistrationNo).ToList();

                data.pkid = pk[0].pasr_id;

               var student = (from a in _db.StudentApplication
                               from b in _db.castecategory
                               from c in _db.AcademicYear
                              
                               where a.pasr_id == data.pkid && b.IMCC_Id == a.CasteCategory_Id && a.MI_Id==data.MI_Id && a.ASMAY_Id==data.ASMAY_Id && a.ASMAY_Id==c.ASMAY_Id 
                               select new PreadmissionNoticeRegistrationReportDTO
                               {
                                   PASR_FirstName = a.PASR_FirstName,
                                   PASR_DOB = a.PASR_DOB,
                                   PASR_Age = a.PASR_Age,
                                   CasteCategory_Id = a.CasteCategory_Id,
                                   IMCC_CategoryName = b.IMCC_CategoryName,
                                   PASR_PerState = a.PASR_PerState,
                                   PASR_PerCity = a.PASR_PerCity,
                                   PASR_PerCountry = a.PASR_PerCountry,
                                  IVRMMC_Id = Convert.ToInt64(a.PASR_PerCountry),
                                   IVRMMS_Id = Convert.ToInt64(a.PASR_PerState),
                                   pasr_id = a.pasr_id
                               }).Distinct().ToList();
                data.studentDetails = student.ToArray();

                List<StudentApplication> studentdet = new List<StudentApplication>();
                studentdet = (from a in _db.StudentApplication
                              where (a.pasr_id == data.pkid
                           )
                              select new StudentApplication
                              {
                                  PASR_PerCountry = a.PASR_PerCountry,
                                  PASR_PerState = a.PASR_PerState
                              }
       ).ToList();

                data.saveddata = (from a in _db.StudentApplication

                                  where (a.pasr_id == data.pkid)
                                  select a).ToArray();

                data.PASR_PerCountry = studentdet.FirstOrDefault().PASR_PerCountry;
                data.PASR_PerState = studentdet.FirstOrDefault().PASR_PerState;
                data.countryDrpDown = _db.country.ToArray();
                data.statedropdown = _db.State.Where(t => t.IVRMMC_Id == Convert.ToInt32(student[0].PASR_PerCountry)).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public PreadmissionNoticeRegistrationReportDTO addtolist(PreadmissionNoticeRegistrationReportDTO data)
        {
            try
            {
                data.reportdetails = (from a in _db.StudentApplication
                                      from b in _db.PreadmissionSchoolRegistrationAdmNoticeStudentsDMO
                                      from c in _db.AcademicYear
                                      from d in _db.State
                                      from e in _db.country
                                      from f in _db.castecategory
                                      where (a.pasr_id == b.PASR_ID && a.pasr_id == data.pasr_id && a.ASMAY_Id == c.ASMAY_Id && a.pasr_id == d.IVRMMS_Id && a.pasr_id == e.IVRMMC_Id && a.CasteCategory_Id == f.IMCC_Id)

                                      select new PreadmissionNoticeRegistrationReportDTO
                                      {
                                          PASR_RegistrationNo = a.PASR_RegistrationNo,
                                          ASMAY_Id = a.ASMAY_Id,
                                          PASR_FirstName = a.PASR_FirstName,
                                          PASR_Age = a.PASR_Age,
                                          PASR_DOB = a.PASR_DOB,
                                          PASR_PerCity = a.PASR_PerCity,
                                          //PASR_PerState = a.PASR_PerState,
                                          // PASR_PerCountry = a.PASR_PerCountry,
                                          CasteCategory_Id = a.CasteCategory_Id,
                                          PASRANS_REMARKS = b.PASRANS_REMARKS,
                                          PASRANS_ADMDATE = b.PASRANS_ADMDATE,
                                          ASMAY_Year = c.ASMAY_Year,
                                          IVRMMS_Id = d.IVRMMS_Id,
                                          PASR_PerState = d.IVRMMS_Name,
                                          IVRMMC_Id = e.IVRMMC_Id,
                                          PASR_PerCountry = e.IVRMMC_CountryName,
                                          IMCC_CategoryName = f.IMCC_CategoryName
                                      }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public PreadmissionNoticeRegistrationReportDTO Savedata(PreadmissionNoticeRegistrationReportDTO data)
        {
            try
            {
                if (data.PASRAN_ID > 0)
                {
                    var res = _db.PreadmissionSchoolRegistrationAdmNoticeStudentsDMO.Where(t => t.PASRAN_ID == data.PASRAN_ID).ToList();
                    if (res.Count > 0)
                    {
                        foreach (var item in res)
                        {
                            var result = _db.PreadmissionSchoolRegistrationAdmNoticeStudentsDMO.Single(a => a.PASRAN_ID == item.PASRAN_ID && a.PASR_ID == item.PASR_ID);
                            _db.Remove(result);
                        }
                        var result1 = _db.PreadmissionSchoolRegistrationAdmissionNoticeDMO.Single(a => a.PASRAN_ID == data.PASRAN_ID);
                        _db.Remove(result1);
                    }
                        PreadmissionSchoolRegistrationAdmissionNoticeDMO objpge1 = new PreadmissionSchoolRegistrationAdmissionNoticeDMO();
                        objpge1.PASRAN_NAME = data.PASRAN_NAME;
                        objpge1.MI_ID = data.MI_Id;
                        objpge1.PASRAN_FEEAMOUNT = data.PASRAN_FEEAMOUNT;
                        objpge1.PASRAN_PAYDATE = data.PASRAN_PAYDATE;
                        objpge1.PASRAN_PAYTIME = data.PASRAN_PAYTIME;
                        objpge1.Createddate = DateTime.Now;
                        objpge1.Updateddate = DateTime.Now;
                        objpge1.CreatedBy = data.UserId;
                        objpge1.UpdatedBy = data.UserId;
                        _db.Add(objpge1);
                        if (data.list.Length > 0)
                        {
                            for (int i = 0; i < data.list.Length; i++)
                            {
                                var pk = _db.StudentApplication.Where(t => t.PASR_RegistrationNo == data.list[i].PASR_RegistrationNo).ToList();
                                data.pkid = pk[0].pasr_id;
                                PreadmissionSchoolRegistrationAdmNoticeStudentsDMO obb6 = new PreadmissionSchoolRegistrationAdmNoticeStudentsDMO();
                                obb6.PASRAN_ID = objpge1.PASRAN_ID;
                                obb6.PASR_ID = data.pkid;
                                obb6.PASRANS_REMARKS = data.list[i].PASRANS_REMARKS;
                                obb6.PASRANS_ADMDATE = data.list[i].PASRANS_ADMDATE;
                                obb6.PASRANS_TIME = data.list[i].PASRANS_TIME;
                                obb6.Createddate = DateTime.Now;
                                obb6.Updateddate = DateTime.Now;
                                _db.PreadmissionSchoolRegistrationAdmNoticeStudentsDMO.Add(obb6);
                            }
                        }
                        try
                        {

                            var contactExists = _db.SaveChanges();
                            if (contactExists > 0)
                            {
                                data.returnresult = true;
                                data.message = "Update";
                            }
                            else
                            {
                                data.returnresult = false;
                                data.message = "Failed";
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                else if (data.PASRAN_ID == 0)
                {
                    var res = _db.PreadmissionSchoolRegistrationAdmissionNoticeDMO.Where(t => t.MI_ID == data.MI_Id && t.PASRAN_NAME == data.PASRAN_NAME && t.PASRAN_FEEAMOUNT == data.PASRAN_FEEAMOUNT && t.PASRAN_PAYDATE == data.PASRAN_PAYDATE && t.PASRAN_PAYTIME == data.PASRAN_PAYTIME && t.PASRAN_ID != 0).ToList();
                    if (res.Count == 0)
                    {
                        try
                        {
                            PreadmissionSchoolRegistrationAdmissionNoticeDMO objpge1 = new PreadmissionSchoolRegistrationAdmissionNoticeDMO();
                            objpge1.PASRAN_NAME = data.PASRAN_NAME;
                            objpge1.MI_ID = data.MI_Id;
                            objpge1.PASRAN_FEEAMOUNT = data.PASRAN_FEEAMOUNT;
                            objpge1.PASRAN_PAYDATE = data.PASRAN_PAYDATE;
                            objpge1.PASRAN_PAYTIME = data.PASRAN_PAYTIME;
                            objpge1.Createddate = DateTime.Now;
                            objpge1.Updateddate = DateTime.Now;
                            objpge1.CreatedBy = data.UserId;
                            objpge1.UpdatedBy = data.UserId;
                            _db.Add(objpge1);
                            if (data.list.Length > 0)
                            {
                                for (int i = 0; i < data.list.Length; i++)
                                {
                                    var pk = _db.StudentApplication.Where(t => t.PASR_RegistrationNo == data.list[i].PASR_RegistrationNo).ToList();
                                    data.pkid = pk[0].pasr_id;
                                    PreadmissionSchoolRegistrationAdmNoticeStudentsDMO obb6 = new PreadmissionSchoolRegistrationAdmNoticeStudentsDMO();
                                    obb6.PASRAN_ID = objpge1.PASRAN_ID;
                                    obb6.PASR_ID = data.pkid;
                                    obb6.PASRANS_REMARKS = data.list[i].PASRANS_REMARKS;
                                    obb6.PASRANS_ADMDATE = data.list[i].PASRANS_ADMDATE;
                                    obb6.PASRANS_TIME = data.list[i].PASRANS_TIME;
                                    obb6.Createddate = DateTime.Now;
                                    obb6.Updateddate = DateTime.Now;
                                    _db.PreadmissionSchoolRegistrationAdmNoticeStudentsDMO.Add(obb6);
                                }
                            }
                            try
                            {
                                var contactExists = _db.SaveChanges();

                                if (contactExists > 0)
                                {
                                    data.returnresult = true;
                                    data.message = "Saved";
                                }
                                else
                                {
                                    data.returnresult = false;
                                    data.message = "Not Saved";
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                    else
                    {
                        data.message = "Duplicate";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public PreadmissionNoticeRegistrationReportDTO viewstudent(PreadmissionNoticeRegistrationReportDTO data)
        {
            try
            {
                data.uploadstuddetails = (from a in _db.StudentApplication
                                          from b in _db.PreadmissionSchoolRegistrationAdmissionNoticeDMO
                                          from c in _db.PreadmissionSchoolRegistrationAdmNoticeStudentsDMO
                                          where (a.pasr_id == c.PASR_ID && b.PASRAN_ID == data.PASRAN_ID && b.PASRAN_ID == c.PASRAN_ID)
                                          select new PreadmissionNoticeRegistrationReportDTO
                                          {
                                              //pasr_id = a.pasr_id,
                                              //PASRAN_ID = b.PASRAN_ID,
                                              PASR_FirstName = a.PASR_FirstName,
                                              //PASR_ActiveFlag = a.PASR_ActiveFlag,
                                              PASRANS_REMARKS = c.PASRANS_REMARKS,
                                              //PASRANS_ID = c.PASRANS_ID
                                          }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public PreadmissionNoticeRegistrationReportDTO Editdata(PreadmissionNoticeRegistrationReportDTO data)
        {
            try
            {
                List<long> PASR_IDS = new List<long>();

                data.programlist = (from a in _db.PreadmissionSchoolRegistrationAdmissionNoticeDMO
                                    where (a.MI_ID == data.MI_Id && a.PASRAN_ID == data.PASRAN_ID)
                                    select new PreadmissionNoticeRegistrationReportDTO
                                    {
                                        PASRAN_ID = a.PASRAN_ID,
                                        PASRAN_NAME = a.PASRAN_NAME,
                                        PASRAN_FEEAMOUNT = a.PASRAN_FEEAMOUNT,
                                        PASRAN_PAYTIME = a.PASRAN_PAYTIME,
                                        PASRAN_PAYDATE = a.PASRAN_PAYDATE,
                                    }).Distinct().OrderByDescending(t => t.PASRAN_ID).ToArray();

                var ids = _db.PreadmissionSchoolRegistrationAdmNoticeStudentsDMO.Where(t => t.PASRAN_ID == data.PASRAN_ID).Distinct().ToList();
                if (ids.Count > 0)
                {
                    foreach (var item in ids)
                    {
                        PASR_IDS.Add(item.PASR_ID);
                    }
                }

                data.reportgrid = (from a in _db.StudentApplication
                                   from b in _db.PreadmissionSchoolRegistrationAdmNoticeStudentsDMO
                                   from c in _db.castecategory
                                   from d in _db.PreadmissionSchoolRegistrationAdmissionNoticeDMO
                                   from e in _db.AcademicYear
                                   where (PASR_IDS.Contains(a.pasr_id) && b.PASRAN_ID == d.PASRAN_ID && a.MI_Id == data.MI_Id && a.pasr_id == b.PASR_ID && a.CasteCategory_Id == c.IMCC_Id && a.ASMAY_Id == e.ASMAY_Id && b.PASRAN_ID==data.PASRAN_ID)
                                   select new PreadmissionNoticeRegistrationReportDTO
                                   {
                                       PASRAN_ID = b.PASRAN_ID,
                                       pasr_id = a.pasr_id,
                                       // ASMAY_Id = a.ASMAY_Id,
                                       ASMAY_Year = e.ASMAY_Year,
                                       PASR_RegistrationNo = a.PASR_RegistrationNo,
                                       PASR_FirstName = a.PASR_FirstName,
                                       PASR_DOB = a.PASR_DOB,
                                       PASR_Age = a.PASR_Age,
                                       PASR_PerCity = a.PASR_PerCity,
                                       IMCC_CategoryName = c.IMCC_CategoryName,
                                       PASRANS_REMARKS = b.PASRANS_REMARKS,
                                       PASRANS_ADMDATE = b.PASRANS_ADMDATE,
                                       PASRANS_TIME = b.PASRANS_TIME,
                                   }).Distinct().ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public PreadmissionNoticeRegistrationReportDTO printData(PreadmissionNoticeRegistrationReportDTO data)
        {
            try
            {
                data.searchstudentDetails2 = (from a in _db.PreadmissionSchoolRegistrationAdmissionNoticeDMO
                                              from b in _db.PreadmissionSchoolRegistrationAdmNoticeStudentsDMO
                                              from c in _db.StudentApplication
                                              where (a.PASRAN_ID == b.PASRAN_ID && b.PASR_ID == c.pasr_id && a.MI_ID == data.MI_Id && a.PASRAN_ID == data.PASRAN_ID )
                                              select new PreadmissionNoticeRegistrationReportDTO
                                              {
                                                  pasr_id = b.PASR_ID,
                                                  PASRAN_FEEAMOUNT = a.PASRAN_FEEAMOUNT,
                                                  PASRAN_PAYDATE = a.PASRAN_PAYDATE,
                                                  PASRAN_PAYTIME = a.PASRAN_PAYTIME,
                                                  PASRANS_ADMDATE = b.PASRANS_ADMDATE,
                                                  PASRANS_TIME = b.PASRANS_TIME,
                                                  PASR_FirstName = c.PASR_FirstName,
                                                  PASR_RegistrationNo = c.PASR_RegistrationNo,
                                                
                                              }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }



    }
}
