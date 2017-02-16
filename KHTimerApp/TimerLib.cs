using System;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;

namespace KHTimerApp
{
    internal class TimerLib : IDisposable
    {
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

        public void Start()
        {
            string res = Call("start");
            Debug.WriteLine("start: " + res);
        }

        public void Stop()
        {
            string res = Call("stop");
            Debug.WriteLine("stop: " + res);
        }

        public void Reset()
        {
            string res = Call("reset");
            Debug.WriteLine("reset: " + res);
        }

        public Tuple<string, int> Time()
        {
            try
            {
                string res = Call("time");
                Debug.WriteLine("reset: " + res);

                string[] vals = res.Split(':');

                string _time = vals[1];
                string[] _timeTokens = _time.Split('.');

                int _mins = int.Parse(_timeTokens[0]);
                int _secs = int.Parse(_timeTokens[1]);

                return new Tuple<string, int>(vals[0], (_mins*60) + _secs);
            }
            catch (Exception)
            {
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
                    Debug.WriteLine("[Exception] " + ex.Message);
                }
            }
            return null;
        }

        #region private

        private string Call(string cmd)
        {
            try
            {
                IsOpen();
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

                return _line;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[Exception] " + ex.Message);
                return "";
            }
        }

        private void IsOpen()
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
                Debug.WriteLine("[Exception] " + ex.Message);
            }
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            try
            {
                if (null != _port && _port.IsOpen)
                {
                    _port.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[Exception] " + ex.Message);
            }
        }
        #endregion
    }
}