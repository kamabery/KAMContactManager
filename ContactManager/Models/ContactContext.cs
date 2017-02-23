using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Threading.Tasks;
using ContactManager.Attributes;

namespace ContactManager.Models
{
    public interface IContactContext
    {
        DbSet<T> SetDb<T>()
            where T : class;

        Task<int> SaveChangesAsync();

        DbEntityEntry Entry(object entity);

        void Dispose();
    }

    public class ContactContext: DbContext, IContactContext
    {
        public DbSet<T> SetDb<T>()
            where T : class
        {
            return Set<T>();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ScanForConfiguration(modelBuilder);
        }

        private static void ScanForConfiguration(DbModelBuilder modelBuilder)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var typesToRegister =
                    assembly.GetTypes()
                        .Where(t => t.GetCustomAttributes(typeof(PersistentAttribute), inherit: true).Any())
                        .Where(
                            t =>
                            {
                                return t.BaseType != null && (t.BaseType.IsGenericType &&
                                                              (t.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>)));
                            });

                foreach (var type in typesToRegister)
                {
                    dynamic configurationInstance = Activator.CreateInstance(type);
                    modelBuilder.Configurations.Add(configurationInstance);
                }
            }
        }

        public DbSet<Contact> Contacts { get; set; }

        public System.Data.Entity.DbSet<ContactManager.Models.Address> Addresses { get; set; }
    }
}