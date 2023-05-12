using Microsoft.EntityFrameworkCore;
using RestAPI_TicTacToe.Data;
using RestAPI_TicTacToe.Configurations;
using RestAPI_TicTacToe.Models;
using System.Linq;
using RestAPI_TicTacToe.StaticInfo;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace RestAPI_TicTacToe.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly GameContext _gameContext;
        public GameRepository(GameContext gameContext)
        {
            _gameContext = gameContext; 
        }

        public async Task<List<Game>> GetAllGamesAsync()
        {
            return await _gameContext.Set<Game>().ToListAsync();
        }

        public async Task<Game> GetGameByIdAsync(int id)
        {
            return await _gameContext.Set<Game>().FindAsync(id);
        }

        public async Task<List<Game>> GetGamesByDateAsync(DateTime date)
        {
            var games = await _gameContext.Games.Where(g => g.Date == date).ToListAsync();
            return games; 
        }

        public async Task<Game> GetGameByCodeAsync(Guid Code)
        {
            var game = await _gameContext.Games.FirstOrDefaultAsync(c => c.CodeGame == Code);
            return await _gameContext.Set<Game>().FindAsync(game.Id);
        }

        public async Task<Game> CreateGameWithFirstPlayerAsync(Game game)
        {
            await _gameContext.Set<Game>().AddAsync(game);
            await _gameContext.SaveChangesAsync();
            return game;
        }
        public async Task<Game> EntryGameWithSecondPlayerAsync(Guid code,GameStatus status)
        {
            var game = await GetGameByCodeAsync(code);
            _gameContext.Entry(game).State = EntityState.Modified;
            await _gameContext.SaveChangesAsync();
            return game;
        }

        public async Task<Game> UpdateGameAsync(Game game)
        {
            _gameContext.Entry(game).State = EntityState.Modified;
            await _gameContext.SaveChangesAsync();
            return game;
        }

        public async Task DeleteGameByIdAsync(int id)
        {
            var game = await GetGameByIdAsync(id);
            if (game != null)
            {
                _gameContext.Games.Remove(game);
                await _gameContext.SaveChangesAsync();
            }
        }
    }
}
