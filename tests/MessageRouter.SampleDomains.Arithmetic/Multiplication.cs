﻿using MessageRouter.Common;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MessageRouter.SampleDomains.Arithmetic.Multiplication
{
  public struct MultiplyCommand : ICommand
  {
    public readonly Int32 Multiplicand;
    public readonly Int32 Multiplier;

    public MultiplyCommand (Int32 multiplicand, Int32 multiplier)
    {
      this.Multiplicand = multiplicand;
      this.Multiplier = multiplier;
    }

    public override string ToString ()
    {
      return String.Format("MultiplyCommand ({0} * {1})", Multiplicand, Multiplier);
    }
  }

  public struct MultipliedEvent : IEvent
  {
    public readonly Int32 Multiplicand;
    public readonly Int32 Multiplier;
    public readonly Int32 Product;

    public MultipliedEvent (Int32 multiplicand, Int32 multiplier, Int32 product)
    {
      this.Multiplicand = multiplicand;
      this.Multiplier = multiplier;
      this.Product = product;
    }

    public override string ToString ()
    {
      return String.Format("MultipliedEvent ({0} * {1} = {2})", Multiplicand, Multiplier, Product);
    }
  }

  public sealed class MultiplyCommandHandler : IHandleCommand<MultiplyCommand>
  {
    public Task Handle (MultiplyCommand command, CancellationToken shutdown)
    {
      return Task.Factory.StartNew(() => {
        Console.WriteLine("[{0}] {1}", DateTime.Now.ToString("O"), command);
      }
      ,shutdown);
    }
  }

  public sealed class InfixMultipliedEventHandler : IHandleEvent<MultipliedEvent>
  {
    private readonly IDictionary<String,String> store;

    public InfixMultipliedEventHandler (IDictionary<String,String> store)
    {
      this.store = store;
    }

    public Task Handle (MultipliedEvent @event, CancellationToken shutdown)
    {
      return Task.Factory.StartNew(() => {
        var key = String.Format("{0:X}_Infix", @event.GetHashCode());
        Thread.Sleep(250); // simulate work!
        store[key] = String.Format("{2} : ({0} * {1})"
                                  ,@event.Multiplicand
                                  ,@event.Multiplier
                                  ,@event.Product);
      }
      ,shutdown);
    }
  }

  public sealed class PrefixMultipliedEventHandler : IHandleEvent<MultipliedEvent>
  {
    private readonly IDictionary<String,String> store;

    public PrefixMultipliedEventHandler (IDictionary<String,String> store)
    {
      this.store = store;
    }

    public Task Handle (MultipliedEvent @event, CancellationToken shutdown)
    {
      return Task.Factory.StartNew(() => {
        var key = String.Format("{0:X}_Prefix", @event.GetHashCode());
        store[key] = String.Format ("{2} : (* {0} {1})"
                                   ,@event.Multiplicand
                                   ,@event.Multiplier
                                   ,@event.Product);
      }
      ,shutdown);
    }
  }

  public sealed class PostfixMultipliedEventHandler : IHandleEvent<MultipliedEvent>
  {
    private readonly IDictionary<String,String> store;

    public PostfixMultipliedEventHandler (IDictionary<String,String> store)
    {
      this.store = store;
    }

    public Task Handle (MultipliedEvent @event, CancellationToken shutdown)
    {
      return Task.Factory.StartNew(() => {
        Thread.Sleep(150); // simulate work!
        var key = String.Format("{0:X}_Postfix", @event.GetHashCode());
        store[key] = String.Format("{2} : ({0} {1} *)"
                                  ,@event.Multiplicand
                                  ,@event.Multiplier
                                  ,@event.Product); 
      }
      ,shutdown);
    }
  }
}
