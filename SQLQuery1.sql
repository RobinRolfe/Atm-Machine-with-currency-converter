select * from  Transactions

select * from AtmAccount

select * from TransType

select * from AccTypes



accoun_Numer Account_balance   Transactiontype   Transactiondate amount  state  accounttype


var entryPoint = (from ep in dbContext.tbl_EntryPoint
                 join e in dbContext.tbl_Entry on ep.EID equals e.EID
                 join t in dbContext.tbl_Title on e.TID equals t.TID
				 join acctype in 
                 where e.OwnerID == user.UID
                 select new {
                     UID = e.OwnerID,
                     TID = e.TID,
                     Title = t.Title,
                     EID = e.EID
                 }).Take(10);





