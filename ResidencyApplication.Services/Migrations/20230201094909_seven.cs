using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ResidencyApplication.Services.Migrations
{
    public partial class seven : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Actions",
                columns: table => new
                {
                    ActionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actions", x => x.ActionId);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationAttachments_Log",
                columns: table => new
                {
                    AttachmentLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttachmentId = table.Column<int>(type: "int", nullable: false),
                    AttachmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachmentPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachmentTypeId = table.Column<int>(type: "int", nullable: false),
                    ApplicationNumber = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationAttachments_Log", x => x.AttachmentLogId);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationStatus",
                columns: table => new
                {
                    ApplicationStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationStatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationStatus", x => x.ApplicationStatusId);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationTypes",
                columns: table => new
                {
                    ApplicationTypeId = table.Column<int>(type: "int", nullable: false),
                    ApplicationTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationTypes", x => x.ApplicationTypeId);
                });

            migrationBuilder.CreateTable(
                name: "AttachmentDocument",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    ApplicationNumber = table.Column<int>(type: "int", nullable: true),
                    ApprovedLetterForResidencyRenewal = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SalaryCertification = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CivilIdCopy = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PassportCopy = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OtherRelatedDocuments = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentDocument", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AttachmentDocument_log",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttachmentDocumentId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    ApplicationNumber = table.Column<int>(type: "int", nullable: true),
                    ApprovedLetterForResidencyRenewal = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SalaryCertification = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CivilIdCopy = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PassportCopy = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OtherRelatedDocuments = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentDocument_log", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AttachmentTypes",
                columns: table => new
                {
                    AttachmentTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttachmentTypeName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentTypes", x => x.AttachmentTypeId);
                });

            migrationBuilder.CreateTable(
                name: "FormAction",
                columns: table => new
                {
                    ActionId = table.Column<int>(type: "int", nullable: true),
                    FormAction = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    PageId = table.Column<int>(type: "int", nullable: true),
                    AccessType = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Gender",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "GeneralSettings",
                columns: table => new
                {
                    FeatureId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeatureName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PreventTypeDays = table.Column<int>(type: "int", nullable: true),
                    EditReturnActivation = table.Column<bool>(type: "bit", nullable: true),
                    ElectronicPaymentActivation = table.Column<bool>(type: "bit", nullable: true),
                    Value = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralSettings", x => x.FeatureId);
                });

            migrationBuilder.CreateTable(
                name: "JobTypes",
                columns: table => new
                {
                    idSergate = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id = table.Column<int>(type: "int", nullable: false),
                    sapid = table.Column<int>(type: "int", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobTypes", x => x.idSergate);
                });

            migrationBuilder.CreateTable(
                name: "Nationalities",
                columns: table => new
                {
                    NationalityId = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    NationalityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "NonSapUsers_log",
                columns: table => new
                {
                    UserLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CivilId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Organization = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    RegistrationStatusId = table.Column<int>(type: "int", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NonSapUsers_log", x => x.UserLogId);
                });

            migrationBuilder.CreateTable(
                name: "NotificationSetting",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AcceptEmailNotification = table.Column<bool>(type: "bit", nullable: true),
                    RejectEmailNotification = table.Column<bool>(type: "bit", nullable: true),
                    ReturnEmailNotification = table.Column<bool>(type: "bit", nullable: true),
                    RegistEmailNotification = table.Column<bool>(type: "bit", nullable: true),
                    PaymentEmailNotification = table.Column<bool>(type: "bit", nullable: true),
                    AcceptSmsNotification = table.Column<bool>(type: "bit", nullable: true),
                    RejectSmsNotification = table.Column<bool>(type: "bit", nullable: true),
                    ReturnSmsNotification = table.Column<bool>(type: "bit", nullable: true),
                    RegistSmsNotification = table.Column<bool>(type: "bit", nullable: true),
                    PaymentSmsNotification = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationSetting", x => x.NotificationId);
                });

            migrationBuilder.CreateTable(
                name: "Organization",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sapId = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    parentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organization", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PassportInformation",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CivilID = table.Column<long>(type: "bigint", nullable: true),
                    NationalityId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PassportNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IssueCountry = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IssueDate = table.Column<DateTime>(type: "date", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "date", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResidencyExpiryDate = table.Column<DateTime>(type: "date", nullable: true),
                    ApplicationNumber = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassportInformation", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PassportInformation_log",
                columns: table => new
                {
                    PassportInformationLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id = table.Column<int>(type: "int", nullable: false),
                    CivilID = table.Column<long>(type: "bigint", nullable: true),
                    NationalityId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PassportNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IssueCountry = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IssueDate = table.Column<DateTime>(type: "date", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "date", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResidencyExpiryDate = table.Column<DateTime>(type: "date", nullable: true),
                    ApplicationNumber = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassportInformation_log", x => x.PassportInformationLogId);
                });

            migrationBuilder.CreateTable(
                name: "PersonalInformation",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeNumber = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeNameArabic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EmployeeNameEnglish = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: true),
                    MobileNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Department = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HireDate = table.Column<DateTime>(type: "date", nullable: true),
                    ApplicationNumber = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    SectionId = table.Column<int>(type: "int", nullable: true),
                    UserTypeId = table.Column<int>(type: "int", nullable: true),
                    JobtypeId = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalInformation", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "refreshTokens",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    expires = table.Column<DateTime>(type: "datetime", nullable: true),
                    created = table.Column<DateTime>(type: "datetime", nullable: true),
                    createdByIp = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    revoked = table.Column<DateTime>(type: "datetime", nullable: true),
                    revokedByIp = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    replacedByToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    userId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refreshTokens", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationStatus",
                columns: table => new
                {
                    RegistrationStatusId = table.Column<int>(type: "int", nullable: false),
                    RegistrationStatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RegistrationStatusNameAr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationStatus", x => x.RegistrationStatusId);
                });

            migrationBuilder.CreateTable(
                name: "sap",
                columns: table => new
                {
                    birthdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    cardholder = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    civilid = table.Column<double>(type: "float", nullable: false),
                    department = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    departmentid = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    domainusername = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    dutytime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    dutytimeid = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    employeename = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    employeestatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    employeestatusid = table.Column<int>(type: "int", nullable: false),
                    employeetype = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    employeetypeid = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    filenumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    financialgrade = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    financialgradearea = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    financialgradetype = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fingerprintid = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    gender = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    genderid = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    hireddate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    islinesupervisorof = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ismanager = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    jobtitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    jobtitleid = table.Column<int>(type: "int", nullable: false),
                    nationality = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    nationalityid = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    organization = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    organizationid = table.Column<int>(type: "int", nullable: false),
                    organizationunitid = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    organizationunitlevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    personelno = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    section = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    sectionid = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    sector = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    sectorid = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    subdepartment = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    subdepartmentid = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    subsection = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    subsectionid = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    workcenter = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    workcenterid = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    workschedulerule = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    workscheduletime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "SapUsers_log",
                columns: table => new
                {
                    SabUsersLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CivilId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmployeeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmployeeType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Sector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Section = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: true),
                    HireDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SapUsers_log", x => x.SabUsersLogId);
                });

            migrationBuilder.CreateTable(
                name: "UserApplications_log",
                columns: table => new
                {
                    ApplicationNumberLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationNumber = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ApplicationStatusId = table.Column<int>(type: "int", nullable: false),
                    ApplicationTypeId = table.Column<int>(type: "int", nullable: false),
                    ApplicationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StepNo = table.Column<int>(type: "int", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserApplications_log", x => x.ApplicationNumberLogId);
                });

            migrationBuilder.CreateTable(
                name: "UserApplicationSteps",
                columns: table => new
                {
                    StepNo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StepName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserApplicationSteps", x => x.StepNo);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserRoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserRoleNameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserRoleNameAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.UserRoleId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CivilIdSerialNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MobileNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResidencyByMoa = table.Column<bool>(type: "bit", nullable: false),
                    IsSapUser = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    NationalityId = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    SectionId = table.Column<int>(type: "int", nullable: true),
                    UserTypeId = table.Column<int>(type: "int", nullable: true),
                    JobtypeId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    EmployeeNumber = table.Column<int>(type: "int", nullable: true),
                    UserRoleId = table.Column<int>(type: "int", nullable: true),
                    GenderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Users_log",
                columns: table => new
                {
                    UserLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CivilIdSerialNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MobileNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResidencyByMoa = table.Column<bool>(type: "bit", nullable: false),
                    NationalityId = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    IsSapUser = table.Column<bool>(type: "bit", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users_log", x => x.UserLogId);
                });

            migrationBuilder.CreateTable(
                name: "UserTypes",
                columns: table => new
                {
                    UserTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SapId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserTypeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTypes", x => x.UserTypeId);
                });

            migrationBuilder.CreateTable(
                name: "SapUsers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CivilId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmployeeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmployeeType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Sector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Section = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: true),
                    HireDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    organization = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SapUsers", x => x.id);
                    table.ForeignKey(
                        name: "FK_SapUsers_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserApplications",
                columns: table => new
                {
                    ApplicationNumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    ApplicationStatusId = table.Column<int>(type: "int", nullable: true),
                    ApplicationTypeId = table.Column<int>(type: "int", nullable: true),
                    ApplicationDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StepNo = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SubmittedBy = table.Column<int>(type: "int", nullable: true),
                    AssignedTo = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserApplications", x => x.ApplicationNumber);
                    table.ForeignKey(
                        name: "FK_UserApplications_ApplicationStatus",
                        column: x => x.ApplicationStatusId,
                        principalTable: "ApplicationStatus",
                        principalColumn: "ApplicationStatusId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserApplications_ApplicationTypes",
                        column: x => x.ApplicationTypeId,
                        principalTable: "ApplicationTypes",
                        principalColumn: "ApplicationTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserApplications_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NonSapUsers",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CivilId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Organization = table.Column<int>(type: "int", nullable: true),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    RegistrationStatusId = table.Column<int>(type: "int", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NonSapUsers", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_NonSapUsers_RegistrationStatus",
                        column: x => x.RegistrationStatusId,
                        principalTable: "RegistrationStatus",
                        principalColumn: "RegistrationStatusId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NonSapUsers_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NonSapUsers_UserTypes",
                        column: x => x.UserTypeId,
                        principalTable: "UserTypes",
                        principalColumn: "UserTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationAttachments",
                columns: table => new
                {
                    AttachmentId = table.Column<int>(type: "int", nullable: false),
                    AttachmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachmentPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachmentTypeId = table.Column<int>(type: "int", nullable: false),
                    ApplicationNumber = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationAttachments", x => x.AttachmentId);
                    table.ForeignKey(
                        name: "FK_ApplicationAttachments_AttachmentTypes",
                        column: x => x.AttachmentTypeId,
                        principalTable: "AttachmentTypes",
                        principalColumn: "AttachmentTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicationAttachments_UserApplications",
                        column: x => x.ApplicationNumber,
                        principalTable: "UserApplications",
                        principalColumn: "ApplicationNumber",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationAttachments_ApplicationNumber",
                table: "ApplicationAttachments",
                column: "ApplicationNumber");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationAttachments_AttachmentTypeId",
                table: "ApplicationAttachments",
                column: "AttachmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NonSapUsers_RegistrationStatusId",
                table: "NonSapUsers",
                column: "RegistrationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_NonSapUsers_UserTypeId",
                table: "NonSapUsers",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SapUsers_UserId",
                table: "SapUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserApplications_ApplicationStatusId",
                table: "UserApplications",
                column: "ApplicationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_UserApplications_ApplicationTypeId",
                table: "UserApplications",
                column: "ApplicationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserApplications_UserId",
                table: "UserApplications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTypes",
                table: "UserTypes",
                column: "UserTypeName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Actions");

            migrationBuilder.DropTable(
                name: "ApplicationAttachments");

            migrationBuilder.DropTable(
                name: "ApplicationAttachments_Log");

            migrationBuilder.DropTable(
                name: "AttachmentDocument");

            migrationBuilder.DropTable(
                name: "AttachmentDocument_log");

            migrationBuilder.DropTable(
                name: "FormAction");

            migrationBuilder.DropTable(
                name: "Gender");

            migrationBuilder.DropTable(
                name: "GeneralSettings");

            migrationBuilder.DropTable(
                name: "JobTypes");

            migrationBuilder.DropTable(
                name: "Nationalities");

            migrationBuilder.DropTable(
                name: "NonSapUsers");

            migrationBuilder.DropTable(
                name: "NonSapUsers_log");

            migrationBuilder.DropTable(
                name: "NotificationSetting");

            migrationBuilder.DropTable(
                name: "Organization");

            migrationBuilder.DropTable(
                name: "PassportInformation");

            migrationBuilder.DropTable(
                name: "PassportInformation_log");

            migrationBuilder.DropTable(
                name: "PersonalInformation");

            migrationBuilder.DropTable(
                name: "refreshTokens");

            migrationBuilder.DropTable(
                name: "sap");

            migrationBuilder.DropTable(
                name: "SapUsers");

            migrationBuilder.DropTable(
                name: "SapUsers_log");

            migrationBuilder.DropTable(
                name: "UserApplications_log");

            migrationBuilder.DropTable(
                name: "UserApplicationSteps");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Users_log");

            migrationBuilder.DropTable(
                name: "AttachmentTypes");

            migrationBuilder.DropTable(
                name: "UserApplications");

            migrationBuilder.DropTable(
                name: "RegistrationStatus");

            migrationBuilder.DropTable(
                name: "UserTypes");

            migrationBuilder.DropTable(
                name: "ApplicationStatus");

            migrationBuilder.DropTable(
                name: "ApplicationTypes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
