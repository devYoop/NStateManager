[![Build status](https://ci.appveyor.com/api/projects/status/byg4n228cinno4xt?svg=true)](https://ci.appveyor.com/project/ScottCarter/nstatemanager) [![NuGet Status](https://img.shields.io/nuget/v/NStateManager.svg)](https://www.nuget.org/packages/NStateManager) ![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=NStateManager&metric=security_rating) ![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=NStateManager&metric=reliability_rating) ![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=NStateManager&metric=sqale_rating)
# Quick Example
![POSv1](https://github.com/scottctr/NStateManager/blob/master/Examples/NStateManager.Example.Sale.Console/POSv2.png)

```C#
//State machine to manage sales for a point of sale system
stateMachine = new StateMachine<Sale, SaleState, SaleEvent>(
  stateAccessor: (sale) => sale.State,
  stateMutator: (sale, state) => sale.State = state);

//Log each time a sale changes state regardless of to/from state
stateMachine.RegisterOnTransitionedAction((sale, transitionDetails) 
  => Output.WriteLine($"Sale {sale.SaleID} transitioned from {transitionDetails.PreviousState} to {transitionDetails.CurrentState}."));

//Configure the Open state
stateMachine.ConfigureState(SaleState.Open)
  //Process the new item on the AddItem event 
  .AddTriggerAction<SaleItem>(SaleEvent.AddItem, (sale, saleItem) => { sale.AddItem(saleItem); })
  //Process the payment on the Pay event
  .AddTriggerAction<Payment>(SaleEvent.Pay, (sale, payment) => { sale.AddPayment(payment); })
  //Transition to the ChangeDue state on Pay event if customer over pays
  .AddTransition(SaleEvent.Pay, SaleState.ChangeDue, condition: sale => sale.Balance < 0, name: "Open2ChangeDue", priority: 1)
  //Transition to the Completed state on Pay event if customer pays exact amount
  .AddTransition(SaleEvent.Pay, SaleState.Complete, condition: sale => sale.Balance == 0, name: "Open2Complete", priority: 2);

//Configure the ChangeDue state
stateMachine.ConfigureState(SaleState.ChangeDue)
  //Process the returned change on the ChangeGiven event
  .AddTriggerAction<Payment>(SaleEvent.ChangeGiven, (sale, payment) => { sale.ReturnChange(); })
  //Transition to the Complete state on ChangeGiven -- no conditions
  .AddTransition(SaleEvent.ChangeGiven, SaleState.Complete);

//No configuration required for Complete state since it's a final state and
//transitions or actions are taken at this point
```

# Background and Goals  
This project was created while developing cloud-based solutions. We needed a state management solution made for our stateless services.  
- **Stateless** so it's thread safe and a single state machine can be used to manage state for all managed instances  
- **Intuitive configuration** of [state change conditions](https://github.com/scottctr/NStateManager/wiki/Changing-States), [behaviors based on current state](https://github.com/scottctr/NStateManager/wiki/Event-Actions), and [actions when changing states](https://github.com/scottctr/NStateManager/wiki/State-Change-Actions)  
- **[async/await support](https://github.com/scottctr/NStateManager/wiki/Async-Await-Support)**, including cancellation and ConfigureAwait       
  
I would appreciate feedback, questions, advice, and contributions. I'm a big believer that WE are smarter than ME and we can work together to create something that will add value to virtually any .Net application.

## Try it out
If you're ready to jump in, go to the [Quick Start](https://github.com/scottctr/NStateManager/wiki/Quick-Start).

Please take a look around and post questions, suggestions, and advice to [Issues](https://github.com/scottctr/NStateManager/issues). Also let me know if you're interested in contributing.
