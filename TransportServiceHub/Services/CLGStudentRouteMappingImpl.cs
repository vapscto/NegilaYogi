using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using DomainModel.Model.com.vapstech.Transport;
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
    public class CLGStudentRouteMappingImpl : Interfaces.CLGStudentRouteMappingInterface
    {
        public TransportContext _trncontext;
        ILogger<CLGStudentRouteMappingImpl> _areaimpl;
        //      public DomainModelMsSqlServerContext _db;


        // parameterized constructor
        public CLGStudentRouteMappingImpl(ILogger<CLGStudentRouteMappingImpl> areaimpl, TransportContext context)
        {

            _areaimpl = areaimpl;
            _trncontext = context;

        }

        public CLGStudentRouteMappingDTO getdata(CLGStudentRouteMappingDTO data)
        {

            try
            {

                data.yealist = _trncontext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

                data.monthdropdown = _trncontext.month.ToArray();

                data.sectionlist = _trncontext.Adm_College_Master_SectionDMO.Where(t => t.MI_Id == data.MI_Id && t.ACMS_ActiveFlag == true).Distinct().OrderBy(t => t.ACMS_Order).ToArray();


                data.routelist = _trncontext.MasterRouteDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMR_ActiveFlg == true).OrderBy(t => t.TRMR_order).ToArray();


                data.grouplist = (from a in _trncontext.FeeHeadDMO
                                  from b in _trncontext.FeeYearlygroupHeadMappingDMO
                                  from c in _trncontext.FeeGroupDMO
                                  where (a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && a.FMH_ActiveFlag == true && a.FMH_Flag == "T" && a.FMH_Id == b.FMH_Id && b.FMG_Id == c.FMG_Id && c.FMG_ActiceFlag == true && b.FYGHM_ActiveFlag == "1")
                                  select c).Distinct().ToArray();

                data.picsesslist = _trncontext.MsterSessionDMO.Where(f => f.MI_Id == data.MI_Id && f.TRMS_ActiveFlg == true && f.TRMS_Flag == "Pick Up").Distinct().ToArray();
                data.drpsesslist = _trncontext.MsterSessionDMO.Where(f => f.MI_Id == data.MI_Id && f.TRMS_ActiveFlg == true && f.TRMS_Flag == "Drop").Distinct().ToArray();



                using (var cmd = _trncontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TR_CLG_GETSTUDENTROUTEMAP";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@Mi_Id",
                       SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(data.MI_Id)
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
                        data.reportdatelist = retObject.ToArray();


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                using (var cmd = _trncontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Student_BusPass_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@Mi_Id",
                       SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(data.MI_Id)
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
                        data.Buspasslist = retObject.ToArray();


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }





                //List<School_M_Class> allclas = new List<School_M_Class>();
                //allclas = _trncontext.admissioncls.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                //data.classlist = allclas.Distinct().ToArray();









                //var locationlist = _trncontext.MasterLocationDMO.Where(a => a.MI_Id == data.MI_Id && a.TRML_ActiveFlg == true).Distinct().ToList();
                //data.locationlist = locationlist.ToArray();

                //List<TR_Route_ScheduleDMO> schedule = new List<TR_Route_ScheduleDMO>();
                //schedule = _trncontext.TR_Route_ScheduleDMO.Where(t => t.MI_Id == data.MI_Id && t.TRRSC_ActiveFlag == true).ToList();
                //data.schedulelist = schedule.ToArray();


                //data.grouplist = (from a in _trncontext.FeeHeadDMO
                //                  from b in _trncontext.FeeYearlygroupHeadMappingDMO
                //                  from c in _trncontext.FeeGroupDMO
                //                  where (a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && a.FMH_ActiveFlag == true && a.FMH_Flag == "T" && a.FMH_Id == b.FMH_Id && b.FMG_Id == c.FMG_Id && c.FMG_ActiceFlag == true && b.FYGHM_ActiveFlag == "1")
                //                  select c).Distinct().ToArray();


                //data.routelist = _trncontext.MasterRouteDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMR_ActiveFlg == true).OrderBy(t => t.TRMR_order).ToArray();



                //data.picsesslist = _trncontext.MsterSessionDMO.Where(f => f.MI_Id == data.MI_Id && f.TRMS_ActiveFlg == true && f.TRMS_Flag == "Pick Up").Distinct().ToArray();
                //data.drpsesslist = _trncontext.MsterSessionDMO.Where(f => f.MI_Id == data.MI_Id && f.TRMS_ActiveFlg == true && f.TRMS_Flag == "Drop").Distinct().ToArray();
            }
            catch (Exception ex)
            {
                _areaimpl.LogInformation("Transport Error Master Area getdata" + ex.Message);
            }
            return data;
        }
        public CLGStudentRouteMappingDTO savedata_backup(CLGStudentRouteMappingDTO data)
        {
            try
            {
                if (data.studenttype == "class_wise")
                {
                    if (data.savetmpdata.Length > 0)
                    {
                        foreach (var std in data.savetmpdata)
                        {
                            foreach (var fee in data.some_data)
                            {
                                if (std.AMCST_Id == fee.amcsT_Id)
                                {

                                    var stu_rec_list = _trncontext.CLGStudentRouteMappingDMO.Where(t => t.MI_Id == data.MI_Id
                                    && t.ASMAY_Id == data.ASMAY_Id && t.AMCST_Id == std.AMCST_Id).ToList();

                                    if (stu_rec_list.Count > 0)
                                    {
                                        foreach (var stdrt in stu_rec_list)
                                        {
                                            var feegrplist = _trncontext.CLGStudentRouteFeeGroupDMO.Where(t => t.TRSRCO_Id == stdrt.TRSRCO_Id).ToList();
                                            if (feegrplist.Count > 0)
                                            {
                                                foreach (var stdfee in feegrplist)
                                                {
                                                    _trncontext.Remove(stdfee);
                                                }
                                            }

                                            _trncontext.Remove(stdrt);
                                        }
                                    }

                                    CLGStudentRouteMappingDMO object123 = new CLGStudentRouteMappingDMO();
                                    object123.MI_Id = data.MI_Id;
                                    object123.ASMAY_Id = data.ASMAY_Id;
                                    object123.AMCST_Id = std.AMCST_Id;
                                    object123.TRSRCO_Date = data.TRSRCO_Date;
                                    object123.TRSRCO_PickUpRoute = std.TRSRCO_PickUpRoute;
                                    object123.TRRSCO_PickUpLocation = std.TRRSCO_PickUpLocation;
                                    object123.TRRSCO_PickUpMobileNo = fee.TRRSCO_PickUpMobileNo > 0 ? fee.TRRSCO_PickUpMobileNo : null;
                                    object123.TRSRCO_DropRoute = std.TRSRCO_DropRoute;
                                    object123.TRRSCO_DropLocation = std.TRRSCO_DropLocation;
                                    object123.TRRSCO_DropMobileNo = fee.TRRSCO_DropMobileNo > 0 ? fee.TRRSCO_DropMobileNo : null;
                                    object123.TRRSCO_ApplicationNo = fee.TRRSCO_ApplicationNo;
                                    object123.TRSRCO_PickupSession = std.PickUp_Session;
                                    object123.TRSRCO_DropSession = std.Drop_Session;
                                    object123.TRRSCO_ActiveFlg = true;
                                    object123.CreatedDate = DateTime.Now;
                                    object123.UpdatedDate = DateTime.Now;
                                    object123.ASTACO_Id = std.ASTACO_Id == 0 ? null : std.ASTACO_Id;
                                    _trncontext.Add(object123);

                                    foreach (var x in fee.grp_list)
                                    {
                                        CLGStudentRouteFeeGroupDMO oobj = new CLGStudentRouteFeeGroupDMO();
                                        oobj.TRSRCO_Id = object123.TRSRCO_Id;
                                        oobj.FMG_Id = x.TRML_Id;
                                        oobj.TRSRFGCO_ActiveFlg = true;
                                        oobj.CreatedDate = DateTime.Now;
                                        oobj.UpdatedDate = DateTime.Now;
                                        _trncontext.Add(oobj);
                                    }
                                }
                            }
                        }
                    }

                    var exists = _trncontext.SaveChanges();
                    if (exists >= 1)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }

                }
                else if (data.studenttype == "route_wise")
                {
                    if (data.savetmpdata.Length > 0 && data.some_data.Length > 0)
                    {
                        foreach (var std in data.savetmpdata)
                        {

                            foreach (var fee in data.some_data)
                            {
                                if (std.AMCST_Id == fee.amcsT_Id)
                                {

                                    var stu_rec_list = _trncontext.CLGStudentRouteMappingDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMCST_Id == std.AMCST_Id).ToList();

                                    if (stu_rec_list.Count > 0)
                                    {
                                        foreach (var stdrt in stu_rec_list)
                                        {
                                            var feegrplist = _trncontext.CLGStudentRouteFeeGroupDMO.Where(t => t.TRSRCO_Id == stdrt.TRSRCO_Id).ToList();
                                            if (feegrplist.Count > 0)
                                            {
                                                foreach (var stdfee in feegrplist)
                                                {
                                                    _trncontext.Remove(stdfee);
                                                }
                                            }

                                            _trncontext.Remove(stdrt);
                                        }

                                    }



                                    CLGStudentRouteMappingDMO object123 = new CLGStudentRouteMappingDMO();
                                    object123.MI_Id = data.MI_Id;
                                    object123.ASMAY_Id = data.ASMAY_Id;
                                    object123.AMCST_Id = std.AMCST_Id;
                                    object123.TRSRCO_Date = data.TRSRCO_Date;
                                    object123.TRSRCO_PickUpRoute = data.TRSRCO_PickUpRoute;
                                    object123.TRRSCO_PickUpLocation = data.TRRSCO_PickUpLocation;
                                    object123.TRRSCO_PickUpMobileNo = fee.TRRSCO_PickUpMobileNo;
                                    object123.TRSRCO_DropRoute = data.TRSRCO_DropRoute;
                                    object123.TRRSCO_DropLocation = data.TRRSCO_DropLocation;
                                    object123.TRRSCO_DropMobileNo = fee.TRRSCO_DropMobileNo;
                                    object123.TRRSCO_ApplicationNo = fee.TRRSCO_ApplicationNo;
                                    object123.TRSRCO_PickupSession = data.PickUp_Session;
                                    object123.TRSRCO_DropSession = data.Drop_Session;
                                    object123.TRRSCO_ActiveFlg = true;
                                    object123.CreatedDate = DateTime.Now;
                                    object123.UpdatedDate = DateTime.Now;
                                    object123.ASTACO_Id = std.ASTACO_Id == 0 ? null : std.ASTACO_Id;
                                    _trncontext.Add(object123);


                                    foreach (var x in fee.grp_list)
                                    {
                                        CLGStudentRouteFeeGroupDMO oobj = new CLGStudentRouteFeeGroupDMO();
                                        oobj.TRSRCO_Id = object123.TRSRCO_Id;
                                        oobj.FMG_Id = x.TRML_Id;
                                        oobj.TRSRFGCO_ActiveFlg = true;
                                        oobj.CreatedDate = DateTime.Now;
                                        oobj.UpdatedDate = DateTime.Now;
                                        _trncontext.Add(oobj);
                                    }


                                }
                            }



                        }
                    }

                    var exists = _trncontext.SaveChanges();
                    if (exists >= 1)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }

                }
                //for (int i = 0; i < data.savetmpdata.Length; i++)
                //{
                //    for (int j = 0; j < data.some_data.Length; j++)
                //    {
                //        if (data.savetmpdata[i].AMCST_Id == data.some_data[j].amsT_Id)
                //        {

                //            var stu_rec_list = _trncontext.CLGStudentRouteMappingDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMST_Id == data.savetmpdata[i].AMCST_Id).ToList();
                //            if (stu_rec_list.Count > 0)
                //            {
                //                var feegrplist = _trncontext.TR_Student_Route_FeeGroupDMO.Where(t => t.TRSR_Id == stu_rec_list[0].TRSR_Id).ToList();
                //                foreach (var delff in feegrplist)
                //                {
                //                    _trncontext.Remove(delff);
                //                }
                //            }


                //            foreach (var del_stu in stu_rec_list)
                //            {
                //                _trncontext.Remove(del_stu);
                //            }


                //            CLGStudentRouteMappingDMO object123 = new CLGStudentRouteMappingDMO();
                //            object123.MI_Id = data.MI_Id;
                //            object123.ASMAY_Id = data.ASMAY_Id;
                //            object123.AMST_Id = data.savetmpdata[i].AMST_Id;
                //            object123.TRSR_Date = data.TRSR_Date;
                //            //object123.FMG_Id = x.TRML_Id;
                //            object123.TRMR_Id = data.savetmpdata[i].TRMR_Id;
                //            object123.TRSR_PickupSchedule = data.savetmpdata[i].TRSR_PickupSchedule;
                //            object123.TRSR_PickUpLocation = data.savetmpdata[i].TRSR_PickUpLocation;
                //            object123.TRSR_PickUpMobileNo = data.some_data[j].TRSR_PickUpMobileNo;
                //            object123.TRMR_Drop_Route = data.savetmpdata[i].TRMR_Drop_Route;
                //            object123.TRSR_DropSchedule = data.savetmpdata[i].TRSR_DropSchedule;
                //            object123.TRSR_DropLocation = data.savetmpdata[i].TRSR_DropLocation;
                //            object123.TRSR_DropMobileNo = data.some_data[j].TRSR_DropMobileNo;
                //            object123.TRSR_ApplicationNo = data.some_data[j].TRSR_ApplicationNo;
                //            object123.TRSR_PickupSession = data.savetmpdata[i].PickUp_Session;
                //            object123.TRSR_DropSession = data.savetmpdata[i].Drop_Session;
                //            object123.TRSR_ActiveFlg = true;
                //            object123.CreatedDate = DateTime.Now;
                //            object123.UpdatedDate = DateTime.Now;
                //            object123.ASTA_Id = data.savetmpdata[i].ASTA_Id == 0 ? null : data.savetmpdata[i].ASTA_Id;
                //            _trncontext.Add(object123);
                //            _trncontext.SaveChanges();
                //            foreach (var x in data.some_data[j].grp_list)
                //            {
                //                TR_Student_Route_FeeGroupDMO oobj = new TR_Student_Route_FeeGroupDMO();
                //                oobj.TRSR_Id = object123.TRSR_Id;
                //                oobj.FMG_Id = x.TRML_Id;
                //                oobj.TRSRFG_ActiveFlg = true;
                //                _trncontext.Add(oobj);
                //            }

                //        }
                //    }
                //}
                //var exists = _trncontext.SaveChanges();
                //if (exists >= 1)
                //{
                //    data.returnval = true;
                //}
                //else
                //{
                //    data.returnval = false;
                //}

                //else if (stu.studenttype == "route_wise")
                //{
                //    for (int i = 0; i < stu.savetmpdata.Length; i++)
                //    {

                //        for (int j = 0; j < stu.some_data.Length; j++)
                //        {
                //            if (stu.savetmpdata[i].AMST_Id == stu.some_data[j].amsT_Id)
                //            {

                //                var stu_rec_list = _trncontext.CLGStudentRouteMappingDMO.Where(t => t.MI_Id == stu.MI_Id && t.ASMAY_Id == stu.ASMAY_Id && t.AMST_Id == stu.savetmpdata[i].AMST_Id).ToList();

                //                foreach (var del_stu in stu_rec_list)
                //                {
                //                    _trncontext.Remove(del_stu);
                //                }

                //                CLGStudentRouteMappingDMO object123 = new CLGStudentRouteMappingDMO();
                //                object123.MI_Id = stu.MI_Id;
                //                object123.ASMAY_Id = stu.ASMAY_Id;
                //                object123.AMST_Id = stu.savetmpdata[i].AMST_Id;
                //                object123.TRSR_Date = stu.TRSR_Date;
                //                // object123.FMG_Id = x.TRML_Id;
                //                object123.TRMR_Id = stu.trmR_Id_pic;
                //                object123.TRSR_PickupSchedule = stu.savetmpdata[i].TRSR_PickupSchedule;
                //                object123.TRSR_PickUpLocation = stu.TRSR_PickUpLocation;
                //                object123.TRSR_PickUpMobileNo = stu.some_data[j].TRSR_PickUpMobileNo;
                //                object123.TRMR_Drop_Route = stu.trmR_Id_drp;
                //                object123.TRSR_DropSchedule = stu.savetmpdata[i].TRSR_DropSchedule;
                //                object123.TRSR_DropLocation = stu.TRSR_DropLocation;
                //                object123.TRSR_DropMobileNo = stu.some_data[j].TRSR_DropMobileNo;
                //                object123.TRSR_ApplicationNo = stu.some_data[j].TRSR_ApplicationNo;
                //                object123.TRSR_PickupSession = stu.PickUp_Session;
                //                object123.TRSR_DropSession = stu.Drop_Session;

                //                object123.TRSR_ActiveFlg = true;
                //                object123.CreatedDate = DateTime.Now;
                //                object123.UpdatedDate = DateTime.Now;
                //                object123.ASTA_Id = stu.savetmpdata[i].ASTA_Id == 0 ? null : stu.savetmpdata[i].ASTA_Id;
                //                _trncontext.Add(object123);
                //                _trncontext.SaveChanges();
                //                foreach (var x in stu.some_data[j].grp_list)
                //                {
                //                    TR_Student_Route_FeeGroupDMO oobj = new TR_Student_Route_FeeGroupDMO();
                //                    oobj.TRSR_Id = object123.TRSR_Id;
                //                    oobj.FMG_Id = x.TRML_Id;
                //                    oobj.TRSRFG_ActiveFlg = true;
                //                    _trncontext.Add(oobj);
                //                }


                //            }
                //        }
                //    }
                //    var exists = _trncontext.SaveChanges();
                //    if (exists >= 1)
                //    {
                //        stu.returnval = true;
                //    }
                //    else
                //    {
                //        stu.returnval = false;
                //    }
                //}

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }
        public CLGStudentRouteMappingDTO savedata(CLGStudentRouteMappingDTO data)
        {
            try
            {
                if (data.studenttype == "class_wise")
                {
                    if (data.savetmpdata != null && data.savetmpdata.Length > 0)
                    {
                        foreach (var std in data.savetmpdata)
                        {
                            var stu_rec_list = _trncontext.CLGStudentRouteMappingDMO.Where(t => t.MI_Id == data.MI_Id
                                  && t.ASMAY_Id == data.ASMAY_Id && t.AMCST_Id == std.AMCST_Id).ToList();

                            if (stu_rec_list.Count > 0)
                            {
                                foreach (var stdrt in stu_rec_list)
                                {
                                    var feegrplist = _trncontext.CLGStudentRouteFeeGroupDMO.Where(t => t.TRSRCO_Id == stdrt.TRSRCO_Id).ToList();
                                    if (feegrplist.Count > 0)
                                    {
                                        foreach (var stdfee in feegrplist)
                                        {
                                            _trncontext.Remove(stdfee);
                                        }
                                    }

                                    _trncontext.Remove(stdrt);
                                }
                            }

                            CLGStudentRouteMappingDMO object123 = new CLGStudentRouteMappingDMO
                            {
                                MI_Id = data.MI_Id,
                                ASMAY_Id = data.ASMAY_Id,
                                AMCST_Id = std.AMCST_Id,
                                TRSRCO_Date = data.TRSRCO_Date,
                                TRSRCO_PickUpRoute = std.TRSRCO_PickUpRoute,
                                TRRSCO_PickUpLocation = std.TRRSCO_PickUpLocation,
                                TRSRCO_DropRoute = std.TRSRCO_DropRoute,
                                TRRSCO_DropLocation = std.TRRSCO_DropLocation,
                                TRSRCO_PickupSession = std.PickUp_Session,
                                TRSRCO_DropSession = std.Drop_Session,
                                TRRSCO_ActiveFlg = true,
                                CreatedDate = DateTime.Now,
                                UpdatedDate = DateTime.Now,
                                ASTACO_Id = std.ASTACO_Id == 0 ? null : std.ASTACO_Id,
                                TRRSCO_PickUpMobileNo = std.TRRSCO_PickUpMobileNo > 0 ? std.TRRSCO_PickUpMobileNo : null,
                                TRRSCO_DropMobileNo = std.TRRSCO_DropMobileNo > 0 ? std.TRRSCO_DropMobileNo : null,
                                TRRSCO_ApplicationNo = std.TRRSCO_ApplicationNo
                            };
                            _trncontext.Add(object123);

                            if (data.some_data != null && data.some_data.Length > 0)
                            {
                                foreach (var fee in data.some_data)
                                {
                                    if (std.AMCST_Id == fee.amcsT_Id)
                                    {
                                        if (fee.grp_list != null && fee.grp_list.Length > 0)
                                        {
                                            foreach (var x in fee.grp_list)
                                            {
                                                CLGStudentRouteFeeGroupDMO oobj = new CLGStudentRouteFeeGroupDMO
                                                {
                                                    TRSRCO_Id = object123.TRSRCO_Id,
                                                    FMG_Id = x.TRML_Id,
                                                    TRSRFGCO_ActiveFlg = true,
                                                    CreatedDate = DateTime.Now,
                                                    UpdatedDate = DateTime.Now
                                                };
                                                _trncontext.Add(oobj);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var exists = _trncontext.SaveChanges();
                    if (exists >= 1)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }

                }

                else if (data.studenttype == "route_wise")
                {
                    if (data.savetmpdata != null && data.savetmpdata.Length > 0)
                    {
                        foreach (var std in data.savetmpdata)
                        {
                            var stu_rec_list = _trncontext.CLGStudentRouteMappingDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMCST_Id == std.AMCST_Id).ToList();

                            if (stu_rec_list.Count > 0)
                            {
                                foreach (var stdrt in stu_rec_list)
                                {
                                    var feegrplist = _trncontext.CLGStudentRouteFeeGroupDMO.Where(t => t.TRSRCO_Id == stdrt.TRSRCO_Id).ToList();
                                    if (feegrplist.Count > 0)
                                    {
                                        foreach (var stdfee in feegrplist)
                                        {
                                            _trncontext.Remove(stdfee);
                                        }
                                    }

                                    _trncontext.Remove(stdrt);
                                }
                            }

                            CLGStudentRouteMappingDMO object123 = new CLGStudentRouteMappingDMO
                            {
                                MI_Id = data.MI_Id,
                                ASMAY_Id = data.ASMAY_Id,
                                AMCST_Id = std.AMCST_Id,
                                TRSRCO_Date = data.TRSRCO_Date,
                                TRSRCO_PickUpRoute = data.TRSRCO_PickUpRoute,
                                TRRSCO_PickUpLocation = data.TRRSCO_PickUpLocation,
                                TRRSCO_PickUpMobileNo = std.TRRSCO_PickUpMobileNo > 0 ? std.TRRSCO_PickUpMobileNo : null,
                                TRSRCO_DropRoute = data.TRSRCO_DropRoute,
                                TRRSCO_DropLocation = data.TRRSCO_DropLocation,
                                TRRSCO_DropMobileNo = std.TRRSCO_DropMobileNo > 0 ? std.TRRSCO_DropMobileNo : null,
                                TRRSCO_ApplicationNo = std.TRRSCO_ApplicationNo,
                                TRSRCO_PickupSession = data.PickUp_Session,
                                TRSRCO_DropSession = data.Drop_Session,
                                TRRSCO_ActiveFlg = true,
                                CreatedDate = DateTime.Now,
                                UpdatedDate = DateTime.Now,
                                ASTACO_Id = std.ASTACO_Id == 0 ? null : std.ASTACO_Id
                            };
                            _trncontext.Add(object123);

                            if (data.some_data != null && data.some_data.Length > 0)
                            {
                                foreach (var fee in data.some_data)
                                {
                                    if (std.AMCST_Id == fee.amcsT_Id)
                                    {
                                        if (fee.grp_list != null && fee.grp_list.Length > 0)
                                        {
                                            foreach (var x in fee.grp_list)
                                            {
                                                CLGStudentRouteFeeGroupDMO oobj = new CLGStudentRouteFeeGroupDMO
                                                {
                                                    TRSRCO_Id = object123.TRSRCO_Id,
                                                    FMG_Id = x.TRML_Id,
                                                    TRSRFGCO_ActiveFlg = true,
                                                    CreatedDate = DateTime.Now,
                                                    UpdatedDate = DateTime.Now
                                                };
                                                _trncontext.Add(oobj);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var exists = _trncontext.SaveChanges();
                    if (exists >= 1)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }

                }


                var feeconfiguration = _trncontext.FeeMasterConfigurationDMO.Where(t => t.MI_Id == data.MI_Id).FirstOrDefault();

                if (feeconfiguration.FMC_Areawise_FeeFlg == 1)
                {
                    try
                    {
                        foreach (var std in data.savetmpdata)
                        {
                            var MapHostelFee = _trncontext.Database.ExecuteSqlCommand("CLG_Auto_Fee_Group_Mapping_Transport_AreaRequest @p0,@p1,@p2,@p3", data.MI_Id, data.ASMAY_Id, std.AMCST_Id, data.UserId);
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
                        foreach (var std in data.savetmpdata)
                        {
                            var MapHostelFee = _trncontext.Database.ExecuteSqlCommand("CLG_Auto_Fee_Group_Mapping_Transport_AreaRequest @p0,@p1,@p2,@p3", data.MI_Id, data.ASMAY_Id, std.AMCST_Id, data.UserId);
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
                        foreach (var std in data.savetmpdata)
                        {
                            var MapHostelFee = _trncontext.Database.ExecuteSqlCommand("CLG_Auto_Fee_Group_Mapping_Transport @p0,@p1,@p2,@p3", data.MI_Id, data.ASMAY_Id, std.AMCST_Id, data.UserId);
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
                        foreach (var std in data.savetmpdata)
                        {
                            var MapHostelFee = _trncontext.Database.ExecuteSqlCommand("CLG_Auto_Fee_Group_Mapping_Transport_LocationRequest @p0,@p1,@p2,@p3", data.MI_Id, data.ASMAY_Id, std.AMCST_Id, data.UserId);
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                #region commented code
                //for (int i = 0; i < data.savetmpdata.Length; i++)
                //{
                //    for (int j = 0; j < data.some_data.Length; j++)
                //    {
                //        if (data.savetmpdata[i].AMCST_Id == data.some_data[j].amsT_Id)
                //        {

                //            var stu_rec_list = _trncontext.CLGStudentRouteMappingDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMST_Id == data.savetmpdata[i].AMCST_Id).ToList();
                //            if (stu_rec_list.Count > 0)
                //            {
                //                var feegrplist = _trncontext.TR_Student_Route_FeeGroupDMO.Where(t => t.TRSR_Id == stu_rec_list[0].TRSR_Id).ToList();
                //                foreach (var delff in feegrplist)
                //                {
                //                    _trncontext.Remove(delff);
                //                }
                //            }


                //            foreach (var del_stu in stu_rec_list)
                //            {
                //                _trncontext.Remove(del_stu);
                //            }


                //            CLGStudentRouteMappingDMO object123 = new CLGStudentRouteMappingDMO();
                //            object123.MI_Id = data.MI_Id;
                //            object123.ASMAY_Id = data.ASMAY_Id;
                //            object123.AMST_Id = data.savetmpdata[i].AMST_Id;
                //            object123.TRSR_Date = data.TRSR_Date;
                //            //object123.FMG_Id = x.TRML_Id;
                //            object123.TRMR_Id = data.savetmpdata[i].TRMR_Id;
                //            object123.TRSR_PickupSchedule = data.savetmpdata[i].TRSR_PickupSchedule;
                //            object123.TRSR_PickUpLocation = data.savetmpdata[i].TRSR_PickUpLocation;
                //            object123.TRSR_PickUpMobileNo = data.some_data[j].TRSR_PickUpMobileNo;
                //            object123.TRMR_Drop_Route = data.savetmpdata[i].TRMR_Drop_Route;
                //            object123.TRSR_DropSchedule = data.savetmpdata[i].TRSR_DropSchedule;
                //            object123.TRSR_DropLocation = data.savetmpdata[i].TRSR_DropLocation;
                //            object123.TRSR_DropMobileNo = data.some_data[j].TRSR_DropMobileNo;
                //            object123.TRSR_ApplicationNo = data.some_data[j].TRSR_ApplicationNo;
                //            object123.TRSR_PickupSession = data.savetmpdata[i].PickUp_Session;
                //            object123.TRSR_DropSession = data.savetmpdata[i].Drop_Session;
                //            object123.TRSR_ActiveFlg = true;
                //            object123.CreatedDate = DateTime.Now;
                //            object123.UpdatedDate = DateTime.Now;
                //            object123.ASTA_Id = data.savetmpdata[i].ASTA_Id == 0 ? null : data.savetmpdata[i].ASTA_Id;
                //            _trncontext.Add(object123);
                //            _trncontext.SaveChanges();
                //            foreach (var x in data.some_data[j].grp_list)
                //            {
                //                TR_Student_Route_FeeGroupDMO oobj = new TR_Student_Route_FeeGroupDMO();
                //                oobj.TRSR_Id = object123.TRSR_Id;
                //                oobj.FMG_Id = x.TRML_Id;
                //                oobj.TRSRFG_ActiveFlg = true;
                //                _trncontext.Add(oobj);
                //            }

                //        }
                //    }
                //}
                //var exists = _trncontext.SaveChanges();
                //if (exists >= 1)
                //{
                //    data.returnval = true;
                //}
                //else
                //{
                //    data.returnval = false;
                //}

                //else if (stu.studenttype == "route_wise")
                //{
                //    for (int i = 0; i < stu.savetmpdata.Length; i++)
                //    {

                //        for (int j = 0; j < stu.some_data.Length; j++)
                //        {
                //            if (stu.savetmpdata[i].AMST_Id == stu.some_data[j].amsT_Id)
                //            {

                //                var stu_rec_list = _trncontext.CLGStudentRouteMappingDMO.Where(t => t.MI_Id == stu.MI_Id && t.ASMAY_Id == stu.ASMAY_Id && t.AMST_Id == stu.savetmpdata[i].AMST_Id).ToList();

                //                foreach (var del_stu in stu_rec_list)
                //                {
                //                    _trncontext.Remove(del_stu);
                //                }

                //                CLGStudentRouteMappingDMO object123 = new CLGStudentRouteMappingDMO();
                //                object123.MI_Id = stu.MI_Id;
                //                object123.ASMAY_Id = stu.ASMAY_Id;
                //                object123.AMST_Id = stu.savetmpdata[i].AMST_Id;
                //                object123.TRSR_Date = stu.TRSR_Date;
                //                // object123.FMG_Id = x.TRML_Id;
                //                object123.TRMR_Id = stu.trmR_Id_pic;
                //                object123.TRSR_PickupSchedule = stu.savetmpdata[i].TRSR_PickupSchedule;
                //                object123.TRSR_PickUpLocation = stu.TRSR_PickUpLocation;
                //                object123.TRSR_PickUpMobileNo = stu.some_data[j].TRSR_PickUpMobileNo;
                //                object123.TRMR_Drop_Route = stu.trmR_Id_drp;
                //                object123.TRSR_DropSchedule = stu.savetmpdata[i].TRSR_DropSchedule;
                //                object123.TRSR_DropLocation = stu.TRSR_DropLocation;
                //                object123.TRSR_DropMobileNo = stu.some_data[j].TRSR_DropMobileNo;
                //                object123.TRSR_ApplicationNo = stu.some_data[j].TRSR_ApplicationNo;
                //                object123.TRSR_PickupSession = stu.PickUp_Session;
                //                object123.TRSR_DropSession = stu.Drop_Session;

                //                object123.TRSR_ActiveFlg = true;
                //                object123.CreatedDate = DateTime.Now;
                //                object123.UpdatedDate = DateTime.Now;
                //                object123.ASTA_Id = stu.savetmpdata[i].ASTA_Id == 0 ? null : stu.savetmpdata[i].ASTA_Id;
                //                _trncontext.Add(object123);
                //                _trncontext.SaveChanges();
                //                foreach (var x in stu.some_data[j].grp_list)
                //                {
                //                    TR_Student_Route_FeeGroupDMO oobj = new TR_Student_Route_FeeGroupDMO();
                //                    oobj.TRSR_Id = object123.TRSR_Id;
                //                    oobj.FMG_Id = x.TRML_Id;
                //                    oobj.TRSRFG_ActiveFlg = true;
                //                    _trncontext.Add(oobj);
                //                }


                //            }
                //        }
                //    }
                //    var exists = _trncontext.SaveChanges();
                //    if (exists >= 1)
                //    {
                //        stu.returnval = true;
                //    }
                //    else
                //    {
                //        stu.returnval = false;
                //    }
                //}

                #endregion commented code
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }

        public CLGStudentRouteMappingDTO geteditdata(CLGStudentRouteMappingDTO data)
        {
            try
            {
                // data.geteditdataarea = _context.MasterAreaDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMA_Id == data.TRMA_Id).ToArray();
            }
            catch (Exception ex)
            {
                _areaimpl.LogInformation("Transport Error Master Area geteditdata " + ex.Message);
            }
            return data;
        }

        public CLGStudentRouteMappingDTO getstudents(CLGStudentRouteMappingDTO data)
        {
            try
            {




                if (data.studenttype == "class_wise")
                {
                    var amst_list = _trncontext.CLGStudentRouteMappingDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.TRRSCO_ActiveFlg == true).Select(d => d.AMCST_Id);

                    data.admsudentslist = (from a in _trncontext.Adm_College_Yearly_StudentDMO
                                           from b in _trncontext.Adm_Master_College_StudentDMO
                                           where (a.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id && b.AMCST_ActiveFlag == true && a.ACMS_Id == data.ACMS_Id && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id && a.ACYST_ActiveFlag == 1 && b.AMCST_SOL == "S" && a.AMCST_Id == b.AMCST_Id && !amst_list.Contains(b.AMCST_Id))
                                           select new CLGStudentRouteMappingDTO
                                           {
                                               AMCST_Id = b.AMCST_Id,
                                               AMCST_FirstName = ((b.AMCST_FirstName == null ? " " : b.AMCST_FirstName) + " " + (b.AMCST_MiddleName == null ? " " : b.AMCST_MiddleName) + " " + (b.AMCST_LastName == null ? " " : b.AMCST_LastName)).Trim(),
                                               AMCST_MobileNo = b.AMCST_MobileNo
                                           }).Distinct().OrderBy(d => d.AMCST_FirstName).ToArray();
                }
                else if (data.studenttype == "route_wise")
                {

                    var amst_list = _trncontext.CLGStudentRouteMappingDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.TRRSCO_ActiveFlg == true).Select(d => d.AMCST_Id);

                    data.admsudentslist = (from a in _trncontext.Adm_College_Yearly_StudentDMO
                                           from b in _trncontext.Adm_Master_College_StudentDMO
                                           where (a.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id && b.AMCST_ActiveFlag == true && a.ACMS_Id == data.ACMS_Id && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id && a.ACYST_ActiveFlag == 1 && b.AMCST_SOL == "S" && a.AMCST_Id == b.AMCST_Id && !amst_list.Contains(b.AMCST_Id))
                                           select new CLGStudentRouteMappingDTO
                                           {
                                               AMCST_Id = b.AMCST_Id,
                                               AMCST_FirstName = ((b.AMCST_FirstName == null ? " " : b.AMCST_FirstName) + " " + (b.AMCST_MiddleName == null ? " " : b.AMCST_MiddleName) + " " + (b.AMCST_LastName == null ? " " : b.AMCST_LastName)).Trim(),
                                               AMCST_MobileNo = b.AMCST_MobileNo,
                                               AMCST_AdmNo = b.AMCST_AdmNo,
                                               ACYST_RollNo = a.ACYST_RollNo,
                                           }).Distinct().OrderBy(d => d.AMCST_FirstName).ToArray();


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }



        public CLGStudentRouteMappingDTO getreporteditbuspass(CLGStudentRouteMappingDTO data)
        {

            try
            {
                using (var cmd = _trncontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TR_CLG_GETSTUDENTROUTEMAP_NEW";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@Mi_Id",
                       SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(data.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                       SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(data.ASMAY_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                       SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(data.AMCO_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                       SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(data.AMB_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                       SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(data.AMSE_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id",
                       SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(data.ACMS_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ACSRM_Monthid",
                    SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(data.IVRM_Month_Id)
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
                        data.admsudentslist = retObject.ToArray();


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


        public CLGStudentRouteMappingDTO savedatabuspass(CLGStudentRouteMappingDTO data)
        {
            try
            {

                try
                {
                    foreach (var std in data.savetmpdata)
                    {
                        var studentbuspass = _trncontext.Database.ExecuteSqlCommand("TR_CLG_GENERATESTUDENTROUTEMAP @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10",
                            data.MI_Id, std.AMCST_Id, data.ASMAY_Id, data.AMCO_Id, data.AMB_Id, data.AMSE_Id, data.ACMS_Id, std.ACSRM_Mapping_Flag, data.ACSRM_Monthid, std.TRSRCO_PickUpRoute, std.TRRSCO_PickUpLocation);
                        
                        if (studentbuspass > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                   

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }




            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }


        public CLGStudentRouteMappingDTO viewrecordspopup(CLGStudentRouteMappingDTO data)
        {
            try
            {


                using (var cmd = _trncontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TR_CLG_GETSTUDENTROUTEMAP_POPUP";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@Mi_Id",
                       SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(data.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@TRSRCO_Id",
                       SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(data.TRSRCO_Id)
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
                        data.reportdatelist1 = retObject.ToArray();


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

        public CLGStudentRouteMappingDTO checkduplicateno(CLGStudentRouteMappingDTO data)
        {
            try
            {

                var checkapplictionnumber = _trncontext.CLGStudentRouteMappingDMO.Where(a => a.MI_Id == data.MI_Id && a.TRRSCO_ApplicationNo.Equals(data.TRRSCO_ApplicationNo) && a.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id != data.AMCST_Id).ToList();
                if (checkapplictionnumber.Count > 0)
                {
                    data.duplicateno = "Duplicate";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }

        public CLGStudentRouteMappingDTO getreportedit(CLGStudentRouteMappingDTO data)
        {

            try
            {
                data.admsudentslist = (from a in _trncontext.Adm_College_Yearly_StudentDMO
                                       from b in _trncontext.Adm_Master_College_StudentDMO
                                       where (a.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id && a.ACYST_ActiveFlag == 1 && a.AMCST_Id == b.AMCST_Id && a.AMCST_Id == data.AMCST_Id)
                                       select new CLGStudentRouteMappingDTO
                                       {
                                           AMCST_Id = b.AMCST_Id,
                                           AMCST_FirstName = ((b.AMCST_FirstName == null ? " " : b.AMCST_FirstName) + " " + (b.AMCST_MiddleName == null ? " " : b.AMCST_MiddleName) + " " + (b.AMCST_LastName == null ? " " : b.AMCST_LastName)).Trim(),
                                           AMCST_MobileNo = b.AMCST_MobileNo,
                                           AMCO_Id = a.AMCO_Id,
                                           AMB_Id = a.AMB_Id,
                                           AMSE_Id = a.AMSE_Id,
                                           ASMAY_Id = data.ASMAY_Id,
                                           ACMS_Id = a.ACMS_Id
                                       }).Distinct().ToArray();
                List<CLGStudentRouteMappingDMO> alrdy_stu_list = new List<CLGStudentRouteMappingDMO>();
                foreach (CLGStudentRouteMappingDTO e in data.admsudentslist)
                {
                    var list_stu_date = _trncontext.CLGStudentRouteMappingDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMCST_Id == e.AMCST_Id && t.TRRSCO_ActiveFlg == true && t.TRSRCO_Id == data.TRSRCO_Id
                    ).Distinct().ToList();

                    data.grpeditlist = _trncontext.CLGStudentRouteFeeGroupDMO.Where(t => t.TRSRCO_Id == list_stu_date[0].TRSRCO_Id).ToArray();

                    if (list_stu_date.Count == 0)
                    {
                        //var list_stu = _trncontext.Adm_Student_Transport_ApplicationDMO.Where(t => t.MI_Id == data.MI_Id && t.ASTA_FutureAY == data.ASMAY_Id && t.AMST_Id == e.AMST_Id && t.ASTA_ApplStatus == "Approved").Distinct().ToList();
                        //var group_list = (from a in _trncontext.FeeStudentTransactionDMO
                        //                  from b in _trncontext.feehead
                        //                  where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == e.AMST_Id && b.MI_Id == data.MI_Id && b.FMH_ActiveFlag == true && b.FMH_Flag == "T" && a.FMH_Id == b.FMH_Id)
                        //                  select a.FMG_Id).Distinct().ToList();

                        //foreach (var s in list_stu)
                        //{
                        //    foreach (var q in group_list)
                        //    {
                        //        TR_Student_RouteDMO obj_pre = new TR_Student_RouteDMO();
                        //        obj_pre.MI_Id = s.MI_Id;
                        //        obj_pre.ASMAY_Id = s.ASTA_FutureAY;
                        //        obj_pre.AMST_Id = s.AMST_Id;
                        //        obj_pre.TRSR_Date = DateTime.Now;
                        //        obj_pre.FMG_Id = q;
                        //        obj_pre.TRMR_Id = s.ASTA_PickUp_TRMR_Id;
                        //        // TRSR_PickupSchedule
                        //        obj_pre.TRSR_PickUpLocation = s.ASTA_PickUp_TRML_Id;
                        //        obj_pre.TRSR_PickUpMobileNo = e.AMST_MobileNo;
                        //        //TRSR_DropSchedule
                        //        obj_pre.TRSR_DropLocation = s.ASTA_Drop_TRML_Id;
                        //        obj_pre.TRSR_DropMobileNo = e.AMST_MobileNo;
                        //        obj_pre.TRSR_ApplicationNo = Convert.ToInt64(s.ASTA_ApplicationNo);
                        //        //TRSR_ActiveFlg
                        //        obj_pre.ASTA_Id = s.ASTA_Id;
                        //        obj_pre.TRMR_Drop_Route = s.ASTA_Drop_TRMR_Id;
                        //        alrdy_stu_list.Add(obj_pre);
                        //    }
                        //    //alrdy_stu_list.Add(obj_pre);
                        //}
                    }
                    else
                    {
                        foreach (var f in list_stu_date)
                        {
                            alrdy_stu_list.Add(f);
                        }
                    }

                }
                data.alrdy_stu_list = alrdy_stu_list.ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }






        public CLGStudentRouteMappingDTO deactivate(CLGStudentRouteMappingDTO data)
        {
            try
            {
                if (data.TRSRCO_Id > 0)
                {
                    var result = _trncontext.CLGStudentRouteMappingDMO.Single(t => t.TRSRCO_Id == data.TRSRCO_Id);
                    var feelist = _trncontext.CLGStudentRouteFeeGroupDMO.Where(t => t.TRSRCO_Id == data.TRSRCO_Id).ToList();

                    if (result.TRRSCO_ActiveFlg == true)
                    {
                        result.TRRSCO_ActiveFlg = false;
                        result.UpdatedDate = DateTime.Now;
                        if (feelist.Count > 0)
                        {
                            foreach (var item in feelist)
                            {
                                item.TRSRFGCO_ActiveFlg = false;
                                item.UpdatedDate = DateTime.Now;
                                _trncontext.Update(item);
                            }
                        }


                        _trncontext.Update(result);

                    }
                    else
                    {
                        result.TRRSCO_ActiveFlg = true;
                        result.UpdatedDate = DateTime.Now;

                        if (feelist.Count > 0)
                        {
                            foreach (var item1 in feelist)
                            {
                                item1.TRSRFGCO_ActiveFlg = true;
                                item1.UpdatedDate = DateTime.Now;
                                _trncontext.Update(item1);
                            }
                        }
                        _trncontext.Update(result);
                    }

                    var flag = _trncontext.SaveChanges();
                    if (flag > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }



            return data;
        }

        public CLGStudentRouteMappingDTO SearchByColumn(CLGStudentRouteMappingDTO data)
        {
            try
            {
                if (data.SearchColumn == "" || data.SearchColumn == null)
                {
                    data.SearchColumn = "0";
                }
                if (data.SearchColumn == "1")
                {
                    DateTime date1 = Convert.ToDateTime(data.EnteredData);
                    data.EnteredData = date1.ToString("yyyy-MM-dd");
                }

                using (var cmd = _trncontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TR_CLG_GETSTUDENTROUTEMAP_SEARCH";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@Mi_Id",
                       SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(data.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@searchtype",
                      SqlDbType.VarChar)
                    {
                        Value = data.SearchColumn
                    });
                    cmd.Parameters.Add(new SqlParameter("@searchtext",
                      SqlDbType.VarChar)
                    {
                        Value = data.EnteredData
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {

                        // var data = cmd.ExecuteNonQuery();

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
                        data.reportdatelist = retObject.ToArray();


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
    }
}
