using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DomainBase
{
    /// <summary>
    /// 领域实体标记
    /// </summary>
    public abstract class Entity
    {

        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 租户
        /// </summary>
        public int TenantId { get; set; }
    }
}
