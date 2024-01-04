namespace IresoftApplication.UserControls
{
    partial class UC_ProgressBar
    {
        /// <summary> 
        /// Vyžaduje se proměnná návrháře.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Uvolněte všechny používané prostředky.
        /// </summary>
        /// <param name="disposing">hodnota true, když by se měl spravovaný prostředek odstranit; jinak false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kód vygenerovaný pomocí Návrháře komponent

        /// <summary> 
        /// Metoda vyžadovaná pro podporu Návrháře - neupravovat
        /// obsah této metody v editoru kódu.
        /// </summary>
        private void InitializeComponent()
        {
            progressBar1 = new ProgressBar();
            lbl_operace = new Label();
            label_currentV = new Label();
            label3 = new Label();
            label_maxV = new Label();
            button_storno = new Button();
            SuspendLayout();
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(16, 42);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(299, 23);
            progressBar1.TabIndex = 0;
            // 
            // lbl_operace
            // 
            lbl_operace.AutoSize = true;
            lbl_operace.Location = new Point(146, 10);
            lbl_operace.Name = "lbl_operace";
            lbl_operace.Size = new Size(51, 15);
            lbl_operace.TabIndex = 1;
            lbl_operace.Text = "Operace";
            // 
            // label_currentV
            // 
            label_currentV.AutoSize = true;
            label_currentV.Font = new Font("Times New Roman", 9F, FontStyle.Bold);
            label_currentV.Location = new Point(16, 81);
            label_currentV.Name = "label_currentV";
            label_currentV.Size = new Size(13, 15);
            label_currentV.TabIndex = 2;
            label_currentV.Text = "0";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Times New Roman", 9F, FontStyle.Bold);
            label3.Location = new Point(168, 81);
            label3.Name = "label3";
            label3.Size = new Size(10, 15);
            label3.TabIndex = 3;
            label3.Text = "/";
            // 
            // label_maxV
            // 
            label_maxV.AutoSize = true;
            label_maxV.Font = new Font("Times New Roman", 9F, FontStyle.Bold);
            label_maxV.Location = new Point(302, 81);
            label_maxV.Name = "label_maxV";
            label_maxV.Size = new Size(13, 15);
            label_maxV.TabIndex = 4;
            label_maxV.Text = "0";
            label_maxV.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // button_storno
            // 
            button_storno.Location = new Point(81, 126);
            button_storno.Name = "button_storno";
            button_storno.Size = new Size(172, 28);
            button_storno.TabIndex = 5;
            button_storno.Text = "Storno";
            button_storno.UseVisualStyleBackColor = true;
            button_storno.Click += button_storno_Click;
            // 
            // UC_ProgressBar
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(button_storno);
            Controls.Add(label_maxV);
            Controls.Add(label3);
            Controls.Add(label_currentV);
            Controls.Add(lbl_operace);
            Controls.Add(progressBar1);
            Name = "UC_ProgressBar";
            Size = new Size(338, 182);
            Load += UC_ProgressBar_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ProgressBar progressBar1;
        private Label lbl_operace;
        private Label label_currentV;
        private Label label3;
        private Label label_maxV;
        private Button button_storno;
    }
}
