using System;
using System.Collections.Generic;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Sample.Client.Core.Util
{
    public class HardwareId
    {
        public static string GetHwid()
        {
            return MD5Hash(CpuId.GetCpuId() + DiskId.GetDiskId());
        }

		private static string GetHash2(string s)
		{
			MD5 sec = new MD5CryptoServiceProvider();
			ASCIIEncoding enc = new ASCIIEncoding();
			byte[] bt = enc.GetBytes(s);
			return GetHexString(sec.ComputeHash(bt));
		}

		private static string GetHexString(byte[] bt)
		{
			string s = string.Empty;
			for (int i = 0; i < bt.Length; i++)
			{
				byte b = bt[i];
				int n, n1, n2;
				n = (int)b;
				n1 = n & 15;
				n2 = (n >> 4) & 15;
				if (n2 > 9)
					s += ((char)(n2 - 10 + (int)'A')).ToString();
				else
					s += n2.ToString();
				if (n1 > 9)
					s += ((char)(n1 - 10 + (int)'A')).ToString();
				else
					s += n1.ToString();
				if ((i + 1) != bt.Length && (i + 1) % 2 == 0) s += "-";
			}
			return s;
		}

        public static string MD5Hash(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(inputString)))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }

    public class CpuId
    {
		private static int PAGE_EXECUTE_READWRITE = 0x40;

		[DllImport("user32", EntryPoint = "CallWindowProcW", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
		private static extern IntPtr CallWindowProcW([In] byte[] bytes, IntPtr hWnd, int msg, [In] [Out] byte[] wParam, IntPtr lParam);

		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern bool VirtualProtect([In] byte[] bytes, IntPtr size, int newProtect, out int oldProtect);

		public static string GetCpuId()
		{
			var sn = new byte[8];

			return !ExecuteCode(ref sn)
				? "ND"
				: string.Format("{0:X8}{1:X8}", BitConverter.ToUInt32(sn, 4), BitConverter.ToUInt32(sn, 0));
		}

		private static bool ExecuteCode(ref byte[] result)
		{
			var isX64Process = IntPtr.Size == 8;
			byte[] code;

			if (isX64Process)
				code = new byte[]
				{
					0x53, /* push rbx */
					0x48, 0xc7, 0xc0, 0x01, 0x00, 0x00, 0x00, /* mov rax, 0x1 */
					0x0f, 0xa2, /* cpuid */
					0x41, 0x89, 0x00, /* mov [r8], eax */
					0x41, 0x89, 0x50, 0x04, /* mov [r8+0x4], edx */
					0x5b, /* pop rbx */
					0xc3 /* ret */
				};
			else
				code = new byte[]
				{
					0x55, /* push ebp */
					0x89, 0xe5, /* mov  ebp, esp */
					0x57, /* push edi */
					0x8b, 0x7d, 0x10, /* mov  edi, [ebp+0x10] */
					0x6a, 0x01, /* push 0x1 */
					0x58, /* pop  eax */
					0x53, /* push ebx */
					0x0f, 0xa2, /* cpuid    */
					0x89, 0x07, /* mov  [edi], eax */
					0x89, 0x57, 0x04, /* mov  [edi+0x4], edx */
					0x5b, /* pop  ebx */
					0x5f, /* pop  edi */
					0x89, 0xec, /* mov  esp, ebp */
					0x5d, /* pop  ebp */
					0xc2, 0x10, 0x00 /* ret  0x10 */
				};

			var ptr = new IntPtr(code.Length);

			if (!VirtualProtect(code, ptr, PAGE_EXECUTE_READWRITE, out _))
				Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

			ptr = new IntPtr(result.Length);
			return CallWindowProcW(code, IntPtr.Zero, 0, result, ptr) != IntPtr.Zero;
		}
	}

	public class DiskId
	{
		public static string GetDiskId()
		{
			return GetDiskId("");
		}

		private static string GetDiskId(string diskLetter)
		{
			if (string.IsNullOrEmpty(diskLetter))
				foreach (var compDrive in DriveInfo.GetDrives())
					if (compDrive.IsReady)
					{
						diskLetter = compDrive.RootDirectory.ToString();
						break;
					}

			if (!string.IsNullOrEmpty(diskLetter) && diskLetter.EndsWith(":\\"))
				diskLetter = diskLetter.Substring(0, diskLetter.Length - 2);
			var disk = new ManagementObject(@"win32_logicaldisk.deviceid=""" + diskLetter + @":""");
			disk.Get();

			var volumeSerial = disk["VolumeSerialNumber"].ToString();
			disk.Dispose();

			return volumeSerial;
		}
	}
}
