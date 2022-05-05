using Autodesk.Revit.UI;

namespace RevitLookup.Commands.Handlers;

/// <summary>
///     Base external event used to change the model.
/// </summary>
/// <remarks>
///     In derived classes create an overload of the Raise() method to add parameters.
///     <code>
///          Command.EventHandler.Raise(value1, value2);
///      </code>
///     Or use public properties.
///     <code>
///         Command.EventHandler.Property1 = "value1";
///         Command.EventHandler.Property2 = "value2";
///         Command.EventHandler.Raise();
///      </code>
/// </remarks>
public class BaseEventHandler : IExternalEventHandler
{
    private readonly ExternalEvent _externalEvent;

    protected BaseEventHandler()
    {
        _externalEvent = ExternalEvent.Create(this);
    }

    public virtual void Execute(UIApplication app)
    {
        throw new NotImplementedException();
    }

    public string GetName()
    {
        return GetType().Name;
    }

    public void Raise()
    {
        _externalEvent.Raise();
    }
}