﻿namespace MessageRouter.SDK

open System
open System.Threading.Tasks

[<AbstractClass>]
type MessageRouterBase () =
  abstract Route : message:obj * onSuccess:Action * onFailure:Action<obj,exn> -> unit
  interface IMessageRouter with
    member R.Route message onSuccess onFailure = R.Route (message,Action onSuccess, Action<_,_> onFailure)

[<AbstractClass>]
type CommandHandlerBase<'command when 'command :> ICommand> () =
  abstract Handle : command:'command * onSuccess:Action -> Task
  interface IHandleCommand<'command> with
    member H.Handle command onSuccess = H.Handle (command,Action onSuccess)

[<AbstractClass>]
type EventHandlerBase<'event when 'event :> IEvent> () =
  abstract Handle : event:'event * onSuccess:Action -> Task
  interface IHandleEvent<'event> with
    member H.Handle event onSuccess = H.Handle (event,Action onSuccess)
    