using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;

namespace GameMain {
	public static class ZipUtility {
		private static bool ZipFileDictory (string FolderToZip, ZipOutputStream s, string ParentFolderName)
		{
			bool res = true;
			string [] folders, filenames;
			ZipEntry entry = null;
			FileStream fs = null;
			Crc32 crc = new Crc32 ();
			try {
				entry = new ZipEntry (Path.Combine (ParentFolderName, Path.GetFileName (FolderToZip) + "/"));
				s.PutNextEntry (entry);
				s.Flush ();
				filenames = Directory.GetFiles (FolderToZip);
				foreach (string file in filenames) {
					fs = File.OpenRead (file);
					byte [] buffer = new byte [fs.Length];
					fs.Read (buffer, 0, buffer.Length);
					entry = new ZipEntry (Path.Combine (ParentFolderName, Path.GetFileName (FolderToZip) + "/" + Path.GetFileName (file)));
					entry.DateTime = DateTime.Now;
					entry.Size = fs.Length;
					fs.Close ();
					crc.Reset ();
					crc.Update (buffer);
					entry.Crc = crc.Value;
					s.PutNextEntry (entry);
					s.Write (buffer, 0, buffer.Length);
				}
			} catch {
				res = false;
			} finally {
				if (fs != null) {
					fs.Close ();
					fs = null;
				}
				if (entry != null) {
					entry = null;
				}
				GC.Collect ();
				GC.Collect (1);
			}
				folders = Directory.GetDirectories (FolderToZip);
				foreach (string folder in folders) {
				if (!ZipFileDictory (folder, s, Path.Combine (ParentFolderName, Path.GetFileName (FolderToZip)))) {
					return false;
				}
			}
			return res;
		}

		/// <summary>
		/// 压缩目录
		/// </summary>
		/// <param name="FolderToZip">待压缩的文件夹，全路径格式</param>
		/// <param name="ZipedFile">压缩后的文件名，全路径格式</param>
		private static bool ZipFileDictory (string FolderToZip, string ZipedFile, int level)
		{
			bool res;
			if (!Directory.Exists (FolderToZip)) {
				return false;
			}
			ZipOutputStream s = new ZipOutputStream (File.Create (ZipedFile));
			s.SetLevel (level);
			res = ZipFileDictory (FolderToZip, s, "");
			s.Finish ();
			s.Close ();
			return res;
		}

		/// <summary>
		/// 压缩文件
		/// </summary>
		/// <param name="FileToZip">要进行压缩的文件名</param>
		/// <param name="ZipedFile">压缩后生成的压缩文件名</param>
		private static bool ZipFile (string FileToZip, string ZipedFile, int level)
		{
			if (!File.Exists (FileToZip)) {
				throw new System.IO.FileNotFoundException ("指定要压缩的文件: " + FileToZip + " 不存在!");
			}
			FileStream fs = null;
			ZipOutputStream zs = null;
			ZipEntry ze = null;
			bool res = true;
			try {
				fs = File.OpenRead (FileToZip);
				byte [] buffer = new byte [fs.Length];
				fs.Read (buffer, 0, buffer.Length);
				fs.Close ();

				fs = File.Create (ZipedFile);
				zs = new ZipOutputStream (fs);
				ze = new ZipEntry (Path.GetFileName (FileToZip));
				zs.PutNextEntry (ze);
				zs.SetLevel (level);

				zs.Write (buffer, 0, buffer.Length);
			} catch {
				res = false;
			} finally {
				if (ze != null) {
					ze = null;
				}
				if (zs != null) {
					zs.Finish ();
					zs.Close ();
				}
				if (fs != null) {
					fs.Close ();
					fs = null;
				}
				GC.Collect ();
				GC.Collect (1);
			}
			return res;
		}

		/// <summary>
		/// 压缩
		/// </summary>
		/// <param name="FileToZip">待压缩的文件目录</param>
		/// <param name="ZipedFile">生成的目标文件</param>
		/// <param name="level">level</param>
		public static bool Zip (String FileToZip, String ZipedFile, int level = 0)
		{
			if (Directory.Exists (FileToZip)) {
				return ZipFileDictory (FileToZip, ZipedFile, level);
			} else if (File.Exists (FileToZip)) {
				return ZipFile (FileToZip, ZipedFile, level);
			} else {
				return false;
			}
		}

		/// <summary>
		/// 解压
		/// </summary>
		/// <param name="FileToUpZip">待解压的文件</param>
		/// <param name="ZipedFolder">解压目标存放目录</param>
		public static void UnZip (string FileToUpZip, string ZipedFolder)
		{
			if (!File.Exists (FileToUpZip)) {
				return;
			}
			if (!Directory.Exists (ZipedFolder)) {
				Directory.CreateDirectory (ZipedFolder);
			}
			ZipInputStream s = null;
			ZipEntry theEntry = null;
			string fileName;
			FileStream fs = null;
			try {
				s = new ZipInputStream (File.OpenRead (FileToUpZip));
				while ((theEntry = s.GetNextEntry ()) != null) {
					if (theEntry.Name != String.Empty) {
						fileName = Path.Combine (ZipedFolder, theEntry.Name);
						if (fileName.EndsWith ("/") || fileName.EndsWith ("\\")) {
							Directory.CreateDirectory (fileName);
							continue;
						}
						fs = File.Create (fileName);
						int size = 2048;
						byte [] data = new byte [2048];
						while (true) {
							size = s.Read (data, 0, data.Length);
							if (size > 0) {
								fs.Write (data, 0, size);
							} else {
								break;
							}
						}
					}
				}
			} finally {
				if (fs != null) {
					fs.Close ();
					fs = null;
				}
				if (theEntry != null) {
					theEntry = null;
				}
				if (s != null) {
					s.Close ();
					s = null;
				}
				GC.Collect ();
				GC.Collect (1);
			}
		}
	}
}
