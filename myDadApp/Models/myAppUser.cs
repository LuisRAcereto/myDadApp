using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace myDadApp.Models
{
    public class MyAppUser : IdentityUser
    {
        [PersonalData]
        public string Organization { get; set; }
        public bool ShowCats { get; set; }
        public int ChoreCnt { get; set; }
        public MyAppUser()
        {
            ShowCats = false;
            ChoreCnt = 0;
        }
    }
}
