using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace DepotDownloader
{
    internal static class Util
    {
        public static string GetSteamOS()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return "windows";   
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return "macos";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return "linux";
            }

            return "unknown";
        }

        public static string ReadPassword()
        {
            ConsoleKeyInfo keyInfo;
            StringBuilder password = new StringBuilder();

            do 
            {
                keyInfo = Console.ReadKey( true );

                if ( keyInfo.Key == ConsoleKey.Backspace )
                {
                    if ( password.Length > 0 )
                        password.Remove( password.Length - 1, 1 );
                    continue;
                }

                /* Printable ASCII characters only */
                char c = keyInfo.KeyChar;
                if ( c >= ' ' && c <= '~' )
                    password.Append( c );
            } while ( keyInfo.Key != ConsoleKey.Enter );

            return password.ToString();
        }

        // Validate a file against Steam3 Chunk data
        public static List<ProtoManifest.ChunkData> ValidateSteam3FileChecksums(FileStream fs, ProtoManifest.ChunkData[] chunkdata)
        {
            var neededChunks = new List<ProtoManifest.ChunkData>();
            int read;

            foreach (var data in chunkdata)
            {
                byte[] chunk = new byte[data.UncompressedLength];
                fs.Seek((long)data.Offset, SeekOrigin.Begin);
                read = fs.Read(chunk, 0, (int)data.UncompressedLength);

                byte[] tempchunk;
                if (read < data.UncompressedLength)
                {
                    tempchunk = new byte[read];
                    Array.Copy(chunk, 0, tempchunk, 0, read);
                }
                else
                {
                    tempchunk = chunk;
                }

                byte[] adler = AdlerHash(tempchunk);
                if (!adler.SequenceEqual(data.Checksum))
                {
                    neededChunks.Add(data);
                }
            }

            return neededChunks;
        }

        public static byte[] AdlerHash(byte[] input)
        {
            uint a = 0, b = 0;
            for (int i = 0; i < input.Length; i++)
            {
                a = (a + input[i]) % 65521;
                b = (b + a) % 65521;
            }
            return BitConverter.GetBytes(a | (b << 16));
        }

        public static byte[] SHAHash( byte[] input )
        {
            using (var sha = SHA1.Create())
            {
                var output = sha.ComputeHash( input );

                return output;
            }
        }

        public static byte[] DecodeHexString( string hex )
        {
            if ( hex == null )
                return null;

            int chars = hex.Length;
            byte[] bytes = new byte[ chars / 2 ];

            for ( int i = 0 ; i < chars ; i += 2 )
                bytes[ i / 2 ] = Convert.ToByte( hex.Substring( i, 2 ), 16 );

            return bytes;
        }

        public static string EncodeHexString( byte[] input )
        {
            return input.Aggregate( new StringBuilder(),
                ( sb, v ) => sb.Append( v.ToString( "x2" ) )
                ).ToString();
        }

        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

    }
}
