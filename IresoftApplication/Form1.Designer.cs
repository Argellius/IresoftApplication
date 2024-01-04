namespace IresoftApplication
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label_pocet_vet = new Label();
            label_pocet_slov = new Label();
            label_pocet_znaku = new Label();
            label_pocet_radku = new Label();
            textBox_pocet_vet = new TextBox();
            textBox_pocet_slov = new TextBox();
            textBox_pocet_znaku = new TextBox();
            textBox_pocet_radku = new TextBox();
            openFileDialog = new OpenFileDialog();
            saveFileDialog = new SaveFileDialog();
            button_load = new Button();
            button_save = new Button();
            button_copy = new Button();
            button_diacritic = new Button();
            button_blank_line = new Button();
            button_white_punc = new Button();
            uC_ProgressBar1 = new UserControls.UC_ProgressBar();
            SuspendLayout();
            // 
            // label_pocet_vet
            // 
            label_pocet_vet.AutoSize = true;
            label_pocet_vet.Location = new Point(16, 20);
            label_pocet_vet.Margin = new Padding(2, 0, 2, 0);
            label_pocet_vet.Name = "label_pocet_vet";
            label_pocet_vet.Size = new Size(56, 15);
            label_pocet_vet.TabIndex = 0;
            label_pocet_vet.Text = "Počet vět";
            // 
            // label_pocet_slov
            // 
            label_pocet_slov.AutoSize = true;
            label_pocet_slov.Location = new Point(115, 20);
            label_pocet_slov.Margin = new Padding(2, 0, 2, 0);
            label_pocet_slov.Name = "label_pocet_slov";
            label_pocet_slov.Size = new Size(61, 15);
            label_pocet_slov.TabIndex = 1;
            label_pocet_slov.Text = "Počet slov";
            // 
            // label_pocet_znaku
            // 
            label_pocet_znaku.AutoSize = true;
            label_pocet_znaku.Location = new Point(214, 20);
            label_pocet_znaku.Margin = new Padding(2, 0, 2, 0);
            label_pocet_znaku.Name = "label_pocet_znaku";
            label_pocet_znaku.Size = new Size(71, 15);
            label_pocet_znaku.TabIndex = 2;
            label_pocet_znaku.Text = "Počet znaků";
            // 
            // label_pocet_radku
            // 
            label_pocet_radku.AutoSize = true;
            label_pocet_radku.Location = new Point(311, 20);
            label_pocet_radku.Margin = new Padding(2, 0, 2, 0);
            label_pocet_radku.Name = "label_pocet_radku";
            label_pocet_radku.Size = new Size(70, 15);
            label_pocet_radku.TabIndex = 3;
            label_pocet_radku.Text = "Počet řádků";
            // 
            // textBox_pocet_vet
            // 
            textBox_pocet_vet.Enabled = false;
            textBox_pocet_vet.Location = new Point(18, 45);
            textBox_pocet_vet.Margin = new Padding(2);
            textBox_pocet_vet.Name = "textBox_pocet_vet";
            textBox_pocet_vet.Size = new Size(76, 23);
            textBox_pocet_vet.TabIndex = 4;
            // 
            // textBox_pocet_slov
            // 
            textBox_pocet_slov.Enabled = false;
            textBox_pocet_slov.Location = new Point(117, 45);
            textBox_pocet_slov.Margin = new Padding(2);
            textBox_pocet_slov.Name = "textBox_pocet_slov";
            textBox_pocet_slov.Size = new Size(76, 23);
            textBox_pocet_slov.TabIndex = 5;
            // 
            // textBox_pocet_znaku
            // 
            textBox_pocet_znaku.Enabled = false;
            textBox_pocet_znaku.Location = new Point(216, 45);
            textBox_pocet_znaku.Margin = new Padding(2);
            textBox_pocet_znaku.Name = "textBox_pocet_znaku";
            textBox_pocet_znaku.Size = new Size(76, 23);
            textBox_pocet_znaku.TabIndex = 6;
            // 
            // textBox_pocet_radku
            // 
            textBox_pocet_radku.Enabled = false;
            textBox_pocet_radku.Location = new Point(314, 45);
            textBox_pocet_radku.Margin = new Padding(2);
            textBox_pocet_radku.Name = "textBox_pocet_radku";
            textBox_pocet_radku.Size = new Size(76, 23);
            textBox_pocet_radku.TabIndex = 7;
            // 
            // openFileDialog
            // 
            openFileDialog.FileName = "openFileDialog1";
            // 
            // button_load
            // 
            button_load.Location = new Point(18, 118);
            button_load.Margin = new Padding(2);
            button_load.Name = "button_load";
            button_load.Size = new Size(174, 30);
            button_load.TabIndex = 8;
            button_load.Text = "Načíst";
            button_load.UseVisualStyleBackColor = true;
            button_load.Click += button_load_Click;
            // 
            // button_save
            // 
            button_save.Location = new Point(216, 118);
            button_save.Margin = new Padding(2);
            button_save.Name = "button_save";
            button_save.Size = new Size(172, 30);
            button_save.TabIndex = 9;
            button_save.Text = "Cílová cesta";
            button_save.UseVisualStyleBackColor = true;
            button_save.Click += button_save_Click;
            // 
            // button_copy
            // 
            button_copy.Location = new Point(18, 195);
            button_copy.Margin = new Padding(2);
            button_copy.Name = "button_copy";
            button_copy.Size = new Size(174, 28);
            button_copy.TabIndex = 10;
            button_copy.Text = "Překopíruj";
            button_copy.UseVisualStyleBackColor = true;
            button_copy.Click += button_copy_Click;
            // 
            // button_diacritic
            // 
            button_diacritic.Location = new Point(18, 243);
            button_diacritic.Margin = new Padding(2);
            button_diacritic.Name = "button_diacritic";
            button_diacritic.Size = new Size(174, 39);
            button_diacritic.TabIndex = 11;
            button_diacritic.Text = "Odstranit diakritiku";
            button_diacritic.UseVisualStyleBackColor = true;
            button_diacritic.Click += button_diacritic_Click;
            // 
            // button_blank_line
            // 
            button_blank_line.Location = new Point(222, 195);
            button_blank_line.Margin = new Padding(2);
            button_blank_line.Name = "button_blank_line";
            button_blank_line.Size = new Size(174, 28);
            button_blank_line.TabIndex = 12;
            button_blank_line.Text = "Odstranit prázdné řádky";
            button_blank_line.UseVisualStyleBackColor = true;
            button_blank_line.Click += button_blank_lines_Click;
            // 
            // button_white_punc
            // 
            button_white_punc.Location = new Point(222, 243);
            button_white_punc.Margin = new Padding(2);
            button_white_punc.Name = "button_white_punc";
            button_white_punc.Size = new Size(174, 39);
            button_white_punc.TabIndex = 13;
            button_white_punc.Text = "Odstranit mezery a interpunkční znaménka";
            button_white_punc.UseVisualStyleBackColor = true;
            button_white_punc.Click += button_white_punc_Click;
            // 
            // uC_ProgressBar1
            // 
            uC_ProgressBar1.Location = new Point(42, 73);
            uC_ProgressBar1.Name = "uC_ProgressBar1";
            uC_ProgressBar1.Size = new Size(330, 174);
            uC_ProgressBar1.TabIndex = 14;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(405, 366);
            Controls.Add(button_white_punc);
            Controls.Add(button_blank_line);
            Controls.Add(button_diacritic);
            Controls.Add(button_copy);
            Controls.Add(button_save);
            Controls.Add(button_load);
            Controls.Add(textBox_pocet_radku);
            Controls.Add(textBox_pocet_znaku);
            Controls.Add(textBox_pocet_slov);
            Controls.Add(textBox_pocet_vet);
            Controls.Add(label_pocet_radku);
            Controls.Add(label_pocet_znaku);
            Controls.Add(label_pocet_slov);
            Controls.Add(label_pocet_vet);
            Controls.Add(uC_ProgressBar1);
            Margin = new Padding(2);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label_pocet_vet;
        private System.Windows.Forms.Label label_pocet_slov;
        private System.Windows.Forms.Label label_pocet_znaku;
        private System.Windows.Forms.Label label_pocet_radku;
        private System.Windows.Forms.TextBox textBox_pocet_vet;
        private System.Windows.Forms.TextBox textBox_pocet_slov;
        private System.Windows.Forms.TextBox textBox_pocet_znaku;
        private System.Windows.Forms.TextBox textBox_pocet_radku;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Button button_load;
        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.Button button_copy;
        private System.Windows.Forms.Button button_diacritic;
        private System.Windows.Forms.Button button_blank_line;
        private System.Windows.Forms.Button button_white_punc;
        private ProgressBar progressBar1;
        private UserControls.UC_ProgressBar uC_ProgressBar1;
    }
}
