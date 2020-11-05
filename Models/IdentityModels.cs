using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using BackEnd.Models.OnlineShop;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BackEnd.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public byte[] UserPhoto { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public string Full_Name { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Institution> Institutions { get; set; }
        public DbSet<Individual> Individuals { get; set; }
        public DbSet<ApprovedInstitution> ApprovedInstitutions { get; set; }
        //public DbSet<Deposit> Deposits { get; set; }
        public DbSet<Models.Deposit> Deposits { get; set; }
        public DbSet<ApprovedDeposit> ApprovedDeposits { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<PackageType> PackageTypes { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Event>Events { get; set; }

        public DbSet<DbDataPoint> dbDataPoints { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<DisplayImages> displayImages { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Cart_Item> Cart_Items { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Order_Item> Order_Items { get; set; }
        public DbSet<Order_Tracking> Order_Trackings { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Shipping_Address> Shipping_Addresses { get; set; }
        public DbSet<Payment> Payments { get; set; }
        //
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Affiliate> Affiliates { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Models.OnlineShop.DepositTrans> DepositsS { get; set; }
        public DbSet<Withdraw> Withdrawals { get; set; }
        public DbSet<Affiliate_Joiner> Affiliate_Joiners { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<WithdrawLevel> WithdrawLevels { get; set; }

        public System.Data.Entity.DbSet<Review> Reviews { get; set; }

    }
}