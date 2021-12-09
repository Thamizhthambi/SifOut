using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.IO;

namespace SifOut
{
    public class Command
    {
        Dictionary<string, string> attVals = new Dictionary<string, string>();
        public static ObjectId[] selIObjIdColl = null;
        public static int[] countcol = null;

        [CommandMethod("SifOut")]
        public void SifOut()
        {
            attVals = new Dictionary<string, string>();
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = Application.DocumentManager.MdiActiveDocument.Database;
            ObjectIdCollection obidcoll = new ObjectIdCollection();
            SelectionSet sset = null;

            ObjectId[] ObjId = null;
            PromptSelectionResult PromptSelRes = acDoc.Editor.GetSelection();
            if (PromptSelRes != null)
            {
                if (PromptSelRes.Status == PromptStatus.OK)
                {
                    sset = PromptSelRes.Value;
                    selIObjIdColl = sset.GetObjectIds();

                    Transaction acTrans = acCurDb.TransactionManager.StartTransaction();

                    //BlockReferenceList(ObjId, acTrans);

                    AttrExtract(acTrans, selIObjIdColl);

                    SifTextRight();

                    acTrans.Commit();
                }
            }
        }

        public void BlockReferenceList(ObjectId[] ObjId, Transaction acTrans)
        {
            countcol = new int[ObjId.Length];
            SOBJIDCollection = new ObjectId[ObjId.Length];
            int q = 0, count = 0;
            bool test;
            for (int val = 0; val < ObjId.Length; val++)
            {
                test = true;
                BlockReference br = (BlockReference)acTrans.GetObject(ObjId[val], OpenMode.ForRead);
                for (int i = 0; i < SOBJIDCollection.Length; i++)
                {
                    if (SOBJIDCollection[i].Database != null)
                    {
                        BlockReference br2 = (BlockReference)acTrans.GetObject(SOBJIDCollection[i], OpenMode.ForRead);
                        if (br.Name == br2.Name) //if (SOBJIDCollection[i] == ObjId[val])
                        {
                            test = false;
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                count = 0;
                if (test)
                {
                    for (int sval = 0; sval < ObjId.Length; sval++)
                    {
                        BlockReference br1 = (BlockReference)acTrans.GetObject(ObjId[sval], OpenMode.ForRead);
                        if (br.Name == br1.Name)
                        {
                            count = count + 1;
                        }
                    }
                    SOBJIDCollection[q] = ObjId[val];
                    countcol[q] = count;
                    q++;
                }
            }
        }
        public void AttrExtract(Transaction acTrans, ObjectId[] ObjIdCol)
        {
            int i = ObjIdCol.Length, l = 0;

            foreach (ObjectId acObjId in ObjIdCol)
            {
                if (acObjId.Database != null)
                {
                    Entity acEnt = (Entity)acTrans.GetObject(acObjId, OpenMode.ForWrite, true);
                    if (acEnt.GetType().FullName == "Autodesk.AutoCAD.DatabaseServices.BlockReference")
                    {
                        BlockReference br = (BlockReference)acTrans.GetObject(acObjId, OpenMode.ForRead);
                        foreach (ObjectId attId in br.AttributeCollection)
                        {
                            AttributeReference attRef = (AttributeReference)acTrans.GetObject(attId, OpenMode.ForRead);
                            attVals.Add(attRef.Tag, attRef.TextString);
                        }
                    }
                }
            }
        }
        public void SifTextRight()
        {

            //Read Att.ini file

            string attFilePath = @"C:\Users\Nagarjuna\Desktop\ATT.ini";

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

            FileStream f = new FileStream("C:\\Users\\nagarjuna\\Desktop\\Sifout.txt", FileMode.Create);
            StreamWriter s = new StreamWriter(f);
            string csv = null;
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
            sortedAttrs.Add("PN=", "");
            foreach (KeyValuePair<string, string> entry in attVals)
            {
                if (partNum.Contains(entry.Key))
                    sortedAttrs["PN="] = entry.Value;
            }
            sortedAttrs.Add("MC=", "");
            foreach (KeyValuePair<string, string> entry in attVals)
            {
                if (mfg.Contains(entry.Key))
                    sortedAttrs["MC="] = entry.Value;
            }
            sortedAttrs.Add("QT=", "");
            foreach (KeyValuePair<string, string> entry in attVals)
            {
                if (qty.Contains(entry.Key))
                {
                    string val = entry.Value;
                    if (val == "")
                        val = "1";
                    sortedAttrs["QT="] = val;
                }
            }
            sortedAttrs.Add("PL=", "");
            foreach (KeyValuePair<string, string> entry in attVals)
            {
                if (entry.Key == "PSLIST")
                {
                    sortedAttrs["PL="] = entry.Value;
                }
            }
            sortedAttrs.Add("TG=", "");
            foreach (KeyValuePair<string, string> entry in attVals)
            {
                if (tag1.Contains(entry.Key))
                    sortedAttrs["TG="] = entry.Value;
            }
            sortedAttrs.Add("GC=", "");
            foreach (KeyValuePair<string, string> entry in attVals)
            {
                if (tag2.Contains(entry.Key))
                    sortedAttrs["GC="] = entry.Value;
            }
            sortedAttrs.Add("T3=", "");
            foreach (KeyValuePair<string, string> entry in attVals)
            {
                if (iText.Contains(entry.Key))
                    sortedAttrs["T3="] = entry.Value;
            }
            sortedAttrs.Add("T4=", "");
            foreach (KeyValuePair<string, string> entry in attVals)
            {
                if (tag4.Contains(entry.Key))
                    sortedAttrs["T4="] = entry.Value;
            }
            sortedAttrs.Add("T5=", "");
            foreach (KeyValuePair<string, string> entry in attVals)
            {
                if (tag5.Contains(entry.Key))
                    sortedAttrs["T5="] = entry.Value;
            }
            sortedAttrs.Add("PD=", "");
            foreach (KeyValuePair<string, string> entry in attVals)
            {
                if (descrip.Contains(entry.Key))
                    sortedAttrs["PD="] = entry.Value;
            }
            sortedAttrs.Add("TK=", "");
            foreach (KeyValuePair<string, string> entry in attVals)
            {
                if (entry.Key == "TYPE")
                    sortedAttrs["TK="] = entry.Value;
            }
            sortedAttrs.Add("EC=", "");
            foreach (KeyValuePair<string, string> entry in attVals)
            {
                if (entry.Key == "PSEC")
                    sortedAttrs["EC="] = entry.Value;
            }
            sortedAttrs.Add("WT=", "");
            foreach (KeyValuePair<string, string> entry in attVals)
            {
                if (entry.Key == "WT")
                {
                    sortedAttrs["WT="] = entry.Value;
                }
            }
            sortedAttrs.Add("VO=", "");
            foreach (KeyValuePair<string, string> entry in attVals)
            {
                if (entry.Key == "VO")
                {
                    sortedAttrs["VO="] = entry.Value;
                }
            }


            //OPTIONALS


            foreach (KeyValuePair<string, string> entry in attVals)
            {
                if (options.Contains(entry.Key))
                {
                    int numOptions = entry.Value.Split('\\').Length;

                    if (numOptions > 0)
                    {
                        string[] opts = entry.Value.Split('\\');
                        int optIndex = 0;
                        foreach (string opt in opts)
                        {
                            if (opt == "")
                                continue;
                            sortedAttrs.Add("ON__" + optIndex + "=", opt);
                            sortedAttrs.Add("OD__" + optIndex + "=", "");
                            sortedAttrs.Add("OL__" + optIndex + "=", "");
                            sortedAttrs.Add("OG__" + optIndex + "=", "");
                            optIndex++;
                        }
                    }
                }
            }
            foreach (KeyValuePair<string, string> entry in attVals)
            {
                if (entry.Key == "CR#")
                    sortedAttrs.Add("CR=", entry.Value);
            }
            foreach (KeyValuePair<string, string> entry in attVals)
            {
                if (entry.Key == "SifNote")
                    sortedAttrs.Add("SifNote=", entry.Value);
            }
            foreach (KeyValuePair<string, string> entry in attVals)
            {
                if (entry.Key == "REPORTNOTE")
                {
                    sortedAttrs.Add("REPORTNOTE=", entry.Value);
                }
            }
            foreach (KeyValuePair<string, string> entry in attVals)
            {
                if (entry.Key == "PSQP")
                {
                    sortedAttrs.Add("QP=", entry.Value);
                }
            }
            foreach (KeyValuePair<string, string> entry in attVals)
            {
                if (entry.Key == "PSQC")
                {
                    sortedAttrs.Add("QC=", entry.Value);
                }
            }
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

            foreach (KeyValuePair<string, string> entry in sortedAttrs)
            {
                if (entry.Key.Contains("__"))
                    s.WriteLine(entry.Key.Substring(0, entry.Key.IndexOf("__")) + "=" + entry.Value);
                else
                    s.WriteLine(entry.Key + entry.Value);
            }

            s.WriteLine("XX=END OF SIF");
            s.Close();
            f.Close();
        }

        public int CountOptions(string Options)
        {
            int NumOptions = 1;
            if (Options == "" || Options == ".")
                return 0;
            while (Options.IndexOf("\\") != -1)
            {
                Options = Options.Substring(Options.Length - Options.IndexOf("\\") - 1);
                NumOptions++;
            }
            return NumOptions;
        }
    }
}
