using RestAPI_TicTacToe.Configurations;
using RestAPI_TicTacToe.Models;

namespace RestAPI_TicTacToe.Repositories
{
    public interface IPlayerRepository
    {
        Task<Player> GetPlayerByIdAsync(int id);
        Task<List<Player>> GetAllPlayersAsync();
        Task<Player> GetPlayerByNameAsync(string name);
        Task<Player> CreatePlayerAsync(Player player);
        Task<Player> UpdatePlayerAsync(Player player);
        Task DeletePlayerAsync(int id);
    }
}
