using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Portals.Student;
using PreadmissionDTOs.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using AutoMapper;

namespace PortalHub.com.vaps.Student.Services
{
    public class ParentsSmartCardImpl : Interfaces.ParentsSmartCardInterface
    {
        private static ConcurrentDictionary<string, ParentSmartCardDTO> _login =
           new ConcurrentDictionary<string, ParentSmartCardDTO>();
        private PortalContext _context;
        public ParentsSmartCardImpl(PortalContext context)
        {
            _context = context;
        }

        public async Task<ParentSmartCardDTO> getloaddata(ParentSmartCardDTO data)
        {
            try
            {

                List<Country> allCountry = new List<Country>();
                allCountry = await _context.country.ToListAsync();
                data.countryDrpDown = allCountry.ToArray();

                List<State> Allstates = new List<State>();
                //allCountry = await _StudentApplicationContext.country.Include(State => State.vstate).ToListAsync();
                Allstates = await _context.state.ToListAsync();
                data.stateDrpDown = Allstates.ToArray();


                var Studentdata = _context.StudentDetailsupdateDMO.Where(d => d.AMST_ID == data.AMST_ID).ToArray();
                if (Studentdata.Count() > 0)
                {
                    data.existsornot = true;
                }
                else
                {
                    data.existsornot = false;
                }
                // var ASMAY_Id = 2;
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_StudentDetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
               SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
              SqlDbType.VarChar)
                    {
                        Value = data.AMST_ID
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.cardData = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd1 = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd1.CommandText = "Portal_UpdatedDetails";
                    cmd1.CommandType = CommandType.StoredProcedure;

                    cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd1.Parameters.Add(new SqlParameter("@ASMAY_Id",
               SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd1.Parameters.Add(new SqlParameter("@AMST_Id",
              SqlDbType.VarChar)
                    {
                        Value = data.AMST_ID
                    });


                    if (cmd1.Connection.State != ConnectionState.Open)
                        cmd1.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd1.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.Griddata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public async Task<ParentSmartCardDTO> getstudata(ParentSmartCardDTO data)
        {
            try
            {

                List<Country> allCountry = new List<Country>();
                allCountry = await _context.country.ToListAsync();
                data.countryDrpDown = allCountry.ToArray();

                List<State> Allstates = new List<State>();
                //allCountry = await _StudentApplicationContext.country.Include(State => State.vstate).ToListAsync();
                Allstates = await _context.state.ToListAsync();
                data.stateDrpDown = Allstates.ToArray();


                var Studentdata = _context.StudentDetailsupdateDMO.Where(d => d.AMST_ID == data.AMST_ID).ToArray();
                if (Studentdata.Count() > 0)
                {
                    data.existsornot = true;
                }
                else
                {
                    data.existsornot = false;
                }
                // var ASMAY_Id = 2;
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_StudentDetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
               SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
              SqlDbType.VarChar)
                    {
                        Value = data.AMST_ID
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.cardData = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd1 = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd1.CommandText = "Portal_UpdatedDetails";
                    cmd1.CommandType = CommandType.StoredProcedure;

                    cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd1.Parameters.Add(new SqlParameter("@ASMAY_Id",
               SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd1.Parameters.Add(new SqlParameter("@AMST_Id",
              SqlDbType.VarChar)
                    {
                        Value = data.AMST_ID
                    });


                    if (cmd1.Connection.State != ConnectionState.Open)
                        cmd1.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd1.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.Griddata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ParentSmartCardDTO guardianDetails(ParentSmartCardDTO data)
        {
            try
            {
                //data.guardianData = (from a in _context.StudentGuardianDMO
                //                    from b in _context.Adm_M_Student                                   
                //                    where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id) //&& a.AMST_Id=data.AMST_Id
                //                     select new ParentSmartCardDTO
                //                    {
                //                        AMSTG_Id = a.AMSTG_Id,
                //                        AMSTG_GuardianName = a.AMSTG_GuardianName,
                //                        AMSTG_GuardianAddress = a.AMSTG_GuardianAddress,
                //                        AMSTG_GuardianPhoneNo = a.AMSTG_GuardianPhoneNo,
                //                        AMSTG_emailid = a.AMSTG_emailid,
                //                        AMSTG_GuardianPhoto = a.AMSTG_GuardianPhoto

                //                    }).Distinct().ToArray();              
            }
            catch (Exception msg)
            {
                Console.WriteLine(msg.Message);
            }
            return data;
        }

        public ParentSmartCardDTO searchfilter(ParentSmartCardDTO data)
        {
            try
            {
                data.searchfilter = data.searchfilter.ToUpper();
                data.fillstudent = (from a in _context.Adm_M_Student
                                    from b in _context.StudentDetailsupdateDMO
                                    from c in _context.School_Adm_Y_StudentDMO
                                    where (a.AMST_Id == b.AMST_ID && a.MI_Id == data.MI_Id && a.AMST_Id == c.AMST_Id && c.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && ((a.AMST_FirstName.Trim().ToUpper() + ' ' + a.AMST_MiddleName.Trim().ToUpper() + ' ' + a.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || (a.AMST_FirstName.Trim().ToUpper() + a.AMST_MiddleName.Trim().ToUpper() + ' ' + a.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || a.AMST_FirstName.ToUpper().Contains(data.searchfilter) || a.AMST_MiddleName.ToUpper().Contains(data.searchfilter) || a.AMST_LastName.ToUpper().Contains(data.searchfilter)))
                                    select new ParentSmartCardDTO
                                    {
                                        AMST_ID = a.AMST_Id,
                                        AMST_FirstName = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName.ToUpper()) + " " + (a.AMST_MiddleName == null ? "" : a.AMST_MiddleName.ToUpper()) + " " + (a.AMST_LastName == null ? "" : a.AMST_LastName.ToUpper()) + ':' + a.AMST_AdmNo).Trim(),
                                    }
           ).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<ParentSmartCardDTO> getStateByCountry(int id)
        {
            ParentSmartCardDTO allstate = new ParentSmartCardDTO();
            try
            {
                //List<City> allcity = new List<City>();
                var st = await _context.state.Where(d => d.IVRMMC_Id == id).ToListAsync();
                allstate.stateDrpDown = st.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return allstate;
        }
        public ParentSmartCardDTO savedata(ParentSmartCardDTO data)
        {
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

            try
            {



                var Studentdata = _context.StudentDetailsupdateDMO.Where(d => d.AMST_ID == data.AMST_ID).ToArray();
                if (Studentdata.Count() == 0)
                {
                    StudentDetailsupdateDMO obj = new StudentDetailsupdateDMO();

                    obj.AMST_ID = data.AMST_ID;
                    obj.STP_SNAME = data.STP_SNAME;
                    obj.STP_SEMAIL = data.STP_SEMAIL;
                    obj.STP_SMOBILENO = data.STP_SMOBILENO;
                    obj.STP_SBLOOD = data.STP_SBLOOD;
                    obj.STP_SPHOTO = data.STP_SPHOTO;
                    obj.STP_FNAME = data.STP_FNAME;
                    obj.STP_FEMAIL = data.STP_FEMAIL;
                    obj.STP_FMOBILENO = data.STP_FMOBILENO;
                    obj.STP_FBLOOD = data.STP_FBLOOD;
                    obj.STP_FPHOTO = data.STP_FPHOTO;
                    obj.STP_MNAME = data.STP_MNAME;
                    obj.STP_MEMAIL = data.STP_MEMAIL;
                    obj.STP_MMOBILENO = data.STP_MMOBILENO;
                    obj.STP_MBLOOD = data.STP_MBLOOD;
                    obj.STP_MPHOTO = data.STP_MPHOTO;
                    obj.STP_PERSTREET = data.STP_PERSTREET;
                    obj.STP_PERAREA = data.STP_PERAREA;
                    obj.STP_PERCITY = data.STP_PERCITY;
                    obj.STP_PERSTATE = data.STP_PERSTATE;
                    obj.STP_PERCOUNTRY = data.STP_PERCOUNTRY;
                    obj.STP_PERPIN = data.STP_PERPIN;
                    obj.STP_CURSTREET = data.STP_CURSTREET;
                    obj.STP_CURAREA = data.STP_CURAREA;
                    obj.STP_CURCITY = data.STP_CURCITY;
                    obj.STP_CURSTATE = data.STP_CURSTATE;
                    obj.STP_CURCOUNTRY = data.STP_CURCOUNTRY;
                    obj.STP_CURPIN = data.STP_CURPIN;
                    obj.STP_STATUS = "Waiting";
                    obj.CreatedDate = indianTime;
                    obj.UpdatedDate = indianTime;

                    if (data.school== "stjm")
                    {
                        obj.STP_DOB = data.STP_DOB;
                        obj.STP_DOBWORDS = data.STP_DOBWORDS;
                    
                    }
                    _context.Add(obj);
                    int returnval = _context.SaveChanges();
                    if (returnval > 0)
                    {
                        data.returnval = "true";
                    }
                    else
                    {
                        data.returnval = "false";
                    }
                }
                else
                {
                    StudentDetailsupdateDMO obj = new StudentDetailsupdateDMO();
                    obj = _context.StudentDetailsupdateDMO.Single(d => d.AMST_ID == data.AMST_ID);


                    if (data.STP_FLAG == "S")
                    {
                        if (data.school == "stjm")
                        {
                            obj.STP_DOB = data.STP_DOB;
                            obj.STP_DOBWORDS = data.STP_DOBWORDS;

                        }
                        obj.STP_SNAME = data.STP_SNAME;
                        obj.STP_SEMAIL = data.STP_SEMAIL;
                        obj.STP_SMOBILENO = data.STP_SMOBILENO;
                        obj.STP_SBLOOD = data.STP_SBLOOD;
                        obj.STP_SPHOTO = data.STP_SPHOTO;
                    }
                    else if (data.STP_FLAG == "F")
                    {
                        obj.STP_FNAME = data.STP_FNAME;
                        obj.STP_FEMAIL = data.STP_FEMAIL;
                        obj.STP_FMOBILENO = data.STP_FMOBILENO;
                        obj.STP_FBLOOD = data.STP_FBLOOD;
                        obj.STP_FPHOTO = data.STP_FPHOTO;
                    }
                    else if (data.STP_FLAG == "M")
                    {
                        obj.STP_MNAME = data.STP_MNAME;
                        obj.STP_MEMAIL = data.STP_MEMAIL;
                        obj.STP_MMOBILENO = data.STP_MMOBILENO;
                        obj.STP_MBLOOD = data.STP_MBLOOD;
                        obj.STP_MPHOTO = data.STP_MPHOTO;
                    }


                    obj.STP_PERSTREET = data.STP_PERSTREET;
                    obj.STP_PERAREA = data.STP_PERAREA;
                    obj.STP_PERCITY = data.STP_PERCITY;
                    obj.STP_PERSTATE = data.STP_PERSTATE;
                    obj.STP_PERCOUNTRY = data.STP_PERCOUNTRY;
                    obj.STP_PERPIN = data.STP_PERPIN;
                    obj.STP_CURSTREET = data.STP_CURSTREET;
                    obj.STP_CURAREA = data.STP_CURAREA;
                    obj.STP_CURCITY = data.STP_CURCITY;
                    obj.STP_CURSTATE = data.STP_CURSTATE;
                    obj.STP_CURCOUNTRY = data.STP_CURCOUNTRY;
                    obj.STP_CURPIN = data.STP_CURPIN;
                    obj.STP_STATUS = "Waiting";
                    // obj.CreatedDate = indianTime;
                    obj.UpdatedDate = indianTime;
                    _context.Update(obj);
                    int returnval = _context.SaveChanges();
                    if (returnval > 0)
                    {
                        data.returnval = "true";
                    }
                    else
                    {
                        data.returnval = "false";
                    }
                }
            }
            catch (Exception msg)
            {
                Console.WriteLine(msg.Message);
            }
            return data;
        }

        public ParentSmartCardDTO savedataadmin(ParentSmartCardDTO data)
        {
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

            try
            {

                var Studentdata = _context.StudentDetailsupdateDMO.Where(d => d.AMST_ID == data.AMST_ID).ToArray();
                if (Studentdata.Count() == 0)
                {
                    StudentDetailsupdateDMO obj = new StudentDetailsupdateDMO();

                    obj.AMST_ID = data.AMST_ID;
                    obj.STP_SNAME = data.STP_SNAME;
                    obj.STP_SEMAIL = data.STP_SEMAIL;
                    obj.STP_SMOBILENO = data.STP_SMOBILENO;
                    obj.STP_SBLOOD = data.STP_SBLOOD;
                    obj.STP_SPHOTO = data.STP_SPHOTO;
                    obj.STP_FNAME = data.STP_FNAME;
                    obj.STP_FEMAIL = data.STP_FEMAIL;
                    obj.STP_FMOBILENO = data.STP_FMOBILENO;
                    obj.STP_FBLOOD = data.STP_FBLOOD;
                    obj.STP_FPHOTO = data.STP_FPHOTO;
                    obj.STP_MNAME = data.STP_MNAME;
                    obj.STP_MEMAIL = data.STP_MEMAIL;
                    obj.STP_MMOBILENO = data.STP_MMOBILENO;
                    obj.STP_MBLOOD = data.STP_MBLOOD;
                    obj.STP_MPHOTO = data.STP_MPHOTO;
                    obj.STP_PERSTREET = data.STP_PERSTREET;
                    obj.STP_PERAREA = data.STP_PERAREA;
                    obj.STP_PERCITY = data.STP_PERCITY;
                    obj.STP_PERSTATE = data.STP_PERSTATE;
                    obj.STP_PERCOUNTRY = data.STP_PERCOUNTRY;
                    obj.STP_PERPIN = data.STP_PERPIN;
                    obj.STP_CURSTREET = data.STP_CURSTREET;
                    obj.STP_CURAREA = data.STP_CURAREA;
                    obj.STP_CURCITY = data.STP_CURCITY;
                    obj.STP_CURSTATE = data.STP_CURSTATE;
                    obj.STP_CURCOUNTRY = data.STP_CURCOUNTRY;
                    obj.STP_CURPIN = data.STP_CURPIN;
                    if (data.school == "stjm")
                    {
                        obj.STP_DOB = data.STP_DOB;
                        obj.STP_DOBWORDS = data.STP_DOBWORDS;

                    }
                    obj.STP_STATUS = "Updated";
                    obj.CreatedDate = indianTime;
                    obj.UpdatedDate = indianTime;
                    _context.Add(obj);
                    int returnval = _context.SaveChanges();
                    if (returnval > 0)
                    {
                        data.returnval = "true";
                    }
                    else
                    {
                        data.returnval = "false";
                    }
                }
                else
                {
                    StudentDetailsupdateDMO obj = new StudentDetailsupdateDMO();
                    obj = _context.StudentDetailsupdateDMO.Single(d => d.AMST_ID == data.AMST_ID);


                    if (data.STP_FLAG == "S")
                    {
                        if (data.school == "stjm")
                        {
                            obj.STP_DOB = data.STP_DOB;
                            obj.STP_DOBWORDS = data.STP_DOBWORDS;

                        }
                        obj.STP_SNAME = data.STP_SNAME;
                        obj.STP_SEMAIL = data.STP_SEMAIL;
                        obj.STP_SMOBILENO = data.STP_SMOBILENO;
                        obj.STP_SBLOOD = data.STP_SBLOOD;
                        obj.STP_SPHOTO = data.STP_SPHOTO;
                    }
                    else if (data.STP_FLAG == "F")
                    {
                        obj.STP_FNAME = data.STP_FNAME;
                        obj.STP_FEMAIL = data.STP_FEMAIL;
                        obj.STP_FMOBILENO = data.STP_FMOBILENO;
                        obj.STP_FBLOOD = data.STP_FBLOOD;
                        obj.STP_FPHOTO = data.STP_FPHOTO;
                    }
                    else if (data.STP_FLAG == "M")
                    {
                        obj.STP_MNAME = data.STP_MNAME;
                        obj.STP_MEMAIL = data.STP_MEMAIL;
                        obj.STP_MMOBILENO = data.STP_MMOBILENO;
                        obj.STP_MBLOOD = data.STP_MBLOOD;
                        obj.STP_MPHOTO = data.STP_MPHOTO;
                    }


                    obj.STP_PERSTREET = data.STP_PERSTREET;
                    obj.STP_PERAREA = data.STP_PERAREA;
                    obj.STP_PERCITY = data.STP_PERCITY;
                    obj.STP_PERSTATE = data.STP_PERSTATE;
                    obj.STP_PERCOUNTRY = data.STP_PERCOUNTRY;
                    obj.STP_PERPIN = data.STP_PERPIN;
                    obj.STP_CURSTREET = data.STP_CURSTREET;
                    obj.STP_CURAREA = data.STP_CURAREA;
                    obj.STP_CURCITY = data.STP_CURCITY;
                    obj.STP_CURSTATE = data.STP_CURSTATE;
                    obj.STP_CURCOUNTRY = data.STP_CURCOUNTRY;
                    obj.STP_CURPIN = data.STP_CURPIN;
                    obj.STP_STATUS = "Updated";
                    // obj.CreatedDate = indianTime;
                    obj.UpdatedDate = indianTime;
                    _context.Update(obj);
                    int returnval = _context.SaveChanges();
                    if (returnval > 0)
                    {
                        data.returnval = "true";
                    }
                    else
                    {
                        data.returnval = "false";
                    }



                    Adm_M_Student objstudent = new Adm_M_Student();
                    objstudent = _context.Adm_M_Student.Single(d => d.AMST_Id == data.AMST_ID);

                    if (data.STP_FLAG == "S")
                    {
                        objstudent.AMST_emailId = data.STP_SEMAIL;
                        objstudent.AMST_MobileNo = Convert.ToInt64(data.STP_SMOBILENO);
                        objstudent.AMST_BloodGroup = data.STP_SBLOOD;
                        objstudent.AMST_Photoname = data.STP_SPHOTO;
                    }
                    else if (data.STP_FLAG == "F")
                    {
                        objstudent.AMST_FatherMobleNo = Convert.ToInt64(data.STP_FMOBILENO);
                        objstudent.AMST_FatheremailId = data.STP_FEMAIL;
                        //objstudent.STP_FBLOOD = data.STP_FBLOOD;
                        objstudent.ANST_FatherPhoto = data.STP_FPHOTO;
                    }
                    else if (data.STP_FLAG == "M")
                    {
                        //objstudent.STP_MNAME = data.STP_MNAME;
                        objstudent.AMST_MotherEmailId = data.STP_MEMAIL;
                        objstudent.AMST_MotherMobileNo = data.STP_MMOBILENO;
                        //objstudent.STP_MBLOOD = data.STP_MBLOOD;
                        objstudent.ANST_MotherPhoto = data.STP_MPHOTO;
                    }
                    
                    objstudent.AMST_PerStreet = data.STP_PERSTREET;
                    objstudent.AMST_PerArea = data.STP_PERAREA;
                    objstudent.AMST_PerCity = data.STP_PERCITY;
                    objstudent.AMST_PerState = data.STP_PERSTATE;
                    objstudent.AMST_PerCountry = data.STP_PERCOUNTRY;
                    objstudent.AMST_PerPincode = Convert.ToInt32(data.STP_PERPIN);
                    objstudent.AMST_ConStreet = data.STP_CURSTREET;
                    objstudent.AMST_ConArea = data.STP_CURAREA;
                    objstudent.AMST_ConCity = data.STP_CURCITY;
                    objstudent.AMST_ConState = data.STP_CURSTATE;
                    objstudent.AMST_ConCountry = data.STP_CURCOUNTRY;
                    objstudent.AMST_ConPincode = Convert.ToInt32(data.STP_CURPIN);
                    if (data.school == "stjm")
                    {
                        objstudent.AMST_DOB = Convert.ToDateTime(data.STP_DOB);
                        objstudent.AMST_DOB_Words = data.STP_DOBWORDS;

                    }
                    //objstudent.UpdatedDate = indianTime;
                    _context.Update(objstudent);
                     returnval = _context.SaveChanges();
                    if (returnval > 0)
                    {
                        data.returnval = "true";
                    }
                    else
                    {
                        data.returnval = "false";
                    }

                    father_mobile_no(data);
                    father_email_ids(data);
                    mother_mobile_no(data);
                    mother_email_id(data);
                    student_mobile_no(data);
                    student_email_id(data);
                }
            }
            catch (Exception msg)
            {
                Console.WriteLine(msg.Message);
            }
            return data;
        }

        public ParentSmartCardDTO father_mobile_no(ParentSmartCardDTO datamobileno)
        {
            try
            {
                if (datamobileno.STP_FMOBILENO != null)
                {
                    var Phone_Noresultremove = _context.Adm_M_Student_FatherMobileNo.Where(t =>t.AMST_Id == datamobileno.AMST_ID).ToArray();
                    if(Phone_Noresultremove.Count()>0)
                    {
                        var Phone_Noresult = _context.Adm_M_Student_FatherMobileNo.Single(t => t.AMSTFMNO_Id == Phone_Noresultremove.FirstOrDefault().AMSTFMNO_Id);
                        Phone_Noresult.UpdatedDate = DateTime.Now;
                        Phone_Noresult.AMST_FatherMobile_No = Convert.ToInt64(datamobileno.STP_FMOBILENO);
                        _context.Update(Phone_Noresult);
                        _context.SaveChanges();
                    }
                    else
                    {
                        Adm_M_Student_FatherMobileNo phone1 =new Adm_M_Student_FatherMobileNo();
                        phone1.CreatedDate = DateTime.Now;
                        phone1.UpdatedDate = DateTime.Now;
                        phone1.AMST_FatherMobile_No = Convert.ToInt64(datamobileno.STP_FMOBILENO);
                        _context.Add(phone1);
                        _context.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
               
            }

            return datamobileno;
        }
        
        //save and update multiple father email ids
        public ParentSmartCardDTO father_email_ids(ParentSmartCardDTO datamobileno)
        {
            try
            {

                if (datamobileno.STP_FEMAIL != null)
                {
                    var Phone_Noresultremove = _context.Adm_Master_Father_Email.Where(t => t.AMST_Id == datamobileno.AMST_ID).ToArray();
                    if (Phone_Noresultremove.Count() > 0)
                    {
                        var Phone_Noresult = _context.Adm_Master_Father_Email.Single(t => t.AMSTFEMAIL_Id == Phone_Noresultremove.FirstOrDefault().AMSTFEMAIL_Id);
                        Phone_Noresult.UpdatedDate = DateTime.Now;
                        Phone_Noresult.AMST_FatheremailId = datamobileno.STP_FEMAIL;
                        _context.Update(Phone_Noresult);
                        _context.SaveChanges();
                    }
                    else
                    {
                        Adm_Master_Father_Email phone1 = new Adm_Master_Father_Email();
                        phone1.CreatedDate = DateTime.Now;
                        phone1.UpdatedDate = DateTime.Now;
                        phone1.AMST_FatheremailId = datamobileno.STP_FEMAIL;
                        _context.Add(phone1);
                        _context.SaveChanges();
                    }
                }
               
           

            }
            catch (Exception e)
            {
                
            }
            return datamobileno;
        }

        //save and update multiple mother mobile number
        public ParentSmartCardDTO mother_mobile_no(ParentSmartCardDTO datamobileno)
        {
            try
            {

                if (datamobileno.STP_MMOBILENO != null)
                {
                    var Phone_Noresultremove = _context.Adm_M_Mother_MobileNo.Where(t => t.AMST_Id == datamobileno.AMST_ID).ToArray();
                    if (Phone_Noresultremove.Count() > 0)
                    {
                        var Phone_Noresult = _context.Adm_M_Mother_MobileNo.Single(t => t.AMSTMMNO_Id == Phone_Noresultremove.FirstOrDefault().AMSTMMNO_Id);
                        Phone_Noresult.UpdatedDate = DateTime.Now;
                        Phone_Noresult.AMST_MotherMobileNo = Convert.ToInt64(datamobileno.STP_MMOBILENO);
                        _context.Update(Phone_Noresult);
                        _context.SaveChanges();
                    }
                    else
                    {
                        Adm_M_Mother_MobileNo phone1 = new Adm_M_Mother_MobileNo();
                        phone1.CreatedDate = DateTime.Now;
                        phone1.UpdatedDate = DateTime.Now;
                        phone1.AMST_MotherMobileNo = Convert.ToInt64(datamobileno.STP_MMOBILENO);
                        _context.Add(phone1);
                        _context.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                
            }

            return datamobileno;
        }

        //saving and updating mother email ids
        public ParentSmartCardDTO mother_email_id(ParentSmartCardDTO datamobileno)
        {
            try
            {
                if (datamobileno.STP_MEMAIL != null)
                {
                    var Phone_Noresultremove = _context.Adm_M_Mother_Emailid.Where(t => t.AMST_Id == datamobileno.AMST_ID).ToArray();
                    if (Phone_Noresultremove.Count() > 0)
                    {
                        var Phone_Noresult = _context.Adm_M_Mother_Emailid.Single(t => t.AMSTMEMAIL_Id == Phone_Noresultremove.FirstOrDefault().AMSTMEMAIL_Id);
                        Phone_Noresult.UpdatedDate = DateTime.Now;
                        Phone_Noresult.AMST_MotheremailId = datamobileno.STP_MEMAIL;
                        _context.Update(Phone_Noresult);
                        _context.SaveChanges();
                    }
                    else
                    {
                        Adm_M_Mother_Emailid phone1 = new Adm_M_Mother_Emailid();
                        phone1.CreatedDate = DateTime.Now;
                        phone1.UpdatedDate = DateTime.Now;
                        phone1.AMST_MotheremailId = datamobileno.STP_MEMAIL;
                        _context.Add(phone1);
                        _context.SaveChanges();
                    }
                }
                
            }
            catch (Exception e)
            {
               
            }
            return datamobileno;
        }

        //saving and updating student mobile
        public ParentSmartCardDTO student_mobile_no(ParentSmartCardDTO datamobileno)
        {
            try
            {

                if (datamobileno.STP_SMOBILENO != null)
                {
                    var Phone_Noresultremove = _context.Adm_M_Student_MobileNo.Where(t => t.AMST_Id == datamobileno.AMST_ID).ToArray();
                    if (Phone_Noresultremove.Count() > 0)
                    {
                        var Phone_Noresult = _context.Adm_M_Student_MobileNo.Single(t => t.AMSTSMS_Id == Phone_Noresultremove.FirstOrDefault().AMSTSMS_Id);
                        Phone_Noresult.UpdatedDate = DateTime.Now;
                        Phone_Noresult.AMSTSMS_MobileNo = Convert.ToString(datamobileno.STP_SMOBILENO);
                        _context.Update(Phone_Noresult);
                        _context.SaveChanges();
                    }
                    else
                    {
                        Adm_M_Student_MobileNo phone1 = new Adm_M_Student_MobileNo();
                        phone1.CreatedDate = DateTime.Now;
                        phone1.UpdatedDate = DateTime.Now;
                        phone1.AMSTSMS_MobileNo = Convert.ToString(datamobileno.STP_SMOBILENO);
                        _context.Add(phone1);
                        _context.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
               
            }
            return datamobileno;
        }

        //saving and updating emailids
        public ParentSmartCardDTO student_email_id(ParentSmartCardDTO datamobileno)
        {
            try
            {

                if (datamobileno.STP_SEMAIL != null)
                {
                    var Phone_Noresultremove = _context.Adm_M_Student_Email_Id.Where(t => t.AMST_Id == datamobileno.AMST_ID).ToArray();
                    if (Phone_Noresultremove.Count() > 0)
                    {
                        var Phone_Noresult = _context.Adm_M_Student_Email_Id.Single(t => t.AMSTE_EmailId == Phone_Noresultremove.FirstOrDefault().AMSTE_EmailId);
                        Phone_Noresult.UpdatedDate = DateTime.Now;
                        Phone_Noresult.AMSTE_EmailId = datamobileno.STP_SEMAIL;
                        _context.Update(Phone_Noresult);
                        _context.SaveChanges();
                    }
                    else
                    {
                        Adm_M_Student_Email_Id phone1 = new Adm_M_Student_Email_Id();
                        phone1.CreatedDate = DateTime.Now;
                        phone1.UpdatedDate = DateTime.Now;
                        phone1.AMSTE_EmailId = datamobileno.STP_MEMAIL;
                        _context.Add(phone1);
                        _context.SaveChanges();
                    }
                }

            }
            catch (Exception e)
            {
               
            }

            return datamobileno;
        }


        public ParentSmartCardDTO getreport(ParentSmartCardDTO data) {

            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_PARENT_SMARTCARD_REPORT";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    //      cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    //SqlDbType.BigInt)
                    //      {
                    //          Value = data.AMST_ID
                    //      });
                    //      cmd.Parameters.Add(new SqlParameter("@RoleName",
                    //SqlDbType.Char)
                    //      {
                    //          Value = data.RoleName
                    //      });
                    cmd.Parameters.Add(new SqlParameter("@STFLAG",
              SqlDbType.Char)
                    {
                        Value = data.STP_FLAG
                    });
                    cmd.Parameters.Add(new SqlParameter("@FRMDATE",
              SqlDbType.Date)
                    {
                        Value = data.FromDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@TODATE",
              SqlDbType.Date)
                    {
                        Value = data.ToDate
                    });



                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader =  cmd.ExecuteReader())
                        {
                            while ( dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.updatestudetailslist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }
    }
}
