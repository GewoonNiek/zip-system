using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zipNiek
{
    internal class s
    {
        public static int pos = 0;

        /// <summary>
        /// Read a file and return the bytes
        /// </summary>
        /// <algo>
        /// Pick a file
        /// return the text in the file converted into bytes
        /// </algo>
        internal static byte[] getBytes()
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                return File.ReadAllBytes(ofd.FileName);
            }

            return null;
        }

        /// <summary>
        /// Check how many times the byte is found in the array
        /// </summary>
        /// <algo>
        /// Check for each possible byte how many times it exists in the bytearray
        /// for each byte found, increase the amount in a uint array
        /// </algo>
        internal static uint[] getFreq(byte[] blist)
        {
            uint[] res = new uint[256];

            for (int i = 0; i < blist.Length; i++) res[blist[i]]++;

            return res;
        }

        /// <summary>
        /// Save the paths to leaves in a bytearray
        /// </summary>
        /// <algo>
        /// check each byte in arr what the bytevalue is
        /// check if that bytevalue exists in the table
        /// if so write each character as a byte in the pathstring from the table to a byte array
        /// 
        /// remove all excess space from the byte[]
        /// 
        /// convert bits into bytes
        /// </algo>
        internal static byte[] savePaths(byte[] arr, List<Tuple<byte, string>> l)
        {
            byte[] temp = new byte[8 * arr.Length];
            int i = 0;

            foreach (byte b in arr)
            {
                foreach (var t in l)
                {
                    if (t.Item1 == b)
                    {
                        string s = t.Item2;
                        foreach (char c in s)
                        {
                            temp[i] = (byte)(c-'0');
                            i++;
                        }
                    }
                }
            }

            int m = (8 - (i % 8)) % 8;
            byte[] temp2 = new byte[i + m];

            for (int j = 0; j < i; j++)
            {
                temp2[j] = temp[j];
            }

            for (int j = i; j < i + m; j++)
            {
                temp2[j] = 0;
            }

            byte[] output = new byte[(temp2.Length / 8) + 1];

            int k = 0;

            for (int n = 0; n < temp2.Length; n += 8)
            {
                int value = 0;
                int weight = 128;

                for (int x = 0; x < 8; x++)
                {
                    if (temp2[n + x] == 1) value += weight;
                    weight /= 2;
                }

                output[k] = (byte)value;
                k++;
            }

            output[output.Length - 1] = (byte)m;

            return output;
        }

        /// <summary>
        /// Translate treebits into bytes
        /// </summary>
        /// <algo>
        /// Check if s can be divided by 8
        /// if not add remaining zero's
        /// 
        /// Substring s in parts of 8 characters
        /// translate characters into byte
        /// put result byte into res[]
        /// 
        /// add last byte with the value of the added zero's
        /// </algo>
        internal static byte[] bitToByteTree(string s)
        {
            int m = 0;
            if (s.Length % 8 != 0)
            {
                m = 8 - (s.Length % 8);
                for (int k = 0; k < m; k++)
                {
                    s += "0";
                }
            }

            byte[] res = new byte[s.Length / 8];

            int j = 0;

            for (int i = 0; i < s.Length; i += 8)
            {
                int w = 128;
                int r = 0;
                string t = s.Substring(i, 8);
                foreach (char c in t)
                {
                    if (c == '1') r += w;
                    w /= 2;
                }
                res[j] = (byte)r;
                j++;
            } 
            return res;
        }

        /// <summary>
        /// Walk through the tree and add relevant information of a node to a string
        /// </summary>
        /// <algo>
        /// Start at the top of the tree (tail)
        /// walk down the tree, always first left then right
        /// 
        /// if you hit a leaf, save relevant data
        /// if you hit a non-leaf, recurse to left and right
        /// </algo>
        internal static string translateTree(node n)
        {
            string res = "";

            if (n.L == null && n.R == null)
            {
                res += "1";
                res += Convert.ToString(n.b, 2).PadLeft(8, '0');
            }
            else
            {
                res += "0";
                res += translateTree(n.L);
                res += translateTree(n.R);
            }

            return res;
        }

        /// <summary>
        /// Translate treebytes into bits
        /// </summary>
        /// <algo>
        /// for each byte in the bytearray, translate it into bitcode
        /// return the translated string
        /// </algo>
        internal static string translateTreeBytes(byte[] blist)
        {
            string res = "";

            foreach (byte b in blist) res += getBitcode(b);

            return res;
        }

        /// <summary>
        /// return normal bitcode of a byte
        /// </summary>
        /// <algo>
        /// Calculate the bitcode of a byte
        /// check if bytevalue is dividable by w
        /// 
        /// if so, add 1 to string, else 0
        /// 
        /// divide w by 2 and repeat
        /// </algo>
        private static string getBitcode(byte b)
        {
            string s = "";

            int B = b;
            int w =128;

            for (int i = 0; i < 8; i++) {
                int res = B / w;

                if (res > 0) 
                {
                    s += '1';
                    B -= w;
                }
                else 
                {
                    s += '0'; 
                }

                w /= 2;
            }
            return s;
        }

        /// <summary>
        /// generate tree using bitstring
        /// </summary>
        /// <algo>
        /// Make the first node
        /// recurse down, starting from top node down using s
        /// </algo>
        internal static treeNode generateTree(string s)
        {
            pos = 0;

            treeNode top = recGenerate(s);

            int rest = 8-pos % 8;
            pos += rest;

            return top;
        }

        /// <summary>
        /// Recursive function to generate a tree
        /// </summary>
        /// <algo>
        /// Check if current position in the string is a 1 or 0
        /// 
        /// if its a 1, consume next 8 bits and translate into byte
        /// if its a 0, recurse to the left, then to the right
        /// 
        /// return first treenode
        /// </algo>
        private static treeNode recGenerate(string s)
        {
            treeNode t = new treeNode(0);
            if (s[pos++] == '1')
            {
                t.b = Convert.ToByte(s.Substring(pos, 8), 2);
                pos += 8;
            }
            else
            {
                t.L = recGenerate(s);
                t.R = recGenerate(s);
            }
            return t;
        }

        /// <summary>
        /// translate bytes into bits
        /// </summary>
        /// <algo>
        /// get the amount of bits added, this is the bytevalue of the last byte in the array
        /// make a big enough array
        /// fill the array with the bitcode of each byte
        /// recurse through the tree to find leaves, put bitcode of found byte in array
        /// 
        /// get rid of excess empty fields in array
        /// </algo>
        internal static byte[] getBits(byte[] arr, treeNode n)
        {
            byte[] temp = new byte[arr.Length * 8];
            int m = arr[arr.Length - 1];
            int j = 0;

            for (int i = 0; i < arr.Length - 1; i++)
            {
                string tmp = getBitcode(arr[i]);

                foreach (char c in tmp)
                {
                    temp[j++] = (byte)(c - '0');
                }
            }

            treeNode start = n;
            byte[] temp2 = new byte[temp.Length];
            int oi = 0;
            int bitAmt = ((arr.Length - 1) * 8) - m;

            for (int i = 0; i < bitAmt; i++)
            {
                if (temp[i] == 1)
                {
                    n = n.L;
                }
                else
                {
                    n = n.R;
                }

                if (n.L == null && n.R == null)
                {
                    temp2[oi++] = n.b;
                    n = start;
                }
            }

            byte[] res = new byte[oi];
            Array.Copy(temp2, res, oi);

            return res;
        }

        /// <summary>
        /// Fill a DLL with nodes consisting of bytes in freq[]
        /// </summary>
        /// <algo>
        /// for each frequency found make a node
        /// add frequency value to the node & byte value
        /// store the node in the DLL
        /// </algo>
        internal static DLL fillDll(uint[] freq)
        {
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

            return myL;
        }

        /// <summary>
        /// Combine 2 byte[] into one
        /// </summary>
        /// <algo>
        /// create a new array with the lenght of the 2 lists
        /// add each byte to the combinedArray
        /// </algo>
        internal static byte[] combineBytes(byte[] treeBytes, byte[] blist2)
        {
            byte[] cArr = new byte[treeBytes.Length + blist2.Length];

            for (int i = 0; i < treeBytes.Length; i++)
            {
                cArr[i] = treeBytes[i];
            }

            int j = 0;

            for (int i = treeBytes.Length; i < blist2.Length + treeBytes.Length; i++)
            {
                cArr[i] = blist2[j];
                j++;
            }

            return cArr;
        }

        /// <summary>
        /// Saves a file to a specified place
        /// </summary>
        /// <algo>
        /// Select a directory where to save the file
        /// Write bytes in arr to file
        /// </algo>
        internal static void saveZipFile(byte[] arr)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                string filesDir = fbd.SelectedPath;

                string filePath = Path.Combine(filesDir, "output.nzip");

                File.WriteAllBytes(filePath, arr);
            }
        }

        /// <summary>
        /// Saves a file to a specified place
        /// </summary>
        /// <algo>
        /// Select a directory where to save the file
        /// Write bytes in arr to file
        /// </algo>
        internal static void saveFile(byte[] arr)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                string filesDir = fbd.SelectedPath;

                string filePath = Path.Combine(filesDir, "unzipped");

                File.WriteAllBytes(filePath, arr);
            }
        }
    }

    internal class treeNode
    {
        public byte b;
        public treeNode L, R;

        public treeNode (byte b)
        {
            this.b = b;
        }
    }

    internal class node
    {
        public byte b;
        public uint f;
        public node P, N, L, R;

        public node(byte b, uint f)
        {
            this.b = b;
            this.f = f;
        }
    }

    internal class DLL
    {
        public node H, T;
        public List<Tuple<byte, string>> paths = new List<Tuple<byte, string>>();

        /// <summary>
        /// insert a node in the dll
        /// </summary>
        /// <algo>
        /// check if head exists 
        /// if not n becomes h & t
        /// 
        /// check if n.f smaller than h.f
        /// if so n becomes head
        /// 
        /// check if n.f bigger than T.f
        /// if so n becomes tail
        /// 
        /// else loop through list till n.f bigger than current.f and smaller than current.next.f
        /// insert node
        /// </algo>
        public void insertNode(node n)
        {
            if (H == null)
            {
                H = T = n;
                return;
            }

            if (n.f <= H.f)
            {
                n.N = H;
                H.P = n;
                H = n;
                return;
            }

            if (n.f > T.f)
            {
                n.P = T;
                T.N = n;
                T = n;
                return;
            }

            node current = H;

            while (n.f > current.f) current = current.N;

            node temp = current.P;

            current.P = n;
            n.N = current;
            n.P = temp;
            temp.N = n;
        }

        /// <summary>
        /// Combine nodes into a new node and insert it in the list based on frequency
        /// </summary>
        /// <algo>
        /// Combine current.f and current.next.f, create a new node with it
        /// insert the node into the dll
        /// repeat untill there is no current.N;
        /// </algo>
        public void combineNodes()
        {
            node current = H;

            while (current.N != null)
            {
                uint newF = current.f + current.N.f;

                byte b = 0;
                node n = new node(b, newF);
                n.L = current;
                n.R = current.N;
                insertNode(n);

                current = current.N.N;
            }
        }

        /// <summary>
        /// Get the paths of how to get to leaves, store them in a table
        /// </summary>
        /// <algo>
        /// Check if theres a left or right
        /// if there is no left and right add path to list
        /// if there is a left and right, recurse with the found node
        /// 
        /// for left add 1 to the string
        /// for right add 0 to the string
        /// </algo>
        public void getPaths(string s, node n)
        {
            string res = s;

            if(n.L == null && n.R == null) paths.Add(Tuple.Create(n.b, res));

            if (n.L != null) getPaths(res + "1", n.L);
            if (n.R != null) getPaths(res + "0", n.R);
        }

    }

}
