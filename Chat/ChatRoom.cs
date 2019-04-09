using System;
using System.Collections.Generic;
using Components;

namespace Server
{
    public class ChatRoom
    {
        private readonly object listOperationsLock = new object();
        private List<User> users = new List<User>();

        public void Join(User user)
        {
            lock (listOperationsLock)
                users.Add(user);
            GetUserName(user);
            ReadMessagesFromUser(user);
        }

        private void GetUserName(User user)
        {
            user.BeginReceive(m => user.Name = GetName(m));
        }

        private void ReadMessagesFromUser(User user)
        {
            user.BeginReceive(m => HandleNewMessage(user, m),
            () => ReadMessagesFromUser(user),
            () => Remove(user, " has lost connection...")
            );
        }

        private void HandleNewMessage(User user, Message m)
        {
            if (m.Equals(user.Name + " : " + "DISCONNECT."))
            {
                Remove(user, " has left the chat...");
                return;
            }

            Console.WriteLine(m);
            SendDataToUsers(m);
        }

        public void Remove(User user, string message)
        {
            lock (listOperationsLock)
                users.Remove(user);
            user.Close();
            SendDataToUsers(new Message(user.Name + message));
            Console.WriteLine(user.Name + message);
        }

        private void SendDataToUsers(Message text)
        {
            lock (listOperationsLock)
            {
                foreach (var user in users)
                {
                    if (user.Name + " :" != GetName(text))

                        try
                        {
                            user.Send(text, null, user.Close);
                        }
                        catch
                        {
                        }
                }
            }
        }

        private string GetName(Message message)
        {
            string temp = message.ToString();
            return temp.Substring(0, temp.IndexOf(':') + 1);
        }
    }
}
