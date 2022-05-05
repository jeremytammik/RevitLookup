using Autodesk.Revit.UI;

namespace RevitLookup.Commands.Handlers;

public class CollectDataHandler : BaseEventHandler
{
    private Action _action;

    public override void Execute(UIApplication app)
    {
        _action.Invoke();
    }

    public void Raise(Action action)
    {
        _action = action;
        Raise();
    }
}