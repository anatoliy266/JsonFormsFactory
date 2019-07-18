using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JsonFormsFactory
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Debug.WriteLine("here: create Form1");
            Label insertJsonLabel = new Label();
            insertJsonLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            insertJsonLabel.Text = "Insert json below";

            insertJsonLabel.Parent = this;

            RichTextBox jsonTextSpace = new RichTextBox();
            jsonTextSpace.Anchor = AnchorStyles.Top;
            jsonTextSpace.Top = insertJsonLabel.Bottom;
            jsonTextSpace.Width = this.Width / 4;
            jsonTextSpace.Height = this.Height / 5;

            jsonTextSpace.Parent = this;

            Button acceptButton = new Button();
            acceptButton.Anchor = AnchorStyles.Left;
            acceptButton.Top = jsonTextSpace.Bottom;
            acceptButton.Text = "Generate";

            acceptButton.Parent = this;
            acceptButton.MouseClick += new MouseEventHandler(onAcceptButtonClick);

            
            this.Update();
        }

        public void onAcceptButtonClick(object sender, MouseEventArgs e)
        {

            Debug.WriteLine("here: MouseClick handled");
            FormsFactory ff = new FormsFactory();
            Form genForm = ff.Generate();
            //genForm.ShowDialog();
            genForm.ShowDialog();
        }
    }
}
