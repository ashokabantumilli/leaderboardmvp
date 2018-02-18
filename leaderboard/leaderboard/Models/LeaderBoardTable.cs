namespace leaderboard.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LeaderBoardTable")]
    public partial class LeaderBoardTable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string CompeteName { get; set; }

        [Required]
        [StringLength(50)]
        public string H2HContestName { get; set; }

        public int Points { get; set; }

        [StringLength(50)]
        public string Referee { get; set; }
    }
}
