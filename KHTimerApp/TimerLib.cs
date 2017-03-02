using System;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using NLog;

namespace KHTimerApp
{
    internal class TimerLib : IDisposable
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private const string Separator = ":";
        private const int TIME_OUT = 1000;

        private string _portName = "";
        private SerialPort _port = null;

        public TimerLib(string name)
        {
            _port = new SerialPort(name, 9600, Parity.None, 8, StopBits.One);
            _port.ReadTimeout = TIME_OUT;
            _portName = name;
        }

        public bool IsOpen 
        {
            get
            {
                return _port.IsOpen;
            }
        }

        public void Start()
        {
            Call("start");
        }

        public void Stop()
        {
            Call("stop");
        }

        public void Reset()
        {
            Call("reset");
        }

        public Tuple<string, int> Time()
        {
            try
            {
                string res = Call("time");

                string[] vals = res.Split(':');

                string _time = vals[1];
                string[] _timeTokens = _time.Split('.');

                int _mins = int.Parse(_timeTokens[0]);
                int _secs = int.Parse(_timeTokens[1]);

                return new Tuple<string, int>(vals[0], (_mins*60) + _secs);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Not able to get the time from external device!");
                return new Tuple<string, int>("stop", 0);
            }
        }

        public bool Ping()
        {
            string v = Call("ver");
            return !string.IsNullOrEmpty(v);
        }

        public string Name
        {
            get { return _portName; }
        }

        public static TimerLib Seek()
        {
            var ports = SerialPort.GetPortNames();
            foreach (var port in ports)
            {
                try
                {
                    TimerLib _timer = new TimerLib(port);
                    if (_timer.Ping()) return _timer;
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Not able to communicate the timer using port: {0}", port);
                }
            }
            return null;
        }

        #region private

        private string Call(string cmd)
        {
            try
            {
                TryOpen();
                _port.WriteLine("::" + cmd + ";");
                string _line = _port.ReadLine();
                _line = _line.TrimEnd(new char[] {';', '\n', '\r'});

                // skip first '0'
                int _off = _line.IndexOf(Separator);
                _line = _line.Substring(_off + 1);

                // skip result
                _off = _line.IndexOf(Separator);
                _line = _line.Substring(_off + 1);

                // skip checksum
                _off = _line.IndexOf(Separator);
                _line = _line.Substring(_off + 1);

                logger.Debug(cmd + ": " + _line);
                return _line;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Not able to call this command: {0} on the device: {1}", cmd, _portName);
                return "";
            }
        }

        private void TryOpen()
        {
            try
            {
                if (!_port.IsOpen)
                {
                    _port.Open();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error opening COM port: {0}", _portName);
            }
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            try
            {
                if (null != _port)
                {
                    if (_port.IsOpen) _port.Close();
                    _port.Dispose();
                    _port = null;
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Cannot dispose the device on port: {0}", _portName);
            }
        }
        #endregion
    }
}