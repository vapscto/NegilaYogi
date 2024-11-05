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
using System.Data;
using System.Data.SqlClient;
using DomainModel.Model.com.vaps.admission;
using System.Dynamic;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeGroupWiseStudentReportImpl :interfaces.FeeGroupWiseStudentReportInterface
    {
        private static ConcurrentDictionary<string, FeeGroupWiseStudentReportDTO> _login =
             new ConcurrentDictionary<string, FeeGroupWiseStudentReportDTO>();

        public FeeGroupContext _db;

        public FeeGroupWiseStudentReportImpl(FeeGroupContext db)
        {
            _db = db;
        }

        public FeeGroupWiseStudentReportDTO getInitailData(FeeGroupWiseStudentReportDTO data)
        {
            FeeGroupWiseStudentReportDTO ctdo = new FeeGroupWiseStudentReportDTO();
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _db.AcademicYear.Where(y=>y.Is_Active==true && y.MI_Id==data.MI_Id).OrderByDescending(y => y.ASMAY_Order).ToList();
                ctdo.YearList = allyear.Distinct().GroupBy(y=>y.ASMAY_Year).Select(y=>y.First()).ToArray();

                List<castecategoryDMO> Allname = new List<castecategoryDMO>();
                Allname = _db.castecategoryDMO.OrderByDescending(a => a.CreatedDate).ToList();
                ctdo.Class_Category_List = Allname.ToArray();



            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return ctdo;
        }



        
        public FeeGroupWiseStudentReportDTO GetStudent(FeeGroupWiseStudentReportDTO data)
        {
            try
            {


                if (data.ASMCL_Id==0)
                {
                    data.Student_Name_List = (from a in _db.School_Adm_Y_StudentDMO
                                              from b in _db.AdmissionStudentDMO
                                              where (a.AMST_Id == b.AMST_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                              select new FeeGroupWiseStudentReportDTO
                                              {
                                                  AMST_Id = a.AMST_Id,
                                                  AMST_FirstName = b.AMST_FirstName,
                                                  AMST_MiddleName = b.AMST_MiddleName,
                                                  AMST_LastName = b.AMST_LastName,
                                                  AMAY_RollNo = a.AMAY_RollNo,
                                              }).Distinct().OrderBy(t => t.AMST_Id).ToArray();
                }

                else if (data.ASMCL_Id != 0 && data.ASMS_Id == 0)
                {
                    data.Student_Name_List = (from a in _db.School_Adm_Y_StudentDMO
                                              from b in _db.AdmissionStudentDMO
                                              where (a.AMST_Id == b.AMST_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id)
                                              select new FeeGroupWiseStudentReportDTO
                                              {
                                                  AMST_Id = a.AMST_Id,
                                                  AMST_FirstName = b.AMST_FirstName,
                                                  AMST_MiddleName = b.AMST_MiddleName,
                                                  AMST_LastName = b.AMST_LastName,
                                                  AMAY_RollNo = a.AMAY_RollNo,
                                              }).Distinct().OrderBy(t => t.AMST_Id).ToArray();
                }


                else
                {
                    data.Student_Name_List = (from a in _db.School_Adm_Y_StudentDMO
                                              from b in _db.AdmissionStudentDMO
                                              where (a.AMST_Id == b.AMST_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id)
                                              select new FeeGroupWiseStudentReportDTO
                                              {
                                                  AMST_Id = a.AMST_Id,
                                                  AMST_FirstName = b.AMST_FirstName,
                                                  AMST_MiddleName = b.AMST_MiddleName,
                                                  AMST_LastName = b.AMST_LastName,
                                                  AMAY_RollNo = a.AMAY_RollNo,
                                              }).Distinct().OrderBy(t => t.AMST_Id).ToArray();
                }

               

            }
            catch (Exception ex)
            {

                throw ex;
            }


            return data;
        }
        public FeeGroupWiseStudentReportDTO Getclass(FeeGroupWiseStudentReportDTO data)
        {
            try
            {

                data.Class_List=(from a in _db.School_Adm_Y_StudentDMO
                 from b in _db.School_M_Class
                 where (a.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id && b.ASMCL_ActiveFlag==true)
                 select new FeeGroupWiseStudentReportDTO
                 {
                    
                    ASMCL_Id=b.ASMCL_Id,
                     ASMCL_ClassName = b.ASMCL_ClassName
                     
                 }).Distinct().ToArray();

                data.Fee_Group_List = (from a in _db.Yearlygroups
                                   from b in _db.FeeGroupDMO
                                   from c in _db.FEeGroupLoginPreviledgeDMO
                where (a.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id && b.FMG_Id ==c.FMG_ID && b.FMG_ActiceFlag == true && c.User_Id==data.user_id)
                                   select new FeeGroupWiseStudentReportDTO
                                   {

                                       FMG_Id = b.FMG_Id,
                                       Fee_Group = b.FMG_GroupName

                                   }).Distinct().ToArray();

                

            }
            catch (Exception ex)
            {

                throw ex;
            }


            return data;
        }

        
        public FeeGroupWiseStudentReportDTO GetSection(FeeGroupWiseStudentReportDTO data)
        {
            try
            {

                data.Section_List = (from a in _db.School_Adm_Y_StudentDMO
                                     from b in _db.school_M_Section
                                     where (a.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id && b.ASMC_ActiveFlag == 1 && a.ASMCL_Id==data.ASMCL_Id)
                                   select new FeeGroupWiseStudentReportDTO
                                   {

                                       ASMS_Id = b.ASMS_Id,
                                       ASMC_SectionName = b.ASMC_SectionName

                                   }).Distinct().ToArray();
                

            }
            catch (Exception ex)
            {

                throw ex;
            }


            return data;
        }

        public FeeGroupWiseStudentReportDTO SearchData(FeeGroupWiseStudentReportDTO Clscatag)
        {
            
            string Where = "";
            string fmg_Id = "0";
            if (Clscatag.radio_selected=="all" || Clscatag.radio_selected== "individual" || Clscatag.radio_selected=="unmapped")
            { 
           
            List<long> fmG_ids = new List<long>();
            foreach (var item in Clscatag.selectfeegroup)
            {
                fmG_ids.Add(item.fmG_Id);
            }

            for (int s = 0; s < fmG_ids.Count(); s++)
            {
                fmg_Id = fmg_Id + ',' + fmG_ids[s].ToString();
            }
            }
            
            if (Clscatag.radio_selected == "all") {
                if (Clscatag.ASMAY_Id > 0 && Clscatag.selectfeegroup.Length > 0)
                {

                    Where = " " + "AND dbo.Fee_Student_Status.MI_Id=" + Clscatag.MI_Id + " AND dbo.Fee_Student_Status.ASMAY_Id=" + Clscatag.ASMAY_Id + " AND dbo.Adm_School_Y_Student.ASMAY_Id=" + Clscatag.ASMAY_Id + " AND dbo.Fee_Master_Group.FMG_Id in (" + fmg_Id + " )";
                }               
            }

            if (Clscatag.radio_selected == "individual" && Clscatag.Stud_Sel == "S")
            {
                if (Clscatag.ASMAY_Id > 0 && Clscatag.selectfeegroup.Length > 0 && Clscatag.ASMCL_Id > 0 && Clscatag.ASMS_Id > 0  )
                {

                    Where = " " + "AND dbo.Fee_Student_Status.MI_Id=" + Clscatag.MI_Id + " AND dbo.Fee_Student_Status.ASMAY_Id=" + Clscatag.ASMAY_Id + " AND dbo.Fee_Master_Group.FMG_Id in (" + fmg_Id + ") AND dbo.Adm_School_M_Class.ASMCL_Id=" + Clscatag.ASMCL_Id + " AND dbo.Adm_School_M_Section.ASMS_Id=" + Clscatag.ASMS_Id + "  AND dbo.Adm_School_Y_Student.ASMAY_Id = " + Clscatag.ASMAY_Id + " AND dbo.Adm_M_Student.AMST_ActiveFlag='1' AND dbo.Adm_School_Y_Student.AMAY_ActiveFlag='1'  AND     dbo.Adm_M_Student.AMST_SOL='" + Clscatag.Stud_Sel + "' ";
                }

                else if (Clscatag.ASMAY_Id > 0 && Clscatag.selectfeegroup.Length > 0 && Clscatag.ASMCL_Id == 0 && Clscatag.ASMS_Id == 0)
                {
                    Where = " " + "AND dbo.Fee_Student_Status.MI_Id=" + Clscatag.MI_Id + " AND dbo.Fee_Student_Status.ASMAY_Id=" + Clscatag.ASMAY_Id + " AND dbo.Fee_Master_Group.FMG_Id in (" + fmg_Id + ") AND dbo.Adm_School_Y_Student.ASMAY_Id = " + Clscatag.ASMAY_Id + " AND dbo.Adm_M_Student.AMST_ActiveFlag='1' AND dbo.Adm_School_Y_Student.AMAY_ActiveFlag='1'  AND     dbo.Adm_M_Student.AMST_SOL='" + Clscatag.Stud_Sel + "' ";
                }
                
                else
                {
                    Where = " " + "AND dbo.Fee_Student_Status.MI_Id=" + Clscatag.MI_Id + " AND dbo.Fee_Student_Status.ASMAY_Id=" + Clscatag.ASMAY_Id + " AND dbo.Adm_School_Y_Student.ASMAY_Id=" + Clscatag.ASMAY_Id + " AND dbo.Fee_Master_Group.FMG_Id in (" + fmg_Id + ") AND dbo.Adm_School_M_Class.ASMCL_Id=" + Clscatag.ASMCL_Id + " AND dbo.Adm_M_Student.AMST_ActiveFlag='1' AND dbo.Adm_School_Y_Student.AMAY_ActiveFlag='1'  AND     dbo.Adm_M_Student.AMST_SOL='" + Clscatag.Stud_Sel + "' ";
                }


            }


            if (Clscatag.radio_selected == "individual" && Clscatag.Stud_Sel == "L")
            {
                if (Clscatag.ASMAY_Id > 0 && Clscatag.selectfeegroup.Length > 0 && Clscatag.ASMCL_Id > 0 && Clscatag.ASMS_Id > 0 )
                {
                    Where = " " + "AND dbo.Fee_Student_Status.MI_Id=" + Clscatag.MI_Id + " AND dbo.Fee_Student_Status.ASMAY_Id=" + Clscatag.ASMAY_Id + " AND dbo.Adm_School_Y_Student.ASMAY_Id=" + Clscatag.ASMAY_Id + " AND dbo.Fee_Master_Group.FMG_Id in (" + fmg_Id + ") AND dbo.Adm_School_M_Class.ASMCL_Id=" + Clscatag.ASMCL_Id + " AND dbo.Adm_School_M_Section.ASMS_Id=" + Clscatag.ASMS_Id + " AND dbo.Adm_M_Student.AMST_ActiveFlag='0' AND dbo.Adm_School_Y_Student.AMAY_ActiveFlag='0'  AND     dbo.Adm_M_Student.AMST_SOL='" + Clscatag.Stud_Sel + "' ";
                }
                else if (Clscatag.ASMAY_Id > 0 && Clscatag.selectfeegroup.Length > 0 && Clscatag.ASMCL_Id == 0 && Clscatag.ASMS_Id == 0)
                {
                    Where = " " + "AND dbo.Fee_Student_Status.MI_Id=" + Clscatag.MI_Id + " AND dbo.Fee_Student_Status.ASMAY_Id=" + Clscatag.ASMAY_Id + " AND dbo.Adm_School_Y_Student.ASMAY_Id=" + Clscatag.ASMAY_Id + " AND dbo.Fee_Master_Group.FMG_Id in (" + fmg_Id + ")  AND dbo.Adm_M_Student.AMST_ActiveFlag='0' AND dbo.Adm_School_Y_Student.AMAY_ActiveFlag='0'  AND     dbo.Adm_M_Student.AMST_SOL='" + Clscatag.Stud_Sel + "' ";
                }
                else
                {
                    Where = " " + "AND dbo.Fee_Student_Status.MI_Id=" + Clscatag.MI_Id + " AND dbo.Fee_Student_Status.ASMAY_Id=" + Clscatag.ASMAY_Id + " AND dbo.Adm_School_Y_Student.ASMAY_Id=" + Clscatag.ASMAY_Id + " AND dbo.Fee_Master_Group.FMG_Id in (" + fmg_Id + ") AND dbo.Adm_School_M_Class.ASMCL_Id=" + Clscatag.ASMCL_Id + " AND dbo.Adm_M_Student.AMST_ActiveFlag='0' AND dbo.Adm_School_Y_Student.AMAY_ActiveFlag='0'  AND     dbo.Adm_M_Student.AMST_SOL='" + Clscatag.Stud_Sel + "' ";
                }
            }

            if (Clscatag.radio_selected == "individual" && Clscatag.Stud_Sel == "D")
            {
                if (Clscatag.ASMAY_Id > 0 && Clscatag.selectfeegroup.Length > 0 && Clscatag.ASMCL_Id > 0 && Clscatag.ASMS_Id > 0)
                {

                    Where = " " + "AND dbo.Fee_Student_Status.MI_Id=" + Clscatag.MI_Id + " AND dbo.Fee_Student_Status.ASMAY_Id=" + Clscatag.ASMAY_Id + " AND dbo.Fee_Master_Group.FMG_Id in (" + fmg_Id + ") AND dbo.Adm_School_M_Class.ASMCL_Id=" + Clscatag.ASMCL_Id + " AND dbo.Adm_School_M_Section.ASMS_Id=" + Clscatag.ASMS_Id + " AND dbo.Adm_School_Y_Student.ASMAY_Id=" + Clscatag.ASMAY_Id + " AND dbo.Adm_M_Student.AMST_ActiveFlag='1' AND dbo.Adm_School_Y_Student.AMAY_ActiveFlag='1'  AND     dbo.Adm_M_Student.AMST_SOL='" + Clscatag.Stud_Sel + "' ";
                }
                else
                {
                    Where = " " + "AND dbo.Fee_Student_Status.MI_Id=" + Clscatag.MI_Id + " AND dbo.Fee_Student_Status.ASMAY_Id=" + Clscatag.ASMAY_Id + " AND dbo.Adm_School_Y_Student.ASMAY_Id=" + Clscatag.ASMAY_Id + " AND dbo.Fee_Master_Group.FMG_Id in (" + fmg_Id + ") AND dbo.Adm_School_M_Class.ASMCL_Id=" + Clscatag.ASMCL_Id + " AND dbo.Adm_M_Student.AMST_ActiveFlag='1' AND dbo.Adm_School_Y_Student.AMAY_ActiveFlag='1'  AND     dbo.Adm_M_Student.AMST_SOL='" + Clscatag.Stud_Sel + "' ";
                }

            }
            if (Clscatag.radio_selected == "individual" && Clscatag.Stud_Sel == "N")
            {
                if (Clscatag.ASMAY_Id > 0 && Clscatag.selectfeegroup.Length > 0 && Clscatag.ASMCL_Id > 0 && Clscatag.ASMS_Id > 0)
                {

                    Where = " " + "AND dbo.Fee_Student_Status.MI_Id=" + Clscatag.MI_Id + " AND dbo.Adm_M_Student.ASMAY_Id=" + Clscatag.ASMAY_Id + " AND dbo.Adm_School_Y_Student.ASMAY_Id=" + Clscatag.ASMAY_Id + " AND dbo.Fee_Master_Group.FMG_Id in (" + fmg_Id + ") AND dbo.Adm_School_M_Class.ASMCL_Id=" + Clscatag.ASMCL_Id + " AND dbo.Adm_School_M_Section.ASMS_Id=" + Clscatag.ASMS_Id + " AND dbo.Adm_M_Student.AMST_ActiveFlag='1' AND dbo.Adm_School_Y_Student.AMAY_ActiveFlag='1'  AND     dbo.Adm_M_Student.AMST_SOL='S' ";

                }
                else
                {
                    Where = " " + "AND dbo.Fee_Student_Status.MI_Id=" + Clscatag.MI_Id + " AND dbo.Adm_M_Student.ASMAY_Id=" + Clscatag.ASMAY_Id + "AND dbo.Adm_School_Y_Student.ASMAY_Id=" + Clscatag.ASMAY_Id + " AND dbo.Fee_Master_Group.FMG_Id in (" + fmg_Id + ") AND dbo.Adm_School_M_Class.ASMCL_Id=" + Clscatag.ASMCL_Id + " AND dbo.Adm_M_Student.AMST_ActiveFlag='1' AND dbo.Adm_School_Y_Student.AMAY_ActiveFlag='1'  AND     dbo.Adm_M_Student.AMST_SOL='S' ";
                }


            }
            if (Clscatag.radio_selected == "student_wise")
            {
                if (Clscatag.ASMAY_Id > 0  && Clscatag.ASMCL_Id > 0 && Clscatag.ASMS_Id > 0 && Clscatag.Stud_Id > 0)
                {

                    Where = " " + "AND dbo.Fee_Student_Status.MI_Id=" + Clscatag.MI_Id + " AND dbo.Fee_Student_Status.ASMAY_Id=" + Clscatag.ASMAY_Id + " AND dbo.Adm_School_Y_Student.ASMAY_Id=" + Clscatag.ASMAY_Id + " AND dbo.Adm_School_M_Class.ASMCL_Id=" + Clscatag.ASMCL_Id + " AND dbo.Adm_School_M_Section.ASMS_Id=" + Clscatag.ASMS_Id + " AND dbo.Adm_M_Student.AMST_Id=" + Clscatag.Stud_Id;
                }
                else
                {
                    Where = " " + "AND dbo.Fee_Student_Status.MI_Id=" + Clscatag.MI_Id + " AND dbo.Fee_Student_Status.ASMAY_Id=" + Clscatag.ASMAY_Id + " AND dbo.Adm_School_Y_Student.ASMAY_Id=" + Clscatag.ASMAY_Id + " AND dbo.Adm_School_M_Class.ASMCL_Id=" + Clscatag.ASMCL_Id + " AND dbo.Adm_M_Student.AMST_Id=" + Clscatag.Stud_Id;
                }


            }

            if (Clscatag.radio_selected == "admCat")
            {
                if (Clscatag.ASMAY_Id > 0 && Clscatag.ASMCL_Id > 0 && Clscatag.ASMS_Id > 0 && Clscatag.FMCC_Id > 0)
                {

                    Where = " " + "AND dbo.Fee_Student_Status.MI_Id=" + Clscatag.MI_Id + " AND dbo.Fee_Student_Status.ASMAY_Id=" + Clscatag.ASMAY_Id + " AND dbo.Adm_School_Y_Student.ASMAY_Id=" + Clscatag.ASMAY_Id + " AND dbo.Adm_School_M_Class.ASMCL_Id=" + Clscatag.ASMCL_Id + " AND dbo.Adm_School_M_Section.ASMS_Id=" + Clscatag.ASMS_Id + " AND dbo.IVRM_Master_Caste_Category.IMCC_Id=" + Clscatag.FMCC_Id;
                }
                else
                {
                    Where = " " + "AND dbo.Fee_Student_Status.MI_Id=" + Clscatag.MI_Id + " AND dbo.Fee_Student_Status.ASMAY_Id=" + Clscatag.ASMAY_Id + " AND dbo.Adm_School_Y_Student.ASMAY_Id=" + Clscatag.ASMAY_Id + " AND dbo.Adm_School_M_Class.ASMCL_Id=" + Clscatag.ASMCL_Id + " AND dbo.IVRM_Master_Caste_Category.IMCC_Id=" + Clscatag.FMCC_Id;
                }


            }
            
            
             if (Clscatag.radio_selected == "unmapped")
            {
              
                if (Clscatag.ASMAY_Id > 0)
                {

                    // Where = " " + "AND dbo.Fee_Student_Status.MI_Id=" + Clscatag.MI_Id + " AND dbo.Fee_Student_Status.ASMAY_Id=" + Clscatag.ASMAY_Id + " AND dbo.Adm_School_Y_Student.ASMAY_Id=" + Clscatag.ASMAY_Id + " AND dbo.Adm_School_M_Class.ASMCL_Id=" + Clscatag.ASMCL_Id + " AND dbo.Adm_School_M_Section.ASMS_Id=" + Clscatag.ASMS_Id + " AND dbo.IVRM_Master_Caste_Category.IMCC_Id=" + Clscatag.FMCC_Id;
                    Where = " " + " Adm_M_Student.MI_Id = " + Clscatag.MI_Id + " and Adm_School_Y_Student.ASMAY_Id =" + Clscatag.ASMAY_Id + " and AMAY_ActiveFlag = 1 and AMST_ActiveFlag = 1   and Adm_M_Student.AMST_Id not in (select AMST_Id from Fee_Student_Status where MI_Id =" + Clscatag.MI_Id + " and ASMAY_Id =" + Clscatag.ASMAY_Id + " and fmg_id in (" + fmg_Id + "))";
                }
                else
                {
                    Where = " " + " Adm_M_Student.MI_Id = " + Clscatag.MI_Id + " and Adm_School_Y_Student.ASMAY_Id =" + Clscatag.ASMAY_Id + "  and AMAY_ActiveFlag = 1 and AMST_ActiveFlag = 1 and fee_master_group.fmg_id in (" + fmg_Id + ")  and Adm_M_Student.AMST_Id not in (select AMST_Id from Fee_Student_Status where MI_Id =" + Clscatag.MI_Id + " and ASMAY_Id =" + Clscatag.ASMAY_Id + ")";
                }


            }

            if (Clscatag.radio_selected != "Termwise")
            {
                try
                {


                    List<FeeGroupWiseStudentReportDTO> result = new List<FeeGroupWiseStudentReportDTO>();

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Group_Wise_Student_Report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 5000000;
                        cmd.Parameters.Add(new SqlParameter("@Where",
                           SqlDbType.VarChar)
                        {
                            Value = Where
                        });
                        cmd.Parameters.Add(new SqlParameter("@flag",
                     SqlDbType.VarChar)
                        {
                            Value = Clscatag.radio_selected
                        });
                        cmd.Parameters.Add(new SqlParameter("@userid",
                          SqlDbType.VarChar)
                        {
                            Value = Clscatag.user_id
                        });


                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();
                        //var data = cmd.ExecuteNonQuery();
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
                            Clscatag.FHWR_searchdatalist = retObject.ToArray();

                        }



                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            else
            {
                Clscatag.Class_Category_List = _db.feeTr.Where(a => a.MI_Id== Clscatag.MI_Id && a.FMT_ActiveFlag==true).ToArray();
     
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FeeTermwiseStudentDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 300000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.VarChar)
                    {
                        Value = Clscatag.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                       SqlDbType.VarChar)
                    {
                        Value = Clscatag.ASMAY_Id
                    });


                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
               SqlDbType.VarChar)
                    {
                        Value = Clscatag.Stud_Id
                    });
                    



                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {

                        using (var dataReader =  cmd.ExecuteReader())
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
                        Clscatag.FHWR_searchdatalist = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FeeTermwiseStudentDetails_datewise";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 300000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.VarChar)
                    {
                        Value = Clscatag.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                       SqlDbType.VarChar)
                    {
                        Value = Clscatag.ASMAY_Id
                    });


                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
               SqlDbType.VarChar)
                    {
                        Value = Clscatag.Stud_Id
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
                        Clscatag.paiddatelist = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return Clscatag;
        }


    }
}
