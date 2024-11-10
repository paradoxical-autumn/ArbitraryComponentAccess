using FrooxEngine;
using ProtoFlux.Core;
using FrooxEngine.ProtoFlux;
using FrooxEngine.ProtoFlux.Runtimes.Execution;
using ArbitraryComponentAccess.ProtoFlux.Components;

using FluxExecutionContext = ProtoFlux.Runtimes.Execution.ExecutionContext;

namespace ArbitraryComponentAccess.ProtoFluxBinds.Components;

[Category( "ProtoFlux/Runtimes/Execution/Nodes/ACA/Components" )]
public class ComponentFromSlot<T> : ObjectFunctionNode<FrooxEngineContext, T> where T : Component
{
    public readonly SyncRef<INodeObjectOutput<Slot>> slot = new();
    public readonly SyncRef<INodeValueOutput<int>> componentIndex = new();
    
    public ComponentFromSlotLogix<T> TypedNodeInstance { get; private set; } = null!;
    public override Type NodeType => typeof( ComponentFromSlotLogix<T> );
    public override INode NodeInstance => TypedNodeInstance!;
    public override int NodeInputCount => base.NodeInputCount + 2;

    public override void ClearInstance()
    {
        TypedNodeInstance = null!;
    }

    public override N Instantiate<N>()
    {
        if ( TypedNodeInstance != null )
        {
            throw new InvalidOperationException( "Node has already been instantiated" );
        }

        TypedNodeInstance = new ComponentFromSlotLogix<T>();
        return (TypedNodeInstance as N)!;
    }

    protected override void AssociateInstanceInternal( INode node )
    {
        if ( node is ComponentFromSlotLogix<T> typedNodeInstance )
        {
            TypedNodeInstance = typedNodeInstance;
            return;
        }
        throw new ArgumentException( "Node instance is not of type " + typeof( ComponentFromSlotLogix<T> ) );
    }

    protected override ISyncRef GetInputInternal( ref int index )
    {
        ISyncRef inputInternal = base.GetInputInternal( ref index );
        if ( inputInternal != null )
        {
            return inputInternal;
        }

        switch ( index )
        {
            case 0:
                return slot;
            case 1:
                return this.componentIndex;
            default:
                index -= 2;
                return null!;
        }
    }

    // FrooxEngine Weaver Stuff:
    protected override void InitializeSyncMembers()
    {
        base.InitializeSyncMembers();
    }

    public override ISyncMember GetSyncMember( int index )
    {
        return index switch
        {
            0 => persistent, 
            1 => updateOrder, 
            2 => EnabledField,
            3 => slot,
            4 => this.componentIndex,
            _ => throw new ArgumentOutOfRangeException(), 
        };
    }

    public static ComponentFromSlot<T> __New()
    {
        return new ComponentFromSlot<T>();
    }
}