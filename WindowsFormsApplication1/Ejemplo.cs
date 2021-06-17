using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distar
{
    /// <summary>
    /// Observer class
    /// </summary>
    abstract class Observer
    {
        public abstract void notify(string eventName, object eventValue);
    }

    /// <summary>
    /// Observable class
    /// </summary>
    abstract class Observable
    {
        //List of observers
        protected Dictionary<Observer, string> _observers = new Dictionary<Observer, string>();

        /// <summary>
        /// Attach an observer
        /// </summary>
        /// <param name="observer"></param>
        /// <param name="eventName"></param>
        public void attach(Observer observer, string eventName = null)
        {
            this._observers.Add(observer, eventName);
        }

        /// <summary>
        /// Detach an observer
        /// </summary>
        /// <param name="observer"></param>
        public void detach(Observer observer)
        {
            this._observers.Remove(observer);
        }

        /// <summary>
        /// Notify an event to registered observers
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventValue"></param>
        public void notify(string eventName, object eventValue)
        {
            foreach (Observer key in _observers.Keys)
            {
                if (_observers[key] == eventName)
                {
                    key.notify(eventName, eventValue);
                }
            }
        }
    }

    /// <summary>
    /// User object to contain properties
    /// </summary>
    class UserObject
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }
    }

    class Users : Observable
    {
        public const string ADD_USER = "addUser";
        public const string REMOVE_USER = "removeUser";
        public const string ERROR = "errorUser";

        protected Dictionary<string, UserObject> _users = new Dictionary<string, UserObject>();

        /// <summary>
        /// Add a user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="address"></param>
        /// <param name="postcode"></param>
        public void addUser(string username, string email, string address, string postcode)
        {
            UserObject user = new UserObject()
            {
                Address = address,
                Email = email,
                Postcode = postcode,
                Username = username
            };
            _users.Add(username, user);
            this.notify(ADD_USER, user);
        }

        /// <summary>
        /// Removes a user
        /// </summary>
        /// <param name="username"></param>
        public void removeUser(string username)
        {
            if (_users.ContainsKey(username))
            {
                this.notify(REMOVE_USER, _users[username]);
                _users.Remove(username);
            }
            else
            {
                this.notify(ERROR, "removing user failed, username not found " + username);
            }
        }
    }

    /// <summary>
    /// Send mail observer, sends an email when a user is added or removed
    /// </summary>
    class SendEmailToNewUsers : Observer
    {
        public override void notify(string eventName, object eventValue)
        {
            if (eventName == Users.ADD_USER)
            {
                Logger.LogMessage("[sendmail] Added a user, sending email to " + ((UserObject)eventValue).Email);
            }
            else if (eventName == Users.REMOVE_USER)
            {
                Logger.LogMessage("[sendmail] Removed a user, sending email to " + ((UserObject)eventValue).Email);
            }
        }
    }

    /// <summary>
    /// Welcome observer, shows a message when a user is added
    /// </summary>
    class WelcomeNewUser : Observer
    {
        public override void notify(string eventName, object eventValue)
        {
            Logger.LogMessage("[welcome] Welcome aboard, " + ((UserObject)eventValue).Username + "!");
        }
    }

    /// <summary>
    /// Error observer, shows a message when an error arises
    /// </summary>
    class ErrorUser : Observer
    {
        public override void notify(string eventName, object eventValue)
        {
            Logger.LogMessage("[error] There was an error: " + eventValue.ToString());
        }
    }

    //Tester program
    class Program
    {
        static void Main(string[] args)
        {
            Logger.LogMessage("Starting test");

            //Observable
            Users users = new Users();

            //observers
            users.attach(new SendEmailToNewUsers());
            users.attach(new WelcomeNewUser(), Users.ADD_USER);
            users.attach(new ErrorUser(), Users.ERROR);

            //process users
            users.addUser("Ian Ross", "ianross@gmail.xxx", "66 London Street", "PE1 2RF");
            users.addUser("Mike Smith", "mikes@gmail.xxx", "123 Main Road", "PE2 6FX");
            users.removeUser("Ian Ross");
            users.addUser("John Wayne", "jwayne@gmail.xxx", "999 Kansas Road", "LE5 5AZ");
            users.removeUser("Anthony McQuinn");
            users.removeUser("John Wayne");

            Logger.LogMessage("End test");
        }
    }
}
