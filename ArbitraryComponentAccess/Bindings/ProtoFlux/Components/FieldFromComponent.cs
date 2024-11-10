using FrooxEngine;
using ProtoFlux.Core;
using FrooxEngine.ProtoFlux;
using FrooxEngine.ProtoFlux.Runtimes.Execution;
using ArbitraryComponentAccess.ProtoFlux.Fields;

using FluxExecutionContext = ProtoFlux.Runtimes.Execution.ExecutionContext;

namespace ArbitraryComponentAccess.ProtoFluxBinds.Fields;

[Category( "ProtoFlux/Runtimes/Execution/Nodes/ACA/Fields" )]
public class FieldFromComponent : ObjectFunctionNode<FrooxEngineContext, IField>
{
    public readonly SyncRef<INodeObjectOutput<Component>> component = new();
    public readonly SyncRef<INodeObjectOutput<string>> fieldName = new();
    
    public FieldFromComponentLogix TypedNodeInstance { get; private set; } = null!;
    public override Type NodeType => typeof( FieldFromComponentLogix );
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

        TypedNodeInstance = new FieldFromComponentLogix();
        return (TypedNodeInstance as N)!;
    }

    protected override void AssociateInstanceInternal( INode node )
    {
        if ( node is FieldFromComponentLogix typedNodeInstance )
        {
            TypedNodeInstance = typedNodeInstance;
            return;
        }
        throw new ArgumentException( "Node instance is not of type " + typeof( FieldFromComponentLogix ) );
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
                return component;
            case 1:
                return fieldName;
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

    public override ISyncMember GetSyncMember(int index)
    {
        return index switch
        {
            0 => persistent, 
            1 => updateOrder, 
            2 => EnabledField,
            3 => component,
            4 => fieldName,
            _ => throw new ArgumentOutOfRangeException(), 
        };
    }

    public static FieldFromComponent __New()
    {
        return new FieldFromComponent();
    }
}
