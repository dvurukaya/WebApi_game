using Microsoft.EntityFrameworkCore;
using RestAPI_TicTacToe.Data;
using RestAPI_TicTacToe.Configurations;
using RestAPI_TicTacToe.Models;

namespace RestAPI_TicTacToe.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly GameContext _gameContext;
        public PlayerRepository(GameContext gameContext)
        {
            _gameContext = gameContext;
        }

        public async Task<Player> GetPlayerByIdAsync(int id)
        {
            return await _gameContext.Players.FindAsync(id);
        }

        public async Task<List<Player>> GetAllPlayersAsync()
        {
            return await _gameContext.Players.ToListAsync();
        }
        public async Task<Player> GetPlayerByNameAsync(string name)
        {
            return await _gameContext.Players.SingleOrDefaultAsync(n => n.Name == name);          
        }

        public async Task<Player> CreatePlayerAsync(Player player)
        {
            await _gameContext.Players.AddAsync(player);
            await _gameContext.SaveChangesAsync();
            return player;
        }

        public async Task<Player> UpdatePlayerAsync(Player player)
        {
            _gameContext.Players.Update(player);
            await _gameContext.SaveChangesAsync();
            return player;
        }

        public async Task DeletePlayerAsync(int id)
        {
            var player = await GetPlayerByIdAsync(id);
            if(player != null)
            {
                _gameContext.Players.Remove(player);
                await _gameContext.SaveChangesAsync();
            }
        }
    }
}
