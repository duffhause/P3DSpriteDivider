using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace P3DSpriteDivider
{
    public partial class Form1 : Form
    {
        P3D p = new P3D();

        public Form1()
        {
            InitializeComponent();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
                    }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.Cancel)
            {
                treeView1.Nodes.Clear();
                p.ReadP3D(openFileDialog1.FileName);

                int i = 0;
                int j = 0;
                foreach (P3D.Chunk chunk in p.Root.subChunks)
                {
                    if (chunk.ChunkType.SequenceEqual(P3D.chunkOrder[3]))
                    {
                        treeView1.Nodes.Add(p.GetSpriteName(chunk));
                        treeView1.Nodes[j].Tag = (object)chunk;

                        P3D.SpriteDataChunk sprite = P3D.GetSpriteData(chunk);
                        if (sprite.ImageCount > 1)
                        {
                            treeView1.Nodes[j].BackColor = Color.Green;
                        } else
                        {
                            treeView1.Nodes[j].BackColor = Color.Red;
                        }
                        j++;

                    }
                    i++;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null)
            {
                MessageBox.Show("No sprite is selected!");
                return;
            }
            else if (treeView1.SelectedNode.BackColor == Color.Green)
            {
                MessageBox.Show("Sprite is already divided!");
                return;
            }
            P3D.Chunk chunk = (P3D.Chunk)treeView1.SelectedNode.Tag;
            chunk = p.DivideSprite(chunk);
            p.Root.subChunks[chunk.Index] = chunk;

            treeView1.SelectedNode.BackColor = Color.Green;
            
        }

        private void treeView1_AfterSelect_1(object sender, TreeViewEventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            p.WriteP3D(openFileDialog1.FileName);
        }
    }
}
