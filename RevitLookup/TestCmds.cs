
    /// <summary>
    ///     Snoop dependent elements using
    ///     Element.GetDependentElements
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class CmdSnoopModScopeDependents : IExternalCommand
    {
        public Result Execute(ExternalCommandData cmdData, ref string msg, ElementSet elems)
        {
            var form = new Objects();
            form.SnoopAndShow(Selector.SnoopDependentElements);
            return Result.Succeeded;
        }
    }

    /// <summary>
    ///     SnoopDB command:  Browse the current view...
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class CmdSnoopActiveView : IExternalCommand
    {
        public Result Execute(ExternalCommandData cmdData, ref string msg, ElementSet elems)
        {
            var form = new Objects();
            form.SnoopAndShow(Selector.SnoopActiveView);
            return Result.Succeeded;
        }
    }

    /// <summary>
    ///     Snoop ModScope command:  Browse all Elements in the current selection set
    ///     In case nothing is selected: browse visible elements
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class CmdSnoopModScope : IExternalCommand
    {
        public Result Execute(ExternalCommandData cmdData, ref string msg, ElementSet elems)
        {
            var form = new Objects();
            form.SnoopAndShow(Selector.SnoopCurrentSelection);
            return Result.Succeeded;
        }
    }

    /// <summary>
    ///     Snoop App command:  Browse all objects that are part of the Application object
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class CmdSnoopApp : IExternalCommand
    {
        public Result Execute(ExternalCommandData cmdData, ref string msg, ElementSet elems)
        {
            var form = new Objects();
            form.SnoopAndShow(Selector.SnoopApplication);
            return Result.Succeeded;
        }
    }

    /// <summary>
    ///     Snoop ModScope command:  Browse all Elements in the current selection set
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class CmdSampleMenuItemCallback : IExternalCommand
    {
        public Result Execute(ExternalCommandData cmdData, ref string msg, ElementSet elems)
        {
            Result result;

            try
            {
                MessageBox.Show("Called back into RevitLookup by picking toolbar or menu item");
                result = Result.Succeeded;
            }
            catch (Exception e)
            {
                msg = e.Message;
                result = Result.Failed;
            }

            return result;
        }
    }

    /// <summary>
    ///     Search by and Snoop command: Browse
    ///     elements found by the condition
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class CmdSearchBy : IExternalCommand
    {
        public Result Execute(ExternalCommandData cmdData, ref string msg, ElementSet elems)
        {
            var revitDoc = cmdData.Application.ActiveUIDocument;
            var dbdoc = revitDoc.Document;
            var form = new SearchBy(dbdoc);
            ModelessWindowFactory.Show(form);

            return Result.Succeeded;
        }
    }
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
}