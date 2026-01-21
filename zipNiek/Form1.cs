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
            byte[] blist = s.getBytes();
            uint[] freq = s.getFreq(blist);

            DLL myL = new DLL();

            for (int i = 0; i < freq.Length; i++)
            {
                if (freq[i] != 0)
                {
                    byte b = (byte)i;
                    node n = new node(b, freq[i]);
                    myL.insertNode(n);
                }
            }

            myL.combineNodes();
            myL.getPaths("", myL.T);

            string bytestring = s.savePaths(blist, myL.paths);
            byte[] blist2 = s.bitToByte(bytestring);

            string treestring = s.translateTree(myL.T);
            byte[] treeBytes = s.bitToByteTree(treestring);

            byte[] combinedBytes = new byte[treeBytes.Length + blist2.Length];

            for (int i = 0; i < treeBytes.Length; i++)
            {
                combinedBytes[i] = treeBytes[i];
            }

            int j = 0;

            for (int i = treeBytes.Length; i < blist2.Length + treeBytes.Length ; i++)
            {
                combinedBytes[i] = blist2[j];
                j++;
            }

            string basePath = AppContext.BaseDirectory;
            string filesDir = Path.Combine(basePath, "files");

            Directory.CreateDirectory(filesDir);

            string filePath = Path.Combine(filesDir, "output.nzip");

            File.WriteAllBytes(filePath, combinedBytes);
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

            string endstring = s.getBits(encdata);
            endstring = s.translateBits(endstring, t);

            string basePath = AppContext.BaseDirectory;
            string filesDir = Path.Combine(basePath, "files");

            Directory.CreateDirectory(filesDir);

            string filePath = Path.Combine(filesDir, "unzipped.txt");
            File.WriteAllText(filePath, endstring);
        }
    }
}
