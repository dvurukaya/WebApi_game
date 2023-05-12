using Microsoft.EntityFrameworkCore;
using RestAPI_TicTacToe.Data;
using RestAPI_TicTacToe.Configurations;
using RestAPI_TicTacToe.Models;

namespace RestAPI_TicTacToe.Repositories
{
    public class MoveRepository : IMoveRepository
    {
        private readonly GameContext _gameContext;
        public MoveRepository(GameContext gameContext)
        {
            _gameContext = gameContext;
        }

        public async Task<Move> GetMoveByIdAsync(int id)
        {
            return await _gameContext.Moves.FindAsync(id);
        }

        public async Task<List<Move>> GetAllMovesByGameIdAsync(int GameId)
        {
            return await _gameContext.Moves.Where(g => g.GameId == GameId).ToListAsync();
        }

        public async Task<Move> CreateAMoveAsync(Move move)
        {
            await _gameContext.Moves.AddAsync(move);
            await _gameContext.SaveChangesAsync();
            return move;
        }

        public async Task UpdateAMoveAsync(Move move)
        {
            _gameContext.Moves.Update(move);
            await _gameContext.SaveChangesAsync();
        }

        public async Task DeleteAMoveAsync(int id)
        {
            var move = await GetMoveByIdAsync(id);
            if(move != null)
            {
                _gameContext.Moves.Remove(move);
                await _gameContext.SaveChangesAsync();
            }
        }
    }
}
