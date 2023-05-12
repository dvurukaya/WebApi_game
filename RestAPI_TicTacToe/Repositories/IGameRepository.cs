using RestAPI_TicTacToe.Models;
using RestAPI_TicTacToe.StaticInfo;

namespace RestAPI_TicTacToe.Repositories
{
    public interface IGameRepository
    {
        Task<List<Game>> GetAllGamesAsync();
        Task<Game> GetGameByIdAsync(int id);
        Task<Game> GetGameByCodeAsync(Guid code);
        Task<List<Game>> GetGamesByDateAsync(DateTime date);
        Task<Game> CreateGameWithFirstPlayerAsync(Game game);
        Task<Game> EntryGameWithSecondPlayerAsync(Guid code, GameStatus status);
        Task<Game> UpdateGameAsync(Game game);
        Task DeleteGameByIdAsync(int id);
    }
}
