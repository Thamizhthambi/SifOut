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
        public static ObjectId[] selIObjIdColl = null;
        List<BlockData> selBlockData = new List<BlockData>();

        [CommandMethod("SifOut")]
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
                        blkData.BlockAttr = new List<Row>();
                        foreach (ObjectId attId in br.AttributeCollection)
                        {
                            AttributeReference attRef = (AttributeReference)acTrans.GetObject(attId, OpenMode.ForRead);

                            if (blkData.BlockAttr.FindAll(x => x.Key == attRef.Tag).Count > 0)
                            {
                                Row sameAttr = blkData.BlockAttr.FindAll(x => x.Key == attRef.Tag)[0];
                                sameAttr.Value = sameAttr.Value + " " + attRef.TextString;
                            }
                            else
                                blkData.BlockAttr.Add(new Row { Key = attRef.Tag, Value = attRef.TextString });
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

            #region Read Att.ini file

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
                    partNum.Add(attLine.Substring(attLine.IndexOf("=") + 1).ToUpper());
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
                    mfg.Add(attLine.Substring(attLine.IndexOf("=") + 1).ToUpper());
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
                    qty.Add(attLine.Substring(attLine.IndexOf("=") + 1).ToUpper());
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
                    options.Add(attLine.Substring(attLine.IndexOf("=") + 1).ToUpper());
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
                    tag1.Add(attLine.Substring(attLine.IndexOf("=") + 1).ToUpper());
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
                    tag2.Add(attLine.Substring(attLine.IndexOf("=") + 1).ToUpper());
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
                    tag3.Add(attLine.Substring(attLine.IndexOf("=") + 1).ToUpper());
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
                    tag4.Add(attLine.Substring(attLine.IndexOf("=") + 1).ToUpper());
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
                    tag5.Add(attLine.Substring(attLine.IndexOf("=") + 1).ToUpper());
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
                    descrip.Add(attLine.Substring(attLine.IndexOf("=") + 1).ToUpper());
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
                    iText.Add(attLine.Substring(attLine.IndexOf("=") + 1).ToUpper());
                    index++;
                }
            }

            #endregion

            #region Get sifout file name and location

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
            #endregion

            FileStream f = new FileStream(sifFilePath, FileMode.Create);
            StreamWriter s = new StreamWriter(f);
            Document acDoc = Application.DocumentManager.MdiActiveDocument;

            #region Writing Sifout header data
            string line = "SF=" + Path.GetFileNameWithoutExtension(acDoc.Name) + ".SPC";
            s.WriteLine(line);
            line = "ST=ProjectSymbols AutoCAD 2018 Takeoff " + DateTime.Now.ToString("HH:mm:ss MM-dd-yyy");
            s.WriteLine(line);
            line = "DT=" + DateTime.Now.ToString("MM-dd-yyyy");
            s.WriteLine(line);
            line = "TM=" + DateTime.Now.ToString("HH:mm:ss");
            s.WriteLine(line);
            #endregion

            //Dictionary<string, string> sortedAttrs = new Dictionary<string, string>();
            List<Row> sortedAttrs = new List<Row>();

            int blockIndex = 0;
            foreach (BlockData blockData in selBlockData)
            {
                sortedAttrs.Add(new Row { Key = "PN=", Value = "" });
                foreach (Row entry in blockData.BlockAttr)
                {
                    if (partNum.Contains(entry.Key.ToUpper()))
                        sortedAttrs[sortedAttrs.Count - 1].Value = entry.Value;
                }
                if (sortedAttrs[sortedAttrs.Count - 1].Key == "PN=")
                {
                    if (sortedAttrs[sortedAttrs.Count - 1].Value == "")
                    {
                        sortedAttrs.RemoveAt(sortedAttrs.Count - 1);
                        continue;
                    }
                }
                sortedAttrs.Add(new Row { Key = "MC=", Value = "" });
                foreach (Row entry in blockData.BlockAttr)
                {
                    if (mfg.Contains(entry.Key.ToUpper()))
                        sortedAttrs[sortedAttrs.Count - 1].Value = entry.Value;
                }
                sortedAttrs.Add(new Row { Key = "QT=", Value = "" });
                foreach (Row entry in blockData.BlockAttr)
                {
                    if (qty.Contains(entry.Key.ToUpper()))
                    {
                        string val = entry.Value;
                        if (val == "")
                            val = "1";
                        sortedAttrs[sortedAttrs.Count - 1].Value = entry.Value;
                    }
                }
                sortedAttrs.Add(new Row { Key = "PL=", Value = "" });
                foreach (Row entry in blockData.BlockAttr)
                {
                    if (entry.Key == "PSLIST")
                    {
                        sortedAttrs[sortedAttrs.Count - 1].Value = entry.Value;
                    }
                }
                sortedAttrs.Add(new Row { Key = "TG=", Value = "" });
                foreach (Row entry in blockData.BlockAttr)
                {
                    if (tag1.Contains(entry.Key.ToUpper()))
                    {
                        if (entry.Value != "" && entry.Value != ".")
                            sortedAttrs[sortedAttrs.Count - 1].Value = entry.Value;
                        else
                            sortedAttrs[sortedAttrs.Count - 1].Value = "";
                    }
                }
                sortedAttrs.Add(new Row { Key = "GC=", Value = "" });
                foreach (Row entry in blockData.BlockAttr)
                {
                    if (tag2.Contains(entry.Key.ToUpper()))
                    {
                        if (entry.Value != "" && entry.Value != ".")
                            sortedAttrs[sortedAttrs.Count - 1].Value = entry.Value;
                        else
                            sortedAttrs[sortedAttrs.Count - 1].Value = "";
                    }
                }
                sortedAttrs.Add(new Row { Key = "T3=", Value = "" });
                foreach (Row entry in blockData.BlockAttr)
                {
                    if (iText.Contains(entry.Key.ToUpper()))
                    {
                        if (entry.Value != "" && entry.Value != ".")
                            sortedAttrs[sortedAttrs.Count - 1].Value = entry.Value;
                        else
                            sortedAttrs[sortedAttrs.Count - 1].Value = "";
                    }
                }
                sortedAttrs.Add(new Row { Key = "T4=", Value = "" });
                foreach (Row entry in blockData.BlockAttr)
                {
                    if (tag4.Contains(entry.Key.ToUpper()))
                    {
                        if (entry.Value != "" && entry.Value != ".")
                            sortedAttrs[sortedAttrs.Count - 1].Value = entry.Value;
                        else
                            sortedAttrs[sortedAttrs.Count - 1].Value = "";
                    }
                }
                sortedAttrs.Add(new Row { Key = "T5=", Value = "" });
                foreach (Row entry in blockData.BlockAttr)
                {
                    if (tag5.Contains(entry.Key.ToUpper()))
                    {
                        if (entry.Value != "" && entry.Value != ".")
                            sortedAttrs[sortedAttrs.Count - 1].Value = entry.Value;
                        else
                            sortedAttrs[sortedAttrs.Count - 1].Value = "";
                    }
                }
                sortedAttrs.Add(new Row { Key = "PD=", Value = "" });
                foreach (Row entry in blockData.BlockAttr)
                {
                    if (descrip.Contains(entry.Key.ToUpper()))
                        sortedAttrs[sortedAttrs.Count - 1].Value = entry.Value;
                }
                sortedAttrs.Add(new Row { Key = "TK=", Value = "" });
                foreach (Row entry in blockData.BlockAttr)
                {
                    if (entry.Key == "TYPE")
                        sortedAttrs[sortedAttrs.Count - 1].Value = entry.Value;
                }
                sortedAttrs.Add(new Row { Key = "EC=", Value = "" });
                foreach (Row entry in blockData.BlockAttr)
                {
                    if (entry.Key == "PSEC")
                        sortedAttrs[sortedAttrs.Count - 1].Value = entry.Value;
                }
                sortedAttrs.Add(new Row { Key = "WT=", Value = "" });
                foreach (Row entry in blockData.BlockAttr)
                {
                    if (entry.Key == "WT")
                    {
                        sortedAttrs[sortedAttrs.Count - 1].Value = entry.Value;
                    }
                }
                sortedAttrs.Add(new Row { Key = "VO=", Value = "" });
                foreach (Row entry in blockData.BlockAttr)
                {
                    if (entry.Key == "VO")
                    {
                        sortedAttrs[sortedAttrs.Count - 1].Value = entry.Value;
                    }
                }

                //OPTIONALS
                foreach (Row entry in blockData.BlockAttr)
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
                                sortedAttrs.Add(new Row { Key = "ON=", Value = opt });
                                sortedAttrs.Add(new Row { Key = "OD=", Value = "" });
                                sortedAttrs.Add(new Row { Key = "OL=", Value = "" });
                                sortedAttrs.Add(new Row { Key = "OG=", Value = "" });
                                optIndex++;
                            }
                        }
                    }
                }
                foreach (Row entry in blockData.BlockAttr)
                {
                    if (entry.Key == "CR#")
                        sortedAttrs.Add(new Row { Key = "CR=", Value = entry.Value });
                }
                foreach (Row entry in blockData.BlockAttr)
                {
                    if (entry.Key == "SifNote")
                        sortedAttrs.Add(new Row { Key = "SifNote=", Value = entry.Value });
                }
                foreach (Row entry in blockData.BlockAttr)
                {
                    if (entry.Key == "REPORTNOTE")
                        sortedAttrs.Add(new Row { Key = "REPORTNOTE=", Value = entry.Value });
                }
                foreach (Row entry in blockData.BlockAttr)
                {
                    if (entry.Key == "PSQP")
                        sortedAttrs.Add(new Row { Key = "QP=", Value = entry.Value });
                }
                foreach (Row entry in blockData.BlockAttr)
                {
                    if (entry.Key == "PSQC")
                        sortedAttrs.Add(new Row { Key = "QC=", Value = entry.Value });
                }
                blockIndex++;
            }

            int partSeparatorIndex = 0;
            FormattedBlocksData fBlockData = new FormattedBlocksData();
            fBlockData.AttValsList = new List<List<Row>>();

            List<List<Row>> formattedBlockData = new List<List<Row>>();

            foreach (Row entry in sortedAttrs)
            {
                if (entry.Key.StartsWith("PN"))
                {
                    if (formattedBlockData.Count > 0)
                        fBlockData.AttValsList.AddRange(formattedBlockData);

                    formattedBlockData = new List<List<Row>>();
                }
                if (entry.Key.StartsWith("PN"))
                {
                    partSeparatorIndex++;
                    formattedBlockData.Add(new List<Row>());
                    formattedBlockData[0].Add(new Row { Key = "PN=", Value = entry.Value });
                }
                else
                    formattedBlockData[0].Add(new Row { Key = entry.Key, Value = entry.Value });

            }

            if (formattedBlockData.Count > 0)
                fBlockData.AttValsList.AddRange(formattedBlockData);



            List<DData> finalOutput = DataFilter(fBlockData.AttValsList);
            foreach (DData d in finalOutput)

            {
                int internalCount = d.index;
                List<Row> blkData = JsonConvert.DeserializeObject<List<Row>>(d.data);
                foreach (Row entry in blkData)
                {
                    if (entry.Key.StartsWith("QT"))
                        s.WriteLine(entry.Key + internalCount);
                    else
                        s.WriteLine(entry.Key + entry.Value);
                }

            }
            s.WriteLine("XX=END OF SIF");
            s.Close();
            f.Close();
        }


        public static List<DData> DataFilter(List<List<Row>> data)
        {
            List<string> indexData = new List<string>();
            List<DData> filterData = new List<DData>();
            foreach (List<Row> item in data)
            {
                string textRow = JsonConvert.SerializeObject(item);
                DData iData = filterData.Find(x => x.data.Equals(textRow));
                int index = filterData.IndexOf(iData);
                if (iData != null)
                {
                    filterData[index].index = filterData[index].index + 1;
                }
                else
                {
                    DData newData = new DData();

                    newData.data = textRow;
                    newData.index = 1;
                    filterData.Add(newData);
                }
            }
            return filterData;
        }

    }



    public class DData
    {
        public int index { get; set; }
        public string data { get; set; }
    }


    public class FormattedBlocksData
    {
        public string PartNumber { get; set; }
        public List<List<Row>> AttValsList { get; set; }
    }

    public class BlockData
    {
        public string ObjectId { get; set; }
        public List<Row> BlockAttr { get; set; }
    }

    public class Row
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
