using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using review_v2.Models.DBViews;
using review_v2.Resources;

namespace review_v2.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [StringLength(11, ErrorMessage = "The minimum length must be at least 11 number.")]
        [Display(Name = "PhoneNumber", ResourceType = typeof(MyTexts))]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "The minimum length must be at least 11 number.")]
        public string PhoneNum { get; set; }

        public string UserImageURL { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<SentimentDb> SentimentsDb { get; set; }
        public DbSet<SentimentSpecial> SentimentsSpecialDb { get; set; }
        public DbSet<StopWord> StopWords { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductsSummaryView> ProductsSummaryVw { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ProductsSummaryViewConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        public System.Data.Entity.DbSet<review_v2.Models.Person> People { get; set; }

        //public System.Data.Entity.DbSet<review_v2.Models.ApplicationUser> ApplicationUsers { get; set; }

        //public System.Data.Entity.DbSet<review_v2.Models.Person> People { get; set; }
    }
    public class ProductsSummaryViewConfiguration : EntityTypeConfiguration<ProductsSummaryView>
    {
        public ProductsSummaryViewConfiguration()
        {
            this.HasKey(t => t.Id);
            this.ToTable("ProductsSummary");
        }
    }
}