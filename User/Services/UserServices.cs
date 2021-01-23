using Data.DbContexts;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Services
{
    public class UserServices
    {

        // If user is registered this metod will return user id else return -1.
        public int Login(User user)
        {
            int check= -1;
            if (user == null)
                return check;
            using (TestDbContext context= new TestDbContext())
            {
                var userCheck=context.Users.Where(x => x.Username == user.Username && x.Password == user.Password).FirstOrDefault();
                if (userCheck != null) {
                    return userCheck.Id;
                }
            }
            return check;
        }
        // This method will register user.If save process be successful will return true else false.
        public bool SingUp(User user)
        {
            bool result = false;
            if (user == null)
                return result;
            using (TestDbContext context = new TestDbContext())
            {
                context.Users.Add(user);
                context.SaveChanges();
                result = true;
            }
            return result;
        }
        // This method  delete user password.

        public User DeleteUserFromId(int userId)
        {
            User user = null;
            using (TestDbContext context = new TestDbContext())
            {
                user = GetUser(userId);
                if (user != null)
                {
                    context.Users.Remove(user);
                    context.SaveChanges();
                }
            }
            return user;
        }
        // This method  change user data.
        public bool EditUser(User user)
        {
            bool result = false;
            if (user == null)
                return result;
            using (TestDbContext context = new TestDbContext())
            {
                var userCheck = context.Users.Where(x => x.Id == user.Id && x.Password==user.Password ).FirstOrDefault();
                if (userCheck != null)
                {
                    userCheck.Username = user.Username;
                    context.SaveChanges();
                    result = true;
                }
            }
            return result;
        }
        // This method get all users.
        public List<User> GetUsers()
        {
            using (TestDbContext context = new TestDbContext())
            {
                List<User> users = context.Users.ToList();
                return users;
            }
        }
        //This method  return user from id
        public User GetUser(int userId)
        {
            User user;
            if (userId <= 0)
                return null;
            using (TestDbContext context = new TestDbContext())
            {
                user = context.Users.Where(x => x.Id == userId).FirstOrDefault();
            }   
            return user;
        }
    }
}
