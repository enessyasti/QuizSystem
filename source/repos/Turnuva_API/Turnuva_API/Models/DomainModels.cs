namespace TournamentAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class Tournament
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public string Status { get; set; } = "Pending";

        public List<User> Participants { get; set; } = new();

        public int? BracketId { get; set; }
        public Bracket? Bracket { get; set; }
    }

    public class Bracket
    {
        public int Id { get; set; }
        public List<Match> Matches { get; set; } = new();
    }

    public class Match
    {
        public int Id { get; set; }
        public int Round { get; set; }

        public int? Player1Id { get; set; }
        public User? Player1 { get; set; }

        public int? Player2Id { get; set; }
        public User? Player2 { get; set; }

        public int? WinnerId { get; set; }
        public User? Winner { get; set; }
    }
}