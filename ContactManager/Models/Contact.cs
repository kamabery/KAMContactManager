using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using ContactManager.Attributes;

namespace ContactManager.Models
{
    public class Contact
    {
        public long Id { get; set; }

        public Guid UserId { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Email Name")]
        public string EmailAddress { get; set; }

        [DisplayName("Birth Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime BirthDate { get; set; }

        [DisplayName("Number of Computers")]
        public int NumberOfComputers { get; set; }

        [DisplayName("Addresses")]
        public ICollection<Address> Addresses { get; set; }
    }

    // In the Same File for Snippet Designer: https://marketplace.visualstudio.com/items?itemName=vs-publisher-2795.SnippetDesigner
    [Persistent]
    public class ContactConfiguration : EntityTypeConfiguration<Contact>
    {
        public ContactConfiguration()
        {
            ToTable("Contact");
            HasKey(p => p.Id);
            Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }

}