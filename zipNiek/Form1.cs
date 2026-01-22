using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zipNiek
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnZip_Click(object sender, EventArgs e)
        {
            byte[] barr = s.getBytes();
            uint[] freq = s.getFreq(barr);

            DLL myL = s.fillDll(freq);

            myL.combineNodes();
            myL.getPaths("", myL.T);

            byte[] pathArr = s.savePaths(barr, myL.paths);

            string treestring = s.translateTree(myL.T);
            byte[] treeBytes = s.bitToByteTree(treestring);

            byte[] combinedBytes = s.combineBytes(treeBytes, pathArr);

            s.saveZipFile(combinedBytes);
        }

        private void btnUnzip_Click(object sender, EventArgs e)
        {
            byte[] blist = s.getBytes();
            string bstring = s.translateTreeBytes(blist);
           
            treeNode t = s.generateTree(bstring);

            byte[] encdata = new byte[blist.Length - s.pos / 8];

            for (int i = 0; i < encdata.Length; i++)
            {
                encdata[i] = blist[i + s.pos / 8];
            }

            byte[] endbytes = s.getBits(encdata, t);

            s.saveFile(endbytes);
        }
    }
}
