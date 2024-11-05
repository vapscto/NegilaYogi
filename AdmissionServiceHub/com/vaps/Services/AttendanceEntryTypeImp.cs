using System.Collections.Generic;
using System.Linq;
using DataAccessMsSqlServerProvider;
using System.Collections.Concurrent;
using PreadmissionDTOs.com.vaps.admission;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model;
using System;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class AttendanceEntryTypeImp : Interfaces.AttendanceEntryTypeInterface
    {
        private static ConcurrentDictionary<string, AttendanceEntryTypeDTO> _login =
      new ConcurrentDictionary<string, AttendanceEntryTypeDTO>();

        private readonly AttendanceEntryTypeContext _AttendanceEntryTypeContext;


        public AttendanceEntryTypeImp(AttendanceEntryTypeContext AttendanceEntryTypeContext)
        {
            _AttendanceEntryTypeContext = AttendanceEntryTypeContext;
        }

        //   async Task<CommonDTO>
        public AttendanceEntryTypeDTO GetAttendanceEntryTypeData(AttendanceEntryTypeDTO AttendanceEntryTypeDTO)//int IVRMM_Id
        {

            List<MasterAcademic> loadallyear = new List<MasterAcademic>();
            loadallyear = _AttendanceEntryTypeContext.year.Where(y => y.MI_Id == AttendanceEntryTypeDTO.MI_Id && y.Is_Active == true).OrderByDescending(y => y.ASMAY_Order).ToList();
            AttendanceEntryTypeDTO.loadallyear = loadallyear.ToArray();

            List<MasterAcademic> allyear = new List<MasterAcademic>();
            allyear = _AttendanceEntryTypeContext.year.Where(y => y.MI_Id == AttendanceEntryTypeDTO.MI_Id && y.Is_Active == true && y.ASMAY_Id == AttendanceEntryTypeDTO.ASMAY_Id).OrderByDescending(y => y.ASMAY_Order).ToList();
            AttendanceEntryTypeDTO.yeardropDown = allyear.ToArray();

            List<School_M_Class> allclass = new List<School_M_Class>();
            allclass = _AttendanceEntryTypeContext.AdmClass.Where(y => y.MI_Id == AttendanceEntryTypeDTO.MI_Id && y.ASMCL_ActiveFlag == true).OrderBy(d => d.ASMCL_Order).ToList();
            AttendanceEntryTypeDTO.classdropDown = allclass.ToArray();

            AttendanceEntryTypeDTO.AttendanceEntryTypeList = (from sp in _AttendanceEntryTypeContext.AttendanceEntryTypeDMO
                                                              from mi in _AttendanceEntryTypeContext.Institute
                                                              from sul in _AttendanceEntryTypeContext.AdmClass
                                                              from y in _AttendanceEntryTypeContext.year
                                                              where (sp.MI_Id == mi.MI_Id && sp.ASMCL_Id == sul.ASMCL_Id && sp.ASMAY_Id == y.ASMAY_Id &&
                                                              sp.MI_Id == AttendanceEntryTypeDTO.MI_Id)
                                                              select new AttendanceEntryTypeDTO
                                                              {
                                                                  InstituteName = mi.MI_Name,
                                                                  ClassName = sul.ASMCL_ClassName,
                                                                  AcedemicYear = y.ASMAY_Year,
                                                                  ASAET_Att_Type = sp.ASAET_Att_Type,
                                                                  ASAET_Id = sp.ASAET_Id
                                                              }).ToArray();

            if (AttendanceEntryTypeDTO.AttendanceEntryTypeList.Length > 0)
            {
                AttendanceEntryTypeDTO.count = AttendanceEntryTypeDTO.AttendanceEntryTypeList.Length;

            }
            else
            {
                AttendanceEntryTypeDTO.count = 0;
            }

            return AttendanceEntryTypeDTO;
        }

        public AttendanceEntryTypeDTO AttendanceEntryTypeData(AttendanceEntryTypeDTO mas)
        {
            if (mas.ASAET_Id > 0)
            {
                var flag_atttype = "";
                long classid;
                var result11 = _AttendanceEntryTypeContext.AttendanceEntryTypeDMO.Single(a => a.ASAET_Id == mas.ASAET_Id);
                classid = result11.ASMCL_Id;
                if (result11.ASAET_Att_Type == "D")
                {
                    flag_atttype = "Dailyonce";
                }
                else if (result11.ASAET_Att_Type == "H")
                {
                    flag_atttype = "Dailytwice";
                }
                else if (result11.ASAET_Att_Type == "M")
                {
                    flag_atttype = "monthly";
                }
                else if (result11.ASAET_Att_Type == "P")
                {
                    flag_atttype = "period";
                }

                if (result11.ASAET_Att_Type == "M")
                {
                    var changing_atttype_after_attgiven = _AttendanceEntryTypeContext.studentattendance.Where(att => att.MI_Id == mas.MI_Id && att.ASMAY_Id == mas.ASMAY_Id &&
               att.ASA_FromDate.Value.Month == DateTime.Now.Month && att.ASA_FromDate.Value.Year == DateTime.Now.Year && att.ASA_Att_Type == flag_atttype && att.ASMCL_Id == classid).ToList();

                    if (changing_atttype_after_attgiven.Count > 0)
                    {
                        mas.typechangeflag = "NotAbleToChangeTheAttEntryType";
                    }
                    else
                    {
                        var result = _AttendanceEntryTypeContext.AttendanceEntryTypeDMO.Single(a => a.ASAET_Id == mas.ASAET_Id);
                        result.MI_Id = result.MI_Id;
                        result.ASMAY_Id = result.ASMAY_Id;
                        result.ASMCL_Id = result.ASMCL_Id;
                        result.ASAET_Att_Type = mas.ASAET_Att_Type;
                        result.CreatedDate = result.CreatedDate;
                        result.UpdatedDate = DateTime.Now;
                        _AttendanceEntryTypeContext.Update(result);
                        int flag = _AttendanceEntryTypeContext.SaveChanges();
                        if (flag > 0)
                        {
                            mas.typechangeflag = "Update";
                            mas.returnval = true;
                        }
                        else
                        {
                            mas.returnval = false;
                            mas.typechangeflag = "";
                        }
                    }
                }
                else
                {
                    if (mas.ASAET_Att_Type == "M")
                    {
                        var changing_atttype_after_attgiven = _AttendanceEntryTypeContext.studentattendance.Where(att => att.MI_Id == mas.MI_Id && att.ASMAY_Id == mas.ASMAY_Id &&
             att.ASA_FromDate.Value.Month == DateTime.Now.Month && att.ASA_FromDate.Value.Year == DateTime.Now.Year && (att.ASA_Att_Type == "Dailytwice" || att.ASA_Att_Type == "Dailyonce") && att.ASMCL_Id == classid).ToList();

                        if (changing_atttype_after_attgiven.Count > 0)
                        {
                            mas.typechangeflag = "NotAbleToChangeTheAttEntryType";
                        }
                        else
                        {
                            var result = _AttendanceEntryTypeContext.AttendanceEntryTypeDMO.Single(a => a.ASAET_Id == mas.ASAET_Id);
                            result.MI_Id = result.MI_Id;
                            result.ASMAY_Id = result.ASMAY_Id;
                            result.ASMCL_Id = result.ASMCL_Id;
                            result.ASAET_Att_Type = mas.ASAET_Att_Type;
                            result.CreatedDate = result.CreatedDate;
                            result.UpdatedDate = DateTime.Now;
                            _AttendanceEntryTypeContext.Update(result);
                            int flag = _AttendanceEntryTypeContext.SaveChanges();
                            if (flag > 0)
                            {
                                mas.typechangeflag = "Update";
                                mas.returnval = true;
                            }
                            else
                            {
                                mas.returnval = false;
                                mas.typechangeflag = "";
                            }
                        }
                    }
                    else
                    {
                        var result = _AttendanceEntryTypeContext.AttendanceEntryTypeDMO.Single(a => a.ASAET_Id == mas.ASAET_Id);
                        result.MI_Id = result.MI_Id;
                        result.ASMAY_Id = result.ASMAY_Id;
                        result.ASMCL_Id = result.ASMCL_Id;
                        result.ASAET_Att_Type = mas.ASAET_Att_Type;
                        result.CreatedDate = result.CreatedDate;
                        result.UpdatedDate = DateTime.Now;
                        _AttendanceEntryTypeContext.Update(result);
                        int flag = _AttendanceEntryTypeContext.SaveChanges();
                        if (flag > 0)
                        {
                            mas.typechangeflag = "Update";
                            mas.returnval = true;
                        }
                        else
                        {
                            mas.returnval = false;
                            mas.typechangeflag = "";
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < mas.SelectedClassDetails.Count; i++)
                {
                    List<AttendanceEntryTypeDMO> Allname1 = new List<AttendanceEntryTypeDMO>();
                    Allname1 = _AttendanceEntryTypeContext.AttendanceEntryTypeDMO.Where(t => t.MI_Id.Equals(mas.MI_Id) && t.ASMAY_Id.Equals(mas.ASMAY_Id) && t.ASMCL_Id.Equals(mas.SelectedClassDetails[i].ASMCL_Id)).ToList().ToList();
                    AttendanceEntryTypeDMO MM2 = new AttendanceEntryTypeDMO();
                    if (Allname1.Count > 0)
                    {
                        mas.message = "Some Duplicate Record Exist";
                    }
                    else
                    {
                        MM2.MI_Id = mas.MI_Id;
                        MM2.ASMAY_Id = mas.ASMAY_Id;
                        MM2.ASMCL_Id = mas.SelectedClassDetails[i].ASMCL_Id;
                        MM2.ASAET_Att_Type = mas.ASAET_Att_Type;

                        MM2.CreatedDate = DateTime.Now;
                        MM2.UpdatedDate = DateTime.Now;
                        _AttendanceEntryTypeContext.Add(MM2);
                        int flag = _AttendanceEntryTypeContext.SaveChanges();
                        if (flag > 0)
                        {
                            mas.returnval = true;
                            mas.typechangeflag = "Add";
                        }
                        else
                        {
                            mas.typechangeflag = "";
                            mas.returnval = false;
                        }
                    }
                }
            }
            return mas;
        }

        public AttendanceEntryTypeDTO GetSelectedRowDetails(AttendanceEntryTypeDTO AttendanceEntryTypeList)
        {
            // AttendanceEntryTypeDTO AttendanceEntryTypeDTO = new AttendanceEntryTypeDTO();
            List<AttendanceEntryTypeDMO> lorg = new List<AttendanceEntryTypeDMO>();
            lorg = _AttendanceEntryTypeContext.AttendanceEntryTypeDMO.Where(t => t.ASAET_Id.Equals(AttendanceEntryTypeList.ASAET_Id)).ToList();
            AttendanceEntryTypeList.AttendanceEntryTypeList = lorg.ToArray();

            //List<AdmissionClass> allclass = new List<AdmissionClass>();
            //allclass = _AttendanceEntryTypeContext.AdmClass.Where(t => t.ASMCL_Id.Equals(lorg[0].AMCL_Id)).ToList();
            //AttendanceEntryTypeDTO.classdropDown = allclass.ToArray();

            return AttendanceEntryTypeList;
        }

        public AttendanceEntryTypeDTO AttendanceDeleteEntryTypeDATA(int ID)
        {
            AttendanceEntryTypeDTO AttendanceEntryTypeDTO = new AttendanceEntryTypeDTO();
            List<AttendanceEntryTypeDMO> masters = new List<AttendanceEntryTypeDMO>();
            masters = _AttendanceEntryTypeContext.AttendanceEntryTypeDMO.Where(t => t.ASAET_Id.Equals(ID)).ToList();



            if (masters.Any())
            {
                _AttendanceEntryTypeContext.Remove(masters.ElementAt(0));
                int flag = _AttendanceEntryTypeContext.SaveChanges();
                if (flag > 0)
                {
                    AttendanceEntryTypeDTO.returnval = true;
                }
                else
                {
                    AttendanceEntryTypeDTO.returnval = false;
                }
            }
            else
            {

            }

            return AttendanceEntryTypeDTO;
        }

    }
}
