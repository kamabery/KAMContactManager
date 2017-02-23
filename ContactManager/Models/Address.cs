using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ContactManager.Models
{
    public class Address
    {
        public long Id { get; set; }

        public long ContactId { get; set; }

        public virtual Contact Contact { get; set; }

        [DisplayName("Address Type")]
        public string AddressType { get; set; }

        [DisplayName("Address Line 1")]
        public string AddressLine1 { get; set; }

        [DisplayName("Address Line 2")]
        public string AddressLine2 { get; set; }

        [DisplayName("City")]
        public string City { get; set; }

        [DisplayName("State")]
        public string StateCode { get; set; }
    }

    public class AddressConfiguration : EntityTypeConfiguration<Address>
    {
        public AddressConfiguration()
        {
            ToTable("Address");
            HasKey(p => p.Id);
            Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            HasRequired(p => p.Contact).WithMany(p => p.Addresses).HasForeignKey(p => p.ContactId);
        }
    }
}