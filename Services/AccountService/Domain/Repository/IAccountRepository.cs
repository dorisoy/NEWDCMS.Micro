﻿using Domain.Entities;
using DomainBase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<Account> FindAccountByAccounId(string loginName);
        Task<bool> CheckAcccountUserInfoAny(int accountId);
        Task<List<string>> GetAccountPermissions(int accountId);
    }
}
