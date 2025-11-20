using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace IAAITW.Models
{
    public partial class DBMdoelContext : DbContext
    {
        public DBMdoelContext()
            : base("name=DBMdoelContext")
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Knowledge> Knowledges { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<Refer> Refers { get; set; }
        public virtual DbSet<Survey> Surveys { get; set; }
        public virtual DbSet<MemberAccount> MemberAccounts { get; set; }
        public virtual DbSet<MemberInfo> MemberInfoes { get; set; }
        public virtual DbSet<MemberServiceExp> MemberServiceExps { get; set; }
        public virtual DbSet<About> Abouts { get; set; }
        public virtual DbSet<Expert> Experts { get; set; }
        public virtual DbSet<History> Histories { get; set; }
        public virtual DbSet<Organization> Organizationes { get; set; }
        public virtual DbSet<MemberDiscussionPost> MemberDiscussionPosts { get; set; }
        public virtual DbSet<MemberDiscussionReply> MemberDiscussionReplies { get; set; }
        public virtual DbSet<News> News { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           
        }
    }
}
