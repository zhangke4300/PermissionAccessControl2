// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using DataKeyParts;
using DataLayer.EfCode.Configurations;
using DataLayer.ExtraAuthClasses;
using DataLayer.MultiTenantClasses;
using System.Linq;

namespace DataLayer.EfCode
{
    public class CompanyDbContext : DbContext
    {
        internal readonly string DataKey;

        public DbSet<TenantBase> Tenants { get; set; }
        public DbSet<ShopStock> ShopStocks { get; set; }
        public DbSet<ShopSale> ShopSales { get; set; }

        public DbSet<TimeStore> TimeStores { get; set; }

        public CompanyDbContext(DbContextOptions<CompanyDbContext> options, IGetClaimsProvider claimsProvider)
            : base(options)
        {
            DataKey = claimsProvider.DataKey;
        }

        //I only have to override these two version of SaveChanges, as the other two SaveChanges versions call these
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.MarkWithDataKeyIfNeeded(DataKey);
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            this.MarkWithDataKeyIfNeeded(DataKey);
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        //private void MarkWithDataKeyIfNeeded(string accessKey)
        //{
        //    //at startup access key can be null. The demo setup sets the DataKey directly.
        //    if (accessKey == null)
        //        return;

        //    foreach (var entityEntry in this.ChangeTracker.Entries()
        //        .Where(e => e.State == EntityState.Added))
        //    {
        //        if (entityEntry.Entity is IShopLevelDataKey hasDataKey)
        //            hasDataKey.SetShopLevelDataKey(accessKey);
        //    }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.TenantBaseConfig();
            modelBuilder.CompanyDbConfig(this);
        }
    }
}