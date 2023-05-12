using Microsoft.EntityFrameworkCore;
using RestAPI_TicTacToe.Data;
using RestAPI_TicTacToe.Models;
using RestAPI_TicTacToe.Repositories;
using RestAPI_TicTacToe.StaticInfo;

namespace RestAPI_TicTacToe_Tests.Repositories
{
    public class MoveRepositoryTests
    {
        private readonly DbContextOptions<GameContext> _options;
        private readonly GameContext _dbContext;

        public MoveRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<GameContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new GameContext(_options);
        }

        [Fact]
        public async Task NotExistingMove()
        {
            //Arrange
            var moveRepository = new MoveRepository(_dbContext);

            //Act
            var move = await moveRepository.GetMoveByIdAsync(3);

            //Assert
            Assert.Null(move);
        }

        [Fact]
        public async Task NotExistingMoves()
        {
            //Arrange
            var moveRepository = new MoveRepository(_dbContext);

            //Act
            var moves = await moveRepository.GetAllMovesByGameIdAsync(2);

            //Assert
            Assert.Empty(moves);
        }

        [Fact]
        public async Task CreateMove()
        {
            //Arrange
            var moveRepository = new MoveRepository(_dbContext);

            var move = new Move
            {
                GameId = 1,
                PlayerId = 2,
                Element = Elements.O,
                Cell = 5
            };

            //Act
            var newMove = await moveRepository.CreateAMoveAsync(move);

            //Assert
            Assert.NotNull(newMove);
            Assert.Equal(move.GameId, newMove.GameId);
            Assert.Equal(move.PlayerId, newMove.PlayerId);
            Assert.Equal(move.Element, newMove.Element);
            Assert.Equal(move.Cell, newMove.Cell);
        }

        [Fact]
        public async Task UpdateMove()
        {
            //Arrange
            var moveRepository = new MoveRepository(_dbContext);

            var move = new Move
            {
                GameId = 1,
                PlayerId = 2,
                Element = Elements.X,
                Cell = 3
            };
            await moveRepository.CreateAMoveAsync(move);

            move.Element = Elements.O;
            move.Cell = 8;

            //Act
            await moveRepository.UpdateAMoveAsync(move);

            var updated = await moveRepository.GetMoveByIdAsync(move.GameId);

            //Assert
            Assert.NotNull(updated);
            Assert.Equal(move.GameId, updated.GameId);
            Assert.Equal(move.PlayerId, updated.PlayerId);
            Assert.Equal(move.Element, updated.Element);
            Assert.Equal(move.Cell, updated.Cell);
        }

        [Fact]
        public async Task DeleteMove()
        {
            //Arrange
            var moveRepository = new MoveRepository(_dbContext);

            var move = new Move
            {
                GameId = 1,
                PlayerId = 2,
                Element = Elements.X,
                Cell = 3
            };
            await moveRepository.CreateAMoveAsync(move);

            //Act
            await moveRepository.DeleteAMoveAsync(move.Id);

            var deleted = await moveRepository.GetMoveByIdAsync(move.Id);

            //Assert
            Assert.Null(deleted);
        }
    }
}
