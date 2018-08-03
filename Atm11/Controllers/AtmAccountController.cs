using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Atm11.Controllers
{
    public class AtmAccountController : Controller
    {

        private BankdbEntities db = new BankdbEntities();
       
        // GET ALL ACCOUNTS OF THE CURRENTLY LOGGED IN USER AND RETURN THEM TO THE VIEW AS A LIST
        [Authorize]
        public ActionResult Index()
        {

            var userId = User.Identity.GetUserId();
            var bankAccs = db.AtmAccounts.Where(a => a.AccUserId == userId).ToList();
            return View(bankAccs.ToList());

        }






        //START OF THE FUND TRANSFER
        //FUND TRANSFER METHOD 
        [Authorize]
        [HttpGet]
         public ActionResult FundTrasnfer()
        {
            return View();
        }

        
        //FILL DROPDOWNLIST SOURCE ACCOUNT LIST
        public String fillDropdownlistSource()
        {
            

            var userId = User.Identity.GetUserId(); //CURRENTLY LOGGED IN USER
            var accountlist = db.AtmAccounts.Where(a => a.AccUserId == userId).ToList();////CREATES A LIST OF THE ACCOUNTS OF THE CURRENTLY LOGGED IN USER TO SELECT AS THE SOURCE ACCOUNT

            string output = "<option value=''>Select Source Account </option>"; ///THIS APPEARS IN THE DROP DOWN MENU AS DEFAULT

            foreach (var item in accountlist)
            {
                output += "<option value='" + item.atmaccId + "'>" + item.AccountNumber + "</option>";

            }

            return output;
        }

        

        //Fill Dropdownlist Target account list
        public String fillDropdownlistTarget()
        {
            

            var userId = User.Identity.GetUserId(); //currently logged in user
            var accountlist = db.AtmAccounts.Where(a => a.AccUserId == userId).ToList();////creates a list of the accounts of the currently logged in user to select as the source account

            string output = "<option value=''>Select Target Account </option>"; ///this appears in the drop down menu as default

            foreach (var item in accountlist)
            {
                output += "<option value='" + item.AccountNumber + "'>" + item.AccountNumber + "</option>";

            }

            return output;
        }


        ////This is the method that executes when the transfer button is clicked
        [Authorize]
        [HttpPost]
        public String FundTrasnfer(int saccount, string daccount, decimal m)
        {
           
            string msg = "";
            string accountNumber = "";
            decimal balance = 0;
            int atypeid = 0;
            var bankAccs = db.AtmAccounts.Where(a => a.atmaccId == saccount).ToList();
            foreach (var item in bankAccs)
            {
                accountNumber = item.AccountNumber;
                balance = item.AccountBalance;
                atypeid = item.AccTypeId;
            }
            if (m > balance)
            {
                msg = "Insufficient Balance to Transfer payment !!!!!!!!!!!!";
            }

            else if (accountNumber == daccount)
            {
                msg = "Source account is the same as target account sorry...try again !!!!!!!!!!!!";
            }





            else
            {

                //this starts transaction from one account to another account.

                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {

                        //Updating the source account
                        var aa = db.AtmAccounts.Find(saccount); //finds the source account
                        aa.AccountNumber = accountNumber; ;
                        aa.AccountBalance = balance - m; //creating the new balance
                        aa.AccUserId = User.Identity.GetUserId();
                        aa.AccTypeId = atypeid;
                        //Save to the database
                        db.Entry(aa).State = System.Data.Entity.EntityState.Modified;  //update the database with the new info
                        db.SaveChanges();


                        //Updating the target account
                        var bankAccs1 = db.AtmAccounts.Where(b1 => b1.AccountNumber == daccount).ToList();
                        
                        int atmid = 0;
                        string accuid = "";
                        int acctypeid = 0;
                        decimal bl = 0;
                        foreach (var item in bankAccs1)
                        {
                            atmid = item.atmaccId; //Target account Id
                            accuid = item.AccUserId;
                            acctypeid = item.AccTypeId;
                            bl = item.AccountBalance;

                        }
                        
                        var b = db.AtmAccounts.Find(atmid);
                        b.AccountBalance = bl + m;
                        b.AccountNumber = daccount;
                        b.AccUserId = accuid;
                        b.AccTypeId = acctypeid;
                        //Save to the database
                        db.Entry(b).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();


                        //This inserts into the transaction table

                        Transaction T = new Transaction();
                        T.atmaccId = saccount;
                        T.TransAmount = m;
                        T.TransDate = DateTime.Now;
                        T.TranstypeId = 3;
                        T.TransState = "Debit";

                        db.Transactions.Add(T);
                        db.SaveChanges();


                        //Transaction for target account 

                        

                        Transaction T1 = new Transaction();
                        T1.atmaccId = atmid;
                        T1.TransAmount = m;
                        T1.TransDate = DateTime.Now;
                        T1.TranstypeId = 3;
                        T1.TransState = "Credit";

                        db.Transactions.Add(T1);
                        db.SaveChanges();


                        transaction.Commit();

                        msg = "Amount Transferred Successfully .!!!!!!!!!!!";
                    }
                    catch (Exception ex)
                    {
                        //  transaction.Rollback();
                        // msg = "Someting went Wrong !!!!!!!!!!!!!!!";
                    }

                }
                
            }
            

            return msg;
        }




        ////Checks the target account exists (this code is not really needed after implementing drop down menus but ill leave it in anyway)
        [Authorize]
        [HttpGet]
        public string checkingTargetAccount(string acc) //this checks whether there is an account that matched the target account number
        {
            string msg = "";

            
            var accountlist = db.AtmAccounts.Where(a => a.AccountNumber == acc).ToList();
            if(accountlist.Count>0) //if 'accountlist' has at least one item...
            {
                msg = "yes"; ///returns if there is an account that matches
            }
            else
            {
                msg = "no"; ///returns if there is NO account that matches  (count is less than zero)
            }
            return msg;
        }


        //This is a method to create a random 10 digit number for use in the create account method
        public string RandomDigits(int length)
        {
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < length; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }

        // GET: atmccount/Create
        [Authorize]
        public ActionResult Create()
        {
            string accno = RandomDigits(10); ////assigns a random number to the 'accno' variable
            AtmAccount atm = new AtmAccount(); ////creates a new instance of AtmAccount from the model called atm
            atm.AccountNumber = accno;  //// assigning values to the properties
            atm.AccountBalance = 0;     //// assigning values to the properties
            atm.AccUserId = User.Identity.GetUserId(); //// assinging the id of the currently logged in user to a property of atm 

            ViewBag.acctypeId = new SelectList(db.AccTypes, "acctypeId", "AccountType");
            return View(atm);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(AtmAccount atm) 
        {

            db.AtmAccounts.Add(atm);//This creates the new record/account in the database
            db.SaveChanges();

            return RedirectToAction("index");
        }


        [Authorize]
        public ActionResult Manageaccount(int id)//This method is called when a user clicks on 'manage account' 
        {

            AtmAccount a = db.AtmAccounts.Find(id); ////finds the account according to the the value 'id' which is the 'atmaccId' (primary key) in the database


            return View(a);
        }


        //START OF THE 'VIEW PREVIOUS TRANSACTIONS' SECTION!!
        //THIS DISPLAYS THE VIEW WHEN A USER CLICKS ON 'VIEW PREVIOUS TRANSACTIONS'
        [Authorize]
        [HttpGet]
        public ActionResult AccountTransStatement()
        {

            return View();
        }

        //This method gets ALL the transactions 
        [Authorize]
        public string GetAllTransaction (int id)
        {
            var userId = User.Identity.GetUserId();

            var transactionrecords = db.GetAllTranaction1(userId, id).ToList();

            
            //Titles of the columns
            String output = "<table class='table'><tr><th>Account Number</th><th>Transaction Type</th><th>Transaction date</th><th>Transaction Amount</th><th>Transfer State</th><th>Account type</th></tr>";

            foreach(var item in transactionrecords)
            {

                //fills the table with the sql from the stored procedure
               output += "<tr><td>"+item.AccountNumber+ "</td><td>" + item.TransType + "</td><td>" + item.TransDate + "</td><td>" + item.TransAmount + "</td><td>" + item.TransState + "</td><td>" + item.AccountType + "</td></tr>";

            }

            output += "</table>";

            return output;
        }

        //This method gets the last 7 days transactions
        [Authorize]
        public string ViewLastWeekTransaction(int id)
        {
            var userId = User.Identity.GetUserId();

            var transactionrecords = db.GetLastWeekTransaction(userId,id).ToList();


            String output = "<table class='table'><tr><th>Account Number</th><th>Transaction Type</th><th>Transaction date</th><th>Transaction Amount</th><th>Transaction State</th><th>Account type</th></tr>";

            foreach (var item in transactionrecords)
            {

                output += "<tr><td>" + item.AccountNumber + "</td><td>" + item.TransType + "</td><td>" + item.TransDate + "</td><td>" + item.TransAmount + "</td><td>" + item.TransState + "</td><td>" + item.AccountType + "</td></tr>";

            }

            output += "</table>";

            return output;
        }

        //This method gets the last 14 days transactions
        [Authorize]
        public string ViewLasttwoWeekTransaction(int id)
        {
            var userId = User.Identity.GetUserId();

            var transactionrecords = db.GetLasttwoWeekTransaction(userId,id).ToList();


            String output = "<table class='table'><tr><th>Account Number</th><th>Transaction Type</th><th>Transaction date</th><th>Transaction Amount</th><th>Transaction State</th><th>Account type</th></tr>";

            foreach (var item in transactionrecords)
            {

                output += "<tr><td>" + item.AccountNumber + "</td><td>" + item.TransType + "</td><td>" + item.TransDate + "</td><td>" + item.TransAmount + "</td><td>" + item.TransState + "</td><td>" + item.AccountType + "</td></tr>";

            }

            output += "</table>";

            return output;
        }


        //This method gets the last 4 weeks transactions
        [Authorize]
        public String ViewLastMonthTransaction(int id)
        {

            var userId = User.Identity.GetUserId();

            var transactionrecords = db.GetLastFourWeekTransaction(userId,id).ToList();


            String output = "<table class='table'><tr><th>Account Number</th><th>Transaction Type</th><th>Transaction date</th><th>Transaction Amount</th><th>Transaction State</th><th>Account type</th></tr>";

            foreach (var item in transactionrecords)
            {

                output += "<tr><td>" + item.AccountNumber + "</td><td>" + item.TransType + "</td><td>" + item.TransDate + "</td><td>" + item.TransAmount + "</td><td>" + item.TransState + "</td><td>" + item.AccountType + "</td></tr>";

            }

            output += "</table>";

            return output;

        }


        //This method gets transactions between two custom dates
        [Authorize]
        public String ViewCustomdateTransaction(int id,string d1, string d2)
        {
         string[] str1 = d1.Split('-');
            string date1 = str1[1] + "/" + str1[2] + "/" + str1[0];

            string[] str2 = d2.Split('-');
            string date2 = str2[1] + "/" + str2[2] + "/" + str2[0];



            var userId = User.Identity.GetUserId();

            var transactionrecords = db.GetcustomdateTransaction(date1,date2,userId,id).ToList();


            String output = "<table class='table'><tr><th>Account Number</th><th>Balance</th><th>Transaction Type</th><th>Transaction date</th><th>Transaction Amount</th><th>Transaction State</th><th>Account type</th></tr>";

            foreach (var item in transactionrecords)
            {

                output += "<tr><td>" + item.AccountNumber + "</td><td>" + item.AccountBalance + "</td><td>" + item.TransType + "</td><td>" + item.TransDate + "</td><td>" + item.TransAmount + "</td><td>" + item.TransState + "</td><td>" + item.AccountType + "</td></tr>";

            }

            output += "</table>";

            return output;

        }


        //This if for making a deposit
        [Authorize]
        [HttpPost]
        public string GetDeposit(string cbal, string amount,string ano,int aid,int atypeid) //These variables are populated from the ajax in the view
        {
            decimal currentbalance = Decimal.Parse(cbal);  //assigning a variable to the current balance prior to deposit
            decimal am = Decimal.Parse(amount);    ///amount to be deposited
            decimal total = am + currentbalance;  ////creating the new balance

            AtmAccount a = new AtmAccount(); ////creates another instance of AtmAccount
            a.AccountNumber = ano; //assigning the values to the properties 
            a.AccountBalance = total;
            a.atmaccId = aid;
            a.AccTypeId = atypeid;
            a.AccUserId= User.Identity.GetUserId();

            db.Entry(a).State = System.Data.Entity.EntityState.Modified; //THIS MODIFIES THE DATABASE WITH THE NEW DATA
            db.SaveChanges();

            ////THIS CREATES A RECORD IN THE TRANSACTIONS TABLE
            Transaction t = new Transaction();
            t.atmaccId = aid;
            t.TransAmount = am;
            t.TransDate = DateTime.Now; //DATE AND TIME OF TRANSACTION
            t.TranstypeId = 1; ////REPRESENTS A DEPOSIT IN THE TRANSACTIONTYPE TABLE

            db.Transactions.Add(t);
            db.SaveChanges();
            return "£" + am + " Deposited successfully ";
        }

        //THIS IS FOR MAKING A WITHDRAWL
        [Authorize]
        [HttpPost]
        public string GetWithDraw(string cbal, string amount, string ano, int aid, int atypeid, string currency)
        {
            string msg = "";
            decimal currentbalance = Decimal.Parse(cbal);
            if (currentbalance > 0)
            {

                decimal am = Decimal.Parse(amount);
                decimal total = currentbalance - am;
                if(total>0)
                {

                    AtmAccount a = new AtmAccount();
                    a.AccountBalance = total;
                    a.AccountNumber = ano;
                    a.AccTypeId = atypeid;
                    a.atmaccId = aid;
                    a.AccUserId= User.Identity.GetUserId();
                    db.Entry(a).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();


                    Transaction t = new Transaction();
                    t.atmaccId = aid;
                    t.TransAmount = am;
                    t.TransDate = DateTime.Now;
                    t.TranstypeId = 2; ////represents a withdrawl in the transactiontype table

                    db.Transactions.Add(t);
                    db.SaveChanges();
                    msg = "£" + am + " Amount Withdrawn successfully";
                }
                else
                {
                    msg = "Insufficent Balance, Sorry !!!!!!!!!!!!!!!";
                }
                
            }
            else
            {
                msg = "You don't Have any Money, Sorry !!!!!!!!!!!!!!!";
            }

            return msg;
        }


        ////******************currency withdrawl


        public ActionResult GetCurrWithDraw(int id)
        {

            AtmAccount b = db.AtmAccounts.Find(id); ////finds the account according to the the value 'id' which is the 'atmaccId' (primary key) in the database


            return View(b);
        }




        //THIS IS FOR MAKING A WITHDRAW USING DIFFERENT CURRENCY
        [Authorize]
        [HttpPost]
        public string GetCurrWithDraw(string cbal, decimal amount, string ano, int aid, int atypeid, decimal currency, string currencyName)
        {

            decimal newAmount = amount;
            string msg = "";

            /////Conversion Start
            if (currencyName == "Euro")
            {
                newAmount = 1 / currency * newAmount;
                
                
                msg = amount + " Euros Sucessfully withdrawn at the exchange rate of £1 to every " + currency;

            }
            else if (currencyName == "Dollar")
            {
                newAmount = 1 / currency * newAmount;
                msg = amount + " US Dollars Sucessfully withdrawn at the exchange rate " + currency;

            }
            else if (currencyName == "Yen")
            {
                //newAmount = 1 / currency * newAmount;
                newAmount = Math.Truncate(1 / currency * newAmount);
                msg = amount + " Japanese Yen Sucessfully withdrawn at the exchange rate " + currency;

            }
            else if (currencyName == "Yuan")
            {
                newAmount = 1 / currency * newAmount;
                msg = amount + " Chinese Yaun Sucessfully withdrawn at the exchange rate " + currency;

            }
            else if (currencyName == "Rubel")
            {
                newAmount = 1 / currency * newAmount;
                msg = amount + " Russian Rubels Sucessfully withdrawn at the exchange rate " + currency;

            }
            else if (currencyName == "KongDollar")
            {
                newAmount = 1 / currency * newAmount;
                msg = amount + " Hong Kong Dollars Sucessfully withdrawn at the exchange rate " + currency;

            }
            ////Conversion End




            decimal currentbalance = Decimal.Parse(cbal);
            if (currentbalance > 0)
            {


                decimal am = newAmount;
                decimal total = currentbalance - am;
                if (total > 0)
                {

                    AtmAccount a = new AtmAccount();
                    a.AccountBalance = total;
                    a.AccountNumber = ano;
                    a.AccTypeId = atypeid;
                    a.atmaccId = aid;
                    a.AccUserId = User.Identity.GetUserId();
                    db.Entry(a).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();


                    Transaction t = new Transaction();
                    t.atmaccId = aid;
                    t.TransAmount = am;
                    t.TransDate = DateTime.Now;
                    t.TranstypeId = 2; ////represents a withdrawl in the transactiontype table

                    db.Transactions.Add(t);
                    db.SaveChanges();
                    //msg = am + " Amount Withdrawn successfully.....";
                }
                else
                {
                    msg = "Insufficent Balance, Sorry !!!!!!!!!!!!!!!";
                }

            }
            else
            {
                msg = "You don't Have any Money, Sorry !!!!!!!!!!!!!!!";
            }

            return msg;
        }
    }
    ///*********************currencey withdrawl



}