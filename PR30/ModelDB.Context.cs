﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PR30
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PR29DatabaseEntities : DbContext
    {
        public PR29DatabaseEntities()
            : base("name=PR29DatabaseEntities")
        {
        }
        private static PR29DatabaseEntities _context;

        public static PR29DatabaseEntities GetContext()
        {
            if (_context == null) _context = new PR29DatabaseEntities();
            return _context;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ExpenseCategory> ExpenseCategory { get; set; }
        public virtual DbSet<IncomeCategorie> IncomeCategorie { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserExpense> UserExpense { get; set; }
        public virtual DbSet<UserIncome> UserIncome { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
    }
}
