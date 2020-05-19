namespace review_v2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class seekDB : DbMigration
    {
        public override void Up()
        {
            Sql(@"
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'3e3dfa3e-11a8-4876-ba50-0180ae3048c9', N'noura@gmail.com', 0, N'ABu2OVXoJkTuiTxwHCHRaoLl9YkFK7MtEdkPtFEs4SAwh3GECwxedDs+Zqfx6ENa5g==', N'a36dd240-6818-4830-91b5-15ca97ed94c6', NULL, 0, 0, NULL, 1, 0, N'noura@gmail.com')
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'6764ec46-a045-4dfb-88d7-6b7625e19066', N'IBM@gmail.com', 0, N'AHnIq42mrFsN0ZlsaTodbTfuwjDGZ4lBErh5qY30DLe+0thX47at08vXMTaIX16Qaw==', N'7790c0f7-e75f-4bac-9fc3-ea21ebffdca7', NULL, 0, 0, NULL, 1, 0, N'IBM@gmail.com')
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'6e59f33e-1cd9-4572-bd10-853da4fbc1cd', N'neven@gmail.com', 0, N'ACj0GLvB1sXp4m3t6URPIcl/thBsTOPc5lJWZu9SicVLiGDujdU5xxTqhlID3bH6EQ==', N'29beb990-5626-4df4-be9d-f56566f00103', NULL, 0, 0, NULL, 1, 0, N'neven@gmail.com')
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'c9f38709-f3a8-489d-8648-a4234be0ee47', N'Abeer@gmail.com', 0, N'AFkqyJ6NKrXF3LTL097RGJn5oCPqnFKAj0ox+hjP1HfDoqU0N7EG2rhrU3U0S+NtgA==', N'3de4d808-38fa-42a6-97e4-9cca082f8440', NULL, 0, 0, NULL, 1, 0, N'Abeer@gmail.com')
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'cc418470-2eb0-433a-b0bc-4c0e5c071bbe', N'hadeer@gmail.com', 0, N'AOtXd8ouwwirNrHetQmYRhpYLW6EJJWTHgC5OOtCznRrpjiG5rPqFWH1n9/TDX8HCg==', N'fedddfaf-f767-4418-babd-3204800baff0', NULL, 0, 0, NULL, 1, 0, N'hadeer@gmail.com')

INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'1', N'Admin')
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'2', N'Company')
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'3', N'Customer')

INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'6e59f33e-1cd9-4572-bd10-853da4fbc1cd', N'1')
INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'6764ec46-a045-4dfb-88d7-6b7625e19066', N'2')
INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'3e3dfa3e-11a8-4876-ba50-0180ae3048c9', N'3')
INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'c9f38709-f3a8-489d-8648-a4234be0ee47', N'3')
INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'cc418470-2eb0-433a-b0bc-4c0e5c071bbe', N'3')


");
        }

        public override void Down()
        {
        }
    }
}
