using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibSDMX
{
    /// <summary>
    /// Header class captures information required to create the Header of an SDMX artefact. 
    /// </summary>
    public class Header
    {
        #region "Properties"

        private string id = string.Empty;

        /// <summary>
        /// Header's ID.
        /// </summary>
        public string ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        private string name = string.Empty;

        /// <summary>
        /// Header's Name.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        private SenderReceiver sender;

        /// <summary>
        /// Sender object.
        /// </summary>
        public SenderReceiver Sender
        {
            get
            {
                return sender;
            }
            set
            {
                sender = value;
            }
        }

        private SenderReceiver receiver;

        /// <summary>
        /// Receiver object.
        /// </summary>
        public SenderReceiver Receiver
        {
            get
            {
                return receiver;
            }
            set
            {
                receiver = value;
            }
        }

        #endregion "Properties"

        #region "Constructors"

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public Header():this(string.Empty, string.Empty, new SenderReceiver(), new SenderReceiver())
        {
        }

        /// <summary>
        /// Constructor with required parameters.
        /// </summary>
        /// <param name="id">ID of the Header.</param>
        /// <param name="name">Name of the Header.</param>
        /// <param name="sender">Sender object in Header.</param>
        /// <param name="receiver">Receiver object in Header.</param>
        public Header(string id, string name, SenderReceiver sender, SenderReceiver receiver)
        {
            this.ID = id;
            this.Name = name;
            this.Sender = sender;
            this.Receiver = receiver;
        }

        #endregion "Constructors"
    }

    /// <summary>
    /// SenderReceiver class represents a sender or receiver.
    /// </summary>
    public class SenderReceiver
    {
        #region "Properties"

        private string id = string.Empty;

        /// <summary>
        /// Sender or Receiver's ID.
        /// </summary>
        public string ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        private string name = string.Empty;

        /// <summary>
        /// Sender or Receiver's Name.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        private Contact contact;

        /// <summary>
        /// Sender or Receiver's contact information.
        /// </summary>
        public Contact Contact
        {
            get
            {
                return contact;
            }
            set
            {
                contact = value;
            }
        }

        #endregion "Properties"

        #region "Constructors"

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public SenderReceiver():this(string.Empty, string.Empty, new Contact())
        {
        }

        /// <summary>
        /// Constructor with id, name and Contact object as parameters.
        /// </summary>
        /// <param name="id">Sender or Receiver's ID.</param>
        /// <param name="name">Sender or Receiver's Name.</param>
        /// <param name="contact">Sender or Receiver's Contact information.</param>
        public SenderReceiver(string id, string name, Contact contact)
        {
            this.ID = id;
            this.Name = name;
            this.Contact = contact;
        }

        /// <summary>
        /// Constructor with id, name, contact name, contact department, contact role, contact email, contact telephone and contact fax as 
        /// parameters.
        /// </summary>
        /// <param name="id">Sender or Receiver's ID.</param>
        /// <param name="name">Sender or Receiver's Name.</param>
        /// <param name="contact_name">Contact's Name.</param>
        /// <param name="contact_department">Contact's Department.</param>
        /// <param name="contact_role">Contact's Role.</param>
        /// <param name="contact_email">Contact's Email.</param>
        /// <param name="contact_telephone">Contact's Telephone.</param>
        /// <param name="contact_fax">Contact's Fax.</param>
        public SenderReceiver(string id, string name, string contact_name, string contact_department, string contact_role, string contact_email, 
                              string contact_telephone, string contact_fax)
        {
            this.ID = id;
            this.Name = name;
            this.Contact = new Contact(contact_name, contact_department, contact_role, contact_email, contact_telephone, contact_fax);
        }

        #endregion "Constructors"
    }

    /// <summary>
    /// Contact class represents contact information of a person.
    /// </summary>
    public class Contact
    {
        #region "Properties"

        private string name = string.Empty;

        /// <summary>
        /// Contact's Name.
        /// </summary>
        public string Name
        {
            get 
            {
                return name;
            }
            set 
            {
                name = value;
            }
        }

        private string department = string.Empty;

        /// <summary>
        /// Contact's Department.
        /// </summary>
        public string Department
        {
            get
            {
                return department;
            }
            set
            {
                department = value;
            }
        }

        private string role = string.Empty;

        /// <summary>
        /// Contact's Role.
        /// </summary>
        public string Role
        {
            get
            {
                return role;
            }
            set
            {
                role = value;
            }
        }

        private string email = string.Empty;

        /// <summary>
        /// Contact's Email.
        /// </summary>
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
            }
        }

        private string telephone = string.Empty;

        /// <summary>
        /// Contact's Telephone.
        /// </summary>
        public string Telephone
        {
            get
            {
                return telephone;
            }
            set
            {
                telephone = value;
            }
        }

        private string fax = string.Empty;
        
        /// <summary>
        /// Contact's Fax.
        /// </summary>
        public string Fax
        {
            get
            {
                return fax;
            }
            set
            {
                fax = value;
            }
        }

        #endregion "Properties"

        #region "Constructors"

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public Contact():this(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Constructor with Contact details.
        /// </summary>
        /// <param name="name">Name of the Contact.</param>
        /// <param name="department">Department of the Contact.</param>
        /// <param name="role">Role of the Contact.</param>
        /// <param name="email">Email of the Contact.</param>
        /// <param name="telephone">Telephone of the Contact.</param>
        /// <param name="fax">Fax of the Contact.</param>
        public Contact(string name, string department, string role, string email, string telephone, string fax)
        {
            this.Name = name;
            this.Department = department;
            this.Role = role;
            this.Email = email;
            this.Telephone = telephone;
            this.Fax = fax;
        }

        #endregion "Constructors"
    }
}
