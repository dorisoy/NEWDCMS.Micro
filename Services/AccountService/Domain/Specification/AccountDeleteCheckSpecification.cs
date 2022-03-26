using Domain.Entities;
using DomainBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specification
{
    public class AccountDeleteCheckSpecification : ISpecification<Account>
    {
        private readonly int CurrentId;
        public AccountDeleteCheckSpecification(int currentId)
        {
            CurrentId = currentId;
        }
        public async Task<bool> IsSatisfiedBy(Account entity)
        {
            if (CurrentId == entity.Id)
                throw new DomainException("登录用户不能删除自己!");
            return await Task.FromResult(true);
        }
    }
}
