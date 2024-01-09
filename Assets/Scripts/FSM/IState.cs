namespace FSM
{
    public interface IState<T>
    {
        /// <summary>
        /// code that runs when we first enter the state
        /// </summary>
        /// <param name="entity"></param>
        public void Enter(T entity)
        {
        }

        /// <summary>
        /// per-frame logic, include condition to transition to a new state
        /// </summary>
        /// <param name="entity"></param>
        public void Execute(T entity)
        {
        }

        /// <summary>
        ///  code that runs when we exit the state
        /// </summary>
        /// <param name="entity"></param>
        public void Exit(T entity)
        {
        }
    }
}