using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using DomainModel.Model.com.vapstech.Transport;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Services
{
    public class MsterSessionImpl :Interfaces.MsterSessionInterface
    {
        public TransportContext _context;
        public ILogger<MsterSessionDTO> _log;
        
        public MsterSessionImpl(TransportContext context, ILogger<MsterSessionDTO> log)
        {
            _context = context;
            _log = log;
        }
        public MsterSessionDTO getdata (int id)
        {
            MsterSessionDTO data = new MsterSessionDTO();
            try
            {
                data.getloaddata = _context.MsterSessionDMO.Where(a => a.MI_Id == id).OrderByDescending(a=>a.TRMS_Id).ToArray();
            }
            catch(Exception ex)
            {
                _log.LogInformation("Transport Error Master Session getdat" + ex.Message);
            }
            return data;
        }

        public MsterSessionDTO savedata(MsterSessionDTO data)
        {
            try
            {
                if (data.TRMS_Id > 0)
                {
                    var checkduplicate = _context.MsterSessionDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMS_SessionName.Equals(data.TRMS_SessionName) && a.TRMS_Flag.Equals(data.TRMS_Flag) && a.TRMS_Id !=data.TRMS_Id).ToList();
                    if (checkduplicate.Count == 0)
                    {
                        var result = _context.MsterSessionDMO.Single(a => a.MI_Id == data.MI_Id && a.TRMS_Id == data.TRMS_Id);
                        result.TRMS_SessionName = data.TRMS_SessionName;
                        result.TRMS_SessionDesc = data.TRMS_SessionDesc;
                        result.TRMS_Flag = data.TRMS_Flag;
                        _context.Update(result);
                        int n = _context.SaveChanges();
                        if (n > 0)
                        {
                            data.message = "Update";
                            data.returnval = true;
                        }
                        else
                        {
                            data.message = "Update";
                            data.returnval = false;
                        }
                    }
                    else
                    {
                        data.message = "Duplicate";
                    }
                }
                else
                {
                    MasterSessionDMO sess = new MasterSessionDMO();
                    var checkduplicate = _context.MsterSessionDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMS_SessionName.Equals(data.TRMS_SessionName) && a.TRMS_Flag.Equals(data.TRMS_Flag)).ToList();
                    if (checkduplicate.Count == 0)
                    {
                        sess.MI_Id = data.MI_Id;
                        sess.TRMS_SessionName = data.TRMS_SessionName;
                        sess.TRMS_SessionDesc = data.TRMS_SessionDesc;
                        sess.TRMS_Flag = data.TRMS_Flag;
                        sess.TRMS_ActiveFlg = true;
                        sess.CreatedDate = DateTime.Now;
                        sess.UpdatedDate = DateTime.Now;
                        _context.Add(sess);
                        int n = _context.SaveChanges();
                        if (n > 0)
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
                    else
                    {
                        data.message = "Duplicate";
                    }
                }
              
            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Master Session savedata" + ex.Message);
            }
            return data;
        }

        public MsterSessionDTO edit(MsterSessionDTO data)
        {
            try
            {
                data.geteditdata = _context.MsterSessionDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMS_Id==data.TRMS_Id).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Master Session edit" + ex.Message);
            }
            return data;
        }

        public MsterSessionDTO activedeactive(MsterSessionDTO data)
        {
            try
            {
                var checksessionused= (from a in _context.VehicleDriverSessionDMO
                                       from b in _context.MsterSessionDMO
                                       where(a.TRMS_Id==b.TRMS_Id && b.MI_Id==data.MI_Id && b.TRMS_ActiveFlg==true && b.TRMS_Id==data.TRMS_Id)
                                       select new MsterSessionDTO {
                                           TRMS_Id=a.TRMS_Id
                                       }).ToList();

                var checksessionused1 = (from a in _context.VehicleRouteSessionDMO
                                        from b in _context.MsterSessionDMO
                                        where (a.TRMS_Id == b.TRMS_Id && b.MI_Id == data.MI_Id && b.TRMS_ActiveFlg == true && b.TRMS_Id == data.TRMS_Id)
                                        select new MsterSessionDTO
                                        {
                                            TRMS_Id = a.TRMS_Id
                                        }).ToList();

                if(checksessionused.Count==0 && checksessionused1.Count == 0)
                {
                    var result = _context.MsterSessionDMO.Single(a => a.MI_Id == data.MI_Id && a.TRMS_Id == data.TRMS_Id);
                    if (result.TRMS_ActiveFlg == true)
                    {
                        result.TRMS_ActiveFlg = false;
                    }
                    else
                    {
                        result.TRMS_ActiveFlg = true;
                    }
                    result.UpdatedDate = DateTime.Now;
                    _context.Update(result);
                    int n = _context.SaveChanges();
                    if (n > 0)
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
                    data.message = "You can Not Deactivate This Record Its Already Mapped";
                }


            }
            catch (Exception ex)
            {
                data.message = "You can Not Deactivate This Record Its Already Mapped";
                _log.LogInformation("Transport Error Master Session activedeactive" + ex.Message);
            }
            return data;
        }
    }
}
