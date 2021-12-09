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

namespace SifOut
{
    public class First
    {
        public static ObjectId[] selIObjIdColl = null;
        List<BlockData> selBlockData = new List<BlockData>();

        [CommandMethod("SifOut1")]
        public void SifOut()
        {
            selBlockData = new List<BlockData>();
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = Application.DocumentManager.MdiActiveDocument.Database;
            SelectionSet sset = null;

            PromptSelectionResult PromptSelRes = acDoc.Editor.GetSelection();
            if (PromptSelRes != null)
            {
                if (PromptSelRes.Status == PromptStatus.OK)
                {
                    sset = PromptSelRes.Value;
                    selIObjIdColl = sset.GetObjectIds();
                    Transaction acTrans = acCurDb.TransactionManager.StartTransaction();

                    AttrExtract(acTrans, selIObjIdColl);

                    GenerateSifOut();

                    acTrans.Commit();
                }
            }
        }

        public void AttrExtract(Transaction acTrans, ObjectId[] ObjIdCol)
        {
            foreach (ObjectId acObjId in ObjIdCol)
            {
                Entity acEnt = (Entity)acTrans.GetObject(acObjId, OpenMode.ForRead, true);
                if (acEnt.GetType().FullName == "Autodesk.AutoCAD.DatabaseServices.BlockReference")
                {
                    BlockReference br = (BlockReference)acTrans.GetObject(acObjId, OpenMode.ForRead);
                    if (br.AttributeCollection.Count != 0)
                    {
                        BlockData blkData = new BlockData();
                        blkData.AttVals = new Dictionary<string, string>();
                        foreach (ObjectId attId in br.AttributeCollection)
                        {
                            AttributeReference attRef = (AttributeReference)acTrans.GetObject(attId, OpenMode.ForRead);
                            if (blkData.AttVals.ContainsKey(attRef.Tag))
                            {
                                blkData.AttVals[attRef.Tag] = blkData.AttVals[attRef.Tag] + " " + attRef.TextString;
                            }
                            else
                                blkData.AttVals.Add(attRef.Tag, attRef.TextString);

                        }
                        selBlockData.Add(blkData);
                    }
                }
            }
        }
        public void GenerateSifOut()
        {

            //Read Att.ini file

            string attFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\ProjectMatrix\ATT.ini";

            List<string> partNum = new List<string>();
            List<string> mfg = new List<string>();
            List<string> qty = new List<string>();
            List<string> options = new List<string>();
            List<string> tag1 = new List<string>();
            List<string> tag2 = new List<string>();
            List<string> tag3 = new List<string>();
            List<string> tag4 = new List<string>();
            List<string> tag5 = new List<string>();
            List<string> descrip = new List<string>();
            List<string> iText = new List<string>();


            var linesRead = File.ReadLines(attFilePath);

            //PartNum
            bool partNumFound = false;
            int index = 0;
            foreach (string attLine in linesRead)
            {
                if (attLine == "[PartNum]")
                {
                    partNumFound = true;
                    continue;
                }
                if (partNumFound == true)
                {
                    if (index == 12)
                    {
                        index = 0;
                        break;
                    }
                    partNum.Add(attLine.Substring(attLine.IndexOf("=") + 1));
                    index++;
                }
            }
            //Mfg
            bool mfgFound = false;
            foreach (string attLine in linesRead)
            {
                if (attLine == "[Mfg]")
                {
                    mfgFound = true;
                    continue;
                }
                if (mfgFound == true)
                {
                    if (index == 12)
                    {
                        index = 0;
                        break;
                    }
                    mfg.Add(attLine.Substring(attLine.IndexOf("=") + 1));
                    index++;
                }
            }
            //Qty
            bool qtyFound = false;
            foreach (string attLine in linesRead)
            {
                if (attLine == "[Qty]")
                {
                    qtyFound = true;
                    continue;
                }
                if (qtyFound == true)
                {
                    if (index == 12)
                    {
                        index = 0;
                        break;
                    }
                    qty.Add(attLine.Substring(attLine.IndexOf("=") + 1));
                    index++;
                }
            }
            //Options
            bool optionsFound = false;
            foreach (string attLine in linesRead)
            {
                if (attLine == "[Options]")
                {
                    optionsFound = true;
                    continue;
                }
                if (optionsFound == true)
                {
                    if (index == 12)
                    {
                        index = 0;
                        break;
                    }
                    options.Add(attLine.Substring(attLine.IndexOf("=") + 1));
                    index++;
                }
            }
            //Tag1
            bool tag1Found = false;
            foreach (string attLine in linesRead)
            {
                if (attLine == "[Tag1]")
                {
                    tag1Found = true;
                    continue;
                }
                if (tag1Found == true)
                {
                    if (index == 12)
                    {
                        index = 0;
                        break;
                    }
                    tag1.Add(attLine.Substring(attLine.IndexOf("=") + 1));
                    index++;
                }
            }
            //Tag2
            bool tag2Found = false;
            foreach (string attLine in linesRead)
            {
                if (attLine == "[Tag2]")
                {
                    tag2Found = true;
                    continue;
                }
                if (tag2Found == true)
                {
                    if (index == 12)
                    {
                        index = 0;
                        break;
                    }
                    tag2.Add(attLine.Substring(attLine.IndexOf("=") + 1));
                    index++;
                }
            }
            //Tag3
            bool tag3Found = false;
            foreach (string attLine in linesRead)
            {
                if (attLine == "[Tag3]")
                {
                    tag3Found = true;
                    continue;
                }
                if (tag3Found == true)
                {
                    if (index == 12)
                    {
                        index = 0;
                        break;
                    }
                    tag3.Add(attLine.Substring(attLine.IndexOf("=") + 1));
                    index++;
                }
            }
            //Tag4
            bool tag4Found = false;
            foreach (string attLine in linesRead)
            {
                if (attLine == "[Tag4]")
                {
                    tag4Found = true;
                    continue;
                }
                if (tag4Found == true)
                {
                    if (index == 12)
                    {
                        index = 0;
                        break;
                    }
                    tag4.Add(attLine.Substring(attLine.IndexOf("=") + 1));
                    index++;
                }
            }
            //Tag2
            bool tag5Found = false;
            foreach (string attLine in linesRead)
            {
                if (attLine == "[Tag5]")
                {
                    tag5Found = true;
                    continue;
                }
                if (tag5Found == true)
                {
                    if (index == 12)
                    {
                        index = 0;
                        break;
                    }
                    tag5.Add(attLine.Substring(attLine.IndexOf("=") + 1));
                    index++;
                }
            }
            //Descrip
            bool descripFound = false;
            foreach (string attLine in linesRead)
            {
                if (attLine == "[Descrip]")
                {
                    descripFound = true;
                    continue;
                }
                if (descripFound == true)
                {
                    if (index == 12)
                    {
                        index = 0;
                        break;
                    }
                    descrip.Add(attLine.Substring(attLine.IndexOf("=") + 1));
                    index++;
                }
            }
            //Itext
            bool iTextFound = false;
            foreach (string attLine in linesRead)
            {
                if (attLine == "[Itext]")
                {
                    iTextFound = true;
                    continue;
                }
                if (iTextFound == true)
                {
                    if (index == 12)
                        break;
                    iText.Add(attLine.Substring(attLine.IndexOf("=") + 1));
                    index++;
                }
            }

            string sifFilePath = "";
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = @"C:\";
            saveFileDialog1.Title = "Save SIF Files";
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "sif";
            saveFileDialog1.Filter = "Sif files (*.sif)|*.sif";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                sifFilePath = saveFileDialog1.FileName;
            }

