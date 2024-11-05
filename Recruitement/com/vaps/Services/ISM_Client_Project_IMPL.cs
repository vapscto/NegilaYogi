using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.VMS.Sales;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.Sales;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Services
{
    public class ISM_Client_Project_IMPL : Interfaces.ISM_Client_ProjectInterface
    {
        public VMSContext _vmsconte;

        public ISM_Client_Project_IMPL(VMSContext vmscon)
        {
            _vmsconte = vmscon;
        }
        //========================= master components
        public ISM_Client_Project_DTO getdate_cmc(ISM_Client_Project_DTO dto)
        {
            try
            {
                dto.components_list = _vmsconte.ISM_Client_Master_Components_DMO_con.Where(a => a.MI_Id == dto.MI_Id).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }

        public ISM_Client_Project_DTO details_cmc(ISM_Client_Project_DTO dto)
        {
            try
            {
                dto.components_details = _vmsconte.ISM_Client_Master_Components_DMO_con.Where(a => a.MI_Id == dto.MI_Id && a.ISMCLTC_Id == dto.ISMCLTC_Id).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }

        public ISM_Client_Project_DTO SaveEdit_cmc(ISM_Client_Project_DTO dto)
        {
            try
            {
                if (dto.ISMCLTC_Id > 0)
                {
                    var result = _vmsconte.ISM_Client_Master_Components_DMO_con.Single(a => a.ISMCLTC_Id == dto.ISMCLTC_Id && a.MI_Id == dto.MI_Id);


                    result.ISMCLTC_Name = dto.ISMCLTC_Name;
                    result.ISMCLTC_Description = dto.ISMCLTC_Description;
                    result.ISMCLTC_UpdatedBy = dto.User_Id;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "Update";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }



                }
                else
                {
                    ISM_Client_Master_Components_DMO cmc = new ISM_Client_Master_Components_DMO();

                    cmc.MI_Id = dto.MI_Id;
                    cmc.ISMCLTC_Name = dto.ISMCLTC_Name;
                    cmc.ISMCLTC_Description = dto.ISMCLTC_Description;
                    cmc.ISMCLTC_ActiveFlag = true;
                    cmc.ISMCLTC_CreatedBy = dto.User_Id;
                    cmc.CreatedDate = DateTime.Now;
                    _vmsconte.Add(cmc);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "Add";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }

        public ISM_Client_Project_DTO deactivate_cmc(ISM_Client_Project_DTO dto)
        {
            try
            {
                var result = _vmsconte.ISM_Client_Master_Components_DMO_con.Single(a => a.ISMCLTC_Id == dto.ISMCLTC_Id && a.MI_Id == dto.MI_Id);
                if (dto.ISMCLTC_ActiveFlag == true)
                {
                    result.ISMCLTC_ActiveFlag = false;
                    _vmsconte.Update(result);
                    var vv = _vmsconte.SaveChanges();

                    if (vv > 0)
                    {
                        dto.returndata = "false";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }
                }
                else
                {
                    result.ISMCLTC_ActiveFlag = true;
                    _vmsconte.Update(result);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "true";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }


        //======================================BOM

        public ISM_Client_Project_DTO getdata_BOM(ISM_Client_Project_DTO dto)
        {
            try
            {
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_client_project_DD_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
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
                        dto.clientproject_dd = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_client_project_BOM_list_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
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
                        dto.bom_list = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }


                dto.components_dd = _vmsconte.ISM_Client_Master_Components_DMO_con.Where(a => a.MI_Id == dto.MI_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }

        public ISM_Client_Project_DTO details_BOM(ISM_Client_Project_DTO dto)
        {
            try
            {
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_client_project_BOM_Details_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMCLTPRBOM_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMCLTPRBOM_Id
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
                        dto.bom_details = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }


        public ISM_Client_Project_DTO SaveEdit_BOM(ISM_Client_Project_DTO dto)
        {
            try
            {
                if (dto.ISMCLTPRBOM_Id > 0)
                {
                    var result = _vmsconte.ISM_Client_Project_BOM_DMO_con.Single(a => a.ISMCLTPRBOM_Id == dto.ISMCLTPRBOM_Id);


                    result.ISMMCLTPR_Id = dto.ISMMCLTPR_Id;
                    result.ISMCLTC_Id = dto.ISMCLTC_Id;
                    result.ISMCLTPRBOM_Qty = dto.ISMCLTPRBOM_Qty;
                    result.ISMCLTPRBOM_Remarks = dto.ISMCLTPRBOM_Remarks;
                    result.ISMCLTPRBOM_UpdatedBy = dto.User_Id;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "Update";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }



                }
                else
                {
                    ISM_Client_Project_BOM_DMO cmc = new ISM_Client_Project_BOM_DMO();

                    cmc.ISMMCLTPR_Id = dto.ISMMCLTPR_Id;
                    cmc.ISMCLTC_Id = dto.ISMCLTC_Id;
                    cmc.ISMCLTPRBOM_Qty = dto.ISMCLTPRBOM_Qty;
                    cmc.ISMCLTPRBOM_Remarks = dto.ISMCLTPRBOM_Remarks;
                    cmc.ISMCLTPRBOM_ActiveFlag = true;
                    cmc.ISMCLTPRBOM_CreatedBy = dto.User_Id;
                    cmc.CreatedDate = DateTime.Now;
                    _vmsconte.Add(cmc);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "Add";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }
        public ISM_Client_Project_DTO deactivate_BOM(ISM_Client_Project_DTO dto)
        {
            try
            {
                var result = _vmsconte.ISM_Client_Project_BOM_DMO_con.Single(a => a.ISMCLTPRBOM_Id == dto.ISMCLTPRBOM_Id);
                if (dto.ISMCLTPRBOM_ActiveFlag == true)
                {
                    result.ISMCLTPRBOM_ActiveFlag = false;
                    _vmsconte.Update(result);
                    var vv = _vmsconte.SaveChanges();

                    if (vv > 0)
                    {
                        dto.returndata = "false";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }
                }
                else
                {
                    result.ISMCLTPRBOM_ActiveFlag = true;
                    _vmsconte.Update(result);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "true";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }

        //======================================Man power

        public ISM_Client_Project_DTO getdata_MP(ISM_Client_Project_DTO dto)
        {
            try
            {
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_client_project_DD_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
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
                        dto.clientproject_dd = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_client_project_MPlist_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
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
                        dto.mp_list = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }

        public ISM_Client_Project_DTO details_MP(ISM_Client_Project_DTO dto)
        {
            try
            {
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_client_project_MP_Details_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMCLTPRMP_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMCLTPRMP_Id
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
                        dto.mp_details = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }


        public ISM_Client_Project_DTO SaveEdit_MP(ISM_Client_Project_DTO dto)
        {
            try
            {
                if (dto.ISMCLTPRMP_Id > 0)
                {
                    var result = _vmsconte.ISM_Client_Project_ManPower_DMO_con.Single(a => a.ISMCLTPRMP_Id == dto.ISMCLTPRMP_Id);


                    result.ISMMCLTPR_Id = dto.ISMMCLTPR_Id;
                    result.ISMCLTPRMP_ResourceName = dto.ISMCLTPRMP_ResourceName;
                    result.ISMCLTPRMP_Qty = dto.ISMCLTPRMP_Qty;
                    result.ISMCLTPRMP_Remarks = dto.ISMCLTPRMP_Remarks;
                    result.ISMCLTPRMP_UpdatedBy = dto.User_Id;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "Update";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }



                }
                else
                {
                    ISM_Client_Project_ManPower_DMO cmc = new ISM_Client_Project_ManPower_DMO();

                    cmc.ISMMCLTPR_Id = dto.ISMMCLTPR_Id;
                    cmc.ISMCLTPRMP_ResourceName = dto.ISMCLTPRMP_ResourceName;
                    cmc.ISMCLTPRMP_Qty = dto.ISMCLTPRMP_Qty;
                    cmc.ISMCLTPRMP_Remarks = dto.ISMCLTPRMP_Remarks;
                    cmc.ISMCLTPRMP_ActiveFlag = true;
                    cmc.ISMCLTPRMP_CreatedBy = dto.User_Id;
                    cmc.CreatedDate = DateTime.Now;
                    _vmsconte.Add(cmc);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "Add";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }
        public ISM_Client_Project_DTO deactivate_MP(ISM_Client_Project_DTO dto)
        {
            try
            {
                var result = _vmsconte.ISM_Client_Project_ManPower_DMO_con.Single(a => a.ISMCLTPRMP_Id == dto.ISMCLTPRMP_Id);
                if (dto.ISMCLTPRMP_ActiveFlag == true)
                {
                    result.ISMCLTPRMP_ActiveFlag = false;
                    _vmsconte.Update(result);
                    var vv = _vmsconte.SaveChanges();

                    if (vv > 0)
                    {
                        dto.returndata = "false";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }
                }
                else
                {
                    result.ISMCLTPRMP_ActiveFlag = true;
                    _vmsconte.Update(result);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "true";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }


        //======================================MASTER DOC

        public ISM_Client_Project_DTO getdata_MDOC(ISM_Client_Project_DTO dto)
        {
            try
            {
                dto.docmaster_list = _vmsconte.ISM_Client_Project_Master_Docs_DMO_con.Where(a => a.MI_Id == dto.MI_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }
        public ISM_Client_Project_DTO details_MDOC(ISM_Client_Project_DTO dto)
        {
            try
            {
                dto.docmaster_details = _vmsconte.ISM_Client_Project_Master_Docs_DMO_con.Where(a => a.MI_Id == dto.MI_Id && a.ISMCLTPRMDOC_Id == dto.ISMCLTPRMDOC_Id).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }
        public ISM_Client_Project_DTO SaveEdit_MDOC(ISM_Client_Project_DTO dto)
        {
            try
            {
                if (dto.ISMCLTPRMDOC_Id > 0)
                {
                    var result = _vmsconte.ISM_Client_Project_Master_Docs_DMO_con.Single(a => a.ISMCLTPRMDOC_Id == dto.ISMCLTPRMDOC_Id && a.MI_Id == dto.MI_Id);


                    result.ISMCLTPRMDOC_Name = dto.ISMCLTPRMDOC_Name;
                    result.ISMCLTPRMDOC_Description = dto.ISMCLTPRMDOC_Description;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "Update";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }



                }
                else
                {
                    ISM_Client_Project_Master_Docs_DMO cmc = new ISM_Client_Project_Master_Docs_DMO();

                    cmc.ISMCLTPRMDOC_Name = dto.ISMCLTPRMDOC_Name;
                    cmc.MI_Id = dto.MI_Id;
                    cmc.ISMCLTPRMDOC_Description = dto.ISMCLTPRMDOC_Description;
                    cmc.ISMCLTPRMDOC_ActiveFlag = true;
                    cmc.CreatedDate = DateTime.Now;
                    _vmsconte.Add(cmc);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "Add";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }
        public ISM_Client_Project_DTO deactivate_MDOC(ISM_Client_Project_DTO dto)
        {
            try
            {
                var result = _vmsconte.ISM_Client_Project_Master_Docs_DMO_con.Single(a => a.ISMCLTPRMDOC_Id == dto.ISMCLTPRMDOC_Id && a.MI_Id == dto.MI_Id);
                if (dto.ISMCLTPRMDOC_ActiveFlag == true)
                {
                    result.ISMCLTPRMDOC_ActiveFlag = false;
                    _vmsconte.Update(result);
                    var vv = _vmsconte.SaveChanges();

                    if (vv > 0)
                    {
                        dto.returndata = "false";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }
                }
                else
                {
                    result.ISMCLTPRMDOC_ActiveFlag = true;
                    _vmsconte.Update(result);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "true";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }

        //======================================  DOC

        public ISM_Client_Project_DTO getdata_DOC(ISM_Client_Project_DTO dto)
        {
            try
            {
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_client_project_DD_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
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
                        dto.clientproject_dd = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_client_project_DOClist_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
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
                        dto.doc_list = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                dto.document_dd = _vmsconte.ISM_Client_Project_Master_Docs_DMO_con.Where(a => a.MI_Id == dto.MI_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }
        public ISM_Client_Project_DTO details_DOC(ISM_Client_Project_DTO dto)
        {
            try
            {
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_client_project_DOC_Details_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMCLTPRDOC_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMCLTPRDOC_Id
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
                        dto.doc_details = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }
        public ISM_Client_Project_DTO SaveEdit_DOC(ISM_Client_Project_DTO dto)
        {
            try
            {
                if (dto.ISMCLTPRDOC_Id > 0)
                {
                    var result = _vmsconte.ISM_Client_Project_Docs_DMO_con.Single(a => a.ISMCLTPRDOC_Id == dto.ISMCLTPRDOC_Id);


                    result.ISMMCLTPR_Id = dto.ISMMCLTPR_Id;
                    result.ISMCLTPRMDOC_Id = dto.ISMCLTPRMDOC_Id;
                    result.ISMCLTPRDOC_FileName = dto.ISMCLTPRDOC_FileName;
                    result.ISMCLTPRDOC_FilePath = dto.ISMCLTPRDOC_FilePath;
                    result.ISMCLTPRDOC_Date = dto.ISMCLTPRDOC_Date;
                    result.ISMCLTPRDOC_UpdatedBy = dto.User_Id;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "Update";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }



                }
                else
                {
                    ISM_Client_Project_Docs_DMO cmc = new ISM_Client_Project_Docs_DMO();
                    cmc.ISMMCLTPR_Id = dto.ISMMCLTPR_Id;
                    cmc.ISMCLTPRMDOC_Id = dto.ISMCLTPRMDOC_Id;
                    cmc.ISMCLTPRDOC_FileName = dto.ISMCLTPRDOC_FileName;
                    cmc.ISMCLTPRDOC_FilePath = dto.ISMCLTPRDOC_FilePath;
                    cmc.ISMCLTPRDOC_Date = dto.ISMCLTPRDOC_Date;
                    cmc.ISMCLTPRDOC_ActiveFlag = true;
                    cmc.ISMCLTPRDOC_CreatedBy = dto.User_Id;
                    cmc.CreatedDate = DateTime.Now;
                    _vmsconte.Add(cmc);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "Add";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }
        public ISM_Client_Project_DTO deactivate_DOC(ISM_Client_Project_DTO dto)
        {
            try
            {
                var result = _vmsconte.ISM_Client_Project_Docs_DMO_con.Single(a => a.ISMCLTPRDOC_Id == dto.ISMCLTPRDOC_Id);
                if (dto.ISMCLTPRDOC_ActiveFlag == true)
                {
                    result.ISMCLTPRDOC_ActiveFlag = false;
                    _vmsconte.Update(result);
                    var vv = _vmsconte.SaveChanges();

                    if (vv > 0)
                    {
                        dto.returndata = "false";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }
                }
                else
                {
                    result.ISMCLTPRDOC_ActiveFlag = true;
                    _vmsconte.Update(result);
                    var vv = _vmsconte.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "true";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }



    }
}
