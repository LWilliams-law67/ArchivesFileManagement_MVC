using ArchivesFileManagement_MVCDB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchivesFileManagement_MVC.Data
{
    public class UserRole
    {
        public static Users GetUserRole(string username)
        {
            BCC_ArchivesContext db = new BCC_ArchivesContext();
            string[] login = username.Split('\\');
            username = login[1];
            return db.Users.Include(u => u.Role).FirstOrDefault(u => u.UserName.Equals(username) || u.Email.Equals(username));
        }
    }
}
