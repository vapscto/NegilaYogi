using DataAccessMsSqlServerProvider;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Portals.IVRS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Configuration;
using DomainModel.Model.com.vaps.admission;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vapstech.Portals.IVRS;
using AutoMapper;
using DomainModel.Model.com.vapstech.Portals.Student;
using DomainModel.Model;

namespace PortalHub.com.vaps.IVRS.Services
{
    public class IVRSImpl : Interfaces.IVRSInterface
    {
        public DomainModelMsSqlServerContext _db;
        public PortalContext _ivrs;
        public IVRSImpl(DomainModelMsSqlServerContext db, PortalContext ivrs)
        {
            _db = db;
            _ivrs = ivrs;
        }
        public JsonResult getdata(IVRSDTO data)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            var retObject1 = new List<dynamic>();

            List<string> operation_list = new List<string>();
            if (data.IIVRSC_SchoolFlg == 1)
            {
                var query = _db.Adm_Master_College_StudentDMO.Where(q => q.AMCST_TPINNO == data.IVRA_TPIN && q.MI_Id == data.MI_Id).ToList();
                if (query.Count > 0)
                {
                    var miid = query.FirstOrDefault().MI_Id.ToString();
                    try
                    {
                        if (data.exeId != null)
                        {

                            operation_list.Add("Exam_Marks_Overall");

                        }
                        else
                        {
                            operation_list.Add("get_stu_details");
                            operation_list.Add("get_student_fee");
                            operation_list.Add("get_student_ExamList");
                            operation_list.Add("get_student_coe");
                            operation_list.Add("Attendance");
                            operation_list.Add("Homework");
                            operation_list.Add("Librarydue");
                            operation_list.Add("Updatemobilenumber");
                            operation_list.Add("Routedetails");
                            data.exeId = "0";
                        }

                        for (int i = 0; i < operation_list.Count; i++)
                        {

                            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                            {
                                if (data.IVRA_TPIN != "" || data.IVRA_TPIN != null)
                                {
                                    cmd.CommandText = "CLG_IVRS_FLOW";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@operation",
                                        SqlDbType.VarChar)
                                    {
                                        Value = operation_list[i]
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@Tpin",
                                       SqlDbType.VarChar)
                                    {
                                        Value = data.IVRA_TPIN
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@exeid",
                                     SqlDbType.VarChar)
                                    {
                                        Value = data.exeId
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@miid",
                                    SqlDbType.VarChar)
                                    {
                                        Value = data.MI_Id
                                    });

                                    if (cmd.Connection.State != ConnectionState.Open)
                                        cmd.Connection.Open();

                                    try
                                    {
                                        using (var dataReader1 = cmd.ExecuteReader())
                                        {
                                            while (dataReader1.Read())
                                            {

                                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                                dataRow1.Add("Operation", operation_list[i]);
                                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                                {
                                                    dataRow1.Add(
                                                        dataReader1.GetName(iFiled1),
                                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                                                    );
                                                }
                                                retObject1.Add((ExpandoObject)dataRow1);

                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    //  Dictionary<string, object> dict = new Dictionary<string, object>();
                    for (int j = 0; j < operation_list.Count; j++)
                    {
                        var stud_det = ((IEnumerable)retObject1).Cast<dynamic>()
                                .Where(p => p.Operation == operation_list[j]).ToList();
                        if (stud_det.Count > 0)
                        {
                            dict.Add(operation_list[j], stud_det);
                        }
                        else
                        {
                            dict.Add(operation_list[j], "No Record Found");
                        }

                    }

                }
                else
                {
                    dict.Add("Message", "Invalid TPIN");
                }
            }
            else
            {
                var query = _db.Adm_M_Student.Where(q => q.AMST_Tpin == data.IVRA_TPIN && q.MI_Id == data.MI_Id).ToList();
                if (query.Count > 0)
                {
                    var miid = query.FirstOrDefault().MI_Id.ToString();
                    try
                    {
                        if (data.exeId != null)
                        {

                            operation_list.Add("Exam_Marks_Overall");

                        }
                        else
                        {
                            operation_list.Add("get_stu_details");
                            operation_list.Add("get_student_fee");
                            operation_list.Add("get_student_ExamList");
                            operation_list.Add("get_student_coe");
                            operation_list.Add("Attendance");
                            operation_list.Add("Homework");
                            operation_list.Add("Librarydue");
                            operation_list.Add("Updatemobilenumber");
                            operation_list.Add("Routedetails");
                            data.exeId = "0";
                        }

                        for (int i = 0; i < operation_list.Count; i++)
                        {

                            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                            {
                                if (data.IVRA_TPIN != "" || data.IVRA_TPIN != null)
                                {
                                    cmd.CommandText = "IVRS_FLOW";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@operation",
                                        SqlDbType.VarChar)
                                    {
                                        Value = operation_list[i]
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@Tpin",
                                       SqlDbType.VarChar)
                                    {
                                        Value = data.IVRA_TPIN
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@exeid",
                                     SqlDbType.VarChar)
                                    {
                                        Value = data.exeId
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@miid",
                                    SqlDbType.VarChar)
                                    {
                                        Value = data.MI_Id
                                    });

                                    if (cmd.Connection.State != ConnectionState.Open)
                                        cmd.Connection.Open();


                                    try
                                    {

                                        using (var dataReader1 = cmd.ExecuteReader())
                                        {
                                            while (dataReader1.Read())
                                            {

                                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                                dataRow1.Add("Operation", operation_list[i]);
                                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                                {
                                                    dataRow1.Add(
                                                        dataReader1.GetName(iFiled1),
                                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                                                    );
                                                }
                                                retObject1.Add((ExpandoObject)dataRow1);

                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    //  Dictionary<string, object> dict = new Dictionary<string, object>();
                    for (int j = 0; j < operation_list.Count; j++)
                    {
                        var stud_det = ((IEnumerable)retObject1).Cast<dynamic>()
                                .Where(p => p.Operation == operation_list[j]).ToList();
                        if (stud_det.Count > 0)
                        {
                            dict.Add(operation_list[j], stud_det);
                        }
                        else
                        {
                            dict.Add(operation_list[j], "No Record Found");
                        }

                    }

                }
                else
                {
                    dict.Add("Message", "Invalid TPIN");
                }
            }
            return new JsonResult(dict);
        }
        public JsonResult getbranch(IVRSDTO data)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            var retObject1 = new List<dynamic>();
            List<string> operation_list = new List<string>();


            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {

                cmd.CommandText = "IVRS_GET_BRANCHES";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@vno", SqlDbType.VarChar) { Value = data.vno });

                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                try
                {

                    using (var dataReader1 = cmd.ExecuteReader())
                    {
                        while (dataReader1.Read())
                        {

                            var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                            for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                            {
                                dataRow1.Add(
                                    dataReader1.GetName(iFiled1),
                                    dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                                );
                            }
                            retObject1.Add((ExpandoObject)dataRow1);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                var stud_det = ((IEnumerable)retObject1).Cast<dynamic>().ToList();
                dict.Add("1", stud_det);
            }
            return new JsonResult(dict);

        }
        public JsonResult updatecredits(IVRSDTO data)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            var institutionName = _ivrs.IVRM_IVRS_ConfigurationDMO.Where(i => i.IIVRSC_MI_Id == data.IMCS_MI_Id && i.IIVRSC_VirtualNo == data.IMCS_VirtualNo).Select(g => g.IIVRSC_SchoolName).FirstOrDefault();
            var result = _ivrs.IVRS_Call_StatusDMO.Single(t => t.IMCS_MI_Id == data.IMCS_MI_Id && t.IMCS_VirtualNo == data.IMCS_VirtualNo);
            if (data.IMCD_InOutFlg == "Inbound")
            {
                result.IMCS_InboundCalls = result.IMCS_InboundCalls + data.IMCD_PulseCount;
            }
            else if (data.IMCD_InOutFlg == "Outbound")
            {
                result.IMCS_OutboundCalls = result.IMCS_OutboundCalls + data.IMCD_PulseCount;
            }
            result.IMCS_AvailableCalls = result.IMCS_AvailableCalls - data.IMCD_PulseCount;
            _ivrs.Update(result);
            var contactExists = _ivrs.SaveChanges();
            if (contactExists == 1)
            {
                data.returnMsg = "sucess";

                var urlget = _ivrs.IVRM_IVRS_ConfigurationDMO.Where(t => t.IIVRSC_MI_Id == data.IMCS_MI_Id && t.IIVRSC_VirtualNo == data.IMCS_VirtualNo).Select(d => d.IIVRSC_URL).FirstOrDefault();

                IVRS_Call_DetailsDMO cald = new IVRS_Call_DetailsDMO();
                cald.IMCD_VirtualNo = data.IMCS_VirtualNo;
                cald.IMCD_MI_Id = data.IMCS_MI_Id;
                cald.IMCD_SchoolName = institutionName;
                cald.IMCD_URL = urlget;
                cald.IMCD_MobileNo = data.IMCD_MobileNo;
                cald.IMCD_InOutFlg = data.IMCD_InOutFlg;
                cald.IMCD_DateTime = DateTime.Now;
                cald.IMCD_CallStatus = data.IMCD_CallStatus;
                cald.IMCD_CallDuration = data.IMCD_CallDuration;
                cald.IMCD_PulseCount = data.IMCD_PulseCount;
                cald.IMCD_IVRSText = data.IMCD_IVRSText;
                cald.IMCD_IVRSVoiceURL = data.IMCD_IVRSVoiceURL;
                cald.IMCD_CreatedBy = 1;
                cald.IMCD_UpdatedBy = 1;
                cald.IMCD_ActiveFlg = true;
                cald.CreatedDate = DateTime.Now;
                cald.UpdatedDate = DateTime.Now;
                _ivrs.Add(cald);
                var contactExists1 = _ivrs.SaveChanges();
                if (contactExists1 == 1)
                {
                    dict.Add("Message", "sucess");
                    data.returnMsg = "sucess";
                }
            }
            else
            {
                data.returnMsg = "failed";
                dict.Add("Message", "failed");
            }

            return new JsonResult(dict);
        }
        public JsonResult UpdateMobile(IVRSDTO data)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            // Adm_M_Student enq = new Adm_M_Student();
            StudentDetailsupdateDMO enq = new StudentDetailsupdateDMO();
            var virtualno = _db.IVRS_ConfigurationDMO.Single(t => t.IIVRSC_VirtualNo == data.IMCS_VirtualNo);

            var result = _db.Adm_M_Student.Where(t => t.AMST_Tpin == data.IVRA_TPIN && t.MI_Id == virtualno.IIVRSC_MI_Id).ToList();
            var result1 = _db.StudentDetailsupdateDMO.Where(t => t.AMST_ID == result[0].AMST_Id).ToList();

            if (result1.Count() == 0)
            {
                if (data.TypeOfMobile == "Father")
                {
                    enq.STP_FMOBILENO = data.IMCD_MobileNo;

                    enq.AMST_ID = result[0].AMST_Id;
                    enq.STP_SNAME = result[0].AMST_FirstName;

                }
                else if (data.TypeOfMobile == "Mother")
                {
                    enq.AMST_ID = result[0].AMST_Id;
                    enq.STP_SNAME = result[0].AMST_FirstName;

                    enq.STP_MMOBILENO = data.IMCD_MobileNo;
                }
                else if (data.TypeOfMobile == "Student")
                {
                    enq.AMST_ID = result[0].AMST_Id;
                    enq.STP_SNAME = result[0].AMST_FirstName;

                    enq.STP_SMOBILENO = data.IMCD_MobileNo;
                }
                _db.Add(enq);
            }
            else if (result1.Count() > 0)
            {
                var resultstu = _db.StudentDetailsupdateDMO.Single(t => t.AMST_ID == result[0].AMST_Id);

                if (data.TypeOfMobile == "Father")
                {
                    resultstu.STP_FMOBILENO = data.IMCD_MobileNo;
                }
                else if (data.TypeOfMobile == "Mother")
                {
                    resultstu.STP_MMOBILENO = data.IMCD_MobileNo;
                }
                else if (data.TypeOfMobile == "Student")
                {
                    resultstu.STP_SMOBILENO = data.IMCD_MobileNo;
                }
                _db.Update(resultstu);
            }


            var contactExists = _db.SaveChanges();
            if (contactExists == 1)
            {
                dict.Add("Message", "sucess");
                data.returnMsg = "sucess";
            }
            else
            {
                dict.Add("Message", "failed");
                data.returnMsg = "failed";
            }
            return new JsonResult(dict);
        }
        public IVRM_IVRS_ConfigurationDTO savedetail(IVRM_IVRS_ConfigurationDTO data)
        {
            IVRM_IVRS_ConfigurationDMO master = Mapper.Map<IVRM_IVRS_ConfigurationDMO>(data);
            if (data.IIVRSC_Id != 0)
            {
                var institutionName = _ivrs.IVRM_IVRS_ConfigurationDMO.Where(i => i.IIVRSC_MI_Id == data.IIVRSC_MI_Id).Select(g => g.IIVRSC_SchoolName).FirstOrDefault();
                var result = _ivrs.IVRM_IVRS_ConfigurationDMO.Single(t => t.IIVRSC_Id == data.IIVRSC_Id);
                result.IIVRSC_PerMonthCall = data.IIVRSC_PerMonthCall;
                result.IIVRSC_CallCharge = data.IIVRSC_CallCharge;
                result.IIVRSC_SchoolName = institutionName;
                result.IIVRSC_URL = data.IIVRSC_URL;
                result.IVRS_MOBILE_URL = data.IVRS_MOBILE_URL;
                result.IVRS_UPDATE_URL = data.IVRS_UPDATE_URL;
                result.IIVRSC_VirtualNo = data.IIVRSC_VirtualNo;
                result.IIVRSC_VFORTTSFlg = data.IIVRSC_VFORTTSFlg;
                result.IIVRSC_MI_Id = data.IIVRSC_MI_Id;
                result.IIVRSC_ActiveFlg = true;
                result.UpdatedDate = DateTime.Now;
                _ivrs.Update(result);
                var contactExists = _ivrs.SaveChanges();
                if (contactExists == 1)
                {
                    data.returnval = true;
                    //one time saving
                    var result2 = _ivrs.IVRS_Call_StatusDMO.Single(t => t.IMCS_MI_Id == data.IIVRSC_MI_Id && t.IMCS_SchoolName == institutionName && t.IMCS_VirtualNo == data.IIVRSC_VirtualNo);
                    IVRS_Call_StatusDMO ne = new IVRS_Call_StatusDMO();
                    result2.IMCS_MI_Id = data.IIVRSC_MI_Id;
                    result2.IMCS_SchoolName = institutionName;
                    result2.IMCS_VirtualNo = data.IIVRSC_VirtualNo;
                    result2.IMCS_URL = data.IIVRSC_URL;
                    result2.IMCS_Year = data.ASMAY_ID;
                    result2.IMCS_Month = data.ivrM_Month_Name;
                    result2.IMCS_AssignedCall = data.IIVRSC_PerMonthCall;
                    result2.IMCS_InboundCalls = 0;
                    result2.IMCS_OutboundCalls = 0;
                    result2.IMCS_AvailableCalls = data.IIVRSC_PerMonthCall;
                    result2.IMCS_ActiveFlg = true;
                    result2.UpdatedDate = DateTime.Now;
                    _ivrs.Add(ne);
                    var contactExists1 = _ivrs.SaveChanges();
                    if (contactExists1 == 1)
                    {
                        data.returnval = true;
                    }
                }
                else
                {
                    data.returnval = false;
                }
            }
            else
            {
                var res = _ivrs.IVRM_IVRS_ConfigurationDMO.Where(t => t.IIVRSC_MI_Id == data.IIVRSC_MI_Id).ToList();
                if (res.Count > 0)
                {
                    data.returnduplicatestatus = "Duplicate";
                }
                else
                {
                    var institutionName = _ivrs.IVRM_IVRS_ConfigurationDMO.Where(i => i.IIVRSC_MI_Id == data.IIVRSC_MI_Id).Select(g => g.IIVRSC_SchoolName).FirstOrDefault();
                    master.IIVRSC_SchoolName = institutionName;
                    master.CreatedDate = DateTime.Now;
                    master.UpdatedDate = DateTime.Now;
                    master.IIVRSC_ActiveFlg = true;
                    _ivrs.Add(master);
                    var contactExists = _ivrs.SaveChanges();
                    if (contactExists == 1)
                    {
                        data.returnval = true;
                        //one time saving
                        IVRS_Call_StatusDMO ne = new IVRS_Call_StatusDMO();
                        ne.IMCS_MI_Id = data.IIVRSC_MI_Id;
                        ne.IMCS_SchoolName = institutionName;
                        ne.IMCS_VirtualNo = data.IIVRSC_VirtualNo;
                        ne.IMCS_URL = data.IIVRSC_URL;
                        ne.IMCS_Year = data.ASMAY_ID;
                        ne.IMCS_Month = data.ivrM_Month_Name;
                        ne.IMCS_AssignedCall = data.IIVRSC_PerMonthCall;
                        ne.IMCS_InboundCalls = 0;
                        ne.IMCS_OutboundCalls = 0;
                        ne.IMCS_AvailableCalls = data.IIVRSC_PerMonthCall;
                        ne.IMCS_ActiveFlg = true;
                        ne.CreatedDate = DateTime.Now;
                        ne.UpdatedDate = DateTime.Now;
                        _ivrs.Add(ne);
                        var contactExists1 = _ivrs.SaveChanges();
                        if (contactExists1 == 1)
                        {
                            data.returnval = true;
                        }
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            return data;
        }
        public IVRSDTO getdetails(IVRSDTO data)
        {
            data.institute = _db.Institution.Where(t => t.MI_ActiveFlag == 1).ToArray();
            data.monthdropdown = _db.month.Where(t => t.Is_Active == true).ToArray();
            data.maindata = _ivrs.IVRM_IVRS_ConfigurationDMO.ToArray();
            //data.yearlist = _db.AcademicYear.Where(t=> t.MI_Id == data.MI_Id).ToArray();


            //List<MasterAcademic> defaultyear = new List<MasterAcademic>();
            //defaultyear = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true && t.ASMAY_Id == data.ASMAY_Id).ToList();
            //data.academicListdefault = defaultyear.OrderByDescending(a => a.ASMAY_Order).ToArray();

            try
            {
                var query = _ivrs.IVRM_IVRS_ConfigurationDMO.Where(q => q.IIVRSC_VirtualNo == data.IMCS_VirtualNo).FirstOrDefault();

                var retObject1 = new List<dynamic>();
                using (var cmd = _ivrs.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Get_due_Current_Due_Amount";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.VarChar, 100)
                    {
                        Value = query.IIVRSC_MI_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    try
                    {
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                                    );
                                }

                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getcurdueamount = retObject1.ToString();
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
        public IVRSDTO getpageedit(int id)
        {
            IVRSDTO TTMC = new IVRSDTO();
            try
            {
                TTMC.yearlist = _db.AcademicYear.Where(t => t.MI_Id == id && t.Is_Active == true).ToList().ToArray();

                TTMC.ASMAY_Id = _db.AcademicYear.Single(a => a.MI_Id == id && a.ASMAY_From_Date <= DateTime.Now && a.ASMAY_To_Date >= DateTime.Now).ASMAY_Id;

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return TTMC;

        }
        public IVRM_IVRS_ConfigurationDTO getdetails_page(int id)
        {
            IVRM_IVRS_ConfigurationDTO TTMC = new IVRM_IVRS_ConfigurationDTO();
            try
            {
                TTMC.maindata_grid = _ivrs.IVRM_IVRS_ConfigurationDMO.Where(t => t.IIVRSC_Id == id).ToArray();
                var mi = _ivrs.IVRM_IVRS_ConfigurationDMO.Where(t => t.IIVRSC_Id == id).ToArray();
                var result2 = _ivrs.IVRS_Call_StatusDMO.Single(t => t.IMCS_MI_Id == mi.Select(n => n.IIVRSC_MI_Id).FirstOrDefault() && t.IMCS_VirtualNo == mi.Select(n => n.IIVRSC_VirtualNo).FirstOrDefault());
                TTMC.ASMAY_ID = result2.IMCS_Year;
                TTMC.ivrM_Month_Name = result2.IMCS_Month;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return TTMC;

        }
        public IVRM_IVRS_ConfigurationDTO deactivate(IVRM_IVRS_ConfigurationDTO acd)
        {
            try
            {
                IVRM_IVRS_ConfigurationDMO pge = Mapper.Map<IVRM_IVRS_ConfigurationDMO>(acd);
                if (pge.IIVRSC_Id > 0)
                {
                    var result = _ivrs.IVRM_IVRS_ConfigurationDMO.Single(t => t.IIVRSC_Id.Equals(pge.IIVRSC_Id));
                    if (result.IIVRSC_ActiveFlg.Equals(true))
                    {
                        result.IIVRSC_ActiveFlg = false;
                    }
                    else
                    {
                        result.IIVRSC_ActiveFlg = true;
                    }
                    _ivrs.Update(result);
                    var flag = _ivrs.SaveChanges();
                    if (flag.Equals(1))
                    {
                        acd.returnval = true;
                    }
                    else
                    {
                        acd.returnval = false;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return acd;
        }

        //============notification stu/staff

        public IVRM_IVRS_ConfigurationDTO student_staff_notification(IVRM_IVRS_ConfigurationDTO dto)
        {
            try
            {
                DateTime today = DateTime.Today;
                DateTime DaysEarlier = today.AddDays(-30);

                var appid = _ivrs.ApplicationUser.Single(a => a.Id == dto.userid);

                var userrole = _ivrs.ApplicationUserRole.Single(a => a.UserId == appid.Id);
                var rolytype = _ivrs.IVRM_Role_Type.Single(a => a.IVRMRT_Id == userrole.RoleTypeId);
                var login_id = _ivrs.StudentAppUserLoginDMO_con.Where(a => a.STD_APP_ID == dto.userid).ToList();
                if (rolytype.IVRMRT_RoleFlag == "STUDENT")
                {
                    dto.notification_stu_stf_list = (from a in _ivrs.PN_Sent_Details_DMO
                                                     from b in _ivrs.PN_Sent_Details_Student_DMO
                                                     where a.PNSD_Id == b.PNSD_Id && b.AMST_Id == login_id[0].AMST_ID
                                                     && a.MI_Id == dto.MI_Id && Convert.ToDateTime(a.PNSD_SentDate) >= DaysEarlier
                                                     select a).OrderByDescending(q => q.PNSD_Id).ToArray();

                }
                else if (rolytype.IVRMRT_RoleFlag == "Staff")
                {

                }



            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return dto;
        }
    }
}