            FileStream f = new FileStream(sifFilePath, FileMode.Create);


            StreamWriter s = new StreamWriter(f);
            Document acDoc = Application.DocumentManager.MdiActiveDocument;

            string line = "SF=" + Path.GetFileNameWithoutExtension(acDoc.Name) + ".SPC";
            s.WriteLine(line);
            line = "ST=ProjectSymbols AutoCAD 2018 Takeoff " + DateTime.Now.ToString("HH:mm:ss MM-dd-yyy");
            s.WriteLine(line);
            line = "DT=" + DateTime.Now.ToString("MM-dd-yyyy");
            s.WriteLine(line);
            line = "TM=" + DateTime.Now.ToString("HH:mm:ss");
            s.WriteLine(line);
            Dictionary<string, string> sortedAttrs = new Dictionary<string, string>();
            int blockIndex = 0;
            foreach (BlockData blockData in selBlockData)
            {
                sortedAttrs.Add("PN__" + blockIndex + "=", "");
                foreach (KeyValuePair<string, string> entry in blockData.AttVals)
                {
                    if (partNum.Contains(entry.Key))
                        sortedAttrs["PN__" + blockIndex + "="] = entry.Value;
                }
                sortedAttrs.Add("MC__" + blockIndex + "=", "");
                foreach (KeyValuePair<string, string> entry in blockData.AttVals)
                {
                    if (mfg.Contains(entry.Key))
                        sortedAttrs["MC__" + blockIndex + "="] = entry.Value;
                }
                sortedAttrs.Add("QT__" + blockIndex + "=", "");
                foreach (KeyValuePair<string, string> entry in blockData.AttVals)
                {
                    if (qty.Contains(entry.Key))
                    {
                        string val = entry.Value;
                        if (val == "")
                            val = "1";
                        sortedAttrs["QT__" + blockIndex + "="] = val;
                    }
                }
                sortedAttrs.Add("PL__" + blockIndex + "=", "");
                foreach (KeyValuePair<string, string> entry in blockData.AttVals)
                {
                    if (entry.Key == "PSLIST")
                    {
                        sortedAttrs["PL__" + blockIndex + "="] = entry.Value;
                    }
                }
                sortedAttrs.Add("TG__" + blockIndex + "=", "");
                foreach (KeyValuePair<string, string> entry in blockData.AttVals)
                {
                    if (tag1.Contains(entry.Key))
                    {
                        if (entry.Value != "" && entry.Value != ".")
                            sortedAttrs["TG__" + blockIndex + "="] = entry.Value;
                        else
                            sortedAttrs["TG__" + blockIndex + "="] = "";
                    }
                }
                sortedAttrs.Add("GC__" + blockIndex + "=", "");
                foreach (KeyValuePair<string, string> entry in blockData.AttVals)
                {
                    if (tag2.Contains(entry.Key))
                    {
                        if (entry.Value != "" && entry.Value != ".")
                            sortedAttrs["GC__" + blockIndex + "="] = entry.Value;
                        else
                            sortedAttrs["GC__" + blockIndex + "="] = "";
                    }
                }
                sortedAttrs.Add("T3__" + blockIndex + "=", "");
                foreach (KeyValuePair<string, string> entry in blockData.AttVals)
                {
                    if (iText.Contains(entry.Key))
                    {
                        if (entry.Value != "" && entry.Value != ".")
                            sortedAttrs["T3__" + blockIndex + "="] = entry.Value;
                        else
                            sortedAttrs["T3__" + blockIndex + "="] = "";
                    }
                }
                sortedAttrs.Add("T4__" + blockIndex + "=", "");
                foreach (KeyValuePair<string, string> entry in blockData.AttVals)
                {
                    if (tag4.Contains(entry.Key))
                    {
                        if (entry.Value != "" && entry.Value != ".")
                            sortedAttrs["T4__" + blockIndex + "="] = entry.Value;
                        else
                            sortedAttrs["T4__" + blockIndex + "="] = "";
                    }
                }
                sortedAttrs.Add("T5__" + blockIndex + "=", "");
                foreach (KeyValuePair<string, string> entry in blockData.AttVals)
                {
                    if (tag5.Contains(entry.Key))
                    {
                        if (entry.Value != "" && entry.Value != ".")
                            sortedAttrs["T5__" + blockIndex + "="] = entry.Value;
                        else
                            sortedAttrs["T5__" + blockIndex + "="] = "";
                    }
                }
                sortedAttrs.Add("PD__" + blockIndex + "=", "");
                foreach (KeyValuePair<string, string> entry in blockData.AttVals)
                {
                    if (descrip.Contains(entry.Key))
                        sortedAttrs["PD__" + blockIndex + "="] = entry.Value;
                }
                sortedAttrs.Add("TK__" + blockIndex + "=", "");
                foreach (KeyValuePair<string, string> entry in blockData.AttVals)
                {
                    if (entry.Key == "TYPE")
                        sortedAttrs["TK__" + blockIndex + "="] = entry.Value;
                }
                sortedAttrs.Add("EC__" + blockIndex + "=", "");
                foreach (KeyValuePair<string, string> entry in blockData.AttVals)
                {
                    if (entry.Key == "PSEC")
                        sortedAttrs["EC__" + blockIndex + "="] = entry.Value;
                }
                sortedAttrs.Add("WT__" + blockIndex + "=", "");
                foreach (KeyValuePair<string, string> entry in blockData.AttVals)
                {
                    if (entry.Key == "WT")
                    {
                        sortedAttrs["WT__" + blockIndex + "="] = entry.Value;
                    }
                }
                sortedAttrs.Add("VO__" + blockIndex + "=", "");
                foreach (KeyValuePair<string, string> entry in blockData.AttVals)
                {
                    if (entry.Key == "VO")
                    {
                        sortedAttrs["VO__" + blockIndex + "="] = entry.Value;
                    }
                }

                //OPTIONALS
                foreach (KeyValuePair<string, string> entry in blockData.AttVals)
                {
                    if (options.Contains(entry.Key))
                    {
                        int numOptions = entry.Value.Split('\\').Length;

                        if (numOptions > 0)
                        {
                            string[] opts = entry.Value.Split('\\');
                            int optIndex = 0;

                            if (entry.Value == ".")
                                continue;

                            if (opts.Length > 1 && opts[1] == "??")
                                continue;

                            foreach (string opt in opts)
                            {
                                if (opt == "" || opt == ",")
                                    continue;
                                sortedAttrs.Add("ON__" + blockIndex + optIndex + "=", opt);
                                sortedAttrs.Add("OD__" + blockIndex + optIndex + "=", "");
                                sortedAttrs.Add("OL__" + blockIndex + optIndex + "=", "");
                                sortedAttrs.Add("OG__" + blockIndex + optIndex + "=", "");
                                optIndex++;
                            }
                        }
                    }
                }
                foreach (KeyValuePair<string, string> entry in blockData.AttVals)
                {
                    if (entry.Key == "CR#")
                        sortedAttrs.Add("CR__" + blockIndex + "=", entry.Value);
                }
                foreach (KeyValuePair<string, string> entry in blockData.AttVals)
                {
                    if (entry.Key == "SifNote")
                        sortedAttrs.Add("SifNote__" + blockIndex + "=", entry.Value);
                }
                foreach (KeyValuePair<string, string> entry in blockData.AttVals)
                {
                    if (entry.Key == "REPORTNOTE")
                        sortedAttrs.Add("REPORTNOTE" + blockIndex + "=", entry.Value);
                }
                foreach (KeyValuePair<string, string> entry in blockData.AttVals)
                {
                    if (entry.Key == "PSQP")
                        sortedAttrs.Add("QP" + blockIndex + "=", entry.Value);
                }
                foreach (KeyValuePair<string, string> entry in blockData.AttVals)
                {
                    if (entry.Key == "PSQC")
                        sortedAttrs.Add("QC" + blockIndex + "=", entry.Value);
                }
                blockIndex++;
            }

