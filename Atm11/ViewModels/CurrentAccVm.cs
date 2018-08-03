using Atm11.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Atm11.ViewModels
{
    public class CurrentAccVm
    {

       // public List<BankAcc> BankAccList { get; set; }
        public string UserName  { get; set; }
        //public BankAcc accs  { get; set; }
        public decimal amount { get; set; }
        public int accUserId { get; set; }
        

    }
}