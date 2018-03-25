using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Chidono.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Age { get; set; }
        public string Address { get; set; }
        public string Sex { get; set; }
        public virtual ObservableCollection<PostedImagesTable> PostedImageTable { get; set; }
        public virtual ObservableCollection<SchoolDetails> SchoolDetails { get; set; }
        public virtual ObservableCollection<NyscDetails> NyscDetails { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
    public class SchoolDetails
    {
        public SchoolDetails()
        {
            this.ApplicationUser = new ObservableCollection<Models.ApplicationUser>();
        }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Department { get; set; }
        public string Faculty { get; set; }
        public string University { get; set; }
        public virtual ObservableCollection<ApplicationUser> ApplicationUser { get; set; }
    }

    public class NyscDetails
    {
        public NyscDetails()
        {
            this.ApplicationUser = new ObservableCollection<Models.ApplicationUser>();
        }
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string StateChoice1 { get; set; }
        public string StateChoice2 { get; set; }
        public string StateChoice3 { get; set; }
        public string RegDate { get; set; }
        public string DeployDate { get; set; }
        public virtual ObservableCollection<ApplicationUser> ApplicationUser { get; set; }
    }
    public class PostedImagesTable
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public int ContentLenght { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public byte[] InputeData { get; set; }
        public virtual ObservableCollection<ApplicationUser> ApplicationUser { get; set; }
    }
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("ChidonoDB", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Migrations.Configuration>("ChidonoDB"));
        }
        public DbSet<SchoolDetails> SchoolDetails { get; set; }
        public DbSet<NyscDetails> NyscDetails { get; set; }
        public DbSet<PostedImagesTable> PostedImagesTable { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SchoolDetails>();
            modelBuilder.Entity<NyscDetails>();
            modelBuilder.Entity<PostedImagesTable>();
            base.OnModelCreating(modelBuilder);
        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}