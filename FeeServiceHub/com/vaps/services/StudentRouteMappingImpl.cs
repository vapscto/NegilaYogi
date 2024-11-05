using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using FeeServiceHub.com.vaps.interfaces;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.IO;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using PreadmissionDTOs.com.vaps.admission;
using DomainModel.Model.com.vaps.admission;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using DomainModel.Model.com.vapstech.Transport;
using System.Globalization;

namespace FeeServiceHub.com.vaps.services
{
    public class StudentRouteMappingImpl : interfaces.StudentRouteMappingInterface
    {


        public FeeGroupContext _FeeGroupContext;
        public StudentRouteMappingImpl(FeeGroupContext frgContext)
        {
            _FeeGroupContext = frgContext;
        }

        public StudentRouteMappingDTO getdata123(StudentRouteMappingDTO data)
        {

            try
            {

                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();//&& t.ASMAY_Id==data.ASMAY_Id
                data.acayear = year.Distinct().ToArray();

                List<School_M_Class> allclas = new List<School_M_Class>();
                allclas = _FeeGroupContext.admissioncls.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                data.classlist = allclas.Distinct().ToArray();

                List<School_M_Section> allsetion = new List<School_M_Section>();
                allsetion = _FeeGroupContext.school_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).ToList();
                data.sectionlist = allsetion.Distinct().ToArray();



                List<MasterRouteDMO> busro = new List<MasterRouteDMO>();
                busro = _FeeGroupContext.MasterRouteDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMR_ActiveFlg == true).ToList();
                data.busroutelist = busro.ToArray();



                var locationlist = _FeeGroupContext.MasterLocationDMO.Where(a => a.MI_Id == data.MI_Id && a.TRML_ActiveFlg == true).Distinct().ToList();
                data.locationlist = locationlist.ToArray();

                List<TR_Route_ScheduleDMO> schedule = new List<TR_Route_ScheduleDMO>();
                schedule = _FeeGroupContext.TR_Route_ScheduleDMO.Where(t => t.MI_Id == data.MI_Id && t.TRRSC_ActiveFlag == true).ToList();
                data.schedulelist = schedule.ToArray();


                data.grouplist = (from a in _FeeGroupContext.FeeHeadDMO
                                  from b in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                                  from c in _FeeGroupContext.FeeGroupDMO
                                  where (a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && a.FMH_ActiveFlag == true && a.FMH_Flag == "T" && a.FMH_Id == b.FMH_Id && b.FMG_Id == c.FMG_Id && c.FMG_ActiceFlag == true && b.FYGHM_ActiveFlag == "1")
                                  select c).Distinct().ToArray();


                data.routelist = _FeeGroupContext.MasterRouteDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMR_ActiveFlg == true).OrderBy(t => t.TRMR_order).ToArray();



