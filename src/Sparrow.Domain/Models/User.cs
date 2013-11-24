using System;

namespace Sparrow.Domain.Models
{
    public class User: EntityBase
    {
        private string _name;
        private string _email;
        private string _role;

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public virtual string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException("value");
                _name = value;
            }
        }

        /// <summary>
        /// Gets or sets the e-mail for the user.
        /// </summary>
        public virtual string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        /// <summary>
        /// Gets or sets the user role.
        /// </summary>
        public virtual string Role
        {
            get { return _role; }
            set { _role = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        protected User()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class with the specified name.
        /// </summary>
        /// <param name="name">User name.</param>
        public User(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            _name = name;
        }
    }
}