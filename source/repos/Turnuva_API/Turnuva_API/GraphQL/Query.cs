using HotChocolate;
using HotChocolate.Data;
using TournamentAPI.Data;
using TournamentAPI.Models;

namespace TournamentAPI.GraphQL
{
    public class Query
    {
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Tournament> GetTournaments([Service] AppDbContext context) => context.Tournaments;

        [UseProjection]
        [UseFiltering]
        public IQueryable<Match> GetMatches([Service] AppDbContext context) => context.Matches;

        public IQueryable<User> GetUsers([Service] AppDbContext context) => context.Users;

        public IQueryable<Match> GetMatchesForRound(int round, [Service] AppDbContext context)
        {
            return context.Matches.Where(m => m.Round == round);
        }
    }
}