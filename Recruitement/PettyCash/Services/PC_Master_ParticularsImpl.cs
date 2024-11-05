using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.IssueManager.PettyCash;
using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.PettyCash.Services
{
    public class PC_Master_ParticularsImpl : Interface.PC_Master_ParticularsInterface
    {
        public PettyCashContext _context;
        public PC_Master_ParticularsImpl(PettyCashContext _con)
        {
            _context = _con;
        }
        public PC_Master_ParticularsDTO onloaddata(PC_Master_ParticularsDTO data)
        {
            try
            {
                data.getloaddetails = _context.PC_Master_ParticularsDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_Master_ParticularsDTO saverecord(PC_Master_ParticularsDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.PCMPART_Id > 0)
                {
                    var checkduplicate = _context.PC_Master_ParticularsDMO.Where(a => a.MI_Id == data.MI_Id
                     && a.PCMPART_ParticularName.Equals(data.PCMPART_ParticularName) && a.PCMPART_Id != data.PCMPART_Id).ToList();

                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        var checkresult = _context.PC_Master_ParticularsDMO.Single(a => a.MI_Id == data.MI_Id && a.PCMPART_Id == data.PCMPART_Id);
                        checkresult.PCMPART_ParticularDesc = data.PCMPART_ParticularDesc;
                        checkresult.PCMPART_ParticularName = data.PCMPART_ParticularName;
                        checkresult.PCMPART_UpdatedBy = data.Userid;
                        checkresult.PCMPART_UpdatedDate = indiantime0;
                        _context.Update(checkresult);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                            data.message = "Update";
                        }
                        else
                        {
                            data.returnval = false;
                            data.message = "Update";
                        }
                    }
                }
                else
                {
                    PC_Master_ParticularsDMO pC_Master_ParticularsDMO = new PC_Master_ParticularsDMO();

                    var checkduplicate = _context.PC_Master_ParticularsDMO.Where(a => a.MI_Id == data.MI_Id
                    && a.PCMPART_ParticularName.Equals(data.PCMPART_ParticularName)).ToList();

                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        pC_Master_ParticularsDMO.MI_Id = data.MI_Id;
                        pC_Master_ParticularsDMO.PCMPART_ParticularName = data.PCMPART_ParticularName;
                        pC_Master_ParticularsDMO.PCMPART_ParticularDesc = data.PCMPART_ParticularDesc;
                        pC_Master_ParticularsDMO.PCMPART_ActiveFlg = true;
                        pC_Master_ParticularsDMO.PCMPART_CreatedBy = data.Userid;
                        pC_Master_ParticularsDMO.PCMPART_UpdatedBy = data.Userid;
                        pC_Master_ParticularsDMO.PCMPART_CreatedDate = indiantime0;
                        pC_Master_ParticularsDMO.PCMPART_UpdatedDate = indiantime0;
                        _context.Add(pC_Master_ParticularsDMO);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.message = "Add";
                            data.returnval = true;
                        }
                        else
                        {
                            data.message = "Add";
                            data.returnval = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                data.returnval = false;
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_Master_ParticularsDTO deactiveY(PC_Master_ParticularsDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var checkactivestatus = _context.PC_Master_ParticularsDMO.Single(a => a.MI_Id == data.MI_Id && a.PCMPART_Id == data.PCMPART_Id);

                if (checkactivestatus.PCMPART_ActiveFlg == true)
                {
                    var checkPC_Requisition_DetailsDMO = _context.PC_Requisition_DetailsDMO.Where(a => a.PCMPART_Id == data.PCMPART_Id 
                    && a.PCREQTNDET_ActiveFlg == true).Count();

                    var checkPC_Indent_DetailsDMO = _context.PC_Indent_DetailsDMO.Where(a => a.PCMPART_Id == data.PCMPART_Id
                    && a.PCINDENTDET_ActiveFlg == true).Count();

                    var checkPC_Indent_Approved_DetailsDMO = _context.PC_Indent_Approved_DetailsDMO.Where(a => a.PCMPART_Id == data.PCMPART_Id
                    && a.PCINDENTAPDT_ActiveFlg == true).Count();

                    if(checkPC_Requisition_DetailsDMO==0 && checkPC_Indent_DetailsDMO==0 && checkPC_Indent_Approved_DetailsDMO == 0)
                    {
                        var resultdeactive = _context.PC_Master_ParticularsDMO.Single(a => a.MI_Id == data.MI_Id && a.PCMPART_Id == data.PCMPART_Id);

                        if (resultdeactive.PCMPART_ActiveFlg == true)
                        {
                            resultdeactive.PCMPART_ActiveFlg = false;
                        }
                        else
                        {
                            resultdeactive.PCMPART_ActiveFlg = true;
                        }

                        resultdeactive.PCMPART_UpdatedDate = indiantime0;
                        resultdeactive.PCMPART_UpdatedBy = data.Userid;
                        _context.Update(resultdeactive);

                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                    else
                    {
                        data.message = "Mapped";
                    }
                }
                else
                {
                    var resultdeactive = _context.PC_Master_ParticularsDMO.Single(a => a.MI_Id == data.MI_Id && a.PCMPART_Id == data.PCMPART_Id);

                    if (resultdeactive.PCMPART_ActiveFlg == true)
                    {
                        resultdeactive.PCMPART_ActiveFlg = false;
                    }
                    else
                    {
                        resultdeactive.PCMPART_ActiveFlg = true;
                    }

                    resultdeactive.PCMPART_UpdatedDate = indiantime0;
                    resultdeactive.PCMPART_UpdatedBy = data.Userid;
                    _context.Update(resultdeactive);

                    var i = _context.SaveChanges();
                    if (i > 0)
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
            return data;
        }
    }
}
