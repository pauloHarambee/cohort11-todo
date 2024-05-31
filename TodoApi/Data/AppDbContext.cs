using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        //app db context
        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options){

        }
        public DbSet<TodoTask> TodoTasks{get;set;}
    }
}