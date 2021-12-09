using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SifOut
{
    public class Main
    {
        string data, dataBack, oldDataBack;
        string dataIndexField, dataBackIndexField;
        string dataIndex, dataBackIndex;
        string tmp, tmpBack;
        int blkType, numSymTypes;
        long blkSelectionSetIndex;

        int typeOne;
        bool lastTypeOne;
        bool typeOne_Flag;
        int type90, val90;
        int dataOneBlockNum, pmBlockName;

        string dataOneTag1, dataOneMfg, dataOnePartNum, dataOneOn;
        string dataOnePartNum1, dataOnePartNum2;
        string dataOneCR, dataOneSifNote, dataOneReportNote;
        string dataOnePL, dataOneS, dataOneP;


        bool dataOneOnFlag, qCommandFlag, eqcFlag, enhFlag;

        string newOD = "";
        string newODList;
        string newOL = "";
        string newOLList;
        string newOG = "";
        string newOGList = "";
        string newWT = "";
        string newWTList;
        string newVO = "";
        string newVOList = "";

        bool newTag3Flag;


        public void SifTakeOff()
        {
            newTag3Flag = true;
            qCommandFlag = false;
            eqcFlag = false;
            SifTakeOff0();
        }

        public void QuickCount()
        {
            newTag3Flag = true;
            qCommandFlag = true;
            eqcFlag = false;
            SifTakeOff0();
        }

        public void EnhQuickCount()
        {
            newTag3Flag = true;
            qCommandFlag = true;
            eqcFlag = true;
            SifTakeOff0();
        }

        public void SifTakeOff0()
        {
            string sifFileName, sifFilePath;
            string partNumAttribute, qtyAttribute, mfgAttribute, optionAttribute, tag1Attribute, tag2Attribute, descripAttribute;
            string partNum, quty, mfg, options, tag1, tage2, description;

            string[] partNumList, optionList, mfgList, tag1List, tag2List;
            int[] blockType;
            string[] selectionSetIndex_TypeArray;
            int numSymols = 0, numSymTypes = 0, index;
            double[] qtList;
            int index2;

            int selectionSetLength;
            char[] temp, key, attIniPath;
            ResultBuffer cmdEchoOld, val;
            string csTemp;
            int iPNIndex;
            bool bFirstOption;
            char[] chBuffer;
            bool attPromptFound;


        }





        public void PrintList(ResultBuffer First_Rb)
        {
            //ResultBuffer Res_Buf;
            //Res_Buf = First_Rb;
            //TypeOne = 0;
            //typeone_flag = false;
            //LastTypeOne = false;
            //DataOneTag1 = "";
            //DataOneMfg = "";
            //DataOnePartNum = "";
            //DataOnePartNum1 = "";
            //DataOnePartNum2 = "";
            //DataOneCR = "";
            //DataOneSifNote = "";
            //DataOneReportNote = "";
            //DataOneON = "";
            //DataOnePL = "";
            //DataOneS = "";
            //DataOneP = "";
            //Type90 = 0;
            //Type40 = 0;
            //DataOneON_flag = false;
            //while (Res_Buf != null)
            //{
            //    Print_Rb_Data(Res_Buf);
            //    Res_Buf = Res_Buf.
            //}
            //if (LastTypeOne)
            //{
            //    char chOptions[1000];
            //    ACHAR acharOptions[500];
            //    wcscpy_s((wchar_t*)chOptions, sizeof(chOptions), DataOneON);
            //    MultiByteToWideChar(CP_ACP, 0, chOptions, -1, acharOptions, sizeof(acharOptions));
            //}
            //else
            //    DataOneON = "";
            //if (DataOnePartNum1 == "")
            //    DataOnePartNum = DataOnePartNum2;
            //else
            //{
            //    if ((DataOnePartNum1.GetLength() <= 3) && (DataOnePartNum2 != ""))
            //        DataOnePartNum = DataOnePartNum2;
            //    else
            //        DataOnePartNum = DataOnePartNum1;
            //}
        }

        public void Print_Rb_Data(ResultBuffer eb)
        {


        }

        public int DataOneCountOptions(string options)
        {
            int numOptions = 0;

            if (options == "" || options == ".")
                return 0;

            while (options.IndexOf("]") != -1)
            {
                int i0 = options.IndexOf("]");
                options = options.Substring(options.Length - (i0 + 1));
                numOptions++;
            }
            return numOptions;
        }

        public void ConvertToData(string Str)
        {
            string String = Str;

            int i = Str.IndexOf("Enterprise");
            Str = Str.Substring(0, i + 12);

            if (Str == "")
            {
                dataBack = ";;;;;;;;;;;>";
                return;
            }

            dataBack = "";

            string Str1 = "";
            int n = 1;

            while (Str != "")
            {
                for (n = 1; n <= 11; n++)
                {
                    i = Str.IndexOf('\t');
                    Str1 += Str.Substring(0, i);
                    Str1 += ";";
                    Str = Str.Substring(i + 1);
                }
                string Str2 = Str.Substring(0, 1);

                if (Str2 == "\n")
                {
                    Str1 += ">";
                    Str = Str.Substring(1);
                }
                else
                {
                    int it, inn;
                    it = Str.IndexOf("\t");
                    inn = Str.IndexOf('\n');

                    if ((it != -1) && (inn != -1) && (inn < it))
                    {
                        Str1 += Str.Substring(0, inn);
                        Str1 += ">";
                        Str = Str.Substring(inn + 1);
                    }
                    if ((it != -1) && (inn != -1) && (it < inn))
                    {
                        Str1 += Str.Substring(0, it);
                        Str1 += ">";
                        Str = Str.Substring(it + 1);
                    }
                    if ((it == -1) && (inn == -1))
                    {
                        Str1 += Str;
                        Str = "";
                    }
                }
            }
            Str1 += ">";
            dataBack = Str1;
            return;
        }

        public void ConvertToBeforeSortDATA(string Str)
        {
            string myStr = Str;
            string myStrL, myStrR;

            dataBack = "";

            string blkRow;
            string blk = "";

            int ind;

            for (ind = 1; ind <= numSymTypes; ind++)
            {
                myStr = Str;

                blkRow = "BLK~" + ind;
                blk = blkRow;

                int i = myStr.IndexOf(blk);

                if (i != -1)
                {
                    myStrL = myStr.Substring(0, i);
                    myStrR = myStr.Substring(i);

                    int ii = myStrL.LastIndexOf('>');
                    if (ii != -1)
                    {
                        myStrL = myStrL.Substring(ii + 1);
                    }

                    int iii = myStrR.IndexOf('>');
                    if (iii != -1)
                    {
                        myStrR = myStrR.Substring(0, iii + 1);
                    }

                    dataBack += myStrL;
                    dataBack += myStrR;
                }
            }
        }

        public bool CompareData()
        {
            string S1 = data;
            string S2 = dataBack;

            string S1last = S1.Substring(S1.Length - 1);
            string S2last = S2.Substring(S2.Length - 1);

            if ((S1last == ">") && (S2last != ">"))
            {
                data = data.Substring(data.Length - 1);
            }
            if (data == dataBack)
                return true;
            else
                return false;
        }

        public bool CompareDataIndex(int ind3)
        {
            string dataLocal = data;
            string databackLocal = dataBack;

            int k = 0;
            int i = 0;
            int ib = 0;

            if (ind3 > 0)
            {
                for (k = 0; k < ind3; k++)
                {
                    i = dataLocal.IndexOf(">");
                    dataLocal = dataLocal.Substring(i + 1);

                    ib = databackLocal.IndexOf(">");
                    databackLocal = databackLocal.Substring(ib + 1);
                }
            }

            i = dataLocal.IndexOf('>');
            if (i != -1)
                dataLocal = dataLocal.Substring(i);


            ib = databackLocal.IndexOf('>');
            if (ib != -1)
                databackLocal = databackLocal.Substring(ib);

            dataIndex = dataLocal;
            dataBackIndex = databackLocal;

            if (dataIndex == dataBackIndex)
                return true;
            else
                return false;
        }

        public int GetNumBlks(string tmpStr)
        {
            string tmp = tmpStr;
            int n = 0, i = 0;
            while (i != -1)
            {
                i = tmp.IndexOf("-");
                if (i != -1)
                {
                    n++;
                    tmp = tmp.Substring(i + 1);
                }
            }
            return n;
        }

        public void GetBlkInfo(string tmpStr, int ind3)
        {
            int k = 0;
            string tmp = tmpStr;
            if (ind3 > 0)
            {
                for (k = 1; k < ind3; k++)
                {
                    tmp = tmp.Substring(0, tmp.IndexOf(",") + 1);
                }
            }
            string tmpBlkType = tmp.Substring(0, tmp.IndexOf("-"));
            string tmpBlkSelSetIndex = tmp.Substring(tmp.IndexOf("-") + 1);
            int i = tmpBlkSelSetIndex.IndexOf(",");
            if (i != -1)
            {
                tmpBlkSelSetIndex = tmpBlkSelSetIndex.Substring(0, i);
            }
            string buf;
            buf = tmpBlkType;
            blkType = Convert.ToInt32(buf);
            buf = buf + tmpBlkSelSetIndex;
            blkSelectionSetIndex = Convert.ToInt64(buf);
        }

        public void GetDataFiled(int field)
        {
            if (field == 1) //PN
            {
                dataIndexField = dataIndex.Substring(0, dataIndex.IndexOf(";"));
                dataBackIndexField = dataBackIndex.Substring(0, dataBackIndex.IndexOf(";"));
                return;
            }
            if (field == 11) //Enterprise
            {
                tmp = dataIndex.Substring(0, dataIndex.LastIndexOf(";"));
                tmpBack = dataBackIndex.Substring(0, dataBackIndex.LastIndexOf(";"));

                dataIndexField = tmp.Substring(tmp.Length - tmp.LastIndexOf(";") + 1);
                dataBackIndexField = tmp.Substring(tmpBack.Length - tmpBack.LastIndexOf(";") + 1);
                return;
            }

            int i;
            string di, dbi;
            di = dataIndex;
            dbi = dataBackIndex;
            for (i = 1; i < field; i++)
            {
                di = di.Substring(di.IndexOf(";") + 1);
                dbi = dbi.Substring(dbi.IndexOf(";") + 1);
            }
            dataIndexField = di.Substring(0, di.IndexOf(";"));
            dataBackIndexField = dbi.Substring(0, dbi.IndexOf(";"));
        }
    }
}
