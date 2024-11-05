using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.Transport;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;

namespace TransportServiceHub.Services
{
    public class StdRouteLocationMapImpl : Interfaces.StdRouteLocationMapInterface
    {
        private static ConcurrentDictionary<string, StdRouteLocationMapDTO> _login =
        new ConcurrentDictionary<string, StdRouteLocationMapDTO>();
        public DomainModelMsSqlServerContext _db;
        public TransportContext _context;
        ILogger<StdRouteLocationMapImpl> _areaimpl;
        public StdRouteLocationMapImpl(ILogger<StdRouteLocationMapImpl> areaimpl, TransportContext context, DomainModelMsSqlServerContext db)
        {

            _areaimpl = areaimpl;
            _context = context;
            _db = db;
        }

        public StdRouteLocationMapDTO getdata(StdRouteLocationMapDTO data)
        {
           // StdRouteLocationMapDTO data = new StdRouteLocationMapDTO();
            //data.MI_Id = id;
            try
            {
                //List<MasterAcademic> year = new List<MasterAcademic>();
                //year = _context.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).ToList();//&& t.ASMAY_Id==data.ASMAY_Id
                //data.acayear = year.Distinct().ToArray();
                data.acayear = _context.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).ToArray();

                List<School_M_Class> allclas = new List<School_M_Class>();
                allclas = _context.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                data.classlist = allclas.Distinct().ToArray();

                List<School_M_Section> allsetion = new List<School_M_Section>();
                allsetion = _context.School_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).ToList();
                data.sectionlist = allsetion.Distinct().ToArray();

               
                List<MasterRouteDMO> busro = new List<MasterRouteDMO>();
                busro = _context.MasterRouteDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMR_ActiveFlg == true).ToList();
                data.busroutelist = busro.ToArray();

                

                var locationlist = _context.MasterLocationDMO.Where(a => a.MI_Id == data.MI_Id && a.TRML_ActiveFlg == true).Distinct().ToList();
                data.locationlist = locationlist.ToArray();

                List<TR_Route_ScheduleDMO> schedule = new List<TR_Route_ScheduleDMO>();
                schedule = _context.TR_Route_ScheduleDMO.Where(t => t.MI_Id == data.MI_Id && t.TRRSC_ActiveFlag == true).ToList();
                data.schedulelist = schedule.ToArray();


                data.grouplist = (from a in _context.FeeHeadDMO
                                  from b in _context.FeeYearlygroupHeadMappingDMO
                                  from c in _context.FeeGroupDMO
                                  where (a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && a.FMH_ActiveFlag == true && a.FMH_Flag == "T" && a.FMH_Id == b.FMH_Id && b.FMG_Id == c.FMG_Id && c.FMG_ActiceFlag == true && b.FYGHM_ActiveFlag == "1")
                                  select c).Distinct().ToArray();





