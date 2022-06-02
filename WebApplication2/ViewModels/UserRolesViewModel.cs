using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.ViewModels
{
    public class UserRolesViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public Patient Patient { get; set; }

        public Doctor Doctor { get; set; }
        public List<RoleViewModel> Roles { get; set; }
    }
}
