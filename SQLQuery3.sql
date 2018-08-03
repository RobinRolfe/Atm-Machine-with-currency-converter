CREATE TABLE [dbo].[AccTypes] (
    [acctypeId]          INT            IDENTITY (1, 1) NOT NULL,
    [AccountType] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_AccTypes] PRIMARY KEY CLUSTERED ([acctypeId] ASC)
	);
CREATE TABLE [dbo].[TransType] (
    [TranstypeId] INT   IDENTITY (1, 1) NOT NULL,
    [TransType] varchar(100) NOT NULL,    
    CONSTRAINT [PK_Deposits] PRIMARY KEY CLUSTERED ([TranstypeId] ASC),   
);

CREATE TABLE [dbo].[AtmAccount] (
    [atmaccId]        INT           IDENTITY (1, 1001) NOT NULL,
	[AccountNumber]     char(10),
    [AccountBalance]   DECIMAL (18)  DEFAULT ((0)) NOT NULL,
    [AccTypeId] INT           NOT NULL,
    [AccUserId] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_BankAccs] PRIMARY KEY CLUSTERED ([atmaccId] ASC),
    CONSTRAINT [FK_AccTypeBankAcc] FOREIGN KEY ([AccTypeId]) REFERENCES [dbo].[AccTypes] ([acctypeId])
);


CREATE TABLE [dbo].[Transactions] (
    [Id]            INT          IDENTITY (1, 1) NOT NULL,
    [TransAmount] DECIMAL (18) NOT NULL,
    [TransDate]   DATETIME     NOT NULL,
	[TranstypeId]  INT  not null,
	[atmaccId]     INT          NOT NULL,
    CONSTRAINT [PK_Transactions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_BankAccDeposit] FOREIGN KEY ([atmaccId] ) REFERENCES [dbo].[AtmAccount] ([atmaccId])
);



