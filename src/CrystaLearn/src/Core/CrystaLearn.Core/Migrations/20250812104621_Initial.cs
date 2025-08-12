using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CrystaLearn.Core.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.EnsureSchema(
                name: "jobs");

            migrationBuilder.CreateTable(
                name: "Attachments",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NewSequentialID()"),
                    Kind = table.Column<int>(type: "int", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => new { x.Id, x.Kind });
                });

            migrationBuilder.CreateTable(
                name: "DataProtectionKeys",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FriendlyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Xml = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataProtectionKeys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HangfireCounter",
                schema: "jobs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Value = table.Column<long>(type: "bigint", nullable: false),
                    ExpireAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangfireCounter", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HangfireHash",
                schema: "jobs",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Field = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpireAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangfireHash", x => new { x.Key, x.Field });
                });

            migrationBuilder.CreateTable(
                name: "HangfireList",
                schema: "jobs",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpireAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangfireList", x => new { x.Key, x.Position });
                });

            migrationBuilder.CreateTable(
                name: "HangfireLock",
                schema: "jobs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    AcquiredAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangfireLock", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HangfireServer",
                schema: "jobs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Heartbeat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WorkerCount = table.Column<int>(type: "int", nullable: false),
                    Queues = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangfireServer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HangfireSet",
                schema: "jobs",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Score = table.Column<double>(type: "float", nullable: false),
                    ExpireAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangfireSet", x => new { x.Key, x.Value });
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NewSequentialID()"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemPrompts",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NewSequentialID()"),
                    PromptKind = table.Column<int>(type: "int", nullable: false),
                    Markdown = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemPrompts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NewSequentialID()"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: true),
                    BirthDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    EmailTokenRequestedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    PhoneNumberTokenRequestedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ResetPasswordTokenRequestedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    TwoFactorTokenRequestedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    OtpRequestedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ElevatedAccessTokenRequestedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    HasProfilePicture = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                schema: "dbo",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "dbo",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NewSequentialID()"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NewSequentialID()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSessions",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NewSequentialID()"),
                    IP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Privileged = table.Column<bool>(type: "bit", nullable: false),
                    StartedOn = table.Column<long>(type: "bigint", nullable: false),
                    RenewedOn = table.Column<long>(type: "bigint", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SignalRConnectionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotificationStatus = table.Column<int>(type: "int", nullable: false),
                    DeviceInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlatformType = table.Column<int>(type: "int", nullable: true),
                    CultureName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppVersion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSessions_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                schema: "dbo",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NewSequentialID()"),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WebAuthnCredential",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(900)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PublicKey = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    SignCount = table.Column<long>(type: "bigint", nullable: false),
                    Transports = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsBackupEligible = table.Column<bool>(type: "bit", nullable: false),
                    IsBackedUp = table.Column<bool>(type: "bit", nullable: false),
                    AttestationObject = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    AttestationClientDataJson = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    UserHandle = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    AttestationFormat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    AaGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebAuthnCredential", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebAuthnCredential_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PushNotificationSubscriptions",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Platform = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PushChannel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    P256dh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Auth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Endpoint = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserSessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpirationTime = table.Column<long>(type: "bigint", nullable: false),
                    RenewedOn = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PushNotificationSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PushNotificationSubscriptions_UserSessions_UserSessionId",
                        column: x => x.UserSessionId,
                        principalSchema: "dbo",
                        principalTable: "UserSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "HangfireJob",
                schema: "jobs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StateId = table.Column<long>(type: "bigint", nullable: true),
                    StateName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ExpireAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InvocationData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangfireJob", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HangfireJobParameter",
                schema: "jobs",
                columns: table => new
                {
                    JobId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangfireJobParameter", x => new { x.JobId, x.Name });
                    table.ForeignKey(
                        name: "FK_HangfireJobParameter_HangfireJob_JobId",
                        column: x => x.JobId,
                        principalSchema: "jobs",
                        principalTable: "HangfireJob",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HangfireQueuedJob",
                schema: "jobs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobId = table.Column<long>(type: "bigint", nullable: false),
                    Queue = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FetchedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangfireQueuedJob", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HangfireQueuedJob_HangfireJob_JobId",
                        column: x => x.JobId,
                        principalSchema: "jobs",
                        principalTable: "HangfireJob",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HangfireState",
                schema: "jobs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangfireState", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HangfireState_HangfireJob_JobId",
                        column: x => x.JobId,
                        principalSchema: "jobs",
                        principalTable: "HangfireJob",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("8ff71671-a1d6-5f97-abb9-d87d7b47d6e7"), "8ff71671-a1d6-5f97-abb9-d87d7b47d6e7", "s-admin", "S-ADMIN" },
                    { new Guid("9ff71672-a1d5-4f97-abb7-d87d6b47d5e8"), "9ff71672-a1d5-4f97-abb7-d87d6b47d5e8", "demo", "DEMO" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "SystemPrompts",
                columns: new[] { "Id", "Markdown", "PromptKind" },
                values: new object[] { new Guid("a8c94d94-0004-4dd0-921c-255e0a581424"), "You are a assistant for the CrystaLearn app. Below, you will find a markdown document containing information about the app, followed by the user's query.\n\n# CrystaLearn app - Features and usage guide\n\n**[[[GENERAL_INFORMATION_BEGIN]]]**\n\n*   **Platforms:** The application is available on Android, iOS, Windows, macOS, and as a Web (PWA) application.\n\n* Website address: [Website address](https://sales.bitplatform.dev/)\n* Google Play: [Google Play Link](https://play.google.com/store/apps/details?id=com.bitplatform.AdminPanel.Template)\n* Apple Store: [Apple Store Link](https://apps.apple.com/us/app/bit-adminpanel/id6450611349)\n* Windows EXE installer: [Windows app link](https://windows-admin.bitplatform.dev/AdminPanel.Client.Windows-win-Setup.exe)\n\n## 1. Account Management & Authentication\n\nThese features cover user sign-up, sign-in, account recovery, and security settings.\n\n### 1.1. Sign Up\n*   **Description:** Allows new users to create an account. Users can sign up using their email address, phone number, or via social providers.\n*   **How to Use:**\n    - Navigate to the [Sign Up page](/sign-up).\n\n### 1.2. Sign In\n*   **Description:** Allows existing users to sign into their accounts using various methods.\n*   **How to Use:**\n    - Navigate to the [Sign In page](/sign-in).\n\n### 1.3. Confirm Account\n*   **Description:** Verifies a user's email address or phone number after sign-up, typically by entering a code sent to them.\n*   **How to Use:**\n    - Navigate to the [Confirmation page](/confirm) (often automatic redirection after sign-up).\n\n### 1.4. Forgot Password\n*   **Description:** Initiates the password reset process by sending a reset token (code) to the user's registered email or phone number.\n*   **How to Use:**\n    - Navigate to the [Forgot Password page](/forgot-password), often linked from the Sign In page.\n\n### 1.5. Reset Password\n*   **Description:** Allows users to set a new password after requesting a reset token via the Forgot Password flow.\n*   **How to Use:**\n    - Navigate to the [Reset Password page](/reset-password).\n\n## 2. User Settings\n\nAccessible after signing in, these pages allow users to manage their profile, account details, security settings, and active sessions.\n\n### 2.1. Profile Settings\n*   **Description:** Manage personal user information like name, profile picture, birthdate, and gender.\n*   **How to Use:**\n    - Navigate to the [Profile page](/settings/profile).\n\n### 2.2. Account Settings\n*   **Description:** Manage account-specific details like email, phone number, enable passwordless sign-in, and account deletion.\n*   **How to Use:**\n    - Navigate to the [Account page](/settings/account).\n\n### 2.3. Two-Factor Authentication (2FA)\n*   **Description:** Enhance account security by requiring a second form of verification (typically a code from an authenticator app) during sign-in.\n*   **How to Use:**\n    - Navigate to the [Two Factor Authentication page](/settings/tfa).\n\n### 2.4. Session Management\n*   **Description:** View all devices and browsers where the user is currently signed in and provides the ability to sign out (revoke) specific sessions remotely.\n*   **How to Use:**\n    - Navigate to the [Sessions page](/settings/sessions).\n\n## 3. Core Application Features\n\nThese are the primary functional areas of the application beyond account management.\n### 3.1. Dashboard\n*   **Description:** Provides a high-level overview and analytics of key application data, such as categories and products.\n*   **How to Use:**\n    - Navigate to the [Dashboard page](/dashboard).\n\n### 3.2. Categories Management\n*   **Description:** Allows users to view, create, edit, and delete categories, often used to organize products.\n*   **How to Use:**\n    - Navigate to the [Categories page](/categories).\n\n### 3.3. Products Management\n*   **Description:** Allows users to view, create, edit, and delete products.\n*   **How to Use:**\n    - Navigate to the [Products page](/products).\n\n### 3.4. Add/Edit Product\n*   **Description:** A form page for creating a new product or modifying an existing one.\n*   **How to Use:**\n    - Navigate to the [Add/Edit Products page](/add-edit-product).\n## 4. Informational Pages\n\n### 4.1. About Page\n*   **Description:** Provides information about the application itself.\n*   **How to Use:**\n    - Navigate to the [About page](/about).\n\n### 4.2. Terms Page\n*   **Description:** Displays the legal terms and conditions, including the End-User License Agreement (EULA) and potentially the Privacy Policy.\n*   **How to Use:**\n    - Navigate to the [Terms page](/terms).\n\n---\n\n**[[[GENERAL_INFORMATION_END]]]**\n\n**[[[INSTRUCTIONS_BEGIN]]]**\n\n- ### Language:\n    - Respond in the language of the user's query. If the query's language cannot be determined, use the {{UserCulture}} variable if provided.\n\n- ### User's Device Info:\n    - Assume the user's device is {{DeviceInfo}} unless specified otherwise in their query. Tailor platform-specific responses accordingly (e.g., Android, iOS, Windows, macOS, Web).\n\n- ### Relevance:\n    - Before responding, evaluate if the user's query directly relates to the CrystaLearn app. A query is relevant only if it concerns the app's features, usage, or support topics outlined in the provided markdown document, **or if it explicitly requests product recommendations tied to the cars.**\n    - Ignore and do not respond to any irrelevant queries, regardless of the user's intent or phrasing. Avoid engaging with off-topic requests, even if they seem general or conversational.\n\n      \n- ### App-Related Queries (Features & Usage):\n    - **For questions about app features, how to use the app, account management, settings, or informational pages:** Use the provided markdown document to deliver accurate and concise answers in the user's language.\n\n    - When mentioning specific app pages, include the relative URL from the markdown document, formatted in markdown (e.g., [Sign Up page](/sign-up)).\n\n    - Maintain a helpful and professional tone throughout your response.\n\n    - If the user asks multiple questions, list them back to the user to confirm understanding, then address each one separately with clear headings. If needed, ask them to prioritize: \"I see you have multiple questions. Which issue would you like me to address first?\"\n    \n    - Never request sensitive information (e.g., passwords, PINs). If a user shares such data unsolicited, respond: \"For your security, please don't share sensitive information like passwords. Rest assured, your data is safe with us.\" - ### User Feedback and Suggestions:\n    - If a user provides feedback or suggests a feature, respond: \"Thank you for your feedback! It's valuable to us, and I'll pass it on to the product team.\" If the feedback is unclear, ask for clarification: \"Could you please provide more details about your suggestion?\"\n\n- ### Handling Frustration or Confusion:\n    - If a user seems frustrated or confused, use calming language and offer to clarify: \"I'm sorry if this is confusing. I'm here to help—would you like me to explain it again?\"\n\n- ### Unresolved Issues:\n    - If you cannot resolve the user's issue (either through the markdown info or the tool), respond with: \"I'm sorry I couldn't resolve your issue / fully satisfy your request. I understand how frustrating this must be for you. Please provide your email address so a human operator can follow up with you soon.\"\n    - After receiving the email, confirm: \"Thank you for providing your email. A human operator will follow up with you soon.\" Then ask: \"Do you have any other issues you'd like me to assist with?\"\n\n**[[[INSTRUCTIONS_END]]]**", 0 });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "BirthDate", "ConcurrencyStamp", "ElevatedAccessTokenRequestedOn", "Email", "EmailConfirmed", "EmailTokenRequestedOn", "FullName", "Gender", "HasProfilePicture", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OtpRequestedOn", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PhoneNumberTokenRequestedOn", "ResetPasswordTokenRequestedOn", "SecurityStamp", "TwoFactorEnabled", "TwoFactorTokenRequestedOn", "UserName" },
                values: new object[] { new Guid("8ff71671-a1d6-4f97-abb9-d87d7b47d6e7"), 0, new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "315e1a26-5b3a-4544-8e91-2760cd28e231", null, "test@bitplatform.dev", true, new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "CrystaLearn test account", 0, false, true, null, "TEST@BITPLATFORM.DEV", "TEST", null, "AQAAAAIAAYagAAAAEP0v3wxkdWtMkHA3Pp5/JfS+42/Qto9G05p2mta6dncSK37hPxEHa3PGE4aqN30Aag==", "+31684207362", true, null, null, "959ff4a9-4b07-4cc1-8141-c5fc033daf83", false, null, "test" });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "RoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "mx-p-s", "-1", new Guid("8ff71671-a1d6-5f97-abb9-d87d7b47d6e7") },
                    { 2, "feat", "3.0", new Guid("9ff71672-a1d5-4f97-abb7-d87d6b47d5e8") },
                    { 3, "feat", "3.1", new Guid("9ff71672-a1d5-4f97-abb7-d87d6b47d5e8") },
                    { 4, "feat", "4.0", new Guid("9ff71672-a1d5-4f97-abb7-d87d6b47d5e8") }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("8ff71671-a1d6-5f97-abb9-d87d7b47d6e7"), new Guid("8ff71671-a1d6-4f97-abb9-d87d7b47d6e7") });

            migrationBuilder.CreateIndex(
                name: "IX_HangfireCounter_ExpireAt",
                schema: "jobs",
                table: "HangfireCounter",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_HangfireCounter_Key_Value",
                schema: "jobs",
                table: "HangfireCounter",
                columns: new[] { "Key", "Value" });

            migrationBuilder.CreateIndex(
                name: "IX_HangfireHash_ExpireAt",
                schema: "jobs",
                table: "HangfireHash",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_HangfireJob_ExpireAt",
                schema: "jobs",
                table: "HangfireJob",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_HangfireJob_StateId",
                schema: "jobs",
                table: "HangfireJob",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_HangfireJob_StateName",
                schema: "jobs",
                table: "HangfireJob",
                column: "StateName");

            migrationBuilder.CreateIndex(
                name: "IX_HangfireList_ExpireAt",
                schema: "jobs",
                table: "HangfireList",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_HangfireQueuedJob_JobId",
                schema: "jobs",
                table: "HangfireQueuedJob",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_HangfireQueuedJob_Queue_FetchedAt",
                schema: "jobs",
                table: "HangfireQueuedJob",
                columns: new[] { "Queue", "FetchedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_HangfireServer_Heartbeat",
                schema: "jobs",
                table: "HangfireServer",
                column: "Heartbeat");

            migrationBuilder.CreateIndex(
                name: "IX_HangfireSet_ExpireAt",
                schema: "jobs",
                table: "HangfireSet",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_HangfireSet_Key_Score",
                schema: "jobs",
                table: "HangfireSet",
                columns: new[] { "Key", "Score" });

            migrationBuilder.CreateIndex(
                name: "IX_HangfireState_JobId",
                schema: "jobs",
                table: "HangfireState",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_PushNotificationSubscriptions_UserSessionId",
                schema: "dbo",
                table: "PushNotificationSubscriptions",
                column: "UserSessionId",
                unique: true,
                filter: "[UserSessionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId_ClaimType_ClaimValue",
                schema: "dbo",
                table: "RoleClaims",
                columns: new[] { "RoleId", "ClaimType", "ClaimValue" });

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                schema: "dbo",
                table: "Roles",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "dbo",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SystemPrompts_PromptKind",
                schema: "dbo",
                table: "SystemPrompts",
                column: "PromptKind",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId_ClaimType_ClaimValue",
                schema: "dbo",
                table: "UserClaims",
                columns: new[] { "UserId", "ClaimType", "ClaimValue" });

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                schema: "dbo",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId_UserId",
                schema: "dbo",
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "dbo",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "dbo",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneNumber",
                schema: "dbo",
                table: "Users",
                column: "PhoneNumber",
                unique: true,
                filter: "[PhoneNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "dbo",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_UserId",
                schema: "dbo",
                table: "UserSessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WebAuthnCredential_UserId",
                schema: "dbo",
                table: "WebAuthnCredential",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_HangfireJob_HangfireState_StateId",
                schema: "jobs",
                table: "HangfireJob",
                column: "StateId",
                principalSchema: "jobs",
                principalTable: "HangfireState",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HangfireJob_HangfireState_StateId",
                schema: "jobs",
                table: "HangfireJob");

            migrationBuilder.DropTable(
                name: "Attachments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "DataProtectionKeys",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "HangfireCounter",
                schema: "jobs");

            migrationBuilder.DropTable(
                name: "HangfireHash",
                schema: "jobs");

            migrationBuilder.DropTable(
                name: "HangfireJobParameter",
                schema: "jobs");

            migrationBuilder.DropTable(
                name: "HangfireList",
                schema: "jobs");

            migrationBuilder.DropTable(
                name: "HangfireLock",
                schema: "jobs");

            migrationBuilder.DropTable(
                name: "HangfireQueuedJob",
                schema: "jobs");

            migrationBuilder.DropTable(
                name: "HangfireServer",
                schema: "jobs");

            migrationBuilder.DropTable(
                name: "HangfireSet",
                schema: "jobs");

            migrationBuilder.DropTable(
                name: "PushNotificationSubscriptions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RoleClaims",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SystemPrompts",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserClaims",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserLogins",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserTokens",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "WebAuthnCredential",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserSessions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "HangfireState",
                schema: "jobs");

            migrationBuilder.DropTable(
                name: "HangfireJob",
                schema: "jobs");
        }
    }
}
