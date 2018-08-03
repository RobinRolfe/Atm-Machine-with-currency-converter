USE [C:\USERS\ROB\DOCUMENTS\VISUAL STUDIO 2015\PROJECTS\ADDING VIEW OF ACCOUNTS TO TRANSFER\DEPOSIT\ATM11\ATM11\APP_DATA\BANKDB.MDF]
GO

DECLARE	@return_value Int

EXEC	@return_value = [dbo].[getAllTransactions]
		@aid = 1,
		@uid = N'63ae8801-ff72-48bb-892a-d03f20142add'

SELECT	@return_value as 'Return Value'

GO
