namespace ContentGeneratorApp
{
    partial class MainForm
    {
        private System.Windows.Forms.TextBox textBoxKeyword;
        private System.Windows.Forms.ComboBox comboBoxStyle;
        private System.Windows.Forms.TextBox textBoxImagePath;
        private System.Windows.Forms.Button buttonBrowseImage;
        private System.Windows.Forms.Button buttonGenerateContent;
        private System.Windows.Forms.TextBox textBoxOutput;
        private System.Windows.Forms.Label labelKeyword;
        private System.Windows.Forms.Label labelStyle;
        private System.Windows.Forms.Label labelImagePath;

        private void InitializeComponent()
        {
            // 创建控件
            this.textBoxKeyword = new System.Windows.Forms.TextBox();
            this.comboBoxStyle = new System.Windows.Forms.ComboBox();
            this.textBoxImagePath = new System.Windows.Forms.TextBox();
            this.buttonBrowseImage = new System.Windows.Forms.Button();
            this.buttonGenerateContent = new System.Windows.Forms.Button();
            this.textBoxOutput = new System.Windows.Forms.TextBox();

            // 创建标签控件
            this.labelKeyword = new System.Windows.Forms.Label();
            this.labelStyle = new System.Windows.Forms.Label();
            this.labelImagePath = new System.Windows.Forms.Label();

            // 设置控件属性
            this.textBoxKeyword.Location = new System.Drawing.Point(120, 20);
            this.textBoxKeyword.Size = new System.Drawing.Size(200, 30);

            this.comboBoxStyle.Location = new System.Drawing.Point(120, 60);
            this.comboBoxStyle.Size = new System.Drawing.Size(200, 30);

            this.textBoxImagePath.Location = new System.Drawing.Point(120, 100);
            this.textBoxImagePath.Size = new System.Drawing.Size(200, 30);

            this.buttonBrowseImage.Location = new System.Drawing.Point(330, 100);
            this.buttonBrowseImage.Text = "浏览图片";
            this.buttonBrowseImage.Size = new System.Drawing.Size(100, 30);
            this.buttonBrowseImage.Click += new EventHandler(buttonBrowseImage_Click);

            this.buttonGenerateContent.Location = new System.Drawing.Point(20, 140);
            this.buttonGenerateContent.Text = "生成文案";
            this.buttonGenerateContent.Size = new System.Drawing.Size(100, 30);
            this.buttonGenerateContent.Click += new EventHandler(buttonGenerateContent_Click);

            this.textBoxOutput.Location = new System.Drawing.Point(20, 180);
            this.textBoxOutput.Size = new System.Drawing.Size(400, 200);
            this.textBoxOutput.Multiline = true;

            // 设置标签属性
            this.labelKeyword.Text = "关键字:";
            this.labelKeyword.Location = new System.Drawing.Point(20, 20);
            this.labelKeyword.Size = new System.Drawing.Size(80, 30);

            this.labelStyle.Text = "风格:";
            this.labelStyle.Location = new System.Drawing.Point(20, 60);
            this.labelStyle.Size = new System.Drawing.Size(80, 30);

            this.labelImagePath.Text = "图片路径:";
            this.labelImagePath.Location = new System.Drawing.Point(20, 100);
            this.labelImagePath.Size = new System.Drawing.Size(100, 30);

            // 将控件添加到窗体
            this.Controls.Add(this.textBoxKeyword);
            this.Controls.Add(this.comboBoxStyle);
            this.Controls.Add(this.textBoxImagePath);
            this.Controls.Add(this.buttonBrowseImage);
            this.Controls.Add(this.buttonGenerateContent);
            this.Controls.Add(this.textBoxOutput);

            // 将标签添加到窗体
            this.Controls.Add(this.labelKeyword);
            this.Controls.Add(this.labelStyle);
            this.Controls.Add(this.labelImagePath);
        }
    }
}
