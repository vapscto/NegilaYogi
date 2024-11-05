using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs.com.vaps.admission;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using System;
using DomainModel.Model.com.vaps.admission;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class MasterDocumentImp : Interfaces.MasterDocumentInterface
    {
        private static ConcurrentDictionary<string, MasterDocumentDTO> _login =
      new ConcurrentDictionary<string, MasterDocumentDTO>();

        private readonly MasterDocumentContext _MasterDocumentContext;


        public MasterDocumentImp(MasterDocumentContext MasterDocumentContext)
        {
            _MasterDocumentContext = MasterDocumentContext;
        }

        public MasterDocumentDTO Getdetails(MasterDocumentDTO MasterDocumentDTO)//int IVRMM_Id
        {

            List<MasterDocumentDMO> values = new List<MasterDocumentDMO>();
            values = _MasterDocumentContext.MasterDocumentDMO.Where(t => t.MI_Id == MasterDocumentDTO.MI_Id).OrderByDescending(a => a.CreatedDate).ToList();
            MasterDocumentDTO.GridviewDetails = values.ToArray();
            if (MasterDocumentDTO.GridviewDetails.Length > 0)
            {
                MasterDocumentDTO.count = MasterDocumentDTO.GridviewDetails.Length;
            }
            else
            {
                MasterDocumentDTO.count = 0;
            }
            return MasterDocumentDTO;
        }

        public MasterDocumentDTO SaveData(MasterDocumentDTO mas)
        {


            if (mas.AMSMD_Id > 0)
            {
                MasterDocumentDMO MM2 = new MasterDocumentDMO();
                var checkDuplicate = _MasterDocumentContext.MasterDocumentDMO.Where(d => d.AMSMD_DocumentName == mas.AMSMD_DocumentName && d.MI_Id == mas.MI_Id && d.AMSMD_Id != mas.AMSMD_Id).ToList();
                if (checkDuplicate.Count > 0)
                {
                    mas.message = "Master Document Already Exist";
                }
                else
                {
                    var result = _MasterDocumentContext.MasterDocumentDMO.Single(t => t.AMSMD_Id.Equals(mas.AMSMD_Id));

                    result.AMSMD_Id = result.AMSMD_Id;
                    result.MI_Id = result.MI_Id;
                    result.AMSMD_DocumentName = mas.AMSMD_DocumentName;
                    result.AMSMD_Description = mas.AMSMD_Description;
                    result.AMSMD_FLAG = mas.AMSMD_FLAG;

                    result.UpdatedDate = DateTime.Now;

                    _MasterDocumentContext.Update(result);
                    int n = _MasterDocumentContext.SaveChanges();
                    if (n > 0)
                    {
                        mas.returnVal = true;
                        mas.messageupdate = "Update";
                    }
                    else
                    {
                        mas.returnVal = false;
                        mas.messageupdate = "Update";
                    }
                }
            }
            else
            {
                MasterDocumentDMO MM2 = new MasterDocumentDMO();
                var checkDuplicate = _MasterDocumentContext.MasterDocumentDMO.Where(d => d.AMSMD_DocumentName == mas.AMSMD_DocumentName && d.MI_Id == mas.MI_Id).ToList();
                if (checkDuplicate.Count > 0)
                {
                    mas.message = "Master Document Already Exists";
                }
                else
                {
                    MM2.MI_Id = mas.MI_Id;
                    MM2.AMSMD_DocumentName = mas.AMSMD_DocumentName;
                    MM2.AMSMD_Description = mas.AMSMD_Description;
                    MM2.AMSMD_FLAG = mas.AMSMD_FLAG;

                    MM2.CreatedDate = DateTime.Now;
                    MM2.UpdatedDate = DateTime.Now;

                    _MasterDocumentContext.Add(MM2);
                    int n = _MasterDocumentContext.SaveChanges();
                    if (n > 0)
                    {
                        mas.returnVal = true;
                    }
                    else
                    {
                        mas.returnVal = false;
                    }
                }


            }
            List<MasterDocumentDMO> values = new List<MasterDocumentDMO>();
            values = _MasterDocumentContext.MasterDocumentDMO.Where(t => t.MI_Id == mas.MI_Id).OrderByDescending(a => a.CreatedDate).ToList();
            mas.GridviewDetails = values.ToArray();

            return mas;
        }

        public MasterDocumentDTO GetSelectedRowDetails(int ID)
        {
            MasterDocumentDTO MasterDocumentDTO = new MasterDocumentDTO();

            List<MasterDocumentDMO> values = new List<MasterDocumentDMO>();
            values = _MasterDocumentContext.MasterDocumentDMO.Where(t => t.AMSMD_Id.Equals(ID)).ToList().ToList();
            MasterDocumentDTO.SelectedRowDetails = values.ToArray();

            return MasterDocumentDTO;
        }

        public MasterDocumentDTO DeleteEntry(int ID)
        {
            MasterDocumentDTO MasterDocumentDTO = new MasterDocumentDTO();
            List<MasterDocumentDMO> masters1 = new List<MasterDocumentDMO>();
            try
            {
                var Deactvate = _MasterDocumentContext.MasterDocumentDMO.Where(t => t.AMSMD_Id.Equals(ID)).FirstOrDefault();
                if (Deactvate.AMSMD_ActiveFlag == true)
                {
                    Deactvate.AMSMD_ActiveFlag = false;
                }
                else
                {
                    Deactvate.AMSMD_ActiveFlag = true;
                }

                Deactvate.UpdatedDate = DateTime.Now;
                _MasterDocumentContext.Update(Deactvate);
                _MasterDocumentContext.SaveChanges();
                var check_doc_used = _MasterDocumentContext.StudentDocumentDMO.Where(t => t.AMSMD_Id == ID).ToList();
                var check_doc_used1 = _MasterDocumentContext.PreadmissionSchoolRegistrationDocuments.Where(t => t.AMSMD_Id == ID).ToList();
                if (check_doc_used.Count == 0 && check_doc_used1.Count == 0)
                {
                    masters1 = _MasterDocumentContext.MasterDocumentDMO.Where(t => t.AMSMD_Id.Equals(ID)).ToList();

                    if (masters1.Any())
                    {
                        _MasterDocumentContext.Remove(masters1.ElementAt(0));
                        int n = _MasterDocumentContext.SaveChanges();
                        if (n > 0)
                        {
                            MasterDocumentDTO.returnVal = true;
                        }
                        else
                        {
                            MasterDocumentDTO.returnVal = false;
                        }
                    }
                    else
                    {

                    }
                }
                else
                {
                    MasterDocumentDTO.message = "Delete";
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                MasterDocumentDTO.message = "Sorry...You Can Not Delete This Record.Because It Is Mapped With Student.";
            }
            List<MasterDocumentDMO> values = new List<MasterDocumentDMO>();
            values = _MasterDocumentContext.MasterDocumentDMO.Where(t => t.MI_Id == MasterDocumentDTO.MI_Id).OrderByDescending(a => a.CreatedDate).ToList();
            MasterDocumentDTO.GridviewDetails = values.ToArray();
            if (MasterDocumentDTO.GridviewDetails.Length > 0)
            {
                MasterDocumentDTO.count = MasterDocumentDTO.GridviewDetails.Length;
            }
            else
            {
                MasterDocumentDTO.count = 0;
            }
            return MasterDocumentDTO;
        }
    }
}
