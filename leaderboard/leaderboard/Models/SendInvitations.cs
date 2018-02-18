namespace leaderboard.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SendInvitations : DbContext
    {
        public SendInvitations()
            : base("name=SendInvitations")
        {
        }

        public virtual DbSet<SendInvitation> SendInvitationS { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
