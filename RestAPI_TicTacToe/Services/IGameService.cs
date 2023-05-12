using RestAPI_TicTacToe.Models;
using RestAPI_TicTacToe.StaticInfo;

namespace RestAPI_TicTacToe.Services
{
    public interface IGameService
    {
        Task<List<Game>> GetAllGamesAsync();
        Task<Game> GetGameByIdAsync(int id);
        Task<Game> GetGameByCodeAsync(Guid code);
        Task<List<Game>> GetGamesByDateAsync(DateTime date);
        Task<List<Game>> GetGamesByPlayersNames(string FirstName, string SecondName);
        Task<Game> CreateGameAsync(int firstPlayerId, int secondPlayerId);
        Task<Game> EntryGameWithSecondPlayerAsync(Guid code);
        Task<Game> CreateMoveAsync(int PlayerId, int gameId, int cell);
        Task<Game> UpdateGameAsync(Game game);
        Task DeleteGameAsync(int id);
    }
}
