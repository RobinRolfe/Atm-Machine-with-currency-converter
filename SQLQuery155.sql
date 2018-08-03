SELECT * 
FROM Transactions 
WHERE CONVERT(date, TransDate, 101) BETWEEN '1/1/2018' and '1/2/2018'
select * from AtmAccount

CREATE PROCEDURE GetcustomdateTransaction
@date1 varchar(100),
@date2 varchar(100),
@userid varchar(200)
AS
select atmacc.AccountNumber,atmacc.AccountBalance,ttype.TransType,trans.TransDate,trans.TransAmount,trans.TransState,atype.AccountType
from AtmAccount  atmacc inner join  Transactions  trans on atmacc.atmaccId=trans.atmaccId
inner join TransType  ttype on ttype.TranstypeId=trans.TranstypeId
inner join AccTypes atype on atype.acctypeId=atmacc.AccTypeId
WHERE  CONVERT(date, TransDate, 101) BETWEEN @date1 and @date2
and atmacc.AccUserId=@userid

Exec GetcustomdateTransaction '1/1/2018' ,'1/2/2018','63ae8801-ff72-48bb-892a-d03f20142add'