                data.reportdatelist = (from a in _context.TR_student_LocMappingDMO
                                       from b in _context.AcademicYear
                                       from c in _context.Adm_M_Student
                                       from d in _context.MasterRouteDMO
                                       from e in _context.FeeGroupDMO
                                       from f in _context.MasterLocationDMO
                                       where (a.MI_Id==b.MI_Id && a.MI_Id==b.MI_Id && a.MI_Id==c.MI_Id && a.MI_Id==d.MI_Id && a.MI_Id==e.MI_Id && a.MI_Id==f.MI_Id && a.MI_Id==data.MI_Id  && a.ASMAY_Id==b.ASMAY_Id  && a.AMST_Id==c.AMST_Id && a.TRMR_Id==d.TRMR_Id && a.TRML_Id==f.TRML_Id && a.FMG_Id==e.FMG_Id)
                                       select new StdRouteLocationMapDTO
                                       {
                                           TRSLM_Id = a.TRSLM_Id,
                                           ASMAY_Id = a.ASMAY_Id,
                                           AMST_Id = a.AMST_Id,
                                           TRMR_Id = a.TRMR_Id,
                                           ASMAY_Year = b.ASMAY_Year,
                                           FMG_Id = a.FMG_Id,
                                           AMST_FirstName = ((c.AMST_FirstName == null ? " " : c.AMST_FirstName) + " " + (c.AMST_MiddleName == null ? " " : c.AMST_MiddleName) + " " + (c.AMST_LastName == null ? " " : c.AMST_LastName)).Trim(),
                                          TRML_Id=f.TRML_Id,
                                           PickUp_LocationName = f.TRML_LocationName,
                                           TRMR_RouteName = d.TRMR_RouteName,
                                           FMG_GroupName = e.FMG_GroupName,
                                           TRSLM_ActiveFlag = a.TRSLM_ActiveFlag,
                                           AMST_AdmNo=c.AMST_AdmNo
                                     //  }).Distinct().OrderByDescending(a => a.TRSLM_Id).Take(10).ToArray();

            }).Distinct().OrderByDescending(a => a.TRSLM_Id).ToArray();

        }
            catch (Exception ex)
            {
                Console.WriteLine("Transport Error Driver Char savedata" + ex.Message);
            }

            return data;
        }




        public StdRouteLocationMapDTO get_cls_secs(StdRouteLocationMapDTO data)
        {
            try
            {

                //data.savegrplst = (from gh in _context.FeeYearlygroupHeadMappingDMO
                //                   where (gh.ASMAY_Id == data.ASMAY_Id && gh.MI_Id == data.MI_Id)
                //                   select new StdRouteLocationMapDTO
                //                   {
                //                       FMH_Id = gh.FMH_Id
                //                   }).Distinct().ToArray();


                //List<long> GrpId = new List<long>();
                //foreach (var item in data.savegrplst)
                //{
                //    GrpId.Add(item.FMH_Id);
                //}

                //data.headlist = (from b in _context.FeeHeadDMO
                //                where GrpId.Contains(b.FMH_Id)

                //               select new StdRouteLocationMapDTO
                //               {
                //                   FMH_Id = b.FMH_Id,
                //                   FMH_FeeName = b.FMH_FeeName,

                //               }
                //       ).Distinct().ToArray();


                data.classlist = (from a in _context.School_Adm_Y_StudentDMO
                                  from b in _context.School_M_Class
                                  where (a.ASMAY_Id == data.ASMAY_Id && a.AMAY_ActiveFlag == 1 && b.MI_Id == data.MI_Id && b.ASMCL_ActiveFlag == true && a.ASMCL_Id == b.ASMCL_Id)
                                  select b).Distinct().ToArray();

                data.sectionlist = (from a in _context.School_Adm_Y_StudentDMO
                                    from b in _context.School_M_Section
                                    where (a.ASMAY_Id == data.ASMAY_Id && a.AMAY_ActiveFlag == 1 && b.MI_Id == data.MI_Id && b.ASMC_ActiveFlag == 1 && a.ASMS_Id == b.ASMS_Id)
                                    select b).Distinct().ToArray();
                data.grouplist = (from a in _context.FeeHeadDMO
                                  from b in _context.FeeYearlygroupHeadMappingDMO
                                  from c in _context.FeeGroupDMO
                                  where (a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && a.FMH_ActiveFlag == true && a.FMH_Flag == "T" && a.FMH_Id == b.FMH_Id && b.FMG_Id == c.FMG_Id && c.FMG_ActiceFlag == true && b.ASMAY_Id == data.ASMAY_Id)
                                  select c).Distinct().ToArray();



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }


        public StdRouteLocationMapDTO getreport(StdRouteLocationMapDTO data)
        {

            try
            {
                var stdlist = _context.TR_student_LocMappingDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id ).ToList();
                if (stdlist.Count >0)
                {
                    //var feemapstd = _context.TR_student_LocMappingDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id).Select(t => t.AMST_Id).ToList();

                    //data.admsudentslist = (from a in _context.School_Adm_Y_StudentDMO
                    //                       from b in _context.Adm_M_Student
                    //                       where (a.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id && b.AMST_ActiveFlag == 1 && a.ASMS_Id == data.ASMS_Id && a.ASMCL_Id == data.ASMCL_Id && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && a.AMST_Id == b.AMST_Id ) && !feemapstd.Contains(a.AMST_Id)
                    //                       select new StdRouteLocationMapDTO
                    //                       {
                    //                           AMST_Id = b.AMST_Id,
                    //                           AMST_AdmNo = b.AMST_AdmNo,
                    //                           AMST_FirstName = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + " " + (b.AMST_LastName == null ? " " : b.AMST_LastName)).Trim(),
                    //                           AMST_MobileNo = b.AMST_MobileNo
                    //                       }).Distinct().ToArray();


                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                       
                        cmd.CommandText = "TRN_STD_LOCATIONMAPPING_GET_STDDETAILS";
                        cmd.CommandTimeout = 100000;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_ID",
                            SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                           SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });
                       
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
             SqlDbType.VarChar)
                        {
                            Value = data.ASMCL_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
             SqlDbType.VarChar)
                        {
                            Value = data.ASMS_Id
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
                else
                {
                    data.admsudentslist = (from a in _context.School_Adm_Y_StudentDMO
                                           from b in _context.Adm_M_Student
                                           where (a.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id && b.AMST_ActiveFlag == 1 && a.ASMS_Id == data.ASMS_Id && a.ASMCL_Id == data.ASMCL_Id && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && a.AMST_Id == b.AMST_Id)
                                           select new StdRouteLocationMapDTO
                                           {
                                               AMST_Id = b.AMST_Id,
                                               AMST_AdmNo = b.AMST_AdmNo,
                                               AMST_FirstName = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + " " + (b.AMST_LastName == null ? " " : b.AMST_LastName)).Trim(),
                                               AMST_MobileNo = b.AMST_MobileNo
                                           }).Distinct().ToArray();

                }





                List<TR_student_LocMappingDMO> alrdy_stu_list = new List<TR_student_LocMappingDMO>();
                foreach (StdRouteLocationMapDTO e in data.admsudentslist)
                {
                    var count_stu = _context.TR_student_LocMappingDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMST_Id == e.AMST_Id).ToList().Count;
                    var date_stu = DateTime.Now;


                    var list_stu_date = (from t in _context.TR_student_LocMappingDMO
                                         where (t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMST_Id == e.AMST_Id )
                                         select new TR_student_LocMappingDMO
                                         {
                                            
                                             MI_Id = t.MI_Id,
                                             TRSLM_Id = t.TRSLM_Id,
                                             ASMAY_Id = t.ASMAY_Id,
                                             AMST_Id = t.AMST_Id,
                                             FMG_Id = t.FMG_Id,
                                             TRMR_Id = t.TRMR_Id,
                                             TRML_Id = t.TRML_Id,
                                      
                                         }).Distinct().ToList();

                    if (list_stu_date.Count == 0)
                    {
                        
                       
                          
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



         public StdRouteLocationMapDTO getreportedit(StdRouteLocationMapDTO data)
        {

            try
            {
                var stdlist = _context.TR_student_LocMappingDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.TRSLM_Id==data.TRSLM_Id).ToList();
                if (stdlist.Count >0)
                {
                   // var feemapstd = _context.TR_student_LocMappingDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id).Select(t => t.AMST_Id).ToList();

                    data.admsudentslist = (from a in _context.School_Adm_Y_StudentDMO
                                           from b in _context.Adm_M_Student
                                           where (a.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id
                                          // && b.AMST_ActiveFlag == 1 && a.ASMS_Id == data.ASMS_Id && a.ASMCL_Id == data.ASMCL_Id && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" 
                                           && a.AMST_Id == b.AMST_Id && a.AMST_Id==data.AMST_Id)
                                           //&& !feemapstd.Contains(a.AMST_Id)
                                           select new StdRouteLocationMapDTO
                                           {
                                               AMST_Id = b.AMST_Id,
                                               AMST_AdmNo = b.AMST_AdmNo,
                                               AMST_FirstName = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + " " + (b.AMST_LastName == null ? " " : b.AMST_LastName)).Trim(),
                                               AMST_MobileNo = b.AMST_MobileNo,
                                               ASMCL_Id=a.ASMCL_Id,
                                               ASMAY_Id = a.ASMAY_Id,
                                               ASMS_Id=a.ASMS_Id
                                           }).Distinct().ToArray();
                }
                else
                {
                    data.admsudentslist = (from a in _context.School_Adm_Y_StudentDMO
                                           from b in _context.Adm_M_Student
                                           where (a.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id && b.AMST_ActiveFlag == 1 && a.ASMS_Id == data.ASMS_Id && a.ASMCL_Id == data.ASMCL_Id && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && a.AMST_Id == b.AMST_Id)
                                           select new StdRouteLocationMapDTO
                                           {
                                               AMST_Id = b.AMST_Id,
                                               AMST_AdmNo = b.AMST_AdmNo,
                                               AMST_FirstName = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + " " + (b.AMST_LastName == null ? " " : b.AMST_LastName)).Trim(),
                                               AMST_MobileNo = b.AMST_MobileNo
                                           }).Distinct().ToArray();

                }





                List<TR_student_LocMappingDMO> alrdy_stu_list = new List<TR_student_LocMappingDMO>();
                foreach (StdRouteLocationMapDTO e in data.admsudentslist)
                {
                    var count_stu = _context.TR_student_LocMappingDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMST_Id == e.AMST_Id && t.TRSLM_Id==data.TRSLM_Id).ToList().Count;
                    var date_stu = DateTime.Now;


                    var list_stu_date = (from t in _context.TR_student_LocMappingDMO
                                         where (t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMST_Id == e.AMST_Id && t.TRSLM_Id == data.TRSLM_Id)
                                         select new TR_student_LocMappingDMO
                                         {
                                            
                                             MI_Id = t.MI_Id,
                                             TRSLM_Id = t.TRSLM_Id,
                                             ASMAY_Id = t.ASMAY_Id,
                                             AMST_Id = t.AMST_Id,
                                             FMG_Id = t.FMG_Id,
                                             TRMR_Id = t.TRMR_Id,
                                             TRML_Id = t.TRML_Id,
                                      
                                         }).Distinct().ToList();

                    if (list_stu_date.Count == 0)
                    {
                        
                       
                          
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
        public StdRouteLocationMapDTO on_pic_route_change(StdRouteLocationMapDTO data)
        {
            try
            {
                data.picloclist = (from a in _context.Route_Location
                                   from b in _context.MasterLocationDMO
                                   where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.TRML_Id == b.TRML_Id && a.TRMR_Id == data.TRMR_Id && b.TRML_ActiveFlg == true && a.TRMRL_ActiveFlag == true
                                   select new StdRouteLocationMapDTO
                                   {

                                       TRML_LocationName = b.TRML_LocationName,
                                       TRML_Id = b.TRML_Id,
                                       TRMRL_Order = a.TRMRL_Order


                                   }).Distinct().OrderBy(x => x.TRMRL_Order).ToArray();

             
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public StdRouteLocationMapDTO get_sections(StdRouteLocationMapDTO data)
        {
            try
            {
                data.sectionlist = (from a in _context.School_Adm_Y_StudentDMO
                                    from b in _context.School_M_Section
                                    where (a.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id && b.ASMC_ActiveFlag == 1 && a.ASMS_Id == b.ASMS_Id && a.ASMCL_Id == data.ASMCL_Id && a.AMAY_ActiveFlag == 1)
                                    select b).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public StdRouteLocationMapDTO savedata(StdRouteLocationMapDTO data
            )
        {
            try
            {
                for (int i = 0; i < data.stddatalist.Length; i++)
                {

                    if (data.TRSLM_Id>0)
                    {
                        var result = _context.TR_student_LocMappingDMO.Single(t => t.TRSLM_Id == data.TRSLM_Id);
                        _context.Remove(result);
                       
                        foreach (var item in data.Inst_data)
                        {
                            if (item.amsT_Id == data.stddatalist[i].AMST_Id)
                            {
                                var regstatus = _context.Database.ExecuteSqlCommand("transportfeemapping_Delete @p0,@p1,@p2", data.MI_Id, data.ASMAY_Id, data.TRSLM_Id, item.FTI_Id);
                            }
                        }
                        var flg = _context.SaveChanges();
                    }


                    var feegroupid = _context.TR_Location_FeeGroup_MappingDMO.Where(g => g.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id && g.TRML_Id == data.stddatalist[i].TRSR_PickUpLocation && g.TRLFM_ActiveFlag == true).ToList();

                    if (feegroupid.Count > 0)
                    {
                        foreach (var item in feegroupid)
                        {
                            TR_student_LocMappingDMO res = new TR_student_LocMappingDMO();
                            res.MI_Id = data.MI_Id;
                            res.ASMAY_Id = data.ASMAY_Id;
                            res.AMST_Id = data.stddatalist[i].AMST_Id;
                            res.TRML_Id = data.stddatalist[i].TRSR_PickUpLocation;
                            res.TRMR_Id = data.stddatalist[i].TRMR_Id;
                            res.FMG_Id = item.FMG_Id;
                            res.TRSLM_ActiveFlag = true;
                            res.CreatedDate = DateTime.Now;
                            res.UpdatedDate = DateTime.Now;
                            _context.Add(res);
                        }




                    }



                    var exists = _context.SaveChanges();
                    if (exists >= 1)

                    {
                        data.returnval = true;
                        string fti_idss = "0";
                        List<long> instid = new List<long>();
                        foreach (var item in data.Inst_data)
                        {
                            if (item.amsT_Id == data.stddatalist[i].AMST_Id)
                            {
                                instid.Add(Convert.ToInt64(item.FTI_Id));
                            }
                        }
                        for (int j = 0; j < instid.Count; j++)
                        {
                            fti_idss = fti_idss + "," + instid[j].ToString();
                        }

                        var feegroupid1 = _context.TR_Location_FeeGroup_MappingDMO.Where(g => g.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id && g.TRML_Id == data.stddatalist[i].TRSR_PickUpLocation && g.TRLFM_ActiveFlag == true).ToList();

                        if (feegroupid1.Count > 0)
                        {
                            var regstatus = _context.Database.ExecuteSqlCommand("transportfeemappingtemp @p0,@p1,@p2,@p3,@p4", data.MI_Id, data.ASMAY_Id, data.stddatalist[i].AMST_Id, data.stddatalist[i].TRSR_PickUpLocation, fti_idss);

                        }




                    }
                    else
                    {
                        data.returnval = false;
                    }




                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }

        public StdRouteLocationMapDTO check_feegroup(StdRouteLocationMapDTO data)
        {
            try
            {
                var check_fee = _context.TR_Location_FeeGroup_MappingDMO.Where(g => g.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id && g.TRML_Id == data.TRML_Id && g.TRLFM_ActiveFlag == true).Distinct().ToList();

                data.check_fee = check_fee.ToArray();

                if (check_fee.Count > 0)
                {
                    string allfmg_ids = "0";
                    List<long> fmg_ids = new List<long>();
                    foreach (var item in check_fee)
                    {
                        fmg_ids.Add(item.FMG_Id);
                    }
                    for (int i = 0; i < fmg_ids.Count; i++)
                    {
                        allfmg_ids = allfmg_ids + "," + fmg_ids[i].ToString();
                    }

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TRN_GET_LOCATIONWISE_FEE_INSTALLMENT";
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
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                          SqlDbType.VarChar)
                        {
                            Value = data.ASMCL_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@FMG_Id",
                        SqlDbType.VarChar)
                        {
                            Value = allfmg_ids
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                         SqlDbType.VarChar)
                        {
                            Value = data.AMST_Id
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
                                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.installmentlist = retObject.ToArray();
                        }

                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TRN_GET_LOCATIONWISE_FEE_INSTALLMENT_SAVED";
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
                            Value = data.AMST_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@FMG_Id",
                        SqlDbType.VarChar)
                        {
                            Value = allfmg_ids
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
                                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.installmentlist_saved = retObject.ToArray();
                        }

                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TRN_GET_LOCATIONWISE_FEE_INSTALLMENT_PAID";
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
                            Value = data.AMST_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@FMG_Id",
                        SqlDbType.VarChar)
                        {
                            Value = allfmg_ids
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
                                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.installmentlist_Paid = retObject.ToArray();
                        }

                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public StdRouteLocationMapDTO deactivate(StdRouteLocationMapDTO data)
        {
            if (data.TRSLM_Id > 0)
            {
                var result = _context.TR_student_LocMappingDMO.Single(t => t.TRSLM_Id == data.TRSLM_Id);
                _context.Remove(result);

                var regstatus = _context.Database.ExecuteSqlCommand("transportfeemapping_Delete @p0,@p1,@p2", data.MI_Id, data.ASMAY_Id, data.TRSLM_Id);

                var flag = _context.SaveChanges();
                if (flag == 1)
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
        public StdRouteLocationMapDTO Getreportdetails(StdRouteLocationMapDTO data)

        {
            try
            {
                //List<StdRouteLocationMapDTO> result1 = new List<StdRouteLocationMapDTO>();
                
                //using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "TripBillAmountProc";

                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.CommandTimeout = 8000000;

                //    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                //        SqlDbType.BigInt)
                //    {
                //        Value = data.MI_Id
                //    });
                    
                //    cmd.Parameters.Add(new SqlParameter("@TRHG_Id",
                //                SqlDbType.BigInt)
                //    {
                //        Value = data.TRHG_Id
                //    });
                  
                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();

                //    var retObject = new List<dynamic>();
                //    try
                //    {
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
                //        data.griddata = retObject.ToArray();
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

        public StdRouteLocationMapDTO get_data(StdRouteLocationMapDTO data)
        {

            try
            {
                var acaorder = _context.AcademicYearDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.MI_Id==data.MI_Id).ToList();
                var acaorder1 = acaorder[0].ASMAY_Order - 1;
                var academicyear = _context.AcademicYearDMO.Where(a => a.ASMAY_Order == acaorder1 && a.MI_Id==data.MI_Id).ToList();

                var stdlist = _context.TR_student_LocMappingDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == academicyear[0].ASMAY_Id).ToList();
                if (stdlist.Count > 0)
                {
                 
                    data.admsudentslist = (from a in _context.School_Adm_Y_StudentDMO
                                           from b in _context.Adm_M_Student
                                           from  c in _context.TR_student_LocMappingDMO
                                           where (a.ASMAY_Id == academicyear[0].ASMAY_Id && b.MI_Id == data.MI_Id
                                              && a.AMST_Id == b.AMST_Id && a.AMST_Id==c.AMST_Id && a.ASMCL_Id==data.ASMCL_Id && a.ASMS_Id==data.ASMS_Id)
                                     
                                           select new StdRouteLocationMapDTO
                                           {
                                               AMST_Id = b.AMST_Id,
                                               AMST_AdmNo = b.AMST_AdmNo,
                                               AMST_FirstName = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + " " + (b.AMST_LastName == null ? " " : b.AMST_LastName)).Trim(),
                                               AMST_MobileNo = b.AMST_MobileNo,
                                               ASMCL_Id = a.ASMCL_Id,
                                               //ASMAY_Id = a.ASMAY_Id,
                                               ASMS_Id = a.ASMS_Id
                                           }).Distinct().ToArray();
                }
                else
                {
                    data.admsudentslist = (from a in _context.School_Adm_Y_StudentDMO
                                           from b in _context.Adm_M_Student
                                           where (a.ASMAY_Id == academicyear[0].ASMAY_Id && b.MI_Id == data.MI_Id && b.AMST_ActiveFlag == 1 &&  a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && a.AMST_Id == b.AMST_Id)
                                           select new StdRouteLocationMapDTO
                                           {
                                               AMST_Id = b.AMST_Id,
                                               AMST_AdmNo = b.AMST_AdmNo,
                                               AMST_FirstName = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + " " + (b.AMST_LastName == null ? " " : b.AMST_LastName)).Trim(),
                                               AMST_MobileNo = b.AMST_MobileNo
                                           }).Distinct().ToArray();

                }





                List<TR_student_LocMappingDMO> alrdy_stu_list = new List<TR_student_LocMappingDMO>();
                foreach (StdRouteLocationMapDTO e in data.admsudentslist)
                {
                    var count_stu = _context.TR_student_LocMappingDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMST_Id == e.AMST_Id && t.TRSLM_Id == data.TRSLM_Id).ToList().Count;
                    var date_stu = DateTime.Now;


                    var list_stu_date = (from t in _context.TR_student_LocMappingDMO
                                         where (t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMST_Id == e.AMST_Id)
                                         select new TR_student_LocMappingDMO
                                         {

                                             MI_Id = t.MI_Id,
                                             TRSLM_Id = t.TRSLM_Id,
                                             //ASMAY_Id = t.ASMAY_Id,
                                             AMST_Id = t.AMST_Id,
                                             FMG_Id = t.FMG_Id,
                                             TRMR_Id = t.TRMR_Id,
                                             TRML_Id = t.TRML_Id,

                                         }).Distinct().ToList();

                    if (list_stu_date.Count == 0)
                    {



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

    }
}
