using FrameworkConsoleApp;
using GraphQL.Client;
using GraphQL.Common.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkClient
{
    class Program
    {
        private readonly GraphQLClient _client;
        static void Main(string[] args)
        {
            Program p = new Program();
            p.Run();
            Console.ReadLine();

        }

        public Program()
        {
            _client = new GraphQLClient(new GraphQLClientOptions() { EndPoint = new Uri("http://localhost:5000/graphql") });
        }

        public async void Run()
        {
            var allQueries = new GraphQLRequest
            {
                Query = @"
                    query playerQuery{
                        players
                        {
                            id,
                            name,
                            team
                        }
                    }                               
                "
            };

            var response = await _client.PostAsync(allQueries);
            List<Player> players = response.GetDataFieldAs<List<Player>>("players");

            Console.WriteLine(string.Join(",", players.Select(x => x.Name)));

            var query = new GraphQLRequest
            {
                Query = @" 
                    query teamPlayerQuery($team:String!){
                        teamPlayers(team: $team)
                        {
                            id,
                            name,
                            team
                        }
                    }
                ",
                Variables = new { team = "India" }
            };

            response = await _client.PostAsync(query);
            players = response.GetDataFieldAs<List<Player>>("teamPlayers");

            Console.WriteLine(string.Join(",",players.Select(x => x.Name)));
        }
    }
}
