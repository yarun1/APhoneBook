using System;
using System.IO;
using System.Collections.Generic;

namespace PhoneBook
{
    class Program
    {
        string contactsFile = "Contacts.csv";
        const char contactDataSeparator = ',';

        static void Main()
        {
            Program program = new Program();

            string phoneBookCommand;
            string contactName;
            string contactPhoneNumber;
            string contactData;

            const string COMMAND_GET_CONTACT = "g";
            const string COMMAND_GET_CONTACT_LIST = "gl";
            const string COMMAND_CREATE_CONTACT = "c";
            const string COMMAND_UPDATE_CONTACT = "u";
            const string COMMAND_REMOVE_CONTACT = "r";
            const string COMMAND_REMOVE_CONTACT_LIST = "rl";
            const string COMMAND_EXIT_PROGRAM = "e";

            Console.WriteLine("WELCOME TO YOUR PHONE BOOK");

            while (true)
            {
                Console.WriteLine($"\n\nPress '{COMMAND_GET_CONTACT}' to get a contact from the contact list");
                Console.WriteLine($"Press '{COMMAND_GET_CONTACT_LIST}' to get the contact list");
                Console.WriteLine($"Press '{COMMAND_CREATE_CONTACT}' to create a new contact");
                Console.WriteLine($"Press '{COMMAND_UPDATE_CONTACT}' to update a contact");
                Console.WriteLine($"Press '{COMMAND_REMOVE_CONTACT}' to remove a contact");
                Console.WriteLine($"Press '{COMMAND_REMOVE_CONTACT_LIST}' to remove the contact list");
                Console.WriteLine($"Press '{COMMAND_EXIT_PROGRAM}' to exit the program \n");
                Console.Write("Input a command: ");

                phoneBookCommand = Console.ReadLine();
                Console.WriteLine();

                switch (phoneBookCommand)
                {
                    case COMMAND_GET_CONTACT:
                    {
                        Console.Write("Input any contact data to find a contact: ");
                        contactData = Console.ReadLine();
                        Console.WriteLine();

                        program.GetContact(contactData);
                        break;
                    }
                    case COMMAND_GET_CONTACT_LIST:
                    {
                        program.GetContactList();
                        break;
                    }
                    case COMMAND_CREATE_CONTACT:
                    {
                        Console.Write("Input a contact name: ");
                        contactName = Console.ReadLine();

                        Console.Write("Input a contact phone number: ");
                        contactPhoneNumber = Console.ReadLine();
                        Console.WriteLine();

                        program.CreateContact(new Contact(contactName, contactPhoneNumber));
                        break;
                    }
                    case COMMAND_UPDATE_CONTACT:
                    {
                        Console.Write("Input any contact data to find a contact: ");
                        contactData = Console.ReadLine();

                        Console.Write("Input a new contact name: ");
                        contactName = Console.ReadLine();

                        Console.Write("Input a new contact phone number: ");
                        contactPhoneNumber = Console.ReadLine();
                        Console.WriteLine();

                        program.UpdateContact(contactData, new Contact(contactName, contactPhoneNumber));
                        break;
                    }
                    case COMMAND_REMOVE_CONTACT:
                    {
                        Console.Write("Input any contact data to find a contact: ");
                        contactData = Console.ReadLine();
                        Console.WriteLine();

                        program.RemoveContact(contactData);
                        break;
                    }
                    case COMMAND_REMOVE_CONTACT_LIST:
                    {
                        program.RemoveContactList();
                        break;
                    }
                    case COMMAND_EXIT_PROGRAM:
                    {
                        return;
                    }
                    default:
                    {
                        Console.WriteLine("\nThe incorrect command");
                        break;
                    }
                }
            }
        }

        void WriteContactList(List<Contact> contactList)
        {
            using (StreamWriter contactStreamWriter = new StreamWriter(contactsFile))
            {
                string contacts = "";

                foreach (Contact contact in contactList)
                {
                    contacts += contact.name + contactDataSeparator + contact.phoneNumber + "\n\r";
                }

                contactStreamWriter.Write(contacts);
            }   
        }

