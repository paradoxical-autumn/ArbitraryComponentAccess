// no auto
using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Runtimes.Execution;
using ProtoFlux.Core;

namespace ArbitraryComponentAccess.ProtoFlux.Components;

public class AddComponentLogix<T> : ActionNode<FrooxEngineContext> where T : Component, new()
{
    public ObjectInput<Slot> slot;
    public readonly ObjectOutput<T> instantiatedComponent;
    public Continuation onAdded;
    public Continuation onFailed;

    public AddComponentLogix()
    {
        instantiatedComponent = new( this );
    }

    protected override IOperation Run( FrooxEngineContext context )
    {
        Slot? s = slot!.Evaluate( context );
        if ( s == null )
            return onFailed.Target;

        T ic;
        try
        {
            ic = s.AttachComponent<T>();
        }
        catch( Exception e )
        {
            UniLogPlugin.Log( e.ToString() );
            return onFailed.Target;
        }
        
        instantiatedComponent.Write( ic, context );

        return onAdded.Target;
    }
}