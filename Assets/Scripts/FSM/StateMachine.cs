using System;

namespace FSM
{
    [Serializable]
    public class StateMachine<T>
    {
        //a pointer to the agent that owns this instance
        private T owner;

        public IState<T> CurrentState { get; private set; }

        //a record of the last state the agent was in
        public IState<T> PreviousState { get; private set; }

        //this is called every time the FSM is updated
        public IState<T> GlobalState { get; private set; }


        public StateMachine(T owner)
        {
            this.owner = owner;
            CurrentState = null;
            PreviousState = null;
            GlobalState = null;
        }

        //use these methods to initialize the FSM
        public void SetCurrentState(IState<T> s)
        {
            CurrentState = s;
        }

        public void SetGlobalState(IState<T> s)
        {
            GlobalState = s;
        }

        public void SetPreviousState(IState<T> s)
        {
            PreviousState = s;
        }

        /// <summary>
        /// Call this to update the FSM
        /// </summary>
        public void Execute()
        {
            //if a global state exists, call its execute method, else do nothing
            if (GlobalState != null)
            {
                GlobalState.Execute(owner);
            }

            //same for the current state
            if (CurrentState != null)
            {
                CurrentState.Execute(owner);
            }
        }

        /// <summary>
        /// Change to a new state
        /// Does not change state if it is in the same state
        /// </summary>
        /// <param name="newState"></param>
        public void ChangeState(IState<T> newState)
        {
            if (newState == null)
            {
                return;
            }

            bool isSameState = CurrentState == newState;
            if (isSameState)
            {
                return;
            }

            //keep a record of the previous state
            PreviousState = CurrentState;

            //call the exit method of the existing state
            if (CurrentState != null)
            {
                CurrentState.Exit(owner);
            }

            //change state to the new state
            CurrentState = newState;

            //call the entry method of the new state
            CurrentState.Enter(owner);
        }

        /// <summary>
        /// Change state back to the previous state
        /// </summary>
        public void RevertToPreviousState()
        {
            ChangeState(PreviousState);
        }

        /// <summary>
        /// Returns true if the current state's type is equal to the type of the
        /// class passed as a parameter. 
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool IsInState(IState<T> state)
        {
            return System.Object.ReferenceEquals(CurrentState.GetType(), state.GetType());
        }
    }
}