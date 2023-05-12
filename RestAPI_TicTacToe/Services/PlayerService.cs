using RestAPI_TicTacToe.Models;
using RestAPI_TicTacToe.Repositories;

namespace RestAPI_TicTacToe.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;
        public PlayerService(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;

        }
        public async Task<Player> GetPlayerByIdAsync(int id)
        {
            return await _playerRepository.GetPlayerByIdAsync(id);
        }

        public async Task<List<Player>> GetAllPlayersAsync()
        {
            return await _playerRepository.GetAllPlayersAsync();
        }

        public async Task<Player> CreatePlayerAsync(string name)
        {
            var player = new Player
            {
                Name = name,
            };
            return await _playerRepository.CreatePlayerAsync(player);
        }

        public async Task<Player> UpdatePlayerAsync(Player player)
        {
            return await _playerRepository.UpdatePlayerAsync(player);
        }

        public async Task DeletePlayerAsync(int id)
        {
            await _playerRepository.DeletePlayerAsync(id);
        }
    }
}

