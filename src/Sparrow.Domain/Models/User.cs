using System;

namespace Sparrow.Domain.Models
{
    public class User: EntityBase
    {
        private string _email;
        private string _role;
        private string _userName;
        private string _firstName;
        private string _lastName;
        private string _location;

        /// <summary>
        /// Gets the user name for current user.
        /// </summary>
        public virtual string UserName
        {
            get { return _userName; }
        }

        /// <summary>
        /// Gets or sets the first name for current user.
        /// </summary>
        public virtual string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        /// <summary>
        /// Gets or sets the last name for current user.
        /// </summary>
        public virtual string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        /// <summary>
        /// Gets the full name for current user.
        /// </summary>
        public virtual string FullName
        {
            get
            {
                var isFirstNameEmpty = string.IsNullOrEmpty(FirstName);
                var isLastNameEmpty = string.IsNullOrEmpty(LastName);

                if (isFirstNameEmpty && isLastNameEmpty)
                    return UserName;
                if (isFirstNameEmpty)
                    return LastName;
                if (isLastNameEmpty)
                    return FirstName;

                return string.Format("{0} {1}", FirstName, LastName);
            }
        }

        /// <summary>
        /// Gets or sets the location for current user.
        /// </summary>
        public virtual string Location
        {
            get { return _location; }
            set { _location = value; }
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
        /// <param name="userName">User name.</param>
        public User(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("userName");
            _userName = userName;
        }
    }
}