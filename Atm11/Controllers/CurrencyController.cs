using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Atm11.Controllers
{
    public class CurrencyController : Controller
    {

        private BankdbEntities db = new BankdbEntities();
        


        // GET: Currency
        public ActionResult Index()
        {
            CurrencyClient cc = new CurrencyClient();
            ViewBag.listCurrencies = cc.findAll();
            return View();
        }


        public String drpCurrencyCountryList()
        {
            CurrencyClient cc = new CurrencyClient();


            // ViewBag.listCurrencies = cc.findAll();

            var currencyList = cc.findAll().ToList();
            string output = "<option value=''>Select Currency</option>"; ///THIS APPEARS IN THE DROP DOWN MENU AS DEFAULT

            foreach (var item in currencyList)
            {
                output += "<option value='" + item.Conversion_Rate + "'>" + item.Currency_Name + "</option>";

            }

            return output;
        }

        public ActionResult GetCurrWithDraw()
        {

            return View();
        }




        //THIS IS FOR MAKING A WITHDRAW USING CURRENCY
        [Authorize]
        [HttpPost]
        public string GetCurrWithDraw(string cbal, string amount, string ano, int aid, int atypeid, string currency)
        {
            string msg = "";
            decimal currentbalance = Decimal.Parse(cbal);
            if (currentbalance > 0)
            {

                decimal am = Decimal.Parse(amount);
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
                    msg = am + " Amount Withdrawn successfully.....";
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
  }
