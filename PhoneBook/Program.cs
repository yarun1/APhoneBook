using System;
using System.IO;
using System.Collections.Generic;

namespace PhoneBook
{
    class Program
    {
        string contactsFile = "Contacts.csv";
        char contactDataSeparator = ',';

        static void Main(string[] args)
        {
            Program program = new Program();

            string phoneBookCommand;
            string contactName;
            string contactPhoneNumber;
            string contactData;

            const string CONTACT_GETTING_COMMAND = "g";
            const string CONTACT_LIST_GETTING_COMMAND = "gl";
            const string CONTACT_CREATION_COMMAND = "c";
            const string CONTACT_UPDATE_COMMAND = "u";
            const string CONTACT_REMOVING_COMMAND = "r";
            const string CONTACT_LIST_REMOVING_COMMAND = "rl";
            const string PROGRAM_EXIT_COMMAND = "e";

            Console.WriteLine("WELCOME TO YOUR PHONE BOOK");

            while (true)
            {
                Console.WriteLine($"\n\nPress '{CONTACT_GETTING_COMMAND}' to get a contact from the contact list");
                Console.WriteLine($"Press '{CONTACT_LIST_GETTING_COMMAND}' to get the contact list");
                Console.WriteLine($"Press '{CONTACT_CREATION_COMMAND}' to create a new contact");
                Console.WriteLine($"Press '{CONTACT_UPDATE_COMMAND}' to update a contact");
                Console.WriteLine($"Press '{CONTACT_REMOVING_COMMAND}' to remove a contact");
                Console.WriteLine($"Press '{CONTACT_LIST_REMOVING_COMMAND}' to remove the contact list");
                Console.WriteLine($"Press '{PROGRAM_EXIT_COMMAND}' to exit the program \n");
                Console.Write("Input a command: ");

                phoneBookCommand = Console.ReadLine();
                Console.WriteLine();

                switch (phoneBookCommand)
                {
                    case CONTACT_GETTING_COMMAND:
                    {
                        Console.Write("Input any contact data to find a contact: ");
                        contactData = Console.ReadLine();
                        Console.WriteLine();

                        program.GetContact(contactData);
                        break;
                    }
                    case CONTACT_LIST_GETTING_COMMAND:
                    {
                        program.GetContactList();
                        break;
                    }
                    case CONTACT_CREATION_COMMAND:
                    {
                        Console.Write("Input a contact name: ");
                        contactName = Console.ReadLine();

                        Console.Write("Input a contact phone number: ");
                        contactPhoneNumber = Console.ReadLine();
                        Console.WriteLine();

                        program.CreateContact(new Contact(contactName, contactPhoneNumber));
                        break;
                    }
                    case CONTACT_UPDATE_COMMAND:
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
                    case CONTACT_REMOVING_COMMAND:
                    {
                        Console.Write("Input any contact data to find a contact: ");
                        contactData = Console.ReadLine();
                        Console.WriteLine();

                        program.RemoveContact(contactData);
                        break;
                    }
                    case CONTACT_LIST_REMOVING_COMMAND:
                    {
                        program.RemoveContactList();
                        break;
                    }
                    case PROGRAM_EXIT_COMMAND:
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

        void WriteContactList(List<Contact> contactListToWrite)
        {
            using (StreamWriter contactStreamWriter = new StreamWriter(contactsFile))
            {
                string contactsToWrite = "";

                foreach (Contact contactToWrite in contactListToWrite)
                {
                    contactsToWrite += contactToWrite.name + contactDataSeparator + contactToWrite.phoneNumber + "\n\r";
                }

                contactStreamWriter.Write(contactsToWrite);
            }   
        }

        List<Contact> ReadContactList()
        {
            using (StreamReader contactStreamReader = new StreamReader(new FileStream(contactsFile, FileMode.OpenOrCreate)))
            {
                string currentContacts = contactStreamReader.ReadToEnd();
                string[] splitCurrentContacts = currentContacts.Split("\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                List<Contact> contactsToRead = new List<Contact>();
                
                foreach (string splitCurrentContact in splitCurrentContacts)
                {
                    string[] currentContact = splitCurrentContact.Split(contactDataSeparator);
                    contactsToRead.Add(new Contact(currentContact[0], currentContact[1]));
                }

                return contactsToRead;
            }
        }

        Contact SearchContact(string contactDataToSearch)
        {
            foreach (Contact currentContact in ReadContactList())
            {
                if (currentContact.name == contactDataToSearch || currentContact.phoneNumber == contactDataToSearch)
                {
                    return currentContact;
                }
            }

            Console.WriteLine("\nThe incorrect contact data");
            return null;
        }

        void GetContact(string contactData)
        {
            Contact searchedContact = SearchContact(contactData);

            if (DoesContactExist(searchedContact))
            {
                PrintContacts(new List<Contact>() { searchedContact });
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
            List<Contact> currentContactList = ReadContactList();

            if (DoContactDataExist(contactData) && !IsContactListEmpty(currentContactList))
            {
                for (int i = currentContactList.Count - 1; i >= 0; i--)
                {
                    if (SearchContact(contactData).Equals(currentContactList[i]))
                    {
                        currentContactList.Remove(currentContactList[i]);
                        Console.WriteLine("\nThis contact was successfully removed");
                    }
                }

                WriteContactList(currentContactList);
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

        bool DoesPhoneNumberContainOnlyDigits(string phoneNumberToCheck)
        {
            foreach (char phoneNumberDigitToCheck in phoneNumberToCheck)
            {
                if (phoneNumberDigitToCheck < '0' || phoneNumberDigitToCheck > '9')
                {
                    Console.WriteLine("\nThe contact phone number does not contain only digits");
                    return false;
                }
            }

            return true;
        }

        bool DoesContactExist(Contact contactToCheck)
        {
            if (contactToCheck == null)
            {
                Console.WriteLine("\nThis contact does not exist");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(contactToCheck.name))
            {
                Console.WriteLine("\nThere is no contact name");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(contactToCheck.phoneNumber))
            {
                Console.WriteLine("\nThere is no contact phone number");
                return false;
            }

            return true;
        }
       
        bool DoContactDataExist(string contactDataToCheck)
        {
            if (string.IsNullOrWhiteSpace(contactDataToCheck))
            {
                Console.WriteLine("\nThere is no contact data to find a contact");
                return false;
            }

            return true;
        }

        bool IsContactListEmpty(List<Contact> contactListToCheck)
        {
            if (contactListToCheck.Count == 0)
            {
                Console.WriteLine("\nThere is no contacts in the phone book");
                return true;
            }

            return false;
        }

        void PrintContacts(List<Contact> contactsToPrint)
        {
            if (!IsContactListEmpty(contactsToPrint))
            {
                Console.WriteLine("\nCONTACT LIST");

                foreach (Contact contactToPrint in contactsToPrint)
                {
                    Console.WriteLine("\n" + contactToPrint.name);
                    Console.WriteLine(contactToPrint.phoneNumber);
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