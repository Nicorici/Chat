using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Threading;
using System.IO;
using Components;

namespace Server
{
    public class ChatRoom
    {
        private ChatStream chatStream;
        private List<User> users = new List<User>();

        public void Join(User user)
        {
            users.Add(user);
            user.BeginReceive(m =>
            {
                if (string.IsNullOrWhiteSpace(user.Name))
                {
                    user.Name = GetName(m);
                    string replace = m.ToString().Replace(":", "");
                    Console.WriteLine(replace);
                    SendDataToUsers(new Message(replace));
                    return;
                }
                Console.WriteLine(m);
                SendDataToUsers(m);
            }
            );
        }

        public void Remove(User user)
        {
            user.Close();
            users.Remove(user);
        }

        private void SendDataToUsers(Message text)
        {
            string disconnectedMsg = "";
            var disconnectedUsers = new List<User>();
            foreach (var user in users)
            {
                if (user.Name != GetName(text))

                    try
                    {
                        user.stream.Write(text.ToString(), null);
                    }
                    catch
                    {
                        disconnectedMsg += user.Name.Replace(":","") + " has disconnected...\0";
                        Console.WriteLine(user.Name.Replace(":","") + " has disconnected...");
                        disconnectedUsers.Add(user);
                    }
            }
            if(disconnectedUsers.Count>0)
            RemoveClients(disconnectedUsers, disconnectedMsg.TrimEnd());
        }

        private void RemoveClients(List<User> list, string message)
        {
            foreach (var user in list)
                Remove(user);

            SendDataToUsers(new Message(message));
        }


        private string GetName(Message message)
        {
            string temp = message.ToString();
            return temp.Substring(0, temp.IndexOf(':')+1);
        }
    }
}
