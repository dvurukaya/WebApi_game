using RestAPI_TicTacToe.Models;

namespace RestAPI_TicTacToe.Services
{
    public interface IPlayerService
    {
        Task<Player> GetPlayerByIdAsync(int id);
        Task<List<Player>> GetAllPlayersAsync();
        Task<Player> CreatePlayerAsync(string name);
        Task<Player> UpdatePlayerAsync(Player player);
        Task DeletePlayerAsync(int id);
    }
}
