namespace CQ.Domain.Entity.QPAccount
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ModelTest : DbContext
    {
        public ModelTest()
            : base("name=QpAccount")
        {
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<AccountFreeze> AccountFreeze { get; set; }
        public virtual DbSet<AccountLastLogin> AccountLastLogin { get; set; }
        public virtual DbSet<AccountParentID> AccountParentID { get; set; }
        public virtual DbSet<AccountRegInfo> AccountRegInfo { get; set; }
        public virtual DbSet<AddGoldLog> AddGoldLog { get; set; }
        public virtual DbSet<BindMachine> BindMachine { get; set; }
        public virtual DbSet<IPBlackList> IPBlackList { get; set; }
        public virtual DbSet<IPWhiteList> IPWhiteList { get; set; }
        public virtual DbSet<OfflineMessage> OfflineMessage { get; set; }
        public virtual DbSet<OfflineMsgBonusLog> OfflineMsgBonusLog { get; set; }
        public virtual DbSet<RbtNickName> RbtNickName { get; set; }
        public virtual DbSet<RbtNickNameLog> RbtNickNameLog { get; set; }
        public virtual DbSet<SafeWayMsg> SafeWayMsg { get; set; }
        public virtual DbSet<UserAccountInfo> UserAccountInfo { get; set; }
        public virtual DbSet<UserAdditionDescribe> UserAdditionDescribe { get; set; }
        public virtual DbSet<UserAdditionSolution> UserAdditionSolution { get; set; }
        public virtual DbSet<UserGYB2Gold> UserGYB2Gold { get; set; }
        public virtual DbSet<UserTasksExtraDayInfo> UserTasksExtraDayInfo { get; set; }
        public virtual DbSet<UserCheckMacList> UserCheckMacList { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .Property(e => e.AccountName)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.Password)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.BankPassword)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<AccountFreeze>()
                .Property(e => e.LockMessage)
                .IsUnicode(false);

            modelBuilder.Entity<AccountLastLogin>()
                .Property(e => e.LastLoginIP)
                .IsUnicode(false);

            modelBuilder.Entity<AccountLastLogin>()
                .Property(e => e.LastLoginMac)
                .IsUnicode(false);

            modelBuilder.Entity<AccountRegInfo>()
                .Property(e => e.RegisterAddress)
                .IsUnicode(false);

            modelBuilder.Entity<AccountRegInfo>()
                .Property(e => e.RegisterMac)
                .IsUnicode(false);

            modelBuilder.Entity<AccountRegInfo>()
                .Property(e => e.IdentityCard)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<AccountRegInfo>()
                .Property(e => e.Telephone)
                .IsUnicode(false);

            modelBuilder.Entity<AddGoldLog>()
                .Property(e => e.account)
                .IsUnicode(false);

            modelBuilder.Entity<BindMachine>()
                .Property(e => e.MACAddress)
                .IsUnicode(false);

            modelBuilder.Entity<OfflineMessage>()
                .Property(e => e.Message)
                .IsUnicode(false);

            modelBuilder.Entity<OfflineMsgBonusLog>()
                .Property(e => e.Message)
                .IsUnicode(false);

            modelBuilder.Entity<SafeWayMsg>()
                .Property(e => e.PhoneNumber)
                .IsUnicode(false);

            modelBuilder.Entity<UserTasksExtraDayInfo>()
                .Property(e => e.UserDayInfo)
                .IsUnicode(false);

            modelBuilder.Entity<UserTasksExtraDayInfo>()
                .Property(e => e.UserGlobalInfo)
                .IsUnicode(false);

            modelBuilder.Entity<UserCheckMacList>()
                .Property(e => e.MacAddressList)
                .IsUnicode(false);
        }
    }
}