            #region Unused Code
            //foreach (KeyValuePair<string, string> entry in attVals)
            //{
            //    if (entry.Key == "OD")
            //    {
            //        sortedAttrs.Add("OD=", entry.Value);
            //    }
            //}
            //foreach (KeyValuePair<string, string> entry in attVals)
            //{
            //    if (entry.Key == "OL")
            //    {
            //        sortedAttrs.Add("OL=", entry.Value);
            //    }
            //}
            //foreach (KeyValuePair<string, string> entry in attVals)
            //{
            //    if (entry.Key == "OG")
            //    {
            //        sortedAttrs.Add("OG=", entry.Value);
            //    }
            //}
            #endregion

            FormattedBlocksData fBlockData = new FormattedBlocksData();
            fBlockData.AttValsList = new List<Dictionary<string, string>>();

            Dictionary<string, string> formattedBlockData = new Dictionary<string, string>();

            foreach (KeyValuePair<string, string> entry in sortedAttrs)
            {
                if (entry.Key.StartsWith("PN_"))
                {
                    if (formattedBlockData.Keys.Count > 0)
                        fBlockData.AttValsList.Add(formattedBlockData);

                    formattedBlockData = new Dictionary<string, string>();
                }
                if (entry.Key.StartsWith("PN_"))
                    formattedBlockData.Add("PN=", entry.Value);
                else
                    formattedBlockData.Add(entry.Key, entry.Value);
            }

