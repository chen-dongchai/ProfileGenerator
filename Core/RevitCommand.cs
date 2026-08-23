using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ProfileGenerator.Export;

namespace ProfileGenerator.Core
{
    [Transaction(TransactionMode.Manual)]
    internal class RevitCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uIDocument = commandData.Application.ActiveUIDocument;
            MainForm mainform = new MainForm();
            mainform.GetActiveUIDocument(uIDocument);

            DWG2DExporter dWG2DExporter = new DWG2DExporter();
            ExternalEvent dWG2DEvent = ExternalEvent.Create(dWG2DExporter);

            RFA3DExporter rFA3DExporter = new RFA3DExporter();
            ExternalEvent rFA3DEvent = ExternalEvent.Create(rFA3DExporter);

            WallExporter wallExporter = new WallExporter();
            ExternalEvent wallExent = ExternalEvent.Create(wallExporter);

            mainform.GetHandler(dWG2DExporter, dWG2DEvent, rFA3DExporter, rFA3DEvent,wallExporter,wallExent);
            
            mainform.Show();

            return Result.Succeeded;
        }
    }
}
