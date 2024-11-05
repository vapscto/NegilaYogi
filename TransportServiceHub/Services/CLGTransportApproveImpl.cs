using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using DomainModel.Model.com.vapstech.Transport;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Services
{
    public class CLGTransportApproveImpl : Interfaces.CLGTransportApproveInterface
    {
        public TransportContext _TransportContext;
        public ILogger<CLGTransportApproveDTO> _log;
        private readonly DomainModelMsSqlServerContext _db;
        public CLGTransportApproveImpl(TransportContext _context, ILogger<CLGTransportApproveDTO> log, DomainModelMsSqlServerContext db)
        {
            _TransportContext = _context;
            _log = log;
            _db = db;
        }

        public CLGTransportApproveDTO getdata(CLGTransportApproveDTO data)
        {
            try
            {
                data.getaccyear = _TransportContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true ).OrderByDescending(t=>t.ASMAY_Order).ToArray();
                data.getcourse = _TransportContext.MasterCourseDMO.Where(a => a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true).ToArray();
                data.routename = _TransportContext.MasterRouteDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMR_ActiveFlg == true).OrderBy(x=>x.TRMR_order).ToArray();

                //added Praveen(01/18/2019)

                data.picsesslist = _TransportContext.MsterSessionDMO.Where(f => f.MI_Id == data.MI_Id && f.TRMS_ActiveFlg == true && f.TRMS_Flag == "Pick Up").Distinct().ToArray();
                data.drpsesslist = _TransportContext.MsterSessionDMO.Where(f => f.MI_Id == data.MI_Id && f.TRMS_ActiveFlg == true && f.TRMS_Flag == "Drop").Distinct().ToArray();


                //End Praveen(01/18/2019)

                using (var cmd = _TransportContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Student_TransportApproval_Details_Collage";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                        SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });   


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
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
                        data.getalldetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

             //   data.routename = _TransportContext.MasterRouteDMO.Where(e => e.MI_Id == data.MI_Id).Distinct().ToArray();

                data.routename = (from a in _TransportContext.Route_Location
                                  from b in _TransportContext.MasterRouteDMO
                                  where (a.TRMR_Id == b.TRMR_Id && b.MI_Id == data.MI_Id)
                                   select new CLGTransportApproveDTO
                                   {
                                       TRMRL_Id=a.TRMRL_Id,
                                       TRML_Id=a.TRML_Id,
                                       TRMR_Id =a.TRMR_Id,
                                       TRMR_RouteName =b.TRMR_RouteName

                                   }).Distinct().OrderBy(m => m.TRMR_RouteName).ToArray();



                data.getdetails = (from a in _TransportContext.Adm_Master_College_StudentDMO
                                   from b in _TransportContext.CLGAdm_Std_Transport_ApplicationDMO
                                   from c in _TransportContext.MasterAreaDMO
                                   from d in _TransportContext.AcademicYearDMO
                                   where (a.AMCST_Id == b.AMCST_Id && b.TRMA_Id == c.TRMA_Id && b.MI_Id == data.MI_Id
                                   && b.ASTACO_ApplStatus != "Waiting" && b.ASTACO_ForAY==d.ASMAY_Id && d.Is_Active==true && d.ASMAY_Id== data.ASMAY_Id && a.AMCST_SOL=="S" && a.AMCST_ActiveFlag==true)
                                   select new CLGTransportApproveDTO
                                   {
                                       studentname = ((a.AMCST_FirstName == null ? "" : a.AMCST_FirstName) +" " +(a.AMCST_MiddleName == null ? "" : " " + a.AMCST_MiddleName)+" " + (a.AMCST_LastName == null ? "" : " " + a.AMCST_LastName)).Trim(),
                                       areaname = c.TRMA_AreaName,
                                       AMCST_Id = b.AMCST_Id,
                                       ASTACO_Id = b.ASTACO_Id,
                                       FASMAY_Id = b.ASTACO_ForAY,
                                       applicationno = b.ASTACO_ApplicationNo,
                                       pickuproute = b.ASTACO_PickUp_TRMR_Id != 0 ? _TransportContext.MasterRouteDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMR_Id == b.ASTACO_PickUp_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",
                                       pickuplocation = b.ASTACO_PickUp_TRML_Id != 0 ? _TransportContext.MasterLocationDMO.Where(l => l.MI_Id == data.MI_Id && l.TRML_Id == b.ASTACO_PickUp_TRML_Id).FirstOrDefault().TRML_LocationName : "--",
                                       drouproute = b.ASTACO_Drop_TRMR_Id != 0 ? _TransportContext.MasterRouteDMO.Where(td => td.MI_Id == data.MI_Id && td.TRMR_Id == b.ASTACO_PickUp_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",
                                       drouplocation = b.ASTACO_Drop_TRML_Id != 0 ? _TransportContext.MasterLocationDMO.Where(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == b.ASTACO_Drop_TRML_Id).FirstOrDefault().TRML_LocationName : "--",
                                       ASTA_ApplStatus = b.ASTACO_ApplStatus,
                                       ASMAY_Year = d.ASMAY_Year,
                                       ASMAY_Order = d.ASMAY_Order,

                                   }).Distinct().OrderByDescending(m=>m.ASMAY_Order).ToArray();

                data.logoheader = (from a in _TransportContext.FeeMasterConfigurationDMO
                                   where (a.MI_Id == data.MI_Id && a.userid == 364)
                                   select new CLGTransportApproveDTO
                                   {
                                       logopath = a.MI_Logo,
                                   }
       ).ToList().ToArray();

            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Approved Form getdata :" + ex.Message);
            }
            return data;
        }


        public CLGTransportApproveDTO gridaconchange(CLGTransportApproveDTO data)
        {

            try
            {
                data.getdetails = (from a in _TransportContext.Adm_Master_College_StudentDMO
                                   from b in _TransportContext.CLGAdm_Std_Transport_ApplicationDMO
                                   from c in _TransportContext.MasterAreaDMO
                                   from d in _TransportContext.AcademicYearDMO
                                   where (a.AMCST_Id == b.AMCST_Id && b.TRMA_Id == c.TRMA_Id && b.MI_Id == data.MI_Id
                                   && b.ASTACO_Amount > 0 && b.ASTACO_ApplStatus != "Waiting" && b.ASTACO_ForAY == d.ASMAY_Id && d.Is_Active == true && d.ASMAY_Id == data.ASMAY_Id && a.AMCST_SOL == "S" && a.AMCST_ActiveFlag == true)
                                   select new CLGTransportApproveDTO
                                   {
                                       studentname = ((a.AMCST_FirstName == null ? "" : a.AMCST_FirstName) + " "+(a.AMCST_MiddleName == null ? "" : " " + a.AMCST_MiddleName) +" "+ (a.AMCST_LastName == null ? "" : " " + a.AMCST_LastName)).Trim(),
                                       areaname = c.TRMA_AreaName,
                                       AMCST_Id = b.AMCST_Id,
                                       ASTACO_Id = b.ASTACO_Id,
                                       FASMAY_Id = b.ASTACO_ForAY,
                                       applicationno = b.ASTACO_ApplicationNo,
                                       pickuproute = b.ASTACO_PickUp_TRMR_Id != 0 ? _TransportContext.MasterRouteDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMR_Id == b.ASTACO_PickUp_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",
                                       pickuplocation = b.ASTACO_PickUp_TRML_Id != 0 ? _TransportContext.MasterLocationDMO.Where(l => l.MI_Id == data.MI_Id && l.TRML_Id == b.ASTACO_PickUp_TRML_Id).FirstOrDefault().TRML_LocationName : "--",
                                       drouproute = b.ASTACO_Drop_TRMR_Id != 0 ? _TransportContext.MasterRouteDMO.Where(td => td.MI_Id == data.MI_Id && td.TRMR_Id == b.ASTACO_PickUp_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",
                                       drouplocation = b.ASTACO_Drop_TRML_Id != 0 ? _TransportContext.MasterLocationDMO.Where(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == b.ASTACO_Drop_TRML_Id).FirstOrDefault().TRML_LocationName : "--",
                                       ASTA_ApplStatus = b.ASTACO_ApplStatus,
                                       ASMAY_Year = d.ASMAY_Year,
                                       ASMAY_Order = d.ASMAY_Order,

                                   }).Distinct().OrderByDescending(m => m.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }
        public CLGTransportApproveDTO editapprove(CLGTransportApproveDTO data)
        {
            try
            {

                if (data.Temp_Save_List.Length>0)
                {
        
                    int sucesscount = 0;
                    int failedcount = 0;
                    foreach (var st  in data.Temp_Save_List)
                    {
                        var lst = _TransportContext.CLGAdm_Std_Transport_ApplicationDMO.Where(w => w.MI_Id == data.MI_Id && w.ASTACO_Id == st.ASTACO_Id && w.ASTACO_ApplStatus == "Approved").ToList();
                        if (lst.Count>0)
                        {
                            var regstatus = _TransportContext.Database.ExecuteSqlCommand("rejectedlist_Collage @p0,@p1,@p2,@p3", st.AMCST_Id, data.MI_Id, st.FASMAY_Id, data.userId
                                                            );
                            if (Convert.ToInt32(regstatus) > 0)
                            {
                                sucesscount += 1;
                            }
                            else
                            {
                                failedcount += 1;
                            }

                            if (sucesscount > 0)
                            {
                                if (sucesscount == data.Temp_Save_List.Length)
                                {
                                    data.message = "Record Updated Sucessfully";
                                }
                                else
                                {
                                    data.message = "Record Updated Sucessfully  Sucess Count " + sucesscount + "And Failed Count " + failedcount + "";
                                }
                            }
                            else
                            {
                                data.message = "Record Not Updated Sucessfully";
                            }
                        }
                        
                        
                       
                    }
                  
                }
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }
        public CLGTransportApproveDTO CancelRejection(CLGTransportApproveDTO data)
        {
            try
            {

                if (data.Temp_Save_List.Length>0)
                {
        
                    int sucesscount = 0;
                    int failedcount = 0;
                    foreach (var st  in data.Temp_Save_List)
                    {

                        var rejlist = _TransportContext.CLGAdm_Std_Transport_ApplicationDMO.Where(e => e.MI_Id == data.MI_Id && e.ASTACO_Id == st.ASTACO_Id && e.ASTACO_ApplStatus == "Rejected").ToList();
                        if (rejlist.Count>0)
                        {
                            var rejlist1 = _TransportContext.CLGAdm_Std_Transport_ApplicationDMO.Single(e => e.MI_Id == data.MI_Id && e.ASTACO_Id == st.ASTACO_Id && e.ASTACO_ApplStatus == "Rejected");
                            rejlist1.UpdatedDate = DateTime.Now;
                            rejlist1.ASTACO_ApplStatus = "Waiting";
                            rejlist1.ASTACO_ActiveFlag = true;
                            _TransportContext.Update(rejlist1);

                            int regstatus = _TransportContext.SaveChanges();

                            if (Convert.ToInt32(regstatus) > 0)
                            {
                                sucesscount += 1;
                            }
                            else
                            {
                                failedcount += 1;
                            }

                        }

                       
                        
                    }
                    if (sucesscount > 0)
                    {
                        if (sucesscount == data.Temp_Save_List.Length)
                        {
                            data.message = "Record Updated Sucessfully";
                        }
                        else
                        {
                            data.message = "Record Updated Sucessfully  Sucess Count " + sucesscount + "And Failed Count " + failedcount + "";
                        }
                    }
                    else
                    {
                        data.message = "Record Not Updated Sucessfully";
                    }
                }
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }

        public CLGTransportApproveDTO searchdetails(CLGTransportApproveDTO data)
        {
            try
            {

                string semidd = "0";

                foreach (var item in data.AMSE_Idss)
                {
                    semidd = semidd + "," + item.AMSE_Id.ToString();
                }


                using (var cmd = _TransportContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TransportApproval_Details_Collage";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                        SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@asmay_id",
                       SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMCO_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMB_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                       SqlDbType.VarChar)
                    {
                        Value = semidd
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASTACO_Regnew",
                       SqlDbType.VarChar)
                    {
                        Value = data.RegularNew
                    });
                    cmd.Parameters.Add(new SqlParameter("@RouteId",
                       SqlDbType.VarChar)
                    {
                        Value = data.TRMR_Id
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
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
                        data.getalldetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                data.routename = _TransportContext.MasterRouteDMO.Where(e => e.MI_Id == data.MI_Id).Distinct().ToArray();



            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Aprroved searchdetails :" + ex.Message);
                _log.LogError(ex.Message);
            }
            return data;
        }
        public async Task<CLGTransportApproveDTO> showmodaldetails(CLGTransportApproveDTO data)
        {
            try
            {
                var studentcurrentyear = (from a in _TransportContext.Adm_College_Yearly_StudentDMO
                                          where (a.AMCST_Id == data.AMCST_Id)
                                          select a
       ).ToList().OrderByDescending(d => d.ACYST_Id).ToArray();

                if (studentcurrentyear.Length > 0)
                {
                    if (studentcurrentyear.FirstOrDefault().ASMAY_Id != data.ASMAY_Id)
                    {
                        data.studentaccyear = studentcurrentyear.FirstOrDefault().ASMAY_Id;
                    }
                    else
                    {
                        data.studentaccyear = data.ASMAY_Id;

                    }

                }

                else
                {
                    var studentadmityear = (from a in _TransportContext.Adm_Master_College_StudentDMO
                                            where (a.AMCST_Id == data.AMCST_Id)
                                            select a
                 ).ToList().ToArray();


                    if (studentadmityear.FirstOrDefault().ASMAY_Id != data.ASMAY_Id)
                    {
                        data.studentaccyear = studentadmityear.FirstOrDefault().ASMAY_Id;
                    }
                    else
                    {
                        data.studentaccyear = data.ASMAY_Id;
                    }
                }

                using (var cmd = _TransportContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "BUSPASS_FORM_DETAILS_COLLAGE";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@amst",
                SqlDbType.VarChar)
                    {
                        Value = data.AMCST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@asta",
              SqlDbType.VarChar)
                    {
                        Value = data.ASTACO_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@mi_id",
              SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@asmay_id",
           SqlDbType.BigInt)
                    {
                        Value = data.studentaccyear
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
                        data.studentdetails = retObject.ToArray();
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

        public CLGTransportApproveDTO savelist(CLGTransportApproveDTO data)
        {
            try
            {
                int sucesscount = 0;
                int failedcount = 0;

                if (data.Flag == "A")
                {

                    for (int i = 0; i < data.Temp_Save_List.Length; i++)
                    {
                        long amstid = data.Temp_Save_List[i].AMCST_Id;
                        long fasmayid = data.Temp_Save_List[i].FASMAY_Id;
                        long astaid = data.Temp_Save_List[i].ASTACO_Id;
                        long picksession = data.Temp_Save_List[i].PickUp_Session;
                        long dropsession = data.Temp_Save_List[i].Drop_Session;
                        CLGTransportApprovedDMO approved = new CLGTransportApprovedDMO();
                        try
                        {
                            var confirmstatus = _TransportContext.Database.ExecuteSqlCommand("CLG_Auto_Fee_Group_Mapping_Transport @p0,@p1,@p2,@p3",
                                data.MI_Id, fasmayid, amstid, data.userId);

                            if (Convert.ToInt32(confirmstatus) >= 0)
                            {
                                var update = _TransportContext.CLGAdm_Std_Transport_ApplicationDMO.Single(a => a.MI_Id == data.MI_Id && a.AMCST_Id == amstid && a.ASTACO_Id == astaid);
                                update.ASTACO_ApplStatus = "Approved";
                                update.UpdatedDate = DateTime.Now;

                                //added Praveen
                                update.ASTACO_PickUp_TRMS_Id = picksession;
                                update.ASTACO_Drop_TRMS_Id = dropsession;
                                //End Praveen
                                _TransportContext.Update(update);

                            var duplicateapp = _TransportContext.CLGTransportApprovedDMO.Where(w => w.ASTACO_Id == astaid).ToList();
                            if (duplicateapp.Count>0)
                            {
                                foreach (var item in duplicateapp)
                                {
                                    _TransportContext.Remove(item);
                                }
                            }


                                approved.ASTACO_Id = astaid;
                                approved.IVRMUL_Id = data.userId;
                                approved.ASTAACO_Date = DateTime.Now;
                                approved.CreatedDate = DateTime.Now;
                                approved.UpdatedDate = DateTime.Now;
                                _TransportContext.Add(approved);
                                var ks = _TransportContext.SaveChanges();
                                if (ks > 0)
                                {

                                    var studDet = _TransportContext.CLGAdm_Std_Transport_ApplicationDMO.Where(t => t.MI_Id == data.MI_Id && t.AMCST_Id == amstid && t.ASTACO_Id== astaid).ToList();
                                    var studDett = _TransportContext.Adm_Master_College_StudentDMO.Where(t => t.MI_Id == data.MI_Id && t.AMCST_Id == amstid).ToList();


                                    SMS sms = new SMS(_db);
                                    string s = sms.sendSms(data.MI_Id, Convert.ToInt64(studDet.FirstOrDefault().ASTACO_PickupSMSMobileNo), "TRANSPORT_APPROVED", amstid).Result;


                                    Email Email = new Email(_db);
                                    string m = Email.sendmail(data.MI_Id, studDett.FirstOrDefault().AMCST_emailId, "TRANSPORT_APPROVED", amstid);

                                    var update1 = _TransportContext.CLGAdm_Std_Transport_ApplicationDMO.Single(a => a.MI_Id == data.MI_Id && a.AMCST_Id == amstid && a.ASTACO_Id == astaid);

                                    var fmgidlist = (from a in _TransportContext.FeeGroupDMO
                                                     from b in _TransportContext.Fee_College_Master_Student_GroupHeadDMO
                                                     where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.FMG_ActiceFlag == true && a.FMG_CompulsoryFlag == "T" && a.FMG_Id == b.FMG_Id && b.ASMAY_Id == fasmayid && b.AMCST_Id == amstid
                                                     select new CLGTransportApproveDTO
                                                     {
                                                         FMG_Id = a.FMG_Id
                                                     }).Distinct().ToList();



                                    var stu_rec_list = _TransportContext.CLGStudentRouteMappingDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == fasmayid && t.AMCST_Id == amstid).ToList();
                                    if (stu_rec_list.Count > 0)
                                    {
                                        var feegrplist = _TransportContext.CLGStudentRouteFeeGroupDMO.Where(t => t.TRSRCO_Id == stu_rec_list[0].TRSRCO_Id).ToList();
                                        foreach (var delff in feegrplist)
                                        {
                                            _TransportContext.Remove(delff);
                                        }

                                        foreach (var del_stu in stu_rec_list)
                                        {
                                            _TransportContext.Remove(del_stu);
                                        }

                                        _TransportContext.SaveChanges();
                                    }
                                    CLGStudentRouteMappingDMO object123 = new CLGStudentRouteMappingDMO();
                                    object123.MI_Id = data.MI_Id;
                                    object123.ASMAY_Id = update1.ASTACO_ForAY;
                                    object123.AMCST_Id = update1.AMCST_Id;
                                    object123.TRSRCO_Date = DateTime.Now.Date;
                                    object123.TRSRCO_PickUpRoute = update1.ASTACO_PickUp_TRMR_Id;

                                    object123.TRRSCO_PickUpLocation = update1.ASTACO_PickUp_TRML_Id;
                                  
                                        object123.TRRSCO_PickUpMobileNo = update1.ASTACO_PickupSMSMobileNo;
                                   
                                    object123.TRSRCO_DropRoute = update1.ASTACO_Drop_TRMR_Id;
                                 
                                    object123.TRRSCO_DropLocation = update1.ASTACO_Drop_TRML_Id;
                                    object123.TRRSCO_DropMobileNo = update1.ASTACO_DropSMSMobileNo;
                                    object123.TRRSCO_ApplicationNo = update1.ASTACO_ApplicationNo;
                                    object123.TRSRCO_PickupSession = update1.ASTACO_PickUp_TRMS_Id; 
                                    object123.TRSRCO_DropSession = update1.ASTACO_Drop_TRMS_Id;
                                    object123.TRRSCO_ActiveFlg = true;
                                    object123.CreatedDate = DateTime.Now;
                                    object123.UpdatedDate = DateTime.Now;
                                    object123.ASTACO_Id = update1.ASTACO_Id;
                                    _TransportContext.Add(object123);

                                    foreach (var x in fmgidlist)
                                    {
                                        CLGStudentRouteFeeGroupDMO oobj = new CLGStudentRouteFeeGroupDMO();
                                        oobj.TRSRCO_Id = object123.TRSRCO_Id;
                                        oobj.FMG_Id = x.FMG_Id;
                                        oobj.TRSRFGCO_ActiveFlg = true;
                                        _TransportContext.Add(oobj);
                                    }


                                    _TransportContext.SaveChanges();


                                    sucesscount = sucesscount + 1;
                                }
                                else
                                {
                                    failedcount = failedcount + 1;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _log.LogError(ex.Message);
                        }
                    }
                    if (sucesscount > 0)
                    {
                        if (sucesscount == data.Temp_Save_List.Length)
                        {
                            data.message = "Record Saved Sucessfully";
                        }
                        else
                        {
                            data.message = "Record Saved Sucessfully  Sucess Count " + sucesscount + "And Failed Count " + failedcount + "";
                        }
                    }
                    else
                    {
                        data.message = "Record Not Saved Sucessfully";
                    }


                    //else if (failedcount != 0)
                    //{
                    //    data.message = "Record Saved Sucessfully And Failed Count " + failedcount;
                    //}

                }
                else
                {
                    string changedStudentData = "";
                    string studentremarkemail = "";
                    long amstid = 0;
                    long astaid = 0;
                    for (int i = 0; i < data.Temp_Save_List.Length; i++)
                    {
                        try
                        {
                             amstid = data.Temp_Save_List[i].AMCST_Id;
                            long fasmayid = data.Temp_Save_List[i].FASMAY_Id;
                             astaid = data.Temp_Save_List[i].ASTACO_Id;
                           
                            for (int j = 0; j < data.data_array.Count(); j++)
                            {
                                if (data.data_array[j].ASTACO_Id== astaid)
                                {
                                    if (data.data_array[j].remarks != null)
                                    {
                                        if (data.data_array[j].remarks.ToString() != "")
                                        {
                                            changedStudentData = data.data_array[j].remarks.ToString();
                                          //  studentremarkemail = data.data_array[j].studentremarkemail.ToString();
                                          
                                        }
                                        if (data.data_array[j].studentremarkemail.ToString() != "")
                                        {
                                           // changedStudentData = data.data_array[j].remarks.ToString();
                                            studentremarkemail = data.data_array[j].studentremarkemail.ToString();

                                        }
                                    }
                                }
                            }

                            var update = _TransportContext.CLGAdm_Std_Transport_ApplicationDMO.Single(a => a.MI_Id == data.MI_Id && a.AMCST_Id == amstid && a.ASTACO_Id == astaid);
                            update.ASTACO_ApplStatus = "Rejected";
                            update.ASTACO_ActiveFlag = false;

                            if (changedStudentData=="" || changedStudentData == null)
                            {
                                update.ASTACO_Remarks = changedStudentData;
                            }
                            else
                            {
                                update.ASTACO_Remarks = studentremarkemail;
                            }
                           
                            update.UpdatedDate = DateTime.Now;
                            _TransportContext.Update(update);
                            var kl = _TransportContext.SaveChanges();
                            if (kl > 0)
                            {

                                Dictionary<string, string> smsemail = new Dictionary<string, string>();
                                smsemail.Add("MESSAGE", studentremarkemail);
                                data.smsemailarry = smsemail.ToArray();

                                var studDet = _TransportContext.CLGAdm_Std_Transport_ApplicationDMO.Where(t => t.MI_Id == data.MI_Id && t.AMCST_Id == amstid).ToList();
                                var studDett = _TransportContext.Adm_Master_College_StudentDMO.Where(t => t.MI_Id == data.MI_Id && t.AMCST_Id == amstid).ToList();

                                if (data.emailcheck == true)
                                {
                          
                                    Email Email = new Email(_db);
                                    Email.sendmailtransreject(data.MI_Id, "TRN-REJECT", smsemail, studDett.FirstOrDefault().AMCST_emailId, "Transport Status");
                                }
                                if (data.smscheck == true)
                                {
                                   
                                    SMS sms = new SMS(_db);
                                    string s = sms.sendSms(data.MI_Id, Convert.ToInt64(studDet.FirstOrDefault().ASTACO_PickupSMSMobileNo), astaid, "Transport Status", changedStudentData).Result;
                                }

                                //SMS sms = new SMS(_db);
                                //string s = sms.sendSms(data.MI_Id, Convert.ToInt64(studDet.FirstOrDefault().ASTA_FatherMobileNo), "TRN-REJECT",astaid).Result;
                                sucesscount = sucesscount + 1;
                            }
                            else
                            {
                                failedcount = failedcount + 1;
                            }
                        }
                        catch (Exception ex)
                        {
                            _log.LogInformation("Transport form  rejected " + ex.Message);
                        }
                    }
                    if (sucesscount > 0)
                    {
                        if (sucesscount == data.Temp_Save_List.Length)
                        {
                           

                            data.message = "Record Saved Sucessfully";
                        }
                        else
                        {
                            data.message = "Record Saved Sucessfully  Sucess Count " + sucesscount + "And Failed Count " + failedcount + "";
                        }
                    }
                    else
                    {
                        data.message = "Record Not Saved Sucessfully";
                    }
                }
                
                 var feeconfiguration = _TransportContext.FeeMasterConfigurationDMO.Where(t => t.MI_Id == data.MI_Id).FirstOrDefault();

                if (feeconfiguration.FMC_Areawise_FeeFlg == 1)
                {
                    try
                    {
                        foreach (var std in data.Temp_Save_List)
                        {
                            var MapHostelFee = _TransportContext.Database.ExecuteSqlCommand("CLG_Auto_Fee_Group_Mapping_Transport_AreaRequest @p0,@p1,@p2,@p3", data.MI_Id, data.ASMAY_Id, std.AMCST_Id, data.userId);
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                if (feeconfiguration.FMC_CommonTransportAreaFeeFlg == true)
                {
                    try
                    {
                        foreach (var std in data.Temp_Save_List)
                        {
                            var MapHostelFee = _TransportContext.Database.ExecuteSqlCommand("CLG_Auto_Fee_Group_Mapping_Transport_AreaRequest @p0,@p1,@p2,@p3", data.MI_Id, data.ASMAY_Id, std.AMCST_Id, data.userId);
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                if (feeconfiguration.FMC_TransportFeeLocationFlag == true)
                {
                    try
                    {
                        foreach (var std in data.Temp_Save_List)
                        {
                            var MapHostelFee = _TransportContext.Database.ExecuteSqlCommand("CLG_Auto_Fee_Group_Mapping_Transport @p0,@p1,@p2,@p3", data.MI_Id, data.ASMAY_Id, std.AMCST_Id, data.userId);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                if (feeconfiguration.FMC_CommonTransportLocationFeeFlg == true)
                {
                    try
                    {
                        foreach (var std in data.Temp_Save_List)
                        {
                            var MapHostelFee = _TransportContext.Database.ExecuteSqlCommand("CLG_Auto_Fee_Group_Mapping_Transport_LocationRequest @p0,@p1,@p2,@p3", data.MI_Id, data.ASMAY_Id, std.AMCST_Id, data.userId);
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Approved Form Savelist :" + ex.Message);
            }
            return data;
        }
    }
}
