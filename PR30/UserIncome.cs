//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class UserIncome
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public int IdCategorieIncome { get; set; }
        public decimal Amount { get; set; }
        public System.DateTime Data { get; set; }
        public string Description { get; set; }
    
        public virtual IncomeCategorie IncomeCategorie { get; set; }
        public virtual User User { get; set; }
    }
}