                data.picsesslist = _FeeGroupContext.MsterSessionDMO.Where(f => f.MI_Id == data.MI_Id && f.TRMS_ActiveFlg == true && f.TRMS_Flag == "Pick Up").Distinct().ToArray();
                data.drpsesslist = _FeeGroupContext.MsterSessionDMO.Where(f => f.MI_Id == data.MI_Id && f.TRMS_ActiveFlg == true && f.TRMS_Flag == "Drop").Distinct().ToArray();






                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TR_GETSTUDENT_ROUTEMAP_LOAD";
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


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }


        public StudentRouteMappingDTO get_cls_secs(StudentRouteMappingDTO data)
        {
            try
            {

                //data.savegrplst = (from gh in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                //                   where (gh.ASMAY_Id == data.ASMAY_Id && gh.MI_Id == data.MI_Id)
                //                   select new StudentRouteMappingDTO
                //                   {
                //                       FMH_Id = gh.FMH_Id
                //                   }).Distinct().ToArray();


                //List<long> GrpId = new List<long>();
                //foreach (var item in data.savegrplst)
                //{
                //    GrpId.Add(item.FMH_Id);
                //}

                //data.headlist = (from b in _FeeGroupContext.FeeHeadDMO
                //                where GrpId.Contains(b.FMH_Id)

                //               select new StudentRouteMappingDTO
                //               {
                //                   FMH_Id = b.FMH_Id,
                //                   FMH_FeeName = b.FMH_FeeName,

                //               }
                //       ).Distinct().ToArray();


                data.classlist = (from a in _FeeGroupContext.School_Adm_Y_StudentDMO
                                  from b in _FeeGroupContext.School_M_Class
                                  where (a.ASMAY_Id == data.ASMAY_Id && a.AMAY_ActiveFlag == 1 && b.MI_Id == data.MI_Id && b.ASMCL_ActiveFlag == true && a.ASMCL_Id == b.ASMCL_Id)
                                  select b).Distinct().OrderBy(t => t.ASMCL_Order).ToArray();

                data.sectionlist = (from a in _FeeGroupContext.School_Adm_Y_StudentDMO
                                    from b in _FeeGroupContext.school_M_Section
                                    where (a.ASMAY_Id == data.ASMAY_Id && a.AMAY_ActiveFlag == 1 && b.MI_Id == data.MI_Id && b.ASMC_ActiveFlag == 1 && a.ASMS_Id == b.ASMS_Id)
                                    select b).Distinct().OrderBy(t => t.ASMC_Order).ToArray();
                data.grouplist = (from a in _FeeGroupContext.FeeHeadDMO
                                  from b in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                                  from c in _FeeGroupContext.FeeGroupDMO
                                  where (a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && a.FMH_ActiveFlag == true && a.FMH_Flag == "T" && a.FMH_Id == b.FMH_Id && b.FMG_Id == c.FMG_Id && c.FMG_ActiceFlag == true && b.ASMAY_Id == data.ASMAY_Id)
                                  select c).Distinct().ToArray();



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public StudentRouteMappingDTO on_pic_route_change(StudentRouteMappingDTO data)
        {
            try
            {
                data.picloclist = (from a in _FeeGroupContext.Route_Location
                                   from b in _FeeGroupContext.MasterLocationDMO
                                   where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.TRML_Id == b.TRML_Id && a.TRMR_Id == data.TRMR_Id && b.TRML_ActiveFlg == true && a.TRMRL_ActiveFlag == true
                                   select new StudentRouteMappingDTO
                                   {

                                       PickUp_LocationName = b.TRML_LocationName,
                                       TRSR_PickUpLocation = b.TRML_Id,
                                       TRMRL_Order = a.TRMRL_Order


                                   }).Distinct().OrderBy(x => x.TRMRL_Order).ToArray();

                data.picsesslist = _FeeGroupContext.MsterSessionDMO.Where(f => f.MI_Id == data.MI_Id && f.TRMS_ActiveFlg == true && f.TRMS_Flag == "Pick Up").Distinct().ToArray();





            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }


        public StudentRouteMappingDTO on_drp_route_change(StudentRouteMappingDTO data)
        {
            try
            {
                data.droploclist = (from a in _FeeGroupContext.Route_Location
                                    from b in _FeeGroupContext.MasterLocationDMO
                                    where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.TRML_Id == b.TRML_Id && a.TRMR_Id == data.TRMR_Id && b.TRML_ActiveFlg == true && a.TRMRL_ActiveFlag == true
                                    select new StudentRouteMappingDTO
                                    {

                                        Drop_LocationName = b.TRML_LocationName,
                                        TRSR_DropLocation = b.TRML_Id,
                                        TRMRL_Order = a.TRMRL_Order


                                    }).Distinct().OrderBy(x => x.TRMRL_Order).ToArray();

                data.drpsesslist = _FeeGroupContext.MsterSessionDMO.Where(f => f.MI_Id == data.MI_Id && f.TRMS_ActiveFlg == true && f.TRMS_Flag == "Drop").Distinct().ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public StudentRouteMappingDTO getlisttwo(StudentRouteMappingDTO stu)
        {
            try
            {
                var outputval = 0;
                if (stu.studenttype == "class_wise")
                {
                    for (int i = 0; i < stu.savetmpdata.Length; i++)
                    {
                        if (stu.some_data.Length > 0)
                        {
                            for (int j = 0; j < stu.some_data.Length; j++)
                            {
                                if (stu.savetmpdata[i].AMST_Id == stu.some_data[j].amsT_Id)
                                {

                                    var stu_rec_list = _FeeGroupContext.TR_Student_RouteDMO.Where(t => t.MI_Id == stu.MI_Id && t.ASMAY_Id == stu.ASMAY_Id && t.AMST_Id == stu.savetmpdata[i].AMST_Id).ToList();
                                    if (stu_rec_list.Count > 0)
                                    {
                                        var feegrplist = _FeeGroupContext.TR_Student_Route_FeeGroupDMO.Where(t => t.TRSR_Id == stu_rec_list[0].TRSR_Id).ToList();
                                        foreach (var delff in feegrplist)
                                        {
                                            _FeeGroupContext.Remove(delff);
                                        }
                                    }


                                    foreach (var del_stu in stu_rec_list)
                                    {
                                        _FeeGroupContext.Remove(del_stu);
                                    }


                                    TR_Student_RouteDMO object123 = new TR_Student_RouteDMO();
                                    object123.MI_Id = stu.MI_Id;
                                    object123.ASMAY_Id = stu.ASMAY_Id;
                                    object123.AMST_Id = stu.savetmpdata[i].AMST_Id;
                                    object123.TRSR_Date = stu.TRSR_Date;
                                    //object123.FMG_Id = x.TRML_Id;
                                    object123.TRMR_Id = stu.savetmpdata[i].TRMR_Id;
                                    object123.TRSR_PickupSchedule = stu.savetmpdata[i].TRSR_PickupSchedule;
                                    object123.TRSR_PickUpLocation = stu.savetmpdata[i].TRSR_PickUpLocation;
                                    object123.TRSR_PickUpMobileNo = stu.some_data[j].TRSR_PickUpMobileNo;
                                    object123.TRMR_Drop_Route = stu.savetmpdata[i].TRMR_Drop_Route;
                                    object123.TRSR_DropSchedule = stu.savetmpdata[i].TRSR_DropSchedule;
                                    object123.TRSR_DropLocation = stu.savetmpdata[i].TRSR_DropLocation;
                                    object123.TRSR_DropMobileNo = stu.some_data[j].TRSR_DropMobileNo;
                                    object123.TRSR_ApplicationNo = stu.some_data[j].TRSR_ApplicationNo;
                                    object123.TRSR_PickupSession = stu.savetmpdata[i].PickUp_Session;
                                    object123.TRSR_DropSession = stu.savetmpdata[i].Drop_Session;
                                    object123.TRSR_ActiveFlg = true;
                                    object123.CreatedDate = DateTime.Now;
                                    object123.UpdatedDate = DateTime.Now;
                                    object123.ASTA_Id = stu.savetmpdata[i].ASTA_Id == 0 ? null : stu.savetmpdata[i].ASTA_Id;
                                    _FeeGroupContext.Add(object123);
                                    _FeeGroupContext.SaveChanges();
                                    foreach (var x in stu.some_data[j].grp_list)
                                    {
                                        TR_Student_Route_FeeGroupDMO oobj = new TR_Student_Route_FeeGroupDMO();
                                        oobj.TRSR_Id = object123.TRSR_Id;
                                        oobj.FMG_Id = x.TRML_Id;
                                        oobj.TRSRFG_ActiveFlg = true;
                                        _FeeGroupContext.Add(oobj);
                                    }



                                }
                            }
                        }
                        else
                        {



                            var stu_rec_list = _FeeGroupContext.TR_Student_RouteDMO.Where(t => t.MI_Id == stu.MI_Id && t.ASMAY_Id == stu.ASMAY_Id && t.AMST_Id == stu.savetmpdata[i].AMST_Id).ToList();
                            if (stu_rec_list.Count > 0)
                            {
                                var feegrplist = _FeeGroupContext.TR_Student_Route_FeeGroupDMO.Where(t => t.TRSR_Id == stu_rec_list[0].TRSR_Id).ToList();
                                foreach (var delff in feegrplist)
                                {
                                    _FeeGroupContext.Remove(delff);
                                }
                            }


                            foreach (var del_stu in stu_rec_list)
                            {
                                _FeeGroupContext.Remove(del_stu);
                            }


                            TR_Student_RouteDMO object123 = new TR_Student_RouteDMO();
                            object123.MI_Id = stu.MI_Id;
                            object123.ASMAY_Id = stu.ASMAY_Id;
                            object123.AMST_Id = stu.savetmpdata[i].AMST_Id;
                            object123.TRSR_Date = stu.TRSR_Date;
                            //object123.FMG_Id = x.TRML_Id;
                            object123.TRMR_Id = stu.savetmpdata[i].TRMR_Id;
                            object123.TRSR_PickupSchedule = stu.savetmpdata[i].TRSR_PickupSchedule;
                            object123.TRSR_PickUpLocation = stu.savetmpdata[i].TRSR_PickUpLocation;
                            //object123.TRSR_PickUpMobileNo = stu.some_data[j].TRSR_PickUpMobileNo;
                            object123.TRMR_Drop_Route = stu.savetmpdata[i].TRMR_Drop_Route;
                            object123.TRSR_DropSchedule = stu.savetmpdata[i].TRSR_DropSchedule;
                            object123.TRSR_DropLocation = stu.savetmpdata[i].TRSR_DropLocation;
                            //object123.TRSR_DropMobileNo = stu.some_data[j].TRSR_DropMobileNo;
                            //object123.TRSR_ApplicationNo = stu.some_data[j].TRSR_ApplicationNo;
                            object123.TRSR_PickupSession = stu.savetmpdata[i].PickUp_Session;
                            object123.TRSR_DropSession = stu.savetmpdata[i].Drop_Session;
                            object123.TRSR_ActiveFlg = true;
                            object123.CreatedDate = DateTime.Now;
                            object123.UpdatedDate = DateTime.Now;
                            object123.ASTA_Id = stu.savetmpdata[i].ASTA_Id == 0 ? null : stu.savetmpdata[i].ASTA_Id;
                            _FeeGroupContext.Add(object123);
                            _FeeGroupContext.SaveChanges();




                        }

                    }


                    //if (outputval > 0)
                    //{
                    var exists = _FeeGroupContext.SaveChanges();
                    if (exists >= 0)
                    {


                        for (int i = 0; i < stu.savetmpdata.Length; i++)
                        {
                            for (int j = 0; j < stu.some_data.Length; j++)
                            {
                                if (stu.savetmpdata[i].AMST_Id == stu.some_data[j].amsT_Id)
                                {



                                    outputval = _FeeGroupContext.Database.ExecuteSqlCommand("AutoFeeGroupmappingLocationWise @p0,@p1,@p2", stu.MI_Id, stu.ASMAY_Id, stu.savetmpdata[i].AMST_Id);
                                }
                            }
                        }
                    }
                    else
                    {

                        stu.returnval = false;
                    }



                    if (exists >= 0)
                    {
                        stu.returnval = true;
                    }
                    else
                    {
                        stu.returnval = false;
                    }


                    //}
                    //else
                    //{

                    //    stu.returnval = false;
                    //}
                }
                else if (stu.studenttype == "route_wise")
                {
                    for (int i = 0; i < stu.savetmpdata.Length; i++)
                    {

                        for (int j = 0; j < stu.some_data.Length; j++)
                        {
                            if (stu.savetmpdata[i].AMST_Id == stu.some_data[j].amsT_Id)
                            {

                                var stu_rec_list = _FeeGroupContext.TR_Student_RouteDMO.Where(t => t.MI_Id == stu.MI_Id && t.ASMAY_Id == stu.savetmpdata[i].ASMAY_Id && t.AMST_Id == stu.savetmpdata[i].AMST_Id).ToList();

                                foreach (var del_stu in stu_rec_list)
                                {
                                    _FeeGroupContext.Remove(del_stu);
                                }

                                TR_Student_RouteDMO object123 = new TR_Student_RouteDMO();
                                object123.MI_Id = stu.MI_Id;
                                object123.ASMAY_Id = stu.savetmpdata[i].ASMAY_Id;
                                object123.AMST_Id = stu.savetmpdata[i].AMST_Id;
                                object123.TRSR_Date = stu.savetmpdata[i].TRSR_Date;
                                // object123.FMG_Id = x.TRML_Id;
                                object123.TRMR_Id = stu.savetmpdata[i].trmR_Id_pic;
                                object123.TRSR_PickupSchedule = 0;
                                object123.TRSR_PickUpLocation = stu.savetmpdata[i].TRSR_PickUpLocation;
                                object123.TRSR_PickUpMobileNo = stu.some_data[j].TRSR_PickUpMobileNo;
                                object123.TRMR_Drop_Route = stu.savetmpdata[i].trmR_Id_drp;
                                object123.TRSR_DropSchedule = stu.savetmpdata[i].TRSR_DropSchedule;
                                object123.TRSR_DropLocation = stu.savetmpdata[i].TRSR_DropLocation;
                                object123.TRSR_DropMobileNo = stu.some_data[j].TRSR_DropMobileNo;
                                object123.TRSR_ApplicationNo = stu.some_data[j].TRSR_ApplicationNo;
                                object123.TRSR_PickupSession = stu.savetmpdata[i].PickUp_Session;
                                object123.TRSR_DropSession = stu.savetmpdata[i].Drop_Session;

                                object123.TRSR_ActiveFlg = true;
                                object123.CreatedDate = DateTime.Now;
                                object123.UpdatedDate = DateTime.Now;
                                object123.ASTA_Id = stu.savetmpdata[i].ASTA_Id == 0 ? null : stu.savetmpdata[i].ASTA_Id;
                                _FeeGroupContext.Add(object123);
                                _FeeGroupContext.SaveChanges();
                                foreach (var x in stu.some_data[j].grp_list)
                                {
                                    TR_Student_Route_FeeGroupDMO oobj = new TR_Student_Route_FeeGroupDMO();
                                    oobj.TRSR_Id = object123.TRSR_Id;
                                    oobj.FMG_Id = x.TRML_Id;
                                    oobj.TRSRFG_ActiveFlg = true;
                                    _FeeGroupContext.Add(oobj);
                                }


                            }
                        }
                    }
                    //if (outputval > 0)
                    //{
                    var exists = _FeeGroupContext.SaveChanges();
                    if (exists >= 0)
                    {
                        for (int i = 0; i < stu.savetmpdata.Length; i++)
                        {
                            for (int j = 0; j < stu.some_data.Length; j++)
                            {
                                if (stu.savetmpdata[i].AMST_Id == stu.some_data[j].amsT_Id)
                                {

                                    //  stu.returnval = true;
                                    outputval = _FeeGroupContext.Database.ExecuteSqlCommand("AutoFeeGroupmappingLocationWise @p0,@p1,@p2", stu.MI_Id, stu.savetmpdata[i].ASMAY_Id, stu.savetmpdata[i].AMST_Id);
                                }
                            }
                        }
                    }
                    else
                    {
                        stu.returnval = false;
                    }
                    //}
                    //else
                    //{
                    //    stu.returnval = false;
                    //}

                    if (exists >= 0 && outputval >= 0)
                    {
                        stu.returnval = true;
                    }
                    else
                    {
                        stu.returnval = false;
                    }
                }

            }

            catch (Exception ee)
            {
                stu.returnval = false;
                Console.WriteLine(ee.Message);
            }

            return stu;
        }


        public StudentRouteMappingDTO get_sections(StudentRouteMappingDTO data)
        {
            try
            {
                data.sectionlist = (from a in _FeeGroupContext.School_Adm_Y_StudentDMO
                                    from b in _FeeGroupContext.school_M_Section
                                    where (a.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id && b.ASMC_ActiveFlag == 1 && a.ASMS_Id == b.ASMS_Id && a.ASMCL_Id == data.ASMCL_Id && a.AMAY_ActiveFlag == 1)
                                    select b).Distinct().OrderBy(t => t.ASMC_Order).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public StudentRouteMappingDTO getreport(StudentRouteMappingDTO data)
        {

            try
            {
                if (data.studenttype == "class_wise")
                {
                    var amst_list = _FeeGroupContext.TR_Student_RouteDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.TRSR_ActiveFlg == true).Select(d => d.AMST_Id);

                    data.admsudentslist = (from a in _FeeGroupContext.School_Adm_Y_StudentDMO
                                           from b in _FeeGroupContext.AdmissionStudentDMO
                                           where (a.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id && b.AMST_ActiveFlag == 1 && a.ASMS_Id == data.ASMS_Id && a.ASMCL_Id == data.ASMCL_Id && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && a.AMST_Id == b.AMST_Id && !amst_list.Contains(b.AMST_Id))
                                           select new StudentRouteMappingDTO
                                           {
                                               AMST_Id = b.AMST_Id,
                                               AMST_FirstName = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + " " + (b.AMST_LastName == null ? " " : b.AMST_LastName)).Trim(),
                                               AMST_MobileNo = b.AMST_MobileNo
                                           }).Distinct().ToArray();
                }
                else if (data.studenttype == "route_wise")
                {

                    var amst_list = _FeeGroupContext.TR_Student_RouteDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.TRSR_ActiveFlg == true).Select(d => d.AMST_Id);

                    data.admsudentslist = (from a in _FeeGroupContext.School_Adm_Y_StudentDMO
                                           from b in _FeeGroupContext.AdmissionStudentDMO
                                           where (a.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id && b.AMST_ActiveFlag == 1 && a.ASMS_Id == data.ASMS_Id && a.ASMCL_Id == data.ASMCL_Id && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && a.AMST_Id == b.AMST_Id && !amst_list.Contains(b.AMST_Id))
                                           select new StudentRouteMappingDTO
                                           {
                                               AMST_Id = b.AMST_Id,
                                               AMST_FirstName = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + " " + (b.AMST_LastName == null ? " " : b.AMST_LastName)).Trim(),
                                               AMST_MobileNo = b.AMST_MobileNo,
                                               amst_admno = b.AMST_AdmNo,
                                               amay_rollno = a.AMAY_RollNo,
                                           }).Distinct().ToArray();


                }

                //using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "TR_STUDENTS_FOR_ROUTE_MAPPING";
                //    cmd.CommandType = CommandType.StoredProcedure;

                //    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                //       SqlDbType.BigInt)
                //    {
                //        Value = data.MI_Id
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                //      SqlDbType.BigInt)
                //    {
                //        Value = data.ASMAY_Id
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                //    SqlDbType.BigInt)
                //    {
                //        Value = data.ASMCL_Id
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                //    SqlDbType.BigInt)
                //    {
                //        Value = data.ASMS_Id
                //    });


                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();

                //    var retObject = new List<dynamic>();

                //    try
                //    {

                //        // var data = cmd.ExecuteNonQuery();

                //        using (var dataReader = cmd.ExecuteReader())
                //        {
                //            while (dataReader.Read())
                //            {
                //                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                {
                //                    dataRow.Add(
                //                        dataReader.GetName(iFiled),
                //                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                //                    );
                //                }

                //                retObject.Add((ExpandoObject)dataRow);
                //            }
                //        }
                //        data.admsudentslist = retObject.ToArray();


                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }
                //}

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public StudentRouteMappingDTO getreportedit(StudentRouteMappingDTO data)
        {

            try
            {
                data.admsudentslist = (from a in _FeeGroupContext.School_Adm_Y_StudentDMO
                                       from b in _FeeGroupContext.AdmissionStudentDMO
                                       where (a.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id && b.AMST_ActiveFlag == 1 && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && a.AMST_Id == b.AMST_Id && a.AMST_Id == data.AMST_Id)
                                       select new StudentRouteMappingDTO
                                       {
                                           AMST_Id = b.AMST_Id,
                                           AMST_FirstName = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + " " + (b.AMST_LastName == null ? " " : b.AMST_LastName)).Trim(),
                                           AMST_MobileNo = b.AMST_MobileNo,
                                           ASMCL_Id = a.ASMCL_Id,
                                           ASMAY_Id = data.ASMAY_Id,
                                           ASMS_Id = a.ASMS_Id
                                       }).Distinct().ToArray();
                List<TR_Student_RouteDMO> alrdy_stu_list = new List<TR_Student_RouteDMO>();
                foreach (StudentRouteMappingDTO e in data.admsudentslist)
                {
                    var count_stu = _FeeGroupContext.TR_Student_RouteDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMST_Id == e.AMST_Id && t.TRSR_ActiveFlg == true).ToList().Count;
                    var date_stu = DateTime.Now.ToString();
                    if (count_stu > 0)
                    {
                        date_stu = _FeeGroupContext.TR_Student_RouteDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMST_Id == e.AMST_Id && t.TRSR_ActiveFlg == true).Distinct().Select(t => t.TRSR_Date).Max().ToString();
                    }

                    var list_stu_date = _FeeGroupContext.TR_Student_RouteDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMST_Id == e.AMST_Id && t.TRSR_ActiveFlg == true
                    //&& t.TRSR_Date == date_stu).Distinct().ToList();
                    ).Distinct().ToList();

                    data.grpeditlist = _FeeGroupContext.TR_Student_Route_FeeGroupDMO.Where(t => t.TRSR_Id == list_stu_date[0].TRSR_Id).ToArray();

                    if (list_stu_date.Count == 0)
                    {
                        var list_stu = _FeeGroupContext.Adm_Student_Transport_ApplicationDMO.Where(t => t.MI_Id == data.MI_Id && t.ASTA_FutureAY == data.ASMAY_Id && t.AMST_Id == e.AMST_Id && t.ASTA_ApplStatus == "Approved").Distinct().ToList();
                        var group_list = (from a in _FeeGroupContext.FeeStudentTransactionDMO
                                          from b in _FeeGroupContext.feehead
                                          where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == e.AMST_Id && b.MI_Id == data.MI_Id && b.FMH_ActiveFlag == true && b.FMH_Flag == "T" && a.FMH_Id == b.FMH_Id)
                                          select a.FMG_Id).Distinct().ToList();

                        foreach (var s in list_stu)
                        {
                            foreach (var q in group_list)
                            {
                                TR_Student_RouteDMO obj_pre = new TR_Student_RouteDMO();
                                obj_pre.MI_Id = s.MI_Id;
                                obj_pre.ASMAY_Id = s.ASTA_FutureAY;
                                obj_pre.AMST_Id = s.AMST_Id;
                                obj_pre.TRSR_Date = DateTime.Now;
                                obj_pre.FMG_Id = q;
                                obj_pre.TRMR_Id = s.ASTA_PickUp_TRMR_Id;
                                // TRSR_PickupSchedule
                                obj_pre.TRSR_PickUpLocation = s.ASTA_PickUp_TRML_Id;
                                obj_pre.TRSR_PickUpMobileNo = e.AMST_MobileNo;
                                //TRSR_DropSchedule
                                obj_pre.TRSR_DropLocation = s.ASTA_Drop_TRML_Id;
                                obj_pre.TRSR_DropMobileNo = e.AMST_MobileNo;
                                obj_pre.TRSR_ApplicationNo = Convert.ToInt64(s.ASTA_ApplicationNo);
                                //TRSR_ActiveFlg
                                obj_pre.ASTA_Id = s.ASTA_Id;
                                obj_pre.TRMR_Drop_Route = s.ASTA_Drop_TRMR_Id;
                                alrdy_stu_list.Add(obj_pre);
                            }
                            //alrdy_stu_list.Add(obj_pre);
                        }
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


        public StudentRouteMappingDTO deactivate(StudentRouteMappingDTO data)
        {
            if (data.TRSR_Id > 0)
            {
                var result = _FeeGroupContext.TR_Student_RouteDMO.Single(t => t.TRSR_Id == data.TRSR_Id);
                var feelist = _FeeGroupContext.TR_Student_Route_FeeGroupDMO.Where(t => t.TRSR_Id == data.TRSR_Id).ToList();

                if (result.TRSR_ActiveFlg == true)
                {
                    result.TRSR_ActiveFlg = false;
                    result.UpdatedDate = DateTime.Now;
                    if (feelist.Count > 0)
                    {
                        foreach (var item in feelist)
                        {
                            item.TRSRFG_ActiveFlg = false;
                            _FeeGroupContext.Update(item);
                        }
                    }


                    _FeeGroupContext.Update(result);

                }
                else
                {
                    result.TRSR_ActiveFlg = true;
                    result.UpdatedDate = DateTime.Now;

                    if (feelist.Count > 0)
                    {
                        foreach (var item1 in feelist)
                        {
                            item1.TRSRFG_ActiveFlg = true;
                            _FeeGroupContext.Update(item1);
                        }
                    }
                    _FeeGroupContext.Update(result);
                }

                var flag = _FeeGroupContext.SaveChanges();
                if (flag > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            return data;
        }
        public StudentRouteMappingDTO searching(StudentRouteMappingDTO stu)
        {

            try
            {

                switch (stu.searchType)
                {
                    case "0":
                        string str = "";
                        stu.TempararyArrayhEADListnew = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                         from b in _FeeGroupContext.feeOpeningBalance
                                                         from c in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                         from d in _FeeGroupContext.admissioncls
                                                         from e in _FeeGroupContext.school_M_Section
                                                         where (a.AMST_Id == b.AMST_Id && b.MI_Id == stu.MI_Id && b.ASMAY_Id == stu.ASMAY_Id && b.AMST_Id == c.AMST_Id && a.AMST_Id == c.AMST_Id && b.ASMAY_Id == c.ASMAY_Id && c.ASMCL_Id == d.ASMCL_Id && c.ASMS_Id == e.ASMS_Id && (((a.AMST_FirstName.ToLower().Trim() + ' ' + (string.IsNullOrEmpty(a.AMST_MiddleName.Trim()) == true ? str : a.AMST_MiddleName.Trim())).Trim() + ' ' + (string.IsNullOrEmpty(a.AMST_LastName.Trim()) == true ? str : a.AMST_LastName.Trim())).Trim().Contains(stu.searchtext) || a.AMST_FirstName.StartsWith(stu.searchtext) || a.AMST_MiddleName.StartsWith(stu.searchtext) || a.AMST_LastName.StartsWith(stu.searchtext)))
                                                         select new StudentRouteMappingDTO
                                                         {
                                                             AMST_Id = a.AMST_Id,
                                                             AMST_FirstName = a.AMST_FirstName,
                                                             AMST_MiddleName = a.AMST_MiddleName,
                                                             AMST_LastName = a.AMST_LastName,
                                                             FMOB_Id = b.FMOB_Id,
                                                             ASMAY_Id = b.ASMAY_Id,
                                                             FMOB_EntryDate = b.FMOB_EntryDate,
                                                             FMOB_Student_Due = b.FMOB_Student_Due,
                                                             FMOB_Institution_Due = b.FMOB_Institution_Due,
                                                             ASMCL_ClassName = d.ASMCL_ClassName,
                                                             ASMC_SectionName = e.ASMC_SectionName,
                                                             //Amst_Id = a.AMST_Id,
                                                             //AMST_FirstName = a.AMST_FirstName,
                                                             //AMST_MiddleName = a.AMST_MiddleName,
                                                             //AMST_LastName = a.AMST_LastName + "-" + a.AMST_RegistrationNo,
                                                         }
                           ).Distinct().OrderByDescending(t => t.AMST_FirstName).ToList().ToArray();

                        break;
                    case "1":
                        stu.TempararyArrayhEADListnew = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                         from b in _FeeGroupContext.feeOpeningBalance
                                                         from c in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                         from d in _FeeGroupContext.admissioncls
                                                         from e in _FeeGroupContext.school_M_Section
                                                         where (a.AMST_Id == b.AMST_Id && b.MI_Id == stu.MI_Id && b.ASMAY_Id == stu.ASMAY_Id && b.AMST_Id == c.AMST_Id && a.AMST_Id == c.AMST_Id && b.ASMAY_Id == c.ASMAY_Id && c.ASMCL_Id == d.ASMCL_Id && c.ASMS_Id == e.ASMS_Id && d.ASMCL_ClassName.ToLower().Contains(stu.searchtext.ToLower()))
                                                         select new StudentRouteMappingDTO
                                                         {
                                                             AMST_Id = a.AMST_Id,
                                                             AMST_FirstName = a.AMST_FirstName,
                                                             AMST_MiddleName = a.AMST_MiddleName,
                                                             AMST_LastName = a.AMST_LastName,
                                                             FMOB_Id = b.FMOB_Id,
                                                             ASMAY_Id = b.ASMAY_Id,
                                                             FMOB_EntryDate = b.FMOB_EntryDate,
                                                             FMOB_Student_Due = b.FMOB_Student_Due,
                                                             FMOB_Institution_Due = b.FMOB_Institution_Due,
                                                             ASMCL_ClassName = d.ASMCL_ClassName,
                                                             ASMC_SectionName = e.ASMC_SectionName,
                                                             //Amst_Id = a.AMST_Id,
                                                             //AMST_FirstName = a.AMST_FirstName,
                                                             //AMST_MiddleName = a.AMST_MiddleName,
                                                             //AMST_LastName = a.AMST_LastName + "-" + a.AMST_RegistrationNo,
                                                         }
                           ).Distinct().OrderByDescending(t => t.ASMCL_ClassName).ToList().ToArray();
                        break;
                    case "2":
                        stu.TempararyArrayhEADListnew = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                         from b in _FeeGroupContext.feeOpeningBalance
                                                         from c in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                         from d in _FeeGroupContext.admissioncls
                                                         from e in _FeeGroupContext.school_M_Section
                                                         where (a.AMST_Id == b.AMST_Id && b.MI_Id == stu.MI_Id && b.ASMAY_Id == stu.ASMAY_Id && b.AMST_Id == c.AMST_Id && a.AMST_Id == c.AMST_Id && b.ASMAY_Id == c.ASMAY_Id && c.ASMCL_Id == d.ASMCL_Id && c.ASMS_Id == e.ASMS_Id && e.ASMC_SectionName.ToLower().Contains(stu.searchtext.ToLower()))
                                                         select new StudentRouteMappingDTO
                                                         {
                                                             AMST_Id = a.AMST_Id,
                                                             AMST_FirstName = a.AMST_FirstName,
                                                             AMST_MiddleName = a.AMST_MiddleName,
                                                             AMST_LastName = a.AMST_LastName,
                                                             FMOB_Id = b.FMOB_Id,
                                                             ASMAY_Id = b.ASMAY_Id,
                                                             FMOB_EntryDate = b.FMOB_EntryDate,
                                                             FMOB_Student_Due = b.FMOB_Student_Due,
                                                             FMOB_Institution_Due = b.FMOB_Institution_Due,
                                                             ASMCL_ClassName = d.ASMCL_ClassName,
                                                             ASMC_SectionName = e.ASMC_SectionName,
                                                             //Amst_Id = a.AMST_Id,
                                                             //AMST_FirstName = a.AMST_FirstName,
                                                             //AMST_MiddleName = a.AMST_MiddleName,
                                                             //AMST_LastName = a.AMST_LastName + "-" + a.AMST_RegistrationNo,
                                                         }
                           ).Distinct().OrderByDescending(t => t.ASMC_SectionName).ToList().ToArray();
                        break;
                    case "3":
                        stu.TempararyArrayhEADListnew = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                         from b in _FeeGroupContext.feeOpeningBalance
                                                         from c in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                         from d in _FeeGroupContext.admissioncls
                                                         from e in _FeeGroupContext.school_M_Section
                                                         where (a.AMST_Id == b.AMST_Id && b.MI_Id == stu.MI_Id && b.ASMAY_Id == stu.ASMAY_Id && b.AMST_Id == c.AMST_Id && a.AMST_Id == c.AMST_Id && b.ASMAY_Id == c.ASMAY_Id && c.ASMCL_Id == d.ASMCL_Id && c.ASMS_Id == e.ASMS_Id && b.FMOB_Student_Due.ToString().Contains(stu.searchnumber))
                                                         select new StudentRouteMappingDTO
                                                         {
                                                             AMST_Id = a.AMST_Id,
                                                             AMST_FirstName = a.AMST_FirstName,
                                                             AMST_MiddleName = a.AMST_MiddleName,
                                                             AMST_LastName = a.AMST_LastName,
                                                             FMOB_Id = b.FMOB_Id,
                                                             ASMAY_Id = b.ASMAY_Id,
                                                             FMOB_EntryDate = b.FMOB_EntryDate,
                                                             FMOB_Student_Due = b.FMOB_Student_Due,
                                                             FMOB_Institution_Due = b.FMOB_Institution_Due,
                                                             ASMCL_ClassName = d.ASMCL_ClassName,
                                                             ASMC_SectionName = e.ASMC_SectionName,
                                                             //Amst_Id = a.AMST_Id,
                                                             //AMST_FirstName = a.AMST_FirstName,
                                                             //AMST_MiddleName = a.AMST_MiddleName,
                                                             //AMST_LastName = a.AMST_LastName + "-" + a.AMST_RegistrationNo,
                                                         }
                          ).Distinct().OrderByDescending(t => t.FMOB_Student_Due).ToList().ToArray();
                        break;
                    case "4":

                        stu.TempararyArrayhEADListnew = (from a in _FeeGroupContext.AdmissionStudentDMO
                                                         from b in _FeeGroupContext.feeOpeningBalance
                                                         from c in _FeeGroupContext.School_Adm_Y_StudentDMO
                                                         from d in _FeeGroupContext.admissioncls
                                                         from e in _FeeGroupContext.school_M_Section
                                                         where (a.AMST_Id == b.AMST_Id && b.MI_Id == stu.MI_Id && b.ASMAY_Id == stu.ASMAY_Id && b.AMST_Id == c.AMST_Id && a.AMST_Id == c.AMST_Id && b.ASMAY_Id == c.ASMAY_Id && c.ASMCL_Id == d.ASMCL_Id && c.ASMS_Id == e.ASMS_Id && b.FMOB_Institution_Due.ToString().Contains(stu.searchnumber))
                                                         select new StudentRouteMappingDTO
                                                         {
                                                             AMST_Id = a.AMST_Id,
                                                             AMST_FirstName = a.AMST_FirstName,
                                                             AMST_MiddleName = a.AMST_MiddleName,
                                                             AMST_LastName = a.AMST_LastName,
                                                             FMOB_Id = b.FMOB_Id,
                                                             ASMAY_Id = b.ASMAY_Id,
                                                             FMOB_EntryDate = b.FMOB_EntryDate,
                                                             FMOB_Student_Due = b.FMOB_Student_Due,
                                                             FMOB_Institution_Due = b.FMOB_Institution_Due,
                                                             ASMCL_ClassName = d.ASMCL_ClassName,
                                                             ASMC_SectionName = e.ASMC_SectionName,
                                                             //Amst_Id = a.AMST_Id,
                                                             //AMST_FirstName = a.AMST_FirstName,
                                                             //AMST_MiddleName = a.AMST_MiddleName,
                                                             //AMST_LastName = a.AMST_LastName + "-" + a.AMST_RegistrationNo,
                                                         }
                          ).Distinct().OrderByDescending(t => t.FMOB_Institution_Due).ToList().ToArray();
                        break;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stu;
        }
        public StudentRouteMappingDTO get_loca_sches(StudentRouteMappingDTO data)
        {
            try
            {
                //   var locationlist = _FeeGroupContext.MasterLocationDMO.Where(a => a.MI_Id == data.MI_Id && a.TRML_ActiveFlg == true).Distinct().ToList();
                //   data.locationlist = locationlist.ToArray();


                //var   schedule = _FeeGroupContext.TR_Route_ScheduleDMO.Where(t => t.MI_Id == data.MI_Id && t.TRRSC_ActiveFlag == true).ToList();
                //   data.schedulelist = schedule.ToArray();

                //data.locationlist = (from a in _FeeGroupContext.Route_Location
                //                     from b in _FeeGroupContext.MasterLocationDMO
                //                     where (a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && b.TRML_ActiveFlg == true && a.TRML_Id == b.TRML_Id && a.TRMR_Id == data.TRMR_Id && a.TRMRL_ActiveFlag == true)
                //                     select b).Distinct().ToArray();
                //data.schedulelist = _FeeGroupContext.TR_Route_ScheduleDMO.Where(t => t.MI_Id == data.MI_Id && t.TRRSC_ActiveFlag == true && t.TRMR_Id == data.TRMR_Id).Distinct().ToArray();

                data.locationlist = (from a in _FeeGroupContext.Route_Location
                                     from b in _FeeGroupContext.MasterLocationDMO
                                     where (a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && b.TRML_ActiveFlg == true && a.TRML_Id == b.TRML_Id && a.TRMR_Id == data.TRMR_Id && a.TRMRL_ActiveFlag == true)
                                     select b).Distinct().ToArray();
                data.schedulelist = _FeeGroupContext.TR_Route_ScheduleDMO.Where(t => t.MI_Id == data.MI_Id && t.TRRSC_ActiveFlag == true).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public StudentRouteMappingDTO viewrecordspopup(StudentRouteMappingDTO data)
        {
            try
            {
                var locationlist = _FeeGroupContext.MasterLocationDMO.Where(a => a.MI_Id == data.MI_Id && a.TRML_ActiveFlg == true).Distinct().ToList();
                data.locationlist = locationlist.ToArray();

                data.reportdatelist1 = (from a in _FeeGroupContext.TR_Student_RouteDMO
                                        from b in _FeeGroupContext.AcademicYear
                                        from c in _FeeGroupContext.AdmissionStudentDMO
                                        from d in _FeeGroupContext.MasterRouteDMO
                                            //from e in _FeeGroupContext.FeeGroupDMO
                                        where (a.MI_Id == data.MI_Id && b.MI_Id == a.MI_Id && c.MI_Id == a.MI_Id && d.MI_Id == a.MI_Id && a.ASMAY_Id == b.ASMAY_Id && a.AMST_Id == c.AMST_Id && a.TRSR_Id == data.TRSR_Id && (a.TRMR_Id == d.TRMR_Id || d.TRMR_Id == a.TRMR_Drop_Route))
                                        //&& a.FMG_Id == e.FMG_Id &&      e.MI_Id == a.MI_Id
                                        select new StudentRouteMappingDTO
                                        {
                                            TRSR_Id = a.TRSR_Id,
                                            ASMAY_Id = a.ASMAY_Id,
                                            AMST_Id = a.AMST_Id,
                                            TRMR_Id = a.TRMR_Id,
                                            ASMAY_Year = b.ASMAY_Year,
                                            FMG_Id = a.FMG_Id,
                                            AMST_FirstName = ((c.AMST_FirstName == null ? " " : c.AMST_FirstName) + " " + (c.AMST_MiddleName == null ? " " : c.AMST_MiddleName) + " " + (c.AMST_LastName == null ? " " : c.AMST_LastName)).Trim(),
                                            TRSR_Date = a.TRSR_Date,
                                            TRSR_ApplicationNo = a.TRSR_ApplicationNo,
                                            TRMR_RouteName = d.TRMR_RouteName,
                                            // FMG_GroupName = e.FMG_GroupName,
                                            // TRSR_PickupSchedule = a.TRSR_PickupSchedule,
                                            //  PickUp_ScheduleName = a.TRSR_PickupSchedule != 0 ? _FeeGroupContext.TR_Route_ScheduleDMO.Where(t => t.MI_Id == data.MI_Id && t.TRRSC_Id == a.TRSR_PickupSchedule).FirstOrDefault().TRRSC_ScheduleName : "No",

                                            PickUp_Session = a.TRSR_PickupSession,
                                            pickup_SessionName = a.TRSR_PickupSession != 0 ? _FeeGroupContext.MsterSessionDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMS_Id == a.TRSR_PickupSession).FirstOrDefault().TRMS_SessionName : "No",

                                            TRSR_PickUpLocation = a.TRSR_PickUpLocation,
                                            PickUp_LocationName = a.TRSR_PickUpLocation != 0 ? locationlist.Where(t => t.TRML_Id == a.TRSR_PickUpLocation).FirstOrDefault().TRML_LocationName : "No",

                                            TRSR_PickUpMobileNo = a.TRSR_PickUpMobileNo,

                                            //TRSR_DropSchedule = a.TRSR_DropSchedule,
                                            // Drop_ScheduleName = a.TRSR_DropSchedule != 0 ? _FeeGroupContext.TR_Route_ScheduleDMO.Where(t => t.MI_Id == data.MI_Id && t.TRRSC_Id == a.TRSR_DropSchedule).FirstOrDefault().TRRSC_ScheduleName : "No",

                                            Drop_Session = a.TRSR_DropSession,
                                            drop_SessionName = a.TRSR_DropSession != 0 ? _FeeGroupContext.MsterSessionDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMS_Id == a.TRSR_DropSession).FirstOrDefault().TRMS_SessionName : "No",


                                            TRSR_DropLocation = a.TRSR_DropLocation,
                                            Drop_LocationName = a.TRSR_DropLocation != 0 ? locationlist.Where(t => t.TRML_Id == a.TRSR_DropLocation).FirstOrDefault().TRML_LocationName : "No",
                                            TRSR_DropMobileNo = a.TRSR_DropMobileNo,

                                            TRSR_ActiveFlg = a.TRSR_ActiveFlg
                                        }).Distinct().OrderByDescending(a => a.TRSR_Id).Take(10).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentRouteMappingDTO SearchByColumn(StudentRouteMappingDTO data)
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

                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TR_GETSTUDENTROUTEMAP_SEARCH";
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



                //switch (data.SearchColumn)
                //{
                //    case "0":
                //        data.reportdatelist = (from a in _FeeGroupContext.TR_Student_RouteDMO
                //                               from b in _FeeGroupContext.AcademicYear
                //                               from c in _FeeGroupContext.AdmissionStudentDMO
                //                               from d in _FeeGroupContext.MasterRouteDMO
                //                               from e in _FeeGroupContext.FeeGroupDMO
                //                               where (a.MI_Id == data.MI_Id && b.MI_Id == a.MI_Id && c.MI_Id == a.MI_Id && d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && a.ASMAY_Id == b.ASMAY_Id && a.AMST_Id == c.AMST_Id  && a.FMG_Id == e.FMG_Id && (c.AMST_FirstName.Contains(data.EnteredData) ||
                //                    c.AMST_MiddleName.Contains(data.EnteredData) || c.AMST_LastName.Contains(data.EnteredData)) && (a.TRMR_Id == d.TRMR_Id || a.TRMR_Drop_Route == d.TRMR_Id))
                //                               select new StudentRouteMappingDTO
                //                               {
                //                                   TRSR_Id = a.TRSR_Id,
                //                                   ASMAY_Id = a.ASMAY_Id,
                //                                   AMST_Id = a.AMST_Id,
                //                                   TRMR_Id = a.TRMR_Id,
                //                                   ASMAY_Year = b.ASMAY_Year,
                //                                   FMG_Id = a.FMG_Id,
                //                                   AMST_FirstName = ((c.AMST_FirstName == null ? " " : c.AMST_FirstName) + " " + (c.AMST_MiddleName == null ? " " : c.AMST_MiddleName) + " " + (c.AMST_LastName == null ? " " : c.AMST_LastName)).Trim(),
                //                                   TRSR_Date = a.TRSR_Date,
                //                                   TRSR_ApplicationNo = a.TRSR_ApplicationNo,
                //                                   TRMR_RouteName = d.TRMR_RouteName,
                //                                   FMG_GroupName = e.FMG_GroupName,
                //                                   TRSR_ActiveFlg = a.TRSR_ActiveFlg
                //                               }).Distinct().ToArray();
                //        break;
                //    case "1":

                //        DateTime date = DateTime.ParseExact(data.EnteredData, "dd/MM/yyyy", CultureInfo.InvariantCulture);


                //        data.reportdatelist = (from a in _FeeGroupContext.TR_Student_RouteDMO
                //                               from b in _FeeGroupContext.AcademicYear
                //                               from c in _FeeGroupContext.AdmissionStudentDMO
                //                               from d in _FeeGroupContext.MasterRouteDMO
                //                               from e in _FeeGroupContext.FeeGroupDMO
                //                               where (a.MI_Id == data.MI_Id && b.MI_Id == a.MI_Id && c.MI_Id == a.MI_Id && d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && a.ASMAY_Id == b.ASMAY_Id && a.AMST_Id == c.AMST_Id  && a.FMG_Id == e.FMG_Id && a.TRSR_Date.Date == Convert.ToDateTime(date.ToString("yyyy-MM-dd")) && (a.TRMR_Id == d.TRMR_Id || a.TRMR_Drop_Route == d.TRMR_Id))
                //                               select new StudentRouteMappingDTO
                //                               {
                //                                   TRSR_Id = a.TRSR_Id,
                //                                   ASMAY_Id = a.ASMAY_Id,
                //                                   AMST_Id = a.AMST_Id,
                //                                   TRMR_Id = a.TRMR_Id,
                //                                   ASMAY_Year = b.ASMAY_Year,
                //                                   FMG_Id = a.FMG_Id,
                //                                   AMST_FirstName = ((c.AMST_FirstName == null ? " " : c.AMST_FirstName) + " " + (c.AMST_MiddleName == null ? " " : c.AMST_MiddleName) + " " + (c.AMST_LastName == null ? " " : c.AMST_LastName)).Trim(),
                //                                   TRSR_Date = a.TRSR_Date,
                //                                   TRSR_ApplicationNo = a.TRSR_ApplicationNo,
                //                                   TRMR_RouteName = d.TRMR_RouteName,
                //                                   FMG_GroupName = e.FMG_GroupName,
                //                                   TRSR_ActiveFlg = a.TRSR_ActiveFlg
                //                               }).Distinct().ToArray();
                //        break;

                //    case "2":
                //        data.reportdatelist = (from a in _FeeGroupContext.TR_Student_RouteDMO
                //                               from b in _FeeGroupContext.AcademicYear
                //                               from c in _FeeGroupContext.AdmissionStudentDMO
                //                               from d in _FeeGroupContext.MasterRouteDMO
                //                               from e in _FeeGroupContext.FeeGroupDMO
                //                               where (a.MI_Id == data.MI_Id && b.MI_Id == a.MI_Id && c.MI_Id == a.MI_Id && d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && a.ASMAY_Id == b.ASMAY_Id && a.AMST_Id == c.AMST_Id  && a.FMG_Id == e.FMG_Id
                //                               && a.TRSR_ApplicationNo == Convert.ToInt64(data.EnteredData) &&  (a.TRMR_Id == d.TRMR_Id || a.TRMR_Drop_Route == d.TRMR_Id))
                //                               select new StudentRouteMappingDTO
                //                               {
                //                                   TRSR_Id = a.TRSR_Id,
                //                                   ASMAY_Id = a.ASMAY_Id,
                //                                   AMST_Id = a.AMST_Id,
                //                                   TRMR_Id = a.TRMR_Id,
                //                                   ASMAY_Year = b.ASMAY_Year,
                //                                   FMG_Id = a.FMG_Id,
                //                                   AMST_FirstName = ((c.AMST_FirstName == null ? " " : c.AMST_FirstName) + " " + (c.AMST_MiddleName == null ? " " : c.AMST_MiddleName) + " " + (c.AMST_LastName == null ? " " : c.AMST_LastName)).Trim(),
                //                                   TRSR_Date = a.TRSR_Date,
                //                                   TRSR_ApplicationNo = a.TRSR_ApplicationNo,
                //                                   TRMR_RouteName = d.TRMR_RouteName,
                //                                   FMG_GroupName = e.FMG_GroupName,
                //                                   TRSR_ActiveFlg = a.TRSR_ActiveFlg
                //                               }).Distinct().ToArray();


                //        break;
                //    case "3":
                //        data.reportdatelist = (from a in _FeeGroupContext.TR_Student_RouteDMO
                //                               from b in _FeeGroupContext.AcademicYear
                //                               from c in _FeeGroupContext.AdmissionStudentDMO
                //                               from d in _FeeGroupContext.MasterRouteDMO
                //                               from e in _FeeGroupContext.FeeGroupDMO
                //                               where (a.MI_Id == data.MI_Id && b.MI_Id == a.MI_Id && c.MI_Id == a.MI_Id && d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && a.ASMAY_Id == b.ASMAY_Id && a.AMST_Id == c.AMST_Id  && a.FMG_Id == e.FMG_Id
                //                               && d.TRMR_RouteName.Contains(data.EnteredData) && (a.TRMR_Id == d.TRMR_Id || a.TRMR_Drop_Route == d.TRMR_Id))
                //                               select new StudentRouteMappingDTO
                //                               {
                //                                   TRSR_Id = a.TRSR_Id,
                //                                   ASMAY_Id = a.ASMAY_Id,
                //                                   AMST_Id = a.AMST_Id,
                //                                   TRMR_Id = a.TRMR_Id,
                //                                   ASMAY_Year = b.ASMAY_Year,
                //                                   FMG_Id = a.FMG_Id,
                //                                   AMST_FirstName = ((c.AMST_FirstName == null ? " " : c.AMST_FirstName) + " " + (c.AMST_MiddleName == null ? " " : c.AMST_MiddleName) + " " + (c.AMST_LastName == null ? " " : c.AMST_LastName)).Trim(),
                //                                   TRSR_Date = a.TRSR_Date,
                //                                   TRSR_ApplicationNo = a.TRSR_ApplicationNo,
                //                                   TRMR_RouteName = d.TRMR_RouteName,
                //                                   FMG_GroupName = e.FMG_GroupName,
                //                                   TRSR_ActiveFlg = a.TRSR_ActiveFlg
                //                               }).Distinct().ToArray();
                //        break;

                //    case "4":
                //        data.reportdatelist = (from a in _FeeGroupContext.TR_Student_RouteDMO
                //                               from b in _FeeGroupContext.AcademicYear
                //                               from c in _FeeGroupContext.AdmissionStudentDMO
                //                               from d in _FeeGroupContext.MasterRouteDMO
                //                               from e in _FeeGroupContext.FeeGroupDMO
                //                               where (a.MI_Id == data.MI_Id && b.MI_Id == a.MI_Id && c.MI_Id == a.MI_Id && d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && a.ASMAY_Id == b.ASMAY_Id && a.AMST_Id == c.AMST_Id  && a.FMG_Id == e.FMG_Id
                //                               && e.FMG_GroupName.Contains(data.EnteredData) && (a.TRMR_Id == d.TRMR_Id || a.TRMR_Drop_Route == d.TRMR_Id))
                //                               select new StudentRouteMappingDTO
                //                               {
                //                                   TRSR_Id = a.TRSR_Id,
                //                                   ASMAY_Id = a.ASMAY_Id,
                //                                   AMST_Id = a.AMST_Id,
                //                                   TRMR_Id = a.TRMR_Id,
                //                                   ASMAY_Year = b.ASMAY_Year,
                //                                   FMG_Id = a.FMG_Id,
                //                                   AMST_FirstName = ((c.AMST_FirstName == null ? " " : c.AMST_FirstName) + " " + (c.AMST_MiddleName == null ? " " : c.AMST_MiddleName) + " " + (c.AMST_LastName == null ? " " : c.AMST_LastName)).Trim(),
                //                                   TRSR_Date = a.TRSR_Date,
                //                                   TRSR_ApplicationNo = a.TRSR_ApplicationNo,
                //                                   TRMR_RouteName = d.TRMR_RouteName,
                //                                   FMG_GroupName = e.FMG_GroupName,
                //                                   TRSR_ActiveFlg = a.TRSR_ActiveFlg
                //                               }).Distinct().ToArray();
                //        break;
                //    default:

                //        data.reportdatelist = (from a in _FeeGroupContext.TR_Student_RouteDMO
                //                               from b in _FeeGroupContext.AcademicYear
                //                               from c in _FeeGroupContext.AdmissionStudentDMO
                //                               from d in _FeeGroupContext.MasterRouteDMO
                //                               from e in _FeeGroupContext.FeeGroupDMO
                //                               where (a.MI_Id == data.MI_Id && b.MI_Id == a.MI_Id && c.MI_Id == a.MI_Id && d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && a.ASMAY_Id == b.ASMAY_Id && a.AMST_Id == c.AMST_Id  && a.FMG_Id == e.FMG_Id && (a.TRMR_Id == d.TRMR_Id || a.TRMR_Drop_Route == d.TRMR_Id))
                //                               select new StudentRouteMappingDTO
                //                               {
                //                                   TRSR_Id = a.TRSR_Id,
                //                                   ASMAY_Id = a.ASMAY_Id,
                //                                   AMST_Id = a.AMST_Id,
                //                                   TRMR_Id = a.TRMR_Id,
                //                                   ASMAY_Year = b.ASMAY_Year,
                //                                   FMG_Id = a.FMG_Id,
                //                                   AMST_FirstName = ((c.AMST_FirstName == null ? " " : c.AMST_FirstName) + " " + (c.AMST_MiddleName == null ? " " : c.AMST_MiddleName) + " " + (c.AMST_LastName == null ? " " : c.AMST_LastName)).Trim(),
                //                                   TRSR_Date = a.TRSR_Date,
                //                                   TRSR_ApplicationNo = a.TRSR_ApplicationNo,
                //                                   TRMR_RouteName = d.TRMR_RouteName,
                //                                   FMG_GroupName = e.FMG_GroupName,
                //                                   TRSR_ActiveFlg = a.TRSR_ActiveFlg
                //                               }).Distinct().ToArray();
                //        break;
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentRouteMappingDTO checkduplicateno(StudentRouteMappingDTO data)
        {
            try
            {
                //var checkapplictionnumber = _FeeGroupContext.TR_Student_RouteDMO.Where(a => a.MI_Id == data.MI_Id && a.TRSR_ApplicationNo.Equals(data.TRSR_ApplicationNo)).ToList();
                var checkapplictionnumber = _FeeGroupContext.TR_Student_RouteDMO.Where(a => a.MI_Id == data.MI_Id && a.TRSR_ApplicationNo.Equals(data.TRSR_ApplicationNo) && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id != data.AMST_Id).ToList();
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

    }
}

