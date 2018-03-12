﻿#region Copyright (c) 2018 Scott L. Carter
//
//Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in
//compliance with the License. You may obtain a copy of the License at
//http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software distributed under the License is 
//distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and limitations under the License.
#endregion
using System;
using System.Threading.Tasks;

namespace NStateManager
{
    public abstract class StateTransitionBase<T, TState, TTrigger>
    {
        public static event Action<T, StateTransitionResult<TState, TTrigger>> OnTransitionedEvent;

        protected string Name;
        protected internal uint Priority;
        protected Func<T, TState> StateAccessor { get; }
        protected Action<T, TState> StateMutator { get; }

        public TState ToState { get; protected set; }

        protected StateTransitionBase(Func<T, TState> stateAccessor
            , Action<T, TState> stateMutator
            , TState toState
            , string name
            , uint priority)
        {
            StateAccessor = stateAccessor ?? throw new ArgumentNullException(nameof(stateAccessor));
            StateMutator = stateMutator ?? throw new ArgumentNullException(nameof(stateMutator));
            ToState = toState;
            Name = name ?? "<not set>";
            Priority = priority;
        }

        public virtual StateTransitionResult<TState, TTrigger> Execute(ExecutionParameters<T, TTrigger> parameters
          , StateTransitionResult<TState, TTrigger> currentResult = null)
        {
            throw new NotImplementedException("Inheritted classes must override this method. Ensure you're calling the correct overloaded version.");
        }

        public virtual Task<StateTransitionResult<TState, TTrigger>> ExecuteAsync(ExecutionParameters<T, TTrigger> parameters
          , StateTransitionResult<TState, TTrigger> currentResult = null)
        {
            throw new NotImplementedException("Inheritted classes must override this method. Ensure you're calling the correct overloaded version.");
        }

        protected void NotifyOfTransition(T context, StateTransitionResult<TState, TTrigger> transitionResult)
        {
            OnTransitionedEvent?.Invoke(context, transitionResult);
        }
    }
}