        List<Contact> ReadContactList()
        {
            using (StreamReader contactStreamReader = new StreamReader(new FileStream(contactsFile, FileMode.OpenOrCreate)))
            {
                string currentContacts = contactStreamReader.ReadToEnd();
                string[] splitCurrentContacts = currentContacts.Split("\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                List<Contact> contactListToRead = new List<Contact>();
                
                foreach (string splitCurrentContact in splitCurrentContacts)
                {
                    string[] currentContact = splitCurrentContact.Split(contactDataSeparator);
                    contactListToRead.Add(new Contact(currentContact[0], currentContact[1]));
                }

                return contactListToRead;
            }
        }

        Contact SearchContact(string contactData)
        {
            foreach (Contact contact in ReadContactList())
            {
                if (contact.name == contactData || contact.phoneNumber == contactData)
                {
                    return contact;
                }
            }

            Console.WriteLine("\nThe incorrect contact data");
            return null;
        }

        void GetContact(string contactData)
        {
            Contact contact = SearchContact(contactData);

            if (DoesContactExist(contact))
            {
                PrintContacts(new List<Contact>() { contact });
            }
        }

        void GetContactList()
        {
            PrintContacts(ReadContactList());
        }

        void CreateContact(Contact contactToCreate)
        {
            if (DoesContactExist(contactToCreate) && DoesPhoneNumberContainOnlyDigits(contactToCreate.phoneNumber))
            {
                List<Contact> currentContactList = ReadContactList();

                currentContactList.Add(contactToCreate);
                WriteContactList(currentContactList);

                Console.WriteLine("\nThis contact was successfully created");
            }
        }

        void UpdateContact(string contactData, Contact newContact)
        {
            List<Contact> currentContactList = ReadContactList();

            if (DoContactDataExist(contactData) && DoesContactExist(newContact) &&
                DoesPhoneNumberContainOnlyDigits(newContact.phoneNumber) && !IsContactListEmpty(currentContactList))
            {
                Contact searchedContact = SearchContact(contactData);
                
                for (int i = currentContactList.Count - 1; i >= 0; i--)
                {
                    if (searchedContact.Equals(currentContactList[i]))
                    {
                        currentContactList.Remove(currentContactList[i]);
                        currentContactList.Add(newContact);

                        Console.WriteLine("\nThis contact was successfully updated");
                        Console.WriteLine($"{searchedContact.name} --> {newContact.name}");
                        Console.WriteLine($"{searchedContact.phoneNumber} --> {newContact.phoneNumber}");
                    }
                }

                WriteContactList(currentContactList);
            }
        }

        void RemoveContact(string contactData)
        {
            List<Contact> contactList = ReadContactList();

            if (DoContactDataExist(contactData) && !IsContactListEmpty(contactList))
            {
                for (int i = contactList.Count - 1; i >= 0; i--)
                {
                    if (SearchContact(contactData).Equals(contactList[i]))
                    {
                        contactList.Remove(contactList[i]);
                        Console.WriteLine("\nThis contact was successfully removed");
                    }
                }

                WriteContactList(contactList);
            }
        }

        void RemoveContactList()
        {
            if (!IsContactListEmpty(ReadContactList()))
            {
                WriteContactList(new List<Contact>());
                Console.WriteLine("\nYour contact list was successfully removed");
            }
        }

        bool DoesPhoneNumberContainOnlyDigits(string phoneNumber)
        {
            foreach (char phoneNumberDigit in phoneNumber)
            {
                if (phoneNumberDigit < '0' || phoneNumberDigit > '9')
                {
                    Console.WriteLine("\nThe contact phone number does not contain only digits");
                    return false;
                }
            }

            return true;
        }

        bool DoesContactExist(Contact contact)
        {
            if (contact == null)
            {
                Console.WriteLine("\nThis contact does not exist");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(contact.name))
            {
                Console.WriteLine("\nThere is no contact name");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(contact.phoneNumber))
            {
                Console.WriteLine("\nThere is no contact phone number");
                return false;
            }

            return true;
        }
       
        bool DoContactDataExist(string contactData)
        {
            if (string.IsNullOrWhiteSpace(contactData))
            {
                Console.WriteLine("\nThere is no contact data to find a contact");
                return false;
            }

            return true;
        }

        bool IsContactListEmpty(List<Contact> contactList)
        {
            if (contactList.Count == 0)
            {
                Console.WriteLine("\nThere is no contacts in the phone book");
                return true;
            }

            return false;
        }

        void PrintContacts(List<Contact> contactList)
        {
            if (!IsContactListEmpty(contactList))
            {
                Console.WriteLine("\nCONTACT LIST");

                foreach (Contact contact in contactList)
                { 
                    Console.WriteLine("\n" + contact.name);
                    Console.WriteLine(contact.phoneNumber);
                }
            }
        }
    }

    class Contact
    {
        public string name;
        public string phoneNumber;

        public Contact(string name, string phoneNumber)
        {
            this.name = name;
            this.phoneNumber = phoneNumber;
        }

        public override bool Equals(object obj)
        {
            Contact contact = obj as Contact;

            if (contact == null)
            {
                return false;
            }

            return name == contact.name && phoneNumber == contact.phoneNumber;
        }
    }
}