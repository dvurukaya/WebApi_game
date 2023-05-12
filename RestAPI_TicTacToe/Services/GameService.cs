using Microsoft.AspNetCore.Components.Web;
using RestAPI_TicTacToe.Data;
using RestAPI_TicTacToe.Models;
using RestAPI_TicTacToe.Repositories;
using RestAPI_TicTacToe.StaticInfo;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RestAPI_TicTacToe.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IMoveRepository _moveRepository;
        private readonly IPlayerRepository _playerRepository;

        public GameService(IGameRepository gameRepository, IMoveRepository moveRepository, IPlayerRepository playerRepository)
        {
            _gameRepository = gameRepository;
            _moveRepository = moveRepository;
            _playerRepository = playerRepository;
        }

        public async Task<List<Game>> GetAllGamesAsync()
        {
            return await _gameRepository.GetAllGamesAsync();
        }

        public async Task<Game> GetGameByIdAsync(int id)
        {
            return await _gameRepository.GetGameByIdAsync(id);
        }

        public async Task<Game> GetGameByCodeAsync(Guid code)
        {
            return await _gameRepository.GetGameByCodeAsync(code);
        }

        public async Task<List<Game>> GetGamesByDateAsync(DateTime date)
        {
            return await _gameRepository.GetGamesByDateAsync(date);           
        }

        public async Task<List<Game>> GetGamesByPlayersNames(string FirstName, string SecondName)
        {
            var first = await _playerRepository.GetPlayerByNameAsync(FirstName);
            var second = await _playerRepository.GetPlayerByNameAsync(SecondName);

            var games = await _gameRepository.GetAllGamesAsync();
            var request = games.Where(f => f.FirstPlayerId == first.Id)
                               .Where(s => s.SecondPlayerId == second.Id)
                               .ToList();

            return request;
        }

        public async Task<Game> CreateGameAsync(int firstPlayerId, int secondPlayerId)
        {
            var game = new Game
            {
                Board = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                FirstPlayerId = firstPlayerId,
                SecondPlayerId = secondPlayerId,
                Draw = false,
                Status = GameStatus.Registered,
                Date = DateTime.Now,
                CodeGame = Guid.NewGuid(),
            };

            return await _gameRepository.CreateGameWithFirstPlayerAsync(game);
        }

        public async Task<Game> EntryGameWithSecondPlayerAsync(Guid code)
        {
            var game = await _gameRepository.GetGameByCodeAsync(code);
            if(game == null)
            {
                throw new ApplicationException($"Game with code {code} wasn't found. Try again.");
            }
            game.Status = GameStatus.Started;
            GameStatus status = game.Status;
            return await _gameRepository.EntryGameWithSecondPlayerAsync(code,status);
        }

        public async Task<Game> UpdateGameAsync(Game game)
        {
            return await _gameRepository.UpdateGameAsync(game);
        }

        public async Task<Game> CreateMoveAsync(int PlayerId, int gameId, int cell)
        {
            var game = await _gameRepository.GetGameByIdAsync(gameId);
            if(game == null)
            {
                throw new ApplicationException($"Game with id {gameId} wasn't found. Try again.");
            }
        
            var checkMoves = await _moveRepository.GetAllMovesByGameIdAsync(gameId);
            if(checkMoves.Any(move => move.Cell == cell))
            {
                throw new ApplicationException($"Try again.The cell you requested [{cell}] is already used");
            }

            var element = game.Status == GameStatus.FirstPlayerTurn ? StaticInfo.Elements.X : StaticInfo.Elements.O;
            var move = new Move()
            {
                GameId = gameId,
                PlayerId = PlayerId,
                Cell = cell,
                Element = element
            };

            var board = UpdateBoard(game.Board, element, cell);
            game.Board = board;

            var checkDraw = IsDrawOrNot(board);
            var result = CheckGameBoard(board);

            if(checkDraw == GameResult.Draw)
            {
                game.Draw = true;
                game.Status = GameStatus.GameOver;
            }

            switch(result)
            {
                case GameResult.FirstPlayerIsWinner:
                    game.WinnerId = game.FirstPlayerId;
                    game.Status = GameStatus.GameOver;
                    break;
                case GameResult.SecondPlayerIsWinner:
                    game.WinnerId = game.SecondPlayerId;
                    game.Status = GameStatus.GameOver;
                    break;
            }

            if(game.Status != GameStatus.GameOver)
            {
                game.Status = game.Status == GameStatus.FirstPlayerTurn ? GameStatus.FirstPlayerTurn :
                                                                          GameStatus.SecondPlayerTurn;
            }

            await _moveRepository.CreateAMoveAsync(move);
            await _gameRepository.UpdateGameAsync(game);

            return game;
        }

        public async Task DeleteGameAsync(int id)
        {
            await _gameRepository.DeleteGameByIdAsync(id);
        }

        private int[] UpdateBoard(int[] board, Elements element, int cell)
        {
            if(board != null)
            {
                board[cell] = (int)element;
                return board;
            }
            else throw new ApplicationException("Error, board doesn't exist.");
        }

        private GameResult IsDrawOrNot(int[] board)
        {
            var result = GameResult.Continue;
            if (board != null)
            {
                var amount = 0;
                for (int i = 0; i < board.Length; i++)
                {
                    if (board[i] == 0)
                    {
                        amount++;
                    }
                }
                if (amount >= 7)
                {
                    return GameResult.Draw;
                }
            }
            return result;
        }

        private GameResult CheckGameBoard(int[] board)
        {
            var result = GameResult.Continue;
            if(board != null)
            {
                var winningCombinations = new List<int[]>
                {
                    //Horizontal lines of winning
                    new int[] {0,1,2},
                    new int[] {3,4,5},
                    new int[] {6,7,8},
                    //Vertical lines of winning
                    new int[] {0,3,6},
                    new int[] {1,4,7},
                    new int[] {2,5,8},
                    //Diagonal lines of winning
                    new int[] {0,4,8},
                    new int[] {2,4,6}
                };

                int amount_X = 0;
                int amount_O = 0;
                foreach (int[] l in winningCombinations)
                {
                    if (amount_X == 3 || amount_O == 3)
                    {
                        break;
                    }
                    else if (amount_X < 3 || amount_O < 3)
                    {
                        amount_X = 0;
                        amount_O = 0;
                    }
                    for (int i = 0; i < l.Count(); i++)
                    {
                        var current = l[i];
                        if (board[current] == 1)
                        {
                            amount_X++;
                        }
                        else if (board[current] == 2)
                        {
                            amount_O++;
                        }
                    }
                }

                if(amount_X == 3)
                {
                    return GameResult.FirstPlayerIsWinner;
                }

                else if(amount_O == 3)
                {
                    return GameResult.SecondPlayerIsWinner;
                }
            }
            return result;
        }
    }
}