            if (formattedBlockData.Keys.Count > 0)
                fBlockData.AttValsList.Add(formattedBlockData);

            var finalOutput = fBlockData.AttValsList.GroupBy(x => x["PN="]);

            //var duplicates = fBlockData.AttValsList
            //                        .SelectMany((x, index) => x.Select(p => new { Index = index, Key = p.Key, Value = p.Value }))
            //                        .GroupBy(x => x.Value)
            //                        .Where(x => x.Count() > 1)
            //                        .Select(x => new
            //                        {
            //                            Value = x.First().Value,
            //                            Occurrences = x.Select(o => new { Index = o.Index, Key = o.Key })
            //                        });

            var duplicates = fBlockData.AttValsList
                                .SelectMany(x => x.Values)
                                .GroupBy(x => x)
                                .Where(x => x.Count() > 1);

            foreach (var d in finalOutput)
            {
                int internalCount = d.Count();
                Dictionary<string, string> firstBlkData = d.First();
                foreach (KeyValuePair<string, string> entry in firstBlkData)
                {
                    if (entry.Key.Contains("__"))
                    {
                        if (entry.Key.StartsWith("QT_"))
                            s.WriteLine(entry.Key.Substring(0, entry.Key.IndexOf("__")) + "=" + internalCount);
                        else
                            s.WriteLine(entry.Key.Substring(0, entry.Key.IndexOf("__")) + "=" + entry.Value);
                    }
                    else
                        s.WriteLine(entry.Key + entry.Value);
                }

            }
            s.WriteLine("XX=END OF SIF");
            s.Close();
            f.Close();
        }


    }

    public class FormattedBlocksData
    {
        public string PartNumber { get; set; }

        public List<Dictionary<string, string>> AttValsList { get; set; }
    }

    public class BlockData
    {
        public string ObjectId { get; set; }
        public Dictionary<string, string> AttVals { get; set; }
    }
}
