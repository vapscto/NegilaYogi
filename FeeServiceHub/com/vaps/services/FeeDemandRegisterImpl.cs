using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;
using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using DomainModel.Model.com.vaps.Fee;
using System.Dynamic;
using System.Collections;
using Newtonsoft.Json;
using DomainModel.Model.com.vaps.admission;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeDemandRegisterImpl:interfaces.FeeDemandRegisterInterface
    {
        FeeGroupContext _feegroupcontext;
        public FeeDemandRegisterImpl(FeeGroupContext fee)
        {
            _feegroupcontext = fee;
        }
        public async Task<FeeDemandRegisterDTO> getinitialdata(FeeDemandRegisterDTO obj)
        {
            try
            {
              
                obj.academicYearList = await  _feegroupcontext.AcademicYear.Where(d => d.MI_Id == obj.MI_Id && d.Is_Active == true).OrderByDescending(d=>d.ASMAY_Order).ToArrayAsync();
                obj.classList = await _feegroupcontext.School_M_Class.Where(d => d.MI_Id == obj.MI_Id && d.ASMCL_ActiveFlag == true).OrderBy(d=>d.ASMCL_Order).ToArrayAsync();
                obj.sectionList = await _feegroupcontext.school_M_Section.Where(d => d.MI_Id == obj.MI_Id && d.ASMC_ActiveFlag == 1).OrderBy(d=>d.ASMC_Order).ToArrayAsync();
                var query = await (from m in _feegroupcontext.feeGGG
                             from n in _feegroupcontext.feegm
                             from o in _feegroupcontext.FeeGroupDMO
                             where m.FMGG_Id == n.FMGG_Id && m.FMG_Id == o.FMG_Id && n.MI_Id == obj.MI_Id && o.FMG_ActiceFlag == true
                             select new FeeDemandRegisterDTO
                             {
                                 FMG_Id=o.FMG_Id,
                                 FMG_GroupName=o.FMG_GroupName
                             }).Distinct().ToListAsync();
                obj.groupList = query.ToArray();
                obj.customgrpList =await _feegroupcontext.feegm.Where(d => d.MI_Id == obj.MI_Id && d.FMGG_ActiveFlag == true).ToArrayAsync();
                obj.userNamesList = await (from a in _feegroupcontext.applicationUser 
                                           from b in _feegroupcontext.Fee_Payment
                                           where a.Id==b.user_id && b.MI_Id==obj.MI_Id                                   
                select new FeeTrailAuditDTO
                {
                    NormalizedUserName = a.NormalizedUserName,
                    userId = a.Id,
                }
              ).Distinct().ToArrayAsync();
                var feeconfig = await _feegroupcontext.feemastersettings.Where(d => d.MI_Id == obj.MI_Id).ToListAsync();
                obj.feeconfiguration = feeconfig.ToArray();
                obj.admissinConfiguration =await _feegroupcontext.AdmissionStandardDMO.Where(d => d.MI_Id == obj.MI_Id).ToArrayAsync();

               
              
                if(feeconfig.Count > 0)
                {
                   if(feeconfig.FirstOrDefault().FMC_GroupOrTermFlg.Equals("T"))
                    {
                        obj.termsList = (from a in _feegroupcontext.feeMTH
                                         from b in _feegroupcontext.feeTr
                                         from c in _feegroupcontext.FEeGroupLoginPreviledgeDMO
                                         where (a.FMH_Id == c.FMH_Id && a.FMT_Id == b.FMT_Id && a.MI_Id == obj.MI_Id && c.User_Id == obj.User_Id)
                                         select new FeeStudentTransactionDTO
                                         {
                                             FMT_Name = b.FMT_Name,
                                             FMT_Id = a.FMT_Id,
                                         }
                      ).Distinct().ToArray();
                        // obj.termsList =await _feegroupcontext.feeTr.Where(d => d.MI_Id == obj.MI_Id && d.FMT_ActiveFlag == true).ToArrayAsync();
                        
                      
                    }
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }
        public async Task<FeeDemandRegisterDTO> getStudentByYrClsSec(FeeDemandRegisterDTO data)
        {
            try
            {
                var status = data.selectedStudType.Select(d => d.value).ToList();
                data.studentList =await (from a in _feegroupcontext.AdmissionStudentDMO
                                   from b in _feegroupcontext.School_Adm_Y_StudentDMO
                                   where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1 && status.Contains(a.AMST_SOL) && b.ASMAY_Id==data.ASMAY_Id && b.ASMCL_Id==data.ASMCL_Id && b.ASMS_Id==data.ASMS_Id)
                                   group new { a, b } by a.AMST_Id into g
                                   select new FeeDemandRegisterDTO
                                   {
                                       AMST_Id = g.FirstOrDefault().a.AMST_Id,
                                       regNo = g.FirstOrDefault().a.AMST_RegistrationNo,
                                       admNo = g.FirstOrDefault().a.AMST_AdmNo,
                                       rollNo = g.FirstOrDefault().b.AMAY_RollNo,
                                       studentName = g.FirstOrDefault().a.AMST_FirstName + ' ' + g.FirstOrDefault().a.AMST_MiddleName ?? "" + ' ' + g.FirstOrDefault().a.AMST_LastName ?? ""
                                   }
                            ).Distinct().ToArrayAsync();
                if(data.studentList.Length > 0)
                {
                    data.studentCount = data.studentList.Length;
                }
                else
                {
                    data.studentCount = 0;
                }

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public async Task<FeeDemandRegisterDTO> getgroupByCG(FeeDemandRegisterDTO data)
        {
            try
            {

                var fmggIds = data.selectedCGList.Select(d => d.FMGG_Id).ToList();
                var query = await (from m in _feegroupcontext.feeGGG
                                   from n in _feegroupcontext.feegm
                                   from o in _feegroupcontext.FeeGroupDMO
                                   where m.FMGG_Id == n.FMGG_Id && m.FMG_Id == o.FMG_Id && n.MI_Id == data.MI_Id && o.FMG_ActiceFlag == true && 
                                   fmggIds.Contains(m.FMGG_Id)
                                   select new FeeDemandRegisterDTO
                                   {
                                       FMG_Id = o.FMG_Id,
                                       FMG_GroupName = o.FMG_GroupName
                                   }).Distinct().ToListAsync();
                data.groupList = query.ToArray();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public async Task<FeeDemandRegisterDTO> getReport(FeeDemandRegisterDTO data)
        {
            try
            {
                string fmgg_id = "0";
                string fmg_id = "0";
                string status = "'A'";
                try
                {

                    if (data.selectedCGList != null)
                    {
                        foreach (FeeDemandRegisterDTO actv in data.selectedCGList)
                        {
                            fmgg_id = fmgg_id + "," + actv.FMGG_Id;
                        }
                    }
                    if (data.selectedGroup != null)
                    {
                        foreach (FeeDemandRegisterDTO actv in data.selectedGroup)
                        {
                            fmg_id = fmg_id + "," + actv.FMG_Id;
                        }
                    }
                    if (data.selectedStudType != null)
                    {
                        foreach (FeeDemandRegisterDTO stu in data.selectedStudType)
                        {
                            status = status + "," + "'"+stu.value+"'";
                        }
                    }
                   
                    using (var cmd = _feegroupcontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandTimeout = 3000;
                        cmd.CommandText = "Fee_Demand_Register_Report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@mi_id",
                            SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@asmay_Id",
                           SqlDbType.BigInt)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@asmcl_id",
                           SqlDbType.BigInt)
                        {
                            Value = data.ASMCL_Id
                        });
                        if (data.ASMS_Id != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@amsc_id",
                             SqlDbType.BigInt)
                            {
                                Value = data.ASMS_Id
                            });
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@amsc_id",
                             SqlDbType.BigInt)
                            {
                                Value = 0
                            });
                        }
                        if (data.AMST_Id != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@amst_id",
                            SqlDbType.BigInt)
                            {
                                Value = data.AMST_Id
                            });
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@amst_id",
                            SqlDbType.BigInt)
                            {
                                Value = 0
                            });
                        }
                       
                       
                        cmd.Parameters.Add(new SqlParameter("@fmgg_id",
                            SqlDbType.VarChar)
                        {
                            Value = fmgg_id
                        });
                        cmd.Parameters.Add(new SqlParameter("@fmg_id",
                           SqlDbType.VarChar)
                        {
                            Value = fmg_id
                        });
                        if (data.Date != null)
                        {
                            var d = data.Date.Value.Date.ToString("dd-MM-yyyy");

                            cmd.Parameters.Add(new SqlParameter("@date",
                                                   SqlDbType.VarChar)
                            {
                                Value = d
                            });
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@date",
                                                  SqlDbType.VarChar)
                            {
                                Value = ""
                            });
                        }

                        if (data.FromDate != null)
                        {
                            var d = data.FromDate.Value.Date.ToString("dd-MM-yyyy");
                            cmd.Parameters.Add(new SqlParameter("@fromdate",
                       SqlDbType.VarChar)
                            {
                                Value = d
                            });
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@fromdate",
                       SqlDbType.VarChar)
                            {
                                Value = ""
                            });
                        }
                        if (data.ToDate != null)
                        {
                            var d = data.ToDate.Value.Date.ToString("dd-MM-yyyy");
                            cmd.Parameters.Add(new SqlParameter("@todate",
                                                 SqlDbType.VarChar)
                            {
                                Value = d

                            });
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@todate",
                     SqlDbType.VarChar)
                            {
                                Value = ""
                            });
                        }
                       
                      
                        cmd.Parameters.Add(new SqlParameter("@type",
                      SqlDbType.VarChar)
                        {
                            Value = data.type
                        });
                        cmd.Parameters.Add(new SqlParameter("@Stu_type",
                     SqlDbType.VarChar)
                        {
                            Value = status
                        });

                        cmd.Parameters.Add(new SqlParameter("@newstud",
                  SqlDbType.VarChar)
                        {
                            Value = data.newstud
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        List<FeeDemandRegisterInstallmentDTO> result1 = new List<FeeDemandRegisterInstallmentDTO>();
                        List<FeeDemandRegisterInstallmentDTO> result2 = new List<FeeDemandRegisterInstallmentDTO>();
                        List<Adm_M_Student> studentlist = new List<Adm_M_Student>();

                        try
                        {
                            using (var dataReader = await cmd.ExecuteReaderAsync())
                            {
                                
                                while (await dataReader.ReadAsync())
                                {

                                    for (int i = 4; i < dataReader.FieldCount; i++)
                                    {                                        
                                        result1.Add(new FeeDemandRegisterInstallmentDTO
                                        {
                                            AMST_Id = Convert.ToInt32(dataReader["AMST_Id"].ToString()),
                                            feename = dataReader["FeeName"].ToString(),
                                            studentname = dataReader["StudentName"].ToString(),
                                            admno = dataReader["Adm_no"].ToString(),
                                            installmentname = dataReader.GetName(i),
                                            installmentfees = dataReader.IsDBNull(i) ? "0": dataReader.GetValue(i).ToString()                                            
                                        });
                                    }

                                    data.FeeDemandRegisterInstallment = result1.ToList();
                                }
                                if(result1.Count > 0)
                                {
                                    var install = result1.Select(t => t.installmentname).Distinct().ToList();
                                    data.installmentdetails = result1.Select(t => t.installmentname).Distinct().ToArray();
                                    var stud = result1.Distinct<FeeDemandRegisterInstallmentDTO>(new progressEqualityComparer()).ToList();
                                    data.studentdetails = result1.Distinct<FeeDemandRegisterInstallmentDTO>(new progressEqualityComparer()).ToArray();
                                    var feesss = result1.Select(d => d.feename).Distinct().OrderBy(d => d).ToList();
                                    data.FeeNames = feesss.ToArray();
                                 
                                    for (int i = 0; i < stud.Count; i++)
                                    {
                                        for (int j = 0; j < feesss.Count; j++)
                                        {
                                            var find = data.FeeDemandRegisterInstallment.Find(d => d.feename == feesss[j] && d.AMST_Id == stud[i].AMST_Id);
                                            if (find != null)
                                            {
                                               
                                                for (int k = 0; k < install.Count; k++)
                                                {
                                                    var find2 = data.FeeDemandRegisterInstallment.Find(d => d.feename == feesss[j] && d.AMST_Id == stud[i].AMST_Id && d.installmentname == install[k]);
                                                    
                                                    result2.Add(new FeeDemandRegisterInstallmentDTO
                                                    {
                                                        AMST_Id = stud[i].AMST_Id,
                                                        feename = feesss[j],
                                                        studentname = stud[i].studentname,
                                                        admno = stud[i].admno,
                                                        installmentname = install[k],
                                                        installmentfees = find2.installmentfees,
                                                    });
                                                }
                                            }
                                            if (find == null)
                                            {
                                                for (int k = 0; k < install.Count; k++)
                                                {
                                                    result2.Add(new FeeDemandRegisterInstallmentDTO
                                                    {
                                                        AMST_Id = stud[i].AMST_Id,
                                                        feename = feesss[j],
                                                        studentname = stud[i].studentname,
                                                        admno = stud[i].admno,
                                                        installmentname = install[k],
                                                        installmentfees = "0"
                                                    });
                                                }

                                            }
                                        }
                                    }
                                }
                                data.FeeDemandRegisterInstallment = result2.ToList();
                                if (data.FeeDemandRegisterInstallment.Count > 0)
                                {
                                    data.count = data.FeeDemandRegisterInstallment.Count;
                                }
                                else
                                {
                                    data.count = 0;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return data;
        }

        class progressEqualityComparer : IEqualityComparer<FeeDemandRegisterInstallmentDTO>
        {
            public bool Equals(FeeDemandRegisterInstallmentDTO b1, FeeDemandRegisterInstallmentDTO b2)
            {
                if (b2 == null && b1 == null)
                    return true;
                else if (b1 == null | b2 == null)
                    return false;
                else if (b1.AMST_Id == b2.AMST_Id)
                    return true;
                else
                    return false;
            }

            public int GetHashCode(FeeDemandRegisterInstallmentDTO bx)
            {
                int hCode = Convert.ToInt32(bx.AMST_Id);
                return hCode.GetHashCode();
            }
        }
    }
}
