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
    public class UpdateRequestImpl : Interfaces.UpdateRequestInterface
    {
        private static ConcurrentDictionary<string, UpdateRequestDTO> _login =
           new ConcurrentDictionary<string, UpdateRequestDTO>();
        private PortalContext _context;
        public UpdateRequestImpl(PortalContext context)
        {
            _context = context;
        }

        public async Task<UpdateRequestDTO> getloaddata(UpdateRequestDTO data)
        {
            try
            {

                List<Country> allCountry = new List<Country>();
                allCountry = await _context.country.ToListAsync();
                data.countryDrpDown = allCountry.ToArray();

                List<State> Allstates = new List<State>();
             
                Allstates = await _context.state.ToListAsync();
                data.stateDrpDown = Allstates.ToArray();
                if (data.RoleName !=null && data.RoleName!="")
                {
                    data.RoleName = data.RoleName.ToLower();
                }
                
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_PORTAL_STUDENT_UPDATE_REQUEST";
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
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
              SqlDbType.BigInt)
                    {
                        Value = data.AMST_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@RoleName",
              SqlDbType.Char)
                    {
                        Value = data.RoleName
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
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<UpdateRequestDTO> getreploaddata(UpdateRequestDTO data)
        {
            try
            {
                data.acamiclist = _context.AcademicYearDMO.Where(e => e.MI_Id == data.MI_Id && e.Is_Active == true).OrderByDescending(e => e.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<UpdateRequestDTO> getreport(UpdateRequestDTO data)
        {
            try
            {
                if (data.RoleName != null && data.RoleName != "")
                {
                    data.RoleName = data.RoleName.ToLower();
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_UPDATE_REQUEST_REPORT";
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
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
              SqlDbType.BigInt)
                    {
                        Value = data.AMST_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@RoleName",
              SqlDbType.Char)
                    {
                        Value = data.RoleName
                    });
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
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public async Task<UpdateRequestDTO> getstudata(UpdateRequestDTO data)
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
        public UpdateRequestDTO guardianDetails(UpdateRequestDTO data)
        {
            try
            {
                //data.guardianData = (from a in _context.StudentGuardianDMO
                //                    from b in _context.Adm_M_Student                                   
                //                    where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id) //&& a.AMST_Id=data.AMST_Id
                //                     select new UpdateRequestDTO
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

        public UpdateRequestDTO searchfilter(UpdateRequestDTO data)
        {
            try
            {
                data.searchfilter = data.searchfilter.ToUpper();
                data.fillstudent = (from a in _context.Adm_M_Student
                                    from b in _context.StudentDetailsupdateDMO
                                    from c in _context.School_Adm_Y_StudentDMO
                                    where (a.AMST_Id == b.AMST_ID && a.MI_Id == data.MI_Id && a.AMST_Id == c.AMST_Id && c.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && ((a.AMST_FirstName.Trim().ToUpper() + ' ' + a.AMST_MiddleName.Trim().ToUpper() + ' ' + a.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || (a.AMST_FirstName.Trim().ToUpper() + a.AMST_MiddleName.Trim().ToUpper() + ' ' + a.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || a.AMST_FirstName.ToUpper().Contains(data.searchfilter) || a.AMST_MiddleName.ToUpper().Contains(data.searchfilter) || a.AMST_LastName.ToUpper().Contains(data.searchfilter)))
                                    select new UpdateRequestDTO
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

        public async Task<UpdateRequestDTO> getStateByCountry(int id)
        {
            UpdateRequestDTO allstate = new UpdateRequestDTO();
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
        public UpdateRequestDTO saverequest(UpdateRequestDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                if (data.ASTUREQ_Id > 0)
                {
                    var updatelist = _context.Adm_Student_Update_RequestDMO.Single(d => d.ASTUREQ_Id == data.ASTUREQ_Id);
                    updatelist.ASTUREQ_PerStreet = data.STP_PERSTREET;
                    updatelist.ASTUREQ_PerArea = data.STP_PERAREA;
                    updatelist.ASTUREQ_PerCity = data.STP_PERCITY;
                    updatelist.ASTUREQ_PerState = data.STP_PERSTATE;
                    updatelist.IVRMMC_Id = data.STP_PERCOUNTRY;
                    updatelist.ASTUREQ_PerPincode = data.STP_PERPIN;
                    updatelist.ASTUREQ_ConStreet = data.STP_CURSTREET;
                    updatelist.ASTUREQ_ConArea = data.STP_CURAREA;
                    updatelist.ASTUREQ_ConCity = data.STP_CURCITY;
                    updatelist.ASTUREQ_ConState = data.STP_CURSTATE;
                    updatelist.ASTUREQ_ConCountryId = data.STP_CURCOUNTRY;
                    updatelist.ASTUREQ_ConPincode = data.STP_CURPIN;
                    updatelist.ASTUREQ_ConPincode = data.STP_CURPIN;
                    updatelist.ASTUREQ_UpdatedDate = indianTime;
                    updatelist.ASTUREQ_Date = indianTime;
                    updatelist.ASTUREQ_CreatedDate = indianTime;
                    updatelist.ASTUREQ_UpdatedBy = data.User_Id;
                    updatelist.ASTUREQ_MobileNo = data.Mobilenumber;
                    updatelist.ASTUREQ_EmailId = data.EmailidforCandidate;
                    updatelist.ASTUREQ_FatherMobleNo = data.AMST_FatherMobleNo;
                    updatelist.ASTUREQ_FatheremailId = data.AMST_FatheremailId;
                    updatelist.ASTUREQ_MotheremailId = data.AMST_MotherEmailId;
                    updatelist.ASTUREQ_MotherMobleNo = data.AMST_MotherMobileNo;
                    updatelist.ASTUREQ_BloodGroup = data.AMST_BloodGroup;
                    updatelist.ASTUREQ_ReqStatus = "PENDING";

                    if (data.AMSTG_Id > 0)
                    {
                        updatelist.AMSTG_Id = data.AMSTG_Id;
                        updatelist.ASTUREQ_GuardianMobileNo = data.ASTUREQ_GuardianMobileNo;
                        updatelist.ASTUREQ_GuardianEmailId = data.ASTUREQ_GuardianEmailId;
                    }

                    _context.Update(updatelist);
                    int rx = _context.SaveChanges();
                    if (rx > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = true;
                    }
                }
                else
                {


                    Adm_Student_Update_RequestDMO obj = new Adm_Student_Update_RequestDMO();

                    obj.MI_Id = data.MI_Id;
                    obj.ASMAY_Id = data.ASMAY_Id;
                    obj.AMST_Id = data.AMST_ID;
                    obj.ASTUREQ_PerStreet = data.STP_PERSTREET;
                    obj.ASTUREQ_PerArea = data.STP_PERAREA;
                    obj.ASTUREQ_PerCity = data.STP_PERCITY;
                    obj.ASTUREQ_PerState = data.STP_PERSTATE;
                    obj.IVRMMC_Id = data.STP_PERCOUNTRY;
                    obj.ASTUREQ_PerPincode = data.STP_PERPIN;
                    obj.ASTUREQ_ConStreet = data.STP_CURSTREET;
                    obj.ASTUREQ_ConArea = data.STP_CURAREA;
                    obj.ASTUREQ_ConCity = data.STP_CURCITY;
                    obj.ASTUREQ_ConState = data.STP_CURSTATE;
                    obj.ASTUREQ_ConCountryId = data.STP_CURCOUNTRY;
                    obj.ASTUREQ_ConPincode = data.STP_CURPIN;
                    obj.ASTUREQ_ConPincode = data.STP_CURPIN;
                    obj.ASTUREQ_UpdatedDate = indianTime;
                    obj.ASTUREQ_Date = indianTime;
                    obj.ASTUREQ_CreatedDate = indianTime;
                    obj.ASTUREQ_UpdatedBy = data.User_Id;
                    obj.ASTUREQ_ActiveFlag = true;

                    obj.ASTUREQ_ApprovedFlg = false;
                    obj.ASTUREQ_MobileNo = data.Mobilenumber;
                    obj.ASTUREQ_EmailId = data.EmailidforCandidate;
                    obj.ASTUREQ_FatherMobleNo = data.AMST_FatherMobleNo;
                    obj.ASTUREQ_FatheremailId = data.AMST_FatheremailId;
                    obj.ASTUREQ_MotheremailId = data.AMST_MotherEmailId;
                    obj.ASTUREQ_MotherMobleNo = data.AMST_MotherMobileNo;
                    obj.ASTUREQ_BloodGroup = data.AMST_BloodGroup;
                    obj.ASTUREQ_ReqStatus = "PENDING";
                    obj.ASTUREQ_ChangeConfirmFlg = "CHANGE";
                    if (data.AMSTG_Id > 0)
                    {
                        obj.AMSTG_Id = data.AMSTG_Id;
                        obj.ASTUREQ_GuardianMobileNo = data.ASTUREQ_GuardianMobileNo;
                        obj.ASTUREQ_GuardianEmailId = data.ASTUREQ_GuardianEmailId;
                    }

                    var existdata = _context.Adm_M_Student.Where(a => a.AMST_Id == data.AMST_ID && a.MI_Id == data.MI_Id && a.AMST_ActiveFlag == 1).ToList();
                    if (existdata.Count > 0)
                    {
                        obj.ASTUREQ_ApplStatus = existdata[0].AMST_ApplStatus;
                        obj.ASTUREQ_FirstName = existdata[0].AMST_FirstName;
                        obj.ASTUREQ_MiddleName = existdata[0].AMST_MiddleName;
                        obj.ASTUREQ_LastName = existdata[0].AMST_LastName;
                        obj.ASTUREQ_RegistrationNo = existdata[0].AMST_RegistrationNo;
                        obj.ASTUREQ_AdmNo = existdata[0].AMST_AdmNo;
                        obj.AMC_Id = existdata[0].AMC_Id;
                        obj.ASTUREQ_Sex = existdata[0].AMST_Sex;
                        obj.ASTUREQ_DOB = existdata[0].AMST_DOB;
                        obj.ASTUREQ_DOBinwords = existdata[0].AMST_DOB_Words;
                        obj.ASTUREQ_Age = existdata[0].PASR_Age;
                        obj.ASMCL_Id = existdata[0].ASMCL_Id;
                       // obj.ASTUREQ_BloodGroup = existdata[0].AMST_BloodGroup;
                        obj.ASTUREQ_MotherTongue = existdata[0].AMST_MotherTongue;
                        obj.ASTUREQ_HomeLaguage = existdata[0].AMST_LanguageSpoken;
                        obj.ASTUREQ_BirthCertNo = existdata[0].AMST_BirthCertNO;
                        obj.IVRMMR_Id = existdata[0].IVRMMR_Id;
                        obj.IMCC_Id = existdata[0].IMCC_Id;
                        obj.IMC_Id = existdata[0].IC_Id;
                        obj.ASTUREQ_StudentSubCaste = existdata[0].AMST_SubCasteIMC_Id;
                        obj.ASTUREQ_PerAdd3 = existdata[0].AMST_PerAdd3;
                        obj.ASTUREQ_ConAdd3 = existdata[0].AMST_PerAdd3;
                        obj.ASTUREQ_Village = existdata[0].AMST_Village;
                        obj.ASTUREQ_Taluk = existdata[0].AMST_Taluk;
                        obj.ASTUREQ_District = existdata[0].AMST_Distirct;
                        obj.ASTUREQ_AadharNo = existdata[0].AMST_AadharNo;
                        obj.ASTUREQ_StuBankAccNo = existdata[0].AMST_StuBankAccNo;
                        obj.ASTUREQ_StudentPANCard = existdata[0].AMST_StudentPANNo;
                        obj.ASTUREQ_StuBankIFSCCode = existdata[0].AMST_StuBankIFSC_Code;
                        obj.ASTUREQ_StuCasteCertiNo = existdata[0].AMST_StuCasteCertiNo;
                        obj.ASTUREQ_FatherAliveFlag = existdata[0].AMST_FatherAliveFlag;
                        obj.ASTUREQ_FatherMaritalStatusFlg = existdata[0].AMST_FatherMaritalStatus;
                        obj.ASTUREQ_FatherName = existdata[0].AMST_FatherName;
                        obj.ASTUREQ_FatherAadharNo = existdata[0].AMST_FatherAadharNo;
                        obj.ASTUREQ_FatherSurname = existdata[0].AMST_FatherSurname;
                        obj.ASTUREQ_FatherEducation = existdata[0].AMST_FatherEducation;
                        obj.ASTUREQ_FatherOccupation = existdata[0].AMST_FatherOccupation;
                        obj.ASTUREQ_FatherOfficeAdd = existdata[0].AMST_FatherOfficeAdd;
                        obj.ASTUREQ_FatherDesignation = existdata[0].AMST_FatherDesignation;
                        obj.ASTUREQ_FatherMonIncome = existdata[0].AMST_FatherMonIncome;
                        obj.ASTUREQ_FatherAnnIncome = existdata[0].AMST_FatherAnnIncome;
                        obj.ASTUREQ_FatherNationality = existdata[0].AMST_FatherNationality;


                        obj.ASTUREQ_FatherReligion = existdata[0].AMST_FatherReligion;


                        obj.ASTUREQ_FatherCaste = existdata[0].AMST_FatherCaste;


                        obj.ASTUREQ_FatherSubCaste = existdata[0].AMST_FatherSubCaste;
                        obj.ASTUREQ_FatherBankAccNo = existdata[0].AMST_FatherBankAccNo;
                        obj.ASTUREQ_FatherBankIFSCCode = existdata[0].AMST_FatherBankIFSC_Code;
                        obj.ASTUREQ_FatherCasteCertiNo = existdata[0].AMST_FatherCasteCertiNo;
                        obj.ASTUREQ_FatherPhoto = existdata[0].ANST_FatherPhoto;
                        obj.ASTUREQ_FatherSign = existdata[0].AMST_Father_Signature;
                        obj.ASTUREQ_FatherFingerprint = existdata[0].AMST_Father_FingerPrint;
                        obj.ASTUREQ_FatherPANCardNo = existdata[0].AMST_FatherPANNo;
                        obj.ASTUREQ_MotherAliveFlag = existdata[0].AMST_MotherAliveFlag;
                        obj.ASTUREQ_MotherName = existdata[0].AMST_MotherName;
                        obj.ASTUREQ_MotherAadharNo = existdata[0].AMST_MotherAadharNo;
                        obj.ASTUREQ_MotherSurname = existdata[0].AMST_MotherSurname;
                        obj.ASTUREQ_MotherEducation = existdata[0].AMST_MotherEducation;
                        obj.ASTUREQ_MotherOccupation = existdata[0].AMST_MotherOccupation;
                        obj.ASTUREQ_MotherOfficeAdd = existdata[0].AMST_MotherOfficeAdd;
                        obj.ASTUREQ_MotherDesignation = existdata[0].AMST_MotherDesignation;
                        obj.ASTUREQ_MotherMonIncome = existdata[0].AMST_MotherMonIncome;
                        obj.ASTUREQ_MotherAnnIncome = existdata[0].AMST_MotherAnnIncome;
                        obj.ASTUREQ_MotherNationality = existdata[0].AMST_MotherNationality;


                        obj.ASTUREQ_MotherReligion = existdata[0].AMST_MotherReligion;

                        obj.ASTUREQ_MotherCaste = existdata[0].AMST_MotherCaste;



                        obj.ASTUREQ_MotherSubCaste = existdata[0].AMST_MotherSubCaste;
                        obj.ASTUREQ_MotherBankAccNo = existdata[0].AMST_MotherBankAccNo;
                        obj.ASTUREQ_MotherBankIFSCCode = existdata[0].AMST_MotherBankIFSC_Code;
                        obj.ASTUREQ_MotherCasteCertiNo = existdata[0].AMST_MotherCasteCertiNo;
                        obj.ASTUREQ_MotherPANCardNo = existdata[0].AMST_MotherPANNo;
                        obj.ASTUREQ_TotalIncome = existdata[0].AMST_TotalIncome;
                        obj.ASTUREQ_MotherSign = existdata[0].AMST_Mother_Signature;
                        obj.ASTUREQ_MotherPhoto = existdata[0].ANST_MotherPhoto;
                        obj.ASTUREQ_MotherFingerprint = existdata[0].AMST_Mother_FingerPrint;
                        obj.ASTUREQ_BirthPlace = existdata[0].AMST_BirthPlace;
                        obj.ASTUREQ_Nationality = existdata[0].AMST_Nationality;
                        obj.ASTUREQ_BPLCardFlag = existdata[0].AMST_BPLCardFlag;
                        obj.ASTUREQ_BPLCardNo = existdata[0].AMST_BPLCardNo;
                        obj.ASTUREQ_HostelReqdFlag = existdata[0].AMST_HostelReqdFlag;
                        obj.ASTUREQ_TransportReqdFlag = existdata[0].AMST_TransportReqdFlag;
                        obj.ASTUREQ_GymReqdFlag = existdata[0].AMST_GymReqdFlag;
                        obj.ASTUREQ_ECSFlag = existdata[0].AMST_ECSFlag;
                        obj.ASTUREQ_PaymentFlag = existdata[0].AMST_PaymentFlag;
                        obj.ASTUREQ_AmountPaid = existdata[0].AMST_AmountPaid;
                        obj.ASTUREQ_PaymentType = existdata[0].AMST_PaymentType;
                        obj.ASTUREQ_PaymentDate = existdata[0].AMST_PaymentDate;
                        obj.ASTUREQ_ReceiptNo = existdata[0].AMST_ReceiptNo;
                        //obj.ASTUREQ_EMSINo = existdata[0].AMST_esi;

                        obj.ASTUREQ_FinalpaymentFlag = existdata[0].AMST_FinalpaymentFlag;
                        obj.ASTUREQ_StudentPhoto = existdata[0].AMST_Photoname;
                        //  obj.ASTUREQ_StudentSign = existdata[0].AMST_Studen;
                        // obj.ASTUREQ_StudentFingerprint = existdata[0].AMST_Student;
                        obj.ASTUREQ_NoofSiblingsSchool = existdata[0].AMST_NoOfSiblingsSchool;
                        obj.ASTUREQ_NoofSiblings = existdata[0].AMST_NoOfSiblings;
                        obj.ASTUREQ_NoOfBrothers = existdata[0].AMST_Noofbrothers;
                        obj.ASTUREQ_NoOfSisters = existdata[0].AMST_Noofsisters;
                        obj.ASTUREQ_NoOfElderBrothers = existdata[0].AMST_NoOfElderBrothers;
                        obj.ASTUREQ_NoOfYoungerBrothers = existdata[0].AMST_NoOfYoungerBrothers;
                        obj.ASTUREQ_NoOfElderSisters = existdata[0].AMST_NoOfElderSisters;
                        obj.ASTUREQ_NoOfYoungerSisters = existdata[0].AMST_NoOfYoungerSisters;
                        obj.ASTUREQ_NoOfDependencies = existdata[0].AMST_NoOfDependencies;
                        obj.ASTUREQ_TPINNO = existdata[0].AMST_Tpin;
                        //  obj.ASTUREQ_ConcessionCategory = existdata[0].AMST_Conc;
                        obj.ASTUREQ_MOInstruction = existdata[0].AMST_MOInstruction;
                        obj.ASTUREQ_GPSTrackingId = existdata[0].AMST_GPSTrackingId;
                        obj.ASTUREQ_AppDownloadedDeviceId = existdata[0].AMST_AppDownloadedDeviceId;
                        obj.ASTUREQ_SecretCode = existdata[0].AMST_SecretCode;
                        obj.ASTUREQ_BiometricId = existdata[0].AMST_BiometricId;
                        obj.ASTUREQ_RFCardNo = existdata[0].AMST_RFCardNo;
                        obj.ASTUREQ_FatherChurchAffiliation = existdata[0].AMST_FatherChurchAffiliation;
                        obj.ASTUREQ_MotherChurchAffiliation = existdata[0].AMST_MotherChurchAffiliation;
                        obj.ASTUREQ_FatherSelfEmployedFlg = existdata[0].AMST_FatherSelfEmployedFlg;
                        obj.ASTUREQ_MotherSelfEmployedFlg = existdata[0].AMST_MotherSelfEmployedFlg;
                        obj.ASTUREQ_ChurchBaptisedDate = existdata[0].AMST_ChurchBaptisedDate;

                    }

                    _context.Add(obj);
                    int rx = _context.SaveChanges();
                    if (rx > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = true;
                    }


                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }

        public UpdateRequestDTO savedataadmin(UpdateRequestDTO data)
        {
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

            try
            {
                
                var updateres = _context.Adm_Student_Update_RequestDMO.Single(d => d.AMST_Id == data.AMST_ID && d.ASTUREQ_Id==data.ASTUREQ_Id);

                updateres.ASTUREQ_ApprovedBy = data.User_Id;
                updateres.ASTUREQ_ApprovedFlg = true;
                updateres.ASTUREQ_UpdatedDate = indianTime;
                updateres.ASTUREQ_ReqStatus = "APPROVED";

                _context.Update(updateres);
                //student Main data
                var Studentdata = _context.Adm_M_Student.Single(r => r.AMST_Id == data.AMST_ID);

                Studentdata.AMST_BloodGroup= updateres.ASTUREQ_BloodGroup;
                Studentdata.AMST_PerStreet= updateres.ASTUREQ_PerStreet;
                Studentdata.AMST_PerArea = updateres.ASTUREQ_PerArea;
                Studentdata.AMST_PerCity =  updateres.ASTUREQ_PerCity;
                Studentdata.AMST_PerState = updateres.ASTUREQ_PerState;
                Studentdata.AMST_PerCountry = updateres.IVRMMC_Id;
                Studentdata.AMST_PerPincode =  Convert.ToInt32(updateres.ASTUREQ_PerPincode);
                Studentdata.AMST_ConStreet =  updateres.ASTUREQ_ConStreet;
                Studentdata.AMST_ConArea = updateres.ASTUREQ_ConArea;
                Studentdata.AMST_ConCity = updateres.ASTUREQ_ConCity;
                Studentdata.AMST_ConState =  updateres.ASTUREQ_ConState ;
                Studentdata.AMST_ConCountry =  updateres.ASTUREQ_ConCountryId ;
                Studentdata.AMST_ConPincode = Convert.ToInt32(updateres.ASTUREQ_ConPincode) ;
               Studentdata.UpdatedDate = indianTime;
               Studentdata.AMST_MobileNo = Convert.ToInt64(updateres.ASTUREQ_MobileNo) ;
               Studentdata.AMST_emailId =  updateres.ASTUREQ_EmailId ;
               Studentdata.AMST_FatherMobleNo = updateres.ASTUREQ_FatherMobleNo ;
               Studentdata.AMST_FatheremailId = updateres.ASTUREQ_FatheremailId ;
               Studentdata.AMST_MotherEmailId = updateres.ASTUREQ_MotheremailId ;
                Studentdata.AMST_MotherMobileNo = updateres.ASTUREQ_MotherMobleNo ;
                _context.Update(Studentdata);
                int ctx = _context.SaveChanges();
                if (ctx>0)
                {
                    data.returnval = true;
                    father_mobile_no(updateres);
                    father_email_ids(updateres);
                    mother_mobile_no(updateres);
                    mother_email_id(updateres);
                    student_mobile_no(updateres);
                    student_email_id(updateres);

                    if (updateres.AMSTG_Id !=null && updateres.AMSTG_Id>0)
                    {
                        guardiandetails(updateres);
                    }
                }
                else
                {
                    data.returnval = false;
                }

              

            }
            catch (Exception msg)
            {
                Console.WriteLine(msg.Message);
            }
            return data;
        }



        public UpdateRequestDTO savereject(UpdateRequestDTO data)
        {

            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                var updateres = _context.Adm_Student_Update_RequestDMO.Single(d => d.AMST_Id == data.AMST_ID && d.ASTUREQ_Id == data.ASTUREQ_Id);
                updateres.ASTUREQ_ApprovedBy = data.User_Id;
                updateres.ASTUREQ_UpdatedDate = indianTime;
                updateres.ASTUREQ_ReqStatus = "REJECTED";
                _context.Update(updateres);
                int cx = _context.SaveChanges();
                if (cx>0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex) 
            {

                throw ex;
            }
            return data;
        }
        public Adm_Student_Update_RequestDMO father_mobile_no(Adm_Student_Update_RequestDMO data)
        {
            try
            {
                if (data.ASTUREQ_FatherMobleNo != null)
                {
                    var Phone_Noresultremove = _context.Adm_M_Student_FatherMobileNo.Where(t =>t.AMST_Id == data.AMST_Id).ToArray();
                    if(Phone_Noresultremove.Count()>0)
                    {
                        var Phone_Noresult = _context.Adm_M_Student_FatherMobileNo.Single(t => t.AMSTFMNO_Id == Phone_Noresultremove.FirstOrDefault().AMSTFMNO_Id);
                        Phone_Noresult.UpdatedDate = DateTime.Now;
                        Phone_Noresult.AMST_FatherMobile_No = Convert.ToInt64(data.ASTUREQ_FatherMobleNo);
                        _context.Update(Phone_Noresult);
                        _context.SaveChanges();
                    }
                    else
                    {
                        Adm_M_Student_FatherMobileNo phone1 =new Adm_M_Student_FatherMobileNo();
                        phone1.CreatedDate = DateTime.Now;
                        phone1.UpdatedDate = DateTime.Now;
                        phone1.AMST_FatherMobile_No = Convert.ToInt64(data.ASTUREQ_FatherMobleNo);
                        _context.Add(phone1);
                        _context.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
               
            }

            return data;
        }
        
        //save and update multiple father email ids
        public Adm_Student_Update_RequestDMO father_email_ids(Adm_Student_Update_RequestDMO data)
        {
            try
            {

                if (data.ASTUREQ_FatheremailId != null)
                {
                    var Phone_Noresultremove = _context.Adm_Master_Father_Email.Where(t => t.AMST_Id == data.AMST_Id).ToArray();
                    if (Phone_Noresultremove.Count() > 0)
                    {
                        var Phone_Noresult = _context.Adm_Master_Father_Email.Single(t => t.AMSTFEMAIL_Id == Phone_Noresultremove.FirstOrDefault().AMSTFEMAIL_Id);
                        Phone_Noresult.UpdatedDate = DateTime.Now;
                        Phone_Noresult.AMST_FatheremailId = data.ASTUREQ_FatheremailId;
                        _context.Update(Phone_Noresult);
                        _context.SaveChanges();
                    }
                    else
                    {
                        Adm_Master_Father_Email phone1 = new Adm_Master_Father_Email();
                        phone1.CreatedDate = DateTime.Now;
                        phone1.UpdatedDate = DateTime.Now;
                        phone1.AMST_FatheremailId = data.ASTUREQ_FatheremailId;
                        _context.Add(phone1);
                        _context.SaveChanges();
                    }
                }
               
           

            }
            catch (Exception e)
            {
                
            }
            return data;
        }

        //save and update multiple mother mobile number
        public Adm_Student_Update_RequestDMO mother_mobile_no(Adm_Student_Update_RequestDMO datamobileno)
        {
            try
            {

                if (datamobileno.ASTUREQ_MotherMobleNo != null)
                {
                    var Phone_Noresultremove = _context.Adm_M_Mother_MobileNo.Where(t => t.AMST_Id == datamobileno.AMST_Id).ToArray();
                    if (Phone_Noresultremove.Count() > 0)
                    {
                        var Phone_Noresult = _context.Adm_M_Mother_MobileNo.Single(t => t.AMSTMMNO_Id == Phone_Noresultremove.FirstOrDefault().AMSTMMNO_Id);
                        Phone_Noresult.UpdatedDate = DateTime.Now;
                        Phone_Noresult.AMST_MotherMobileNo = Convert.ToInt64(datamobileno.ASTUREQ_MotherMobleNo);
                        _context.Update(Phone_Noresult);
                        _context.SaveChanges();
                    }
                    else
                    {
                        Adm_M_Mother_MobileNo phone1 = new Adm_M_Mother_MobileNo();
                        phone1.CreatedDate = DateTime.Now;
                        phone1.UpdatedDate = DateTime.Now;
                        phone1.AMST_MotherMobileNo = Convert.ToInt64(datamobileno.ASTUREQ_MotherMobleNo);
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
        public Adm_Student_Update_RequestDMO mother_email_id(Adm_Student_Update_RequestDMO datamobileno)
        {
            try
            {
                if (datamobileno.ASTUREQ_MotheremailId != null )
                {
                    var Phone_Noresultremove = _context.Adm_M_Mother_Emailid.Where(t => t.AMST_Id == datamobileno.AMST_Id).ToArray();
                    if (Phone_Noresultremove.Count() > 0)
                    {
                        var Phone_Noresult = _context.Adm_M_Mother_Emailid.Single(t => t.AMSTMEMAIL_Id == Phone_Noresultremove.FirstOrDefault().AMSTMEMAIL_Id);
                        Phone_Noresult.UpdatedDate = DateTime.Now;
                        Phone_Noresult.AMST_MotheremailId = datamobileno.ASTUREQ_MotheremailId;
                        _context.Update(Phone_Noresult);
                        _context.SaveChanges();
                    }
                    else
                    {
                        Adm_M_Mother_Emailid phone1 = new Adm_M_Mother_Emailid();
                        phone1.CreatedDate = DateTime.Now;
                        phone1.UpdatedDate = DateTime.Now;
                        phone1.AMST_MotheremailId = datamobileno.ASTUREQ_MotheremailId;
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
        public Adm_Student_Update_RequestDMO student_mobile_no(Adm_Student_Update_RequestDMO datamobileno)
        {
            try
            {

                if (datamobileno.ASTUREQ_MobileNo != null)
                {
                    var Phone_Noresultremove = _context.Adm_M_Student_MobileNo.Where(t => t.AMST_Id == datamobileno.AMST_Id).ToArray();
                    if (Phone_Noresultremove.Count() > 0)
                    {
                        var Phone_Noresult = _context.Adm_M_Student_MobileNo.Single(t => t.AMSTSMS_Id == Phone_Noresultremove.FirstOrDefault().AMSTSMS_Id);
                        Phone_Noresult.UpdatedDate = DateTime.Now;
                        Phone_Noresult.AMSTSMS_MobileNo = Convert.ToString(datamobileno.ASTUREQ_MobileNo);
                        _context.Update(Phone_Noresult);
                        _context.SaveChanges();
                    }
                    else
                    {
                        Adm_M_Student_MobileNo phone1 = new Adm_M_Student_MobileNo();
                        phone1.CreatedDate = DateTime.Now;
                        phone1.UpdatedDate = DateTime.Now;
                        phone1.AMSTSMS_MobileNo = Convert.ToString(datamobileno.ASTUREQ_MobileNo);
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
        public Adm_Student_Update_RequestDMO student_email_id(Adm_Student_Update_RequestDMO datamobileno)
        {
            try
            {

                if (datamobileno.ASTUREQ_EmailId != null)
                {
                    var Phone_Noresultremove = _context.Adm_M_Student_Email_Id.Where(t => t.AMST_Id == datamobileno.AMST_Id).ToArray();
                    if (Phone_Noresultremove.Count() > 0)
                    {
                        var Phone_Noresult = _context.Adm_M_Student_Email_Id.Single(t => t.AMSTE_EmailId == Phone_Noresultremove.FirstOrDefault().AMSTE_EmailId);
                        Phone_Noresult.UpdatedDate = DateTime.Now;
                        Phone_Noresult.AMSTE_EmailId = datamobileno.ASTUREQ_EmailId;
                        _context.Update(Phone_Noresult);
                        _context.SaveChanges();
                    }
                    else
                    {
                        Adm_M_Student_Email_Id phone1 = new Adm_M_Student_Email_Id();
                        phone1.CreatedDate = DateTime.Now;
                        phone1.UpdatedDate = DateTime.Now;
                        phone1.AMSTE_EmailId = datamobileno.ASTUREQ_EmailId;
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


        public Adm_Student_Update_RequestDMO guardiandetails(Adm_Student_Update_RequestDMO data)
        {
            try
            {

                if (data.AMSTG_Id != null && data.AMSTG_Id >0)
                {
                    var Phone_Noresultremove = _context.StudentGuardianDMO.Where(t => t.AMST_Id == data.AMST_Id && t.AMSTG_Id== data.AMSTG_Id).ToArray();
                    if (Phone_Noresultremove.Count() > 0)
                    {
                        var Phone_Noresult = _context.StudentGuardianDMO.Single(t => t.AMST_Id == data.AMST_Id && t.AMSTG_Id == data.AMSTG_Id);
                        Phone_Noresult.UpdatedDate = DateTime.Now;
                        Phone_Noresult.AMSTG_emailid = data.ASTUREQ_GuardianEmailId;
                        Phone_Noresult.AMSTG_GuardianPhoneNo = data.ASTUREQ_GuardianMobileNo;
                        _context.Update(Phone_Noresult);
                        _context.SaveChanges();
                    }
                 
                }

            }
            catch (Exception e)
            {

            }

            return data;
        }


    }
}
