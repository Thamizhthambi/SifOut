using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;
using Newtonsoft.Json;

namespace SifOut
{
    public class Command
    {

        [CommandMethod("SifOut")]
        public void SifOut()
        {
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = Application.DocumentManager.MdiActiveDocument.Database;
            PromptSelectionResult PromptSelRes = acDoc.Editor.GetSelection();
            if (PromptSelRes != null)
            {
                if (PromptSelRes.Status == PromptStatus.OK)
                {
                    SifTakeOff sif = new SifTakeOff(PromptSelRes);
                    sif.ShowDialog();
                }
            }
        }
    }
}
