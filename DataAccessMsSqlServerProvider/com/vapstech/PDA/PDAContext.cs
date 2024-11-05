using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using DomainModel.Model.com.vaps.Fee;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.Fee;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.Transport;
using DomainModel.Model.com.vapstech.PDA;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.Inventory;

namespace DataAccessMsSqlServerProvider.com.vapstech.PDA
{
    public class PDAContext:DbContext
    {
        public PDAContext(DbContextOptions<PDAContext> options) :base(options)
        { Database.SetCommandTimeout(30000); }

        public DbSet<FeeHeadDMO> feehead { get; set; }
        public DbSet<PDA_Master_HeadDMO> pdahead { get; set; }
        public DbSet<PDA_ExpenditureDMO> pdaexpenditure { get; set; }
        public DbSet<PDA_Expenditure_HeadsDMO> pdaexpenditurehead { get; set; }
        public DbSet<Master_Numbering> Master_Numbering { get; set; }
        public DbSet<MasterRoleType> IVRM_Role_Type { get; set; }
        public DbSet<Adm_M_Student> AdmissionStudentDMO { get; set; }
        public DbSet<School_Adm_Y_StudentDMO> School_Adm_Y_StudentDMO { get; set; }
        public DbSet<School_M_Class> School_M_Class { get; set; }
        public DbSet<School_M_Section> school_M_Section { get; set; }
        public DbSet<PDA_StatusDMO> PDA_StatusDMO { get; set; }
        public DbSet<StudentApplication> stuapp { get; set; }
        public DbSet<MasterAcademic> AcademicYear { get; set; }

        public DbSet<FeeStudentTransactionDMO> FeeStudentTransactionDMO { get; set; }

        public DbSet<PDA_RefundDMO> PDA_RefundDMO { get; set; }
        public DbSet<Month> month { get; set; }
        public DbSet<INV_Master_GroupDMO> INV_Master_GroupDMO { get; set; }
        public DbSet<INV_Master_ItemDMO> INV_Master_ItemDMO { get; set; }
        public DbSet<INV_StockDMO> INV_StockDMO { get; set; }


    }
}
