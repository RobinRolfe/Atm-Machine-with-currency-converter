select DATEADD(dd,-14,CONVERT(datetime,CONVERT(nvarchar(11),GETDATE()))) 

select * from AtmAccount

SELECT *
FROM Transactions
WHERE TransDate >= DATEADD(day,-14, GETDATE()) 

SELECT * 
FROM Transactions 
WHERE CONVERT(date, TransDate, 101) BETWEEN '1/1/2013' and '1/2/2013


CREATE PROCEDURE GetLasttwoWeekTransaction
@userid varchar(200)
AS

select atmacc.AccountNumber,atmacc.AccountBalance,ttype.TransType,trans.TransDate,trans.TransAmount,trans.TransState,atype.AccountType
from AtmAccount  atmacc inner join  Transactions  trans on atmacc.atmaccId=trans.atmaccId
inner join TransType  ttype on ttype.TranstypeId=trans.TranstypeId
inner join AccTypes atype on atype.acctypeId=atmacc.AccTypeId
WHERE trans.TransDate >= DATEADD(day,-14, GETDATE()) 
and atmacc.AccUserId=@userid

exec GetLasttwoWeekTransaction '63ae8801-ff72-48bb-892a-d03f20142add'