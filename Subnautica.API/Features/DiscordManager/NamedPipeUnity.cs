namespace Subnautica.API.Features.DiscordManager
{
    using System;
    using System.IO;

    using DiscordRPC.IO;

    public class NamedPipeUnity : INamedPipeClient
    {
        const string PIPE_NAME = @"discord-ipc-{0}";

        private NamedPipeClientStream _stream;
        private byte[] _buffer = new byte[PipeFrame.MAX_SIZE];

        public DiscordRPC.Logging.ILogger Logger { get; set; }
        public bool IsConnected { get { return _stream != null && _stream.IsConnected; } }
        public int ConnectedPipe { get; private set; }

        private volatile bool _isDisposed = false;

        public bool Connect(int pipe)
        {
            if (_isDisposed)
                throw new ObjectDisposedException("NamedPipe");

            if (pipe > 9)
                throw new ArgumentOutOfRangeException("pipe", "Argument cannot be greater than 9");

            if (pipe < 0)
            {
                //If we have -1,  then we need to iterate over every single pipe until we get it
                //Iterate until we connect to a pipe
                for (int i = 0; i < 10; i++)
                {
                    if (AttemptConnection(i) || AttemptConnection(i, true))
                        return true;
                }

                //We failed everythign else
                return false;
            }
            else
            {
                //We have a set one so we should just straight up try to connect to it
                return AttemptConnection(pipe) || AttemptConnection(pipe, true);
            }
        }

        private bool AttemptConnection(int pipe, bool doSandbox = false)
        {
            //Make sure the stream is null
            if (_stream != null)
            {
                Log.Error("Attempted to create a new stream while one already exists!");
                return false;
            }

            //Make sure we are disconnected
            if (IsConnected)
            {
                Log.Error("Attempted to create a new connection while one already exists!");
                return false;
            }

            try
            {
                //Prepare the sandbox
                string sandbox = doSandbox ? GetPipeSandbox() : "";
                if (doSandbox && sandbox == null)
                {
                    // Log.Error("Skipping sandbox because this platform does not support it.");
                    return false;
                }

                //Prepare the name
                string pipename = GetPipeName(pipe);

                //Attempt to connect
                ConnectedPipe = pipe;
                _stream = new NamedPipeClientStream(".", pipename);
                _stream.Connect();

                return true;
            }
            catch (Exception)
            {
                // Log.Error("Failed: " + e.GetType().FullName + ", " + e.Message);
                ConnectedPipe = -1;
                Close();
                return false;
            }
        }

        public void Close()
        {
            if (_stream != null)
            {
                // Log.Error("Closing stream");
                _stream.Dispose();
                _stream = null;
            }
        }

        public void Dispose()
        {
            if (_isDisposed) return;
            Log.Error("Disposing Stream");
            _isDisposed = true;
            Close();
        }

        public bool ReadFrame(out PipeFrame frame)
        {
            if (_isDisposed)
                throw new ObjectDisposedException("_stream");

            //We are not connected so we cannot read!
            if (!IsConnected)
            {
                frame = default;
                return false;
            }

            //Try and read a frame
            int length = _stream.Read(_buffer, 0, _buffer.Length);

            if (length == 0)
            {
                frame = default;
                return false;
            }

            //Read the stream now
            using (MemoryStream memory = new MemoryStream(_buffer, 0, length))
            {
                frame = new PipeFrame();
                if (!frame.ReadStream(memory))
                {
                    Log.Error("Failed to read a frame! " + frame.Opcode);
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public bool WriteFrame(PipeFrame frame)
        {
            if (_isDisposed)
                throw new ObjectDisposedException("_stream");

            //Write the frame. We are assuming proper duplex connection here
            if (!IsConnected)
            {
                Log.Error("Failed to write frame because the stream is closed");
                return false;
            }

            try
            {
                frame.WriteStream(_stream);
                return true;
            }
            catch (IOException io)
            {
                Log.Error("Failed to write frame because of a IO Exception: " + io.Message);
            }
            catch (ObjectDisposedException)
            {
                Log.Error("Failed to write frame as the stream was already disposed");
            }
            catch (InvalidOperationException)
            {
                Log.Error("Failed to write frame because of a invalid operation");
            }

            //We must have failed the try catch
            return false;
        }

        private string GetPipeName(int pipe, string sandbox = "")
        {
            switch (Environment.OSVersion.Platform)
            {
                default:
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                    return sandbox + string.Format(PIPE_NAME, pipe);
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    return Path.Combine(GetEnviromentTemp(), sandbox + string.Format(PIPE_NAME, pipe));
            }
        }

        private string GetPipeSandbox()
        {
            switch (Environment.OSVersion.Platform)
            {
                default:
                    return null;
                case PlatformID.Unix:
                    return "snap.discord/";
            }
        }

        private string GetEnviromentTemp()
        {
            string temp = null;
            temp = temp ?? Environment.GetEnvironmentVariable("XDG_RUNTIME_DIR");
            temp = temp ?? Environment.GetEnvironmentVariable("TMPDIR");
            temp = temp ?? Environment.GetEnvironmentVariable("TMP");
            temp = temp ?? Environment.GetEnvironmentVariable("TEMP");
            temp = temp ?? "/tmp";
            return temp;
        }
    }
}
