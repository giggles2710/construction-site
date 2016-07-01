USE [master]
GO
/****** Object:  Database [ConstructionSite]    Script Date: 01/07/2016 4:27:43 SA ******/
CREATE DATABASE [ConstructionSite]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ConstructionSite', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\ConstructionSite.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'ConstructionSite_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\ConstructionSite_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [ConstructionSite] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ConstructionSite].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ConstructionSite] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ConstructionSite] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ConstructionSite] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ConstructionSite] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ConstructionSite] SET ARITHABORT OFF 
GO
ALTER DATABASE [ConstructionSite] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ConstructionSite] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [ConstructionSite] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ConstructionSite] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ConstructionSite] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ConstructionSite] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ConstructionSite] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ConstructionSite] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ConstructionSite] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ConstructionSite] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ConstructionSite] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ConstructionSite] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ConstructionSite] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ConstructionSite] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ConstructionSite] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ConstructionSite] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ConstructionSite] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ConstructionSite] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ConstructionSite] SET RECOVERY FULL 
GO
ALTER DATABASE [ConstructionSite] SET  MULTI_USER 
GO
ALTER DATABASE [ConstructionSite] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ConstructionSite] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ConstructionSite] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ConstructionSite] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'ConstructionSite', N'ON'
GO
USE [ConstructionSite]
GO
/****** Object:  StoredProcedure [dbo].[CheckUniqueEmailAddress]    Script Date: 01/07/2016 4:27:47 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CheckUniqueEmailAddress]
	-- Add the parameters for the stored procedure here
	@EmailAddress nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT TOP 1 AccountID FROM Account
	WHERE EmailAddress = @EmailAddress
END

GO
/****** Object:  StoredProcedure [dbo].[GetHashTokenByEmailAddress]    Script Date: 01/07/2016 4:27:47 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetHashTokenByEmailAddress] 
	-- Add the parameters for the stored procedure here
	@EmailAddress nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT TOP 1 AccountID, HashToken FROM Account 
	WHERE EmailAddress = @EmailAddress AND IsActive = 1
END

GO
/****** Object:  StoredProcedure [dbo].[LoginByEmailAdress]    Script Date: 01/07/2016 4:27:47 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Thuan,,Nguyen>
-- Create date: <05/06/2016>
-- Description:	<login with email address and password>
-- =============================================
CREATE PROCEDURE [dbo].[LoginByEmailAdress] 
	-- Add the parameters for the stored procedure here
	@EmailAddress nvarchar(50),
	@Password nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT AccountID, Username FROM Account
	WHERE EmailAddress = @EmailAddress AND Password = @Password
END

GO
/****** Object:  Table [dbo].[Account]    Script Date: 01/07/2016 4:27:47 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Account](
	[AccountID] [nvarchar](50) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[PhoneNumber] [nvarchar](50) NULL,
	[EmailAddress] [nvarchar](max) NULL,
	[RoleID] [int] NULL,
	[HashToken] [nvarchar](max) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[ModifiedDate] [date] NOT NULL,
	[CreatedUserID] [nvarchar](50) NULL,
	[Note] [nvarchar](max) NULL,
	[Image] [varbinary](max) NULL,
	[ImageType] [nvarchar](50) NULL,
	[FailedLoginCount] [int] NULL,
	[EndDeactiveTime] [datetime] NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[AccountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Category]    Script Date: 01/07/2016 4:27:47 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Category](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[ParentID] [int] NULL,
	[Image] [varbinary](max) NULL,
	[ImageType] [nvarchar](50) NULL,
	[DateModified] [date] NOT NULL,
	[CreatedUserID] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 01/07/2016 4:27:47 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[CustomerID] [int] IDENTITY(1,1) NOT NULL,
	[Fullname] [nvarchar](100) NOT NULL,
	[Address] [nvarchar](50) NOT NULL,
	[City] [nvarchar](50) NOT NULL,
	[District] [nvarchar](50) NOT NULL,
	[Phone] [nvarchar](50) NOT NULL,
	[EmailAddress] [nvarchar](50) NOT NULL,
	[ShipAddress] [nvarchar](50) NULL,
	[ShipCity] [nvarchar](50) NULL,
	[ShipDistrict] [nvarchar](50) NULL,
	[ShipPhone] [nvarchar](50) NULL,
	[DateEntered] [date] NULL,
	[AdditionalInformation] [nvarchar](220) NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EmailTemplate]    Script Date: 01/07/2016 4:27:47 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailTemplate](
	[EmailTemplateId] [int] IDENTITY(1,1) NOT NULL,
	[EmailTemplateName] [nvarchar](200) NOT NULL,
	[EmailSubject] [nvarchar](200) NOT NULL,
	[HtmlBody] [nvarchar](max) NULL,
	[PlainText] [nvarchar](max) NULL,
	[IsBodyHtml] [bit] NULL,
	[IsEnable] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[LastUpdatedDate] [datetime] NULL,
	[LastUpdatedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_EmailTemplate] PRIMARY KEY CLUSTERED 
(
	[EmailTemplateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MergeField]    Script Date: 01/07/2016 4:27:47 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MergeField](
	[FieldId] [int] IDENTITY(1,1) NOT NULL,
	[FieldName] [nvarchar](50) NOT NULL,
	[FieldType] [int] NOT NULL,
 CONSTRAINT [PK_MergeField] PRIMARY KEY CLUSTERED 
(
	[FieldId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Order]    Script Date: 01/07/2016 4:27:47 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerID] [int] NOT NULL,
	[OrderNumber] [nvarchar](50) NOT NULL,
	[OrderDate] [date] NOT NULL,
	[ShipDate] [date] NULL,
	[RequiredDate] [date] NULL,
	[ShipperID] [int] NULL,
	[Freight] [float] NOT NULL,
	[SalesTax] [money] NOT NULL,
	[OrderStatus] [nvarchar](50) NOT NULL,
	[IsFulfilled] [bit] NOT NULL,
	[IsCanceled] [bit] NOT NULL,
	[Paid] [money] NULL,
	[PaymentDate] [date] NULL,
	[ModifiedDate] [date] NOT NULL,
	[ModifiedUserID] [nvarchar](50) NULL,
 CONSTRAINT [PK_OrderID] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OrderDetail]    Script Date: 01/07/2016 4:27:47 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetail](
	[OrderDetailID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NOT NULL,
	[OrderID] [int] NOT NULL,
	[OrderNumber] [nvarchar](50) NOT NULL,
	[Price] [money] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Discount] [float] NULL,
	[Total] [money] NOT NULL,
	[Size] [int] NOT NULL,
	[IsFulfilled] [bit] NOT NULL,
	[ShipDate] [date] NULL,
	[PaidDate] [date] NULL,
 CONSTRAINT [PK_OrderDetail] PRIMARY KEY CLUSTERED 
(
	[OrderDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Product]    Script Date: 01/07/2016 4:27:47 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Product](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Summary] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Image] [varbinary](max) NULL,
	[ImageType] [nvarchar](5) NULL,
	[CategoryID] [int] NULL,
	[UnitsInStock] [int] NOT NULL,
	[Discount] [float] NULL,
	[UnitPrice] [money] NOT NULL,
	[DiscountedPrice] [money] NULL,
	[IsDiscountAvailable] [bit] NOT NULL,
	[IsAvailable] [bit] NOT NULL,
	[SupplierID] [int] NULL,
	[OrderCount] [int] NULL,
	[ViewCount] [int] NULL,
	[Rate] [int] NULL,
	[DateModified] [date] NOT NULL,
	[CreatedUserID] [nvarchar](50) NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProductDetail]    Script Date: 01/07/2016 4:27:47 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductDetail](
	[ProductDetailID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[Value] [nvarchar](150) NOT NULL,
	[ProductID] [int] NOT NULL,
	[Type] [int] NOT NULL,
 CONSTRAINT [PK_Specification] PRIMARY KEY CLUSTERED 
(
	[ProductDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Request]    Script Date: 01/07/2016 4:27:47 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Request](
	[RequestID] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](150) NOT NULL,
	[EmailAddress] [nvarchar](50) NOT NULL,
	[RequestContent] [nvarchar](max) NOT NULL,
	[IsNew] [bit] NOT NULL,
	[ReplyUser] [nvarchar](50) NULL,
	[Reply] [nvarchar](max) NULL,
	[DateCreated] [date] NOT NULL,
 CONSTRAINT [PK_Request] PRIMARY KEY CLUSTERED 
(
	[RequestID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Role]    Script Date: 01/07/2016 4:27:47 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[RoleID] [nvarchar](50) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Shipper]    Script Date: 01/07/2016 4:27:47 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Shipper](
	[ShipperID] [int] IDENTITY(1,1) NOT NULL,
	[CompanyName] [nvarchar](50) NOT NULL,
	[Phone] [nvarchar](50) NOT NULL,
	[EmailAddress] [nvarchar](50) NULL,
	[Address] [nvarchar](50) NULL,
	[City] [nvarchar](50) NULL,
	[District] [nvarchar](50) NULL,
 CONSTRAINT [PK_Shipper] PRIMARY KEY CLUSTERED 
(
	[ShipperID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Supplier]    Script Date: 01/07/2016 4:27:47 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Supplier](
	[SupplierID] [int] IDENTITY(1,1) NOT NULL,
	[CompanyName] [nvarchar](50) NOT NULL,
	[ContactFName] [nvarchar](50) NOT NULL,
	[ContactLName] [nvarchar](50) NOT NULL,
	[Address1] [nvarchar](50) NOT NULL,
	[Address2] [nvarchar](50) NULL,
	[City] [nvarchar](50) NOT NULL,
	[Phone] [nvarchar](50) NOT NULL,
	[Fax] [nvarchar](50) NULL,
	[EmailAddress] [nvarchar](50) NULL,
	[Discount] [float] NOT NULL,
	[ProductType] [nvarchar](50) NOT NULL,
	[Logo] [varbinary](max) NULL,
	[ImageType] [nvarchar](50) NULL,
	[IsDiscountAvailable] [bit] NULL,
 CONSTRAINT [PK_Supplier] PRIMARY KEY CLUSTERED 
(
	[SupplierID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SystemVariable]    Script Date: 01/07/2016 4:27:47 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemVariable](
	[VariableId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_SystemVariable] PRIMARY KEY CLUSTERED 
(
	[VariableId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [FK_Account_Account] FOREIGN KEY([CreatedUserID])
REFERENCES [dbo].[Account] ([AccountID])
GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [FK_Account_Account]
GO
ALTER TABLE [dbo].[Category]  WITH CHECK ADD  CONSTRAINT [FK_Category_Category] FOREIGN KEY([ParentID])
REFERENCES [dbo].[Category] ([CategoryID])
GO
ALTER TABLE [dbo].[Category] CHECK CONSTRAINT [FK_Category_Category]
GO
ALTER TABLE [dbo].[EmailTemplate]  WITH CHECK ADD  CONSTRAINT [FK_EmailTemplate_Account] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Account] ([AccountID])
GO
ALTER TABLE [dbo].[EmailTemplate] CHECK CONSTRAINT [FK_EmailTemplate_Account]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Account] FOREIGN KEY([ModifiedUserID])
REFERENCES [dbo].[Account] ([AccountID])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Account]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Customer] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([CustomerID])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Customer]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Shipper] FOREIGN KEY([ShipperID])
REFERENCES [dbo].[Shipper] ([ShipperID])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Shipper]
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetail_Order] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Order] ([OrderID])
GO
ALTER TABLE [dbo].[OrderDetail] CHECK CONSTRAINT [FK_OrderDetail_Order]
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetail_Product] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO
ALTER TABLE [dbo].[OrderDetail] CHECK CONSTRAINT [FK_OrderDetail_Product]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Account] FOREIGN KEY([CreatedUserID])
REFERENCES [dbo].[Account] ([AccountID])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Account]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Category] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Category] ([CategoryID])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Category]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Supplier] FOREIGN KEY([SupplierID])
REFERENCES [dbo].[Supplier] ([SupplierID])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Supplier]
GO
ALTER TABLE [dbo].[ProductDetail]  WITH CHECK ADD  CONSTRAINT [FK_ProductDetail_Product] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO
ALTER TABLE [dbo].[ProductDetail] CHECK CONSTRAINT [FK_ProductDetail_Product]
GO
ALTER TABLE [dbo].[Request]  WITH CHECK ADD  CONSTRAINT [FK_Request_Account] FOREIGN KEY([ReplyUser])
REFERENCES [dbo].[Account] ([AccountID])
GO
ALTER TABLE [dbo].[Request] CHECK CONSTRAINT [FK_Request_Account]
GO
ALTER TABLE [dbo].[Role]  WITH CHECK ADD  CONSTRAINT [FK_Role_Account] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Account] ([AccountID])
GO
ALTER TABLE [dbo].[Role] CHECK CONSTRAINT [FK_Role_Account]
GO
USE [master]
GO
ALTER DATABASE [ConstructionSite] SET  READ_WRITE 
GO
