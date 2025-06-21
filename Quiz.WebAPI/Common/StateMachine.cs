using Quiz.DataAccessLayer.Interfaces;
using Quiz.DataAccessLayer.Models;
using Quiz.WebAPI.Interfaces;

namespace Quiz.WebAPI.Common;

/// <summary>
    /// Машина состояний викторины
    /// </summary>
    public class QuizStateMachine(IEnumerable<IQuizState> states,
                                    IQuizStateRepository repository)
    {
        private readonly Dictionary<QuizStateEnum, IQuizState> _states = states.ToDictionary(s => s.State);
        private IQuizState? _currentState;

        public QuizStateEnum Current => _currentState?.State
                                        ?? throw new InvalidOperationException("State machine not initialized");

        /// <summary>
        /// Инициализация из БД. Вызывать при старте приложения.
        /// </summary>
        public async Task InitializeAsync()
        {
            var saved = await repository.GetActiveStateAsync();
            if (saved.IsSuccess(out var dto))
            {
                if (!_states.TryGetValue(dto.State, out _currentState!))
                    throw new InvalidOperationException($"State {saved} not found in registered states");
            }
            else
            {
                throw new InvalidOperationException("State machine not initialized");
            }

            await _currentState.OnEnterAsync(dto.QuizQuestionId);
        }

        /// <summary>
        /// Переход в новое состояние
        /// </summary>
        public async Task ChangeStateAsync(QuizStateEnum next)
        {
            var activeState = await repository.GetActiveStateAsync();

            if (!activeState.IsSuccess(out var dto))
                throw new InvalidOperationException($"State machine not initialized");

            if (!_states.TryGetValue(next, out IQuizState? value))
                throw new InvalidOperationException($"State {next} not registered.");

            await _currentState?.OnExitAsync()!;
            _currentState = value;
            await _currentState.OnEnterAsync(dto.QuizQuestionId);
        }
    }
