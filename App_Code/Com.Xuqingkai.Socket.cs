namespace SH
{
    public class Socket
    {
        private System.Text.Encoding encode = System.Text.Encoding.GetEncoding("UTF-8");

        public System.Collections.Generic.Dictionary<string, System.Net.Sockets.Socket> sockets = new System.Collections.Generic.Dictionary<string, System.Net.Sockets.Socket>();

        /// <summary>
        /// 监听请求
        /// </summary>
        /// <param name="port"></param>
        public void Listen(int port)
        {
            System.Net.Sockets.Socket listenSocket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
            try
            {
                listenSocket.Bind(new System.Net.IPEndPoint(System.Net.IPAddress.Any, port));
            }
            catch
            {
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.Write("已运行");
                System.Web.HttpContext.Current.Response.End();
            }
            listenSocket.Listen(100);
            while (true)
            {
                string ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] + "";
                if (!sockets.ContainsKey(ip))
                {
                    System.Net.Sockets.Socket acceptSocket = listenSocket.Accept();
                    string receiveData = Receive(acceptSocket, 5000); //5 seconds timeout.
                    if (receiveData != null && receiveData.Length > 0)
                    {
                        ip += "." + receiveData.Substring(0,1); 
                        //string result = Http("http://pay.05370.com/pay/callback.aspx/save?data=" + receiveData, "");
                        acceptSocket.Send(encode.GetBytes(ip + ":(" + receiveData + ")"));
                    }
                    sockets.Add(ip, acceptSocket);
                    //DestroySocket(acceptSocket); 
                }
                else
                {
                    System.Net.Sockets.Socket acceptSocket = sockets[ip];
                    string receiveData = Receive(acceptSocket, 5000); //5 seconds timeout.
                    if (receiveData != null && receiveData.Length > 0)
                    {
                        //string result = Http("http://pay.05370.com/pay/callback.aspx/save?data=" + receiveData,"");
                        acceptSocket.Send(encode.GetBytes(ip + ":(" + receiveData + ")"));
                    }
                }
            }
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Send(string host, int port, string data)
        {
            string result = null;
            System.Net.Sockets.Socket clientSocket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
            clientSocket.Connect(host, port);
            clientSocket.Send(encode.GetBytes(data));
            result = Receive(clientSocket, 1000 * 20); //10 seconds timeout.
            DestroySocket(clientSocket);
            return result;
        }
        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        private string Receive(System.Net.Sockets.Socket socket, int timeout)
        {
            string result = string.Empty;
            socket.ReceiveTimeout = timeout;
            System.Collections.Generic.List<byte> data = new System.Collections.Generic.List<byte>();
            byte[] buffer = new byte[1024];
            int length = 0;
            try
            {
                while ((length = socket.Receive(buffer)) > 0)
                {
                    for (int j = 0; j < length; j++)
                    {
                        data.Add(buffer[j]);
                    }
                    if (length < buffer.Length)
                    {
                        break;
                    }
                }
            }
            catch { }
            if (data.Count > 0)
            {
                result = encode.GetString(data.ToArray(), 0, data.Count);
            }

            return result;
        }
        /// <summary>
        /// 销毁Socket对象
        /// </summary>
        /// <param name="socket"></param>
        private void DestroySocket(System.Net.Sockets.Socket socket)
        {
            if (socket.Connected)
            {
                socket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
            }
            //socket.Disconnect(true);
            socket.Close();
            //socket.Dispose();
            //socket = null;
        }
    }
}