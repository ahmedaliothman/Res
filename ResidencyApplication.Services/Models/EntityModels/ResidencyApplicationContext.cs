using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ResidencyApplication.Services.Models.EntityModels
{
    public partial class ResidencyApplicationContext : DbContext
    {
        public ResidencyApplicationContext()
        {
        }

        public ResidencyApplicationContext(DbContextOptions<ResidencyApplicationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Action> Actions { get; set; }
        public virtual DbSet<ApplicationAttachment> ApplicationAttachments { get; set; }
        public virtual DbSet<ApplicationAttachmentsLog> ApplicationAttachmentsLogs { get; set; }
        public virtual DbSet<ApplicationStatus> ApplicationStatuses { get; set; }
        public virtual DbSet<ApplicationType> ApplicationTypes { get; set; }
        public virtual DbSet<AttachmentDocument> AttachmentDocuments { get; set; }
        public virtual DbSet<AttachmentDocumentLog> AttachmentDocumentLogs { get; set; }
        public virtual DbSet<AttachmentType> AttachmentTypes { get; set; }
        public virtual DbSet<EmployeeView> EmployeeViews { get; set; }
        public virtual DbSet<FormAction> FormActions { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<GeneralSetting> GeneralSettings { get; set; }
        public virtual DbSet<JobType> JobTypes { get; set; }
        public virtual DbSet<Nationality> Nationalities { get; set; }
        public virtual DbSet<NonSapUser> NonSapUsers { get; set; }
        public virtual DbSet<NonSapUsersLog> NonSapUsersLogs { get; set; }
        public virtual DbSet<NotificationSetting> NotificationSettings { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<PassportInformation> PassportInformations { get; set; }
        public virtual DbSet<PassportInformationLog> PassportInformationLogs { get; set; }
        public virtual DbSet<PersonalInformation> PersonalInformations { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<RegistrationStatus> RegistrationStatuses { get; set; }
        public virtual DbSet<Sap> Saps { get; set; }
        public virtual DbSet<SapUser> SapUsers { get; set; }
        public virtual DbSet<SapUsersLog> SapUsersLogs { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserApplication> UserApplications { get; set; }
        public virtual DbSet<UserApplicationStep> UserApplicationSteps { get; set; }
        public virtual DbSet<UserApplicationsLog> UserApplicationsLogs { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<UserType> UserTypes { get; set; }
        public virtual DbSet<UsersLog> UsersLogs { get; set; }
        public virtual DbSet<VCountAssignedApplication> VCountAssignedApplications { get; set; }
        public virtual DbSet<VUserApplication> VUserApplications { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=DevConnectionString");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Arabic_CI_AI");

            modelBuilder.Entity<Action>(entity =>
            {
                entity.Property(e => e.ActionName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ApplicationAttachment>(entity =>
            {
                entity.HasKey(e => e.AttachmentId);

                entity.Property(e => e.AttachmentId).ValueGeneratedNever();

                entity.Property(e => e.AttachmentName).IsRequired();

                entity.Property(e => e.AttachmentPath).IsRequired();

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.UpdatedDate).HasColumnType("date");

                entity.HasOne(d => d.ApplicationNumberNavigation)
                    .WithMany(p => p.ApplicationAttachments)
                    .HasForeignKey(d => d.ApplicationNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApplicationAttachments_UserApplications");

                entity.HasOne(d => d.AttachmentType)
                    .WithMany(p => p.ApplicationAttachments)
                    .HasForeignKey(d => d.AttachmentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApplicationAttachments_AttachmentTypes");
            });

            modelBuilder.Entity<ApplicationAttachmentsLog>(entity =>
            {
                entity.HasKey(e => e.AttachmentLogId);

                entity.ToTable("ApplicationAttachments_Log");

                entity.Property(e => e.Action).HasMaxLength(50);

                entity.Property(e => e.AttachmentName).IsRequired();

                entity.Property(e => e.AttachmentPath).IsRequired();

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.UpdatedDate).HasColumnType("date");
            });

            modelBuilder.Entity<ApplicationStatus>(entity =>
            {
                entity.ToTable("ApplicationStatus");

                entity.Property(e => e.ApplicationStatusName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.UpdatedDate).HasColumnType("date");
            });

            modelBuilder.Entity<ApplicationType>(entity =>
            {
                entity.Property(e => e.ApplicationTypeId).ValueGeneratedNever();

                entity.Property(e => e.ApplicationTypeName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.UpdatedDate).HasColumnType("date");
            });

            modelBuilder.Entity<AttachmentDocument>(entity =>
            {
                entity.ToTable("AttachmentDocument");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ApprovedLetterForResidencyRenewal).HasMaxLength(500);

                entity.Property(e => e.CivilIdCopy).HasMaxLength(500);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.OtherRelatedDocuments).HasMaxLength(500);

                entity.Property(e => e.PassportCopy).HasMaxLength(500);

                entity.Property(e => e.SalaryCertification).HasMaxLength(500);

                entity.Property(e => e.UpdatedDate).HasColumnType("date");
            });

            modelBuilder.Entity<AttachmentDocumentLog>(entity =>
            {
                entity.ToTable("AttachmentDocument_log");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Action).HasMaxLength(50);

                entity.Property(e => e.ApprovedLetterForResidencyRenewal).HasMaxLength(500);

                entity.Property(e => e.CivilIdCopy).HasMaxLength(500);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.OtherRelatedDocuments).HasMaxLength(500);

                entity.Property(e => e.PassportCopy).HasMaxLength(500);

                entity.Property(e => e.SalaryCertification).HasMaxLength(500);

                entity.Property(e => e.UpdatedDate).HasColumnType("date");
            });

            modelBuilder.Entity<AttachmentType>(entity =>
            {
                entity.Property(e => e.AttachmentTypeName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.UpdatedDate).HasColumnType("date");
            });

            modelBuilder.Entity<EmployeeView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Employee_View");

                entity.Property(e => e.CivilId).HasMaxLength(50);

                entity.Property(e => e.CivilIdSerialNumber).HasMaxLength(50);

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.MobileNumber).HasMaxLength(50);

                entity.Property(e => e.NationalityId)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.StatusMessage)
                    .IsRequired()
                    .HasMaxLength(47)
                    .IsUnicode(false)
                    .HasColumnName("statusMessage");

                entity.Property(e => e.UserName).IsRequired();

                entity.Property(e => e.Valid)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("valid");
            });

            modelBuilder.Entity<FormAction>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("FormAction");

                entity.Property(e => e.AccessType)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.FormAction1)
                    .HasMaxLength(10)
                    .HasColumnName("FormAction")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Gender");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<GeneralSetting>(entity =>
            {
                entity.HasKey(e => e.FeatureId);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.FeatureName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedDate).HasColumnType("date");
            });

            modelBuilder.Entity<JobType>(entity =>
            {
                entity.HasKey(e => e.IdSergate);

                entity.Property(e => e.IdSergate).HasColumnName("idSergate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Sapid).HasColumnName("sapid");
            });

            modelBuilder.Entity<Nationality>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.NationalityId)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.NationalityName).IsRequired();

                entity.Property(e => e.UpdatedDate).HasColumnType("date");
            });

            modelBuilder.Entity<NonSapUser>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.CivilId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.EmployeeName).IsRequired();

                entity.Property(e => e.EmployeeNumber).HasMaxLength(50);

                entity.Property(e => e.RegistrationDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedDate).HasColumnType("date");

                entity.HasOne(d => d.RegistrationStatus)
                    .WithMany(p => p.NonSapUsers)
                    .HasForeignKey(d => d.RegistrationStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NonSapUsers_RegistrationStatus");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.NonSapUser)
                    .HasForeignKey<NonSapUser>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NonSapUsers_Users");

                entity.HasOne(d => d.UserType)
                    .WithMany(p => p.NonSapUsers)
                    .HasForeignKey(d => d.UserTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NonSapUsers_UserTypes");
            });

            modelBuilder.Entity<NonSapUsersLog>(entity =>
            {
                entity.HasKey(e => e.UserLogId);

                entity.ToTable("NonSapUsers_log");

                entity.Property(e => e.CivilId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.EmployeeName).IsRequired();

                entity.Property(e => e.EmployeeNumber).HasMaxLength(50);

                entity.Property(e => e.Organization).HasMaxLength(100);

                entity.Property(e => e.RegistrationDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedDate).HasColumnType("date");
            });

            modelBuilder.Entity<NotificationSetting>(entity =>
            {
                entity.HasKey(e => e.NotificationId);

                entity.ToTable("NotificationSetting");

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.UpdatedDate).HasColumnType("date");
            });

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.ToTable("Organization");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.ParentId).HasColumnName("parentId");

                entity.Property(e => e.SapId).HasColumnName("sapId");
            });

            modelBuilder.Entity<PassportInformation>(entity =>
            {
                entity.ToTable("PassportInformation");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CivilId).HasColumnName("CivilID");

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.ExpiryDate).HasColumnType("date");

                entity.Property(e => e.IssueCountry).HasMaxLength(200);

                entity.Property(e => e.IssueDate).HasColumnType("date");

                entity.Property(e => e.NationalityId).HasMaxLength(50);

                entity.Property(e => e.PassportNumber).HasMaxLength(50);

                entity.Property(e => e.ResidencyExpiryDate).HasColumnType("date");

                entity.Property(e => e.UpdatedDate).HasColumnType("date");
            });

            modelBuilder.Entity<PassportInformationLog>(entity =>
            {
                entity.HasKey(e => e.PassportInformationLogId);

                entity.ToTable("PassportInformation_log");

                entity.Property(e => e.Action).HasMaxLength(50);

                entity.Property(e => e.CivilId).HasColumnName("CivilID");

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.ExpiryDate).HasColumnType("date");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IssueCountry).HasMaxLength(200);

                entity.Property(e => e.IssueDate).HasColumnType("date");

                entity.Property(e => e.NationalityId).HasMaxLength(50);

                entity.Property(e => e.PassportNumber).HasMaxLength(50);

                entity.Property(e => e.ResidencyExpiryDate).HasColumnType("date");

                entity.Property(e => e.UpdatedDate).HasColumnType("date");
            });

            modelBuilder.Entity<PersonalInformation>(entity =>
            {
                entity.ToTable("PersonalInformation");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.Department).HasMaxLength(50);

                entity.Property(e => e.EmployeeNameArabic).HasMaxLength(200);

                entity.Property(e => e.EmployeeNameEnglish).HasMaxLength(200);

                entity.Property(e => e.HireDate).HasColumnType("date");

                entity.Property(e => e.JobTitle).HasMaxLength(50);

                entity.Property(e => e.MobileNumber).HasMaxLength(50);

                entity.Property(e => e.UpdatedDate).HasColumnType("date");
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("refreshTokens");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasColumnName("created");

                entity.Property(e => e.CreatedByIp)
                    .HasMaxLength(500)
                    .HasColumnName("createdByIp");

                entity.Property(e => e.Expires)
                    .HasColumnType("datetime")
                    .HasColumnName("expires");

                entity.Property(e => e.ReplacedByToken).HasColumnName("replacedByToken");

                entity.Property(e => e.Revoked)
                    .HasColumnType("datetime")
                    .HasColumnName("revoked");

                entity.Property(e => e.RevokedByIp)
                    .HasMaxLength(500)
                    .HasColumnName("revokedByIp");

                entity.Property(e => e.Token).HasColumnName("token");

                entity.Property(e => e.UserId).HasColumnName("userId");
            });

            modelBuilder.Entity<RegistrationStatus>(entity =>
            {
                entity.ToTable("RegistrationStatus");

                entity.Property(e => e.RegistrationStatusId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.RegistrationStatusName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.RegistrationStatusNameAr).HasMaxLength(50);

                entity.Property(e => e.UpdatedDate).HasColumnType("date");
            });

            modelBuilder.Entity<Sap>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("sap");

                entity.Property(e => e.Birthdate).HasColumnName("birthdate");

                entity.Property(e => e.Cardholder)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("cardholder");

                entity.Property(e => e.Civilid).HasColumnName("civilid");

                entity.Property(e => e.Department)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("department");

                entity.Property(e => e.Departmentid)
                    .HasMaxLength(50)
                    .HasColumnName("departmentid");

                entity.Property(e => e.Domainusername)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("domainusername");

                entity.Property(e => e.Dutytime)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("dutytime");

                entity.Property(e => e.Dutytimeid)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("dutytimeid");

                entity.Property(e => e.Employeename)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("employeename");

                entity.Property(e => e.Employeestatus)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("employeestatus");

                entity.Property(e => e.Employeestatusid).HasColumnName("employeestatusid");

                entity.Property(e => e.Employeetype)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("employeetype");

                entity.Property(e => e.Employeetypeid)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("employeetypeid");

                entity.Property(e => e.Filenumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("filenumber");

                entity.Property(e => e.Financialgrade)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("financialgrade");

                entity.Property(e => e.Financialgradearea)
                    .HasMaxLength(50)
                    .HasColumnName("financialgradearea");

                entity.Property(e => e.Financialgradetype)
                    .HasMaxLength(50)
                    .HasColumnName("financialgradetype");

                entity.Property(e => e.Fingerprintid)
                    .HasMaxLength(50)
                    .HasColumnName("fingerprintid");

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("gender");

                entity.Property(e => e.Genderid)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("genderid");

                entity.Property(e => e.Hireddate).HasColumnName("hireddate");

                entity.Property(e => e.Islinesupervisorof)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("islinesupervisorof");

                entity.Property(e => e.Ismanager)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("ismanager");

                entity.Property(e => e.Jobtitle)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("jobtitle");

                entity.Property(e => e.Jobtitleid).HasColumnName("jobtitleid");

                entity.Property(e => e.Nationality)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("nationality");

                entity.Property(e => e.Nationalityid)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("nationalityid");

                entity.Property(e => e.Organization)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("organization");

                entity.Property(e => e.Organizationid).HasColumnName("organizationid");

                entity.Property(e => e.Organizationunitid)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("organizationunitid");

                entity.Property(e => e.Organizationunitlevel)
                    .HasMaxLength(50)
                    .HasColumnName("organizationunitlevel");

                entity.Property(e => e.Personelno)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("personelno");

                entity.Property(e => e.Section)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("section");

                entity.Property(e => e.Sectionid)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("sectionid");

                entity.Property(e => e.Sector)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("sector");

                entity.Property(e => e.Sectorid)
                    .HasMaxLength(50)
                    .HasColumnName("sectorid");

                entity.Property(e => e.Subdepartment)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("subdepartment");

                entity.Property(e => e.Subdepartmentid)
                    .HasMaxLength(50)
                    .HasColumnName("subdepartmentid");

                entity.Property(e => e.Subsection)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("subsection");

                entity.Property(e => e.Subsectionid)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("subsectionid");

                entity.Property(e => e.Workcenter)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("workcenter");

                entity.Property(e => e.Workcenterid)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("workcenterid");

                entity.Property(e => e.Workschedulerule)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("workschedulerule");

                entity.Property(e => e.Workscheduletime)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("workscheduletime");
            });

            modelBuilder.Entity<SapUser>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.CivilId).HasMaxLength(50);

                entity.Property(e => e.EmployeeName).IsRequired();

                entity.Property(e => e.EmployeeNumber).HasMaxLength(50);

                entity.Property(e => e.EmployeeType).HasMaxLength(50);

                entity.Property(e => e.HireDate).HasColumnType("date");

                entity.Property(e => e.JobTitle).HasMaxLength(100);

                entity.Property(e => e.Organization).HasColumnName("organization");

                entity.Property(e => e.Username).HasMaxLength(50);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SapUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SapUsers_Users");
            });

            modelBuilder.Entity<SapUsersLog>(entity =>
            {
                entity.HasKey(e => e.SabUsersLogId);

                entity.ToTable("SapUsers_log");

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.CivilId).HasMaxLength(50);

                entity.Property(e => e.EmployeeName).IsRequired();

                entity.Property(e => e.EmployeeNumber).HasMaxLength(50);

                entity.Property(e => e.EmployeeType).HasMaxLength(50);

                entity.Property(e => e.HireDate).HasColumnType("date");

                entity.Property(e => e.JobTitle).HasMaxLength(100);

                entity.Property(e => e.Username).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.CivilIdSerialNumber).HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.IsAdmin).HasDefaultValueSql("((0))");

                entity.Property(e => e.MobileNumber).HasMaxLength(50);

                entity.Property(e => e.NationalityId)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.UpdatedDate).HasColumnType("date");
            });

            modelBuilder.Entity<UserApplication>(entity =>
            {
                entity.HasKey(e => e.ApplicationNumber);

                entity.Property(e => e.ApplicationDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.UpdatedDate).HasColumnType("date");

                entity.HasOne(d => d.ApplicationStatus)
                    .WithMany(p => p.UserApplications)
                    .HasForeignKey(d => d.ApplicationStatusId)
                    .HasConstraintName("FK_UserApplications_ApplicationStatus");

                entity.HasOne(d => d.ApplicationType)
                    .WithMany(p => p.UserApplications)
                    .HasForeignKey(d => d.ApplicationTypeId)
                    .HasConstraintName("FK_UserApplications_ApplicationTypes");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserApplications)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserApplications_Users");
            });

            modelBuilder.Entity<UserApplicationStep>(entity =>
            {
                entity.HasKey(e => e.StepNo);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.StepName).HasMaxLength(100);

                entity.Property(e => e.UpdatedDate).HasColumnType("date");
            });

            modelBuilder.Entity<UserApplicationsLog>(entity =>
            {
                entity.HasKey(e => e.ApplicationNumberLogId);

                entity.ToTable("UserApplications_log");

                entity.Property(e => e.Action).HasMaxLength(50);

                entity.Property(e => e.ApplicationDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.UpdatedDate).HasColumnType("date");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.UserRoleNameAr).HasMaxLength(100);

                entity.Property(e => e.UserRoleNameEn).HasMaxLength(100);
            });

            modelBuilder.Entity<UserType>(entity =>
            {
                entity.HasIndex(e => e.UserTypeName, "IX_UserTypes")
                    .IsUnique();

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.SapId).HasMaxLength(50);

                entity.Property(e => e.UpdatedDate).HasColumnType("date");

                entity.Property(e => e.UserTypeName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<UsersLog>(entity =>
            {
                entity.HasKey(e => e.UserLogId);

                entity.ToTable("Users_log");

                entity.Property(e => e.Action).HasMaxLength(50);

                entity.Property(e => e.CivilIdSerialNumber).HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.IsAdmin).HasDefaultValueSql("((0))");

                entity.Property(e => e.MobileNumber).HasMaxLength(50);

                entity.Property(e => e.NationalityId)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.UpdatedDate).HasColumnType("date");
            });

            modelBuilder.Entity<VCountAssignedApplication>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_CountAssignedApplications");

                entity.Property(e => e.Employee).IsRequired();
            });

            modelBuilder.Entity<VUserApplication>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_UserApplication");

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.Department).HasMaxLength(50);

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.EmployeeNameArabic).HasMaxLength(200);

                entity.Property(e => e.EmployeeNameEnglish).HasMaxLength(200);

                entity.Property(e => e.JobTitle).HasMaxLength(50);

                entity.Property(e => e.MobileNumber).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
