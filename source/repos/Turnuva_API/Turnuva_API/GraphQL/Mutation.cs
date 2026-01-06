using HotChocolate;
using Microsoft.EntityFrameworkCore;
using TournamentAPI.Data;
using TournamentAPI.Models;

namespace TournamentAPI.GraphQL
{
    public class Mutation
    {
        public async Task<User> Register(string email, string password, string firstName, string lastName, [Service] AppDbContext context)
        {
            var user = new User { Email = email, Password = password, FirstName = firstName, LastName = lastName };
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user;
        }

        public string Login(string email, string password, [Service] AppDbContext context)
        {
            var user = context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user == null) throw new Exception("Invalid credentials");
            return $"token_for_{user.Id}_{user.Email}";
        }

        public async Task<Tournament> CreateTournament(string name, [Service] AppDbContext context)
        {
            var tournament = new Tournament { Name = name, StartDate = DateTime.Now, Status = "Pending" };
            context.Tournaments.Add(tournament);
            await context.SaveChangesAsync();
            return tournament;
        }

        public async Task<bool> AddParticipant(int tournamentId, int userId, [Service] AppDbContext context)
        {
            var tournament = await context.Tournaments.Include(t => t.Participants).FirstOrDefaultAsync(t => t.Id == tournamentId);
            var user = await context.Users.FindAsync(userId);

            if (tournament == null || user == null) return false;

            tournament.Participants.Add(user);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Tournament> StartTournament(int tournamentId, [Service] AppDbContext context)
        {
            var tournament = await context.Tournaments
                .Include(t => t.Participants)
                .FirstOrDefaultAsync(t => t.Id == tournamentId);

            if (tournament == null || tournament.Participants.Count < 2) throw new Exception("Not enough participants");

            var bracket = new Bracket();
            var match = new Match
            {
                Round = 1,
                Player1 = tournament.Participants[0],
                Player2 = tournament.Participants[1]
            };

            bracket.Matches.Add(match);
            context.Brackets.Add(bracket);

            tournament.Bracket = bracket;
            tournament.Status = "Active";

            await context.SaveChangesAsync();
            return tournament;
        }

        public async Task<Tournament> FinishTournament(int tournamentId, [Service] AppDbContext context)
        {
            var tournament = await context.Tournaments.FindAsync(tournamentId);
            if (tournament == null) throw new Exception("Tournament not found");

            tournament.Status = "Finished";
            await context.SaveChangesAsync();
            return tournament;
        }

        public async Task<Match> PlayMatch(int matchId, int winnerId, [Service] AppDbContext context)
        {
            var match = await context.Matches.FindAsync(matchId);
            if (match == null) throw new Exception("Match not found");

            var winner = await context.Users.FindAsync(winnerId);
            match.Winner = winner;

            await context.SaveChangesAsync();
            return match;
        }
    }
}