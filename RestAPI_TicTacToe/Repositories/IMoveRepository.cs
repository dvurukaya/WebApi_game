using Microsoft.EntityFrameworkCore;
using RestAPI_TicTacToe.Data;
using RestAPI_TicTacToe.Configurations;
using RestAPI_TicTacToe.Models;

namespace RestAPI_TicTacToe.Repositories
{
    public interface IMoveRepository
    {
        Task<Move> GetMoveByIdAsync(int id);
        Task<List<Move>> GetAllMovesByGameIdAsync(int GameId);
        Task<Move> CreateAMoveAsync(Move move);
        Task UpdateAMoveAsync(Move move);
        Task DeleteAMoveAsync(int id);
    }
}
