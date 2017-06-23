// Decompiled with JetBrains decompiler
// Type: isolateCPU.isolateCPU
// Assembly: isolateCPU, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EE39AFBC-1B67-4B84-B415-DFA0EEFD39E6
// Assembly location: C:\Users\Dark\Documents\Visual Studio 2015\Projects\isolateCPU\isolateCPU\bin\Release\isolateCPU.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace isolateCPU
{
  public class isolateCPU : Form
  {
    private Process[] processlist = Process.GetProcesses();
    public string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
    public int cpu;
    public int cpucore;
    public int topro;
    public string[] exclude;
    public string problemkid;
    public string problemkidid;
    private IContainer components;
    private ListBox listBox1;
    private ListBox listBox2;
    private Label label1;
    private Label label2;
    private Button button1;
    private Label label3;
    private Label status;
    private Label label4;
    private Label isolatednumber;
    private Button toright;
    private Button toleft;
    private Button release;
    private Label label5;
    private Label label6;
    private Button excludedlist;
    private Timer timer1;

    public isolateCPU()
    {
      this.InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      if (!File.Exists(this.path + "\\exclude.txt"))
      {
        File.Create(this.path + "\\exclude.txt").Close();
        File.AppendAllText(this.path + "\\exclude.txt", "isolateCPU" + Environment.NewLine);
        this.exclude = File.ReadAllLines(this.path + "\\exclude.txt");
      }
      else
        this.exclude = File.ReadAllLines(this.path + "\\exclude.txt");
      this.cpucore = Environment.ProcessorCount;
      if (this.cpucore == 1)
        this.cpu = 1;
      if (this.cpucore == 2)
        this.cpu = 3;
      else if (this.cpucore == 3)
        this.cpu = 7;
      else if (this.cpucore == 4)
        this.cpu = 15;
      else if (this.cpucore == 5)
        this.cpu = 31;
      else if (this.cpucore == 6)
        this.cpu = 63;
      else if (this.cpucore == 7)
        this.cpu = (int) sbyte.MaxValue;
      else if (this.cpucore == 8)
        this.cpu = (int) byte.MaxValue;
      else
        this.cpu = Process.GetCurrentProcess().ProcessorAffinity.ToInt32();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      while (true)
      {
        try
        {
          this.isolatednumber.Text = "0";
          this.topro = 0;
          foreach (object obj in this.listBox1.Items)
          {
            string str = obj.ToString();
            this.problemkid = str.Remove(str.IndexOf(" || "), str.Length - str.IndexOf(" || "));
            this.problemkidid = str.Remove(0, str.IndexOf(" || ") + 4);
            Process.GetProcessById(Convert.ToInt32(str.Remove(0, str.IndexOf(" || ") + 4))).ProcessorAffinity = (IntPtr) 1;
            this.topro = this.topro + 1;
            this.isolatednumber.Text = this.topro.ToString();
          }
          foreach (object obj in this.listBox2.Items)
          {
            string str = obj.ToString();
            this.problemkid = str.Remove(str.IndexOf(" || "), str.Length - str.IndexOf(" || "));
            this.problemkidid = str.Remove(0, str.IndexOf(" || ") + 4);
            Process.GetProcessById(Convert.ToInt32(str.Remove(0, str.IndexOf(" || ") + 4))).ProcessorAffinity = (IntPtr) this.cpu - 1;
          }
          this.status.Text = "ON";
          this.status.ForeColor = Color.Green;
          break;
        }
        catch
        {
          File.AppendAllText(this.path + "\\exclude.txt", this.problemkid + Environment.NewLine);
          Array.Clear((Array) this.exclude, 0, this.exclude.Length);
          this.exclude = File.ReadAllLines(this.path + "\\exclude.txt");
          this.listBox1.Items.Remove((object) (this.problemkid + " || " + this.problemkidid));
          this.listBox2.Items.Remove((object) (this.problemkid + " || " + this.problemkidid));
        }
      }
    }

    private void release_Click(object sender, EventArgs e)
    {
      while (true)
      {
        try
        {
          foreach (object obj in this.listBox1.Items)
          {
            string str = obj.ToString();
            this.problemkid = str.Remove(str.IndexOf(" || "), str.Length - str.IndexOf(" || "));
            this.problemkidid = str.Remove(0, str.IndexOf(" || ") + 4);
            Process.GetProcessById(Convert.ToInt32(str.Remove(0, str.IndexOf(" || ") + 4))).ProcessorAffinity = (IntPtr) this.cpu;
          }
          foreach (object obj in this.listBox2.Items)
          {
            string str = obj.ToString();
            this.problemkid = str.Remove(str.IndexOf(" || "), str.Length - str.IndexOf(" || "));
            this.problemkidid = str.Remove(0, str.IndexOf(" || ") + 4);
            Process.GetProcessById(Convert.ToInt32(str.Remove(0, str.IndexOf(" || ") + 4))).ProcessorAffinity = (IntPtr) this.cpu;
          }
          this.status.Text = "OFF";
          this.status.ForeColor = Color.Red;
          this.isolatednumber.Text = "0";
          break;
        }
        catch
        {
          File.AppendAllText(this.path + "\\exclude.txt", this.problemkid + Environment.NewLine);
          Array.Clear((Array) this.exclude, 0, this.exclude.Length);
          this.exclude = File.ReadAllLines(this.path + "\\exclude.txt");
          this.listBox1.Items.Remove((object) (this.problemkid + " || " + this.problemkidid));
          this.listBox2.Items.Remove((object) (this.problemkid + " || " + this.problemkidid));
        }
      }
    }

    private void excludedlist_Click(object sender, EventArgs e)
    {
      Process.Start("notepad.exe", this.path + "\\exclude.txt").WaitForExit();
      Array.Clear((Array) this.exclude, 0, this.exclude.Length);
      this.exclude = File.ReadAllLines(this.path + "\\exclude.txt");
    }

    private void timer1_Tick_1(object sender, EventArgs e)
    {
      this.processlist = Process.GetProcesses();
      foreach (Process process in this.processlist)
      {
        if (!((IEnumerable<string>) this.exclude).Contains<string>(process.ProcessName) && !this.listBox2.Items.Contains((object) this.listBox1.Items.Contains((object) (process.ProcessName + " || " + (object) process.Id))) && !this.listBox1.Items.Contains((object) (process.ProcessName + " || " + (object) process.Id)))
          this.listBox1.Items.Add((object) (process.ProcessName + " || " + (object) process.Id));
      }
      for (int index = 0; index < this.listBox1.Items.Count; ++index)
      {
        string str = this.listBox1.Items[index].ToString();
        if (Process.GetProcessesByName(str.Remove(str.IndexOf(" || "), str.Length - str.IndexOf(" || "))).Length == 0)
        {
          this.listBox1.Items.RemoveAt(index);
          --index;
        }
      }
      for (int index = 0; index < this.listBox2.Items.Count; ++index)
      {
        string str = this.listBox2.Items[index].ToString();
        if (Process.GetProcessesByName(str.Remove(str.IndexOf(" || "), str.Length - str.IndexOf(" || "))).Length == 0)
        {
          this.listBox2.Items.RemoveAt(index);
          --index;
        }
      }
    }

    private void toleft_Click(object sender, EventArgs e)
    {
      if (this.listBox2.SelectedItem == null)
        return;
      this.listBox1.Items.Add(this.listBox2.SelectedItem);
      this.listBox2.Items.Remove(this.listBox2.SelectedItem);
    }

    private void toright_Click(object sender, EventArgs e)
    {
      if (this.listBox1.SelectedItem == null)
        return;
      this.listBox2.Items.Add(this.listBox1.SelectedItem);
      this.listBox1.Items.Remove(this.listBox1.SelectedItem);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (isolateCPU.isolateCPU));
      this.listBox1 = new ListBox();
      this.listBox2 = new ListBox();
      this.label1 = new Label();
      this.label2 = new Label();
      this.button1 = new Button();
      this.label3 = new Label();
      this.status = new Label();
      this.label4 = new Label();
      this.isolatednumber = new Label();
      this.toright = new Button();
      this.toleft = new Button();
      this.release = new Button();
      this.label5 = new Label();
      this.label6 = new Label();
      this.excludedlist = new Button();
      this.timer1 = new Timer(this.components);
      this.SuspendLayout();
      this.listBox1.FormattingEnabled = true;
      this.listBox1.Location = new Point(12, 22);
      this.listBox1.Name = "listBox1";
      this.listBox1.Size = new Size(133, 251);
      this.listBox1.Sorted = true;
      this.listBox1.TabIndex = 0;
      this.listBox2.FormattingEnabled = true;
      this.listBox2.Location = new Point(188, 22);
      this.listBox2.Name = "listBox2";
      this.listBox2.Size = new Size(130, 251);
      this.listBox2.Sorted = true;
      this.listBox2.TabIndex = 1;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(10, 5);
      this.label1.Name = "label1";
      this.label1.Size = new Size(73, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "All Processes:";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(186, 5);
      this.label2.Name = "label2";
      this.label2.Size = new Size(106, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Excluded Processes:";
      this.button1.Location = new Point(131, 283);
      this.button1.Name = "button1";
      this.button1.Size = new Size(75, 23);
      this.button1.TabIndex = 4;
      this.button1.Text = "Isolate";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new EventHandler(this.button1_Click);
      this.label3.AutoSize = true;
      this.label3.Location = new Point((int) byte.MaxValue, 324);
      this.label3.Name = "label3";
      this.label3.Size = new Size(40, 13);
      this.label3.TabIndex = 5;
      this.label3.Text = "Status:";
      this.status.AutoSize = true;
      this.status.ForeColor = Color.Red;
      this.status.Location = new Point(291, 324);
      this.status.Name = "status";
      this.status.Size = new Size(27, 13);
      this.status.TabIndex = 6;
      this.status.Text = "OFF";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(9, 284);
      this.label4.Name = "label4";
      this.label4.Size = new Size(102, 13);
      this.label4.TabIndex = 7;
      this.label4.Text = "Isolated Processes: ";
      this.isolatednumber.AutoSize = true;
      this.isolatednumber.Location = new Point(103, 285);
      this.isolatednumber.Name = "isolatednumber";
      this.isolatednumber.Size = new Size(13, 13);
      this.isolatednumber.TabIndex = 8;
      this.isolatednumber.Text = "0";
      this.toright.Location = new Point(151, 104);
      this.toright.Name = "toright";
      this.toright.Size = new Size(31, 23);
      this.toright.TabIndex = 9;
      this.toright.Text = ">";
      this.toright.UseVisualStyleBackColor = true;
      this.toright.Click += new EventHandler(this.toright_Click);
      this.toleft.Location = new Point(151, 171);
      this.toleft.Name = "toleft";
      this.toleft.Size = new Size(31, 23);
      this.toleft.TabIndex = 10;
      this.toleft.Text = "<";
      this.toleft.UseVisualStyleBackColor = true;
      this.toleft.Click += new EventHandler(this.toleft_Click);
      this.release.Location = new Point(131, 312);
      this.release.Name = "release";
      this.release.Size = new Size(75, 23);
      this.release.TabIndex = 14;
      this.release.Text = "Release";
      this.release.UseVisualStyleBackColor = true;
      this.release.Click += new EventHandler(this.release_Click);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(110, 299);
      this.label5.Name = "label5";
      this.label5.Size = new Size(13, 13);
      this.label5.TabIndex = 16;
      this.label5.Text = "0";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(9, 298);
      this.label6.Name = "label6";
      this.label6.Size = new Size(109, 13);
      this.label6.TabIndex = 15;
      this.label6.Text = "Excluded Processes: ";
      this.excludedlist.Location = new Point(7, 314);
      this.excludedlist.Name = "excludedlist";
      this.excludedlist.Size = new Size(116, 23);
      this.excludedlist.TabIndex = 17;
      this.excludedlist.Text = "Open Excluded List";
      this.excludedlist.UseVisualStyleBackColor = true;
      this.excludedlist.Click += new EventHandler(this.excludedlist_Click);
      this.timer1.Enabled = true;
      this.timer1.Interval = 1000;
      this.timer1.Tick += new EventHandler(this.timer1_Tick_1);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(330, 342);
      this.Controls.Add((Control) this.excludedlist);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.release);
      this.Controls.Add((Control) this.toleft);
      this.Controls.Add((Control) this.toright);
      this.Controls.Add((Control) this.isolatednumber);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.status);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.listBox2);
      this.Controls.Add((Control) this.listBox1);
      this.FormBorderStyle = FormBorderStyle.Fixed3D;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.Name = "isolateCPU";
      this.Text = "isolateCPU";
      this.Load += new EventHandler(this.Form1_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
