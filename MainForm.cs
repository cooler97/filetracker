using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Atechnology.Components;
using Atechnology.ecad;

namespace FileTracker
{
	public partial class MainForm : AtUserControl
	{
		private FileSystemWatcher fileWatcher;
		
		private SettingVar settingVar;
		
		public MainForm()
		{
			InitializeComponent();

			foreach(SettingVar var in Atechnology.ecad.Settings.SettingVarList)
			{
				comboBox1.Items.Add(new SettingVarWrapper(var));
			}
			
		}

		private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
		{
			fileWatcher.EnableRaisingEvents = false;
			
			FileInfo objFileInfo = new FileInfo(e.FullPath);
			
			if (!objFileInfo.Exists)
				return;

			if(settingVar == null)
			{
				AddMessage("Selected var is null");
				return;
			}
			
			settingVar.blbvalue = TryReadFile(e.FullPath);
			
			Settings.Save();
			
			AddMessage(String.Format("File successfull update {0}", e.FullPath));
			
			fileWatcher.EnableRaisingEvents = true;
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			FileDialog fileDialog = new OpenFileDialog();
			
			if(fileDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			
			string fileWatcheName = fileDialog.FileName;
			
			textBox1.Text = fileWatcheName;
			
			fileWatcher = new FileSystemWatcher(Path.GetDirectoryName(fileWatcheName), Path.GetFileName(fileWatcheName));
			
			fileWatcher.NotifyFilter = NotifyFilters.LastWrite;
			
			this.SetTitle("Watching file: " + Path.GetFileName(fileWatcheName));
			
			fileWatcher.Changed += FileSystemWatcher_Changed;
			
			fileWatcher.EnableRaisingEvents = true;

		}
		
		private void AddMessage(string message)
		{
			StringBuilder builder = new StringBuilder();
			
			builder.AppendFormat("[{0}]: {1}", DateTime.Now, message);
			
			builder.AppendLine();
			
			if(logBox.InvokeRequired)
			{
				logBox.Invoke(new Action<string>((s) => logBox.AppendText(s)), builder.ToString());
			}
			else
			{
				logBox.AppendText(builder.ToString());
			}
		}
		
		private byte[] TryReadFile(string path)
		{
			int count = 0;
			
			while(FileIsLocked(path, FileAccess.Read)) {
				
				if(count > 5)
				{
					throw new IOException(String.Format("File is locked '{0}'", path));
				}
				
				Thread.Sleep(1000);
				count++;
			}
			
			return ReadFile(path);
		}
		
		private byte[] ReadFile(string path)
		{
			FileStream fileStream = null;
			
			try
			{
				fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
				byte[] buffer = new byte[fileStream.Length];
				fileStream.Read(buffer, 0, (int) fileStream.Length);
				return buffer;
			}
			catch (Exception ex)
			{
				AddMessage(ex.Message);
			}
			finally {
				
				if(fileStream != null )
					fileStream.Close();
			}
			
			return new byte[0];
		}
		
		private bool FileIsLocked(string filename, FileAccess file_access)
		{
			try
			{
				FileStream fs =
					new FileStream(filename, FileMode.Open, file_access);
				fs.Close();
				return false;
			}
			catch (IOException)
			{
				return true;
			}
			catch (Exception)
			{
				throw;
			}
		}
		
		void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			
			if(comboBox1.SelectedItem == null)
				return;
			
			SettingVarWrapper varWrapper = comboBox1.SelectedItem as SettingVarWrapper;
			
			if(varWrapper == null)
				return;
			
			settingVar = varWrapper.Value;
		}

	}
}
